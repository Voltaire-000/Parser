using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TransNew
{
    public class TextToJsonConverter
    {
        public static List<Root> ParseTexFile(string path)
        {
            //var lines = File.ReadAllLines(path);
            StreamReader streamReader = new StreamReader("..\\..\\trans.inp");
            var roots = new List<Root>();
            Root currentRoot = null;
            string currentLine = "";
            while (!streamReader.EndOfStream)
            {
                if (currentLine.Length == 0) { currentLine = streamReader.ReadLine(); }
                if (currentLine.Length < 81)
                {
                    var viscosityTempIntervals = int.Parse(currentLine[35].ToString());
                    var conductivityTemperatureIntervals = int.Parse(currentLine[37].ToString());
                    // we have a new record
                    if (currentRoot != null)
                    {
                        roots.Add(currentRoot);
                    }
                    var specieName = new List<string> { currentLine.Substring(0, 15).Trim() };
                    var comments = currentLine.Substring(40).Trim();
                    // we reached end of line, readline
                    currentLine = streamReader.ReadLine();
                    List<double> v_tempRange = new List<double>();
                    List<double> v_coefficients = new List<double>();
                    List<double> c_tempRange = new List<double>();
                    List<double> c_coefficients = new List<double>();
                    List<MeasurementData> Viscosity = new List<MeasurementData>();
                    List<MeasurementData> Conductivity = new List<MeasurementData>();
                    var type = currentLine.Substring(1, 1);
                    if (type == "V")
                    {
                        // for loop for the number of V records
                        for (int i = 0; i < viscosityTempIntervals; i++)
                        {
                            // get the temperature records for viscosity
                            double firstTemperature = double.Parse(currentLine.Substring(2, 9));
                            double secondTemperature = double.Parse(currentLine.Substring(9, 11));
                            v_tempRange.Add(firstTemperature);
                            v_tempRange.Add(secondTemperature);
                            var firstCoeff = currentLine.Substring(20, 15);
                            string cleanCoeffline = Regex.Replace(firstCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double v_1Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            //v_coefficients.Add(v_Coeff);
                            var secondCoeff = currentLine.Substring(35, 15);
                            cleanCoeffline = Regex.Replace(secondCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double v_2Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            //v_coefficients.Add(v_Coeff);
                            var thirdCoeff = currentLine.Substring(50, 15);
                            cleanCoeffline = Regex.Replace(thirdCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double v_3Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            //v_coefficients.Add(v_Coeff);
                            var fourthCoeff = currentLine.Substring(65, 15);
                            cleanCoeffline = Regex.Replace(fourthCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double v_4Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            //v_coefficients.Add(v_Coeff);
                            var measurementData = new MeasurementData
                            {
                                TemperatureRange = new List<double> { firstTemperature, secondTemperature},
                                Coefficients = new List<double> {v_1Coeff, v_2Coeff, v_3Coeff, v_4Coeff }
                            };
                            Viscosity.Add(measurementData);
                            // at end of line read new line
                            currentLine = streamReader.ReadLine();
                        }
                    }
                    type = currentLine.Substring(1, 1);
                    if (type == "C")
                    {
                        for (int i = 0; i < conductivityTemperatureIntervals; i++)
                        {
                            // get the temperature records for conductivity
                            double firstTemperature = double.Parse(currentLine.Substring(2, 9));
                            double secondTemperature = double.Parse(currentLine.Substring(9, 11));
                            c_tempRange.Add(firstTemperature);
                            c_tempRange.Add(secondTemperature);
                            var firstCoeff = currentLine.Substring(20, 15);
                            string cleanCoeffline = Regex.Replace(firstCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double c1_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            //c_coefficients.Add(c_Coeff);
                            var secondCoeff = currentLine.Substring(35, 15);
                            cleanCoeffline = Regex.Replace(secondCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double c2_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            //c_coefficients.Add(c_Coeff);
                            var thirdCoeff = currentLine.Substring(50, 15);
                            cleanCoeffline = Regex.Replace(thirdCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double c3_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            //c_coefficients.Add(c_Coeff);
                            var fourthCoeff = currentLine.Substring(65, 15);
                            cleanCoeffline = Regex.Replace(fourthCoeff, @"([Ee])\s+", "1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "1+2");
                            double c4_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            //c_coefficients.Add(c_Coeff);
                            var measurementData = new MeasurementData
                            {
                                TemperatureRange = new List<double> { firstTemperature, secondTemperature},
                                Coefficients = new List<double> { c1_Coeff, c2_Coeff, c3_Coeff, c4_Coeff}
                            };
                            Conductivity.Add(measurementData);
                            // end of line, read new line in for loop
                            currentLine = streamReader.ReadLine();
                        }

                    }

                    // now that records are read add to new root
                    currentRoot = new Root
                    {
                        SpecieName = specieName,
                        Comments = comments,
                        DataRecord = new List<DataRecord>
                            {
                                new DataRecord
                                {
                                    Viscosity = Viscosity,
                                    Conductivity = Conductivity,
                                }
                            }
                    };
                }

                if (currentRoot != null)
                {
                    roots.Add(currentRoot);
                }

            }
            return roots;
        }
        public static void ConvertTextToJson(string inputFilePath, string outputFilePath)
        {
            var data = ParseTexFile(inputFilePath);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(outputFilePath, json);
        }
    }
}
