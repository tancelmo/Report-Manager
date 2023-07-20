using System.Data;
using System.Diagnostics;
using System.Globalization;
using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using Report_Manager.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Report_Manager.Views.Field_Services.StatusReport.Schedule;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Edit : Page
{
    public static Edit? EditCurrrent;
    public int ID;

    public string connectionString = @"Server = " + Globals.server + "; Database=report_manager;Uid=newuser;Pwd=New@Mic15;";
    public Edit()
    {
        this.InitializeComponent();
        getData();
        PageOptions.EditControlsText(this);
        EditCurrrent = this;

        if (SchedulePage.SchedulePageCurrent != null)
        {
            foreach (var data in SchedulePage.SchedulePageCurrent.dataGrid.SelectedItems)
            {
                ScheduleData? myData = data as ScheduleData;

                if (myData != null)
                {
                    ID = myData.ID;
                    cbxStatus.SelectedItem = myData.Status;

                    datePicker.Date = myData.Date;


                    Installation.Text = myData.Installation;

                    Costumer.Text = myData.Costumer;
                    City.Text = myData.City;
                    meterSerialNumber.Text = myData.MeterSerialNumber;
                    meterSerialType.Text = myData.MeterType;
                    ClassificationTbx.SelectedItem = myData.Classification;
                    PTZ.Text = myData.PTZ;
                    PTZSN.Text = myData.PTZSerialNumber;
                    //Key.Text = myData.Key;
                    District.Text = myData.District;
                    Street.Text = myData.Street;
                    Meter.Text = myData.Meter;
                    meterSerialType.SelectedItem = myData.MeterType;
                    //Date.Text = Convert.ToDateTime(myData.Date).ToString("D");
                    //Price.Text = myData.Price.ToString();
                    GasSupplyStop.IsChecked = myData.Bypass;
                    tbxNotes.Text = myData.Notes;
                    Emergency.IsChecked = myData.Emergency;

                }

            }
        }

    }

    private void datePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        Debug.WriteLine((datePicker.Date, CultureInfo.InvariantCulture));
    }

    private void getData()
    {
        using (MySqlConnection Conn = new MySqlConnection(connectionString))
        {
            try
            {

                MySqlDataAdapter adapter = new MySqlDataAdapter("select * from metertypes", Conn);
                DataTable typedata = new DataTable();

                adapter.Fill(typedata);

                for (int i = 0; i < typedata.Rows.Count; i++)
                {
                    meterSerialType.Items.Add(typedata.Rows[i]["metertypes"]);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            try
            {

                MySqlDataAdapter adapter = new MySqlDataAdapter("select * from prices", Conn);
                DataTable typedata = new DataTable();

                adapter.Fill(typedata);

                for (int i = 0; i < typedata.Rows.Count; i++)
                {
                    ClassificationTbx.Items.Add(typedata.Rows[i]["service"]);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            try

            {
                cbxStatus.Items.Clear();
                MySqlDataAdapter adapter = new MySqlDataAdapter("select * from status where enabled=true", Conn);
                DataTable typedata = new DataTable();

                adapter.Fill(typedata);

                for (int i = 0; i < typedata.Rows.Count; i++)
                {
                    cbxStatus.Items.Add(typedata.Rows[i][CultureInfo.CurrentCulture.ThreeLetterISOLanguageName]);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Conn.Close();

        }
    }
}

