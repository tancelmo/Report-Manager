using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;

using Report_Manager.ViewModels;
using Windows.System;
using Windows.UI.Input.Preview.Injection;
using Report_Manager.Common;
using Report_Manager.Helpers;
using Report_Manager.Filters;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Windowing;

namespace Report_Manager.Views;

public sealed partial class SonicalPage : Page
{
    ObservableCollection<object> collection = new ObservableCollection<object>();
    public DataTable data = new();
    public DataTable FilterData = new DataTable();
    public static SonicalPage? SONICALCurrent;
    public SonicalViewModel ViewModel
    {
        get;
    }

    public SonicalPage()
    {
        ViewModel = App.GetService<SonicalViewModel>();
        InitializeComponent();
        SONICALCurrent = this;
        FilterData = data;
        Date1.Date = DateTime.Now;
        Date2.Date = DateTime.Now;
        SearchBox.ComboboxFilter(ShellPage.CurrentMain.NavigationViewControl, ShellPage.CurrentMain.GeneralSearchFilter);
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            GeneralSearchBox.Visibility = Visibility.Collapsed;
        }
        TemplateSelector.Selector("TemplateFolder3", CbxTemplates);
    }
    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {

        await Task.Delay(100);

        DataGridRing.Visibility = Visibility.Collapsed;
        Loadinglbl.Visibility = Visibility.Collapsed;
        DataRetriever.Sonical(dataGrid, collection, data);

        pageGrid.Visibility = Visibility.Visible;
    }



    private void GeneralSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {

        SearchBox.Search(ShellPage.CurrentMain.NavigationViewControl, GeneralSearchBox, SnFilter, DateFilter, ShellPage.CurrentMain.GeneralSearchFilter);

    }

    private void GeneralSearchBox_GotFocus(object sender, RoutedEventArgs e)
    {
        GeneralSearchBox.PlaceholderText = "GeneralSearchPlaceHolder2".GetLocalized();
    }

    private void GeneralSearchBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
    {
        GeneralSearchBox.PlaceholderText = "GeneralSearchPlaceHolder".GetLocalized();
    }

    private void DataFilter_Checked(object sender, RoutedEventArgs e)
    {
        FilterByDate.FilterSN(data, SnFilter, dataGrid, Date1, Date2, TbxInit, TbxFinal);
    }

    private void DataFilter_Unchecked(object sender, RoutedEventArgs e)
    {
        if (SnFilter.IsChecked == true)
        {
            FilterBySN.FilterSN(data, SnFilter, dataGrid, Date1, Date2, TbxInit, TbxFinal);
        }
        else
        {
            var dataView = data.DefaultView;
            dataView.RowFilter = string.Empty;
            RefreshData.RefreshContent(data, dataGrid);
        }
    }

    private void SnFilter_Checked(object sender, RoutedEventArgs e)
    {
        FilterBySN.FilterSN(data, DateFilter, dataGrid, Date1, Date2, TbxInit, TbxFinal);

    }

    private void SnFilter_Unchecked(object sender, RoutedEventArgs e)
    {
        if (DateFilter.IsChecked == true)
        {
            FilterByDate.FilterSN(data, SnFilter, dataGrid, Date1, Date2, TbxInit, TbxFinal);
        }
        else
        {
            var dataView = data.DefaultView;
            dataView.RowFilter = string.Empty;
            RefreshData.RefreshContent(data, dataGrid);
        }

    }

    private void TbxInit_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (SnFilter.IsChecked == true)
        {
            FilterBySN.FilterSN(data, DateFilter, dataGrid, Date1, Date2, TbxInit, TbxFinal);
        }

    }

    private void TbxFinal_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (SnFilter.IsChecked == true)
        {
            FilterBySN.FilterSN(data, DateFilter, dataGrid, Date1, Date2, TbxInit, TbxFinal);
        }

    }

    private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DataGridCounter.CounterSelectedItems(dataGrid, Counter);
        //TODO PREVIEW SONICAL
        DataGridResultsPreview.ShowPreviewSN(dataGrid, SeeMoreInfo, ResultsPreview, ResultsGrid);

    }

    private void dataGrid_Sorting(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridColumnEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = data;
        var currentSortDirection = e.Column.SortDirection;

        foreach (var column in dataGrid.Columns)
        {
            column.SortDirection = null;
        }

        var sortOrder = "ASC";

        if ((currentSortDirection == null || currentSortDirection == DataGridSortDirection.Descending))
        {
            e.Column.SortDirection = DataGridSortDirection.Ascending;
        }
        else
        {
            sortOrder = "DESC";
            e.Column.SortDirection = DataGridSortDirection.Descending;
        }

        var dataView = dt.DefaultView;

        dataView.Sort = e.Column.Header + " " + sortOrder;
        dt = dataView.ToTable();

        RefreshData.RefreshContent(dt, dataGrid);
    }


    private void Date1_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (DateFilter.IsChecked == true && Date1 != null && Date2 != null)
        {
            FilterByDate.FilterSN(data, SnFilter, dataGrid, Date1, Date2, TbxInit, TbxFinal);
        }

    }

    private void Date2_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        if (DateFilter.IsChecked == true && Date1 != null && Date2 != null)
        {
            FilterByDate.FilterSN(data, SnFilter, dataGrid, Date1, Date2, TbxInit, TbxFinal);
        }

    }


    private async void btnRefresh_Click(object sender, RoutedEventArgs e)
    {

        prgRingRefresh.Visibility = Visibility.Visible;
        btnRefresh.Content = string.Empty;
        await Task.Delay(100);
        // Clear filters -----------------------------------------------
        DateFilter.IsChecked = false;
        SnFilter.IsChecked = false;
        ShellPage.CurrentMain.GeneralSearchBox.Text = string.Empty;
        // -------------------------------------------------------------
        data.Clear();
        DataRetriever.Sonical(dataGrid, collection, data);
        RefreshData.RefreshContent(data, dataGrid);

        btnRefresh.Content = "RefreshButtonStr".GetLocalized();
        prgRingRefresh.Visibility = Visibility.Collapsed;





    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

        this.dataGrid.Focus(FocusState.Programmatic);
        int count = dataGrid.SelectedItems.Count;
        for (int i = 0; i < count; i++)
        {

            Debug.WriteLine(((TextBlock)dataGrid.Columns[2].GetCellContent(dataGrid.SelectedItems[i])).Text + " " + i + "ª data");
        }


    }

    private void btnSelectAll_Click(object sender, RoutedEventArgs e)
    {


        this.dataGrid.Focus(FocusState.Programmatic);

        var inputInjector = InputInjector.TryCreate();
        // Press the keys.
        inputInjector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo
            {
                VirtualKey = (ushort)VirtualKey.Control,
                KeyOptions = InjectedInputKeyOptions.None
            }});

        inputInjector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo
            {
                VirtualKey = (ushort)VirtualKey.A,
                KeyOptions = InjectedInputKeyOptions.None
            }});

        // Release the keys.
        inputInjector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo
            {
                VirtualKey = (ushort)VirtualKey.Control,
                KeyOptions = InjectedInputKeyOptions.KeyUp
            }});

        inputInjector.InjectKeyboardInput(new[] { new InjectedInputKeyboardInfo
            {
                VirtualKey = (ushort)VirtualKey.A,
                KeyOptions = InjectedInputKeyOptions.KeyUp
            }});




    }
}
