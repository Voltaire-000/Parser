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
            List<string> record = new List<string>();
            List<double[]> temperatureRange = new List<double[]>();
            List<double[]> viscosityCoeff = new List<double[]>();
            try
            {
                bool hasViscosityCoefficient = false;
                bool hasConductivityCoefficients = false;
                int viscosityTempIntervals = 0;
                int conductivityTemperatureIntervals = 0;
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (line.Length < 80)
                    {
                        string speciesName = line.Substring(0, 15).Trim();
                        string secondSpeciesName = line.Substring(16, 15).Trim();
                        hasViscosityCoefficient = line[34] == 'V';
                        viscosityTempIntervals = int.Parse(line[35].ToString());
                        hasConductivityCoefficients = line[36] == 'C';
                        conductivityTemperatureIntervals = int.Parse(line[37].ToString());
                        string comments = line.Substring(40).Trim();

                        record.Add(speciesName + " ," + secondSpeciesName + " " + comments);
                    }
                    else
                    {
                        string coefficientLine = line.Substring(0, 2);
                        bool isViscosity = coefficientLine[1] == 'V';
                        // get the temperature
                        if (isViscosity)
                        {
                            double firstTemperature = double.Parse(line.Substring(2, 9));
                            double secondTemperature = double.Parse(line.Substring(9, 11));
                            double[] templist = new double[2];
                            templist[0] = firstTemperature;
                            templist[1] = secondTemperature;
                            temperatureRange.Add(templist);
                            var firstCoeff = line.Substring(21, 14);
                            string cleanCoeffline = Regex.Replace(firstCoeff, @"([Ee])\s+", "$1");
                            cleanCoeffline = Regex.Replace(cleanCoeffline, @"([Ee])(?!\+|-)(\d)", "$1+$2");
                            double v_Coeff = double.Parse(cleanCoeffline, NumberStyles.Float, CultureInfo.InvariantCulture);
                            double[] vCoeffList = new double[4];
                            vCoeffList[0] = v_Coeff;

                        }
                    }
       
                }
                Console.WriteLine($"first line : { record}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured : {ex.Message}");
            }



            

        }
    }
}
