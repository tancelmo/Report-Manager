using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Report_Manager.Common;
using Report_Manager.Common.Login;
using Report_Manager.Contracts.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Report_Manager.Views.Login;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class User : Page
{
    ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
    public User()
    {
        this.InitializeComponent();
    }
    private void tbxUserName_TextChanged(object sender, TextChangedEventArgs e)
    {
        loginError.Visibility = Visibility.Collapsed;
    }

    private async void LoginNext_Click(object sender, RoutedEventArgs e)
    {
        ringLoading.Visibility = Visibility.Visible;
        await Task.Delay(100);
        LoginCommands.LoginUser(tbxUserName, ringLoading, this, loginError);
        if (configFile.Read("SaveCredentials", "Login") == "1")
        {
            configFile.Write("Credentials", tbxUserName.Text, "Login");
        }
        else
        {
            configFile.Write("Credentials", string.Empty, "Login");
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        tbxUserName.Focus(FocusState.Programmatic);

        //--------- Get Save Credential At Startup

        if (configFile.Read("SaveCredentials", "Login") == "1")
        {
            chxCredent.IsChecked = true;
            tbxUserName.Text = configFile.Read("Credentials", "Login");
        }
        else
        {
            chxCredent.IsChecked = false;
            tbxUserName.Text = string.Empty;
        }
    }

    private async void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        ringLoading.Visibility = Visibility.Visible;
        await Task.Delay(100);
        if (configFile.Read("SaveCredentials", "Login") == "1")
        {
            configFile.Write("Credentials", tbxUserName.Text, "Login");
        }
        else
        {
            configFile.Write("Credentials", string.Empty, "Login");
        }
        LoginCommands.LoginUser(tbxUserName, ringLoading, this, loginError);
    }

    private async void KeyboardAccelerator_Invoked_1(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        await App.GetService<IActivationService>().ActivateAsyncMain(args);

        Report_Manager.Login.CurrentLogin.Close();
    }

    private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
    {
        this.Frame.Navigate(typeof(Views.Login.NewUser), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
    }

    private void chxCredent_Checked(object sender, RoutedEventArgs e)
    {
        configFile.Write("SaveCredentials", "1", "Login");
    }

    private void chxCredent_Unchecked(object sender, RoutedEventArgs e)
    {
        configFile.Write("SaveCredentials", string.Empty, "Login");
    }
}
