using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransNew
{
    public class DataRecord
    {
        public List<ViscosityProperties> Viscosity { get; set; }
        public List<ConductivityProperties> Conductivity { get; set; }
    }
}
