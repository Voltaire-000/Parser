using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static TransNew.TextFileParser;
using System.Linq;

namespace TransNew
{
    public class Program
    {
        static void Main(string[] args)
        {
            TextToJsonConverter.ConvertTextToJson("..\\..\\trans.inp", "..\\..\\tramsMos.json");
        }
    }
}
