using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Report_Manager.Views.Field_Services.StatusReport.Schedule;
using Report_Manager.Helpers;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using Report_Manager.Common;
using Report_Manager.Views.Field_Services.StatusReport;
using System.Data;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using System.Data.Odbc;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Globalization;

namespace Report_Manager.Views.Field_Services
{
    internal class FieldServicesDialogs
    {
        

        public static async void ScheduleEditOptions(Page page)
        {

            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            
            dialog.XamlRoot = page.XamlRoot;
            
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "EditServiceTitle".GetLocalized();
            dialog.PrimaryButtonText = "ApplyBtn".GetLocalized();
            dialog.CloseButtonText = "CloseBtn".GetLocalized();
            dialog.DefaultButton = ContentDialogButton.Primary;

            dialog.Content = new Edit();

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                UpdateRecords();
            }
            else
            {
                Debug.WriteLine("ESC Cancel or back button");
            }

        }

        public static async void ExecuteActionOptions(Page page)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app

            dialog.XamlRoot = page.XamlRoot;
            
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "EditServiceTitle".GetLocalized();
            dialog.PrimaryButtonText = "ApplyBtn".GetLocalized();
            dialog.CloseButtonText = "CloseBtn".GetLocalized();
            dialog.DefaultButton = ContentDialogButton.Primary;

            dialog.Content = new ExecuteAction();
            //dialog.MaxWidth = ShellPage.CurrentMain.ActualWidth * .8;
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
               // UpdateRecords();
            }
            else
            {
                Debug.WriteLine("ESC Cancel or back button in ExecuteAction");
            }
        }
        private static void UpdateRecords()
        {
            
            string langCurrent = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
            
            string connectionString = @"Server = " + Globals.server + "; Database=report_manager;Uid=newuser;Pwd=New@Mic15;";

            string Status = "";

            using (MySqlConnection Conn = new MySqlConnection(connectionString))
            {
                
                string query = "UPDATE schedule SET Status=@status, installation=@Instalation, costumer=@costumer, City=@city, District=@district, street=@street, Classification=@classification, Meter=@meter, SN=@meterSN, Type=@meterType, VolumeCon=@volCon, VolumeConSN=@volConSN, Date=@date, Notes=@notes, Emergency=@emergency, Supply=@supply WHERE ID=@ID";
                string queryStatus = "SELECT * from status where " + langCurrent + "='" + Edit.EditCurrrent.cbxStatus.SelectedItem.ToString() + "'";
                
                try
                {
                    Conn.Open();

                    MySqlCommand cmd = new MySqlCommand(queryStatus, Conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Status = reader["status_name"].ToString();
                    }

                    Debug.WriteLine("Status: " + Status.ToString());
                    Conn.Close();
                }
                
                catch(Exception ex)
                {
                    Debug.WriteLine("ERRO Status: " + Status.ToString() + ex);
                }

                try
                {
                    Conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, Conn);

                    cmd.Parameters.AddWithValue("@Instalation", Edit.EditCurrrent.Installation.Text);
                    cmd.Parameters.AddWithValue("@status", Status);
                    cmd.Parameters.AddWithValue("@costumer", Edit.EditCurrrent.Costumer.Text);
                    cmd.Parameters.AddWithValue("@city", Edit.EditCurrrent.City.Text);
                    cmd.Parameters.AddWithValue("@district", Edit.EditCurrrent.District.Text);
                    cmd.Parameters.AddWithValue("@street", Edit.EditCurrrent.Street.Text);
                    cmd.Parameters.AddWithValue("@classification", Edit.EditCurrrent.ClassificationTbx.SelectedItem);
                    cmd.Parameters.AddWithValue("@meter", Edit.EditCurrrent.Meter.Text);
                    cmd.Parameters.AddWithValue("@meterSN", Edit.EditCurrrent.meterSerialNumber.Text);
                    cmd.Parameters.AddWithValue("@meterType", Edit.EditCurrrent.meterSerialType.SelectedItem);
                    cmd.Parameters.AddWithValue("@volCon", Edit.EditCurrrent.PTZ.Text);
                    cmd.Parameters.AddWithValue("@volConSN", Edit.EditCurrrent.PTZSN.Text);
                    cmd.Parameters.AddWithValue("@date", Edit.EditCurrrent.datePicker.Date);
                    cmd.Parameters.AddWithValue("@notes", Edit.EditCurrrent.tbxNotes.Text);
                    cmd.Parameters.AddWithValue("@emergency", Edit.EditCurrrent.Emergency.IsChecked);
                    cmd.Parameters.AddWithValue("@supply", Edit.EditCurrrent.GasSupplyStop.IsChecked);

                    cmd.Parameters.AddWithValue("@ID", Edit.EditCurrrent.ID);

                    cmd.ExecuteNonQuery();
                    Conn.Close();

                    PageOptions.ScheduleRefreshData(SchedulePage.SchedulePageCurrent, SchedulePage.SchedulePageCurrent.storyBoardGrid);
                    SchedulePage.SchedulePageCurrent.counterLabel.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    LogFile.Write(ex.ToString(), "#01001818");
                }
                Debug.WriteLine("Statis here: " + Status + " #######" + " " + queryStatus);
            }
            
        }
        public static async void ScheduleNew(Page page)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app

            dialog.XamlRoot = page.XamlRoot;

            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "EditServiceTitle".GetLocalized();
            dialog.PrimaryButtonText = "ApplyBtn".GetLocalized();
            dialog.CloseButtonText = "CloseBtn".GetLocalized();
            dialog.DefaultButton = ContentDialogButton.Primary;

            dialog.Content = new NewScheduleItem();
            //dialog.MaxWidth = ShellPage.CurrentMain.ActualWidth * .8;
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                MessageDialog.Show(page, "TODO - ADD NEW PAGE FOR SCHEDULE NEW FACILITIES");
            }
            else
            {
                Debug.WriteLine("ESC Cancel or back button in ExecuteAction");
            }
        }
    }
}
