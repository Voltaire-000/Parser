using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransNew
{
    public class Root
    {
        [JsonProperty("SpecieName")]
        public List<string> SpecieName { get; set; }

        [JsonProperty("Comments")]
        public string Comments { get; set; }

        [JsonProperty("DataRecord")]
        public List<DataRecord> DataRecord { get; set; }
    }
}
