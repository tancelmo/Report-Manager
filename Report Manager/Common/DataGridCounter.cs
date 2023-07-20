using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Report_Manager.Helpers;

namespace Report_Manager.Common;
internal class DataGridCounter
{
    public static void CounterSelectedItems(DataGrid dataGrid, TextBlock Counter)
    {
        Counter.Text = dataGrid.SelectedItems.Count + " " + "DataGridCounterLabel".GetLocalized();
    }
}
