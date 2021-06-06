using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.DataHelper
{
    public static class IniHelper
    {
        /// <summary>        
        /// 写入INI文件        
        /// </summary>        
        /// <param name="section">节点名称[如[TypeName]]</param>        
        /// <param name="key">键</param>        
        /// <param name="val">值</param>        
        /// <param name="fileFullPath">文件路径</param>        
        /// <returns></returns>        
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string fileFullPath);

        /// <summary>        
        /// 读取INI文件        
        /// </summary>        
        /// <param name="section">节点名称</param>        
        /// <param name="key">键</param>        
        /// <param name="def">值</param>        
        /// <param name="retval">stringbulider对象</param>        
        /// <param name="size">字节大小</param>        
        /// <param name="fileFullPath">文件路径</param>        
        /// <returns></returns>        
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string fileFullPath);

        /// <summary>
        /// 自定义读取INI文件中的内容方法  
        /// </summary>
        /// <param name="fileFullPath">文件路径</param>
        /// <param name="section">节点</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string ContentValue(string fileFullPath, string section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, "", temp, 1024, fileFullPath);
            return temp.ToString();
        }

        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="section">节点</param>
        /// <param name="configurations">配置参数</param>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件夹路径</param>
        public static void Write(string section, Dictionary<string, string> configurations, string fileName, string filePath)
        {
            string fileRoute = Path.Combine(filePath, fileName);
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (!File.Exists(fileRoute))
                {
                    FileStream fs = new FileStream(fileRoute, FileMode.Create);
                    fs.Close();
                }
                foreach (KeyValuePair<string, string> item in configurations)
                {
                    WritePrivateProfileString(section, item.Key, item.Value, fileRoute);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件夹路径</param>
        /// <param name="strSec">节点</param>
        /// <param name="configurations">配置参数</param>
        public static void Read(string fileName, string filePath, string strSec, ref Dictionary<string, string> configurations)
        {
            string fileRoute = Path.Combine(filePath, fileName);
            Dictionary<string, string> temp = new Dictionary<string, string>();
            try
            {
                if (File.Exists(fileRoute))//读取时先要判读INI文件是否存在            
                {
                    foreach (KeyValuePair<string, string> item in configurations)
                    {
                        temp.Add(item.Key, ContentValue(fileRoute, strSec, item.Key));
                    }
                    configurations = temp;
                }
                else
                {
                    MessageBox.Show("保存参数文件不存在");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 数据To属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="configurations">配置参数</param>
        public static void DataToAttribute(Object obj, Dictionary<string, string> configurations)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo;
            foreach (KeyValuePair<string, string> item in configurations)
            {
                propertyInfo = type.GetProperty(item.Key);
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        propertyInfo.SetValue(obj, int.Parse(item.Value), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(uint))
                    {
                        propertyInfo.SetValue(obj, uint.Parse(item.Value), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(string))
                    {
                        propertyInfo.SetValue(obj, item.Value, null);
                    }
                    else if (propertyInfo.PropertyType == typeof(short))
                    {
                        propertyInfo.SetValue(obj, short.Parse(item.Value), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(ushort))
                    {
                        propertyInfo.SetValue(obj, ushort.Parse(item.Value), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(double))
                    {
                        propertyInfo.SetValue(obj, double.Parse(item.Value), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(obj, bool.Parse(item.Value), null);
                    }
                    else if (propertyInfo.PropertyType == typeof(float))
                    {
                        propertyInfo.SetValue(obj, float.Parse(item.Value), null);
                    }
                    else if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(obj, Enum.Parse(propertyInfo.PropertyType, item.Value), null);
                    }
                }
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="obj">数据对象</param>
        /// <param name="section">节点</param>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件夹路径</param>
        public static void AttributeToData(Object obj, string section, string fileName, string filePath)
        {
            string fileRoute = Path.Combine(filePath, fileName);
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (!File.Exists(fileRoute))
                {
                    FileStream fs = new FileStream(fileRoute, FileMode.Create);
                    fs.Close();
                }
                Type type = obj.GetType();
                PropertyInfo[] propertyInfo = type.GetProperties();
                foreach (PropertyInfo pi in propertyInfo)
                {
                    WritePrivateProfileString(section, pi.Name, pi.GetValue(obj, null).ToString(), fileRoute);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="obj">数据对象</param>
        /// <param name="section">节点</param>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件夹路径</param>
        public static void DataToAttribute(Object obj, string section, string fileName, string filePath)
        {
            string fileRoute = Path.Combine(filePath, fileName);
            try
            {
                if (File.Exists(fileRoute))//读取时先要判读INI文件是否存在            
                {
                    Type type = obj.GetType();
                    PropertyInfo[] propertyInfo = type.GetProperties();
                    foreach (PropertyInfo pi in propertyInfo)
                    {
                        if ((pi != null) && (ContentValue(fileRoute, section, pi.Name) != ""))
                        {
                            if (pi.PropertyType == typeof(int))
                            {
                                pi.SetValue(obj, int.Parse(ContentValue(fileRoute, section, pi.Name)), null);
                            }
                            else if (pi.PropertyType == typeof(uint))
                            {
                                pi.SetValue(obj, uint.Parse(ContentValue(fileRoute, section, pi.Name)), null);
                            }
                            else if (pi.PropertyType == typeof(string))
                            {
                                pi.SetValue(obj, ContentValue(fileRoute, section, pi.Name), null);
                            }
                            else if (pi.PropertyType == typeof(short))
                            {
                                pi.SetValue(obj, short.Parse(ContentValue(fileRoute, section, pi.Name)), null);
                            }
                            else if (pi.PropertyType == typeof(ushort))
                            {
                                pi.SetValue(obj, ushort.Parse(ContentValue(fileRoute, section, pi.Name)), null);
                            }
                            else if (pi.PropertyType == typeof(double))
                            {
                                pi.SetValue(obj, double.Parse(ContentValue(fileRoute, section, pi.Name)), null);
                            }
                            else if (pi.PropertyType == typeof(bool))
                            {
                                pi.SetValue(obj, bool.Parse(ContentValue(fileRoute, section, pi.Name)), null);
                            }
                            else if (pi.PropertyType == typeof(float))
                            {
                                pi.SetValue(obj, float.Parse(ContentValue(fileRoute, section, pi.Name)), null);
                            }
                            else if (pi.PropertyType.IsEnum)
                            {
                                pi.SetValue(obj, Enum.Parse(pi.PropertyType, ContentValue(fileRoute, section, pi.Name)), null);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("保存参数文件不存在");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
