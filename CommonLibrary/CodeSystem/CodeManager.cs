using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.CodeSystem
{
    public class CodeManager
    {
        private static readonly Lazy<CodeManager> m_instance = new Lazy<CodeManager>(() => new CodeManager());

        private static readonly string m_strCodeDirectory = Path.Combine(Application.StartupPath, "CodeSystem");
        private static readonly string m_strCodePartDirectory = Path.Combine(m_strCodeDirectory, "CodePart");
        private static readonly string m_strThirdPartyDLLDirectory = Path.Combine(m_strCodeDirectory, "ThirdPartyDLL");
        private static readonly string m_strHistoryMainCodePath = Path.Combine(m_strCodeDirectory, "MainCodePath.txt");
        private static readonly string m_strCodeHeaderPath = Path.Combine(m_strCodePartDirectory, HEADER_PAGE);
        private static readonly string m_strCompiledCodePath = Path.Combine(m_strCodePartDirectory, GENERATE_PAGE);
        private static readonly string m_strCodeSystemReferencePath = Path.Combine(m_strCodePartDirectory, SYSTEM_REFERENCE_PAGE);
        private static readonly string m_strCodeCustomReferencePath = Path.Combine(m_strCodePartDirectory, CUSTOM_REFERENCE_PAGE);

        public const string NAMESPACE_NAME = "DynamicCodeGenerate";
        public const string CLASS_NAME = "CodeSystem";
        public const string EXECUTE_NAME = "Main";
        public const string SUB_SUFFIX = ".sub";
        public const string MAIN_PAGE = "Main.main";
        public const string HEADER_PAGE = "Using.head";
        public const string SYSTEM_REFERENCE_PAGE = "SystemReference.ref";
        public const string CUSTOM_REFERENCE_PAGE = "CustomReference.ref";
        public const string GENERATE_PAGE = "Code.cs";

        public const string DEFAULT_HEADER = "using System;\r\n" +
                                            "using System.Collections.Generic;\r\n" +
                                            "using System.ComponentModel;\r\n" +
                                            "using System.Data;\r\n" +
                                            "using System.Diagnostics;\r\n" +
                                            "using System.IO;\r\n" +
                                            "using System.Linq;\r\n" +
                                            "using System.Reflection;\r\n" +
                                            "using System.Text;\r\n" +
                                            "using System.Threading;\r\n" +
                                            "using System.Threading.Tasks;\r\n" +
                                            "using System.Windows.Forms;";

        public const string DEFAULT_SYSTEM_REFERENCE = "Microsoft.CSharp.dll\r\n" +
                                                        "System.dll\r\n" +
                                                        "System.Core.dll\r\n" +
                                                        "System.Data.dll\r\n" +
                                                        "System.Data.DataSetExtensions.dll\r\n" +
                                                        "System.Deployment.dll\r\n" +
                                                        "System.Drawing.dll\r\n" +
                                                        "System.Net.Http.dll\r\n" +
                                                        "System.Windows.Forms.dll\r\n" +
                                                        "System.Xml.dll\r\n" +
                                                        "System.Xml.Linq.dll";

        private CompilerResults m_compilerResults;
        private List<string> m_listSystemReferences = new List<string>();
        private List<string> m_listCustomReferences = new List<string>();
        private Thread m_threadCode;
        private string m_strErrors = string.Empty;
        private CodeStatus m_codeStatus = CodeStatus.Idle;

        public static CodeManager Instance { get => m_instance.Value; }

        public delegate void StatusChangeEventHandler();
        public StatusChangeEventHandler OnStatusChange { get; set; }
        public string MainCodeFullPath { get; set; }
        public string CodeDirectory => m_strCodeDirectory;
        public string CodePartDirectory => m_strCodePartDirectory;
        public string ThirdPartyDLLDirectory => m_strThirdPartyDLLDirectory;
        public HashSet<string> SubMethods = new HashSet<string>();
        public List<string> CustomReferences => m_listCustomReferences;

        public CodeStatus Status
        {
            get { return m_codeStatus; }
            set
            {
                m_codeStatus = value;
                OnStatusChange?.Invoke();
            }
        }

        private CodeManager()
        {
            if (!Directory.Exists(m_strCodeDirectory))
            {
                Directory.CreateDirectory(m_strCodeDirectory);
            }
            if (!Directory.Exists(m_strCodePartDirectory))
            {
                Directory.CreateDirectory(m_strCodePartDirectory);
            }
            if (!Directory.Exists(m_strThirdPartyDLLDirectory))
            {
                Directory.CreateDirectory(m_strThirdPartyDLLDirectory);
            }
            if (!File.Exists(m_strCodeHeaderPath))
            {
                using (StreamWriter sw = new StreamWriter(m_strCodeHeaderPath))
                {
                    sw.Write(DEFAULT_HEADER);
                }
            }
            if (!File.Exists(m_strCodeSystemReferencePath))
            {
                using (StreamWriter sw = new StreamWriter(m_strCodeSystemReferencePath))
                {
                    sw.Write(DEFAULT_SYSTEM_REFERENCE);
                }
            }
            string[] strSubMethods = Directory.GetFiles(m_strCodePartDirectory, $"*{SUB_SUFFIX}");
            foreach (string strMethod in strSubMethods)
            {
                SubMethods.Add(Path.GetFileName(strMethod));
            }
            if (File.Exists(m_strHistoryMainCodePath))
            {
                using (StreamReader sr = new StreamReader(m_strHistoryMainCodePath))
                {
                    MainCodeFullPath = sr.ReadLine();
                    if (!File.Exists(MainCodeFullPath))
                    {
                        MainCodeFullPath = Path.Combine(m_strCodePartDirectory, MAIN_PAGE);
                    }
                }
            }
            else
            {
                MainCodeFullPath = Path.Combine(m_strCodePartDirectory, MAIN_PAGE);
            }
            GetCodeReference();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly[] select = assemblies.Where(x => x.FullName.Equals(args.Name, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (select.Length == 0)
            {
                try
                {
                    string strDLLPath;
                    string strDLLName = $"{args.Name.Split(',')[0]}.dll";
                    Assembly assembly = null;
                    strDLLPath = Path.Combine(Application.StartupPath, strDLLName);
                    if (File.Exists(strDLLPath))
                    {
                        assembly = Assembly.LoadFile(strDLLPath);
                    }
                    else
                    {
                        strDLLPath = Path.Combine(m_strThirdPartyDLLDirectory, strDLLName);
                        if (File.Exists(strDLLPath))
                        {
                            assembly = Assembly.LoadFile(strDLLPath);
                        }
                    }
                    return assembly;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return select[0];
            }
        }

        private string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetCodeHeader());
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append($"namespace {NAMESPACE_NAME}\r\n");
            sb.Append("{\r\n");
            sb.Append($"    public class {CLASS_NAME}\r\n");
            sb.Append("     {\r\n");
            sb.Append($"        public void {EXECUTE_NAME}()\r\n");
            sb.Append("         {\r\n");
            sb.Append(GetMainCode());
            sb.Append("         }\r\n");
            sb.Append(GetSubMethodCode());
            sb.Append("     }\r\n");
            sb.Append("}");

            string code = sb.ToString();
            return code;
        }

        private void GetCodeReference()
        {
            if (File.Exists(m_strCodeSystemReferencePath))
            {
                using (StreamReader sr = new StreamReader(m_strCodeSystemReferencePath))
                {
                    string strDLLs = sr.ReadToEnd();
                    m_listSystemReferences = strDLLs.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            else
            {
                m_listSystemReferences = DEFAULT_SYSTEM_REFERENCE.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            if (File.Exists(m_strCodeCustomReferencePath))
            {
                using (StreamReader sr = new StreamReader(m_strCodeCustomReferencePath))
                {
                    string strDLLs = sr.ReadToEnd();
                    m_listCustomReferences = strDLLs.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
        }

        private string GetCodeHeader()
        {
            string strHeaderCode = string.Empty;
            if (File.Exists(m_strCodeHeaderPath))
            {
                using (StreamReader sr = new StreamReader(m_strCodeHeaderPath))
                {
                    strHeaderCode = sr.ReadToEnd();
                }
            }
            else
            {
                strHeaderCode = DEFAULT_HEADER;
            }
            return strHeaderCode;
        }

        private string GetMainCode()
        {
            string strMainCode;
            StringBuilder stringBuilder = new StringBuilder();
            if (File.Exists(MainCodeFullPath))
            {
                using (StreamReader sr = new StreamReader(MainCodeFullPath))
                {
                    while (!sr.EndOfStream)
                    {
                        stringBuilder.AppendLine($"\t\t\t{sr.ReadLine()}");
                    }
                }
            }
            strMainCode = stringBuilder.ToString();
            return strMainCode;
        }

        private string GetSubMethodCode()
        {
            StringBuilder sb = new StringBuilder();
            string strSubMethodPath;
            foreach (string strSubMethod in SubMethods)
            {
                strSubMethodPath = Path.Combine(m_strCodePartDirectory, strSubMethod);
                if (File.Exists(strSubMethodPath))
                {
                    using (StreamReader sr = new StreamReader(strSubMethodPath))
                    {
                        sb.Append(Environment.NewLine);
                        while (!sr.EndOfStream)
                        {
                            sb.AppendLine($"\t\t{sr.ReadLine()}");
                        }
                    }
                }
            }
            return sb.ToString();
        }

        private void Run()
        {
            if ((m_compilerResults != null) && (!m_compilerResults.Errors.HasErrors))
            {
                try
                {
                    Assembly assembly = m_compilerResults.CompiledAssembly;
                    object objectInstance = assembly.CreateInstance($"{NAMESPACE_NAME}.{CLASS_NAME}");
                    if (objectInstance == null)
                    {
                        MessageBox.Show("创建实例失败，请检查代码.");
                        return;
                    }
                    MethodInfo methodInfo = objectInstance.GetType().GetMethod(EXECUTE_NAME);
                    if (methodInfo == null)
                    {
                        MessageBox.Show("执行方法不存在，请检查代码.");
                        return;
                    }
                    methodInfo.Invoke(objectInstance, null);
                    Status = CodeStatus.Idle;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}\r\n{(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
                    Status = CodeStatus.AbnormalStop;
                }
            }
            else
            {
                MessageBox.Show("代码编译失败，请检查代码");
            }
        }

        public bool Compile()
        {
            bool bIsCompileSuccess;
            try
            {
                CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();

                CompilerParameters compilerParameters = new CompilerParameters();
                GetCodeReference();
                if (m_listSystemReferences.Count != 0)
                {
                    for (int i = 0; i < m_listSystemReferences.Count; i++)
                    {
                        compilerParameters.ReferencedAssemblies.Add(m_listSystemReferences[i]);
                    }
                }
                if (m_listCustomReferences.Count != 0)
                {
                    string strDLLPath;
                    for (int i = 0; i < m_listCustomReferences.Count; i++)
                    {
                        strDLLPath = Path.Combine(Application.StartupPath, m_listCustomReferences[i]);
                        if (File.Exists(strDLLPath))
                        {
                            compilerParameters.ReferencedAssemblies.Add(strDLLPath);
                        }
                        else
                        {
                            strDLLPath = Path.Combine(m_strThirdPartyDLLDirectory, m_listCustomReferences[i]);
                            if (File.Exists(strDLLPath))
                            {
                                compilerParameters.ReferencedAssemblies.Add(strDLLPath);
                            }
                        }
                    }
                }
                compilerParameters.GenerateExecutable = false;
                compilerParameters.GenerateInMemory = true;
                string strFullCode = GenerateCode();
                m_compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, strFullCode);
                if (m_compilerResults.Errors.HasErrors)
                {
                    string strError = string.Empty;
                    strError += "编译错误：\r\n";
                    foreach (CompilerError err in m_compilerResults.Errors)
                    {
                        strError += $"Line{err.Line}:{err.ErrorText}\r\n";
                    }
                    m_strErrors = strError;
                    m_compilerResults = null;
                    bIsCompileSuccess = false;
                }
                else
                {
                    m_strErrors = "编译成功";
                    bIsCompileSuccess = true;
                }
                using (StreamWriter sw = new StreamWriter(m_strCompiledCodePath))
                {
                    sw.WriteLine(strFullCode);
                }
            }
            catch (Exception ex)
            {
                m_strErrors += $"编译错误：\r\n{ex.Message}";
                m_compilerResults = null;
                bIsCompileSuccess = false;
            }
            return bIsCompileSuccess;
        }

        public bool RunCode()
        {
            if (Status == CodeStatus.Pause)
            {
                Status = CodeStatus.Run;
                m_threadCode.Resume();
            }
            else if (Status == CodeStatus.Idle || Status == CodeStatus.AbnormalStop)
            {
                if ((m_compilerResults == null))
                {
                    if (!Compile())
                        return false;
                }

                Status = CodeStatus.Run;
                m_threadCode = new Thread(Run);
                m_threadCode.IsBackground = true;
                m_threadCode.Start();
            }
            return true;
        }

        public void PauseCode()
        {
            m_threadCode.Suspend();
            Status = CodeStatus.Pause;
        }

        public void StopCode()
        {
            try
            {
                if ((m_threadCode.ThreadState & ThreadState.Suspended) == ThreadState.Suspended)
                {
                    m_threadCode.Resume();
                }
                m_threadCode.Abort();
            }
            catch
            {
            }
            Status = CodeStatus.Idle;
        }

        public string GetErrors()
        {
            return m_strErrors;
        }

        public void SaveHistoryCodePath()
        {
            if (!Directory.Exists(CodeDirectory))
            {
                Directory.CreateDirectory(CodeDirectory);
            }
            using (StreamWriter sw = new StreamWriter(m_strHistoryMainCodePath))
            {
                sw.Write(MainCodeFullPath);
            }
        }
    }
}
