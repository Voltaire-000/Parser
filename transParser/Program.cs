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

                DoSwitch(streamWriter, streamReader, m_currentLine);


                //continue;
                //break;
                //if (streamReader.EndOfStream)
                //{
                //    streamReader.Close();
                //    streamWriter.Close();
                //}


            }



        }

        private static void DoSwitch(StreamWriter streamWriter, StreamReader streamReader, string m_currentLine)
        {
            string m_rootLabel = "transport_property_coefficients";
            UnicodeCategory unicodeCategory;
            char m_char = m_currentLine.First();
            unicodeCategory = char.GetUnicodeCategory(m_char);

            switch (unicodeCategory)
            {
                case UnicodeCategory.LowercaseLetter:
                    if (m_char != 'e')
                    {
                        PrintRootName(streamWriter, m_rootLabel);
                    }
                    if (m_char == 'x')
                    {
                        //  end of file
                        EndOfFile(streamWriter, streamReader);
                    }
 

                    break;
                case UnicodeCategory.UppercaseLetter:
                    //  Uppercase letter is new record set
                    //PrintSymbolsAndDescription(streamWriter, m_currentLine);
                    PrintNewSpecies(streamWriter, streamReader, m_currentLine);
                    break;
                case UnicodeCategory.SpaceSeparator:
                    //DoRecordSet(streamWriter, streamReader, m_currentLine);

                    break;

            }
        }

        private static void PrintNewSpecies(StreamWriter streamWriter, StreamReader streamReader, string m_currentLine)
        {
            //------------------Print open curly brace for species---------------------
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t");
            streamWriter.Write("{");
            //streamWriter.WriteLine();
            //-------------------------------------------------------------------------
            PrintSymbolsAndDescription(streamWriter, m_currentLine);
            DoRecordSet(streamWriter, streamReader, m_currentLine);

            //------------------Print closing curly brace for species------------------
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t");
            streamWriter.Write("}" + ",");
            //streamWriter.WriteLine();
            //------------------------------------------------------------------------------
        }

        private static void EndOfFile(StreamWriter streamWriter, StreamReader streamReader)
        {
            streamWriter.Close();
            streamReader.Close();
            //throw new NotImplementedException();
        }

        private static void DoRecordSet(StreamWriter streamWriter, StreamReader streamReader, string currentLine)
        {
            bool IsViscosityLine = false;
            bool IsCoefLine = false;
            int v_count = 0;
            int c_count = 0;

            //  --------------------------Print the open curly brace for the record set------------------
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t");
            streamWriter.Write("\"" + "RecordData" + "\"" + ":");
            streamWriter.Write("{");
            streamWriter.WriteLine();
            //  ------------------------------------------------------------------------------------------

            int m_peek = streamReader.Peek();
            while (m_peek == 32)
            {
                m_peek = streamReader.Peek();
                
                //streamWriter.WriteLine("test" + ",");
                IsViscosityLine = GetViscosityLine(currentLine);
                if (IsViscosityLine)
                {
                    v_count = v_count+ 1;
                    //streamWriter.WriteLine("\"" + "viscosity_" + v_count.ToString() + "\"");
                    PrintVTemps(streamWriter, currentLine, v_count);
                    PrintVcoefficients(streamWriter, currentLine);
                }

                IsCoefLine = GetCoefficientLine(currentLine);
                if (IsCoefLine)
                {
                    c_count= c_count+ 1;
                    //streamWriter.WriteLine("\"" + "coeff_" + c_count.ToString() +  "\"");
                    PrintCtemps(streamWriter, currentLine, c_count);
                    PrintCcoefficients(streamWriter, currentLine);

                }
                if (m_peek == 32)
                {
                    currentLine = streamReader.ReadLine();
                }
                


            }



            // ---------------------------Print the close curly brace for the record
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t");
            streamWriter.Write("}" + ",");
            streamWriter.WriteLine();
        }

        private static void PrintCtemps(StreamWriter streamWriter, string currentLine, int c_count)
        {
            string rangeLabel = "C_range_";
            string m_temp_1 = currentLine.Substring(2, 7);
            m_temp_1 = m_temp_1.Trim();
            string m_temp_2 = currentLine.Substring(9, 10);
            m_temp_2 = m_temp_2.Trim();
            streamWriter.WriteLine("\t\t\t\t\t\t\t" + "\"" + rangeLabel + c_count.ToString() + "\"" + ":");
            streamWriter.Write("\t\t\t\t\t\t\t");
            streamWriter.WriteLine("{");
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            streamWriter.WriteLine("\"" + "temperatureRange" + "\"" + ":" + "[" + m_temp_1 + ", " + m_temp_2 + "]" + ",");
        }

        private static bool GetCoefficientLine(string m_currentLine)
        {
            UnicodeCategory unicodeCategory;
            int m_C = 0;
            //m_currentLine = streamReader.ReadLine();
            char m_char = m_currentLine.First();
            unicodeCategory = char.GetUnicodeCategory(m_char);
            if (unicodeCategory == UnicodeCategory.SpaceSeparator)
            {
                m_C = m_currentLine.IndexOf('C');
                if (m_C == 1)
                {
                    return true;
                }

            }

            return false;
        }

        private static void PrintVTemps(StreamWriter streamWriter, string line, int count)
        {
            string rangeLabel = "V_range_";
            string m_temp_1 = line.Substring(2, 7);
            m_temp_1 = m_temp_1.Trim();
            string m_temp_2 = line.Substring(9, 10);
            m_temp_2 = m_temp_2.Trim();
            streamWriter.WriteLine("\t\t\t\t\t\t\t" + "\"" + rangeLabel + count.ToString() + "\"" + ":");
            streamWriter.Write("\t\t\t\t\t\t\t");
            streamWriter.WriteLine("{");
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            streamWriter.WriteLine("\"" + "temperatureRange" + "\"" + ":" + "[" + m_temp_1 + ", " + m_temp_2 + "]" + ",");
        }

        private static void PrintVtemps(StreamWriter streamWriter, string m_currentLine)
        {
            string rangeLabel = "range_";
            string m_temp_1 = m_currentLine.Substring(2, 7);
            m_temp_1 = m_temp_1.Trim();
            string m_temp_2 = m_currentLine.Substring(9, 10);
            m_temp_2 = m_temp_2.Trim();
            streamWriter.WriteLine("\"" + rangeLabel + "\"" + ":");
            streamWriter.Write("\t\t\t\t\t\t\t");
            streamWriter.WriteLine("{");
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            streamWriter.WriteLine("\"" + "V_temperatureRange" + "\"" + ":" + "[" + m_temp_1 + ", " + m_temp_2 + "]" + ",");
        }
        private static void PrintCtemps(StreamWriter streamWriter, string m_currentLine)
        {
            string rangeLabel = "temperaturesAndCoefficients";
            string m_temp_1 = m_currentLine.Substring(2, 7);
            m_temp_1 = m_temp_1.Trim();
            string m_temp_2 = m_currentLine.Substring(9, 10);
            m_temp_2 = m_temp_2.Trim();
            //streamWriter.WriteLine("\"" + rangeLabel + "\"" + ":");
            streamWriter.Write("\t\t\t\t\t\t\t");
            streamWriter.WriteLine("{");
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            streamWriter.WriteLine("\"" + "C_temperatureRange" + "\"" + ":" + "[" + m_temp_1 + ", " + m_temp_2 + "]" + ",");
        }

        private static bool GetViscosityLine(string m_currentLine)
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

        //private static void DoRecordSet(StreamWriter streamWriter, StreamReader streamReader, string currentLine)
        //{
        //    bool newRecordSet = false;
        //    bool viscosityLine = false;
        //    bool coefficientLine = false;
        //    string m_viscosityLabel = "tempsAndCoefficients";
        //    string m_coeff = "coefficients";
        //    string m_tempRangeLabel = "temperatureRange";
        //    string m_rangeLabel = "range_";
        //    int visCount = 0;
        //    int coefCount = 0;

        //    //------------------------- print the open bracket for record-----------------------------------------
        //    streamWriter.Write("\t\t\t\t");
        //    streamWriter.Write("{");
        //    //----------------------------------------------------------------------------------------------

        //    //  print symbols and description lines, 2 records
        //    PrintSymbolsAndDescription(streamWriter, currentLine);


        //    int m_peekNextLine = streamReader.Peek();

        //    while (m_peekNextLine == 32)
        //    {
        //        m_peekNextLine = streamReader.Peek();
        //        currentLine = streamReader.ReadLine();

        //        viscosityLine = GetNextViscosityLine(streamWriter, streamReader, currentLine);
        //        if (viscosityLine)
        //        {
        //            visCount = visCount + 1;
        //            if (visCount <= 1)
        //            {
        //                PrintViscosityLabels(streamWriter, m_viscosityLabel, m_tempRangeLabel);
        //            }

        //            //  print the temp range
        //            PrintViscosityLine(streamWriter, streamReader, currentLine, m_rangeLabel, visCount);
        //            //  print the coefficients
        //            PrintCoefficients(streamWriter, currentLine);
        //            streamWriter.Flush();
        //        }
        //        coefficientLine = GetCoefficientLine(streamWriter, streamReader, currentLine);
        //        if (coefficientLine)
        //        {
        //            coefCount = coefCount + 1;
        //            if (coefCount <= 1)
        //            {
        //                PrintViscosityLabels(streamWriter, m_coeff, m_tempRangeLabel);
        //            }
        //            //  print the temp range
        //            PrintCoeffLine(streamWriter, streamReader, currentLine, m_rangeLabel, coefCount);
        //            //  print the coefficients
        //            PrintCoefficients(streamWriter, currentLine);
        //            streamWriter.Flush();
        //        }

        //    }




        //    // ------------------------------ print the closing bracket for record------------------------------------------
        //    streamWriter.Write("\t\t\t\t");
        //    streamWriter.Write("}" + ",");
        //    //---------------------------------------------------------------------------------------------------------------
        //}

        private static void PrintViscosityLine(StreamWriter writer, StreamReader reader, string line)
        {
            string rangeLabel = "V_range_";
            // print the rangeLabel
            //rangeLabel = "V_" + rangeLabel + count.ToString();
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

        private static void PrintCoeffLine(StreamWriter streamWriter, StreamReader streamReader, string line, string rangeLabel, int count)
        {
            // print the rangeLabel
            rangeLabel = "C_" + rangeLabel + count.ToString();
            string m_temp_1 = line.Substring(2, 7);
            m_temp_1 = m_temp_1.Trim();
            string m_temp_2 = line.Substring(9, 10);
            m_temp_2 = m_temp_2.Trim();
            streamWriter.WriteLine("\"" + rangeLabel + "\"" + ":");
            streamWriter.Write("\t\t\t\t\t\t\t");
            streamWriter.WriteLine("{");
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            streamWriter.WriteLine("\"" + "temperatureRange" + "\"" + ":" + "[" + m_temp_1 + ", " + m_temp_2 + "]" + ",");
        }

        private static void PrintVcoefficients(StreamWriter streamWriter, string line)
        {
            // print coefficients
            //streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            streamWriter.Write("\"" + "coefficients" + "\"" + ":" + "[");
            line = line.Replace('E', 'e');
            int ml = line.Length;
            line = line.Insert(65, ",");
            line = line.Insert(50, ",");
            line = line.Insert(35, ",");
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
            streamWriter.WriteLine("}" + ",");
            //streamWriter.WriteLine("}");
        }
        private static void PrintCcoefficients(StreamWriter streamWriter, string line)
        {
            // print coefficients
            //streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t\t\t\t");
            streamWriter.Write("\"" + "coefficients" + "\"" + ":" + "[");
            line = line.Replace('E', 'e');
            int ml = line.Length;
            line = line.Insert(65, ",");
            line = line.Insert(50, ",");
            line = line.Insert(35, ",");
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
            streamWriter.WriteLine("}" + ",");
            //streamWriter.WriteLine("}");
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

        private static bool GetViscosityLine(StreamWriter streamWriter, StreamReader streamReader, string m_currentLine)
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

        private static bool GetCoefficientLine(StreamWriter streamWriter, StreamReader streamReader, string currentLine)
        {
            UnicodeCategory unicodeCategory;
            int m_V = 0;
            //m_currentLine = streamReader.ReadLine();
            char m_char = currentLine.First();
            unicodeCategory = char.GetUnicodeCategory(m_char);
            if (unicodeCategory == UnicodeCategory.SpaceSeparator)
            {
                m_V = currentLine.IndexOf('C');
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
                    //streamWriter.WriteLine();
                    //streamWriter.Write("\t\t\t\t\t");

                }
            }
        }

        private static bool PrintRootName(StreamWriter streamWriter, string fieldName)
        {
            streamWriter.Write("{");
            streamWriter.WriteLine();
            streamWriter.Write("\t" + "\"" + fieldName + "\"" + ":" + " " + "[");
            return true;
        }
    }
}
