using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Report_Manager.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Report_Manager.Views.Field_Services.StatusReport.Schedule.SettingsViews;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage1 : Page
{
    ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
    public SettingsPage1()
    {
        this.InitializeComponent();
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        var value0 = int.TryParse(FrozenColumns.Value.ToString(), out var value);

        if (value0)
        {
            configFile.Write("FronzenColumns0", value.ToString(), "ScheduleGrid");
            Globals.FronzenColumns0 = value;
        }
    }
}
