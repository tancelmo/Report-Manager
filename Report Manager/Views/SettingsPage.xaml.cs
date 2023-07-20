using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Report_Manager.Common;
using Report_Manager.Helpers;
using Report_Manager.ViewModels;

namespace Report_Manager.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);

    private string CurrenSelectedtLanguage;
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {

        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
        LanguageOptions();
    }

    private void LanguageOptions()
    {
        cbxLanguage.Items.Add("EnglishLanguage".GetLocalized());
        cbxLanguage.Items.Add("PortugueseLanguage".GetLocalized());

        string CurrentLanguage = configFile.Read("Language", "General");

        if (CurrentLanguage == "en-US")
        {
            cbxLanguage.SelectedItem = "EnglishLanguage".GetLocalized();
        }
        else if (CurrentLanguage == "pt-BR")
        {
            cbxLanguage.SelectedItem = "PortugueseLanguage".GetLocalized();
        }
        else
        {
            cbxLanguage.SelectedItem = "EnglishLanguage".GetLocalized();
            string ci = Thread.CurrentThread.CurrentUICulture.ToString();
            configFile.Write("Language", ci, "General");
        }
        CurrenSelectedtLanguage = cbxLanguage.SelectedItem.ToString();
        
    }

    private void InfoBarOpen(string messageInfo, string titleInfo)
    {
        infoBarGeneral.IsOpen = true;
        infoBarGeneral.Message = messageInfo;
        infoBarGeneral.Title = titleInfo;
    }

    private void cbxLanguage_DropDownClosed(object sender, object e)
    {
        if(cbxLanguage.SelectedItem.ToString() != CurrenSelectedtLanguage)
        {
            if (cbxLanguage.SelectedItem as string == "EnglishLanguage".GetLocalized())
            {
                configFile.Write("Language", "en-us", "General");
                InfoBarOpen(string.Format("InfoBarSettingfLanguageMessage".GetLocalized(), cbxLanguage.SelectedItem), "InfoBarSettingsLanguageTitle".GetLocalized());
                CurrenSelectedtLanguage = cbxLanguage.SelectedItem.ToString();
            }
            if (cbxLanguage.SelectedItem as string == "PortugueseLanguage".GetLocalized())
            {
                configFile.Write("Language", "pt-br", "General");
                InfoBarOpen(string.Format("InfoBarSettingfLanguageMessage".GetLocalized(), cbxLanguage.SelectedItem), "InfoBarSettingsLanguageTitle".GetLocalized());
                CurrenSelectedtLanguage = cbxLanguage.SelectedItem.ToString();
            }
            
        }
        
    }
}
