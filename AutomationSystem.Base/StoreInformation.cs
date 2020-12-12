using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutomationSystem.Base
{
    public static class StoreInformation
    {
        public static void CreateCSV(string fileName, string filePath, List<string> header)
        {
            string fileRoute = filePath + fileName + ".CSV";
            StreamWriter sw = null;
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (!File.Exists(fileRoute))
                {
                    sw = new StreamWriter(fileRoute, true, Encoding.GetEncoding("gb2312"));
                    string head = "";
                    for (int i = 0; i < header.Count; i++)
                    {
                        if (i != header.Count - 1)
                        {
                            head += header[i] + ",";
                        }
                        else
                        {
                            head += header[i];
                        }
                    }
                    sw.WriteLine(head);
                }
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        public static void SaveCSV(string fileName, string filePath, List<string> saveData)
        {
            string fileRoute = filePath + fileName + ".CSV";
            StreamWriter sw = null;
            try
            {
                if (File.Exists(fileRoute))
                {
                    sw = new StreamWriter(fileName, true, Encoding.GetEncoding("gb2312"));
                    string data = "";
                    for (int i = 0; i < saveData.Count; i++)
                    {
                        if (i != saveData.Count - 1)
                        {
                            data += saveData[i] + ",";
                        }
                        else
                        {
                            data += saveData[i];
                        }
                    }
                    sw.WriteLine(data);
                }
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        public static void ReadTxt(string fileName, string filePath, out List<string> readData)
        {
            string fileRoute = filePath + fileName + ".txt";
            readData = new List<string>();
            if (File.Exists(fileRoute))
            {
                string content;
                using (StreamReader sr = new StreamReader(fileRoute))
                {
                    while ((content = sr.ReadLine()) != null)
                    {
                        readData.Add(content);
                    }
                }
            }
        }

        public static void WriteTxt(string fileName, string filePath, List<string> saveData, bool append = true)
        {
            string fileRoute = filePath + fileName + ".txt";
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
                for (int i = 0; i < saveData.Count; i++)
                {
                    sw.WriteLine(saveData[i]);
                }
            }
        }
    }
}
