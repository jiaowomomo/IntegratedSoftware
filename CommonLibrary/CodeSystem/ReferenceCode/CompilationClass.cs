//************************************************************************************
// Developer : Mohammed Salah
// Email : MS_Soft89@Yahoo or hotmail.com
// for any question you can email me
//************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CommonLibrary.CodeSystem
{
    public class ErrorEventArgs : EventArgs
    {
        public readonly List<string> Errors = new List<string>();
        public ErrorEventArgs(List<string> errors)
        {
            this.Errors = errors;
        }
    }

    public delegate void ErrorEventHandler(Object sender, ErrorEventArgs e);

    public class CompilationClass
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        CompilerResults result = null;
        CSharpCodeProvider provider = new CSharpCodeProvider();
        bool isRuning = false;

        public bool IsRuning
        {
            get { return isRuning; }
        }

        public CompilationClass()
        {
            RefAssemblies = new List<Assembly>();
        }

        List<Assembly> assemblies = new List<Assembly>();

        public List<Assembly> RefAssemblies { get { return assemblies; } set { assemblies = value; } }

        private static IEnumerable<string> GetFilesRecursive(string dirPath)
        {
            if (!Directory.Exists(dirPath)) return new List<string>();
            DirectoryInfo dinfo = new DirectoryInfo(dirPath);
            return GetFilesRecursive(dinfo, "*.dll*");
        }

        private static IEnumerable<string> GetFilesRecursive(DirectoryInfo dirInfo, string searchPattern)
        {
            foreach (DirectoryInfo di in dirInfo.GetDirectories())
                foreach (string fi in GetFilesRecursive(di, searchPattern))
                    yield return fi;
            foreach (FileInfo fi in dirInfo.GetFiles(searchPattern))
                yield return fi.FullName;
        }

        public List<string> AssemblyDirectories()
        {
            List<string> assemblies = new List<string>();
            Microsoft.Win32.RegistryKey key = Registry.LocalMachine;
            key = key.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\AssemblyFolders");
            string[] SubKeynames = key.GetSubKeyNames();
            foreach (string subkeys in SubKeynames)
            {
                RegistryKey subkey = Registry.LocalMachine;
                subkey = subkey.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\AssemblyFolders\" + subkeys);
                string[] l = subkey.GetValueNames();
                foreach (string v in l)
                {
                    if (subkey.GetValueKind(v) == RegistryValueKind.String)
                    {
                        string value = (string)subkey.GetValue(v);
                        assemblies.Add(value);
                    }
                }
            }
            assemblies.Add(Environment.SystemDirectory + @"..\..\Windows\Microsoft.NET\Framework\v2.0.50727");
            return assemblies;
        }

        public List<string> GetAssembliesFile()
        {
            List<string> refAssemblies = new List<string>();
            if (!File.Exists(Environment.CurrentDirectory + "\\assemblies.sft"))
            {
                List<string> dirs = AssemblyDirectories();
                foreach (string dir in dirs)
                {
                    foreach (string assemblyfile in GetFilesRecursive(dir))
                    {


                        refAssemblies.Add(assemblyfile);

                    }
                }
                foreach (string assemblyfile in System.IO.Directory.GetFiles(@"C:\Windows\Microsoft.NET\Framework"))
                {
                    FileInfo fino = new FileInfo(assemblyfile);
                    refAssemblies.Add(fino.FullName);

                }
                SerializeAssemblyDirs(refAssemblies);
            }
            else
            {
                BinaryFormatter binary = new BinaryFormatter();
                Stream sreader = new FileStream(Environment.CurrentDirectory + "\\assemblies.sft", FileMode.Open);

                refAssemblies = (List<string>)binary.Deserialize(sreader);
                sreader.Close();
            }
            return refAssemblies;
        }

        public void SerializeAssemblyDirs(List<string> assemblies)
        {

            Stream str = new FileStream(Environment.CurrentDirectory + "\\assemblies.sft", FileMode.OpenOrCreate);
            BinaryFormatter binary = new BinaryFormatter();
            binary.Serialize(str, assemblies);
        }

        public event ErrorEventHandler CompilationError;

        protected virtual void OnCompilationError(ErrorEventArgs e)
        {
            if (CompilationError != null)
            {
                CompilationError(this, e);
            }
        }

        public event EventHandler FileNotExists;

        protected virtual void OnFileNotExists(EventArgs e)
        {
            if (FileNotExists != null)
            {
                FileNotExists(this, e);
            }
        }

        public static string Classname(string code)
        {
            string mainNameSpace = string.Empty;
            List<string> classcode = new List<string>();
            List<string> classname = new List<string>();
            List<string> NamespaceCode = new List<string>();
            List<string> nmSpaceName;
            string mainClass = string.Empty;
            ManiPulateMainClass.ManipulateNameSpace(code, out NamespaceCode, out nmSpaceName);
            for (int x = 0; x < nmSpaceName.Count; x++)
            {
                ManiPulateMainClass.ManipulateClasses(NamespaceCode[x], out classname, out classcode);
                mainClass = ManiPulateMainClass.GetMainClass(classname, classcode);
                mainNameSpace = nmSpaceName[x];
            }
            string[] mainClassName = mainClass.Split(':');
            return string.Format("{0}.{1}", mainNameSpace.Trim(), mainClassName[0].Trim());
        }

        string mainClass = string.Empty;
        string compilerPath = Application.StartupPath + "\\";
        string outputassembly = "DynamicalCode.dll";

        private List<String> m_listExternDll = new List<string>();

        public void AddExtern(String strExternDll)
        {
            if (!m_listExternDll.Contains(strExternDll))
            {
                m_listExternDll.Add(strExternDll);
            }
        }

        public string Outputassembly
        {
            get { return outputassembly; }
            set { outputassembly = value; }
        }

        public bool CompileCSharp(string code, ref List<string> errors)
        {
            if (!Directory.Exists(compilerPath))
            {
                Directory.CreateDirectory(compilerPath);
            }

            #region CompilerParameters

            mainClass = Classname(code);
            string[] strArr = mainClass.Split(new char[] { '.' });
            if (strArr.Length >= 2)
            {
                if (strArr[1] == "none")
                {
                    List<string> cerrors = new List<string>();
                    cerrors.Add("cannot find Entry Point Main");
                    OnCompilationError(new ErrorEventArgs(cerrors));
                    return false;
                }
            }
            CompilerParameters parameters = new CompilerParameters();

            // parameters.OutputAssembly = outputassembly;
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.ReferencedAssemblies.Clear();
            parameters.MainClass = mainClass;
            foreach (Assembly asm in assemblies)
            {
                if (!parameters.ReferencedAssemblies.Contains(asm.Location)) parameters.ReferencedAssemblies.Add(asm.Location);
            }

            for (int i = 0; i < m_listExternDll.Count; ++i)
            {
                if (!parameters.ReferencedAssemblies.Contains(m_listExternDll[i]))
                {
                    parameters.ReferencedAssemblies.Add(m_listExternDll[i]);
                }
            }

            // Add available assemblies - this should be enough for the simplest
            // applications.

            Assembly[] ssemblyArr = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!parameters.ReferencedAssemblies.Contains(asm.Location)) parameters.ReferencedAssemblies.Add(asm.Location);
            }

            #endregion

            result = provider.CompileAssemblyFromSource(parameters, code);
            if (result.Errors.Count == 0)
            {
                List<string> cerrors = new List<string>();
                OnCompilationError(new ErrorEventArgs(cerrors));
                errors = cerrors;
                return true;
            }
            else
            {
                List<string> cerrors = new List<string>();
                errors = cerrors;
                foreach (CompilerError er in result.Errors)
                {
                    cerrors.Add("Error:" + er.ErrorText + " Line (" + er.Line.ToString() + ")");
                    OnCompilationError(new ErrorEventArgs(cerrors));
                }
                result.Errors.Clear();
                return false;
            }
        }

        //运行静态方法
        public object Run_functionStatic(string funcName)
        {
            try
            {
                if (objDynamicAssenmbly != null)
                {
                    MethodInfo objMI = objDynamicAssenmbly.GetType().GetMethod(funcName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                    object objRet = objMI.Invoke(objDynamicAssenmbly, null);

                    return objRet;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        //运行当前实例
        public object Run_function(string funcName)
        {
            try
            {
                if (objDynamicAssenmbly != null)
                {
                    MethodInfo objMI = objDynamicAssenmbly.GetType().GetMethod(funcName);
                    object objRet = objMI.Invoke(objDynamicAssenmbly, null);

                    return objRet;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        object objDynamicAssenmbly = null;

        public void RunCode(string funcName = "")
        {
            isRuning = true;
            /*
            if (!System.IO.File.Exists(compilerPath + outputassembly))
            {
                OnFileNotExists(new EventArgs());
                return;
            }
            */
            if (string.IsNullOrEmpty(funcName))
                funcName = "Main";
            try
            {
                if (result != null && result.Errors.Count == 0)
                {
                    // 通过反射，调用HelloWorld的实例
                    Assembly objAssembly = result.CompiledAssembly;
                    objDynamicAssenmbly = objAssembly.CreateInstance(mainClass);
                    MethodInfo objMI = objDynamicAssenmbly.GetType().GetMethod(funcName);
                    objMI.Invoke(objDynamicAssenmbly, null);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                isRuning = false;
                GC.Collect();
            }
        }
    }
}
