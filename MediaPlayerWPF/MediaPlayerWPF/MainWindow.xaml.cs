using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NAudio.Wave;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Data.SqlTypes;

namespace MediaPlayerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ASCIIEncoding asciiEnc = new ASCIIEncoding();
        // strings to byte arrays קוד להמיר ASCII  
       

        // תור לסכימת הסגמנטים של קבצי השמע
        private Queue<List<byte>> segmentsQueue = new Queue<List<byte>>();

        // דגל שקובע אם הניגון הפסיק
        private bool finish = false;

        // דגל שקובע אם הנגן כעת מנגן משהו
        private bool mediaPlayerIsPlaying = false;

        // הסוקט ליצירת הקשר עם הסרבר
        
        private double sound_volume = 0.5;
        // IP address and endpoint לסרבר
 

        // Reference to the secondary window
        SecondWindow window;

        // דגל שקובע אם השיר הופסק באמצע
        private bool IsPause = true;

        private ParameterizedThreadStart downloadThreadStart = null;
        private Thread downloadThread = null;
        private ThreadStart playThreadStart = null;
        private Thread playThread = null;
        private bool Isplaying = false;
        private int next = 0;
        bool stop = true;
       

        //משתנים מאותחלים למעלה על מנת שיהיו ציבוריים ואני אשתמש בהם לכל הפונקציות
        public MainWindow()
        {
            InitializeComponent();
            Helper.create_socket(); // פתיחת סוקט להלפר כדי שהיא תהיה סטאטית


        }
        private void OnSecondWindowButtonClicked(object sender, RoutedEventArgs e)
        {
            // איבנט שמעביר לחלון השני במידה ויש לחיצה על הכפתור
            if (window == null || window.IsClosed)
            {
                window = new SecondWindow(Helper.socket);
            }
            window.WindowState = WindowState.Maximized;
            window.Show();
        }
        
        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mePlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            //userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            //userIsDraggingSlider = false;
            //mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //    lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        public static string MillisecondsToMinutesSeconds(double milliseconds)
        {
            int minutes = (int)(milliseconds / 60000); //60ms * 60s * 60m
            int seconds = (int)((milliseconds % 60000) / 1000);
            return minutes + ":" + seconds;
        }
        private void DownloadAsync(Object obj)
        {
            segmentsQueue.Clear();
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () {
                browseBtn.IsEnabled = false;
            });
            // הפנייה לאובייקט
            Socket socket = obj as Socket;
            object sender = new object();
            RoutedEventArgs e = new RoutedEventArgs();
            byte[] binDataOut;

            string str = window.GetSongName(sender, e);
            binDataOut = asciiEnc.GetBytes(Security.EncryptStringToString("1$" + str,Helper.aeskey));
            // שולח את שם השיר לשרת
            socket.Send(binDataOut, 0, binDataOut.Length, SocketFlags.None);
            byte[] binDataIn = new byte[Helper.buffersize];
            // מקבל מהשרת את אורך השיר
            int k = socket.Receive(binDataIn);
            int songLength = Int32.Parse(Security.DecryptStringToString( asciiEnc.GetString(binDataIn, 0, k),Helper.aeskey)); //מקבלים מהשרת את אורך השיר וזה מעביר מסטרינג לאינט

            binDataIn = new byte[Helper.buffersize];
            List<byte> lsbytes = new List<byte>();
            Thread.Sleep(150); // עוצר את התוכנית ל 150 מיליסקנד

            k = socket.Receive(binDataIn, 0, Helper.buffersize, SocketFlags.None); // מקבל מהסרבר את גודל הקובץ אותו מורידים
            string info = Security.DecryptStringToString(asciiEnc.GetString(binDataIn, 0, k),Helper.aeskey);
            int segments = int.Parse(info.Split('#')[1]);// START # NUM OF SEGMENTS
            
            // עוברים על גודל הסגמנטים
            for (int i = 0; i < segments; i++)
            {
                int lastseglen = -1;
                if (!finish)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        sliProgress.Value = (((double)i / (double)segments) * 100);
                        lblProgressStatus.Text = MillisecondsToMinutesSeconds((double)i * 2000);
                    });
                }
                if (next > 1)
                {
                    next = 1;
                    finish = true;
                   
                }
                // מקבלים סגמנט
                k = socket.Receive(binDataIn, 0, Helper.buffersize, SocketFlags.None);

                while (k >= 255)
                {
                    // מקבלים את הסגמנט עצמו
                    lsbytes.AddRange(binDataIn);
                    k = socket.Receive(binDataIn, 0, Helper.buffersize, SocketFlags.None);
                    //binDataIn = Security.EencryptBytesToBytes(binDataIn, Helper.aeskey); // היה מצפין רק חלקים מהסגמנט ולא היה אפשר לפענח
                    //k = binDataIn.Length;
                }
                while (k != 3)
                {
                    // מקבלים את הסגמנט עצמו
                    lsbytes.AddRange(binDataIn);
                    lastseglen = 255-k;
                    k = socket.Receive(binDataIn, 0, Helper.buffersize, SocketFlags.None);
                }
                lsbytes.RemoveRange(lsbytes.Count - lastseglen, lastseglen);//למחוק מהסוף
                // משרשרים את הסגמט לתור סגמנטים
                segmentsQueue.Enqueue(new List<byte>(Security.DecryptBytesToBytes(lsbytes.ToArray(),Helper.aeskey))); // 352155
                lsbytes.Clear();

                string status = Security.EncryptStringToString("end of segment " + (i + 1) + " from " + segments,Helper.aeskey);
                byte[] binData = asciiEnc.GetBytes(status);
                // שולח שקיבל הכל 
                if (stop)
                {
                    next = 1;
                    finish = true;
                    playThread.Abort();
                    socket.Send(asciiEnc.GetBytes(Security.EncryptStringToString("stop",Helper.aeskey)));
                    break;
                }
                else
                    socket.Send(binData);

            }

            finish = false;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () {
                browseBtn.IsEnabled = true;
            });
        }

        private void Play()
        {
         
            while (!finish)
            {
                // Iterate through the segments
                while (segmentsQueue.Count > 0)
                {
                    // If paused, do nothing
                    if (IsPause)
                    {
                        List<byte> lsbytes = segmentsQueue.Dequeue();
                        MemoryStream ms = new MemoryStream(lsbytes.ToArray());
                        WaveOutEvent waveOut = new WaveOutEvent();
                        waveOut.Volume = (float)sound_volume; // Set volume (0.0 to 1.0)
                        WaveFileReader waveStream = new WaveFileReader(ms);// starting
                        if (!finish) waveOut.PlaybackStopped += (s, e) => { waveOut.Stop(); };
                        waveOut.Init(waveStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing && !finish)
                        {
                            Thread.Sleep(1);
                        }
                    }   
                }
            }
            finish = false;
            
        }
        //הופך את המשתנה ל פולס כדי לעצור את השמע
        private void DoPause(object sender, RoutedEventArgs e)
        {
            IsPause = !IsPause;
        }
    
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stop = !stop;

                if (!stop)
                {
                    startstopImage.Source = new BitmapImage(new Uri("./assets/stop.png", UriKind.RelativeOrAbsolute));
                    
                    
                    if (window.GetSongName(sender, e) == "empty")
                    {
                        return;
                    }
                    if (Isplaying)
                    {
                        downloadThread.Abort();
                        playThread.Abort();
                    }
                    Isplaying = true;
                    IsPause = true;
                    downloadThreadStart = new ParameterizedThreadStart(DownloadAsync);
                   
                    downloadThread = new Thread(downloadThreadStart);
                    downloadThread.Start(Helper.socket);
                    playThreadStart = new ThreadStart(Play);
                    playThread = new Thread(playThreadStart);
                    playThread.Start();
                    
                }
                else
                    startstopImage.Source = new BitmapImage(new Uri("./assets/start.png", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {

            }
        }

        private void pbStatus_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void sliVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sound_volume = sliVolume.Value/100;
        }


    
    }
}