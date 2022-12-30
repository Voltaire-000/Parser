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

            //StreamReader elementReader = new StreamReader("..\\..\\zipElement.txt");
            //StreamReader atomReader = new StreamReader("..\\..\\atomic.txt");
            //StreamReader valenceReader = new StreamReader("..\\..\\valence.txt");

            //StreamWriter elementWriter = new StreamWriter("..\\..\\zipElement.txt");
            //StreamWriter atomWriter = new StreamWriter("..\\..\\zipAtomic.txt");
            //StreamWriter valenceWriter = new StreamWriter("..\\..\\zipValence.txt");
            //StreamWriter zipWriter = new StreamWriter("..\\..\\elementAndAtomWeight.txt");

            StreamReader elementAndAtomReader = new StreamReader("..\\..\\elementAndAtomWeight.txt");
            StreamReader valenceReader = new StreamReader("..\\..\\zipValence.txt");

            StreamWriter tableElements = new StreamWriter("..\\..\\TableOfElements.json");
            tableElements.AutoFlush = true;

            string m_elementAndAtomLine = "";
            string m_valenceLine = "";

            //atomWriter.AutoFlush = true;

            tableElements.Write("{");
            tableElements.WriteLine();
            tableElements.WriteLine("\t" + "\"" + "TableOfElements" + "\"" + ":" + "[");
            tableElements.WriteLine("\t\t\t\t\t" + "{");

            while (!elementAndAtomReader.EndOfStream)
            {
                char separator = ',';



                //char oldChar = '\'';
                //char newChar = '"';

                //atomWriter.NewLine = ",";
                m_elementAndAtomLine = elementAndAtomReader.ReadLine();
                m_elementAndAtomLine = m_elementAndAtomLine.Replace(":", "\r" + "\t\t\t\t\t\t" + "\"" + "atomic_weight" + "\"" + ":");
                m_elementAndAtomLine = m_elementAndAtomLine.Replace("[", "");
                m_elementAndAtomLine = m_elementAndAtomLine.Replace(",", "," + "\r");


                m_valenceLine = valenceReader.ReadLine();

                m_valenceLine.Prepend(';');



                tableElements.Write("\t\t\t\t\t\t" + "\"" + "symbol" + "\"" + ":" + m_elementAndAtomLine);
                //tableElements.WriteLine("\t\t\t" + "\"" + ":" );
                tableElements.Write(m_valenceLine);
                //tableElements.Write(",");
                tableElements.WriteLine();
                //char rrr = '\r';
                //char nnn = '\n';
                //string m_elements = elementReader.ReadToEnd();
                //m_elements = m_elements.Replace(rrr, ' ');
                //m_elements = m_elements.Replace(nnn, ' ');
                //m_elements = m_elements.Replace("       ", "");


                //char oldChar = 'D';
                //char newChar = 'e';
                //string atoms = atomReader.ReadToEnd();
                //atoms = atoms.Replace(oldChar, newChar);
                //atoms = atoms.Replace(rrr, ' ');
                //atoms = atoms.Replace(nnn, ' ');
                //atoms = atoms.Replace("    ", "");

                //  Valence
                //string valence = valenceReader.ReadToEnd();
                //valence = valence.Replace('.', ' ');
                //valence = valence.Replace(rrr, ' ');
                //valence = valence.Replace(nnn, ' ');
                //string[] z_valence = valence.Split(separator);
                //valence = valence.Replace(rrr, ' ');
                //valence = valence.Replace(nnn, ' ');
                ////valence = valence.Replace("           ", "");
                //for (int i = 0; i < z_valence.Length; i++)
                //{
                //    z_valence[i] = z_valence[i].ToString().Trim();
                //}
                //foreach (var valenceItem in z_valence)
                //{
                //    valenceWriter.Write(valenceItem);
                //    valenceWriter.WriteLine();
                //    valenceWriter.Flush();
                //}



                //  Elements
                //string[] z_elements = m_elements.Split(separator);
                //for (int i = 0; i < z_elements.Length; i++)
                //{
                //    z_elements[i] = z_elements[i].ToString().Trim();
                //}

                ////  Atomic Weight
                //string[] z_atoms = atoms.Split(separator);
                //for (int i = 0; i < z_atoms.Length; i++)
                //{
                //    z_atoms[i] = z_atoms[i].ToString().Trim();
                //}

                //var elementsZip = z_elements.Zip(z_atoms, (first, second) => first + " " + ":" + " " + "[" + second);
                //foreach (var item in elementsZip)
                //{
                //    zipWriter.Write(item + ", ");
                //    zipWriter.WriteLine();
                //    zipWriter.Flush();
                //}


                //var table = elementsZip.Zip(z_valence, (third, fourth) => third + " " + fourth);
                //foreach (var val in table)
                //{
                //    //zipWriter.Write( "," + val);
                //    //zipWriter.WriteLine();
                //    //zipWriter.Flush();

                //}


            }
            //atomReader.Close();
            //elementReader.Close();
            //valenceReader.Close();
            //atomWriter.Close();
            //zipWriter.Close();
            //valenceWriter.Close();
            elementAndAtomReader.Close();
            valenceReader.Close();

            tableElements.Close();

        }
    }
}
