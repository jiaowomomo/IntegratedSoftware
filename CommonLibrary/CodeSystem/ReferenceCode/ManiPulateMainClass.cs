//************************************************************************************
// Developer : Mohammed Salah
// Email : MS_Soft89@Yahoo or hotmail.com
// for any question you can email me
//************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.CodeSystem
{
    class ManiPulateMainClass
    {
        public static void ManipulateNameSpace(string code, out List<string> Namespace, out List<string> Nmspacename)
        {
            Namespace = new List<string>();
            Nmspacename = new List<string>();
            string allstrings = code;
            string[] splitArray = { "namespace" };
            string[] parts = allstrings.Split(splitArray, StringSplitOptions.None);

            foreach (string allstring in parts)
            {
                if (allstring.Contains("{") && !allstring.StartsWith("\""))
                {
                    int l_nmspace = allstring.IndexOf("{");
                    Namespace.Add(allstring.Remove(0, l_nmspace + 1));
                    string name = allstring.Substring(0, l_nmspace);
                    Nmspacename.Add(name);
                }
            }
        }

        public static List<string> GetAssemblyNames(string code)
        {
            List<string> names = new List<string>();
            string allstrings = code;
            string[] splitArray = { "using" };
            string[] parts = allstrings.Split(splitArray, StringSplitOptions.None);

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Contains(";"))
                {
                    int l_fbrace = parts[i].IndexOf(";");
                    string name = parts[i].Substring(1, l_fbrace - 1); ;
                    names.Add(name + ".dll");
                }
            }
            return names;
        }

        public static void ManipulateClasses(string NameSpace, out List<string> Classname, out List<string> ClassCode)
        {
            ClassCode = new List<string>();
            Classname = new List<string>();
            string allstrings = NameSpace;
            string[] splitArray = { "class" };
            string[] parts = allstrings.Split(splitArray, StringSplitOptions.None);

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Contains("{") && !parts[i].StartsWith("\""))
                {
                    ClassCode.Add(parts[i]);

                    int l_fbrace = parts[i].IndexOf("{");
                    int l_dot = 0;
                    if (parts[i].Contains(":"))
                        l_dot = parts[i].IndexOf(":");
                    int l_classname;
                    if (l_fbrace < l_dot)
                    {
                        l_classname = l_fbrace;
                    }
                    else
                    {
                        l_classname = l_dot;
                    }
                    string name = parts[i].Substring(1, l_fbrace - 1); ;
                    Classname.Add(name);
                }
            }
        }

        public static void ManipulateNameSpace(string code, out List<string> Nmspacename)
        {
            Nmspacename = new List<string>();
            string allstrings = code;
            string[] splitArray = { "namespace" };
            string[] parts = allstrings.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);

            foreach (string allstring in parts)
            {
                if (allstring.Contains("{") && !allstring.StartsWith("\""))
                {
                    int l_nmspace = allstring.IndexOf("{");

                    string name = allstring.Substring(0, l_nmspace);
                    Nmspacename.Add(name);
                }
            }
        }

        public static string GetMainClass(List<string> classname, List<string> classCode)
        {
            for (int i = 0; i < classCode.Count; i++)
            {
                if (classCode[i].Contains("Main(") && !classCode.Contains("\""))
                {
                    return classname[i];
                }
            }
            return "none";
        }
    }
}
