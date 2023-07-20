using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace Report_Manager.Common;
internal class TemplateSelector
{
    public static void Selector(string key, ComboBox comboBox)
    {
        //remover extensao
        ConfigFile readIni = new ConfigFile(Globals.ConfigFilePath);
        DirectoryInfo directoryInfo = new DirectoryInfo(readIni.Read(key, "DirectoryTemplates"));

        try
        {

            FileInfo[] files = directoryInfo.GetFiles("*.xls?", SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files)
            {
                comboBox.Items.Add(Path.GetFileNameWithoutExtension(file.Name));

            }
        }
        catch (Exception ex)
        {
            LogFile.Write("#800008", ex.Message);
        }


    }
}
