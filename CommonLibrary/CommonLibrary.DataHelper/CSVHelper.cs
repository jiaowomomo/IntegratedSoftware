using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.DataHelper
{
    public static class CSVHelper
    {
        public static void CreateCSV(string fileName, string filePath, List<string> headers)
        {
            string fileRoute = Path.Combine(filePath, fileName);
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
                    for (int i = 0; i < headers.Count; i++)
                    {
                        if (i != headers.Count - 1)
                        {
                            head += headers[i] + ",";
                        }
                        else
                        {
                            head += headers[i];
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

        public static void SaveCSV(string fileName, string filePath, List<string> contents)
        {
            string fileRoute = Path.Combine(filePath, fileName);
            StreamWriter sw = null;
            try
            {
                if (File.Exists(fileRoute))
                {
                    sw = new StreamWriter(fileRoute, true, Encoding.GetEncoding("gb2312"));
                    string data = "";
                    for (int i = 0; i < contents.Count; i++)
                    {
                        if (i != contents.Count - 1)
                        {
                            data += contents[i] + ",";
                        }
                        else
                        {
                            data += contents[i];
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
    }
}
