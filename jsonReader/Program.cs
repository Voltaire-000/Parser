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

            string file = "..\\..\\thermoINPjson.json";
            //string file = "..\\..\\shortThermo.json";
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
            ch4.TryGetProperty("description", out m_description);
            string desc = m_description.ToString();

            JsonElement m_chemFor;
            ch4.TryGetProperty("chemicalFormula", out m_chemFor);
            

            //JsonElement numberOfCarbonAtoms = m_chemFor.GetProperty("C");
            //double c_num = numberOfCarbonAtoms.GetDouble();

            //JsonElement numberHydrogen = m_chemFor.GetProperty("H");
            //double h_num = numberHydrogen.GetDouble();

            JsonElement m_weight;
            ch4.TryGetProperty("molecularWeight", out m_weight);
            double moleWeight = m_weight.GetDouble();

            JsonElement m_tempRange = ch4.GetProperty("temperatureRange");
            JsonElement m_range1 = m_tempRange.GetProperty("range_1");
            JsonElement m_temp = m_range1.GetProperty("temperatureRange");
            int m_num = m_temp.GetArrayLength();
            double lowTemp = m_temp[0].GetDouble();

            JsonElement.ArrayEnumerator m_reactants = thermo_element.EnumerateArray();
            for (int i = 0; i < thermolength; i++)
            {
                m_reactants.MoveNext();
                JsonElement m_current =  m_reactants.Current;
                JsonElement m_name;
                bool m_foundReactant = m_current.TryGetProperty("reactant", out m_name);
                string elename = m_name.GetString();

                if (elename == "RP-1")
                {
                    int sum = 99;
                }
            }
            


            Console.WriteLine("Hello world");


            Console.Read();
            
        }
    }
}
