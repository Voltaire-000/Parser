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

            StreamReader elementReader = new StreamReader("..\\..\\zipElement.txt");
            StreamReader atomReader = new StreamReader("..\\..\\atomic.txt");
            StreamReader valenceReader = new StreamReader("..\\..\\valence.txt");

            //StreamWriter elementWriter = new StreamWriter("..\\..\\zipElement.txt");
            //StreamWriter atomWriter = new StreamWriter("..\\..\\zipAtomic.txt");
            //StreamWriter valenceWriter = new StreamWriter("..\\..\\zipValence.txt");
            StreamWriter zipWriter = new StreamWriter("..\\..\\elementAndAtomWeight.txt");

            //atomWriter.AutoFlush = true;

            while (!atomReader.EndOfStream)
            {
                char separator = ',';

                //char oldChar = '\'';
                //char newChar = '"';

                //atomWriter.NewLine = ",";


                char rrr = '\r';
                char nnn = '\n';
                string m_elements = elementReader.ReadToEnd();
                m_elements = m_elements.Replace(rrr, ' ');
                m_elements = m_elements.Replace(nnn, ' ');
                m_elements = m_elements.Replace("       ", "");


                char oldChar = 'D';
                char newChar = 'e';
                string atoms = atomReader.ReadToEnd();
                atoms = atoms.Replace(oldChar, newChar);
                atoms = atoms.Replace(rrr, ' ');
                atoms = atoms.Replace(nnn, ' ');
                atoms = atoms.Replace("    ", "");

                string valence = valenceReader.ReadToEnd();
                string[] z_valence = valence.Split(separator);


                string[] z_elements = m_elements.Split(separator);
                for (int i = 0; i < z_elements.Length; i++)
                {
                    z_elements[i] = z_elements[i].ToString().Trim();
                }

                string[] z_atoms = atoms.Split(separator);
                for (int i = 0; i < z_atoms.Length; i++)
                {
                    z_atoms[i] = z_atoms[i].ToString().Trim();
                }

                var elementsZip = z_elements.Zip(z_atoms, (first, second) => first + " " + ":" + " " + second);
                foreach (var item in elementsZip)
                {
                    //zipWriter.Write(item + ", ");
                    //zipWriter.WriteLine();
                    //zipWriter.Flush();
                }

                var table = elementsZip.Zip(z_valence, (third, fourth) => third + " " + fourth);
                foreach (var item in table)
                {
                    zipWriter.Write(item + ",");
                    zipWriter.WriteLine();
                    zipWriter.Flush();

                }


            }
            atomReader.Close();
            elementReader.Close();
            valenceReader.Close();
                //atomWriter.Close();
            zipWriter.Close();

        }
    }
}
