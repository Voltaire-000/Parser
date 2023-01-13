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
        private static int t_intervalValue = 0;
        private static int m_specieValue = 0;

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
            //m_currentLine = streamReader.ReadLine();

            // temp range
            char separator = ' ';
            char comma = ',';

            if (t_intervalValue == 0 && m_specieValue >= 1)
            {
                m_currentLine = streamReader.ReadLine();
                string boilingPointLabel = "boilingPoint";
                string boilingPointSubstring = m_currentLine.Substring(0, 12);
                boilingPointSubstring= boilingPointSubstring.Trim();
                streamWriter.WriteLine();
                streamWriter.Write("\t\t\t");
                streamWriter.Write("\"" + boilingPointLabel + "\"" + ": ");
                streamWriter.Write(boilingPointSubstring);
            }
            else
            {
                for (int i = 0; i < t_intervalValue; i++)
                {
                    m_currentLine = streamReader.ReadLine();
                    string tempRange = m_currentLine.Substring(0, 22);
                    tempRange = tempRange.Trim();
                    int indx = tempRange.IndexOf(separator);
                    //tempRange = tempRange.Replace(' ', ',');
                    tempRange = tempRange.Insert(indx, ",");
                    string[] tempRangeLine = tempRange.Split(comma);

                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("{");
                    streamWriter.Write("\t\t\t\t\t");

                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "temperatureRange" + "\"" + ":");
                    streamWriter.Write("[" + tempRangeLine[0] + ", " + tempRangeLine[1].Trim() + "]" + ",");

                    // number of coefficients
                    string m_coeff = m_currentLine.Substring(22, 1);

                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "numberOfCoefficients" + "\"" + ":");
                    streamWriter.Write(" " + m_coeff + ",");

                    // t exponents array
                    string m_texponents = m_currentLine.Substring(23, 40);
                    m_texponents = m_texponents.Trim();
                    m_texponents = m_texponents.Replace(" ", ",");
                    m_texponents = m_texponents.Replace(",,", ",");
                    string[] tExponentLine = m_texponents.Split(comma);

                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "tExponents" + "\"" + ":");
                    streamWriter.Write("[" + tExponentLine[0] + ", " + tExponentLine[1] + ", " + tExponentLine[2] + ", " + tExponentLine[3] + ", " + tExponentLine[4] + ", " + tExponentLine[5] + ", " + tExponentLine[6] + ", " + tExponentLine[7] + "]" + ",");

                    // H line
                    string m_hLine = m_currentLine.Substring(66, 14);
                    m_hLine = m_hLine.Trim();

                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "hJmol" + "\"" + ":");
                    streamWriter.Write(" " + m_hLine + ",");
                    //*****************************************
                    // must read new line here
                    m_currentLine = streamReader.ReadLine();
                    string coefFirstPart = m_currentLine;
                    string coefSecondPart = streamReader.ReadLine();
                    string concantCoef = coefFirstPart + coefSecondPart;
                    concantCoef = concantCoef.Replace('D', 'e');
                    string coefSubstring = concantCoef.Substring(0, 128);
                    //coefSubstring = coefSubstring.Trim();
                    int firstE = coefSubstring.IndexOf('e');
                    int secondE = coefSubstring.IndexOf('e', firstE);
                    int thirdE = coefSubstring.IndexOf('e', secondE);
                    int forthE = coefSubstring.IndexOf('e', thirdE);
                    int fifthE = coefSubstring.IndexOf('e', forthE);
                    int sixthE = coefSubstring.IndexOf('e', fifthE);
                    int seventhE = coefSubstring.IndexOf('e', sixthE);
                    int eighthE = coefSubstring.IndexOf('e', seventhE);

                    string firstCoef = coefSubstring.Substring(0, firstE + 4);
                    string secondCoef = coefSubstring.Substring(secondE + 4, firstE + 4);
                    string thirdCoef = coefSubstring.Substring(thirdE + 4 + secondE + 4, firstE + 4);
                    string forthCoef = coefSubstring.Substring(forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);
                    string fifthCoef = coefSubstring.Substring(fifthE + 4 + forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);
                    string sixthCoef = coefSubstring.Substring(sixthE + 4 + fifthE + 4 + forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);
                    string seventhCoef = coefSubstring.Substring(seventhE + 4 + sixthE + 4 + fifthE + 4 + forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);
                    string eighthCoef = coefSubstring.Substring(eighthE + 4 + seventhE + 4 + sixthE + 4 + fifthE + 4 + forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);

                    string CoefConcant = firstCoef + " " + secondCoef + " " + thirdCoef + " " + forthCoef + " " + fifthCoef + " " + sixthCoef + " " + seventhCoef + " " + eighthCoef;
                    CoefConcant = CoefConcant.Trim();
                    CoefConcant = CoefConcant.Replace(" ", ",");
                    CoefConcant = CoefConcant.Replace(",,", ",");
                    string[] coeline = CoefConcant.Split(comma);


                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "coefficients" + "\"" + ":");

                    if (coeline.Count() == 7)
                    {
                        streamWriter.Write("[" + coeline[0] + ", " + coeline[1] + ", " + coeline[2] + ", " + coeline[3] + ", " + coeline[4] + ", " + coeline[5] + ", " + coeline[6] + "]" + ",");
                    }
                    if (coeline.Count() == 8)
                    {
                        streamWriter.Write("[" + coeline[0] + ", " + coeline[1] + ", " + coeline[2] + ", " + coeline[3] + ", " + coeline[4] + ", " + coeline[5] + ", " + coeline[6] + ", " + coeline[7] + "]" + ",");
                    }


                    //  integration constants
                    string integrationConstants = concantCoef.Substring(128, 32);
                    int firstConstant_e = integrationConstants.IndexOf('e');
                    int secondConstant_e = integrationConstants.IndexOf('e', firstConstant_e);
                    string firstIntegrate = integrationConstants.Substring(0, firstConstant_e + 4);
                    string secondIntegrate = integrationConstants.Substring(secondConstant_e + 4, firstConstant_e + 4);
                    string integrateConcant = firstIntegrate + " " + secondIntegrate;
                    integrateConcant = integrateConcant.Trim();
                    integrateConcant = integrateConcant.Replace(" ", ",");
                    integrateConcant = integrateConcant.Replace(",,", ",");
                    string[] integrationLine = integrateConcant.Split(comma);

                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");
                    streamWriter.Write("\"" + "integrationConstants" + "\"" + ":");
                    streamWriter.Write("[" + integrationLine[0] + ", " + integrationLine[1] + "]");
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t\t\t");

                    if (integrationLine[1].Length == 0)
                    {
                        int sun = 99;
                    }

                    // if this is last loop dont print comma
                    if (i < t_intervalValue - 1)
                    {
                        streamWriter.WriteLine("}" + ",");
                    }
                    else if (i >= t_intervalValue - 1)
                    {
                        streamWriter.Write("}");
                    }

                    //m_currentLine = streamReader.ReadLine();

                }
            }
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
            //streamWriter.WriteLine();
            streamWriter.Write("\t\t");
            streamWriter.Write("{");
            //------------------------------------------------
            PrintSpeciesAndDescription();
            m_currentLine = streamReader.ReadLine();
            t_intervalValue = PrintTintervalsLine();
            if (t_intervalValue > 0 )
            {
                //int xx = 99;
                PrintDataRecords();
            }
            else if (t_intervalValue == 0 && m_specieValue >=1)
            {
                DoRecordSet();
            }

            

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
            //streamWriter.WriteLine();
            //streamWriter.Write("\t\t\t\t");
            //streamWriter.Write("  {");

            streamWriter.WriteLine();
            //streamWriter.Write("\t\t\t\t\t");
            //streamWriter.Write("\"" + "stuff" + "\"" + ":" + "1.0");
            DoRecordSet();

            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t\t");
            //streamWriter.Write("  }");
            //streamWriter.WriteLine();
            //streamWriter.Write("\t\t\t\t");
            streamWriter.Write("]");
        }

        private static int PrintTintervalsLine()
        {
            t_intervalValue = 0;
            //m_specieValue = 0;
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
                int.TryParse(m_tIntervals, out t_intervalValue);
                streamWriter.Write(t_intervalValue + " ,");
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
            streamWriter.Write("\"" + m_formulaLabel + "\"" + ":");
            string chemFormulaSubstring = m_currentLine.Substring(9, 41);
            chemFormulaSubstring = chemFormulaSubstring.Trim();
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
            if (firstElement.Length > 1)
            {
                string fchar = firstElement.Substring(0, 1);
                string schar = firstElement.Substring(1);
                schar = schar.ToLower();
                firstElement = fchar + schar;
            }
            string firstAtoms = chemFormulaSubstring.Substring(firstColumn, secondColumn);

            string secondElement = chemFormulaSubstring.Substring(firstColumn + secondColumn, thirdColumn);
            secondElement = secondElement.Trim();
            if (secondElement.Length > 1)
            {
                string fchar = secondElement.Substring(0, 1);
                string schar = secondElement.Substring(1);
                schar = schar.ToLower();
                secondElement = fchar + schar;
            }
            int l_secondElement = secondElement.Length;
            string secondAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn, fourthColumn);
            double n_secondAtoms = 0.0;
            double.TryParse(secondAtoms, out n_secondAtoms);

            string thirdElement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn, fifthColumn);
            thirdElement = thirdElement.Trim();
            if (thirdElement.Length > 1)
            {
                string fchar = thirdElement.Substring(0, 1);
                string schar = thirdElement.Substring(1);
                schar = schar.ToLower();
                thirdElement = fchar + schar;
            }
            int l_thirdElement = thirdElement.Length;
            string thirdAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn, sixthColumn);
            double n_thirdAtoms = 0.0;
            double.TryParse(thirdAtoms, out n_thirdAtoms);

            string fourthElement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn, seventhColumn);
            fourthElement = fourthElement.Trim();
            if (fourthElement.Length > 1)
            {
                string fchar = fourthElement.Substring(0, 1);
                string schar = fourthElement.Substring(1);
                schar = schar.ToLower();
                fourthElement = fchar + schar;
            }
            int l_fourthElement = fourthElement.Length;
            string fourthAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn + seventhColumn, eigthColumn);
            double n_fourthAtoms = 0.0;
            double.TryParse(fourthAtoms, out n_fourthAtoms);

            string fifthelement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn + seventhColumn + eigthColumn, ninthColumn);
            fifthelement = fifthelement.Trim();
            if (fifthelement.Length > 1)
            {
                string fchar = fifthelement.Substring(0, 1);
                string schar = fifthelement.Substring(1);
                schar = schar.ToLower();
                fifthelement = fchar + schar;
            }
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


                if (n_thirdAtoms != 0)
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

                    if (n_fourthAtoms != 0)
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

                        if (n_fifthAtoms != 0)
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
            string speciesType = m_currentLine.Substring(51, 1);
            int.TryParse(speciesType, out m_specieValue);
            if (speciesType == "0")
            {
                streamWriter.Write("\"" + "phase_value" + "\"" + ": ");
                streamWriter.Write(speciesType + ",");
            }
            else
            {
                streamWriter.Write("\"" + "phase_value" + "\"" + ": ");
                streamWriter.Write(speciesType + ",");
            }

            // molecular weight line
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            // todo fix for e species
            string moleWeightSubstring = m_currentLine.Substring(52, 14);
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
            return t_intervalValue;

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

            //  todo fix air line description
            m_species = m_currentLine.Substring(0, 15);
            m_species = m_species.Trim();
            m_description = m_currentLine.Substring(18, 62);
            m_description = m_description.Trim();
            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            streamWriter.Write("\"" + m_speciesLabel + "\"" + ": ");
            streamWriter.Write("\"" + m_species + "\"" + ",");

            streamWriter.WriteLine();
            streamWriter.Write("\t\t\t");
            streamWriter.Write("\"" + m_descriptionLabel + "\"" + ": ");
            streamWriter.Write("\"" + m_description + "\"" + ",");
        }
    }
}
