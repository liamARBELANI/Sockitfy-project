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
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
namespace MediaPlayerWPF
{
    /// <summary>
    /// Interaction logic for SignUPWindow.xaml
    /// </summary>
    public partial class SignUPWindow : Window
    {
        public SignUPWindow()
        {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            // לעבור לסיין אין בלחצה
            SignInWindow login = new SignInWindow();
            login.Show();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //בלחיצה עושה ריסט לפילדים
            Reset();
        }
        public void Reset()
        {
            // עושה ריסט לפילדים

            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxEmail.Text = "";

            passwordBox.Password = "";
            passwordBoxConfirm.Password = "";
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            // סוגר חלון
            Close();
        }




        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            // מפעיל סיין אפ
            if (textBoxEmail.Text.Length == 0)
            {
                // בודק אם כתב מייל
                errormessage.Text = "Enter an email.";
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                //בודק אם המייל טקין עם רגאקס
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
                // בודק סיסמה והתאמה של סיסמה
                string firstname = textBoxFirstName.Text;
                string lastname = textBoxLastName.Text;
                string email = textBoxEmail.Text;
                string password = passwordBox.Password;
                if (passwordBox.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    passwordBox.Focus();
                }
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                    passwordBoxConfirm.Focus();
                }
                else if (passwordBox.Password != passwordBoxConfirm.Password)
                {
                    errormessage.Text = "Confirm password must be same as password.";
                    passwordBoxConfirm.Focus();
                }
                else
                {
                    errormessage.Text = "";

                    Helper.socket.Send(Encoding.ASCII.GetBytes(Security.EncryptStringToString("3$" + firstname + "$" + lastname+"$"+email+"$"+password,Helper.aeskey)));

                    byte[] binDataIn = new byte[Helper.buffersize];
                    int k = Helper.socket.Receive(binDataIn);
                    string data =Security.DecryptStringToString(Encoding.ASCII.GetString(binDataIn, 0, k),Helper.aeskey);
                    errormessage.Text = "You have Registered successfully.";
                }

             }

        }
        //בודק תקינות שם ושם במשתנה את הערך
        private void TextBoxFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 
            if (textBoxFirstName.Text =="")
                errormessage.Text = "please Enter your FirstName.";
            if (!Regex.Match(textBoxFirstName.Text, "^[A-Z][a-zA-Z]*$").Success)
            {
                // first name was incorrect  
                errormessage.Text="Invalid first name";
               textBoxFirstName.Focus();
                return;

            } // end if   
        }
        //בודק תקינות שם ושם במשתנה את הערך
        private void TextBoxLastName_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!Regex.Match(textBoxLastName.Text, "^[A-Z][a-zA-Z]*$").Success)
            {
                // last name was incorrect  
                errormessage.Text="Invalid last name";
               textBoxLastName.Focus();
                return;
            }// end if            
        }
        //בודק תקינות איימיל ושם במשתנה את הערך

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
               textBoxEmail.Focus();
           }
            else
            {
              string email = textBoxEmail.Text;
            }
       }
        //בודק תקינות סיסמה ושם במשתנה את הערך

        private void TextBoxPassword_TextChanged(Object sender,TextChangedEventArgs e)
        {
            
            if ( passwordBox.Password.Length == 0&&passwordBox.Password.Length<8)
            {
                errormessage.Text = "Enter password that has not less than  8 chars.";
                passwordBox.Focus();
            }
           else
                if(!Regex.IsMatch(passwordBox.Password, @"^.* (?=.{ 8,})(?=.*\d)(?=.*[a - z])(?=.*[A - Z])(?=.*[!*@#$%^&+=]).*$"))
            {
                errormessage.Text = "Enter a valid password.";
                
            }
            string pass = passwordBox.Password;
        }
    }
}    
