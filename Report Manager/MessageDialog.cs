using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Report_Manager.Helpers;
using Report_Manager.Views.Field_Services.StatusReport.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Manager
{
    internal class MessageDialog
    {
        public static async void Show(Page page, string message)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = page.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "AppDisplayName".GetLocalized();
            dialog.CloseButtonText = "CloseButton".GetLocalized();
            dialog.DefaultButton = ContentDialogButton.Primary;

            dialog.Content = message;

            var result = await dialog.ShowAsync();
        }
    }
}
