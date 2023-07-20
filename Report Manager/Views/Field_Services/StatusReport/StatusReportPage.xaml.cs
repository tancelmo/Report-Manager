using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;

using Report_Manager.ViewModels;
using Report_Manager.Views.Field_Services.StatusReport;
using static ABI.System.Windows.Input.ICommand_Delegates;
using Windows.System;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media;
using Report_Manager.Views.Field_Services.StatusReport.Executed;

namespace Report_Manager.Views;

public sealed partial class StatusReportPage : Page
{
    public static StatusReportPage? statusReportCurrent;
    public StatusReportViewModel ViewModel
    {
        get;
    }

    public StatusReportPage()
    {
        ViewModel = App.GetService<StatusReportViewModel>();
        InitializeComponent();
        statusReportCurrent = this;
        ContentFrame.Background = new SolidColorBrush(Colors.Transparent);
    }

    private double NavViewCompactModeThresholdWidth
    {
        get
        {
            return NavView.CompactModeThresholdWidth;
        }
    }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }
    // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
    private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
    {
        ("schedule", typeof(SchedulePage)),
        ("add", typeof(Executed)),

    };

    private void NavView_Loaded(object sender, RoutedEventArgs e)
    {
        // Add handler for ContentFrame navigation.
        ContentFrame.Navigated += On_Navigated;

        // NavView doesn't load any page by default, so load home page.
        NavView.SelectedItem = NavView.MenuItems[0];
        // If navigation occurs on SelectionChanged, this isn't needed.
        // Because we use ItemInvoked to navigate, we need to call Navigate
        // here to load the home page.
        NavView_Navigate("schedule", new EntranceNavigationTransitionInfo());




    }
    private void NavView_ItemInvoked(NavigationView sender,
                                 NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked == true)
        {
            NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
        }
        else if (args.InvokedItemContainer != null)
        {
            var navItemTag = args.InvokedItemContainer.Tag.ToString();
            NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
        }
    }
    private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
    {
        Type _page = null;
        if (navItemTag == "settings")
        {
            _page = typeof(SettingsPage);
        }
        else
        {
            var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
            _page = item.Page;
        }
        // Get the page type before navigation so you can prevent duplicate
        // entries in the backstack.
        var preNavPageType = ContentFrame.CurrentSourcePageType;

        // Only navigate if the selected page isn't currently loaded.
        if (!(_page is null) && !Type.Equals(preNavPageType, _page))
        {
            ContentFrame.Navigate(_page, null, transitionInfo);
        }
    }
    private void NavView_BackRequested(NavigationView sender,
                                   NavigationViewBackRequestedEventArgs args)
    {
        TryGoBack();
    }

    private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
    {
        // When Alt+Left are pressed navigate back
        if (e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
            && e.VirtualKey == VirtualKey.Left
            && e.KeyStatus.IsMenuKeyDown == true
            && !e.Handled)
        {
            e.Handled = TryGoBack();
        }
    }

    private void System_BackRequested(object sender, BackRequestedEventArgs e)
    {
        if (!e.Handled)
        {
            e.Handled = TryGoBack();
        }
    }

    private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
    {
        // Handle mouse back button.
        if (e.CurrentPoint.Properties.IsXButton1Pressed)
        {
            e.Handled = TryGoBack();
        }
    }

    private bool TryGoBack()
    {
        if (!ContentFrame.CanGoBack)
            return false;

        // Don't go back if the nav pane is overlayed.
        if (NavView.IsPaneOpen &&
            (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
             NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
            return false;

        ContentFrame.GoBack();
        return true;
    }

    private void On_Navigated(object sender, NavigationEventArgs e)
    {
        NavView.IsBackEnabled = ContentFrame.CanGoBack;

        if (ContentFrame.SourcePageType == typeof(SettingsPage))
        {
            // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
            NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
            NavView.Header = "Settings";
        }
        else if (ContentFrame.SourcePageType != null)
        {
            var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

            NavView.SelectedItem = NavView.MenuItems
                .OfType<NavigationViewItem>()
                .First(n => n.Tag.Equals(item.Tag));

            NavView.Header =
                ((NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
        }
    }

    private void btnRefresh_Click(object sender, RoutedEventArgs e)
    {
        PageOptions.ScheduleRefreshData(SchedulePage.SchedulePageCurrent, SchedulePage.SchedulePageCurrent.storyBoardGrid);
    }

    private void btnPageSettings_Click(object sender, RoutedEventArgs e)
    {
        if (SchedulePage.SchedulePageCurrent != null)
        {
            if (SchedulePage.SchedulePageCurrent.settingsTeachinTip.IsOpen == false)
            {
                SchedulePage.SchedulePageCurrent.settingsTeachinTip.IsOpen = true;
            }
            else
            {
                SchedulePage.SchedulePageCurrent.settingsTeachinTip.IsOpen = false;
            }

        }
        //SettingsDialog.OpenDialogSchedule(this);
    }

    private void btnMapView_Click(object sender, RoutedEventArgs e)
    {
        MapView view = new();
        view.Activate();
    }
}
