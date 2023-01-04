using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThermoParser
{
    internal class Program
    {
        private static string m_currentLine = "";
        private static StreamReader streamReader;
        private static StreamWriter streamWriter;

        static void Main(string[] args)
        {
            streamReader = new StreamReader("..\\..\\thermo.inp");
            streamWriter = new StreamWriter("..\\..\\thermoINP.json");
            streamWriter.AutoFlush = true;

            while (!streamReader.EndOfStream)
            {
                m_currentLine = streamReader.ReadLine();
                DoSwitch(streamReader, streamWriter, m_currentLine);
                streamWriter.WriteLine();

                if (streamReader.BaseStream == null)
                {
                    break;
                }

            }
        }

        private static void DoSwitch(StreamReader streamReader, StreamWriter streamWriter, string m_currentLine)
        {

            UnicodeCategory unicodeCategory;
            char m_char = m_currentLine.First();
            unicodeCategory = char.GetUnicodeCategory(m_char);

            switch (unicodeCategory)
            {
                case UnicodeCategory.LowercaseLetter:
                    if (m_char != 'e')
                    {
                        PrintRootName();
                    }
                    else
                    {
                        Print_ReferenceSpecies();
                        break;
                    }
                    if (m_char == 'x')
                    {
                        //  end of file
                        EndOfFile();
                    }
                    break;
                    case UnicodeCategory.UppercaseLetter:
                    //  uppercase is new species with the exception of ref species
                    PrintNewSpecies();
                    break;
                case UnicodeCategory.SpaceSeparator:
                    break;

            }

        }

        private static void Print_ReferenceSpecies()
        {
            PrintNewSpecies();
        }

        private static void DoRecordSet()
        {
            throw new NotImplementedException();
        }

        private static void EndOfFile()
        {
            throw new NotImplementedException();
        }

        private static void BeginNewJsonFile(StreamWriter streamWriter, StreamReader streamReader, string m_currentFile)
        {
            //--------------Print open curly for file--------------
            streamWriter.Write("{");

            //PrintRootName();


            //--------------Print close curly for file
            streamWriter.WriteLine();
            streamWriter.Write("}");

        }

        private static void PrintRootName()
        {
            string rootName = "thermo";
            //string rootName = "species";
            streamWriter.Write("{");
            streamWriter.WriteLine();
            streamWriter.Write("\t");
            streamWriter.Write("\"" + rootName + "\"" + ":");
            streamWriter.WriteLine();
            streamWriter.Write("\t");
            streamWriter.Write("[");
        }

        private static void PrintOpenCloseBracket(StreamWriter streamWriter)
        {
            streamWriter.WriteLine();
            streamWriter.Write("\t");
            streamWriter.Write("[");
            PrintNewSpecies();
            streamWriter.WriteLine();
            streamWriter.Write("\t");
            streamWriter.Write("]");

        }

        private static void PrintNewSpecies()
        {
            //----------------Open curly brace for new species
            streamWriter.WriteLine();
            streamWriter.Write("\t\t");
            streamWriter.Write("{");
            //------------------------------------------------
            PrintSpeciesAndDescription();
            m_currentLine = streamReader.ReadLine();
            PrintTintervalsLine();

            //---------------Close curly for new species
            streamWriter.WriteLine();
            streamWriter.Write("\t\t");
            streamWriter.Write("}" + ",");
            //-------------------------------------------

        }

        private static void PrintTintervalsLine()
        {
            // t intervals
            string m_tIntervalsLabel = "tIntervals";
            string m_tIntervals;
            char separator = ' ';
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            streamWriter.Write("\"" + m_tIntervalsLabel + "\"" + ":");
            
            if (m_currentLine != null)
            {
                m_tIntervals = m_currentLine.Substring(0, 2);
                int.TryParse(m_tIntervals, out int value);
                streamWriter.Write(value + " ,"); 
            }

            //  id code
            string m_IdLabel = "idCode";
            string m_IdCode;
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            if (m_currentLine != null)
            {
                m_IdCode = m_currentLine.Substring(3, 7);
                m_IdCode = m_IdCode.Trim();
                streamWriter.Write("\"" + m_IdLabel + "\"" + ":");
                streamWriter.Write("\"" + m_IdCode + "\"" + ","); 
            }
            //  chemical formula line


        }

        private static void PrintSpeciesAndDescription()
        {
            string m_speciesLabel = "species";
            string m_species;
            string m_descriptionLabel = "description";
            string m_description;

            m_species = m_currentLine.Substring(0, 15);
            m_species = m_species.Trim();
            m_description = m_currentLine.Substring(15, 15);
            m_description = m_description.Trim();
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            streamWriter.Write("\"" + m_speciesLabel + "\"" + ": ");
            streamWriter.Write("\"" + m_species + "\"" + ",");

            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            streamWriter.Write("\"" + m_descriptionLabel+ "\"" + ": ");
            streamWriter.Write("\"" + m_description+ "\"" + ",");
        }
    }
}
