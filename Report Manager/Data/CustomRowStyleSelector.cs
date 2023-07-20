using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Report_Manager.Common;
using Syncfusion.UI.Xaml.DataGrid;

namespace Report_Manager.Data
{
    public class CustomRowStyleSelector : StyleSelector
    {
        ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var data = item as ScheduleData;

            if (data != null && ((container as GridCell).ColumnBase.GridColumn.MappingName == "Status"))
            {

                //custom condition is checked based on data.

                if (data.Status == configFile.Read("Status_000", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_000"] as Style;
                }
                    
                if (data.Status == configFile.Read("Status_001", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_001"] as Style;
                }
                    
                if (data.Status == configFile.Read("Status_002", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_002"] as Style;
                }
                    
                if (data.Status == configFile.Read("Status_003", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_003"] as Style;
                }
                    
                if (data.Status == configFile.Read("Status_004", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_004"] as Style;
                }
                if (data.Status == configFile.Read("Status_005", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_005"] as Style;
                }
                if (data.Status == configFile.Read("Status_006", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_006"] as Style;
                }
                if (data.Status == configFile.Read("Status_007", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_007"] as Style;
                }
                if (data.Status == configFile.Read("Status_008", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_008"] as Style;
                }
                if (data.Status == configFile.Read("Status_009", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_009"] as Style;
                }
                if (data.Status == configFile.Read("Status_010", "ScheduleGrid"))
                {
                    return Application.Current.Resources["Status_010"] as Style;
                }


                // return Application.Current.Resources["aquaRowStyle"] as Style;
            }

            return base.SelectStyleCore(item, container);

            
        }
    }
}
