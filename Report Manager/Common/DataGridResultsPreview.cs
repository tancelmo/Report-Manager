using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Report_Manager.Helpers;

namespace Report_Manager.Common;
internal class DataGridResultsPreview
{
    // Number of Results
    public static int resultsCount = 0;
    public static double wmeResult = 0;
    public static double qMax = 0;

    public static int position;

    public static void ShowPreviewUT(DataGrid dataGrid, ToggleSwitch toggleSwitch, TeachingTip teachingTip, Grid grid)
    {
        Globals.ErrorDB = null;
        try
        {
            if (toggleSwitch.IsOn && dataGrid.SelectedItem != null)
            {

                //Selected Id in datagrid
                string selectedItem = ((TextBlock)dataGrid.Columns[0].GetCellContent(dataGrid.SelectedItem)).Text;




                ConfigFile configFile = new(Globals.ConfigFilePath);
                ConnectionODBC connection = new(Globals.DbAdressUT);
                connection.Connect();

                // Catch Qmax
                try
                {
                    OdbcCommand cmd = new OdbcCommand();
                    string query = "SELECT * FROM Calibrations where CalibrationId=" + selectedItem;
                    cmd.CommandText = query;
                    cmd.Connection = connection.Connect();
                    OdbcDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        qMax = Convert.ToInt32(reader["Qmax"]);

                    }

                }

                catch (Exception ex)
                {
                    LogFile.Write("#8000067", ex.Message);
                }

                // Catch number of results
                try
                {
                    OdbcCommand cmd = new OdbcCommand();
                    string query = "SELECT count(*) FROM Tests where CalibrationId=" + selectedItem;
                    cmd.CommandText = query;
                    cmd.Connection = connection.Connect();
                    OdbcDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        resultsCount = Convert.ToInt32(reader[0]);

                    }

                }

                catch (Exception ex)
                {
                    LogFile.Write("#8000068", ex.Message);
                }

                //--------Clear Textblocks
                grid.Children.Clear();
                grid.RowDefinitions.Clear();
                //------------------------



                try
                {
                    int count = 1;
                    int empcount = 0;
                    double sum = 0;



                    List<double> error = new List<double>();
                    List<double> QTeoriacal = new List<double>();

                    while (count <= resultsCount)
                    {
                        OdbcCommand cmd = new OdbcCommand();
                        string query = "SELECT * FROM Tests where CalibrationId=" + selectedItem + " AND PointNum=" + count;
                        cmd.CommandText = query;
                        cmd.Connection = connection.Connect();
                        OdbcDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            // WME Values for calc
                            error.Add(Convert.ToDouble(reader["MeterErrorWithGears"]));

                            if ((Convert.ToDouble(reader["ReqFlowRate"]) / qMax) * 100 == 100)
                            {
                                QTeoriacal.Add(.4);
                            }
                            else
                            {
                                QTeoriacal.Add(((Convert.ToDouble(reader["ReqFlowRate"]) / qMax) * 100) / 100);
                            }
                            if (count == resultsCount)
                            {
                                for (int i = 0; i < count; i++)
                                {
                                    sum += QTeoriacal[i] * error[i];
                                }
                                wmeResult = sum / QTeoriacal.Sum();

                            }
                            //----------------------------------------------

                            Debug.WriteLine(error[empcount].ToString(), QTeoriacal[empcount].ToString());

                            string result = string.Format("{0:0.00}", (Convert.ToDouble(reader["ReqFlowRate"]) / qMax) * 100);
                            string result2 = string.Format("{0:0.00}", reader["RealFlowRate"]);
                            string result3 = string.Format("{0:0.00}", reader["MeterErrorWithGears"]) + " %";
                            //TODO tolerancia
                            string result4 = "± " + "∞" + " %";


                            CreateResultFields(grid, result, result2, result3, result4, count);

                        }

                        empcount++;
                        count++;

                    }
                    Debug.WriteLine("WME: " + wmeResult.ToString());
                    teachingTip.IsOpen = true;
                    connection.Disconnect();

                }

                catch (Exception ex)
                {
                    LogFile.Write("#8000069", ex.Message);
                }

                try
                {
                    OdbcCommand cmd = new OdbcCommand();
                    string query = "SELECT * FROM Meters where CalibrationId=" + selectedItem;
                    cmd.CommandText = query;
                    cmd.Connection = connection.Connect();
                    OdbcDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string meterK0 = reader["PulseWeight2"].ToString();
                        if (meterK0.Length == 17)
                        {
                            meterK0 = meterK0.Remove(meterK0.Length - 10);
                        }

                        if (meterK0.Length == 18)
                        {
                            meterK0 = meterK0.Remove(meterK0.Length - 11);
                        }
                        string meterNS = "MeterId".GetLocalized() + " " + selectedItem + ": " + Convert.ToString(reader["SerialNumber"]);
                        string meterK = "KHeader".GetLocalized() + "  " + meterK0;
                        string notes = reader["Remarks"].ToString();
                        string wme = "WMEHeader".GetLocalized() + " " + string.Format("{0:0.00}", wmeResult) + " %";
                        CreateInfoFields(grid, meterNS, meterK, wme, notes);


                    }

                    connection.Disconnect();
                }

                catch (Exception ex)
                {
                    LogFile.Write("#8000070", ex.Message);
                }
            }
        }catch(Exception ex)
        {


            Globals.ErrorDB = "ErrorDB".GetLocalized();
            LogFile.Write("#8000170", ex.Message);
        }
            
    }

    public static void ShowPreviewUM(DataGrid dataGrid, ToggleSwitch toggleSwitch, TeachingTip teachingTip, Grid grid)
    {

        if (toggleSwitch.IsOn && dataGrid.SelectedItem != null)
        {
            //Selected Id in datagrid
            string selectedItem = ((TextBlock)dataGrid.Columns[0].GetCellContent(dataGrid.SelectedItem)).Text;




            ConfigFile configFile = new(Globals.ConfigFilePath);
            ConnectionODBC connection = new(Globals.DbAdressUM);
            connection.Connect();


            // Catch number of results
            try
            {
                OdbcCommand cmd = new OdbcCommand();
                string query = "SELECT count(*) FROM EnsayosCaudal where IdCalibración=" + "'" + selectedItem + "'";
                cmd.CommandText = query;
                cmd.Connection = connection.Connect();
                OdbcDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    resultsCount = Convert.ToInt32(reader[0]);

                }

            }

            catch (Exception ex)
            {
                LogFile.Write("#8000015", ex.Message);
            }

            //--------Clear Textblocks
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            //------------------------



            try
            {
                int count = 1;
                int empcount = 0;
                double sum = 0;

                List<double> error = new List<double>();
                List<double> QTeoriacal = new List<double>();

                while (count <= resultsCount)
                {
                    OdbcCommand cmd = new OdbcCommand();
                    string query = "SELECT * FROM EnsayosCaudal where IdCalibración=" + "'" + selectedItem + "'" + "AND NumPunto=" + count;
                    cmd.CommandText = query;
                    cmd.Connection = connection.Connect();
                    OdbcDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // WME Values for calc
                        error.Add(Convert.ToDouble(reader["ErrorCorregido"]));

                        if (Convert.ToInt32(reader["QPerc"]) == 100)
                        {
                            QTeoriacal.Add(.4);
                        }
                        else
                        {
                            QTeoriacal.Add(Convert.ToDouble(reader["QPerc"]) / 100);
                        }
                        if (count == resultsCount)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                sum += QTeoriacal[i] * error[i];
                            }
                            wmeResult = sum / QTeoriacal.Sum();

                        }
                        //----------------------------------------------

                        Debug.WriteLine(error[empcount].ToString(), QTeoriacal[empcount].ToString());
                        Debug.WriteLine("WME:", wmeResult.ToString());

                        string result = string.Format("{0:0.00}", reader["QPerc"]);
                        string result2 = string.Format("{0:0.00}", reader["QReal"]);
                        string result3 = string.Format("{0:0.00}", reader["ErrorCorregido"]) + " %";
                        string result4 = "± " + reader["Tolerancia"].ToString() + " %";


                        CreateResultFields(grid, result, result2, result3, result4, count);

                    }
                    empcount++;
                    count++;

                }

                teachingTip.IsOpen = true;
                connection.Disconnect();

            }

            catch (Exception ex)
            {
                LogFile.Write("#8000025", ex.Message);
            }

            try
            {
                OdbcCommand cmd = new OdbcCommand();
                string query = "SELECT * FROM Calibraciones where IdCalibración=" + "'" + selectedItem + "'";
                cmd.CommandText = query;
                cmd.Connection = connection.Connect();
                OdbcDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string meterK0 = reader["ConstanteK"].ToString();
                    if (meterK0.Length == 17)
                    {
                        meterK0 = meterK0.Remove(meterK0.Length - 10);
                    }

                    if (meterK0.Length == 18)
                    {
                        meterK0 = meterK0.Remove(meterK0.Length - 11);
                    }
                    string meterNS = "MeterId".GetLocalized() + " " + selectedItem + ": " + Convert.ToString(reader["NúmeroSerie"]);
                    string meterK = "KHeader".GetLocalized() + "  " + meterK0;
                    string notes = reader["Notas"].ToString();
                    string wme = "WMEHeader".GetLocalized() + " " + string.Format("{0:0.00}", wmeResult) + " %";
                    CreateInfoFields(grid, meterNS, meterK, wme, notes);


                }

                connection.Disconnect();
            }

            catch (Exception ex)
            {
                LogFile.Write("#8000015", ex.Message);
            }
        }


    }

    public static void ShowPreviewSN(DataGrid dataGrid, ToggleSwitch toggleSwitch, TeachingTip teachingTip, Grid grid)
    {

        if (toggleSwitch.IsOn && dataGrid.SelectedItem != null)
        {
            //Selected Id in datagrid
            string selectedItem = ((TextBlock)dataGrid.Columns[0].GetCellContent(dataGrid.SelectedItem)).Text;
            string selectedItemSN = ((TextBlock)dataGrid.Columns[2].GetCellContent(dataGrid.SelectedItem)).Text;
            Console.WriteLine("ID: " + selectedItem + " - SN: " + "'" + selectedItemSN);



            ConfigFile configFile = new(Globals.ConfigFilePath);
            ConnectionODBC connection = new(Globals.DbAdressSN);
            connection.Connect();

            // Catch Qmax an position
            try
            {
                OdbcCommand cmd = new OdbcCommand();
                string query = "SELECT * FROM Meters where CalibrationId=" + selectedItem + " AND SerialNumber=" + "'" + selectedItemSN + "'";
                cmd.CommandText = query;
                cmd.Connection = connection.Connect();
                OdbcDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    qMax = Convert.ToDouble(reader["Qmax"]);
                    //position
                    position = Convert.ToInt32(reader["Position"]);

                }

            }

            catch (Exception ex)
            {
                LogFile.Write("#8000067", ex.Message);
            }

            // Catch number of results
            try
            {
                OdbcCommand cmd = new OdbcCommand();
                string query = "SELECT count(*) FROM Tests where CalibrationId=" + selectedItem + " AND Position=" + position;
                cmd.CommandText = query;
                cmd.Connection = connection.Connect();
                OdbcDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    resultsCount = Convert.ToInt32(reader[0]);

                }

            }

            catch (Exception ex)
            {
                LogFile.Write("#8000068", ex.Message);
            }

            //--------Clear Textblocks
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            //------------------------



            try
            {
                int count = 1;
                int empcount = 0;
                double sum = 0;

                List<double> error = new List<double>();
                List<double> QTeoriacal = new List<double>();

                while (count <= resultsCount)
                {
                    OdbcCommand cmd = new OdbcCommand();
                    string query = "SELECT * FROM Tests where CalibrationId=" + selectedItem + " AND PointNum=" + count + " AND Position=" + position;
                    cmd.CommandText = query;
                    cmd.Connection = connection.Connect();
                    OdbcDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // WME Values for calc
                        error.Add(Convert.ToDouble(reader["MeterErrorWithGears"]));

                        if ((Convert.ToDouble(reader["ReqFlowRate"]) / qMax) * 100 == 100)
                        {
                            QTeoriacal.Add(.4);
                        }
                        else
                        {
                            QTeoriacal.Add(((Convert.ToDouble(reader["ReqFlowRate"]) / qMax) * 100) / 100);
                        }
                        if (count == resultsCount)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                sum += QTeoriacal[i] * error[i];
                            }
                            wmeResult = sum / QTeoriacal.Sum();
                            Debug.WriteLine("WME:", wmeResult.ToString());
                        }
                        //----------------------------------------------

                        Debug.WriteLine(error[empcount].ToString(), QTeoriacal[empcount].ToString());

                        string result = string.Format("{0:0.00}", (Convert.ToDouble(reader["ReqFlowRate"]) / qMax) * 100);
                        string result2 = string.Format("{0:0.000}", reader["RealFlowRate"]);
                        string result3 = string.Format("{0:0.00}", reader["MeterErrorWithGears"]) + " %";
                        //TODO tolerancia
                        string result4 = "± " + "∞" + " %";


                        CreateResultFields(grid, result, result2, result3, result4, count);
                        Debug.WriteLine("Result count Total: " + resultsCount);
                        Debug.WriteLine("Result count atual: " + count);
                    }
                    empcount++;
                    count++;

                }


                connection.Disconnect();

            }

            catch (Exception ex)
            {
                LogFile.Write("#8000069", ex.Message);
            }

            try
            {
                Debug.WriteLine("Qmax: " + qMax);
                OdbcCommand cmd = new OdbcCommand();
                //string query = "SELECT * FROM Tests where CalibrationId=" + selectedItem + " AND Position=" + position + " AND ReqFlowRate=" + "'" + Convert.ToString(qMax, CultureInfo.InvariantCulture) + "'";//, CultureInfo.InvariantCulture;
                string query = "SELECT * FROM Tests where CalibrationId=" + selectedItem + " AND Position=" + position + " AND PointNum=1";

                cmd.CommandText = query;
                cmd.Connection = connection.Connect();
                OdbcDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string meterK0 = string.Format("{0:0.00}", reader["MeterPLoss"]);

                    string meterNS = "MeterId".GetLocalized() + " " + selectedItem + ": " + selectedItemSN;
                    string meterK = "Qmax PLoss (mbar): " + meterK0;
                    string notes = "";
                    string wme = "WMEHeader".GetLocalized() + " " + string.Format("{0:0.00}", wmeResult) + " %";
                    CreateInfoFields(grid, meterNS, meterK, wme, notes);


                }

                connection.Disconnect();
            }

            catch (Exception ex)
            {
                LogFile.Write("#8000070", ex.Message);
            }

            teachingTip.IsOpen = true;

        }


    }

    private static void CreateResultFields(Grid grid, string result, string result2, string result3, string result4, int counter)
    {


        RowDefinition rowDef1 = new RowDefinition();
        rowDef1.Height = GridLength.Auto;

        grid.RowDefinitions.Add(rowDef1);

        //Q Percent
        TextBlock textBlock = new TextBlock();
        textBlock.Text = result;
        textBlock.IsTextSelectionEnabled = true;
        textBlock.Margin = new Thickness(0, 0, 10, 0);
        Grid.SetColumn(textBlock, 0);
        Grid.SetRow(textBlock, counter + 4);
        grid.Children.Add(textBlock);

        //Q Real
        TextBlock textBlock2 = new TextBlock();
        textBlock2.Text = result2;
        textBlock2.IsTextSelectionEnabled = true;
        textBlock2.Margin = new Thickness(0, 0, 10, 0);
        Grid.SetColumn(textBlock2, 1);
        Grid.SetRow(textBlock2, counter + 4);
        grid.Children.Add(textBlock2);

        //Errors
        TextBlock textBlock3 = new TextBlock();
        textBlock3.Text = result3;
        textBlock3.IsTextSelectionEnabled = true;
        textBlock3.Padding = new Thickness(0, 0, 10, 0);
        textBlock3.HorizontalTextAlignment = TextAlignment.Right;
        Grid.SetColumn(textBlock3, 2);
        Grid.SetRow(textBlock3, counter + 4);
        grid.Children.Add(textBlock3);

        //Tolerancia
        TextBlock textBlock4 = new TextBlock();
        textBlock4.Text = result4;
        textBlock4.IsTextSelectionEnabled = true;
        textBlock4.Padding = new Thickness(0, 0, 20, 0);
        textBlock4.HorizontalTextAlignment = TextAlignment.Right;
        Grid.SetColumn(textBlock4, 3);
        Grid.SetRow(textBlock4, counter + 4);
        grid.Children.Add(textBlock4);




    }

    private static void CreateInfoFields(Grid grid, string meterSN, string meterK, string wme, string notes)
    {


        RowDefinition rowDef1 = new RowDefinition();
        RowDefinition rowDef2 = new RowDefinition();
        RowDefinition rowDef3 = new RowDefinition();
        RowDefinition rowDef4 = new RowDefinition();
        RowDefinition rowDef5 = new RowDefinition();
        rowDef1.Height = GridLength.Auto;
        rowDef2.Height = GridLength.Auto;
        rowDef3.Height = GridLength.Auto;
        rowDef4.Height = GridLength.Auto;
        rowDef5.Height = GridLength.Auto;
        grid.RowDefinitions.Add(rowDef1);
        grid.RowDefinitions.Add(rowDef2);
        grid.RowDefinitions.Add(rowDef3);
        grid.RowDefinitions.Add(rowDef4);
        grid.RowDefinitions.Add(rowDef5);



        //Sn and ID
        TextBlock textBlockSn = new TextBlock();
        textBlockSn.Text = meterSN;
        textBlockSn.IsTextSelectionEnabled = true;
        Grid.SetColumn(textBlockSn, 0);
        Grid.SetColumnSpan(textBlockSn, 4);
        Grid.SetRow(textBlockSn, 0);
        grid.Children.Add(textBlockSn);

        //MeterK / PLoss
        TextBlock textBlockK = new TextBlock();
        textBlockK.Text = meterK;
        textBlockK.IsTextSelectionEnabled = true;
        Grid.SetColumn(textBlockK, 0);
        Grid.SetColumnSpan(textBlockK, 4);
        Grid.SetRow(textBlockK, 1);
        grid.Children.Add(textBlockK);

        //WME
        TextBlock textBlockWME = new TextBlock();
        textBlockWME.Text = wme;
        textBlockWME.Margin = new Thickness(0, 0, 0, 10);
        textBlockWME.IsTextSelectionEnabled = true;
        Grid.SetColumn(textBlockWME, 0);
        Grid.SetColumnSpan(textBlockWME, 4);
        Grid.SetRow(textBlockWME, 2);
        grid.Children.Add(textBlockWME);

        //textBlockResultsHeader
        TextBlock textBlockResultsHeader = new TextBlock();
        textBlockResultsHeader.Text = "ResultsHeader".GetLocalized();
        textBlockResultsHeader.Margin = new Thickness(0, 0, 0, 10);
        Grid.SetColumn(textBlockResultsHeader, 0);
        Grid.SetRow(textBlockResultsHeader, 3);
        Grid.SetColumnSpan(textBlockResultsHeader, 2);
        grid.Children.Add(textBlockResultsHeader);

        //textBlockNotesHeader
        TextBlock textBlockNotesHeader = new TextBlock();
        textBlockNotesHeader.Text = "NotesHeader".GetLocalized();
        textBlockNotesHeader.Margin = new Thickness(10, 0, 0, 10);
        Grid.SetColumn(textBlockNotesHeader, 4);
        Grid.SetRow(textBlockNotesHeader, 3);
        grid.Children.Add(textBlockNotesHeader);

        //textBlockNotes
        TextBlock textBlockNotes = new TextBlock();
        textBlockNotes.IsTextSelectionEnabled = true;
        textBlockNotes.Text = notes;
        textBlockNotes.Margin = new Thickness(10, 0, 0, 0);
        textBlockNotes.TextWrapping = TextWrapping.Wrap;
        textBlockNotes.VerticalAlignment = VerticalAlignment.Top;
        textBlockNotes.TextAlignment = TextAlignment.Start;
        textBlockNotes.Width = 350;
        textBlockNotes.Height = 150;
        Grid.SetColumn(textBlockNotes, 4);
        Grid.SetRow(textBlockNotes, 4);
        Grid.SetColumnSpan(textBlockNotes, 1);
        Grid.SetRowSpan(textBlockNotes, 14);
        grid.Children.Add(textBlockNotes);

        //Q%Header
        TextBlock QPercHeader = new TextBlock();
        QPercHeader.FontWeight = FontWeights.SemiBold;
        QPercHeader.Text = "Q %";
        QPercHeader.Margin = new Thickness(0, 0, 0, 5);
        Grid.SetColumn(QPercHeader, 0);
        Grid.SetRow(QPercHeader, 4);
        grid.Children.Add(QPercHeader);

        //QReal
        TextBlock QRealHeader = new TextBlock();
        QRealHeader.FontWeight = FontWeights.SemiBold;
        QRealHeader.Text = "Q Real";
        QRealHeader.Margin = new Thickness(0, 0, 0, 5);
        Grid.SetColumn(QRealHeader, 1);
        Grid.SetRow(QRealHeader, 4);
        grid.Children.Add(QRealHeader);

        //Errors Header
        TextBlock QError = new TextBlock();
        QError.FontWeight = FontWeights.SemiBold;
        QError.Text = "  " + "ErrorHeader".GetLocalized(); ;
        QError.Margin = new Thickness(0, 0, 0, 5);
        Grid.SetColumn(QError, 2);
        Grid.SetRow(QError, 4);
        grid.Children.Add(QError);

        // Tolerance
        TextBlock QTolerance = new TextBlock();
        QTolerance.FontWeight = FontWeights.SemiBold;
        QTolerance.Text = "ToleranceHeader".GetLocalized();
        QTolerance.Margin = new Thickness(0, 0, 0, 5);
        Grid.SetColumn(QTolerance, 3);
        Grid.SetRow(QTolerance, 4);
        grid.Children.Add(QTolerance);
    }
}
