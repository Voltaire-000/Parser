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
            string m_numberOfcoefficientsFieldName = "numberOfCoefficients";
            string m_tExponentsFiledName = "tExponents";
            string m_HlineJmolFieldName = "H^(298.15)-H^(0) J/mol";
            string m_CoefficientsFieldName = "coefficients";
            string m_integrationConstantsFieldName = "integrationConstants";

            int count = 0;
            int m_peek = 0;
            string m_char = "";
            UnicodeCategory unicodeCategory;
            bool openBracketPrinted = false;
            string m_currentLine = "";

            StreamWriter streamWriter = new StreamWriter("..\\..\\z_json.json");
            StreamReader streamReader = new StreamReader("..\\..\\thermo.inp");

            while (!streamReader.EndOfStream)
            {
                //  print open bracket
                if (!openBracketPrinted)
                {
                    // we are at start of file
                    streamWriter.WriteLine("{");
                    openBracketPrinted = true;
                }

                m_currentLine = streamReader.ReadLine();

                //m_peek = streamReader.Peek();
                //m_peek != -1
                if (m_currentLine == "")
                {
                    m_currentLine = streamReader.ReadLine();
                }
                Char m_firstChar = m_currentLine.First();

                unicodeCategory = Char.GetUnicodeCategory(m_firstChar);

                if (unicodeCategory == UnicodeCategory.LowercaseLetter && m_firstChar == 't')
                {
                    // this is thermo line
                    Console.WriteLine("Thermo line");
                    m_thermoFieldName = JsonStart(m_currentLine, m_thermo, m_thermoFieldName, streamReader);
                    m_thermoFieldName = addQuotesAndSemicolon(m_thermoFieldName);
                    streamWriter.Write(m_thermoFieldName);
                    streamWriter.WriteLine("[");
                    streamWriter.WriteLine("\t\t{");
                }

                m_currentLine = streamReader.ReadLine();
                m_firstChar = m_currentLine.First();
                unicodeCategory = Char.GetUnicodeCategory(m_firstChar);

                if (unicodeCategory == UnicodeCategory.UppercaseLetter)
                {
                    //  this is a reactant field start of line. print reactant name to file
                    //string m_nextLine = streamReader.ReadLine();
                    char separator = ' ';
                    string[] m_line = m_currentLine.Split(separator);
                    
                    //  reactant line
                    m_reactantFieldName = addQuotesAndSemicolon(m_reactantFieldName);
                    string reactant = "\"" + m_line[0] + "\"" + ",";

                    //  description line comments
                    int startdescription = reactant.Length;
                    string m_description = m_currentLine.Substring(startdescription);
                    m_descriptionFieldName = addQuotesAndSemicolon(m_descriptionFieldName);
                    m_description = addQuotesAndComma(m_description);
                    
                    streamWriter.Write("\t\t");
                    streamWriter.Write(m_reactantFieldName + reactant);
                    //streamWriter.Write(reactant);
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    streamWriter.Write(m_descriptionFieldName + m_description);
                    //streamWriter.Write(m_description);
                    streamWriter.WriteLine();

                    //  new record line
                    //  T intervals line 
                    m_currentLine = streamReader.ReadLine();
                    string t_intervals = m_currentLine.Substring(0, 2);
                    m_tIntervalsFieldName = addQuotesAndSemicolon(m_tIntervalsFieldName);
                    streamWriter.Write("\t\t");
                    streamWriter.Write(m_tIntervalsFieldName + t_intervals);
                    //streamWriter.Write(t_intervals);
                    streamWriter.Write(",");

                    //  optional id continued from T intervals line
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_optionalIdFieldName = addQuotesAndSemicolon(m_optionalIdFieldName);
                    string optionalId = m_currentLine.Substring(3, 7);
                    optionalId= addQuotesAndComma(optionalId);
                    streamWriter.Write(m_optionalIdFieldName+ optionalId);

                    //  chemical formula
                    string chemFormula = m_currentLine.Substring(9, 41);
                    chemFormula = chemFormula.Trim();
                    char[] chemCharArray = chemFormula.ToCharArray();
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_chemformulaFieldName = addQuotesAndSemicolon(m_chemformulaFieldName);
                    streamWriter.Write(m_chemformulaFieldName + "[");
                    string[] formulaLine = chemFormula.Split(separator);
                    int formulaLineLength = formulaLine.Length;
                    int itemCount = 0;
                    string m_item = "";
                    foreach (var item in formulaLine)
                    {
                        itemCount = itemCount + 1;
                        int m_itemLength = item.Count();
                        // skip over spaces
                        if (item == "")
                        {
                            continue;
                        }
                        // add the quotes to formula name
                        if (itemCount < 2)
                        {
                            m_item = item.ToString();
                            m_item = addQuotesAndComma(m_item);
                            streamWriter.Write(m_item);
                        }

                        if (itemCount >= 2 && itemCount < formulaLineLength)
                        {
                            //m_item = addQuotesAndComma(m_item);
                            streamWriter.Write(formulaLine[itemCount - 1] + ",");
                        }
                        else if(itemCount >=formulaLineLength)
                        {
                            //m_item= addQuotes(m_item);
                            streamWriter.Write(formulaLine[itemCount -1] + "]" + ",");
                        }
                        
                    }

                    //  Gaseous species, true or false
                    string speciesType = m_currentLine.Substring(41, 1);
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_speciesTypeFieldName = addQuotesAndSemicolon(m_speciesTypeFieldName);
                    streamWriter.Write(m_speciesTypeFieldName);
                    if (speciesType == "0")
                    {
                        streamWriter.Write(" false" + ",");
                    }
                    else
                    {
                        streamWriter.Write(" true" + ",");
                    }

                    //  molecular weight
                    string molecularWeight = m_currentLine.Substring(54, 11);
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_molecularWeightFieldName = addQuotesAndSemicolon(m_molecularWeightFieldName);
                    streamWriter.Write(m_molecularWeightFieldName);
                    streamWriter.Write( " " + molecularWeight + ",");

                    //  heat of formation
                    string heatOfFormation = m_currentLine.Substring(65, 15);
                    heatOfFormation = heatOfFormation.Trim();
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_heatOfFormationFieldName = addQuotesAndSemicolon(m_heatOfFormationFieldName);
                    streamWriter.Write(m_heatOfFormationFieldName);
                    streamWriter.Write(" " + heatOfFormation + ",");
                    //  end record line

                    //  new record line = temp range =column 2-21, number of coefficients = 23, T exponents = 24-63, H^ = 66-80
                    m_currentLine = streamReader.ReadLine();
                    string temp_range = m_currentLine.Substring(0, 22);
                    temp_range = temp_range.Trim();
                    //char[] tempRangeCharArray = temp_range.ToCharArray();
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_temperatureRangeFieldName = addQuotesAndSemicolon(m_temperatureRangeFieldName);
                    streamWriter.Write(m_temperatureRangeFieldName + "[");
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
                            streamWriter.Write(m_temp + ",");
                        }
                        else if (tempRangeCount >= tempRangeLineLength)
                        {
                            //m_temp  = addQuotesAndComma(m_temp);
                            streamWriter.Write(m_temp + "]" + ",");
                        }

                    }
                    // number of Coefficients column 23
                    string m_coeff = m_currentLine.Substring(22, 1);
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_numberOfcoefficientsFieldName = addQuotesAndSemicolon(m_numberOfcoefficientsFieldName);
                    streamWriter.Write(m_numberOfcoefficientsFieldName);
                    streamWriter.Write(m_coeff + ",");
                    //  T exponents line column 24-63,  38spaces
                    string m_tExponents = m_currentLine.Substring(23, 40);
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_tExponentsFiledName = addQuotesAndSemicolon(m_tExponentsFiledName);
                    streamWriter.Write(m_tExponentsFiledName + "[");
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
                        if (tExponentCount <= tExpLineLength -1)
                        {
                            streamWriter.Write(exponent + ",");
                        }
                        else if (tExponentCount >= tExpLineLength)
                        {
                            streamWriter.Write(exponent + "]" + ",");
                        }
                        
                    }

                    //  H line column 66-80
                    string m_Hline = m_currentLine.Substring(66, 14);
                    m_Hline = m_Hline.Trim();
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_HlineJmolFieldName = addQuotesAndSemicolon(m_HlineJmolFieldName);
                    streamWriter.Write(m_HlineJmolFieldName);
                    streamWriter.Write(" " + m_Hline + ",");
                    //  end record line

                    //  new record line, line of Coefficients 1-80 ( first 5 coefficients), put all on 1 line
                    m_currentLine = streamReader.ReadLine();
                    string mLineContinue = streamReader.ReadLine();
                    //  concant the lines m_current and mLineContinue
                    string concantLine = m_currentLine + mLineContinue;
                    string coefficientSubstring = concantLine.Substring(0, 128);
                    string[] coefficientLine = coefficientSubstring.Split(separator);
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_CoefficientsFieldName = addQuotesAndSemicolon(m_CoefficientsFieldName);
                    streamWriter.Write(m_CoefficientsFieldName + "[");
                    string[] m_coefficientLine = coefficientSubstring.Split(separator);
                    int m_coefficientCount = 0;
                    int m_coefficientLineLength = m_coefficientLine.Length;
                    foreach (var coefficient in m_coefficientLine)
                    {
                        m_coefficientCount = m_coefficientCount + 1;
                        if (coefficient == "")
                        {
                            continue;
                        }
                        if (m_coefficientCount <= spaceSkip)
                        {
                            streamWriter.Write(coefficient.Remove(11,4) + ", ");
                        }
                        else if (m_coefficientCount > spaceSkip -1)
                        {
                            streamWriter.Write(coefficient.Remove(11,4) + "]" + ", ");
                        }
                    }
                    //  integration constants
                    streamWriter.WriteLine();
                    streamWriter.Write("\t\t");
                    m_integrationConstantsFieldName = addQuotesAndSemicolon(m_integrationConstantsFieldName);
                    streamWriter.Write(m_integrationConstantsFieldName + "[");
                    string m_integrationSubstring = concantLine.Substring(129, 31);
                    string[] integrationLine = m_integrationSubstring.Split(separator);
                    int integrationCount = 0;
                    int integrationLineLength = integrationLine.Length;
                    spaceSkip= 0;
                    foreach (var integrationConstant in integrationLine)
                    {
                        integrationCount = integrationCount + 1;
                        if (integrationConstant == "")
                        {
                            spaceSkip = spaceSkip + 1;
                            continue;
                        }
                        if (integrationCount <= spaceSkip)
                        {
                            streamWriter.Write(integrationConstant.Remove(11, 4) + ", ");
                        }
                        else if (integrationCount > spaceSkip -1)
                        {
                            streamWriter.Write(integrationConstant.Remove(11, 4) + "]" + ", ");
                        }
                    }


                    //  repeat tempRange and coefficients for each temp interval, t_intervals
                    //  convert string to integer
                    string t_intIntervals = t_intervals;
                    int intTempintervals;
                    int.TryParse(t_intIntervals, out intTempintervals);
                    for (int i = 0; i < intTempintervals; i++)
                    {
                        // repeat lines use function method
                    }

                    


                }
                
                m_firstChar = m_currentLine.First();
                unicodeCategory = Char.GetUnicodeCategory(m_firstChar);

                if (unicodeCategory == UnicodeCategory.SpaceSeparator)
                {
                    // print T Intervals

                }

                else
                {
                    //  we are at end of file. close the streamWriter
                    streamWriter.Close();
                }

                //Console.WriteLine(count);
                //count = count +1;

            }


            ////int startDescription = compoundName.Length;
            ////string m_description = m_nextLine.Substring(startDescription);
            ////m_description = "\"" + m_description + "\"" + ",";
            ////m_nextLine = streamReader.ReadLine();
            ////string t_intervals = m_nextLine.Substring(0, 2);

            ////string optionalId = m_nextLine.Substring(3, 9);
            ////Console.WriteLine(m_line[0]);


            //TextFileParsers.FixedWidthFieldParser parser = new TextFileParsers.FixedWidthFieldParser("..\\..\\thermo.inp");
            //TextFileParsers.DelimitedFieldParser delimitedFieldParser = new DelimitedFieldParser("..\\..\\thermo.inp");

            //parser.SkipLine();
            //parser.SetFieldWidths(6);

            //delimitedFieldParser.SetDelimiters(' ');
            //delimitedFieldParser.SkipLines(2);
            //delimitedFieldParser.SqueezeDelimiters = true;
            ////TextFields m_description = delimitedFieldParser.ReadFields();
            //parser.SetFieldWidths(2);
            //TextFields m_name = parser.ReadFields();

            //m_reactantFieldName = addQuotesAndSemicolon(m_reactantFieldName);
            //m_descriptionFieldName = addQuotesAndSemicolon(m_descriptionFieldName);
            //t_intervalsFieldName = addQuotesAndSemicolon(t_intervalsFieldName);

            ////string descriptionField = "\n\t\t\t\"" + "description" + "\"" + ":" + " ";
            ////string t_intervalField  = "\n\t\t\t\"" + "t_intervals"  + "\"" + ":" + "";

            //parser.SetFieldWidths(24);
            //TextFields m_descr = parser.ReadFields();

            //Console.WriteLine(" Thermo file to json");

            //streamWriter.Write("\t\t");
            //streamWriter.Write(m_descriptionFieldName);
            ////streamWriter.Write(m_description);
            //streamWriter.WriteLine();

            //streamWriter.Write("\t\t");
            //streamWriter.Write(t_intervalsFieldName);
            ////streamWriter.Write(t_intervals);
            //streamWriter.Close();
            //Console.Read();
        }

        private static string addQuotesAndSemicolon(string fieldName)
        {
            fieldName = "\t\"" + fieldName + "\"" + ":" + "";
            return fieldName;
        }

        private static string addQuotesAndComma(string fieldName)
        {
            fieldName = "\t\"" + fieldName.Trim() + "\"" + ",";
            return fieldName;
        }
        private static string addQuotes(string fieldName)
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
                m_thermoField = addQuotesAndSemicolon(m_thermoField);
            }
            return m_thermoField;
        }
    }
}
