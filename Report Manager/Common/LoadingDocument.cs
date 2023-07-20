using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Report_Manager.Helpers;

namespace Report_Manager.Common;
internal class LoadingDocument
{
    public static async void OpenDialog(Page page)
    {

        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = page.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.CloseButtonText = "DialogCancel".GetLocalized();
        //dialog.Content = new LoadingRing();
        var result = await dialog.ShowAsync();
        //dialog.Hide();


    }

    public static async void OpenDialogExcelExport(Page page)
    {

        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = page.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Select a date";
        dialog.PrimaryButtonText = "Export";
        dialog.CloseButtonText = Application.Current.Resources["btnColorPickerDialogCancel"].ToString();
        dialog.DefaultButton = ContentDialogButton.Primary;

        //dialog.Content = new ExportToExcel();

        var result = await dialog.ShowAsync();

    }

    public static async void OpenErrorDialog(Page page, string error)
    {

        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = page.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.CloseButtonText = "DialogCancel".GetLocalized();
        dialog.Content = error;
        var result = await dialog.ShowAsync();
        //dialog.Hide();


    }
}
