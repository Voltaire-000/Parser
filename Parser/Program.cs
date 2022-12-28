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
        private static int intGlobalInterval = 0;

        static void Main(string[] args)
        {
            string m_thermo = "";
            string m_thermoFieldName = "thermo";
            string m_reactantFieldName = "reactant";
            string m_descriptionFieldName = "description";
            string m_tIntervalsFieldName = "t_intervals";
            string m_optionalIdFieldName = "id_code";
            string m_chemformulaFieldName = "chemicalFormula";
            string m_speciesTypeFieldName = "gaseous";
            string m_molecularWeightFieldName = "molecularWeight";
            string m_heatOfFormationFieldName = "heatOfFormation";
            string m_temperatureRangeFieldName = "temperatureRange";
            string m_specialCase = "temperatureRange";
            string x_temperatureRangeFieldName = "\"" + m_specialCase + "\"" + ":" + "{";
            string m_numberOfcoefficientsFieldName = "numberOfCoefficients";
            string m_tExponentsFieldName = "tExponents";
            string m_HlineJmolFieldName = "H^(298.15)-H^(0) J/mol";
            string m_CoefficientsFieldName = "coefficients";
            string m_integrationConstantsFieldName = "integrationConstants";
            string m_range = "range_";

            int count = 0;
            int m_peek = 0;
            string m_char = "";
            UnicodeCategory unicodeCategory;
            bool openBracketPrinted = false;
            bool thermoLineDone = false;
            bool m_tIntervalsIsZero = false;
            string m_currentLine = "";

            //StreamWriter streamWriter = new StreamWriter("..\\..\\thermoINPjson.json");
            StreamWriter streamWriter = new StreamWriter("..\\..\\shortThermo.json");
            streamWriter.AutoFlush = true;

            //StreamReader streamReader = new StreamReader("..\\..\\thermo.inp");
            StreamReader streamReader = new StreamReader("..\\..\\shortthermo.inp");

            while (!streamReader.EndOfStream)
            {
                int numberOfTemperatureIntervals = 0;

                if (!thermoLineDone)
                {
                    // read until thermo
                    m_currentLine = streamReader.ReadLine();
                    thermoLineDone = PrintThermoLine(streamWriter, streamReader, m_currentLine, m_thermoFieldName);
                }

                m_currentLine = streamReader.ReadLine();
                bool m_newReactant = IsNewReactant(streamReader, m_currentLine);
                if (!m_newReactant)
                {
                    //  todo
                }
                else
                {
                    intGlobalInterval = 0;
                    // check for record line or end statement
                    // print open curly bracket, new line, and get reactant line
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t" + "{");
                    //m_currentLine= streamReader.ReadLine();
                    PrintReactantAndCommentsLine(streamWriter, streamReader, m_currentLine, m_reactantFieldName, m_descriptionFieldName);
                    m_currentLine = streamReader.ReadLine();
                    numberOfTemperatureIntervals = PrintTintervalsDataLine(streamWriter, streamReader, m_currentLine, m_tIntervalsFieldName, m_optionalIdFieldName, m_chemformulaFieldName, m_speciesTypeFieldName, m_molecularWeightFieldName, m_heatOfFormationFieldName);
                    if (numberOfTemperatureIntervals == 0)
                    {
                        m_tIntervalsIsZero = true;
                        // special case for temperature intervals == 0
                        // just print temp range line and move on
                        for (int i = 0; i < 1; i++)
                        {
                            m_currentLine = streamReader.ReadLine();

                            string ZeroTemprange = m_currentLine.Substring(0, 22);
                            string ZeroNumberCoeff = m_currentLine.Substring(22, 3);
                            string ZeroTexponents8 = m_currentLine.Substring(23, 40);
                            string ZeroHline = m_currentLine.Substring(75, 5);
                            streamWriter.WriteLine();
                            PrintTemperatureRange(streamWriter, m_currentLine, m_temperatureRangeFieldName);
                            PrintNumberOfCoefficients(streamWriter, m_currentLine, m_numberOfcoefficientsFieldName);
                            PrintTexponentsArray(streamWriter, m_currentLine, m_tExponentsFieldName);
                            PrintZeroHline(streamWriter, m_currentLine, m_HlineJmolFieldName, m_tIntervalsIsZero);

                        }

                        m_tIntervalsIsZero = false;
         
                    }

                    //x_temperatureRangeFieldName = AddQuotesSemiColonAndOpenBracket(x_temperatureRangeFieldName);
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t\t");
                    streamWriter.Write(x_temperatureRangeFieldName);
                    for (int i = 0; i < numberOfTemperatureIntervals; i++)
                    {
                        m_currentLine = streamReader.ReadLine();
                        int c = i + 1;
                        //m_range = m_range + c.ToString();

                        streamWriter.WriteLine();
                        streamWriter.Write("\t\t\t\t\t");
                        //m_range = AddQuotesSemiColonAndOpenBracket(m_range);
                        streamWriter.WriteLine( "\"" + m_range + c.ToString() + "\"" + ":" + "{");
                        //streamWriter.WriteLine();
                        //streamWriter.Write("\t\t\t\t\t\t");

                        PrintTemperatureRange(streamWriter, m_currentLine, m_temperatureRangeFieldName);
                        PrintNumberOfCoefficients(streamWriter, m_currentLine, m_numberOfcoefficientsFieldName);
                        PrintTexponentsArray(streamWriter, m_currentLine, m_tExponentsFieldName);
                        PrintH_line(streamWriter, m_currentLine, m_HlineJmolFieldName);
                        PrintCoeffAndIntegrationConstants(streamWriter, streamReader, m_currentLine, m_CoefficientsFieldName, m_integrationConstantsFieldName, numberOfTemperatureIntervals);
                        //  print closing curly brace for range
                    }

                    // print the close curly bracket for reactant
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    streamWriter.WriteLine("}" + ",");

                    if (streamReader.Peek() == -1)
                    {
                        streamWriter.Write("]");
                        streamReader.Close();
                    }

                }

            }
            

        }

        private static void PrintZeroHline(StreamWriter writer, string line, string fieldName, bool intervals)
        {
            string m_Hline = line.Substring(66, 14);
            m_Hline = m_Hline.Trim();
            writer.WriteLine();
            if (intervals)
            {
                writer.Write("\t\t\t\t");
            }
            writer.Write("\t\t");
            fieldName = AddQuotesAndSemicolon(fieldName);
            writer.Write(fieldName);
            writer.Write(" " + m_Hline);
        }

        private static int PrintTintervalsDataLine(StreamWriter writer, StreamReader reader, string line, string fieldName1, string fieldName2, string fieldName3, string fieldName4, string fieldName5, string fieldName6)
        {
            char separator = ' ';
            writer.WriteLine();
            writer.Write("\t\t");
            // t intervals
            fieldName1 = AddQuotesAndSemicolon(fieldName1);
            writer.Write(fieldName1);
            string intervalsSubstring = line.Substring(0, 2);
            int.TryParse(intervalsSubstring, out int value);
            writer.Write(intervalsSubstring + ",");

            // id code
            writer.WriteLine();
            writer.Write("\t\t");
            fieldName2 = AddQuotesAndSemicolon(fieldName2);
            writer.Write(fieldName2);
            string idCodeSubstring = line.Substring(3, 7);
            idCodeSubstring = idCodeSubstring.Trim();
            idCodeSubstring = AddQuotesAndComma(idCodeSubstring);
            writer.Write(idCodeSubstring);

            //  chemical formula line
            writer.WriteLine();
            writer.Write("\t\t");
            fieldName3 = AddQuotesSemiColonAndOpenBracket(fieldName3);
            writer.Write(fieldName3);
            string chemFormulaSubstring = line.Substring(9, 41);
            chemFormulaSubstring = chemFormulaSubstring.Trim();
            //UnicodeCategory.DecimalDigitNumber
            char[] firstSpace = chemFormulaSubstring.ToCharArray();
            char zz = 'A';
            char xx = 'G';
            string vv = zz.ToString() + xx.ToString();
            int formulalength = chemFormulaSubstring.Length;
            string[] formulaLine = chemFormulaSubstring.Split(separator);
            int formulaLineLength = formulaLine.Length;

            char[] anyof = { '9', '8', '7', '6', '5', '4', '3', '2', '1' };
            //var eew = chemFormulaSubstring.IndexOf((char)49);

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

            string thirdElement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn, fifthColumn);
            thirdElement = thirdElement.Trim();
            int l_thirdElement = thirdElement.Length;
            string thirdAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn, sixthColumn);

            string fourthElement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn, seventhColumn);
            fourthElement = fourthElement.Trim();
            int l_fourthElement = fourthElement.Length;
            string fourthAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn + seventhColumn, eigthColumn);

            string fifthelement = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn + seventhColumn + eigthColumn, ninthColumn);
            fifthelement= fifthelement.Trim();
            int l_fifthElement = fifthelement.Length;
            string fifthAtoms = chemFormulaSubstring.Substring(firstColumn + secondColumn + thirdColumn + fourthColumn + fifthColumn + sixthColumn + seventhColumn + eigthColumn + ninthColumn, tenthColumn);

            writer.WriteLine();
            writer.Write("\t\t\t\t\t");
            firstElement = AddQuotesAndSemicolon(firstElement);
            writer.Write(firstElement);

            if (l_secondElement > 0)
            {
                writer.Write(firstAtoms + ","); 
            }
            else
            {
                writer.Write(firstAtoms);
            }

            if (l_secondElement > 0)
            {
                writer.WriteLine();
                writer.Write("\t\t\t\t\t");
                secondElement = AddQuotesAndSemicolon(secondElement);
                writer.Write(secondElement);

                if (l_thirdElement > 0)
                {
                    writer.Write(secondAtoms + ","); 
                }
                else
                {
                    writer.Write(secondAtoms);
                }

                if (l_thirdElement > 0)
                {
                    writer.WriteLine();
                    writer.Write("\t\t\t\t\t");
                    thirdElement = AddQuotesAndSemicolon(thirdElement);
                    writer.Write(thirdElement);

                    if (l_fourthElement > 0)
                    {
                        writer.Write(thirdAtoms + ","); 
                    }
                    else
                    {
                        writer.Write(thirdAtoms);
                    }

                    if (l_fourthElement > 0)
                    {
                        writer.WriteLine();
                        writer.Write("\t\t\t\t\t");
                        fourthElement = AddQuotesAndSemicolon(fourthElement);
                        writer.Write(fourthElement);

                        if (l_fifthElement > 0)
                        {
                            writer.Write(fourthAtoms + ","); 
                        }
                        else
                        {
                            writer.Write(fourthAtoms);
                        }

                        if (l_fifthElement > 0)
                        {
                            writer.WriteLine();
                            writer.Write("\t\t\t\t\t");
                            fifthelement = AddQuotesAndSemicolon(fifthelement);
                            writer.Write(fifthelement);
                            writer.Write(fifthAtoms);  
                        } 
                    } 
                }
            }

            writer.WriteLine();
            writer.Write("\t\t\t\t\t\t");
            writer.Write("}" + ",");

            // gas species line
            writer.WriteLine();
            writer.Write("\t\t");
            string speciesType = line.Substring(41, 1);
            fieldName4 = AddQuotesAndSemicolon(fieldName4);
            writer.Write(fieldName4);
            if (speciesType == "0")
            {
                writer.Write(" false" + ",");
            }
            else
            {
                writer.Write(" true" + ",");
            }

            //  molecular weight line
            writer.WriteLine();
            writer.Write("\t\t");
            string m_molecularWeight = line.Substring(54, 11);
            fieldName5 = AddQuotesAndSemicolon(fieldName5);
            writer.Write(fieldName5);
            writer.Write(" " + m_molecularWeight + ",");

            //  heat of formation line
            writer.WriteLine();
            writer.Write("\t\t");
            string m_heatOfFormation = line.Substring(65, 15);
            m_heatOfFormation = m_heatOfFormation.Trim();
            fieldName6 = AddQuotesAndSemicolon(fieldName6);
            writer.Write(fieldName6);
            writer.Write(" " + m_heatOfFormation + ",");
            //  end record start read new line
            return value;
        }

        private static void PrintReactantAndCommentsLine(StreamWriter writer, StreamReader reader, string line, string fieldName1, string fieldName2)
        {
            writer.WriteLine();
            writer.Write("\t\t");
            fieldName1 = AddQuotesAndSemicolon(fieldName1);
            writer.Write(fieldName1);
            string formulaString = line.Substring(0, 18);
            formulaString = formulaString.Trim();
            formulaString = AddQuotesAndComma(formulaString);
            writer.Write(formulaString);
            writer.WriteLine();
            writer.Write("\t\t");
            fieldName2 = AddQuotesAndSemicolon(fieldName2);
            writer.Write(fieldName2);
            string descriptionSubstring = line.Substring(18);
            descriptionSubstring = descriptionSubstring.Trim();
            descriptionSubstring = AddQuotesAndComma(descriptionSubstring);
            writer.Write(descriptionSubstring);

        }

        private static bool PrintThermoLine(StreamWriter writer, StreamReader reader, string line, string fieldName)
        {
            fieldName = AddQuotesAndSemicolon(fieldName);
            writer.WriteLine("{");
            writer.Write(fieldName + "[");
            return true;
        }

        private static bool IsNewReactant(StreamReader reader, string line)
        {
            //bool thisIsNewReactant = false;
            //int recordPeek = reader.Peek();
            char firstCharacter = line.First();
            UnicodeCategory unicodeCategory;
            unicodeCategory = char.GetUnicodeCategory(firstCharacter);
            if (unicodeCategory != UnicodeCategory.UppercaseLetter && unicodeCategory != UnicodeCategory.OpenPunctuation)
            {
                // we are at end of file
                reader.Close();
            }
            else
            {
                // we have a new record, check for END
                //string checkForEND = "END";
                // check if END
                bool endRecord = EndRecord(line);
                if (!endRecord)
                {
                    // start new record
                    //reader.ReadLine();
                    return true;
                }
                else
                {
                    // end of file print close curly brace, close  bracket and final close curly brace
                    //streamWriter.WriteLine();
                    ////streamWriter.WriteLine("\t\t}");
                    //streamWriter.WriteLine("\t]");
                    //streamWriter.WriteLine("}");
                    return false;
                }

            }

            return false;
        }

        private static bool EndRecord(string m_currentLine)
        {
            //bool endRecord = false;
            string endSubstring = m_currentLine.Substring(0, 3);
            //endSubstring = "END";
            if (endSubstring == "END")
            {
                return true;
            }
            return false;
        }

        // unused
        private static void PrintIntegrationConstants(StreamWriter writer, StreamReader reader, string line, string fieldName)
        {
            // new record line
            writer.WriteLine();
            writer.Write("\t\t");
            writer.Write(fieldName + "[");
            char separator = ' ';
            Stream s = reader.BaseStream;
            line = reader.ReadLine();

            string mLineContinue = reader.ReadLine();
            //  concant the lines m_current and mLineContinue
            //string concantLine = line + mLineContinue;
            //concantLine = concantLine.Replace('D', 'e');
            // (129,31)
            //string m_integrationSubstring = concantLine.Substring(129, 31);

        }

        private static void PrintCoeffAndIntegrationConstants(StreamWriter writer, StreamReader reader, string line, string fieldName1, string fieldName2, int intervals)
        {
            // new record line
            writer.WriteLine();
            writer.Write("\t\t\t\t\t\t");
            fieldName1 = AddQuotesAndSemicolon(fieldName1);
            writer.Write(fieldName1 + "[");
            char separator = ' ';
            Stream stream = reader.BaseStream;
            line = reader.ReadLine();
            Stream stream1 = reader.BaseStream;
            string mLineContinue = reader.ReadLine();
            //  concant the lines m_current and mLineContinue
            string concantLine = line + mLineContinue;
            concantLine = concantLine.Replace('D', 'e');
            string coefficientSubstring = concantLine.Substring(0, 128);

            int firstE = coefficientSubstring.IndexOf('e');
            int secondE = coefficientSubstring.IndexOf('e', firstE);
            int thirdE = coefficientSubstring.IndexOf('e', secondE);
            int forthE = coefficientSubstring.IndexOf('e', thirdE);
            int fifthE = coefficientSubstring.IndexOf('e', forthE);
            int sixthE = coefficientSubstring.IndexOf('e', fifthE);
            int seventhE = coefficientSubstring.IndexOf('e', sixthE);
            int eighthE = coefficientSubstring.IndexOf('e', seventhE);

            string firstCoef = coefficientSubstring.Substring(0, firstE + 4);
            string secondCoef = coefficientSubstring.Substring(secondE + 4, firstE + 4);
            string thirdCoef = coefficientSubstring.Substring(thirdE + 4 + secondE + 4, firstE + 4);
            string forthCoef = coefficientSubstring.Substring(forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);
            string fifthCoef = coefficientSubstring.Substring(fifthE + 4 + forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);
            string sixthCoef = coefficientSubstring.Substring(sixthE + 4 + fifthE + 4 + forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);
            string seventhCoef = coefficientSubstring.Substring(seventhE + 4 + sixthE + 4 + fifthE + 4 + forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);
            string eighthCoef = coefficientSubstring.Substring(eighthE + 4 + seventhE + 4 + sixthE + 4 + fifthE + 4 + forthE + 4 + thirdE + 4 + secondE + 4, firstE + 4);

            string CoefConcant = firstCoef + " " + secondCoef + " " + thirdCoef + " " + forthCoef + " " + fifthCoef + " " + sixthCoef + " " + seventhCoef + " " + eighthCoef;
            CoefConcant = CoefConcant.Trim();

            int m_coefficientCount = 0;

            string[] coefficientLine = CoefConcant.Split(separator);
            int m_coefficientLineLength = coefficientLine.Length;
            int spaceSkip = 0;
            foreach (var coefficient in coefficientLine)
            {
                m_coefficientCount = m_coefficientCount + 1;
                if (coefficient == "")
                {
                    spaceSkip = spaceSkip + 1;
                    continue;
                }
                if (m_coefficientCount <= m_coefficientLineLength - 1)
                {
                    //writer.Write(coefficient.Remove(11, 4) + ", ");
                    writer.Write(coefficient + ", ");
                }
                else if (m_coefficientCount >= m_coefficientLineLength)
                {
                    //writer.Write(coefficient.Remove(11, 4) + "]" + ", ");
                    writer.Write(coefficient + "]" + ", ");
                }
            }

            //  done with Coefficients, do integration constants
            // new record line
            writer.WriteLine();
            writer.Write("\t\t\t\t\t\t");
            fieldName2 = AddQuotesAndSemicolon(fieldName2);
            writer.Write(fieldName2 + "[");

            int m_integrationCount = 0;
             
            string integrationConstants = concantLine.Substring(128, 32);
            int firstConstant_e = integrationConstants.IndexOf('e');
            int secondConstant_e = integrationConstants.IndexOf('e', firstConstant_e);
            string firstIntegrate = integrationConstants.Substring(0, firstConstant_e + 4);
            string secondIntegrate = integrationConstants.Substring(secondConstant_e + 4, firstConstant_e + 4);
            string integrateConcant = firstIntegrate + " " + secondIntegrate;
            integrateConcant = integrateConcant.Trim();

            string[] integrationLine = integrateConcant.Split(separator);
            int m_integrationLineLength = integrationLine.Length;
            int spaceIntegrateSkip = 0;

            foreach (var integrateNum in integrationLine)
            {

                m_integrationCount = m_integrationCount + 1;
                if (integrateNum == "")
                {
                    spaceIntegrateSkip = spaceIntegrateSkip + 1;
                    continue;
                }
                if (m_integrationCount <= m_integrationLineLength - 1)
                {
                    writer.Write(integrateNum + ", ");
                }
                else if (m_integrationCount >= m_integrationLineLength)
                {
                    if (intGlobalInterval != intervals - 1)
                    {
                        writer.Write(integrateNum + "]");
                        writer.WriteLine();
                        writer.Write("\t\t\t\t\t\t\t" + "}" + ",");
                    }
                    else
                    {
                        writer.Write(integrateNum + "]");
                        writer.WriteLine();
                        writer.Write("\t\t\t\t\t\t\t" + "}");
                        writer.WriteLine();
                        writer.Write("\t\t\t\t\t\t" + "}");
                    }

                    intGlobalInterval = intGlobalInterval + 1;
                }
            }


        }

        private static void PrintH_line(StreamWriter writer, string line, string fieldName)
        {
            string m_Hline = line.Substring(66, 14);
            m_Hline = m_Hline.Trim();
            writer.WriteLine();
            writer.Write("\t\t\t\t\t\t");
            fieldName = AddQuotesAndSemicolon(fieldName);
            writer.Write(fieldName);
            writer.Write(" " + m_Hline + ",");
        }

        private static void PrintTexponentsArray(StreamWriter writer, string line, string fieldName)
        {
            char separator = ' ';
            writer.WriteLine();
            writer.Write("\t\t\t\t\t\t");
            string m_tExponents = line.Substring(23, 40);
            fieldName = AddQuotesAndSemicolon(fieldName);
            writer.Write(fieldName + "[");
            string[] tExponentLine = m_tExponents.Split(separator);
            int tExponentCount = 0;
            int spaceSkip = 0;
            int tExpLineLength = tExponentLine.Length;
            foreach (var exponent in tExponentLine)
            {
                tExponentCount = tExponentCount + 1;
                if (exponent == "")
                {
                    spaceSkip = spaceSkip + 1;
                    continue;
                }
                if (tExponentCount <= tExpLineLength - 1)
                {
                    writer.Write(exponent + ",");
                }
                else if (tExponentCount >= tExpLineLength)
                {
                    writer.Write(exponent + "]" + ",");
                }

            }
        }

        private static void PrintNumberOfCoefficients(StreamWriter writer, string line, string fieldName)
        {
            writer.WriteLine();
            writer.Write("\t\t\t\t\t\t");
            // number of Coefficients column 23
            string m_coeff = line.Substring(22, 1);
            fieldName = AddQuotesAndSemicolon(fieldName);
            writer.Write(fieldName);
            writer.Write(" " + m_coeff + ",");
        }

        private static void PrintTemperatureRange(StreamWriter writer, string line, string fieldName)
        {
            char separator = ' ';
            //writer.WriteLine();
            writer.Write("\t\t\t\t\t\t");
            string temp_range = line.Substring(0, 22);
            temp_range = temp_range.Trim();
            fieldName = AddQuotesAndSemicolon(fieldName);
            writer.Write(fieldName + "[");
            string[] tempRangeLine = temp_range.Split(separator);
            int tempRangeCount = 0;
            int tempRangeLineLength = tempRangeLine.Length;
            foreach (var tempItem in tempRangeLine)
            {
                tempRangeCount = tempRangeCount + 1;
                if (tempItem == "")
                {
                    continue;
                }
                // add the quotes
                string m_temp = tempItem.ToString();
                if (tempRangeCount <= tempRangeLineLength - 1)
                {
                    //m_temp = addQuotesAndComma(m_temp);
                    writer.Write(m_temp + ", ");
                }
                else if (tempRangeCount >= tempRangeLineLength)
                {
                    //m_temp  = addQuotesAndComma(m_temp);
                    writer.Write(m_temp + "]" + ", ");
                }

            }
        }

        private static string AddQuotesAndSemicolon(string fieldName)
        {
            fieldName = "\t\"" + fieldName + "\"" + ":" + "";
            return fieldName;
        }

        private static string AddQuotesSemiColonAndOpenBracket(string fieldName)
        {
            fieldName = "\t\"" + fieldName + "\"" + ":" + "{";
            return fieldName;
        }

        private static string AddQuotesAndComma(string fieldName)
        {
            fieldName = "\t\"" + fieldName.Trim() + "\"" + ",";
            return fieldName;
        }
        private static string AddQuotes(string fieldName)
        {
            fieldName = "\"" + fieldName + "\"";
            return fieldName;
        }

        private static string JsonStart(string m_line1, string m_thermo, string m_thermoField, StreamReader streamReader)
        {
            if (m_line1 == "")
            {
                string m_thermoLine = streamReader.ReadLine();
                m_thermo = m_thermoLine.Substring(0, 6);
                //m_thermoField = "\t\"" + m_thermo.ToString() + "\"";
                m_thermoField = AddQuotesAndSemicolon(m_thermoField);
            }
            return m_thermoField;
        }
    }
}
