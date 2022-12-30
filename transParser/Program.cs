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
            StreamReader streamReader = new StreamReader("..\\..\\trans.inp");
            StreamWriter streamWriter = new StreamWriter("..\\..\\trans.json");
            streamWriter.AutoFlush = true;

            string rootName = "transport_property_coefficients";
            bool printRoot = false;

            string m_currentLine = "";
            string m_symbol = "symbol";
            string m_description = "description";
            string m_viscosityLabel = "viscosity_coefficients";

            while (!streamReader.EndOfStream)
            {

                if (!printRoot)
                {
                    printRoot = PrintRootName(streamWriter, rootName);

                }

                m_currentLine = streamReader.ReadLine();

                // skip over first line
                if (m_currentLine.First() == 't')
                {
                    m_currentLine = streamReader.ReadLine();
                }

                PrintNameAndDescription(streamWriter, m_currentLine, m_symbol, m_description);
                GetFirstCharEachLine(streamWriter, streamReader, m_currentLine);


                break;

            }
                streamReader.Close();
                streamWriter.Close();

        }

        private static void GetFirstCharEachLine(StreamWriter streamWriter, StreamReader streamReader, string m_currentLine)
        {
            
        }

        private static void PrintNameAndDescription(StreamWriter streamWriter, string m_currentLine, string m_symbol, string m_description)
        {
            UnicodeCategory unicodeCategory;
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("{");
            streamWriter.WriteLine();

            char m_char = m_currentLine.First();
            unicodeCategory = char.GetUnicodeCategory(m_char);
            Type type = m_char.GetType();
            if (unicodeCategory != UnicodeCategory.LowercaseLetter || m_char == 'e')
            {
                m_symbol = m_currentLine.Substring(0, 15);
                m_description = m_currentLine.Substring(15);
                m_symbol = m_symbol.Trim();
                m_description = m_description.Trim();
                streamWriter.Write("\t\t\t\t\t");
                streamWriter.Write("\"" + "symbol" + "\"" + ": ");
                streamWriter.Write("\"" + m_symbol + "\"" + ",");
                streamWriter.WriteLine();
                streamWriter.Write("\t\t\t\t\t");
                streamWriter.Write("\"" + "description" + "\"" + ": ");
                streamWriter.Write("\"" + m_description + "\"" + ",");

            }


        }

        private static bool PrintRootName(StreamWriter streamWriter, string fieldName)
        {
            streamWriter.Write("{");
            streamWriter.WriteLine();
            streamWriter.Write("\t" + "\"" + fieldName + "\"" + ":" + " " + "[");
            streamWriter.WriteLine();
            return true;
        }
    }
}
