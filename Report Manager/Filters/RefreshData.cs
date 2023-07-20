using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.UI.Controls;

namespace Report_Manager.Filters;
internal class RefreshData
{
    public static void RefreshContent(DataTable dataTable, DataGrid dataGrid)
    {
        // Create collection
        var collection = new ObservableCollection<object>();
        foreach (DataRow row in dataTable.Rows)
        {
            collection.Add(row.ItemArray);

        }

        dataGrid.ItemsSource = collection;
    }
}
