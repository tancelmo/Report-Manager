using System.Diagnostics;
using System.Globalization;
using Microsoft.UI.Xaml;
using Report_Manager.Common;
using Syncfusion.UI.Xaml.DataGrid;

namespace Report_Manager;
internal class StartUp
{
    public static void CreateFilesIfNotExist()
    {
        if (!Directory.Exists(Globals.ConfigPath))
        {
            Directory.CreateDirectory(Globals.ConfigPath);
            
            
        }

        if (!File.Exists(Globals.ConfigFilePath))
        {

            CreateConfigFile();
        }
    }

    internal static void CreateConfigFile()
    {
        using (StreamWriter writer = new StreamWriter(Globals.ConfigFilePath, append: true))
        {
            writer.Write(
@"[General]
Language=en-US
Theme=
BackDropStyle=
AccentColor=#FF0078D4
MarkUP=##
ReportExtension=.xls
ShowPreview=0

[Connection]
ServerAdress=

[Login]
SaveCredentials=0
Credentials=

[FilterDates]
FilterDateSN=2021-01-01
FilterDateUT=2000-01-01
FilterDateUM=2000-01-01

[Database]
DbUT=D:\0 - Development\ReportManager\bin\x86\Debug\Results.mdb
DbUM=D:\0 - Development\ReportManager\bin\x86\Debug\UM.mdb
DbSN=D:\0 - Development\ReportManager\bin\x86\Debug\Sonical\Results.mdb

[DirectoryTemplates]
TemplateFolder1=D:\0 - Development\ReportManager\bin\x86\Debug\ReportTemplates\UT
TemplateFolder2=D:\0 - Development\ReportManager\bin\x86\Debug\ReportTemplates\UM
TemplateFolder3=D:\0 - Development\ReportManager\bin\x86\Debug\ReportTemplates\SN

[TemporaryFolderFiles]
TempFolder1=D:\0 - Development\ReportManager\bin\x86\Debug\ReportTemplates\UT\Temp
TempFolder2=D:\0 - Development\ReportManager\bin\x86\Debug\ReportTemplates\UM\Temp
TempFolder3=D:\0 - Development\ReportManager\bin\x86\Debug\ReportTemplates\SN\Temp

[ScheduleGrid]
RowPsition=0
FronzenColumns0=0
FronzenColumns1=0
FronzenColumns2=0
FronzenColumns3=0
FronzenColumns4=0
Status_000=
Status_001=
Status_002=
Status_003=
Status_004=
Status_005=
Status_006=
Status_007=
Status_008=
Status_009=
Status_010=

[StyleGrid]
Status_000Foreground=
Status_000Background=
Status_001Foreground=
Status_001Background=
Status_002Foreground=
Status_002Background=
Status_003Foreground=
Status_003Background=
Status_004Foreground=
Status_004Background=
Status_005Foreground=
Status_005Background=
Status_006Foreground=
Status_006Background=
Status_007Foreground=
Status_007Background=
Status_008Foreground=
Status_008Background=
Status_009Foreground=
Status_009Background=
Status_010Foreground=
Status_010Background=");

        }
    }
    public static void SetTheme()
    {
        ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
        var theme = configFile.Read("Theme", "General");

        if (theme == "Dark")
        {
            App.Current.RequestedTheme = ApplicationTheme.Dark;
        }
        else if (theme == "Light")
        {
            App.Current.RequestedTheme = ApplicationTheme.Light;
        }
        
    }
    public static void Language()
    {
        ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
        var language = configFile.Read("Language", "General");
        if(language == "")
        {
            language = Thread.CurrentThread.CurrentCulture.ToString();
            configFile.Write("Language", language, "General");
        }
        CultureInfo ci = new CultureInfo(language);
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = language;
        

    }
    public static void getStrings()
    {
        ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
        string seletedTheme = configFile.Read("Theme", "General");
        string seletedBackdrop = configFile.Read("BackDropStyle", "General");
        string selectedLanguage = configFile.Read("Language", "General");
        //ComboBox index theme
        switch (seletedTheme)
        {
            case "Light":

                Globals.seletedThemes = "0";
                break;
            case "Dark":

                Globals.seletedThemes = "1";
                break;
            default:

                Globals.seletedThemes = "2";
                break;
        }
        //ComboBox index backdrop
        switch (seletedBackdrop)
        {
            case "Mica":
                Globals.seletedBackdropStyle = "0";
                break;

            case "MicaAlt":
                Globals.seletedBackdropStyle = "1";
                break;

            case "Acrylic":
                Globals.seletedBackdropStyle = "2";
                break;

            default:
                Globals.seletedBackdropStyle = "3";
                break;
        }
        //ComboBox index language
        if (selectedLanguage == "en-us")
        {
            Globals.seletedLanguage = "0";
        }
        else if (selectedLanguage == "pt-br")
        {
            Globals.seletedLanguage = "1";
        }
        else
        {
            Globals.seletedLanguage = "2";
        }

        //server
        Globals.server = configFile.Read("ServerAdress", "Connection");
        //DataBaseAdress

        Globals.DbAdressSN = configFile.Read("DbSN", "Database");
        Globals.DbAdressUT = configFile.Read("DbUT", "Database");
        Globals.DbAdressUM = configFile.Read("DbUM", "Database");

        //TemplateFolder Adress

        Globals.TemplateFolderSN = configFile.Read("TemplateFolder1", "DirectoryTemplates");
        Globals.TemplateFolderUT = configFile.Read("TemplateFolder2", "DirectoryTemplates");
        Globals.TemplateFolderUM = configFile.Read("TemplateFolder3", "DirectoryTemplates");

        // Fronzen Columns

        Globals.FronzenColumns0 = Convert.ToInt32(configFile.Read("FronzenColumns0", "ScheduleGrid"));
        Globals.FronzenColumns1 = Convert.ToInt32(configFile.Read("FronzenColumns1", "ScheduleGrid"));
        Globals.FronzenColumns2 = Convert.ToInt32(configFile.Read("FronzenColumns2", "ScheduleGrid"));
        Globals.FronzenColumns3 = Convert.ToInt32(configFile.Read("FronzenColumns3", "ScheduleGrid"));
        Globals.FronzenColumns4 = Convert.ToInt32(configFile.Read("FronzenColumns4", "ScheduleGrid"));

        //Show Preview
        if(configFile.Read("ShowPreview", "General") == "1")
        {
            Globals.ShowPreview = true;
        }
        else
        {
            Globals.ShowPreview = false;
        }
        if (configFile.Read("ShowNotes", "General") == "1")
        {
            Globals.ShowNotes = true;
        }
        else
        {
            Globals.ShowNotes = false;
        }
        if (configFile.Read("ShowEvent", "General") == "1")
        {
            Globals.ShowEvent = true;
        }
        else
        {
            Globals.ShowEvent = false;
        }

    }
    public static void CustomStylesGridCell()
    {
        ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
        for (int i = 0; i < 11; i++)
        {
            if (configFile.Read("Status_0" + i.ToString("00") + "Foreground", "StyleGrid") != "" && configFile.Read("Status_0" + i.ToString("00") + "Background", "StyleGrid") != "")
            {
                Style style = new Style(typeof(GridCell));
                style.Setters.Add(new Setter(GridCell.BackgroundProperty, configFile.Read("Status_0" + i.ToString("00") + "BackGround", "StyleGrid")));
                style.Setters.Add(new Setter(GridCell.ForegroundProperty, configFile.Read("Status_0" + i.ToString("00") + "Foreground", "StyleGrid")));
                Application.Current.Resources["Status_0" + i.ToString("00")] = style;
            }
            Debug.WriteLine("Status_0" + i.ToString("00") + " - ForeGround: " + configFile.Read("Status_0" + i.ToString("00") + "Foreground", "StyleGrid") + " - Background: " + configFile.Read("Status_0" + i.ToString("00") + "Background", "StyleGrid"));
        }
        
        
    }
}
