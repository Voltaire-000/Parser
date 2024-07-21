using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransNew
{
    public class DataRecord
    {
        [JsonProperty("Viscosity")]
        public List<MeasurementData> Viscosity { get; set; }

        [JsonProperty("Conductivity")]
        public List<MeasurementData> Conductivity { get; set; }
    }
}
