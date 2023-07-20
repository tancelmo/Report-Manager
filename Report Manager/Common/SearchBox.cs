using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Report_Manager.Views;
using Report_Manager.Helpers;
using System.Diagnostics;
using Syncfusion.UI.Xaml.Data;
using Report_Manager.Data;
using CommunityToolkit.WinUI.UI.Controls;

namespace Report_Manager.Common;
internal class SearchBox
{
    public static bool FilterRecords(object o)
    {
        string filterText = ShellPage.CurrentMain.GeneralSearchBox.Text;
        var item = o as ScheduleData;

        if (item != null)
        {
            var selection = (string)ShellPage.CurrentMain.GeneralSearchFilter.SelectedValue;

            if (selection == "ScheduleColumn0".GetLocalized())
            {
                if (item.Status.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn1".GetLocalized())
            {
                if (item.Installation.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn3".GetLocalized())
            {
                if (item.Costumer.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn4".GetLocalized())
            {
                if (item.Meter.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn5".GetLocalized())
            {
                if (item.MeterSerialNumber.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn6".GetLocalized())
            {
                if (item.MeterType.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn7".GetLocalized())
            {
                if (item.Classification.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn8".GetLocalized())
            {
                if (item.PTZ.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn9".GetLocalized())
            {
                if (item.PTZSerialNumber.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn10".GetLocalized())
            {
                if (item.Key.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn11".GetLocalized())
            {
                if (item.City.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn12".GetLocalized())
            {
                if (item.District.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn13".GetLocalized())
            {
                if (item.Street.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (selection == "ScheduleColumn16".GetLocalized())
            {
                if (item.Notes.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

        }
        return false;
    }
    public static void Search(NavigationView NavView, AutoSuggestBox GeneralSearchBox, CheckBox checkBox, CheckBox checkBox1, ComboBox comboBox)
    {
        NavigationViewItem ItemContent = NavView.SelectedItem as NavigationViewItem;

        var selection = (string)comboBox.SelectedValue;

        if (ItemContent == null)
        {
            return;
        }
        else
        {
            try
            {
                //TODO unselec checkbox
                if (checkBox != null && checkBox.IsChecked == true)
                {
                    checkBox.IsChecked = false;
                }
                if (checkBox1 != null && checkBox1.IsChecked == true)
                {
                    checkBox1.IsChecked = false;
                }

                if (ItemContent.Tag.ToString() == "UTG")
                {
                    DataTable dt = new DataTable();
                    dt = UTG250Page.UTCurrent.data;
                    var dataView = dt.DefaultView;


                    dataView.RowFilter = string.Format("[" + selection + "] Like '%{0}%'", GeneralSearchBox.Text);
                    dt.DefaultView.Sort = "UTHeader2".GetLocalized() + " ASC";
                    dt = dataView.ToTable();





                    if (GeneralSearchBox.Text == String.Empty)
                    {
                        dt = UTG250Page.UTCurrent.data;
                        //dataView.RowFilter = String.Empty;
                        var collection = new ObservableCollection<object>();
                        foreach (DataRow row in dt.Rows)
                        {
                            collection.Add(row.ItemArray);
                        }
                        UTG250Page.UTCurrent.dataGrid.ItemsSource = collection;
                    }


                    else
                    {
                        var collection = new ObservableCollection<object>();
                        foreach (DataRow row in dt.Rows)
                        {
                            collection.Add(row.ItemArray);
                        }
                        UTG250Page.UTCurrent.dataGrid.ItemsSource = collection;
                    }
                }

                else if (ItemContent.Tag.ToString() == "UM4000")
                {
                    DataTable dt = new DataTable();
                    dt = UM4000Page.UMCurrent.data;
                    var dataView = dt.DefaultView;


                    dataView.RowFilter = string.Format("[" + selection + "] Like '%{0}%'", GeneralSearchBox.Text);
                    dt.DefaultView.Sort = "UMHeader2".GetLocalized() + " ASC";
                    dt = dataView.ToTable();




                    if (GeneralSearchBox.Text == String.Empty)
                    {
                        dt = UM4000Page.UMCurrent.data;
                        //dataView.RowFilter = String.Empty;
                        var collection = new ObservableCollection<object>();
                        foreach (DataRow row in dt.Rows)
                        {
                            collection.Add(row.ItemArray);
                        }
                        UM4000Page.UMCurrent.dataGrid.ItemsSource = collection;
                    }


                    else
                    {
                        var collection = new ObservableCollection<object>();
                        foreach (DataRow row in dt.Rows)
                        {
                            collection.Add(row.ItemArray);
                        }
                        UM4000Page.UMCurrent.dataGrid.ItemsSource = collection;
                    }
                }

                else if (ItemContent.Tag.ToString() == "SONICAL")
                {
                    DataTable dt = new DataTable();
                    dt = SonicalPage.SONICALCurrent.data;
                    var dataView = dt.DefaultView;


                    dataView.RowFilter = string.Format("[" + selection + "] Like '%{0}%'", GeneralSearchBox.Text);
                    dt.DefaultView.Sort = "SNHeader2".GetLocalized() + " ASC";
                    dt = dataView.ToTable();




                    if (GeneralSearchBox.Text == String.Empty)
                    {
                        dt = SonicalPage.SONICALCurrent.data;
                        //dataView.RowFilter = String.Empty;
                        var collection = new ObservableCollection<object>();
                        foreach (DataRow row in dt.Rows)
                        {
                            collection.Add(row.ItemArray);
                        }
                        SonicalPage.SONICALCurrent.dataGrid.ItemsSource = collection;
                    }


                    else
                    {
                        var collection = new ObservableCollection<object>();
                        foreach (DataRow row in dt.Rows)
                        {
                            collection.Add(row.ItemArray);
                        }
                        SonicalPage.SONICALCurrent.dataGrid.ItemsSource = collection;
                    }
                }

                else if (ItemContent.Tag.ToString() == "Status_Report")
                {

                    //NavView2 from StatusReprot

                    NavigationViewItem ItemContent2 = StatusReportPage.statusReportCurrent.NavView.SelectedItem as NavigationViewItem;

                    //sfDataGrid.Columns["OrderID"].FilterPredicates.Add(new FilterPredicate() { FilterType = FilterType.Equals, FilterValue = "1005" });
                    //SchedulePage.SchedulePageCurrent.dataGrid.

                    if (ItemContent2.Tag.ToString() == "schedule")
                    {
                        if (SchedulePage.SchedulePageCurrent.dataGrid != null && SchedulePage.SchedulePageCurrent.dataGrid.View != null)
                        {
                            SchedulePage.SchedulePageCurrent.dataGrid.View.Filter = FilterRecords;
                            SchedulePage.SchedulePageCurrent.dataGrid.View.RefreshFilter();
                        }
                    }

                }
                else
                {
                    Debug.Write("Here..");
                }
            }
            catch (Exception ex)
            {
                LogFile.Write("#800000", ex.Message);
                Debug.WriteLine(ex);
            }

        }

    }

    public static void ComboboxFilter(NavigationView NavView, ComboBox comboBox)
    {
        NavigationViewItem ItemContent = NavView.SelectedItem as NavigationViewItem;

        if (ItemContent.Tag.ToString() == "SONICAL")
        {
            comboBox.Items.Clear();
            comboBox.Visibility = Visibility.Visible;
            comboBox.Items.Add("SNHeader0".GetLocalized());
            comboBox.Items.Add("SNHeader2".GetLocalized());
            comboBox.Items.Add("SNHeader3".GetLocalized());
            comboBox.Items.Add("SNHeader4".GetLocalized());
            comboBox.Items.Add("SNHeader5".GetLocalized());
            comboBox.Items.Add("SNHeader6".GetLocalized());
            comboBox.Items.Add("SNHeader7".GetLocalized());
            comboBox.SelectedIndex = 1;
        }

        else if (ItemContent.Tag.ToString() == "UTG")
        {
            comboBox.Items.Clear();
            comboBox.Visibility = Visibility.Visible;
            comboBox.Items.Add("UTHeader0".GetLocalized());
            comboBox.Items.Add("UTHeader1".GetLocalized());
            comboBox.Items.Add("UTHeader2".GetLocalized());
            comboBox.Items.Add("UTHeader3".GetLocalized());
            comboBox.Items.Add("UTHeader4".GetLocalized());
            comboBox.Items.Add("UTHeader5".GetLocalized());
            comboBox.Items.Add("UTHeader6".GetLocalized());
            comboBox.Items.Add("UTHeader7".GetLocalized());
            comboBox.Items.Add("UTHeader8".GetLocalized());
            comboBox.Items.Add("UTHeader9".GetLocalized());
            comboBox.Items.Add("UTHeader10".GetLocalized());
            comboBox.Items.Add("UTHeader11".GetLocalized());
            comboBox.Items.Add("UTHeader12".GetLocalized());
            comboBox.Items.Add("UTHeader13".GetLocalized());
            comboBox.Items.Add("UTHeader14".GetLocalized());

            comboBox.SelectedIndex = 2;
        }

        else if (ItemContent.Tag.ToString() == "UM4000")
        {
            comboBox.Items.Clear();
            comboBox.Visibility = Visibility.Visible;
            comboBox.Items.Add("UMHeader0".GetLocalized());
            comboBox.Items.Add("UMHeader1".GetLocalized());
            comboBox.Items.Add("UMHeader2".GetLocalized());
            comboBox.Items.Add("UMHeader3".GetLocalized());
            comboBox.Items.Add("UMHeader4".GetLocalized());
            comboBox.Items.Add("UMHeader5".GetLocalized());
            comboBox.Items.Add("UMHeader6".GetLocalized());
            comboBox.Items.Add("UMHeader7".GetLocalized());
            comboBox.Items.Add("UMHeader8".GetLocalized());
            comboBox.Items.Add("UMHeader9".GetLocalized());
            comboBox.Items.Add("UMHeader10".GetLocalized());
            comboBox.Items.Add("UMHeader11".GetLocalized());
            comboBox.Items.Add("UMHeader12".GetLocalized());
            comboBox.Items.Add("UMHeader13".GetLocalized());

            comboBox.SelectedIndex = 2;
        }

        else if (ItemContent.Tag.ToString() == "Status_Report")
        {
            comboBox.Items.Clear();
            comboBox.Visibility = Visibility.Visible;
            comboBox.Items.Add("ScheduleColumn0".GetLocalized());
            comboBox.Items.Add("ScheduleColumn1".GetLocalized());
            comboBox.Items.Add("ScheduleColumn3".GetLocalized());
            comboBox.Items.Add("ScheduleColumn4".GetLocalized());
            comboBox.Items.Add("ScheduleColumn5".GetLocalized());
            comboBox.Items.Add("ScheduleColumn6".GetLocalized());
            comboBox.Items.Add("ScheduleColumn7".GetLocalized());
            comboBox.Items.Add("ScheduleColumn8".GetLocalized());
            comboBox.Items.Add("ScheduleColumn9".GetLocalized());
            comboBox.Items.Add("ScheduleColumn10".GetLocalized());
            comboBox.Items.Add("ScheduleColumn11".GetLocalized());
            comboBox.Items.Add("ScheduleColumn12".GetLocalized());
            comboBox.Items.Add("ScheduleColumn13".GetLocalized());
            comboBox.Items.Add("ScheduleColumn16".GetLocalized());

            comboBox.SelectedIndex = 1;




        }
        else
        {
            comboBox.Items.Clear();
            comboBox.Visibility = Visibility.Collapsed;
        }
    }
}
