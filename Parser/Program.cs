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

            //int[] fieldWidths = [0, 6]
            parser.SetFieldWidths(6);
            parser.IgnoreBlankLines = true;
            TextFields tx =  parser.ReadFields();
            string mstr = tx.ToString();

            Console.WriteLine(  "Hello world");
            Console.Read();
        }
    }
}
