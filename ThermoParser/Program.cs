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
            //  end in middle of file
            string endSubstring = m_currentLine.Substring(0, 3);
            if (endSubstring != "END")
            {

            }

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
            PrintDataRecords();

            //---------------Close curly for new species
            streamWriter.WriteLine();
            streamWriter.Write("\t\t");
            streamWriter.Write("}" + ",");
            //-------------------------------------------

        }

        private static void PrintDataRecords()
        {
            string m_recordLabel = "dataRecords";
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            streamWriter.Write("\"" + m_recordLabel + "\"" + ":");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("[");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("  {");

            streamWriter.WriteLine();
            streamWriter.Write("\"" + "stuff" + "\"" + ":" + "1.0");

            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("  }");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("]");
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
            string m_formulaLabel = "chemicalformula";
            string m_symbolLabel = "symbol";
            string m_numElementslabel = "numberOfAtoms";
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            streamWriter.Write("\"" + m_formulaLabel+ "\"" + ":");
            string chemFormulaSubstring = m_currentLine.Substring(9, 41);
            chemFormulaSubstring= chemFormulaSubstring.Trim();
            int firstColumn = 3;    // element
            int secondColumn = 5;   // number
            int thirdColumn = 3;    // element
            int fourthColumn = 5;   // number
            int fifthColumn = 3;
            int sixthColumn = 5;    // element
            int seventhColumn = 3;  // number
            int eigthColumn = 5;    // element
            int ninthColumn = 3;    // number
            int tenthColumn = 5;    // element

            //int firstNumber = chemFormulaSubstring.IndexOfAny(anyof);   // returns 4
            string firstElement = chemFormulaSubstring.Substring(0, firstColumn); // return "AG"
            firstElement = firstElement.Trim();
            int l_firstElement = firstElement.Length;
            string firstAtoms = chemFormulaSubstring.Substring(firstColumn, secondColumn);

            string secondElement = chemFormulaSubstring.Substring(firstColumn + secondColumn, thirdColumn);
            secondElement = secondElement.Trim();
            int l_secondElement = secondElement.Length;
            string secondAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn, fourthColumn);
            double n_secondAtoms = 0.0;
            double.TryParse(secondAtoms, out n_secondAtoms);

            string thirdElement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn, fifthColumn);
            thirdElement = thirdElement.Trim();
            int l_thirdElement = thirdElement.Length;
            string thirdAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn, sixthColumn);
            double n_thirdAtoms = 0.0;
            double.TryParse(thirdAtoms, out n_thirdAtoms);

            string fourthElement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn, seventhColumn);
            fourthElement = fourthElement.Trim();
            int l_fourthElement = fourthElement.Length;
            string fourthAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn + seventhColumn, eigthColumn);
            double n_fourthAtoms = 0.0;
            double.TryParse(fourthAtoms, out n_fourthAtoms);

            string fifthelement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn + seventhColumn + eigthColumn, ninthColumn);
            fifthelement = fifthelement.Trim();
            int l_fifthElement = fifthelement.Length;
            string fifthAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn + seventhColumn + eigthColumn + ninthColumn, tenthColumn);
            double n_fifthAtoms = 0.0;
            double.TryParse(fifthAtoms, out n_fifthAtoms);
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("[");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("  {");
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t");

            //  write the element symbol
            streamWriter.Write("\"" + m_symbolLabel + "\"" + ": ");
            streamWriter.Write("\"" + firstElement + "\"" + ",");

            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t\t");
            streamWriter.Write("\"" + m_numElementslabel + "\"" + ": ");
            streamWriter.Write(firstAtoms);
            //  if no more atoms print close curly, else print curly + ,
            if (n_secondAtoms != 0)
            {
                streamWriter.WriteLine();
                streamWriter.Write("\t\t\t\t");
                streamWriter.Write("  }" + ",");
                streamWriter.WriteLine();
                streamWriter.Write("\t\t\t\t");
                streamWriter.Write("  {");
                streamWriter.WriteLine();
                streamWriter.Write("\t\t\t\t\t");
                streamWriter.Write("\"" + m_symbolLabel + "\"" + ": ");
                streamWriter.Write("\"" + secondElement + "\"" + ",");
                streamWriter.WriteLine();
                streamWriter.Write("\t\t\t\t\t");
                streamWriter.Write("\"" + m_numElementslabel + "\"" + ":");
                streamWriter.Write(secondAtoms);


                if (n_thirdAtoms !=0)
                {
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t");
                    streamWriter.Write("  }" + ",");

                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t");
                    streamWriter.Write("  {");
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + m_symbolLabel + "\"" + ": ");
                    streamWriter.Write("\"" + thirdElement + "\"" + ",");
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + m_numElementslabel + "\"" + ":");
                    streamWriter.Write(thirdAtoms);

                    if (n_fourthAtoms !=0)
                    {
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t");
                        streamWriter.Write("  }" + ",");

                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t");
                        streamWriter.Write("  {");
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t");
                        streamWriter.Write("\"" + m_symbolLabel + "\"" + ": ");
                        streamWriter.Write("\"" + fourthElement + "\"" + ",");
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t");
                        streamWriter.Write("\"" + m_numElementslabel + "\"" + ":");
                        streamWriter.Write(fourthAtoms);

                        if (n_fifthAtoms !=0)
                        {
                            streamWriter.WriteLine();
                            streamWriter.Write("\t\t\t\t");
                            streamWriter.Write("  }" + ",");

                            streamWriter.WriteLine();
                            streamWriter.Write("\t\t\t\t");
                            streamWriter.Write("  {");
                            streamWriter.WriteLine();
                            streamWriter.Write("\t\t\t\t\t");
                            streamWriter.Write("\"" + m_symbolLabel + "\"" + ": ");
                            streamWriter.Write("\"" + fifthelement + "\"" + ",");
                            streamWriter.WriteLine();
                            streamWriter.Write("\t\t\t\t\t");
                            streamWriter.Write("\"" + m_numElementslabel + "\"" + ":");
                            streamWriter.Write(fifthAtoms);
                            streamWriter.WriteLine();
                            streamWriter.Write("\t\t\t\t");
                            streamWriter.Write("  }");
                        }
                        else
                        {
                            streamWriter.WriteLine();
                            streamWriter.Write("\t\t\t\t");
                            streamWriter.Write("  }");
                        }


                    }
                    else
                    {
                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t");
                        streamWriter.Write("  }");
                    }

                }
                else
                {
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t");
                    streamWriter.Write("  }");
                }


            }
            else
            {
                streamWriter.WriteLine();
                streamWriter.Write("\t\t\t\t");
                streamWriter.Write("  }");
            }

            //  print the closing bracket for formula
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t");
            streamWriter.Write("]" + ",");

            //  gas species line
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            string speciesType = m_currentLine.Substring(41, 1);
            streamWriter.Write("\"" + "gaseous" + "\"" + ": ");
            if (speciesType == "0")
            {
                streamWriter.Write("false" + ",");
            }
            else
            {
                streamWriter.Write("true" + ",");
            }

            // molecular weight line
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            string moleWeightSubstring = m_currentLine.Substring(53, 12);
            moleWeightSubstring = moleWeightSubstring.Trim();
            streamWriter.Write("\"" + "molecularWeight" + "\"" + ": ");
            streamWriter.Write(moleWeightSubstring + ",");

            //  heat of formation line
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            string heatSubstring = m_currentLine.Substring(65, 15);
            heatSubstring = heatSubstring.Trim();
            streamWriter.Write("\"" + "heatOfFormation" + "\"" + ":");
            streamWriter.Write(" " + heatSubstring + ",");

            // end of line read new line

        }

        private static void PrintSpeciesAndDescription()
        {
            //  special case for END PRODUCTS line 15347
            string endProducts = m_currentLine.Substring(0, 3);
            if (endProducts == "END")
            {
                //  skip over line
                m_currentLine = streamReader.ReadLine();
            }

            //  special case for air
            string airSubstring = m_currentLine.Substring(0, 3);
            if (airSubstring == "Air")
            {

                //throw new NotImplementedException();
            }

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
