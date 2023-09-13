using Windows.ApplicationModel;

namespace Report_Manager;

internal class Globals
{
    public static string ConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Settings\\Config.ini";

    public static string ConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Settings";

    public static string? seletedThemes
    {
        get; set;
    }

    public static string? server
    {
        get; set;
    }
    public static string? seletedBackdropStyle
    {
        get; set;
    }

    public static string? seletedLanguage
    {
        get; set;
    }


    public static string AppVersion = "Report Manager " + Package.Current.Id.Version.Major + "." + Package.Current.Id.Version.Minor + "." + Package.Current.Id.Version.Build + "." + Package.Current.Id.Version.Revision;

    public static string? DbAdressSN
    {
        get; set;
    }

    public static string? DbAdressUT
    {
        get; set;
    }
    public static string? ErrorDB
    {
        get; set;
    }

    public static string? DbAdressUM
    {
        get; set;
    }

    public static string? TemplateFolderSN
    {
        get; set;
    }
    public static string? TemplateFolderUT
    {
        get; set;
    }
    public static string? TemplateFolderUM
    {
        get; set;
    }

    public static string? SelectedNumber
    {
        get; set;
    }
    public static string ScheduleNotes = "";

    public static string UserAccess = "";

    public static int FronzenColumns0;

    public static int FronzenColumns1;

    public static int FronzenColumns2;

    public static int FronzenColumns3;

    public static int FronzenColumns4;

    public static bool ShowPreview;

    public static bool ShowNotes;

    public static bool ShowEvent;
}
