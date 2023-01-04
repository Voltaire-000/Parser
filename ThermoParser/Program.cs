using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThermoParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StreamReader streamReader = new StreamReader("..\\..\\thermo.inp");
            StreamWriter streamWriter = new StreamWriter("..\\..\\thermoINP.json");
        }
    }
}
