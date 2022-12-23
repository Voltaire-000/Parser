using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextFileParsers;

namespace Parser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string m_thermo = "";
            string m_thermoField = "";
            StreamWriter streamWriter = new StreamWriter("..\\..\\z_json.json");
            StreamReader streamReader = new StreamReader("..\\..\\thermo.inp");

            string m_line1 = streamReader.ReadLine();
            // we are at start of file
            streamWriter.WriteLine("{");
            //streamWriter.Write("\t");
            //streamWriter.Write("\"");
            m_thermoField = JsonStart(m_line1, m_thermo, m_thermoField, streamReader);
            streamWriter.Write(m_thermoField);
            //streamWriter.Write("\"");

            string m_nextLine = streamReader.ReadLine();
            char separator = ' ';
            string[] m_line = m_nextLine.Split(separator);
            string compoundName = m_line[0];
            int startDescription = compoundName.Length;
            string m_description = m_nextLine.Substring(startDescription);
            Console.WriteLine(m_line[0]);
            

            TextFileParsers.FixedWidthFieldParser parser = new TextFileParsers.FixedWidthFieldParser("..\\..\\thermo.inp");
            TextFileParsers.DelimitedFieldParser delimitedFieldParser = new DelimitedFieldParser("..\\..\\thermo.inp");

            parser.SkipLine();
            parser.SetFieldWidths(6);

            delimitedFieldParser.SetDelimiters(' ');
            delimitedFieldParser.SkipLines(2);
            delimitedFieldParser.SqueezeDelimiters = true;
            //TextFields m_description = delimitedFieldParser.ReadFields();
            parser.SetFieldWidths(2);
            TextFields m_name = parser.ReadFields();
            string mm_name = "\"" + m_name.ToString() + "\"" + ",";
            string nameField = "\t\t\t\"" + "name" + "\"" + ":" + " ";
            string descriptionField = "\n\t\t\t\"" + "description" + "\"" + ":" + " ";
            
            parser.SetFieldWidths(24);
            TextFields m_descr = parser.ReadFields();

            Console.WriteLine(" Thermo file to json");

            

            //streamWriter.Write(m_thermo);
            
            
            streamWriter.Write(":");
            streamWriter.WriteLine(" [");
            streamWriter.WriteLine("\t\t{");
            streamWriter.Write(nameField);
            streamWriter.Write(mm_name);
            streamWriter.Write(descriptionField);
            streamWriter.Close();
            Console.Read();
        }

        private static string JsonStart(string m_line1, string m_thermo,string m_thermoField, StreamReader streamReader)
        {
            if (m_line1 == "")
            {
                string m_thermoLine = streamReader.ReadLine();
                m_thermo = m_thermoLine.Substring(0, 6);
                m_thermoField = "\t\"" + m_thermo.ToString() + "\"";
            }
            return m_thermoField;
        }
    }
}
