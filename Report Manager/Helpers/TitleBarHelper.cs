using System.Diagnostics;
using System.Runtime.InteropServices;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Report_Manager.Common;
using Report_Manager.Views;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Report_Manager.Helpers;

// Helper class to workaround custom title bar bugs.
// DISCLAIMER: The resource key names and color values used below are subject to change. Do not depend on them.
// https://github.com/microsoft/TemplateStudio/issues/4516
internal class TitleBarHelper
{
    private const int WAINACTIVE = 0x00;
    private const int WAACTIVE = 0x01;
    private const int WMACTIVATE = 0x0006;

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    public static bool SetTitleBarColors(Microsoft.UI.Windowing.AppWindow m_AppWindow)
    {
        //Check to see if customization is supported.
       // Currently only supported on Windows 11.
        if (Microsoft.UI.Windowing.AppWindowTitleBar.IsCustomizationSupported())
        {

            var titleBar = m_AppWindow.TitleBar;
            ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
            // Set active window colors
            if (configFile.Read("Theme", "General") == "Light" || configFile.Read("Theme", "General") != "Dark")
            {

                titleBar.ForegroundColor = Colors.Black;
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverForegroundColor = Colors.Black;
                titleBar.ButtonPressedForegroundColor = Colors.Black;
                titleBar.ButtonHoverBackgroundColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#EDEDED");
                titleBar.ButtonPressedBackgroundColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#E9E9E9");
            }
            else
            {
                titleBar.ForegroundColor = Colors.White;
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverForegroundColor = Colors.White;
                titleBar.ButtonPressedForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#383838");
                titleBar.ButtonPressedBackgroundColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#3D3D3D");

            }

            //titleBar.ButtonHoverForegroundColor = Colors.Gainsboro;
            //titleBar.ButtonHoverBackgroundColor = Colors.DarkSeaGreen;
            //titleBar.ButtonPressedForegroundColor = Colors.Gray;
            //titleBar.ButtonPressedBackgroundColor = Colors.LightGreen;

            // Set inactive window colors
            titleBar.InactiveForegroundColor = Colors.Gray;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Gray;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            return true;
        }
        return false;
    }
    public static void UpdateTitleBar(ElementTheme theme)
    {
        Debug.WriteLine(theme + "-" + App.Current.RequestedTheme.ToString());
        ConfigFile _configFile = new ConfigFile(Globals.ConfigFilePath);
        _configFile.Write("Theme", theme.ToString(), "General");


        if (Microsoft.UI.Windowing.AppWindowTitleBar.IsCustomizationSupported())
        {

            var titleBar = ShellPage.m_AppWindow.TitleBar;
            ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
            // Set active window colors
            switch (theme.ToString())
            {
                case "Light":
                    LightTheme();
                    break;

                case "Dark":
                    DarkTheme();
                    break;

                default:
                    if (App.Current.RequestedTheme.ToString() == "Light")
                    {
                        LightTheme();
                    }
                    else
                    {
                        DarkTheme();
                    }
                    break;
            }

            void LightTheme()
            {
                titleBar.ForegroundColor = Colors.Black;
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverForegroundColor = Colors.Black;
                titleBar.ButtonPressedForegroundColor = Colors.Black;
                titleBar.ButtonHoverBackgroundColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#EDEDED");
                titleBar.ButtonPressedBackgroundColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#E9E9E9");
            }
            void DarkTheme()
            {
                titleBar.ForegroundColor = Colors.White;
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverForegroundColor = Colors.White;
                titleBar.ButtonPressedForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#383838");
                titleBar.ButtonPressedBackgroundColor = CommunityToolkit.WinUI.Helpers.ColorHelper.ToColor("#3D3D3D");

            }

            // Set inactive window colors
            titleBar.InactiveForegroundColor = Colors.Gray;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Gray;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

        }
        if (App.MainWindow.ExtendsContentIntoTitleBar)
        {
            if (theme == ElementTheme.Default)
            {
                var uiSettings = new UISettings();
                var background = uiSettings.GetColorValue(UIColorType.Background);

                theme = background == Colors.White ? ElementTheme.Light : ElementTheme.Dark;
            }

            if (theme == ElementTheme.Default)
            {
                theme = Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Dark;
            }

            Application.Current.Resources["WindowCaptionForeground"] = theme switch
            {
                ElementTheme.Dark => new SolidColorBrush(Colors.White),
                ElementTheme.Light => new SolidColorBrush(Colors.Black),
                _ => new SolidColorBrush(Colors.Transparent)
            };

            Application.Current.Resources["WindowCaptionForegroundDisabled"] = theme switch
            {
                ElementTheme.Dark => new SolidColorBrush(Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF)),
                ElementTheme.Light => new SolidColorBrush(Color.FromArgb(0x66, 0x00, 0x00, 0x00)),
                _ => new SolidColorBrush(Colors.Transparent)
            };

            Application.Current.Resources["WindowCaptionButtonBackgroundPointerOver"] = theme switch
            {
                ElementTheme.Dark => new SolidColorBrush(Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF)),
                ElementTheme.Light => new SolidColorBrush(Color.FromArgb(0x33, 0x00, 0x00, 0x00)),
                _ => new SolidColorBrush(Colors.Transparent)
            };

            Application.Current.Resources["WindowCaptionButtonBackgroundPressed"] = theme switch
            {
                ElementTheme.Dark => new SolidColorBrush(Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF)),
                ElementTheme.Light => new SolidColorBrush(Color.FromArgb(0x66, 0x00, 0x00, 0x00)),
                _ => new SolidColorBrush(Colors.Transparent)
            };

            Application.Current.Resources["WindowCaptionButtonStrokePointerOver"] = theme switch
            {
                ElementTheme.Dark => new SolidColorBrush(Colors.White),
                ElementTheme.Light => new SolidColorBrush(Colors.Black),
                _ => new SolidColorBrush(Colors.Transparent)
            };

            Application.Current.Resources["WindowCaptionButtonStrokePressed"] = theme switch
            {
                ElementTheme.Dark => new SolidColorBrush(Colors.White),
                ElementTheme.Light => new SolidColorBrush(Colors.Black),
                _ => new SolidColorBrush(Colors.Transparent)
            };

            Application.Current.Resources["WindowCaptionBackground"] = new SolidColorBrush(Colors.Transparent);
            Application.Current.Resources["WindowCaptionBackgroundDisabled"] = new SolidColorBrush(Colors.Transparent);

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            if (hwnd == GetActiveWindow())
            {
                SendMessage(hwnd, WMACTIVATE, WAINACTIVE, IntPtr.Zero);
                SendMessage(hwnd, WMACTIVATE, WAACTIVE, IntPtr.Zero);
            }
            else
            {
                SendMessage(hwnd, WMACTIVATE, WAACTIVE, IntPtr.Zero);
                SendMessage(hwnd, WMACTIVATE, WAINACTIVE, IntPtr.Zero);
            }
        }
    }

    public static void ApplySystemThemeToCaptionButtons()
    {
        var res = Application.Current.Resources;
        var frame = App.AppTitlebar as FrameworkElement;
        if (frame != null)
        {
            if (frame.ActualTheme == ElementTheme.Dark)
            {
                res["WindowCaptionForeground"] = Colors.White;
            }
            else
            {
                res["WindowCaptionForeground"] = Colors.Black;
            }

            UpdateTitleBar(frame.ActualTheme);
        }
    }
}
