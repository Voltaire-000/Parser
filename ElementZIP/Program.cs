using System.Globalization;
using System.IO;
using System.Linq;

namespace ElementZIP
{
    internal class Program
    {
        static void Main(string[] args)
        {

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

                streamWriter.NewLine = ",";
                string m_elements = streamReader.ReadToEnd();
                m_elements= m_elements.Trim();
                m_elements =  m_elements.Replace(oldChar, newChar);
                string[] z_elements = m_elements.Split(separator);
                

                string mnew = streamWriter.NewLine;
                for (int i = 0; i < z_elements.Length; i++)
                {
                    streamWriter.WriteLine(z_elements[i]);
                }
                

            }

        }
    }
}
