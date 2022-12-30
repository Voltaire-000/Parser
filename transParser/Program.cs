using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace transParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UnicodeCategory unicodeCategory;

            StreamReader streamReader = new StreamReader("..\\..\\trans.inp");
            StreamWriter streamWriter = new StreamWriter("..\\..\\trans.json");
            streamWriter.AutoFlush = true;

            string m_currentLine = "";

            streamWriter.Write("{");
            streamWriter.WriteLine();
            streamWriter.Write("\t" + "\"" + "transport_property_coefficients" + "\"" + ":" + " " + "[");
            streamWriter.WriteLine();
            //streamWriter.Write("\t\t\t\t\t" + "{");

            while (!streamReader.EndOfStream)
            {
                m_currentLine = streamReader.ReadLine();

                // skip over first line
                if (m_currentLine.First() == 't')
                {
                    m_currentLine = streamReader.ReadLine();
                }

                streamWriter.Write("\t\t\t\t");
                streamWriter.Write("{");
                streamWriter.WriteLine();

                char m_char = m_currentLine.First();
                unicodeCategory = char.GetUnicodeCategory(m_char);
                Type type = m_char.GetType();
                if (unicodeCategory != UnicodeCategory.LowercaseLetter || m_char == 'e')
                {
                    string m_speciesName = m_currentLine.Substring(0, 15);
                    string m_description = m_currentLine.Substring(15);
                    m_speciesName = m_speciesName.Trim();
                    m_description = m_description.Trim();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "symbol" + "\"" + ": ");
                    streamWriter.Write("\"" + m_speciesName + "\"" + ",");
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "description" + "\"" + ": ");
                    streamWriter.Write("\"" + m_description + "\"" + ",");
                    m_currentLine = streamReader.ReadLine();
                    m_char = m_currentLine.First();
                    unicodeCategory = char.GetUnicodeCategory(m_char);
                }
                int vCount = 0;
                if (unicodeCategory == UnicodeCategory.SpaceSeparator)
                {
                    char m_V = m_currentLine.ElementAt(1);
                    if (m_V == 'V')
                    {
                        vCount = vCount + 1;
                        //  we have viscosity coefficients
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t");
                        streamWriter.Write("\"" + "viscosity_coefficients" + "\"" + ": " + "{");
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t\t\t");
                        streamWriter.Write("\"" + "temperatureRange" + "\"" + ": " + "{");
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t\t\t\t\t");
                        streamWriter.Write("\"" + "range_" + vCount.ToString() + "\"" + ": " + "{ ");
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t\t\t\t\t\t");
                        streamWriter.Write("\"" + "temperatureRange" + "\"" + ":" + "[");
                        string m_tempInterval_1 = m_currentLine.Substring(3, 6);
                        streamWriter.Write(m_tempInterval_1 + ", ");
                        string m_tempInterval_2 = m_currentLine.Substring(10, 8);
                        m_tempInterval_2 = m_tempInterval_2.Trim();
                        streamWriter.Write(m_tempInterval_2 + "]" + ",");
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t\t\t\t\t\t");
                        streamWriter.Write("\"" + "coefficients" + "\"" + ":" + "[");
                        string m_coeff = m_currentLine.Substring(21, 59);
                        m_coeff = m_coeff.Replace('E', 'e');
                        m_coeff = m_coeff.Replace("e ", "e+");
                        m_coeff = m_coeff.Insert(14, " ");
                        m_coeff = m_coeff.Replace(" ", ", ");
                        
                        streamWriter.Write(m_coeff + "]");
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t\t\t\t\t\t");
                        streamWriter.Write("}" + ",");

                        m_currentLine = streamReader.ReadLine();
                        m_V = m_currentLine.ElementAt(1);

                    }
                }
                {

                    //streamWriter.Write
                }


                //break;

            }
                streamReader.Close();
                streamWriter.Close();

        }
    }
}
