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

            int v_count = 0;

            while (!streamReader.EndOfStream)
            {
                //  reads new line here
                m_currentLine = streamReader.ReadLine();
                if (!printRoot)
                {
                    printRoot = PrintRootName(streamWriter, rootName);
                }

                if (m_currentLine.First() != 't')
                {
                    DoRecordSet(streamWriter, streamReader, m_currentLine);
                    //m_currentLine = streamReader.ReadLine();
                    //PrintSymbolsAndDescription(streamWriter, streamReader, m_currentLine, m_symbol, m_description);
                    streamWriter.Flush();
                }



                //  do we have Coefficient line
                if (true)
                {

                }

                //continue;
                //break;
                if (streamReader.EndOfStream)
                {
                    streamReader.Close();
                    streamWriter.Close();
                }


            }



        }

        private static void DoRecordSet(StreamWriter streamWriter, StreamReader streamReader, string currentLine)
        {
            bool newRecordSet = false;
            bool viscosityLine = false;
            string m_viscosityLabel = "viscosity_coefficients";
            string m_tempRangeLabel = "temperatureRange";
            string m_rangeLabel = "range_";
            int visCount = 0;

            //------------------------- print the open bracket for record-----------------------------------------
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("{");
            //----------------------------------------------------------------------------------------------

            //  print symbols and description lines, 2 records
            PrintSymbolsAndDescription(streamWriter, currentLine);


            int m_peekNextLine = streamReader.Peek();

            while (m_peekNextLine == 32)
            {
                m_peekNextLine = streamReader.Peek();
                currentLine= streamReader.ReadLine();

                if (viscosityLine)
                {
                    visCount = visCount + 1;
                    if (visCount <= 1)
                    {
                        PrintViscosityLabels(streamWriter, m_viscosityLabel, m_tempRangeLabel);
                    }

                    //  print the temp range
                    PrintTempRange(streamWriter, streamReader, currentLine, m_rangeLabel, visCount);
                    //  print the coefficients
                    PrintCoefficients(streamWriter, currentLine);
                    streamWriter.Flush();
                }
                if (coefficientLine)
                {

                }

            }

            


            // ------------------------------ print the closing bracket for record------------------------------------------
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("}" + ",");
            //---------------------------------------------------------------------------------------------------------------
        }

        private static void PrintTempRange(StreamWriter writer, StreamReader reader, string line, string rangeLabel, int count)
        {
            // print the rangeLabel
            rangeLabel = rangeLabel + count.ToString();
            string m_temp_1 = line.Substring(2, 7);
            m_temp_1 = m_temp_1.Trim();
            string m_temp_2 = line.Substring(9, 10);
            m_temp_2 = m_temp_2.Trim();
            writer.WriteLine("\"" + rangeLabel + "\"" + ":");
            writer.Write("\t\t\t\t\t\t\t");
            writer.WriteLine("{");
            writer.Write("\t\t\t\t\t\t\t\t");
            writer.WriteLine("\"" + "temperatureRange" + "\"" + ":" + "[" + m_temp_1 + ", " + m_temp_2 + "]" + ",");
        }

        private static void PrintCoefficients(StreamWriter streamWriter, string line)
        {
            // print coefficients
            //streamWriter.WriteLine();
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

        private static void PrintViscosityLabels(StreamWriter streamWriter, string coefficientLabel, string tempLabel)
        {
            //m_currentLine = streamReader.ReadLine();
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t");
            streamWriter.Write("\"" + coefficientLabel + "\"" + ":" + "{");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t");
            streamWriter.Write("\"" + tempLabel + "\"" + ":" + "{");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t\t");

        }

        private static bool GetNextViscosityLine(StreamWriter streamWriter, StreamReader streamReader, string m_currentLine)
        {
            UnicodeCategory unicodeCategory;
            int m_V = 0;
            //m_currentLine = streamReader.ReadLine();
            char m_char = m_currentLine.First();
            unicodeCategory = char.GetUnicodeCategory(m_char);
            if (unicodeCategory == UnicodeCategory.SpaceSeparator)
            {
                m_V = m_currentLine.IndexOf('V');
                if (m_V == 1)
                {
                    return true;
                }

            }

            return false;
        }

        private static void PrintSymbolsAndDescription(StreamWriter streamWriter, string line)
        {
            string m_description = "description";

            string mx = line;

            if (mx != "end ")
            {
                UnicodeCategory unicodeCategory;
                streamWriter.WriteLine();
                char m_char = mx.First();
                unicodeCategory = char.GetUnicodeCategory(m_char);
                Type type = m_char.GetType();
                if (unicodeCategory != UnicodeCategory.LowercaseLetter || m_char == 'e')
                {
                    string name1 = mx.Substring(0, 15);
                    name1 = name1.Trim();
                    string name2 = mx.Substring(15, 15);
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

                    m_description = mx.Substring(32);
                    m_description = m_description.Trim();

                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "description" + "\"" + ": ");
                    streamWriter.Write("\"" + m_description + "\"" + ",");

                }
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
