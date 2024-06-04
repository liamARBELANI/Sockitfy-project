using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FilesServer;

namespace MediaPlayerWPF
{
    public  static class Helper
    {
        public static string aeskey;
        public static IPAddress hostAddress = null;
        public static IPEndPoint hostEndPoint = null;
        public static Socket socket = null;
        public static int buffersize = 255;
        public static void create_socket()
        {
            try
            {
                if (socket == null || !socket.Connected)
                {

                    DotEnv.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
                    aeskey = Environment.GetEnvironmentVariable("AES_KEY");
                    hostAddress = IPAddress.Parse("127.0.0.1");
                    hostEndPoint = new IPEndPoint(hostAddress, 8001);
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(hostEndPoint);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("server is not running");

                socket.Close();
                Environment.Exit(0);
                //Console.WriteLine("Error..." + e.StackTrace);
            }
        }
        public static BitmapImage MyToImage(byte[] array)
        {
            //הופך סטרים של תמונה לביטמאפ
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(array))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        //public BitmapImage ToImageLocal()
        //{
        //    byte[] imgdata = System.IO.File.ReadAllBytes("7years.png");
        //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imgdata))
        //    {
        //        BitmapImage image = new BitmapImage();
        //        image.BeginInit();
        //        image.StreamSource = ms;
        //        image.EndInit();
        //        return image;
        //    }
        //}

        public static System.Drawing.Image MyByteArrayToImage(byte[] byteArrayIn)
        {
            // הופך מערך של בייטים לאובייקט תמונה
            System.Drawing.Image returnImage = null;
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Write(byteArrayIn, 0, byteArrayIn.Length);
                returnImage = System.Drawing.Image.FromStream(ms, true);
            }
            catch { }
            return returnImage;
        }

        public static ImageSource MyToImageSource(this System.Drawing.Image image)
        {
            // מעביר לאימג לאימג סורס
            var bitmap = new BitmapImage();

            using (var stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }

            return bitmap;
        }
    }
}

