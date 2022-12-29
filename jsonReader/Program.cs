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
            var x = thermo_element[1];
            int thermolength = thermo_element.GetArrayLength();
            var ch4 = thermo_element[7];
            Type ch4Type = ch4.GetType();
            JsonElement m_description;
            JsonElement m_chemFor;
            ch4.TryGetProperty("description", out m_description);
            ch4.TryGetProperty("chemicalFormula", out m_chemFor);
            string desc = m_description.ToString();
            JsonElement numberOfCarbonAtoms = m_chemFor.GetProperty("C");
            double c_num = numberOfCarbonAtoms.GetDouble();
            JsonElement numberHydrogen = m_chemFor.GetProperty("H");
            double h_num = numberHydrogen.GetDouble();
            JsonElement m_weight;
            ch4.TryGetProperty("molecularWeight", out m_weight);
            double moleWeight = m_weight.GetDouble();

            JsonElement m_tempRange = ch4.GetProperty("temperatureRange");






            Console.WriteLine("Hello world");


            Console.Read();
            
        }
    }
}
