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
            string m_speciesNames = "species names";
            string m_description = "description";
            string m_viscosityLabel = "viscosity_coefficients";
            string m_tempRangeLabel = "temperatureRange";
            string m_rangeLabel = "range_";
            int v_count = 0;

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

                if (m_currentLine.First() != ' ')
                {
                    PrintSymbolsAndDescription(streamWriter, m_currentLine, m_symbol, m_description);
                    streamWriter.Flush();
                }

                
                string viscosityLine = GetNextViscosityLine(streamWriter, streamReader, m_currentLine);
                if (viscosityLine != null)
                {
                    PrintViscosityLabels(streamWriter, viscosityLine, m_viscosityLabel, m_tempRangeLabel, m_rangeLabel, v_count);
                    //m_currentLine= streamReader.ReadLine();
                    PrintTempRange(streamWriter, streamReader, m_currentLine);
                    PrintCoefficients(streamWriter, m_currentLine);
                    streamWriter.FlushAsync();
                }


                //break;

            }
                streamReader.Close();
                streamWriter.Close();

        }

        private static void PrintTempRange(StreamWriter streamWriter, StreamReader streamReader, string line)
        {
            //line = streamReader.ReadLine();
            string m_temp_1 = line.Substring(2, 7);
            m_temp_1= m_temp_1.Trim();
            string m_temp_2 = line.Substring(9, 10);
            m_temp_2= m_temp_2.Trim();
            streamWriter.WriteLine(m_temp_1 + ", " + m_temp_2 + "]" + ",");
        }

        private static void PrintCoefficients(StreamWriter streamWriter, string line)
        {
            // print coefficients
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            streamWriter.Write("\"" + "coefficients" + "\"" + ":" + "[");
            line = line.Replace('E', 'e');
            int ml = line.Length;
            line = line.Insert(66, ",");
            line = line.Insert(51, ",");
            line = line.Insert(36, ",");
            //line = line.Insert(21, ",");
            string m_viscosityLine = line.Substring(20);
            m_viscosityLine = m_viscosityLine.Trim();
            //m_viscosityLine = m_viscosityLine.Replace(' ', '+');
            string[] split = m_viscosityLine.Split(',');
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Trim();
            }
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Replace(' ', '+');
            }
            streamWriter.Write(split[0] + ", " + split[1] + ", " + split[2] + ", " + split[3] + "]");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t\t");
            streamWriter.Write("}");
        }

        private static void PrintViscosityLabels(StreamWriter streamWriter, string line, string coefficientLabel, string tempLabel, string rangeLabel, int count)
        {
            //m_currentLine = streamReader.ReadLine();
            int v_count = count + 1;
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t");
            streamWriter.Write("\"" + coefficientLabel + "\"" + ":" + "{");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t");
            streamWriter.Write("\"" + tempLabel + "\"" + ":" + "{");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t\t");
            streamWriter.Write("\"" + rangeLabel + v_count.ToString() + "\"" + ":");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t\t");
            streamWriter.Write("{");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            ////  print temperature range as array [200.0, 1000.0],
            //streamWriter.Write("\"" + tempLabel + "\"" + ":" + "[");
            //string m_temp_1 = line.Substring(2, 7);
            //m_temp_1 = m_temp_1.Trim();
            //string m_temp_2 = line.Substring(9, 10);
            //m_temp_2 = m_temp_2.Trim();
            //streamWriter.Write(m_temp_1 + ", " + m_temp_2 + "]" + ",");

            //// print coefficients
            //streamWriter.WriteLine();
            //streamWriter.Write("\t\t\t\t\t\t\t\t");
            //streamWriter.Write("\"" + "coefficients" + "\"" + ":" + "[");
            //line = line.Replace('E', 'e');
            //int ml = line.Length;
            //line = line.Insert(66, ",");
            //line = line.Insert(51, ",");
            //line = line.Insert(36, ",");
            ////line = line.Insert(21, ",");
            //string m_viscosityLine = line.Substring(20);
            //m_viscosityLine = m_viscosityLine.Trim();
            ////m_viscosityLine = m_viscosityLine.Replace(' ', '+');
            //string[] split = m_viscosityLine.Split(',');
            //for (int i = 0; i < split.Length; i++)
            //{
            //    split[i] = split[i].Trim();
            //}
            //for (int i = 0; i < split.Length; i++)
            //{
            //    split[i] = split[i].Replace(' ', '+');
            //}

            //streamWriter.Write(split[0] + ", " + split[1] + ", " + split[2] + ", " + split[3] + "]");
            //streamWriter.WriteLine();
            //streamWriter.Write("\t\t\t\t\t\t\t");
            //streamWriter.Write("}");

        }

        private static string GetNextViscosityLine(StreamWriter streamWriter, StreamReader streamReader, string m_currentLine)
        {
            UnicodeCategory unicodeCategory;
            int m_V = 0;
            m_currentLine = streamReader.ReadLine();
            char m_char = m_currentLine.First();
            unicodeCategory = char.GetUnicodeCategory(m_char);
            if (unicodeCategory == UnicodeCategory.SpaceSeparator)
            {
                m_V = m_currentLine.IndexOf('V');
                return m_currentLine;
            }

            return null;
        }

        private static void PrintSymbolsAndDescription(StreamWriter streamWriter, string m_currentLine, string m_symbol, string m_description)
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
                string name1 = m_currentLine.Substring(0, 15);
                name1 = name1.Trim();
                string name2 = m_currentLine.Substring(15, 15);
                name2 = name2.Trim();
                if (name2.Length > 0)
                {
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "species names" + "\"" + ": " + "[");
                    streamWriter.Write("\"" + name1 + "\"" + "," + "\"" + name2 + "\"" + "]" + ",");
                }
                else
                {
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "species names" + "\"" + ": " + "[");
                    streamWriter.Write("\"" + name1 + "\"" + "]" + ",");
                }

                m_description = m_currentLine.Substring(32);
                m_description = m_description.Trim();

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
