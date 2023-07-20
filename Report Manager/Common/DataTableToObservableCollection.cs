using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Report_Manager.Common;
internal class DataTableToObservableCollection
{
    public static void Convert(DataTable data, DataGrid dataGrid, ObservableCollection<object> observableCollection)
    {
        for (int i = 0; i < data.Columns.Count; i++)
        {
            dataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = data.Columns[i].ColumnName,
                Binding = new Binding { Path = new PropertyPath("[" + i.ToString() + "]") }
            });
        }

        foreach (DataRow row in data.Rows)
        {
            observableCollection.Add(row.ItemArray);
        }

        dataGrid.ItemsSource = observableCollection;

    }
}
