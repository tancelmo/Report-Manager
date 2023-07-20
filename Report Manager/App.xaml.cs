using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using Report_Manager.Activation;
using Report_Manager.Contracts.Services;
using Report_Manager.Core.Contracts.Services;
using Report_Manager.Core.Services;
using Report_Manager.Helpers;
using Report_Manager.Models;
using Report_Manager.Services;
using Report_Manager.ViewModels;
using Report_Manager.Views;

namespace Report_Manager;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        StartUp.CreateFilesIfNotExist();
        StartUp.getStrings();
        StartUp.Language();

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjU1OTUzNEAzMjMyMmUzMDJlMzBOemFHRW1YL2dDR0RNU1ZXWThvV2xUUVVPWmxaWmVKS05DYnpBck1UL0U4PQ==");

        InitializeComponent();

        // Apply theme choice for login window.
        StartUp.SetTheme();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<InventoryViewModel>();
            services.AddTransient<InventoryPage>();
            services.AddTransient<HomeFieldServicesViewModel>();
            services.AddTransient<HomeFieldServicesPage>();
            services.AddTransient<ScheduleViewModel>();
            services.AddTransient<SchedulePage>();
            services.AddTransient<StatusReportViewModel>();
            services.AddTransient<StatusReportPage>();
            services.AddTransient<UTG250ViewModel>();
            services.AddTransient<UTG250Page>();
            services.AddTransient<UM4000ViewModel>();
            services.AddTransient<UM4000Page>();
            services.AddTransient<SonicalViewModel>();
            services.AddTransient<SonicalPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    //protected async override void OnLaunched(LaunchActivatedEventArgs args)
    //{
    //    base.OnLaunched(args);

    //    await App.GetService<IActivationService>().ActivateAsync(args);
    //}

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Login window = new Login();
        window.Activate();

        //base.OnLaunched(args);

        //await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
