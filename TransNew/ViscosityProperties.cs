using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransNew
{
    public class MeasurementData
    {
        [JsonProperty("Temperature")]
        public List<double> TemperatureRange { get; set; }

        [JsonProperty("Coeff")]
        public List<double> Coefficients { get; set; }
    }
}
