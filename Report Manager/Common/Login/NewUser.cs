using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using MySql.Data.MySqlClient;
using Report_Manager.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mail;

namespace Report_Manager.Common.Login
{
    class NewUser
    {
        
        public static void CheckNewUser(TextBox tbxNewUserEmail, TextBox tbxNewUserName, PasswordBox tbxNewUserPass, PasswordBox tbxNewUserPass2, InfoBar loginInfo, ProgressRing progressRing, FontIcon borderAccepted, StackPanel panelContent, Storyboard test)
        {
            
            // empty values

            if (tbxNewUserEmail.Text == string.Empty || tbxNewUserName.Text == string.Empty || tbxNewUserPass.Password == string.Empty || tbxNewUserPass2.Password == string.Empty)
            {
                loginInfo.Message = "BlankParam".GetLocalized();
                loginInfo.Severity = InfoBarSeverity.Error;
                loginInfo.IsOpen = true;
            }

            //valid mail
            else if (!IsValidEmail(tbxNewUserEmail.Text))
            {
                loginInfo.Message = "InvalidMail".GetLocalized();
                loginInfo.Severity = InfoBarSeverity.Error;
                loginInfo.IsOpen = true;
            }
            //password no equal
            else if (tbxNewUserPass.Password != tbxNewUserPass2.Password)
            {
                loginInfo.Message = "PassError".GetLocalized();
                loginInfo.Severity = InfoBarSeverity.Error;
                loginInfo.IsOpen = true;
            }
            //password length
            else if (tbxNewUserPass.Password.Length < 4 || tbxNewUserPass2.Password.Length < 4)
            {
                loginInfo.Message = "InvalidPass".GetLocalized();
                loginInfo.Severity = InfoBarSeverity.Error;
                loginInfo.IsOpen = true;
            }
            //name short
            else if (tbxNewUserName.Text.Length < 4)
            {
                loginInfo.Message = "InvalidName".GetLocalized();
                loginInfo.Severity = InfoBarSeverity.Error;
                loginInfo.IsOpen = true;

            }
            //ok
            else
            {
                CreateUser(tbxNewUserPass.Password, tbxNewUserEmail.Text, tbxNewUserName.Text, loginInfo, borderAccepted, panelContent, test);
                

            }
            progressRing.Visibility = Visibility.Collapsed;

        }
        // Check Valid Mail
        public static bool IsValidEmail(string email)
        {
            try
            {
                var email1 = new EmailAddressAttribute();
                if (email1.IsValid(email)){

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erro no mail: " + e);
                return false;
                
            }
        }

        //create user
        public static void CreateUser(string pass, string userMail, string userName, InfoBar loginInfo, FontIcon borderAccepted, StackPanel panelContent, Storyboard test)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(pass);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            int count = 0;


            string conn = @"Server = " + Globals.server + "; Database=report_manager;Uid=newuser;Pwd=New@Mic15;";
            MySqlConnection connection = new MySqlConnection(conn);
            MySqlDataReader reader;
            connection.Open();

            // Check if email already exists
            try
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = "select count(*) from login where UserName='" + userMail + "'";
                

                count = Convert.ToInt32(comm.ExecuteScalar());
                Debug.WriteLine(count);
            }
            catch (MySqlException ex)
            {
                LogFile.Write("#800025##CreateUser", ex.Message);
            }
            
            if (count > 0)
            {
                loginInfo.Severity = InfoBarSeverity.Error;
                loginInfo.Message = "User Email Already Exists";
                loginInfo.IsOpen = true;
                
            }
            else
            {
                Debug.WriteLine(count);
                try
                {
                    MySqlCommand comm = connection.CreateCommand();
                    comm.CommandText = "INSERT INTO `report_manager`.`login` (`UserName`, `Name`, `Password`, `Access`, `activated`) VALUES (@userMail, @userName, @hash, '0', '0');";
                    comm.Parameters.AddWithValue("@userMail", userMail);
                    comm.Parameters.AddWithValue("@userName", userName);
                    comm.Parameters.AddWithValue("@hash", hash);
                    Debug.WriteLine(comm.CommandText);
                    comm.ExecuteNonQuery();
                    loginInfo.Message = "User created, contact your system administration for activate your account";
                    loginInfo.Severity = InfoBarSeverity.Success;
                    loginInfo.IsOpen = true;
                    borderAccepted.Visibility = Visibility.Visible;
                    test.Begin();
                    panelContent.Visibility = Visibility.Collapsed;

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    
                    LogFile.Write("#NewUser01", e.Message);
                }
            }
            connection.Close();

        }

    }
}
