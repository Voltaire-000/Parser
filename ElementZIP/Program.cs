using System.Globalization;
using System.IO;
using System.Linq;

namespace ElementZIP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string m_currentLine;
            int m_currentChar;
            
            StreamReader streamReader = new StreamReader("..\\..\\element.txt");
            //StreamReader streamReader = new StreamReader("..\\..\\atomic.txt");
            //StreamReader streamReader = new StreamReader("..\\..\\valence.txt");

            StreamWriter streamWriter = new StreamWriter("..\\..\\zipElement.txt");
            //StreamWriter streamWriter = new StreamWriter("..\\..\\zipAtomic.txt");
            //StreamWriter streamWriter = new StreamWriter("..\\..\\zipValence.txt");

            streamWriter.AutoFlush = true;

            while (!streamReader.EndOfStream)
            {
                char separator = ',';
                char oldChar = '\'';
                char newChar = '"';

                //m_currentLine = streamReader.ReadLine();
                string m_elements = streamReader.ReadToEnd();
                m_elements= m_elements.Trim();
                m_elements =  m_elements.Replace(oldChar, newChar);

            }

        }
    }
}
