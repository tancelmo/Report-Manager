using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml;
using Report_Manager.Contracts.Services;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Report_Manager.Common.Login
{
    internal class LoginCommands
    {
        MySqlCommand MySqlCommand = new MySqlCommand();
        ConnectionMySQL ConnectionMySQL = new ConnectionMySQL();
        private MySqlDataReader? reader;

        public bool checkLogin(String login)
        {
            MySqlCommand.CommandText = "SELECT * from login where UserName='" + login + "'";
            try
            {
                MySqlCommand.Connection = ConnectionMySQL.Connect();
                reader = MySqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["UserName"].ToString() == login.ToLower())
                    {
                        User.Name = reader["Name"].ToString();
                    }

                    return true;
                }

            }
            catch (MySqlException ex)
            {
                LogFile.Write("#800025 Login", ex.Message);
            }
            ConnectionMySQL.Disconnect();
            return false;
        }

        public bool checkPass(String pass)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(pass);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            
            MySqlCommand.CommandText = "SELECT * from login where UserName='" + User.login + "'";
            try
            {
                MySqlCommand.Connection = ConnectionMySQL.Connect();
                reader = MySqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["Password"].ToString() == hash)
                    {
                        Globals.UserAccess = reader["Access"].ToString();
                        return true;
                    }

                }
                
            }
            catch (MySqlException ex)
            {
                LogFile.Write("#800025 Login", ex.Message);
            }
            ConnectionMySQL.Disconnect();
            return false;
        }

        public static void LoginUser(TextBox textBox, ProgressRing progressRing, Page page, TextBlock textBlock)
        {
            LoginCommands loginCommands = new LoginCommands();
            if (loginCommands.checkLogin(textBox.Text))
            {
                page.Frame.Navigate(typeof(Views.Login.Pass), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                User.login = textBox.Text.ToLower();

            }
            else
            {
                textBlock.Visibility = Visibility.Visible;
                progressRing.Visibility = Visibility.Collapsed;
            }
        }

        public async static void LoginUserPass(PasswordBox passwordBox, ProgressRing progressRing, Window window, TextBlock textBlock, RoutedEventArgs e)
        {
            LoginCommands loginCommands = new LoginCommands();

            if (loginCommands.checkPass(passwordBox.Password))
            {
                await App.GetService<IActivationService>().ActivateAsyncMain(e);
                window.Close();

            }
            else
            {
                textBlock.Visibility = Visibility.Visible;
                progressRing.Visibility = Visibility.Collapsed;
            }
        }
    }
}
