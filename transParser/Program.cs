using System.Globalization;
using System.IO;
using System.Linq;

namespace transParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StreamReader streamReader = new StreamReader("..\\..\\trans.inp");
            StreamWriter streamWriter = new StreamWriter("..\\..\\trans.json");
            streamWriter.AutoFlush = true;

            string m_currentLine = "";

            streamWriter.Write("{");
            streamWriter.WriteLine();
            streamWriter.Write("\t" + "\"" + "transport_property_coefficients" + "\"" + ":" + " " + "[");
            streamWriter.WriteLine();
            //streamWriter.Write("\t\t\t\t\t" + "{");

            while (!streamReader.EndOfStream)
            {
                m_currentLine = streamReader.ReadLine();

                // skip over first line
                if (m_currentLine.First() == 't')
                {
                    m_currentLine = streamReader.ReadLine();
                }

                streamWriter.Write("\t\t\t\t");
                streamWriter.Write("{");
                streamWriter.WriteLine();

                string m_speciesName = m_currentLine.Substring(0, 15);
                m_speciesName = m_speciesName.Trim();
                streamWriter.Write("\t\t\t\t\t");
                streamWriter.Write("\"" + m_speciesName + "\"" + ":");
                streamWriter.WriteLine();
                streamWriter.Write("\t\t\t\t\t");
                streamWriter.Write("\"" + "description" + "\"");

            }
                streamReader.Close();
                streamWriter.Close();

        }
    }
}
