using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ElementZIP
{
    internal class Program
    {
        static void Main(string[] args)
        {

            StreamReader elementReader = new StreamReader("..\\..\\zip_Element.txt");
            StreamReader atomReader = new StreamReader("..\\..\\atomic.txt");
            //StreamReader valenceReader = new StreamReader("..\\..\\valence.txt");

            //StreamWriter elementWriter = new StreamWriter("..\\..\\zipElement.txt");
            //StreamWriter atomWriter = new StreamWriter("..\\..\\zipAtomic.txt");
            //StreamWriter valenceWriter = new StreamWriter("..\\..\\zipValence.txt");

            //atomWriter.AutoFlush = true;

            while (!atomReader.EndOfStream)
            {
                char separator = ',';
                char oldChar = '\'';
                char newChar = '"';

                //atomWriter.NewLine = ",";


                string m_elements = elementReader.ReadToEnd();
                string[] z_elements = m_elements.Split(separator);

                string atoms = atomReader.ReadToEnd();
                string[] z_atoms = atoms.Split(separator);

                var elementsZip = z_elements.Zip(z_atoms, (first, second) => first + " " + second);


            }
                atomReader.Close();
                //atomWriter.Close();

        }
    }
}
