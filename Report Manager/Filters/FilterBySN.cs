using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Report_Manager.Helpers;

namespace Report_Manager.Filters;
internal class FilterBySN
{
    public static void FilterUM(DataTable dataTable, CheckBox checkBox, CommunityToolkit.WinUI.UI.Controls.DataGrid dataGrid, CalendarDatePicker datePicker1, CalendarDatePicker datePicker2, TextBox textBox1, TextBox textBox2)
    {
        DataTable dt = new DataTable();
        dt = dataTable;
        string filter;
        var dataView = dt.DefaultView;
        RefreshData.RefreshContent(dataTable, dataGrid);
        if (checkBox.IsChecked == true)
        {
            if (datePicker1.Date == null || datePicker2.Date == null)
            {
                filter = string.Empty;

            }
            else
            {
                filter = string.Format("[" + "UMHeader1".GetLocalized() + "] >= '{0}' AND [" + "UMHeader1".GetLocalized() + "] <= '{1}' AND [" + "UMHeader2".GetLocalized() + "] >= '" + textBox1.Text + "' AND [" + "UMHeader2".GetLocalized() + "] <= '" + textBox2.Text + "'", datePicker1.Date.Value.ToString("d"), datePicker2.Date.Value.AddDays(1).ToString("d"));

            }

        }
        else
        {
            filter = string.Format("[" + "UMHeader2".GetLocalized() + "] >= '" + textBox1.Text + "' AND [" + "UMHeader2".GetLocalized() + "] <= '" + textBox2.Text + "'");

        }
        dataView.RowFilter = string.Format(filter);
        dt.DefaultView.Sort = "UMHeader2".GetLocalized() + " ASC";
        dt = dataView.ToTable();

        RefreshData.RefreshContent(dt, dataGrid);
    }

    public static void FilterUT(DataTable dataTable, CheckBox checkBox, CommunityToolkit.WinUI.UI.Controls.DataGrid dataGrid, CalendarDatePicker datePicker1, CalendarDatePicker datePicker2, TextBox textBox1, TextBox textBox2)
    {
        DataTable dt = new DataTable();
        dt = dataTable;
        string filter;
        var dataView = dt.DefaultView;
        RefreshData.RefreshContent(dataTable, dataGrid);
        if (checkBox.IsChecked == true)
        {
            if (datePicker1.Date == null || datePicker2.Date == null)
            {
                filter = string.Empty;

            }
            else
            {
                filter = string.Format("[" + "UTHeader1".GetLocalized() + "] >= '{0}' AND [" + "UTHeader1".GetLocalized() + "] <= '{1}' AND [" + "UTHeader2".GetLocalized() + "] >= '" + textBox1.Text + "' AND [" + "UTHeader2".GetLocalized() + "] <= '" + textBox2.Text + "'", datePicker1.Date.Value.ToString("d"), datePicker2.Date.Value.AddDays(1).ToString("d"));

            }

        }
        else
        {
            filter = string.Format("[" + "UTHeader2".GetLocalized() + "] >= '" + textBox1.Text + "' AND [" + "UTHeader2".GetLocalized() + "] <= '" + textBox2.Text + "'");

        }
        dataView.RowFilter = string.Format(filter);
        dt.DefaultView.Sort = "UTHeader0".GetLocalized() + " ASC";
        dt = dataView.ToTable();

        RefreshData.RefreshContent(dt, dataGrid);
    }

    public static void FilterSN(DataTable dataTable, CheckBox checkBox, CommunityToolkit.WinUI.UI.Controls.DataGrid dataGrid, CalendarDatePicker datePicker1, CalendarDatePicker datePicker2, TextBox textBox1, TextBox textBox2)
    {
        DataTable dt = new DataTable();
        dt = dataTable;
        string filter;
        var dataView = dt.DefaultView;
        RefreshData.RefreshContent(dataTable, dataGrid);
        if (checkBox.IsChecked == true)
        {
            if (datePicker1.Date == null || datePicker2.Date == null)
            {
                filter = string.Empty;

            }
            else
            {
                filter = string.Format("[" + "SNHeader1".GetLocalized() + "] >= '{0}' AND [" + "SNHeader1".GetLocalized() + "] <= '{1}' AND [" + "SNHeader2".GetLocalized() + "] >= '" + textBox1.Text + "' AND [" + "SNHeader2".GetLocalized() + "] <= '" + textBox2.Text + "'", datePicker1.Date.Value.ToString("d"), datePicker2.Date.Value.AddDays(1).ToString("d"));

            }

        }
        else
        {
            filter = string.Format("[" + "SNHeader2".GetLocalized() + "] >= '" + textBox1.Text + "' AND [" + "SNHeader2".GetLocalized() + "] <= '" + textBox2.Text + "'");

        }
        dataView.RowFilter = string.Format(filter);
        dt.DefaultView.Sort = "SNHeader2".GetLocalized() + " ASC";
        dt = dataView.ToTable();

        RefreshData.RefreshContent(dt, dataGrid);
    }
}
