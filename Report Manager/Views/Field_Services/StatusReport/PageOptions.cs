using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using MySql.Data.MySqlClient;
using Report_Manager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;
using Syncfusion.UI.Xaml.DataGrid;
using Report_Manager.Helpers;
using Report_Manager.Views.Field_Services.StatusReport.Schedule;

namespace Report_Manager.Views.Field_Services.StatusReport
{
    internal class PageOptions
    {
        
        public static async void ScheduleRefreshData(SchedulePage page, Storyboard storyboard)
        {
            
            ConfigFile _configFile = new ConfigFile(Globals.ConfigFilePath);
            
            await Task.Delay(100);
            page.Orders.Clear();
            page.pageGrid.Visibility = Visibility.Collapsed;
            page.DataGridRing.Visibility = Visibility.Collapsed;
            page.Loadinglbl.Visibility = Visibility.Visible;

            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(page.connectionString);
                conn.Open();
                page.connOk = true;
                conn.Close();
            }
            catch (Exception ex)
            {

                page.DataGridRing.Visibility = Visibility.Collapsed;
                page.Loadinglbl.Text = "Failed to connect to data server.";
                page.Loadinglbl.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (page.connOk)
            {
                try
                {
                    var scheduleData = await page.GetDataAsync();
                    scheduleData.ForEach(o => page.Orders.Add(o));
                    storyboard.Begin();
                    page.pageGrid.Visibility = Visibility.Visible;
                    page.dataGrid.ScrollInView(new Syncfusion.UI.Xaml.Grids.ScrollAxis.RowColumnIndex(Convert.ToInt32(_configFile.Read("RowPsition", "ScheduleGrid")) + 20, 0));
                    page.DataGridRing.Visibility = Visibility.Collapsed;
                    page.Loadinglbl.Visibility = Visibility.Collapsed;
                    await Task.Delay(10);

                 

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error aqui");
                    LogFile.Write("#80099Schedule", ex.ToString());

                    throw;
                }
            }
        }

        public static void ScheduleHeaderText(SfDataGrid dataGrid)
        {
            dataGrid.Columns[0].HeaderText = "ScheduleColumn0".GetLocalized().ToUpper();
            dataGrid.Columns[1].HeaderText = "ScheduleColumn1".GetLocalized().ToUpper();
            dataGrid.Columns[2].HeaderText = "ScheduleColumn2".GetLocalized().ToUpper();
            dataGrid.Columns[3].HeaderText = "ScheduleColumn3".GetLocalized().ToUpper();
            dataGrid.Columns[4].HeaderText = "ScheduleColumn4".GetLocalized().ToUpper();
            dataGrid.Columns[5].HeaderText = "ScheduleColumn5".GetLocalized().ToUpper();
            dataGrid.Columns[6].HeaderText = "ScheduleColumn6".GetLocalized().ToUpper();
            dataGrid.Columns[7].HeaderText = "ScheduleColumn10".GetLocalized().ToUpper();
            dataGrid.Columns[8].HeaderText = "ScheduleColumn8".GetLocalized().ToUpper();
            dataGrid.Columns[9].HeaderText = "ScheduleColumn9".GetLocalized().ToUpper();
            dataGrid.Columns[10].HeaderText = "ScheduleColumn7".GetLocalized().ToUpper();
            dataGrid.Columns[11].HeaderText = "ScheduleColumn11".GetLocalized().ToUpper();
            dataGrid.Columns[12].HeaderText = "ScheduleColumn12".GetLocalized().ToUpper();
            dataGrid.Columns[13].HeaderText = "ScheduleColumn13".GetLocalized().ToUpper();
            dataGrid.Columns[14].HeaderText = "ScheduleColumn14".GetLocalized().ToUpper();
            dataGrid.Columns[15].HeaderText = "ScheduleColumn15".GetLocalized().ToUpper();
            dataGrid.Columns[16].HeaderText = "ScheduleColumn16".GetLocalized().ToUpper();
            dataGrid.Columns[17].HeaderText = "ScheduleColumn17".GetLocalized().ToUpper();
            dataGrid.Columns[18].HeaderText = "ScheduleColumn18".GetLocalized().ToUpper();
            dataGrid.Columns[19].HeaderText = "ScheduleColumn19".GetLocalized().ToUpper();

        }

        public static void RibbonControlsText(SchedulePage page)
        {
            page.lblInstallation.Text = "ScheduleColumn1".GetLocalized();
            page.lblCostumer.Text = "ScheduleColumn3".GetLocalized();
            page.lblAdress.Text = "ScheduleLocal".GetLocalized();
            page.lblClassification.Text = "ScheduleColumn7".GetLocalized();
            page.lblMeterSn.Text = "ScheduleColumn5".GetLocalized();
            page.lblMeter.Text = "ScheduleColumn4".GetLocalized();
            page.lblVolConUnit.Text = "ScheduleColumn8".GetLocalized();
            page.lblVolConUnitSN.Text = "ScheduleColumn9".GetLocalized();
            page.GasSupplyStop.Content = "ScheduleColumn17".GetLocalized();
            page.Emergency.Content = "ScheduleColumn18".GetLocalized();
        }

        public static void EditControlsText(Edit page)
        {
            page.lblInstallation.Text = "ScheduleColumn1".GetLocalized();
            page.lblCostumer.Text = "ScheduleColumn3".GetLocalized();
            page.lblCity.Text = "ScheduleColumn11".GetLocalized();
            page.lblDistrict.Text = "ScheduleColumn12".GetLocalized();
            page.lblStreet.Text = "ScheduleColumn13".GetLocalized();
            page.lblClassification.Text = "ScheduleColumn7".GetLocalized();
            page.lblMeterSn.Text = "ScheduleColumn5".GetLocalized();
            page.lblType.Text = "ScheduleColumn6".GetLocalized();
            page.lblMeter.Text = "ScheduleColumn4".GetLocalized();
            page.lblVolConUnit.Text = "ScheduleColumn8".GetLocalized();
            page.lblVolConUnitSN.Text = "ScheduleColumn9".GetLocalized();
            page.GasSupplyStop.Content = "ScheduleColumn17".GetLocalized();
            page.Emergency.Content = "ScheduleColumn18".GetLocalized();
            page.lblNotes.Text = "ScheduleColumn16".GetLocalized();
        }
    }
}
