using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.DataHelper
{
    public static class TxtHelper
    {
        public static void ReadTxt(string fileName, string filePath, out List<string> contents)
        {
            string fileRoute = Path.Combine(filePath, fileName);
            contents = new List<string>();
            if (File.Exists(fileRoute))
            {
                string content;
                using (StreamReader sr = new StreamReader(fileRoute))
                {
                    while ((content = sr.ReadLine()) != null)
                    {
                        contents.Add(content);
                    }
                }
            }
        }

        public static void WriteTxt(string fileName, string filePath, List<string> contents, bool append = true)
        {
            string fileRoute = Path.Combine(filePath, fileName);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!File.Exists(fileRoute))
            {
                FileStream fs = new FileStream(fileRoute, FileMode.Create);
                fs.Close();
            }
            using (StreamWriter sw = new StreamWriter(fileRoute, append))
            {
                for (int i = 0; i < contents.Count; i++)
                {
                    sw.WriteLine(contents[i]);
                }
            }
        }
    }
}
