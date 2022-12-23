﻿using System;
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

            int[] fieldWidths = { 6, 0, 2, 0 };
            parser.SetFieldWidths(fieldWidths);
            parser.IgnoreBlankLines = false;
            parser.TrimWhiteSpace = false;
            int[] m_width = parser.GetFieldWidths();
            long m_linenumber = parser.LineNumber;
            parser.Read();
            //parser.ReadLine();
           
            TextFields m_thermo =  parser.ReadFields();
            string mstr = m_thermo.ToString();
            parser.Read();
            TextFields m_name = parser.ReadFields();

            Console.WriteLine(  "Hello world");
            Console.Read();
        }
    }
}
