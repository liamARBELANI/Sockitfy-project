using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaPlayerWPF
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        private Socket socket = null;
        private ASCIIEncoding asciiEnc = new ASCIIEncoding();
        string songname = "empty";
        public SecondWindow()
        {
            InitializeComponent();
        }

        public SecondWindow(Socket socket)
        {
            InitializeComponent();
            this.socket = socket;
            DownloadGallery();
        }
        
        private void DownloadGallery()
        {
            //שולח לסרבר לקבל שמות של שירים
            byte[] binDataOut;
            binDataOut =asciiEnc.GetBytes(Security.EncryptStringToString("2$names", Helper.aeskey));
            socket.Send(binDataOut, 0, binDataOut.Length, SocketFlags.None);
            byte[] binDataIn = new byte[Helper.buffersize];

            //מקבל את שמות השירים
            int k = socket.Receive(binDataIn);
            string [] songNames = Security.DecryptStringToString(asciiEnc.GetString(binDataIn, 0, k),Helper.aeskey).Split('$');
            songNames = songNames.Select(x => x.Replace("jpg", "wav")).ToArray();
            Console.WriteLine(songNames);

            List <ImageSource > lsImgs= DownloadAllImgs();
            CreateGridGrallery(songNames, lsImgs);
        }

        private void CreateGridGrallery(string[] songNames, List<ImageSource> lsImgs)
        {
            //יוצר גריד של שירים 
            Grid grid = this.mygrid;
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            int image_rows = 2;
            int row = 0;
            for (int i = 0; i < songNames.Length; i+= image_rows)
            {
                //מוסיף שורה לשורות הגרידים
                grid.RowDefinitions.Add(new RowDefinition());
                int col = 0;
                for (int j = i; j < i+image_rows && j < songNames.Length; j++)
                {
                    //מוסיף טור לטורי הגרידים
                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                    //יוצר תמונה וסטאק פאנל לשים תמונה
                    StackPanel sp = new StackPanel();
                    //הגדרות של תמונה וכפתור
                    Image im = new Image();
                    im.Width = 100;
                    im.Height = 100;
                    im.Source = lsImgs[j];
                    Button b = new Button();
                    b.Content = im;
                    b.ToolTip = songNames[j];
                    b.Width = 150;
                    b.Height = 150;
                    b.Click += B_Click;
                    sp.Children.Add(b);
                    //מוסיף לייבל עם השם של השיר
                    Label l = new Label();
                    l.Content = songNames[j];
                    
                    sp.Children.Add(l);
                    sp.Margin = new Thickness(10);
                    Grid.SetRow(sp, row);
                    Grid.SetColumn(sp, col++);
                    //מוסיף לגריד ולשורה וטור הנכונים
                    mygrid.Children.Add(sp);

                }
                row++;
                
                

            }
            
            
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            //לחיצה על כפתור משנה את שם השיר הנוכחי וסוגרת את החלון
            songname = (sender as Button).ToolTip.ToString();
            Close();
        }

        //מוריד את כל התמונות של השירים
        private List<ImageSource> DownloadAllImgs()
        {
            List<ImageSource> imgs = new List<ImageSource>();
            ImageSource src = null;
            byte[] binDataOut;
            do
            {
                byte[] binDataIn = new byte[Helper.buffersize];
                binDataOut = asciiEnc.GetBytes(Security.EncryptStringToString("getnext", Helper.aeskey));
                socket.Send(binDataOut, 0, binDataOut.Length, SocketFlags.None);
                src = DownloadsingleImg();
                if (src != null)
                    imgs.Add(src);
                
            } while (src != null);

            binDataOut = asciiEnc.GetBytes(Security.EncryptStringToString("done", Helper.aeskey));
            //שולח לסרבר שקיבל את כל התמונות  
            socket.Send(binDataOut, 0, binDataOut.Length, SocketFlags.None);
            return imgs;

        }

        private ImageSource DownloadsingleImg()
        {
            //try
            {
                List<byte> lsbytes = new List<byte>();
                Thread.Sleep(100);
                byte[] bufferIn = new byte[Helper.buffersize];
                //מקבל מהסרבר את השירים
                int k = socket.Receive(bufferIn, 0, Helper.buffersize, SocketFlags.None);
                //DONE!!
                if (k == 1)
                    return null;
                while (k >= Helper.buffersize)
                {
                    lsbytes.AddRange(bufferIn);
                    //מקבל מהסרבר את המשך השירים
                    k = socket.Receive(bufferIn, 0, Helper.buffersize, SocketFlags.None);
                }

                byte[] temp = new byte[k];
                Array.Copy(bufferIn, temp, k);
                lsbytes.AddRange(temp);
                //myimage.Source = ToImage(lsbytes.ToArray());
                byte[] imgdata = Security.DecryptBytesToBytes(lsbytes.ToArray(),Helper.aeskey);
                System.Drawing.Image tempImg = Helper.MyByteArrayToImage(imgdata);
                return Helper.MyToImageSource(tempImg);
            }
            //catch (Exception)
            {

                // return null;
            }
           
        }

        //public void VivalaVida(object sender, RoutedEventArgs e)
        //{
        //    songname = "Coldplay - Viva La Vida.wav";
        //    Close();
        //}
        //public void Stars(object sender, RoutedEventArgs e)
        //{
        //    songname = "Coldplay - a sky full of stars.wav";
        //    Close();
        //}
        //public void Bohemian(object sender, RoutedEventArgs e)
        //{
        //    songname = "Queen - Bohemian Rhapsody.wav";
        //    Close();
        //}
        //public void SevenYears(object sender, RoutedEventArgs e)
        //{
        //    songname = "Lukas Graham - 7 Years.wav";
        //    Close();
        //}
        //public void Melody(object sender, RoutedEventArgs e)
        //{
        //    songname = "original.wav";
        //    Close();
        //}
        //public void Radioactive(object sender, RoutedEventArgs ee)
        //{
        //    songname = "Imagine Dragons - Radioactive.wav";
        //    Close();
        //}

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.IsClosed = true;
        }

        public string GetSongName(object sender, RoutedEventArgs ee)
        {
            return songname;
        }
        public bool IsClosed { get; private set; }
    }
}