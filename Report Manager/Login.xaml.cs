using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Report_Manager.Helpers;
using WinRT.Interop;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Report_Manager;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Login : WindowEx
{
    private AppWindow m_AppWindow;
    private OverlappedPresenter _presenter;
    public static Login CurrentLogin;
    public Login()
    {
        this.InitializeComponent();

        CurrentLogin = this;
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));

        Title = "AppDisplayName".GetLocalized();
        this.SetWindowSize(705, 470);
        this.IsResizable = false;
        //this.IsMaximizable = false;
        WindowManager.Get(this).IsMaximizable = false;
        m_AppWindow = GetAppWindowForCurrentWindow();
        GetAppWindowAndPresenter();
        //m_AppWindow.SetIcon("Assets/report-manager.ico");
        TitleBarHelper.SetTitleBarColors(m_AppWindow);
        contentFrame.Navigate(typeof(Views.Login.User));
        version.Text = Globals.AppVersion;
        

        


        //Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");



        //-------------------------------------------------------------------//

        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var titleBar = m_AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;

            //SetTitleBarColors();
            //window_main.Background = null;
            // Set active window colors
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            // Set inactive window colors
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            
        }
        else
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBarW10);
            AppTitleBarW10.Visibility = Visibility.Visible;

            AppTitleBar.Visibility = Visibility.Collapsed;

            // Show alternative UI for any functionality in
            // the title bar, such as search.
        }

        //------------------------------------------------------------------//

    }

    public void GetAppWindowAndPresenter()
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        m_AppWindow = AppWindow.GetFromWindowId(myWndId);
        _presenter = m_AppWindow.Presenter as OverlappedPresenter;
        //_presenter.IsResizable = false;
        //_presenter.IsMaximizable = false;
        //_presenter.SetBorderAndTitleBar(true, false);

        // center screen
        if (m_AppWindow is not null)
        {
            Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(myWndId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
            if (displayArea is not null)
            {
                var CenteredPosition = m_AppWindow.Position;
                CenteredPosition.X = ((displayArea.WorkArea.Width - m_AppWindow.Size.Width) / 2);
                CenteredPosition.Y = ((displayArea.WorkArea.Height - m_AppWindow.Size.Height) / 2);
                m_AppWindow.Move(CenteredPosition);
            }
        }
    }
    private bool SetTitleBarColors()
    {
        // Check to see if customization is supported.
        // Currently only supported on Windows 11.
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            if (m_AppWindow is null)
            {
                m_AppWindow = GetAppWindowForCurrentWindow();
            }
            var titleBar = m_AppWindow.TitleBar;

            // Set active window colors
            //titleBar.ForegroundColor = Colors.White;
            //titleBar.BackgroundColor = Colors.Green;
            //titleBar.ButtonForegroundColor = Colors.White;
            //titleBar.ButtonBackgroundColor = Colors.SeaGreen;
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
    private AppWindow GetAppWindowForCurrentWindow()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }
}
