using System;
using System.Collections.Generic;
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
            
            string path = "..\\..\\mJson.json";
            FileMode fileMode = FileMode.CreateNew;

            FileStream fs = new FileStream(path, fileMode);
            //fs.
            //TextWriter textWriter;
            //textWriter.
            TextFileParsers.FixedWidthFieldParser parser = new TextFileParsers.FixedWidthFieldParser("..\\..\\thermo.inp");
            TextFileParsers.DelimitedFieldParser delimitedFieldParser = new DelimitedFieldParser("..\\..\\thermo.inp");

            //int[] fieldWidths = { 6, 0, 2, 0 };
            //parser.SetFieldWidths(fieldWidths);
            //parser.IgnoreBlankLines = false;
            //parser.TrimWhiteSpace = false;
            //int[] m_width = parser.GetFieldWidths();
            //long m_linenumber = parser.LineNumber;
            //parser.Read();
            //parser.ReadLine();
            parser.SkipLine();
            parser.SetFieldWidths(6);
            TextFields m_thermo = parser.ReadFields();

            delimitedFieldParser.SetDelimiters(' ');
            delimitedFieldParser.SkipLines(2);
            delimitedFieldParser.SqueezeDelimiters = true;
            TextFields m_description = delimitedFieldParser.ReadFields();
            parser.SetFieldWidths(2);
            TextFields m_name = parser.ReadFields();
            
            parser.SetFieldWidths(24);
            TextFields m_descr = parser.ReadFields();

            Console.WriteLine(  m_thermo);
           
            Console.Read();
        }
    }
}
