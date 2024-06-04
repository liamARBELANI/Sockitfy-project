using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace MediaPlayerWPF
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        
        
        public SignInWindow()
        {
            InitializeComponent();
            Helper.create_socket();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //DragMove();
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            // עובר לעמוד סיין אפ
            SignUPWindow signUPWindow = new SignUPWindow();
            signUPWindow.Show();
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // עושה את הלוגין
            if (txtEmail.Text.Length == 0)
            {
                // לייבל של איימיל
                errormessage.Text = "Enter an email.";
                txtEmail.Focus();
            }
            else if (!Regex.IsMatch(txtEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                // בודק אם האיימיל וליד עם רגאקס
                errormessage.Text = "Enter a valid email.";
                txtEmail.Select(0, txtEmail.Text.Length);
                txtEmail.Focus();
            }
            else
            {
                // מתחבר לדיבי ובודק אם המשתמש קיים
                string email = txtEmail.Text;
                string password = txtPassword.Password;

                Helper.socket.Send(Encoding.ASCII.GetBytes(Security.EncryptStringToString("4$" +email+"$"+password,Helper.aeskey)));

                byte[] binDataIn = new byte[Helper.buffersize];
                int k = Helper.socket.Receive(binDataIn);
                string data =Security.DecryptStringToString(Encoding.ASCII.GetString(binDataIn, 0, k),Helper.aeskey);
                if (data == "Success")
                {
                    //MessageBox.Show(data);
                    errormessage.Text = "Success";
                    MainWindow window = new MainWindow();
                    window.Show();
                    this.Close();
                }
                else
                    errormessage.Text = "Sorry! Please enter existing emailid/password.";
            }
        }
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            //עבור לסייןאפ  בלחיצה 
            SignUPWindow reg = new SignUPWindow();
            reg.Show();
            Close();
        }
    }
    }

