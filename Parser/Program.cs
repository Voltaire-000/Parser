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
             
            StreamWriter streamWriter = new StreamWriter("..\\..\\z_json.json");
            //string path = "..\\..\\mJson.json";
            //FileMode fileMode = FileMode.CreateNew;

            //FileStream fs = new FileStream(path, fileMode);
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
            string mm_name = "\"" + m_name.ToString() + "\"" + ",";
            string nameField = "\t\t\t\"" + "name" + "\"" + ":" + " ";
            string descriptionField = "\n\t\t\t\"" + "description" + "\"" + ":" + " ";
            
            parser.SetFieldWidths(24);
            TextFields m_descr = parser.ReadFields();

            Console.WriteLine(  m_thermo);

            streamWriter.WriteLine("{");
            streamWriter.Write("\t");
            streamWriter.Write("\"");
            streamWriter.Write(m_thermo);
            streamWriter.Write("\"");
            streamWriter.Write(":");
            streamWriter.WriteLine(" [");
            streamWriter.WriteLine("\t\t{");
            streamWriter.Write(nameField);
            streamWriter.Write(mm_name);
            streamWriter.Write(descriptionField);
            streamWriter.Close();
            Console.Read();
        }
    }
}
