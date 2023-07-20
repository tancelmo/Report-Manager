using Microsoft.UI.Xaml.Controls;

using Report_Manager.ViewModels;

namespace Report_Manager.Views;

public sealed partial class HomeFieldServicesPage : Page
{
    public HomeFieldServicesViewModel ViewModel
    {
        get;
    }

    public HomeFieldServicesPage()
    {
        ViewModel = App.GetService<HomeFieldServicesViewModel>();
        InitializeComponent();
    }
}
