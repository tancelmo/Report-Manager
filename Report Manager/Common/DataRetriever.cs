using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Report_Manager.Helpers;

namespace Report_Manager.Common;
internal class DataRetriever
{
    public static void Sonical(DataGrid dataGrid, ObservableCollection<object> observableCollection, DataTable data)
    {
        dataGrid.ItemsSource = null;
        dataGrid.Columns.Clear();

        try
        {
            ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
            string date = configFile.Read("FilterDateSN", "FilterDates");
            OdbcConnection connection = new OdbcConnection();
            //Database Adress
            connection.ConnectionString = @"Driver={Microsoft Access Driver (*.mdb)}; Dbq=" + configFile.Read("DbSN", "Database");
            connection.Open();
            //String Query
            string query = "SELECT Meters.CalibrationId as \"" + "SNHeader0".GetLocalized() +
                "\", Calibrations.CalibrationDate as \"" + "SNHeader1".GetLocalized() +
                "\", Meters.SerialNumber as \"" + "SNHeader2".GetLocalized() +
                "\", Calibrations.Operator as \"" + "SNHeader3".GetLocalized() +
                "\", Calibrations.Program as \"" + "SNHeader4".GetLocalized() +
                "\", Meters.GSize as \"" + "SNHeader5".GetLocalized() +
                "\", Meters.MeterData1 as \"" + "SNHeader6".GetLocalized() +
                "\", Meters.MeterData5 as \"" + "SNHeader7".GetLocalized() +
                "\" from Meters inner join Calibrations on Meters.CalibrationId=Calibrations.CalibrationId where Calibrations.CalibrationDate >= {d '" + date + "'} ORDER BY Calibrations.CalibrationId DESC";


            OdbcCommand command = new OdbcCommand(query, connection);

            command.ExecuteNonQuery();
            OdbcDataAdapter adapter = new OdbcDataAdapter(command);

            //Fill Data
            adapter.Fill(data);
            dataGrid.DataContext = data;
            
            dataGrid.ItemsSource = data.DefaultView;

            adapter.Update(data);
            connection.Close();
            //Convert Data Table to ObservableCollection UWP/WinUI2 ~ WinUI3 only

            DataTableToObservableCollection.Convert(data, dataGrid, observableCollection);
        }
        catch (Exception ex)
        {
            LogFile.Write("#800003", ex.Message);
        }

    }
    public static void UTG250(DataGrid dataGrid, ObservableCollection<object> observableCollection, DataTable data)
    {
        dataGrid.ItemsSource = null;
        dataGrid.Columns.Clear();

        try
        {
            ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
            string date = configFile.Read("FilterDateUT", "FilterDates");
            OdbcConnection connection = new OdbcConnection();
            //Database Adress
            connection.ConnectionString = @"Driver={Microsoft Access Driver (*.mdb)}; Dbq=" + configFile.Read("DbUT", "Database");
            connection.Open();
            //String Query
            string query = "SELECT Calibrations.CalibrationId as \"" + "UTHeader0".GetLocalized() +
                "\", Calibrations.CalibrationDate as \"" + "UTHeader1".GetLocalized() +
                "\",  Meters.SerialNumber as \"" + "UTHeader2".GetLocalized() +
                "\", Calibrations.Operator as \"" + "UTHeader3".GetLocalized() +
                "\", Calibrations.Program as \"" + "UTHeader4".GetLocalized() +
                "\", Meters.MeterData1 as \"" + "UTHeader5".GetLocalized() +
                "\", Meters.MeterData2 as \"" + "UTHeader6".GetLocalized() +
                "\", Meters.MeterData3 as \"" + "UTHeader7".GetLocalized() +
                "\", Meters.MeterData4 as \"" + "UTHeader8".GetLocalized() +
                "\", Meters.MeterData5 as \"" + "UTHeader9".GetLocalized() +
                "\", Meters.MeterData6 as \"" + "UTHeader10".GetLocalized() +
                "\", Meters.MeterData7 as \"" + "UTHeader11".GetLocalized() +
                "\", Meters.MeterData8 as \"" + "UTHeader12".GetLocalized() +
                "\", Meters.MeterData9 as \"" + "UTHeader13".GetLocalized() +
                "\", Meters.MeterData10 as \"" + "UTHeader14".GetLocalized() +
                "\" from Calibrations inner join Meters on Calibrations.CalibrationId=Meters.CalibrationId where Calibrations.CalibrationDate >= {d '" + date + "'} ORDER BY Calibrations.CalibrationId DESC";

            OdbcCommand command = new OdbcCommand(query, connection);
            command.ExecuteNonQuery();
            OdbcDataAdapter adapter = new OdbcDataAdapter(command);

            //Fill Data
            adapter.Fill(data);
            dataGrid.DataContext = data;

            dataGrid.ItemsSource = data.DefaultView;

            adapter.Update(data);
            connection.Close();
            //Convert Data Table to ObservableCollection UWP/WinUI2 ~ WinUI3 only

            DataTableToObservableCollection.Convert(data, dataGrid, observableCollection);
        }
        catch (Exception ex)
        {
            LogFile.Write("#800004", ex.Message);
        }

    }
    public static void UM4000(DataGrid dataGrid, ObservableCollection<object> observableCollection, DataTable data)
    {
        dataGrid.ItemsSource = null;
        dataGrid.Columns.Clear();
        try
        {
            ConfigFile configFile = new ConfigFile(Globals.ConfigFilePath);
            string date = configFile.Read("FilterDateUM", "FilterDates");
            OdbcConnection connection = new OdbcConnection();
            //Database Adress
            connection.ConnectionString = @"Driver={Microsoft Access Driver (*.mdb)}; Dbq=" + configFile.Read("DbUM", "Database");
            connection.Open();

            //String Query

            string query = "SELECT Calibraciones.IdCalibración as \"" + "UMHeader0".GetLocalized() +
                   "\", Calibraciones.FechaCalibración as \"" + "UMHeader1".GetLocalized() +
                   "\", Calibraciones.NúmeroSerie as \"" + "UMHeader2".GetLocalized() +
                   "\", Calibraciones.Operador as \"" + "UMHeader3".GetLocalized() +
                   "\", Contadores.MarcaContador as \"" + "UMHeader4".GetLocalized() +
                   "\", Contadores.ModeloContador as \"" + "UMHeader5".GetLocalized() +
                   "\", Contadores.Diámetro as \"" + "UMHeader6".GetLocalized() +
                   "\", Contadores.TamañoContador as \"" + "UMHeader7".GetLocalized() +
                   "\", Contadores.PresiónNominal as \"" + "UMHeader8".GetLocalized() +
                   "\", Calibraciones.TotalizadorIni & ' m³' as \"" + "UMHeader9".GetLocalized() +
                   "\", Calibraciones.TotalizadorFin & ' m³' as \"" + "UMHeader10".GetLocalized() +
                   "\", Contadores.Qmax & ' m³/h' as \"" + "UMHeader11".GetLocalized() +
                   "\", Contadores.Qmin & ' m³/h' as \"" + "UMHeader12".GetLocalized() +
                   "\",  Calibraciones.IdLocalización as \"" + "UMHeader13".GetLocalized() +
                   "\" from Calibraciones inner join Contadores on Calibraciones.NúmeroSerie=Contadores.NúmeroSerie where Calibraciones.FechaCalibración >= {d '" + date + "'} ORDER BY Calibraciones.FechaCalibración DESC";

            OdbcCommand command = new OdbcCommand(query, connection);
            command.ExecuteNonQuery();
            OdbcDataAdapter adapter = new OdbcDataAdapter(command);

            //Fill Data
            adapter.Fill(data);
            dataGrid.DataContext = data;

            dataGrid.ItemsSource = data.DefaultView;

            adapter.Update(data);
            connection.Close();
            //Comvert Data Table to ObservableCollection UWP/WinUI2 ~ WinUI3 only

            DataTableToObservableCollection.Convert(data, dataGrid, observableCollection);
        }
        catch (Exception ex)
        {

            LogFile.Write("#800005", ex.Message);

        }

    }
}
