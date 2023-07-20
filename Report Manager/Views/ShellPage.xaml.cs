using Microsoft.UI;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

using Report_Manager.Contracts.Services;
using Report_Manager.Helpers;
using Report_Manager.ViewModels;

using Windows.System;
using WinRT.Interop;
using Microsoft.UI.Xaml.Media.Animation;
using Report_Manager.Common;
using User = Report_Manager.Common.User;

namespace Report_Manager.Views;

// TODO: Update NavigationViewItem titles and icons in ShellPage.xaml.
public sealed partial class ShellPage : Page
{
    public static AppWindow? m_AppWindow;

    public static ShellPage? CurrentMain;
    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel)
    {
        CurrentMain = this;
        ViewModel = viewModel;
        InitializeComponent();
        PersonFlyoutUserName.Text = User.Name;
        PersonFlyoutUserMail.Text = User.login;
        StartUp.CustomStylesGridCell();


        m_AppWindow = GetAppWindowForCurrentWindow();

        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var titleBar = m_AppWindow.TitleBar;
            // Hide default title bar.
            titleBar.ExtendsContentIntoTitleBar = true;


            AppTitleBar.Loaded += AppTitleBar_Loaded;
            AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
            m_AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

            //SetTitleBarColors();


        }
        else
        {
            // Title bar customization using these APIs is currently
            // supported only on Windows 11. In other cases, hide
            // the custom title bar element.
            AppTitleBar.Visibility = Visibility.Collapsed;
            App.MainWindow.ExtendsContentIntoTitleBar = true;
            App.MainWindow.SetTitleBar(AppTitleBar);
            App.MainWindow.Activated += MainWindow_Activated;
        }
        NavigationFrame.Navigate(typeof(Views.MainPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        //App.MainWindow.ExtendsContentIntoTitleBar = true;
        //App.MainWindow.SetTitleBar(AppTitleBar);
        //App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
        if (m_AppWindow.Presenter is OverlappedPresenter p)
        {
            p.SetBorderAndTitleBar(true, true);
            p.IsResizable = true;
        }

    }

    private AppWindow GetAppWindowForCurrentWindow()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(App.MainWindow);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        if (AppWindowTitleBar.IsCustomizationSupported()
            && m_AppWindow.TitleBar.ExtendsContentIntoTitleBar)
        {
            SetDragRegionForCustomTitleBar(m_AppWindow);
        }
    }

    private bool HideByCode;
    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (AppWindowTitleBar.IsCustomizationSupported()
            && m_AppWindow.TitleBar.ExtendsContentIntoTitleBar && m_AppWindow != null)
        {
            // Update drag region if the size of the title bar changes.
            SetDragRegionForCustomTitleBar(m_AppWindow);
        }

        // Serach box Adaptative width
        Debug.WriteLine(this.ActualWidth);


    }
    [DllImport("Shcore.dll", SetLastError = true)]
    internal static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    internal enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
    }

    private double GetScaleAdjustment()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(App.MainWindow);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
        IntPtr hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        // Get DPI.
        int result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out uint dpiX, out uint _);
        if (result != 0)
        {
            throw new Exception("Could not get DPI for monitor.");
        }

        uint scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }
    private void SetDragRegionForCustomTitleBar(AppWindow appWindow)
    {
        if (AppWindowTitleBar.IsCustomizationSupported()
            && appWindow.TitleBar.ExtendsContentIntoTitleBar)
        {
            double scaleAdjustment = GetScaleAdjustment();

            //RightPaddingColumn.Width = new GridLength(appWindow.TitleBar.RightInset / scaleAdjustment);
            //LeftPaddingColumn.Width = new GridLength(appWindow.TitleBar.LeftInset / scaleAdjustment);

            List<Windows.Graphics.RectInt32> dragRectsList = new();

            Windows.Graphics.RectInt32 dragRectL;
            dragRectL.X = (int)((LeftPaddingColumn.ActualWidth) * scaleAdjustment) + 45;
            dragRectL.Y = 0;
            dragRectL.Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment);
            dragRectL.Width = (int)((IconColumn.ActualWidth
                                    + TitleColumn.ActualWidth
                                    + LeftDragColumn.ActualWidth) * scaleAdjustment);
            dragRectsList.Add(dragRectL);

            Windows.Graphics.RectInt32 dragRectR;
            dragRectR.X = (int)((LeftPaddingColumn.ActualWidth
                                + IconColumn.ActualWidth
                                + AppTitleBarText.ActualWidth
                                + LeftDragColumn.ActualWidth
                                + ComboBoxFilterColumn.ActualWidth
                                + SearchColumn.ActualWidth - 105) * scaleAdjustment);
            dragRectR.Y = 0;
            dragRectR.Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment);
            dragRectR.Width = (int)(RightDragColumn.ActualWidth * scaleAdjustment) - 180;
            dragRectsList.Add(dragRectR);

            Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();

            appWindow.TitleBar.SetDragRectangles(dragRects);

            if (this.ActualWidth < 1120 && GeneralSearchFilter.Visibility == Visibility.Visible && this.ActualWidth > 720)
            {
                GeneralSearchBox.Width = 500 - (1120 - this.ActualWidth);
            }
            //else
            //{
            //    GeneralSearchBox.Width = 500;
            //}
            if (this.ActualWidth < 720 && GeneralSearchFilter.Visibility == Visibility.Visible)
            {
                HideByCode = true;
                GeneralSearchBox.Visibility = Visibility.Collapsed;
                GeneralSearchFilter.Visibility = Visibility.Collapsed;
            }
            if (this.ActualWidth > 720 && GeneralSearchFilter.Visibility == Visibility.Collapsed && HideByCode)
            {
                //create a bool

                GeneralSearchBox.Visibility = Visibility.Visible;
                GeneralSearchFilter.Visibility = Visibility.Visible;
                HideByCode = false;
            }
            if (this.ActualWidth < 940 && GeneralSearchFilter.Visibility == Visibility.Collapsed)
            {
                GeneralSearchBox.Width = 500 - (940 - this.ActualWidth);
            }
            if (this.ActualWidth < 645 && GeneralSearchFilter.Visibility == Visibility.Collapsed)
            {
                HideByCode = true;
                GeneralSearchBox.Visibility = Visibility.Collapsed;

            }
            //else
            //{
            //    if(HideByCode)
            //    {
            //        GeneralSearchBox.Visibility = Visibility.Visible;
            //        GeneralSearchFilter.Visibility = Visibility.Visible;
            //    }

            //}
        }
    }
    private void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (!AppWindowTitleBar.IsCustomizationSupported())
        {
            TitleBarHelper.UpdateTitleBar(RequestedTheme);
        }


        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

        AppTitleBarText.Foreground = (SolidColorBrush)App.Current.Resources[resource];
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    private void NavigationViewControl_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        SetDragRegionForCustomTitleBar(m_AppWindow);
        GeneralSearchFilter.Visibility = Visibility.Collapsed;

        //if (args.IsSettingsInvoked == true)
        //{
        //    NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
        //}
        //else if (args.InvokedItemContainer != null)
        //{
        //    SetDragRegionForCustomTitleBar(m_AppWindow);

        //    var navItemTag = args.InvokedItemContainer.Tag.ToString();
        //    NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
        //}
    }

    private void GeneralSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        SearchBox.Search(NavigationViewControl, GeneralSearchBox, null, null, GeneralSearchFilter);
    }
}
