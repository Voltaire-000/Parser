using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace jsonReader
{
    public class Root
    {
        public List<Thermo> thermo { get; set; }

    }
    public class Thermo
    {
        public string reactant { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            
            //string file = "..\\..\\thermoINPjson.json";
            string file = "..\\..\\shortThermo.json";
            string jsonString = File.ReadAllText(file);
            JsonDocument jsonDocument;
            jsonDocument = JsonDocument.Parse(jsonString);
            JsonElement rootElement =  jsonDocument.RootElement;
            JsonElement thermo_element =  rootElement.GetProperty("thermo");
.
            //JsonElement r_element = thermo_element.GetProperty("reactant");





            Console.WriteLine("Hello world");
            
        }
    }
}
