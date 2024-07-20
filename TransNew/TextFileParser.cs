using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace TransNew
{
    public class TextFileParser
    {
        //    public class Record
        //    {
        //        public string SpeciesName { get; set; }
        //        public string SecondSpeciesName { get; set; }
        //        public bool HasViscosityCoefficient { get; set; }
        //        public int ViscosityTemperatureIntervals { get; set; }
        //        public bool HasThermalConductivityCoefficients { get; set; }
        //        public int ConductivityTemperatureIntervals { get; set; }
        //        public string Comments { get; set; }
        //        public bool IsViscosity { get; set; }
        //        public bool IsThermalConductivity { get; set; }
        //        public double FirstTemperature { get; set; }
        //        public double LastTemperature { get; set; }
        //        public double[] Coefficients { get; set; }
        //    }

        //    public static Record ParseRecord(string line)
        //    {
        //        var speciesName = "";
        //        var secondSpeciesName = "";
        //        var hasViscosityCoefficient = false;
        //        var viscosityTempIntervals = 0;
        //        var hasConductivityCoefficients = false;
        //        var conductivityTemperatureIntervals = 0;       
        //        var comments = "";


        //        if (line.Length < 80)
        //        {
        //            //throw new ArgumentException(" Line to short to contain all required fields");
        //            speciesName = line.Substring(0, 15).Trim();
        //            secondSpeciesName = line.Substring(16, 15).Trim();
        //            hasViscosityCoefficient = line[34] == 'V';
        //            viscosityTempIntervals = int.Parse(line[35].ToString());
        //            hasConductivityCoefficients = line[36] == 'C';
        //            conductivityTemperatureIntervals = int.Parse(line[37].ToString());
        //            comments = line.Substring(40).Trim();
        //        }
        //        else
        //        {
        //            string coefficientLine = line.Substring(80);
        //        }

        //        var record = new Record
        //        {
        //            SpeciesName = speciesName,
        //            SecondSpeciesName = secondSpeciesName,
        //            HasViscosityCoefficient = hasViscosityCoefficient,
        //            ViscosityTemperatureIntervals = viscosityTempIntervals,
        //            HasThermalConductivityCoefficients = hasConductivityCoefficients,
        //            ConductivityTemperatureIntervals = conductivityTemperatureIntervals,
        //            Comments = comments,
        //            IsViscosity = hasViscosityCoefficient,
        //            FirstTemperature = 

        //        };
        //            if (line.Length >= 80)
        //            {
        //                string coefficientLine = line.Substring(80);
        //                record.IsViscosity = coefficientLine[1] == 'V';
        //            }
        //        }


        //        // parse coefficien line
        //        if (line.Length >= 80)
        //        {
        //            string coefficientLine = line.Substring(80);
        //            record.IsViscosity = coefficientLine[1] == 'V';
        //            record.IsThermalConductivity = coefficientLine[1] == 'C';
        //            record.FirstTemperature = double.Parse(coefficientLine.Substring(2, 9));
        //            record.LastTemperature = double.Parse(coefficientLine.Substring(11, 9));
        //            record.Coefficients = new double[4];
        //            for (int i = 0; i < 4; i++)
        //            {
        //                record.Coefficients[i] = double.Parse(coefficientLine.Substring(20 + i * 15, 15));
        //            }
        //        }
        //        return record;
        //    }
        //}
    }
}
