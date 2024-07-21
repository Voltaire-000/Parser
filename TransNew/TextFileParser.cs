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
        public class Record
        {
            public List<string[]> SpeciesName { get; set; }
            public string Comments { get; set; }
            public int ViscosityRecordCount { get; set; }
            public int ConductivityRecordCount { get; set; }
            public List<DataRecord> DataRecords { get; set; }
            //public bool HasViscosityCoefficient { get; set; }
            //public int ViscosityTemperatureIntervals { get; set; }
            public List<double[]> ViscosityTemperatureRange { get; set; }
            public List<double[]> ViscosityCoefficients { get; set; }
            public List<double[]> ConductivityTemperatureRange { get; set; }
            public List<double[]> ConductivityCoefficients { get; set; }
            //public bool HasThermalConductivityCoefficients { get; set; }
            //public int ConductivityTemperatureIntervals { get; set; }
            
            //public bool IsViscosity { get; set; }
            //public bool IsThermalConductivity { get; set; }
            //public double FirstTemperature { get; set; }
            //public double LastTemperature { get; set; }
            //public double[] Coefficients { get; set; }
        }
    }
}
