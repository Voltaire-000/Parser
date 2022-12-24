using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
            string m_thermoFieldName = "thermo";
            string reactantFieldName = "reactant";
            string descriptionFieldName = "description";
            string t_intervalsFieldName = "T intervals";
            int count = 0;
            int m_peek = 0;
            string m_char = "";
            UnicodeCategory unicodeCategory;
            bool openBracketPrinted = false;

            StreamWriter streamWriter = new StreamWriter("..\\..\\z_json.json");
            StreamReader streamReader = new StreamReader("..\\..\\thermo.inp");

            while (!streamReader.EndOfStream)
            {
                //  print open bracket
                if (!openBracketPrinted)
                {
                    // we are at start of file
                    streamWriter.WriteLine("{");
                    openBracketPrinted= true;
                }

                streamReader.ReadLine();

                m_peek = streamReader.Peek();
                if (m_peek != -1)
                {
                    m_char = char.ConvertFromUtf32(m_peek);
                    unicodeCategory =  Char.GetUnicodeCategory(m_char, 0);
                    if (unicodeCategory == UnicodeCategory.LowercaseLetter && m_char == "t")
                    {
                        // this is thermo line
                        Console.WriteLine("Thermo line");
                        string m_line1 = streamReader.ReadLine();
                        m_thermoFieldName = JsonStart(m_line1, m_thermo, m_thermoFieldName, streamReader);
                        m_thermoFieldName = addQuotesAndSemicolon(m_thermoFieldName);
                        streamWriter.Write(m_thermoFieldName);
                        streamWriter.WriteLine("[");
                        streamWriter.WriteLine("\t\t{");
                    }

                    if (unicodeCategory == UnicodeCategory.UppercaseLetter)
                    {
                        //  this is a reactant field start of line. print reactant name to file
                        string m_nextLine = streamReader.ReadLine();
                        char separator = ' ';
                        string[] m_line = m_nextLine.Split(separator);
                        string compoundName = "\"" + m_line[0] + "\"" + ",";
                        streamWriter.Write("\t\t");
                        streamWriter.Write(reactantFieldName);
                        streamWriter.Write(compoundName);
                        streamWriter.WriteLine();
                    }
                }
                else
                {
                    //  we are at end of file. close the streamWriter
                    streamWriter.Close();
                }

                //Console.WriteLine(count);
                //count = count +1;

            }


            //int startDescription = compoundName.Length;
            //string m_description = m_nextLine.Substring(startDescription);
            //m_description = "\"" + m_description + "\"" + ",";
            //m_nextLine = streamReader.ReadLine();
            //string t_intervals = m_nextLine.Substring(0, 2);

            //string optionalId = m_nextLine.Substring(3, 9);
            //Console.WriteLine(m_line[0]);
            

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

            reactantFieldName = addQuotesAndSemicolon(reactantFieldName);
            descriptionFieldName = addQuotesAndSemicolon(descriptionFieldName);
            t_intervalsFieldName = addQuotesAndSemicolon(t_intervalsFieldName);

            //string descriptionField = "\n\t\t\t\"" + "description" + "\"" + ":" + " ";
            //string t_intervalField  = "\n\t\t\t\"" + "t_intervals"  + "\"" + ":" + "";
            
            parser.SetFieldWidths(24);
            TextFields m_descr = parser.ReadFields();

            Console.WriteLine(" Thermo file to json");

            streamWriter.Write("\t\t");
            streamWriter.Write(descriptionFieldName);
            //streamWriter.Write(m_description);
            streamWriter.WriteLine();

            streamWriter.Write("\t\t");
            streamWriter.Write(t_intervalsFieldName);
            //streamWriter.Write(t_intervals);
            streamWriter.Close();
            Console.Read();
        }

        private static string addQuotesAndSemicolon(string fieldName)
        {
            fieldName = "\t\"" + fieldName + "\"" + ":" + "";
            return fieldName;
        }

        private static string JsonStart(string m_line1, string m_thermo,string m_thermoField, StreamReader streamReader)
        {
            if (m_line1 == "")
            {
                string m_thermoLine = streamReader.ReadLine();
                m_thermo = m_thermoLine.Substring(0, 6);
                //m_thermoField = "\t\"" + m_thermo.ToString() + "\"";
                m_thermoField = addQuotesAndSemicolon(m_thermoField);
            }
            return m_thermoField;
        }
    }
}
