using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static TransNew.TextFileParser;
using System.Linq;

namespace TransNew
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "..\\..\\trans.inp";

            List<Record> records = new List<Record>();

            //List<double[]> v_temperatureRange = new List<double[]>();
            //List<double[]> c_temperatureRange = new List<double[]>();

            //bool isViscosity = false;
            //bool isConductivity = false;
            bool hasViscosityCoefficient = false;
            bool hasConductivityCoefficients = false;
            string[] lines = File.ReadAllLines(filePath);
            var readlines = File.ReadLines(filePath);
            var filestream = File.OpenRead(filePath);
            StreamReader streamReader = new StreamReader("..\\..\\trans.inp");
            string currentLine = "";
            while (!streamReader.EndOfStream)
            {
                //currentLine = streamReader.ReadLine();
                if (currentLine.Length == 0)
                {
                    currentLine = streamReader.ReadLine();
                }
                if (currentLine.Length < 81)
                {
                    //currentLine = streamReader.ReadLine();
                    // we have a new record
                    // reinitialize the variables
                    var viscosityTempIntervals = int.Parse(currentLine[35].ToString());
                    var conductivityTemperatureIntervals = int.Parse(currentLine[37].ToString());
                    var recordLineCount = viscosityTempIntervals + conductivityTemperatureIntervals;

                    // add to new record
                    var speciesName = currentLine.Substring(0, 15).Trim();
                    var secondSpeciesName = currentLine.Substring(16, 15).Trim();
                    string[] nameString = new string[2];
                    nameString[0] = speciesName;
                    nameString[1] = secondSpeciesName;
                    List<string[]> Names = new List<string[]>();
                    Names.Add(nameString);
                    var mDataRecords = new List<DataRecord>();
                    var mRecord = new DataRecord();
                    var comments = currentLine.Substring(40).Trim();

                    // reached end of line, read new line
                    currentLine = streamReader.ReadLine();
                    List<double[]> viscosityCoeff = new List<double[]>();
                    List<double[]> conductivityCoeff = new List<double[]>();
                    var isViscosity = currentLine[1] == 'V';
                    List<double[]> v_temperatureRange = new List<double[]>();
                    if (isViscosity)
                    {

                        for (int i = 0; i < viscosityTempIntervals; i++)
                        {
                            double[] v_templist = new double[2];
                            double[] v_Coefflist = new double[4];

                            double firstTemperature = double.Parse(currentLine.Substring(2, 9));
                            double secondTemperature = double.Parse(currentLine.Substring(9, 11));

                            v_templist[0] = firstTemperature;
                            v_templist[1] = secondTemperature;
                            v_temperatureRange.Add(v_templist);

                            var firstCoeff = currentLine.Substring(20, 15);
                            string cleanCoeffline = Regex.Replace(firstCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            v_Coefflist[0] = v_Coeff;
                            var secondCoeff = currentLine.Substring(35, 15);
                            cleanCoeffline = Regex.Replace(secondCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            v_Coefflist[1] = v_Coeff;
                            var thirdCoeff = currentLine.Substring(50, 15);
                            cleanCoeffline = Regex.Replace(thirdCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            v_Coefflist[2] = v_Coeff;
                            var fourthCoeff = currentLine.Substring(65, 15);
                            cleanCoeffline = Regex.Replace(fourthCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            v_Coefflist[3] = v_Coeff;
                            viscosityCoeff.Add(v_Coefflist);

                            // reached the end of line, read new line
                            currentLine = streamReader.ReadLine();
                        }
                    }
                    var isConductivity = currentLine[1] == 'C';
                    List<double[]> c_temperatureRange = new List<double[]>();
                    if (isConductivity)
                    {

                        for (int i = 0; i < conductivityTemperatureIntervals; i++)
                        {
                            double[] c_templist = new double[2];
                            double[] c_Coefflist = new double[4];

                            double firstTemperature = double.Parse(currentLine.Substring(2, 9));
                            double secondTemperature = double.Parse(currentLine.Substring(9, 11));
                            c_templist[0] = firstTemperature;
                            c_templist[1] = secondTemperature;
                            c_temperatureRange.Add(c_templist);
                            var firstCoeff = currentLine.Substring(20, 15);
                            string cleanCoeffline = Regex.Replace(firstCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            c_Coefflist[0] = v_Coeff;
                            var secondCoeff = currentLine.Substring(35, 15);
                            cleanCoeffline = Regex.Replace(secondCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            c_Coefflist[1] = v_Coeff;
                            var thirdCoeff = currentLine.Substring(50, 15);
                            cleanCoeffline = Regex.Replace(thirdCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            c_Coefflist[2] = v_Coeff;
                            var fourthCoeff = currentLine.Substring(65, 15);
                            cleanCoeffline = Regex.Replace(fourthCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            c_Coefflist[3] = v_Coeff;
                            conductivityCoeff.Add(c_Coefflist);

                            // reached the end of line, read new line
                            currentLine = streamReader.ReadLine();
                        }
                    }
                    var viscosityprop = new ViscosityProperties
                    {
                        TemperatureRange = v_temperatureRange,
                        Coefficients = viscosityCoeff,
                    };
                    mRecord.Viscosity = new List<ViscosityProperties> { viscosityprop };
                    mDataRecords.Add(mRecord);
                    var conductivityprop = new ConductivityProperties
                    {
                        TemperatureRange = c_temperatureRange,
                        Coefficients = conductivityCoeff,
                    };
                    mRecord.Conductivity = new List<ConductivityProperties> { conductivityprop };
                    mDataRecords.Add(mRecord);

                    var record = new Record
                    {
                        SpeciesName = Names,
                        Comments = comments,
                        ViscosityRecordCount = viscosityTempIntervals,
                        ConductivityRecordCount = conductivityTemperatureIntervals,
                        DataRecords = mDataRecords,
                        ViscosityTemperatureRange = v_temperatureRange,
                        ViscosityCoefficients = viscosityCoeff,
                        ConductivityTemperatureRange = c_temperatureRange,
                        ConductivityCoefficients = conductivityCoeff,
                    };
                    records.Add(record);
                    //if (currentLine.Length >83)
                    //{
                    //    break;
                    //}
                    if (currentLine.StartsWith("XXX"))
                    {
                        //currentLine = streamReader.ReadLine();
                        //currentLine = streamReader.ReadLine();
                        //currentLine = streamReader.ReadLine();
                        //currentLine = streamReader.ReadLine();
                        //currentLine = streamReader.ReadLine();
                        //var m_length = currentLine.Length;
                        //streamReader.Close();
                        //break;
                    }

                }
            }
            streamReader.Close();
            //foreach (var item in records)
            //{
            //    var mname = item.SpeciesName;
            //    Console.WriteLine(mname);
            //}
            //var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonConvert.SerializeObject(records, Formatting.Indented);
            File.WriteAllText("TransportMod.json", jsonString);
            Console.WriteLine("hello");
            Console.ReadLine();
        }

        private static void MakeNewRecord(string line, int recordLineCount)
        {
            var speciesName = line.Substring(0, 15).Trim();
            var secondSpeciesName = line.Substring(16, 15).Trim();
            var comments = line.Substring(40).Trim();

        }

        //private static Record BeginNewRecord(string speciesName, string secondSpeciesName, string comments,
        //    List<double[]> vTemperatureRange, List<double[]> viscosityCoeff, List<double[]> cTemperatureRange, List<double[]> cCoefficients)
        //{
        //    var record = new Record
        //    {
        //        SpeciesName = speciesName,
        //        SecondSpeciesName = secondSpeciesName,
        //        Comments = comments,

        //        ViscosityTemperatureRange = vTemperatureRange,
        //        ViscosityCoefficients = viscosityCoeff,

        //        ConductivityTemperatureRange = cTemperatureRange,
        //        ConductivityCoefficients = cCoefficients,

        //    };
        //    return record;
        //}
    }
}
