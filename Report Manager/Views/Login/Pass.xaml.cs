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
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Report_Manager.Common.Login;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Report_Manager.Views.Login;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Pass : Page
{
    public Pass()
    {
        this.InitializeComponent();
    }
    private void btnLoginBack_Click(object sender, RoutedEventArgs e)
    {
        this.Frame.Navigate(typeof(Views.Login.User), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
    }

    private void pbxPass_PasswordChanged(object sender, RoutedEventArgs e)
    {
        loginError.Visibility = Visibility.Collapsed;
    }

    private async void LoginNext_Click(object sender, RoutedEventArgs e)
    {
        ringLoading.Visibility = Visibility.Visible;
        await Task.Delay(100);
        LoginCommands.LoginUserPass(pbxPass, ringLoading, Report_Manager.Login.CurrentLogin, loginError, e);
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        pbxPass.Focus(FocusState.Programmatic);
    }

    private async void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        ringLoading.Visibility = Visibility.Visible;
        await Task.Delay(100);
        LoginCommands.LoginUserPass(pbxPass, ringLoading, Report_Manager.Login.CurrentLogin, loginError, null);
    }
}
