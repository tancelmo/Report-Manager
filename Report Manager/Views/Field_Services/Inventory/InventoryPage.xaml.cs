using Microsoft.UI.Xaml.Controls;

using Report_Manager.ViewModels;

namespace Report_Manager.Views;

public sealed partial class InventoryPage : Page
{
    public InventoryViewModel ViewModel
    {
        get;
    }

    public InventoryPage()
    {
        ViewModel = App.GetService<InventoryViewModel>();
        InitializeComponent();
    }
}
