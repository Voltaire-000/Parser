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
                    string m_description = m_currentLine.Substring(15, 42);
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
                if (unicodeCategory == UnicodeCategory.SpaceSeparator)
                {
                    string m_coeffLine = m_currentLine;
                    string m_
                }
                {

                    //streamWriter.Write
                }


                break;

            }
                streamReader.Close();
                streamWriter.Close();

        }
    }
}
