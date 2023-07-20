using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Report_Manager.Views.Field_Services.StatusReport.Schedule;

namespace Report_Manager.Common;
internal class SettingsDialog
{
    public static async void OpenDialogSchedule(Page page)
    {

        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = page.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Content = new SchedulePageSettings();

        var result = await dialog.ShowAsync();

    }

}
