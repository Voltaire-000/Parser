using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static TransNew.TextFileParser;

namespace TransNew
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "..\\..\\trans.inp";
            //List<string> record = new List<string>();
            List<Record> records = new List<Record>();
            string speciesName = "";
            string secondSpeciesName = "";
            string comments = "";
            List<double[]> v_temperatureRange = new List<double[]>();
            List<double[]> c_temperatureRange = new List<double[]>();
            
            

            int recordLineCount = 0;
            bool isViscosity = false;
            bool isConductivity = false;
            bool hasViscosityCoefficient = false;
            bool hasConductivityCoefficients = false;
            int viscosityTempIntervals = 0;
            int conductivityTemperatureIntervals = 0;
            string[] lines = File.ReadAllLines(filePath);
            var readlines = File.ReadLines(filePath);
            var filestream = File.OpenRead(filePath);
            StreamReader streamReader = new StreamReader("..\\..\\trans.inp");
            string currentLine = "";
            while (!streamReader.EndOfStream)
            {
                currentLine = streamReader.ReadLine();
                if (currentLine.Length < 80)
                {
                    viscosityTempIntervals = int.Parse(currentLine[35].ToString());
                    conductivityTemperatureIntervals = int.Parse(currentLine[37].ToString());
                    recordLineCount = viscosityTempIntervals + conductivityTemperatureIntervals;
                    speciesName = currentLine.Substring(0, 15).Trim();
                    secondSpeciesName = currentLine.Substring(16, 15).Trim();
                    comments = currentLine.Substring(40).Trim();
                }
                if (currentLine.Length >= 80)
                {
                    List<double[]> viscosityCoeff = new List<double[]>();
                    List<double[]> conductivityCoeff = new List<double[]>();
                    for (int i = 0; i < recordLineCount; i++)
                    {
                        double[] v_Coefflist = new double[4];
                        double[] c_Coefflist = new double[4];

                        string coefficientLine = currentLine.Substring(0, 2);
                        isViscosity = coefficientLine[1] == 'V';
                        isConductivity = coefficientLine[1] == 'C';
                        if (isViscosity)
                        {
                            double[] v_templist = new double[2];
                            
                            double firstTemperature = double.Parse(currentLine.Substring(2, 9));
                            double secondTemperature = double.Parse(currentLine.Substring(9, 11));

                            v_templist[0] = firstTemperature;
                            v_templist[1] = secondTemperature;
                            v_temperatureRange.Add(v_templist);
                            var firstCoeff = currentLine.Substring(20, 15);
                            string cleanCoeffline = Regex.Replace(firstCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            double v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            v_Coefflist[0] = v_Coeff;
                            var secondCoeff = currentLine.Substring(35, 15);
                            cleanCoeffline = Regex.Replace(secondCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            v_Coefflist[1] = v_Coeff;
                            var thirdCoeff = currentLine.Substring(50, 15);
                            cleanCoeffline = Regex.Replace(thirdCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            v_Coefflist[2] = v_Coeff;
                            var fourthCoeff = currentLine.Substring(65, 15);
                            cleanCoeffline = Regex.Replace(fourthCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            v_Coefflist[3] = v_Coeff;
                            viscosityCoeff.Add(v_Coefflist);
                            
                        }
                        if (isConductivity)
                        {
                            double[] c_templist = new double[2];
                            //double[] c_Coefflist = new double[4];
                            double firstTemperature = double.Parse(currentLine.Substring(2, 9));
                            double secondTemperature = double.Parse(currentLine.Substring(9, 11));
                            c_templist[0] = firstTemperature;
                            c_templist[1] = secondTemperature;
                            c_temperatureRange.Add(c_templist);
                            var firstCoeff = currentLine.Substring(20, 15);
                            string cleanCoeffline = Regex.Replace(firstCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            double v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            c_Coefflist[0] = v_Coeff;
                            var secondCoeff = currentLine.Substring(35, 15);
                            cleanCoeffline = Regex.Replace(secondCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            c_Coefflist[1] = v_Coeff;
                            var thirdCoeff = currentLine.Substring(50, 15);
                            cleanCoeffline = Regex.Replace(thirdCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            c_Coefflist[2] = v_Coeff;
                            var fourthCoeff = currentLine.Substring(65, 15);
                            cleanCoeffline = Regex.Replace(fourthCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            c_Coefflist[3] = v_Coeff;
                            conductivityCoeff.Add(c_Coefflist);
                        }
                        // make new record here
                        
                        //currentLine = streamReader.ReadLine();
                    }
                    var record = BeginNewRecord(speciesName, secondSpeciesName, comments, v_temperatureRange, viscosityCoeff, c_temperatureRange, conductivityCoeff);
                    records.Add(record);
                    //currentLine = streamReader.ReadLine();
                }
                currentLine = streamReader.ReadLine();
            }

        }

        private static void MakeNewRecord(string line, int recordLineCount)
        {
            var speciesName = line.Substring(0, 15).Trim();
            var secondSpeciesName = line.Substring(16, 15).Trim();
            var comments = line.Substring(40).Trim();
            
        }

        private static Record BeginNewRecord(string speciesName, string secondSpeciesName, string comments,
            List<double[]> vTemperatureRange, List<double[]> viscosityCoeff, List<double[]> cTemperatureRange, List<double[]> cCoefficients)
        {
            var record = new Record
            {
                SpeciesName = speciesName,
                SecondSpeciesName = secondSpeciesName,
                Comments = comments,

                ViscosityTemperatureRange = vTemperatureRange,
                ViscosityCoefficients = viscosityCoeff,

                ConductivityTemperatureRange = cTemperatureRange,
                ConductivityCoefficients = cCoefficients,

            };
            return record;
        }
    }
}
