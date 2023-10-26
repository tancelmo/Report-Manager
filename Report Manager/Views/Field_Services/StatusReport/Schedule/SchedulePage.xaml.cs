using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using MySql.Data.MySqlClient;
using Report_Manager.Common;
using Report_Manager.Data;
using Report_Manager.ViewModels;
using Syncfusion.UI.Xaml.DataGrid.Export;
using Syncfusion.XlsIO;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Report_Manager.Helpers;
using Report_Manager.Views.Field_Services.StatusReport;
using Report_Manager.Views.Field_Services;
using System.Diagnostics;
using Windows.Devices.PointOfService;
using Mysqlx.Crud;
using System.Globalization;
using System.Data;
using Syncfusion.UI.Xaml.Data;
using Syncfusion.UI.Xaml.DataGrid;
using CommunityToolkit.WinUI.UI.Controls;
using Syncfusion.Drawing;
using Syncfusion.UI.Xaml.Grids;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Media.Animation;
using Report_Manager.Views.Field_Services.StatusReport.Schedule.SettingsViews;
using Microsoft.UI.Xaml.Navigation;
using Org.BouncyCastle.Asn1.X509;
using System.Drawing;
using System.Security.Policy;
using Report_Manager.Views.Field_Services.StatusReport.Schedule;
using System.Diagnostics.Metrics;

namespace Report_Manager.Views;

public sealed partial class SchedulePage : Page, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChaged;
    public event PropertyChangedEventHandler? PropertyChanged;
    public static SchedulePage? SchedulePageCurrent;
    ToolTip toolTip = new ToolTip();
    public string currenStatus;

    ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);

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

    ConfigFile _configFile = new ConfigFile(Globals.ConfigFilePath);
    private void OnPropertyChanged(string propertyName) => PropertyChaged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public bool connOk;
    // ObservableCollection<object> collection = new ObservableCollection<object>();
    private ObservableCollection<ScheduleData> _scheduleData;

    public ScheduleViewModel ViewModel
    {
        get;
    }

    public SchedulePage()
    {
        StartUp.Language();
        ViewModel = App.GetService<ScheduleViewModel>();
        InitializeComponent();
        SearchBox.ComboboxFilter(ShellPage.CurrentMain.NavigationViewControl, ShellPage.CurrentMain.GeneralSearchFilter);
        datePickerSch.Date = DateTime.Now;

        SchedulePageCurrent = this;
        PageOptions.RibbonControlsText(this);
        Orders = new ObservableCollection<ScheduleData>();
    }

    private async void excelExp_Click(object sender, RoutedEventArgs e)
    {
        ringExcel.Visibility = Visibility.Visible;
        lblbtnExcel.Text = string.Empty;
        await Task.Delay(100);
        var options = new DataGridExcelExportOptions();
        options.ExcelVersion = ExcelVersion.Excel2013;
        options.ExcludedColumns.Add("Status");
        options.GridExportHandler = GridExportHandler;
        var excelEngine = dataGrid.ExportToExcel(dataGrid.SelectedItems, options);
        var workBook = excelEngine.Excel.Workbooks[0];
        MemoryStream outputStream = new MemoryStream();
        workBook.SaveAs(outputStream);
        Save(outputStream, "Schedule".GetLocalized() + "_" + DateTime.Now);
        ringExcel.Visibility = Visibility.Collapsed;
        lblbtnExcel.Text = "ExportButtonNormal".GetLocalized();
    }

    private void GridExportHandler(object sender, DataGridExcelExportStartOptions e)
    {
        if (e.CellType == ExportCellType.HeaderCell)
        {
            e.Style.Color = Syncfusion.Drawing.Color.FromArgb(100, 228, 234);
            e.Style.Font.Color = ExcelKnownColors.White;
            e.Handled = true;
        }

        else if (e.CellType == ExportCellType.RecordCell)
        {
            e.Style.Color = Syncfusion.Drawing.Color.FromArgb(240, 224, 144);
            e.Handled = true;
        }

        else if (e.CellType == ExportCellType.GroupCaptionCell)
        {
            e.Style.Color = Syncfusion.Drawing.Color.FromArgb(252, 159, 161);
            e.Handled = true;
        }
        //if (e.CellType == ExportCellType.RecordCell)
        //{

        //    e.Style.ColorIndex = ExcelKnownColors.Custom17;
        //    e.Style.Font.Color = ExcelKnownColors.Custom14;
        //}
    }

    async void Save(MemoryStream stream, string filename)
    {
        StorageFile stFile;

        if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")))
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.DefaultFileExtension = ".xlsx";
            savePicker.SuggestedFileName = filename;
            savePicker.FileTypeChoices.Add("Excel Documents", new List<string>() { ".xlsx" });
            var hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);
            stFile = await savePicker.PickSaveFileAsync();
        }
        else
        {
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            stFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        }
        if (stFile != null)
        {
            using (IRandomAccessStream zipStream = await stFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                //Write the compressed data from the memory to the file
                using (Stream outstream = zipStream.AsStreamForWrite())
                {
                    byte[] buffer = stream.ToArray();
                    outstream.Write(buffer, 0, buffer.Length);
                    outstream.Flush();
                }
            }
            //Launch the saved Excel file.
            await Windows.System.Launcher.LaunchFileAsync(stFile);
        }
    }

    public static T MyFindDataGridChildOfType<T>(DependencyObject root) where T : class
    {
        var MyQueue = new Queue<DependencyObject>();
        MyQueue.Enqueue(root);
        while (MyQueue.Count > 0)
        {
            DependencyObject current = MyQueue.Dequeue();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
            {
                var child = VisualTreeHelper.GetChild(current, i);
                var typedChild = child as T;
                if (typedChild != null)
                {
                    return typedChild;
                }
                MyQueue.Enqueue(child);
            }
        }
        return null;
    }

    private void Scroll_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        var index = (int)(e.NewValue) / 32;
        _configFile.Write("RowPsition", index.ToString(), "ScheduleGrid");


    }
    private async void btnRefresh_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {


        //// Clear previous returned file name, if it exists, between iterations of this scenario
        ////PickAPhotoOutputTextBlock.Text = "";

        //// Create a file picker
        //var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        //// Retrieve the window handle (HWND) of the current WinUI 3 window.
        ////var window = WindowHelper.GetWindowForElement(this);
        //var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

        ////// Initialize the file picker with the window handle (HWND).
        //WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        //// Set options for your file picker
        //openPicker.ViewMode = PickerViewMode.Thumbnail;
        //openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //openPicker.FileTypeFilter.Add(".xls");
        //openPicker.FileTypeFilter.Add(".xlsx");
        //openPicker.FileTypeFilter.Add(".xlsm");

        //// Open the picker for the user to pick a file
        //var file = await openPicker.PickSingleFileAsync();
        //if (file != null)
        //{
        //    //PickAPhotoOutputTextBlock.Text = file.Path;
        //    try
        //    {


        //        string conn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'", file.Path);
        //        OleDbConnection connection = new OleDbConnection();
        //        connection.ConnectionString = conn;
        //        connection.Open();

        //        OleDbCommand command = new OleDbCommand("SELECT * from [User$]", connection);
        //        OleDbDataAdapter adapter = new OleDbDataAdapter();
        //        adapter.SelectCommand = command;
        //        DataTable data = new DataTable();
        //        adapter.Fill(data);
        //        try
        //        {
        //            dataGrid.DataContext = data;

        //            dataGrid.ItemsSource = data.DefaultView;

        //            adapter.Update(data);
        //            connection.Close();
        //            //Convert Data Table to ObservableCollection UWP/WinUI2 ~ WinUI3 only

        //            //DataTableToObservableCollection.Convert(data, dataGrid, collection);
        //            //dataGrid.ItemsSource = data.DefaultView;
        //            DataGridRing.Visibility = Visibility.Collapsed;
        //            Loadinglbl.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        //        }
        //        catch (Exception ex)
        //        {
        //            LogFile.Write("TesteFilePicker", ex.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFile.Write("ExcpetionTestDataBaseEngine", ex.ToString());
        //    }
        //}
        //else
        //{
        //    //PickAPhotoOutputTextBlock.Text = "Operation cancelled.";
        //}
    }

    //private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    //Installation.Text = ((TextBlock)dataGrid.Columns[1].GetValue(dataGrid.SelectedItem)).Text;
    //    //Costumer.Text = ((TextBlock)dataGrid.Columns[3].GetCellContent(dataGrid.SelectedItem)).Text;
    //    //Meter.Text = ((TextBlock)dataGrid.Columns[6].GetCellContent(dataGrid.SelectedItem)).Text;
    //}


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

            DataGridRing.Visibility = Visibility.Collapsed;
            Loadinglbl.Text = "Failed to connect to data server.";
            Loadinglbl.Foreground = new SolidColorBrush(Colors.Red);
        }
        if (connOk)
        {
            try
            {
                var scheduleData = await GetDataAsync();
                scheduleData.ForEach(o => Orders.Add(o));
                storyBoardGrid.Begin();
                pageGrid.Visibility = Visibility.Visible;
                DataGridRing.Visibility = Visibility.Collapsed;
                Loadinglbl.Visibility = Visibility.Collapsed;
                await Task.Delay(100);


                //scroll position datagrid SchedulePage

                dataGrid.ScrollInView(new Syncfusion.UI.Xaml.Grids.ScrollAxis.RowColumnIndex(Convert.ToInt32(_configFile.Read("RowPsition", "ScheduleGrid")) + 20, 0));
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
        var scrollbar = MyFindDataGridChildOfType<ScrollBar>(dataGrid);

        scrollbar.ValueChanged += Scroll_ValueChanged;


    }

    public async Task<List<ScheduleData>> GetDataAsync()
    {
        try
        {
            var query = "select * from schedule";

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
                            if (!await reader.IsDBNullAsync(2))
                                order.Installation = reader.GetString(2);
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
                            if (order.Notes != "" && order.Notes != null)
                            {
                                order.NotesValidation = true;
                            }
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
                            if (!await reader.IsDBNullAsync(25))
                                order.Events = reader.GetString(25);
                            if(order.Events != "" && order.Events != null)
                            {
                                order.EventsValidation = true;
                            }

                            scheduleData.Add(order);

                        }

                    }
                }
                using (MySqlConnection Conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        statustext.Items.Clear();
                        MySqlDataAdapter adapter = new MySqlDataAdapter("select * from status where enabled=true", Conn);
                        DataTable typedata = new DataTable();

                        adapter.Fill(typedata);

                        for (int i = 0; i < typedata.Rows.Count; i++)
                        {
                            statustext.Items.Add(typedata.Rows[i][CultureInfo.CurrentCulture.ThreeLetterISOLanguageName]);
                            configFile.Write(typedata.Rows[i]["status_name"].ToString(), typedata.Rows[i][CultureInfo.CurrentCulture.ThreeLetterISOLanguageName].ToString(), "ScheduleGrid");
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }


                    Conn.Close();

                }
                connection.Close();

            }
            return scheduleData;



        }
        catch (Exception ex)
        {
            Loadinglbl.Text = "Failed to loading data.";
            Loadinglbl.Foreground = new SolidColorBrush(Colors.Red);
            throw;
        }

    }



    private void dataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grids.GridSelectionChangedEventArgs e)
    {


        foreach (var data in dataGrid.SelectedItems)
        {

            ScheduleData? myData = data as ScheduleData;

            if (myData != null)
            {
                currenStatus = GetLabel(myData.Status);
                toolTip.Content = "ScheduleColumn10".GetLocalized() + " " + myData.Key;
                ToolTipService.SetToolTip(KeyBorder, toolTip);
                Installation.Text = myData.Installation;
                Costumer.Text = myData.Costumer;
                Medidor.Text = myData.District + " - " + myData.City;
                ClassificationTbx.Text = myData.Classification;
                meterSerialNumber.Text = myData.MeterType + " - " + myData.MeterSerialNumber;
                meterSerialType.Text = myData.Meter;
                PTZ.Text = myData.PTZ;
                PTZSN.Text = myData.PTZSerialNumber;
                GasSupplyStop.IsChecked = myData.Bypass;
                Emergency.IsChecked = myData.Emergency;
                //PTZSN.Text = dataGrid.CurrentItem.ToString();
                statustext.SelectedValue = myData.Status;// + " - " + myData.ID.ToString();
                statustext.IsEnabled = true;
                lblDate.Text = Convert.ToDateTime(myData.Date).ToString("dd");
                lblDatefull.Text = Convert.ToDateTime(myData.Date).ToString("dddd").ToUpper();
                lblDateshort.Text = Convert.ToDateTime(myData.Date).ToString("d");
                datePickerSch.Date = myData.Date;
                double y = 0;
                
                if (myData.Notes != "" && myData.Notes != null && StatusReportPage.statusReportCurrent != null && Globals.ShowNotes)
                {

                    StatusReportPage.statusReportCurrent.NotesTeachingTip.IsOpen = true;
                    StatusReportPage.statusReportCurrent.NotesTeachingTip.Title = "Notes";
                    StatusReportPage.statusReportCurrent.NotesTeachingTipText.Text = myData.Notes;
                    y = StatusReportPage.statusReportCurrent.NotesTeachingTip.ActualHeight;
                    
                }
                else
                {
                    StatusReportPage.statusReportCurrent.NotesTeachingTip.IsOpen = false;
                }

                if (myData.Events != "" && myData.Events != null && StatusReportPage.statusReportCurrent != null && Globals.ShowEvent)
                {
                    if(StatusReportPage.statusReportCurrent.NotesTeachingTip.IsOpen == true)
                    {
                        
                        StatusReportPage.statusReportCurrent.EventTeachingTip.PlacementMargin = new Thickness(20, y + 40, 20, 20);
                        StatusReportPage.statusReportCurrent.EventTeachingTip.IsOpen = true;
                        StatusReportPage.statusReportCurrent.EventTeachingTip.Title = "Events";
                        StatusReportPage.statusReportCurrent.EventTeachingTipText.Text = myData.Events;
                        Debug.WriteLine("Notes é true, heigth é: " + y + " e ActualHeigth é: ");
                    }
                    else
                    {
                        StatusReportPage.statusReportCurrent.EventTeachingTip.PlacementMargin = new Thickness(20);
                        StatusReportPage.statusReportCurrent.EventTeachingTip.IsOpen = true;
                        StatusReportPage.statusReportCurrent.EventTeachingTip.Title = "Events";
                        StatusReportPage.statusReportCurrent.EventTeachingTipText.Text = myData.Events;
                        Debug.WriteLine("Notes é false");
                    }
                    
                }
                else
                {
                    StatusReportPage.statusReportCurrent.EventTeachingTip.IsOpen = false;
                }

                datePickerSch.IsEnabled = true;
                datagridSelectedItemsCounter = dataGrid.SelectedItems.Count;
                AddMobile.Text = "Enviar " + dataGrid.SelectedItems.Count + " Item(ns) para RPMobile";
                RemoveMobile.Text = "Remover " + dataGrid.SelectedItems.Count + " Item(ns) para RPMobile";
                counterLabel.Text = dataGrid.SelectedItems.Count.ToString() + " " + "ExportButtonSelected".GetLocalized();
                if (int.TryParse(myData.Key, out int num))
                {
                    storyBoardKey.Begin();
                    KeyBorder.Visibility = Visibility.Visible;
                    meterSerialNumber.Width = 255;
                    KeyLabel.Text = myData.Key;
                }
                else
                {
                    KeyBorder.Visibility = Visibility.Collapsed;

                    storyBoardKey2.Begin();
                }

                if (Globals.ShowPreview)
                {
                    if (ResultsPreview.IsOpen == false)
                    {

                        ResultsPreview.IsOpen = true;
                        PrevStatus.Text = myData.Status;
                        PrevInstallation.Text = myData.Installation;
                        PrevTeam.Text = myData.Team;
                        PrevCostumer.Text = myData.Costumer;
                        PrevMeter.Text = myData.Meter;
                        PrevMeterSN.Text = myData.MeterType + " - " + myData.MeterSerialNumber;
                        PrevClassification.Text = myData.Classification;
                        PrevPTZ.Text = myData.PTZ;
                        PrevPTZSN.Text = myData.PTZSerialNumber;
                        PrevKey.Text = myData.Key;
                        PrevDistrict.Text = myData.District;
                        PrevStreet.Text = myData.Street;
                        PrevCity.Text = myData.City;
                        PrevDate.Text = Convert.ToDateTime(myData.Date).ToString("D");
                        PrevPrice.Text = myData.Price.ToString("C");
                        PrevStopGas.IsChecked = myData.Bypass;
                        PrevEmergency.IsChecked = myData.Emergency;
                        PrevNotes.Text = myData.Notes;

                    }
                    else
                    {
                        ResultsPreview.IsOpen = false;
                    }

                }
            }

        }


    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (dataGrid.SelectedItem != null)
        {
            FieldServicesDialogs.ScheduleEditOptions(this);
        }
        else
        {
            MessageDialog.Show(this, "NoItemSelected".GetLocalized());
        }

    }

    private void statustext_DropDownClosed(object sender, object e)
    {
        if (dataGrid.SelectedItem != null && statustext.SelectedItem.ToString() != currenStatus)
        {
            Debug.WriteLine(currenStatus + " " + statustext.SelectedItem.ToString());
            FieldServicesDialogs.ExecuteActionOptions(this);

        }
        else if (dataGrid.SelectedItem != null && statustext.SelectedItem.ToString() == currenStatus)
        {

        }
        else
        {
            MessageDialog.Show(this, "NoItemSelected".GetLocalized());
            statustext.SelectedValue = 0;

        }
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


    private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
    {
        ("page1", typeof(SettingsPage1)),
        ("page2", typeof(SettingsPage2)),
        ("page3", typeof(SettingsPage3)),

    };
    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked == true)
        {
            NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
        }
        else if (args.InvokedItemContainer != null)
        {
            var navItemTag = args.InvokedItemContainer.Tag.ToString();
            NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
        }
    }

    private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
    {
        Type _page = null;
        if (navItemTag == "settings")
        {
            _page = typeof(SettingsPage);
        }
        else
        {
            var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
            _page = item.Page;
        }
        // Get the page type before navigation so you can prevent duplicate
        // entries in the backstack.
        var preNavPageType = contentFrame.CurrentSourcePageType;

        // Only navigate if the selected page isn't currently loaded.
        if (!(_page is null) && !Type.Equals(preNavPageType, _page))
        {
            contentFrame.Navigate(_page, null, transitionInfo);
        }
    }
    private bool TryGoBack()
    {
        if (!contentFrame.CanGoBack)
            return false;

        // Don't go back if the nav pane is overlayed.
        if (NavView.IsPaneOpen &&
            (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
             NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
            return false;

        contentFrame.GoBack();
        return true;
    }
    private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        TryGoBack();
    }

    private void contentFrame_NavigationFailed(object sender, Microsoft.UI.Xaml.Navigation.NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }
    private void NavView_Loaded(object sender, RoutedEventArgs e)
    {
        // Add handler for ContentFrame navigation.
        contentFrame.Navigated += On_Navigated;

        // NavView doesn't load any page by default, so load home page.
        NavView.SelectedItem = NavView.MenuItems[0];
        // If navigation occurs on SelectionChanged, this isn't needed.
        // Because we use ItemInvoked to navigate, we need to call Navigate
        // here to load the home page.
        NavView_Navigate("page1", new EntranceNavigationTransitionInfo());

    }

    private void On_Navigated(object sender, NavigationEventArgs e)
    {
        NavView.IsBackEnabled = contentFrame.CanGoBack;

        if (contentFrame.SourcePageType == typeof(SettingsPage))
        {
            // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
            NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
            NavView.Header = "Settings";
        }
        else if (contentFrame.SourcePageType != null)
        {
            var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

            NavView.SelectedItem = NavView.MenuItems
                .OfType<NavigationViewItem>()
                .First(n => n.Tag.Equals(item.Tag));

            NavView.Header =
                ((NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
        }
    }

    private void BtnNew_Click(object sender, RoutedEventArgs e)
    {
        FieldServicesDialogs.ScheduleNew(this);
        
    }

    private void showPreview_Checked(object sender, RoutedEventArgs e)
    {
        Globals.ShowPreview = true;
        configFile.Write("ShowPreview", "1", "General");
    }

    private void showPreview_Unchecked(object sender, RoutedEventArgs e)
    {
        Globals.ShowPreview = false;
        configFile.Write("ShowPreview", "0", "General");
    }

    private void showEvent_Checked(object sender, RoutedEventArgs e)
    {
        Globals.ShowEvent = true;
        configFile.Write("ShowEvent", "1", "General");
    }

    private void showEvent_Unchecked(object sender, RoutedEventArgs e)
    {
        Globals.ShowEvent = false;
        configFile.Write("ShowEvent", "0", "General");
    }

    private void showNotes_Checked(object sender, RoutedEventArgs e)
    {
        Globals.ShowNotes = true;
        configFile.Write("ShowNotes", "1", "General");
    }

    private void showNotes_Unchecked(object sender, RoutedEventArgs e)
    {
        Globals.ShowNotes = false;
        configFile.Write("ShowNotes", "0", "General");
    }
    int selectedItem;
    string costumer;
    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        
       
        if (SchedulePage.SchedulePageCurrent != null)
        {

            foreach (var data in SchedulePage.SchedulePageCurrent.dataGrid.SelectedItems)
            {
                ScheduleData? myData = data as ScheduleData;

                if (myData != null)
                {
                    selectedItem = myData.ID;
                    costumer = myData.Costumer;
                }
                using (MySqlConnection Conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        Conn.Open();
                        MySqlCommand comm = Conn.CreateCommand();
                        comm.CommandText = "UPDATE schedule set mobileAvaliable=1 WHERE ID=@ID";
                        comm.Parameters.AddWithValue("@ID", selectedItem.ToString());
                        Debug.WriteLine(comm.CommandText);
                        comm.ExecuteNonQuery();
                        //MessageDialog.Show(this, "Enviado " + costumer);

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        //MessageDialog.Show(this, "Deu ruim");
                    }


                    Conn.Close();
                }

            }
            //using (MySqlConnection Conn = new MySqlConnection(connectionString))
            //{
            //    try
            //    {
            //        Conn.Open();
            //        MySqlCommand comm = Conn.CreateCommand();
            //        comm.CommandText = "UPDATE schedule set mobileAvaliable=1 WHERE ID=@ID";
            //        comm.Parameters.AddWithValue("@ID", selectedItem.ToString());
            //        Debug.WriteLine(comm.CommandText);
            //        comm.ExecuteNonQuery();
            //        MessageDialog.Show(this, "Enviado " + costumer);

            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine(ex.Message);
            //        MessageDialog.Show(this, "Deu ruim");
            //    }


            //    Conn.Close();
            //}
            
        }
    }

    private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
    {
        if (SchedulePage.SchedulePageCurrent != null)
        {

            foreach (var data in SchedulePage.SchedulePageCurrent.dataGrid.SelectedItems)
            {
                ScheduleData? myData = data as ScheduleData;
                
                if (myData != null)
                {
                    selectedItem = myData.ID;
                    costumer = myData.Costumer;
                    Debug.WriteLine(myData.Costumer);
                }
                using (MySqlConnection Conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        Conn.Open();
                        MySqlCommand comm = Conn.CreateCommand();
                        comm.CommandText = "UPDATE schedule set mobileAvaliable=0 WHERE ID=@ID";
                        comm.Parameters.AddWithValue("@ID", selectedItem.ToString());
                        Debug.WriteLine(comm.CommandText);
                        comm.ExecuteNonQuery();
                        //MessageDialog.Show(this, "Removido do " + costumer);

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        //MessageDialog.Show(this, "Deu ruim");
                    }


                    Conn.Close();
                }
            }
            //using (MySqlConnection Conn = new MySqlConnection(connectionString))
            //{
            //    try
            //    {
            //        Conn.Open();
            //        MySqlCommand comm = Conn.CreateCommand();
            //        comm.CommandText = "UPDATE schedule set mobileAvaliable=0 WHERE ID=@ID";
            //        comm.Parameters.AddWithValue("@ID", selectedItem.ToString());
            //        Debug.WriteLine(comm.CommandText);
            //        comm.ExecuteNonQuery();
            //        MessageDialog.Show(this, "Removido do " + costumer);

            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine(ex.Message);
            //        MessageDialog.Show(this, "Deu ruim");
            //    }


            //    Conn.Close();
            //}

        }
    }
}
