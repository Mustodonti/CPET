using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace CPET
{
    class WorkWithData
    {
         static public List<double> DataFromTXT(string Path)
        {
            List<double> Data = new List<double>() { };
            StreamReader sr = new StreamReader(Path);
            string[] headerRow = sr.ReadLine().Split(' ');
            sr.Close();
            foreach (string i in headerRow)
            {
                try
                {
                    Data.Add(Convert.ToDouble(i));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return Data;
        }
        static public void CreateDirectoryToTargetPathWWD001(string Target_Path)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Target_Path);//Работа с Директорией
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    Console.WriteLine("\nCreate destination folder for old files\n------------------------------Successful---------------------------------\n");
                }
                else Console.WriteLine("Папка уже существует,прежде удалите ее, для того чтобы продолжить");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "ErrorIn#WWD001");
                Console.WriteLine(ex.Message + "\nErrorIn#WWD001\n");
            }
        }
    }
}
