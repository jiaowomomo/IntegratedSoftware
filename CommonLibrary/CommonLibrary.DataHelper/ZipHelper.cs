using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.DataHelper
{
    public static class ZipHelper
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFilePath">压缩内容路径</param>
        /// <param name="destinationZipFilePath">压缩文件保存路径</param>
        public static void CreateZip(string sourceFilePath, string destinationZipFilePath)
        {
            if (sourceFilePath[sourceFilePath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                sourceFilePath += System.IO.Path.DirectorySeparatorChar;
            }
            ZipOutputStream zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
            //zipStream.Password = strPassWd;
            zipStream.SetLevel(6);  //设置压缩级别 1-9
            CreateZipFiles(sourceFilePath, zipStream);
            zipStream.Finish();
            zipStream.Close();
        }

        /// <summary>
        /// 递归压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
        /// <param name="staticFile"></param>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream)
        {
            Crc32 crc = new Crc32();
            string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
            foreach (string file in filesArray)
            {
                if (Directory.Exists(file))//如果当前是文件夹，递归
                {
                    CreateZipFiles(file, zipStream);
                }
                else//如果是文件，开始压缩
                {
                    FileStream fileStream = File.OpenRead(file);
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    string tempFile = file.Substring(sourceFilePath.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempFile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fileStream.Length;

                    fileStream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipStream.PutNextEntry(entry);
                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="strSourceFilePath">压缩文件路径</param>
        /// <param name="destinationPath">解压文件路径</param>
        public static void UnZip(string strSourceFilePath, string destinationPath)
        {
            if (destinationPath[destinationPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                destinationPath += System.IO.Path.DirectorySeparatorChar;
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(strSourceFilePath)))
            {
                ZipEntry theEntry;
                int size = 2048;
                byte[] data = new byte[2048];
                List<byte> buf;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.IsFile)
                    {
                        if (theEntry.Name.Contains('\\'))
                        {
                            string parentDirPath = theEntry.Name.Remove(theEntry.Name.LastIndexOf("\\") + 1);
                            if (!Directory.Exists(parentDirPath))
                            {
                                Directory.CreateDirectory(destinationPath + parentDirPath);
                            }
                        }
                        else if (theEntry.Name.Contains('/'))
                        {
                            string parentDirPath = theEntry.Name.Remove(theEntry.Name.LastIndexOf("/") + 1);
                            if (!Directory.Exists(parentDirPath))
                            {
                                Directory.CreateDirectory(destinationPath + parentDirPath);
                            }
                        }

                        buf = new List<byte>();
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {

                                if (size == data.Length)
                                {
                                    buf.AddRange(data.ToList());
                                }
                                else
                                {
                                    for (int i = 0; i < size; ++i)
                                    {
                                        buf.Add(data[i]);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        string strPath = destinationPath + theEntry.Name;
                        FileStream fs = new FileStream(strPath, FileMode.Create);
                        fs.Write(buf.ToArray(), 0, buf.Count);
                        fs.Close();

                    }
                    else if (theEntry.IsDirectory)
                    {
                        if (!Directory.Exists(theEntry.Name))
                        {
                            Directory.CreateDirectory(theEntry.Name);
                        }
                    }
                }
            }
        }
    }
}
