using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using Report_Manager.Common;
using Report_Manager.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Report_Manager.Views.Field_Services.StatusReport.Executed;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Executed : Page, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChaged;
    public event PropertyChangedEventHandler? PropertyChanged;

    private ObservableCollection<ScheduleData> _scheduleData;
    public ObservableCollection<ScheduleData> Orders
    {
        get
        {
            return _scheduleData;

        }
        set
        {
            _scheduleData = value;
            OnPropertyChanged(nameof(Orders));
        }
    }

    public string connectionString = @"Server = " + Globals.server + "; Database=report_manager;Uid=newuser;Pwd=New@Mic15;";

    private void OnPropertyChanged(string propertyName) => PropertyChaged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public bool connOk;
    public Executed()
    {
        this.InitializeComponent();
        Orders = new ObservableCollection<ScheduleData>();
    }
    public async Task<List<ScheduleData>> GetDataAsync()
    {
        try
        {
            var query = "select * from schedule where executed=true";

            var scheduleData = new List<ScheduleData>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var order = new ScheduleData();

                            if (!await reader.IsDBNullAsync(0))
                                order.ID = reader.GetInt32(0);
                            if (!await reader.IsDBNullAsync(1))
                                order.Status = GetLabel(reader.GetString(1));
                            if (!await reader.IsDBNullAsync(20))
                                order.Invoice = reader.GetString(20);
                            if (!await reader.IsDBNullAsync(3))
                                order.Team = reader.GetString(3);
                            if (!await reader.IsDBNullAsync(4))
                                order.Costumer = reader.GetString(4);
                            if (!await reader.IsDBNullAsync(7))
                                order.Meter = reader.GetString(7);
                            if (!await reader.IsDBNullAsync(6))
                                order.MeterSerialNumber = reader.GetString(5);
                            if (!await reader.IsDBNullAsync(6))
                                order.MeterType = reader.GetString(6);
                            if (!await reader.IsDBNullAsync(8))
                                order.Classification = reader.GetString(8);
                            if (!await reader.IsDBNullAsync(9))
                                order.PTZ = reader.GetString(9);
                            if (!await reader.IsDBNullAsync(10))
                                order.PTZSerialNumber = reader.GetString(10);
                            if (!await reader.IsDBNullAsync(11))
                                order.City = reader.GetString(11);
                            if (!await reader.IsDBNullAsync(12))
                                order.District = reader.GetString(12);
                            if (!await reader.IsDBNullAsync(13))
                                order.Key = reader.GetString(13);
                            if (!await reader.IsDBNullAsync(15))
                                order.Notes = reader.GetString(15);
                            if (!await reader.IsDBNullAsync(16))
                                order.Date = reader.GetDateTime(16);
                            if (!await reader.IsDBNullAsync(17))
                                order.Price = reader.GetDouble(17);
                            if (!await reader.IsDBNullAsync(14))
                                order.Bypass = reader.GetBoolean(14);
                            if (!await reader.IsDBNullAsync(18))
                                order.Emergency = reader.GetBoolean(18);
                            if (!await reader.IsDBNullAsync(19))
                                order.ExpirationDate = reader.GetDateTime(19).ToString("d");
                            if (!await reader.IsDBNullAsync(22))
                                order.Street = reader.GetString(22);
                            scheduleData.Add(order);



                        }

                    }
                }
                connection.Close();

            }
            return scheduleData;



        }
        catch (Exception ex)
        {
            //Loadinglbl.Text = "Failed to loading data.";
            //Loadinglbl.Foreground = new SolidColorBrush(Colors.Red);
            throw;
        }


    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {

        await Task.Delay(100);


        MySqlConnection conn = null;
        try
        {
            conn = new MySqlConnection(connectionString);
            conn.Open();
            connOk = true;
            conn.Close();
        }
        catch (Exception ex)
        {

            //DataGridRing.Visibility = Visibility.Collapsed;
            //Loadinglbl.Text = "Failed to connect to data server.";
            // Loadinglbl.Foreground = new SolidColorBrush(Colors.Red);
        }
        if (connOk)
        {
            try
            {
                var scheduleData = await GetDataAsync();
                scheduleData.ForEach(o => Orders.Add(o));
                //storyBoardGrid.Begin();
                //pageGrid.Visibility = Visibility.Visible;
                //DataGridRing.Visibility = Visibility.Collapsed;
                // Loadinglbl.Visibility = Visibility.Collapsed;
                await Task.Delay(100);

                //scroll position datagrid SchedulePage

                // dataGrid.ScrollInView(new Syncfusion.UI.Xaml.Grids.ScrollAxis.RowColumnIndex(Convert.ToInt32(_configFile.Read("RowPsition", "ScheduleGrid")) + 20, 0));
                //set Headers Name Datagrid
                PageOptions.ScheduleHeaderText(dataGrid);
                //--------------------------------------

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error aqui");
                LogFile.Write("#80099Schedule", ex.ToString());

                throw;
            }
        }
        // var scrollbar = MyFindDataGridChildOfType<ScrollBar>(dataGrid);

        // scrollbar.ValueChanged += Scroll_ValueChanged;



    }

    private static string GetLabel(string stringkey)
    {

        //TODO: Performance
        string connectionString = @"Server = " + Globals.server + "; Database=report_manager;Uid=newuser;Pwd=New@Mic15;";
        string queryStatus = "SELECT * from status where status_name='" + stringkey + "'";
        using (MySqlConnection Conn = new MySqlConnection(connectionString))
        {
            try
            {
                Conn.Open();

                MySqlCommand cmd = new MySqlCommand(queryStatus, Conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    stringkey = reader[CultureInfo.CurrentCulture.ThreeLetterISOLanguageName].ToString();
                }


                Conn.Close();
            }

            catch (Exception ex)
            {
                Debug.WriteLine("ERRO Status: " + ex);
            }
        }


        return stringkey;
    }
}

