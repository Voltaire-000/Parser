using System;
using System.Collections.Generic;
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
            TextFileParsers.FixedWidthFieldParser parser = new TextFileParsers.FixedWidthFieldParser("..\\..\\thermo.inp");

            int[] fieldWidths = { 6, 6 };
            parser.SetFieldWidths(fieldWidths);
            parser.IgnoreBlankLines = true;
            parser.TrimWhiteSpace = true;
            int[] m_width = parser.GetFieldWidths();
            long m_linenumber = parser.LineNumber;
            parser.ReadLine();
            TextFields tx =  parser.ReadFields();
            string mstr = tx.ToString();

            Console.WriteLine(  "Hello world");
            Console.Read();
        }
    }
}
