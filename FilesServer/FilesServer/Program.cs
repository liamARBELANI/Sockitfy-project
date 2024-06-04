using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using NAudio.Wave;
using System.Runtime.Serialization;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data.OleDb;
using System.Data;
using System.Security.Cryptography;

namespace FilesServer
{
    class Program
    {
        // סוקט לתקשורת עם הלקוח
        static Socket socket = null;
        // מאזין TCP לקבלת חיבורים נכנסים
        static TcpListener myListener = null;
        // קידוד ASCII להמרת מחרוזות לבייטים ולהיפך
        static ASCIIEncoding asciiEnc = new ASCIIEncoding();
        public static string aeskey = "";
        static void Main(string[] args)
        {
            
            DotEnv.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

            aeskey = Environment.GetEnvironmentVariable("AES_KEY");
            // הגדרת כתובת ה-IP והפורט של השרת
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            myListener = new TcpListener(ipAddr, 8001);
            myListener.Start(); // הפעלת המאזין
            //com = new Comunicator();

            // לולאה אינסופית כדי לשמור על השרת פועל
            while (true)
            {
             
                    Console.WriteLine("the server is running at port:" + myListener.LocalEndpoint);
                    Console.WriteLine("waiting for a connection");
                    socket = myListener.AcceptSocket(); // קבלת חיבור חדש
                    Console.WriteLine("connection accepted");
                    byte[] binDataIn = new byte[1024];
                    Console.WriteLine("the message from client side");
                    try
                    {
                        // עיבוד הודעות מהלקוח בלולאה
                        while (true)
                        {
                            // login$username$password
                            int k = socket.Receive(binDataIn); // קבלת נתונים מהלקוח
                            if (k <= 0)
                                break;
                            string[] msgDetails = Security.DecryptStringToString(asciiEnc.GetString(binDataIn, 0, k), aeskey).Split('$');
                            // $
                            // טיפול בהודעות לפי סוגן
                            switch (msgDetails[0])
                            {
                                case "1":
                                    HandleWavStream(msgDetails[1]); // קריאה לפונקציה שמטפלת בקובץ WAV
                                    break;
                                case "2":
                                    HandleGallery(msgDetails[1]); // קריאה לפונקציה שמטפלת בגלריה
                                    break;
                                case "3":
                                    HandleSignUp(msgDetails); //קריאה לפונקצייה שמטפלת בהרשמה למערכת
                                    break;
                                case "4":
                                    HandleSignIn(msgDetails); //קריאה לפונקצייה שמטפלת בכניסה למערכת
                                break;


                            }
                        }
                    }
                    catch(System.Net.Sockets.SocketException){}
                

            }
        }

        private static void HandleSignIn(string[] msgDetails)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\project_gal\FilesServer\FilesServer\FileShareDB.accdb";
            string query = "SELECT * FROM users WHERE Email = @Email AND Password = @Password";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@Email", msgDetails[1]);
                    command.Parameters.AddWithValue("@Password", ComputeSha256Hash(msgDetails[2]));
                    connection.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        if (dataSet.Tables[0].Rows.Count > 0)
                            socket.Send(asciiEnc.GetBytes(Security.EncryptStringToString("Success",aeskey)));
                        else
                            socket.Send(asciiEnc.GetBytes(Security.EncryptStringToString("NoSuccess",aeskey)));
                    }
                }
            }
        }

        private static void HandleSignUp(string[] msgDetails)
        {
            OleDbConnection con_string = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\project_gal\FilesServer\FilesServer\FileShareDB.accdb");
            con_string.Open();

            string query = "INSERT INTO users ([Fname], [lname], [Password], [Email]) VALUES (?, ?, ?, ?)";
            OleDbCommand cmd = new OleDbCommand(query, con_string);

            // Add parameters with the appropriate values
            cmd.Parameters.AddWithValue("@Fname", msgDetails[1]);
            cmd.Parameters.AddWithValue("@lname", msgDetails[2]);
            cmd.Parameters.AddWithValue("@Password", ComputeSha256Hash(msgDetails[4]));
            cmd.Parameters.AddWithValue("@Email", msgDetails[3]);

            cmd.ExecuteNonQuery();
            con_string.Close();

            socket.Send(asciiEnc.GetBytes(Security.EncryptStringToString("Success", aeskey)));
        }
        // טיפול בבקשות שקשורות לגלריה
        private static void HandleGallery(string cmd)
        {
            string[] fileEntries = null;
            int i = 0;
            while (cmd != "done")
            {
                byte[] binDataIn = new byte[255];
                
               
               switch (cmd)
                {
                    case "names":
                        fileEntries = Directory.GetFiles("img"); // קבלת רשימת קבצים מהתיקייה 
                        string songsname = String.Join("$", fileEntries.Select(x => x.Remove(x.IndexOf("img\\"), "img\\".Length)).ToArray());
                        Console.WriteLine();

                        byte[] bufferOut = asciiEnc.GetBytes(Security.EncryptStringToString(songsname,aeskey)); // המרת שמות הקבצים לבייטים
                        socket.Send(bufferOut, 0, bufferOut.Length, SocketFlags.None); // שליחת הנתונים ללקוח
                        break;
                    case "getnext":
                        Thread.Sleep(100);
                        if (i == fileEntries.Length)
                            bufferOut = new byte[1];
                        else
                            bufferOut = Security.EncryptBytesToBytes(ImageToBytes(fileEntries[i++]),aeskey);

                        socket.Send(bufferOut, 0, bufferOut.Length, SocketFlags.None);
                        break;

                }
                int k = socket.Receive(binDataIn);
                cmd = Security.DecryptStringToString(asciiEnc.GetString(binDataIn, 0, k) ,aeskey);
            }
        }

        private static byte[] ImageToBytes(string imgname)
        {
            byte[] imgdata = System.IO.File.ReadAllBytes(imgname); // קריאת תוכן הקובץ למערך בייטים
            return imgdata;
        }

        // טיפול בבקשות שקשורות להזרמת קבצי WAV
        private static void HandleWavStream(string msg) 
        {
            byte[] binDataIn = new byte[1024];
            string status = "";
            System.Threading.Thread.Sleep(500); // השהיה לצורך סנכרון
            string filePath = @msg; // נתיב הקובץ מההודעה
            string outPath = @"tempmydir\"; // ספריית הפלט
            DirectoryInfo di = Directory.CreateDirectory(outPath); // יצירת הספרייה
            Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(outPath));
            int segements = SpliToSegemnts(filePath, outPath);
            byte[] binOut = asciiEnc.GetBytes(Security.EncryptStringToString(segements.ToString(),aeskey));
            socket.Send(binOut);
            binOut = asciiEnc.GetBytes(Security.EncryptStringToString(string.Format("strat#{0}", segements),aeskey));
            socket.Send(binOut);
            System.Threading.Thread.Sleep(200);

            // שליחת כל מקטע ללקוח
            for (int i = 1; i <= segements; i++)
            {
                string segemntPath = string.Format("{0}{1}.wav", outPath, i);
                using (FileStream fs = new FileStream(segemntPath, FileMode.Open, FileAccess.Read)) //פתיחת קובץ עם פייל סטרים
                {
                    int length = Convert.ToInt32(fs.Length);
                    binOut = new byte[length];
                    fs.Read(binOut, 0, length); // קריאת תוכן הקובץ למערך בייטים
                    fs.Close();
                    byte[] res = Security.EncryptBytesToBytes(binOut, aeskey); // 352064
                    socket.Send(res); // שליחת המקטע ללקוח
                    binOut = asciiEnc.GetBytes("end");
                    System.Threading.Thread.Sleep(1500);
                    socket.Send(binOut); // שליחת הודעה על סיום המקטע
                    int x = socket.Receive(binDataIn); // קבלת אישור מהלקוח
                    status = Security.DecryptStringToString(asciiEnc.GetString(binDataIn, 0, x),aeskey);
                    if (status == "stop")
                        break;
                    Console.WriteLine(status);
                }
            }
            // מחיקת קבצים זמניים
            string[] files = Directory.GetFiles(@"tempmydir");
            foreach (string file in files)
                File.Delete(file);
        }


        public static string MillisecondsToMinutesSeconds(double milliseconds)
        {
            int minutes = (int)(milliseconds / 60000);
            int seconds = (int)((milliseconds % 60000) / 1000);
            return minutes + ":" + seconds;
        }

        // חלוקת קובץ WAV למקטעים

        public static int SpliToSegemnts(string filePath, string outPath)
        {

            using (var wfr = new WaveFileReader(filePath))
            {
               
                TimeSpan totalTime = wfr.TotalTime;// משך זמן הקובץ

                double num = totalTime.TotalMilliseconds;

                Console.WriteLine("Time: "+MillisecondsToMinutesSeconds(num));
                int count = 2; int segment = 1;
                double start = 0, end = count * 1000;

                // חלוקת הקובץ לפי משך הזמן
                while (end < num)
                {
                    WavFileUtils.TrimWavFile(filePath, outPath, TimeSpan.FromMilliseconds(start), TimeSpan.FromMilliseconds(end), segment);
                    start = end;
                    count += 2; //לשקול להוסיף חתיכה דינאמית של הסגמנטים לפי רוחב פס
                    end = count * 1000;
                    segment++;
                }       
                WavFileUtils.TrimWavFile(filePath, outPath, TimeSpan.FromMilliseconds(start), TimeSpan.FromMilliseconds(num), segment);
                return segment;
            }
        }
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();//מחזיר בחזרה את הטקסט המוצפן
            }
        }
        
    }
}
