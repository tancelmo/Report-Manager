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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Report_Manager.Views.Login;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NewUser : Page
{
    public static NewUser CurrentNewUser;
    public NewUser()
    {
        this.InitializeComponent();
        CurrentNewUser = this;
    }

    private void btnLoginBack_Click(object sender, RoutedEventArgs e)
    {
        this.Frame.Navigate(typeof(Views.Login.User), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });

    }

    private async void CreateAccount_Click(object sender, RoutedEventArgs e)
    {
        ringLoading.Visibility = Visibility.Visible;
        await Task.Delay(100);
        loginInfo.IsOpen = false;
        Common.Login.NewUser.CheckNewUser(tbxNewUserEmail, tbxNewUserName, tbxNewUserPass, tbxNewUserPass2, loginInfo, ringLoading, borderAccepted, panelContent, test);
        test.Begin();
    }

    private void tbxNewUserEmail_TextChanged(object sender, TextChangedEventArgs e)
    {
        loginInfo.IsOpen = false;
    }

    private void tbxNewUserName_TextChanged(object sender, TextChangedEventArgs e)
    {
        loginInfo.IsOpen = false;
    }

    private void tbxNewUserPass_PasswordChanged(object sender, RoutedEventArgs e)
    {
        loginInfo.IsOpen = false;
    }

    private void tbxNewUserPass2_PasswordChanged(object sender, RoutedEventArgs e)
    {
        loginInfo.IsOpen = false;
    }
}
