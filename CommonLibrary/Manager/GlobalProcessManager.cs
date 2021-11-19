using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Manager
{
    public static class GlobalProcessManager
    {
        private static Dictionary<string, object> m_singleProcessManagerDictionary = new Dictionary<string, object>();
        private static Dictionary<string, object> m_multiProcessManagerDictionary = new Dictionary<string, object>();

        public static ProcessManagerResult<T> CreateSingleProcessManager<T>(string strProcessManagerName)
        {
            ProcessManagerResult<T> managerResult = new ProcessManagerResult<T>();
            if (m_singleProcessManagerDictionary.ContainsKey(strProcessManagerName))
            {
                managerResult.OK = false;
                managerResult.Error = ErrorReason.KeyExist;
                return managerResult;
            }
            else
            {
                ProcessManager<T> processManager = new ProcessManager<T>();
                m_singleProcessManagerDictionary.Add(strProcessManagerName, processManager);
                return managerResult;
            }
        }

        public static ProcessManagerResult<T> CreateMultiProcessManager<T>(string strProcessManagerName, uint nCapacity = 0)
        {
            ProcessManagerResult<T> managerResult = new ProcessManagerResult<T>();
            if (m_multiProcessManagerDictionary.ContainsKey(strProcessManagerName))
            {
                managerResult.OK = false;
                managerResult.Error = ErrorReason.KeyExist;
                return managerResult;
            }
            else
            {
                List<ProcessManager<T>> processManagers = new List<ProcessManager<T>>();
                for (int i = 0; i < nCapacity; i++)
                {
                    processManagers.Add(new ProcessManager<T>());
                }
                m_multiProcessManagerDictionary.Add(strProcessManagerName, processManagers);
                return managerResult;
            }
        }

        public static ProcessManagerResult<ProcessManager<T>> GetProcessManager<T>(string strProcessManagerName)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = new ProcessManagerResult<ProcessManager<T>>();
            if (m_singleProcessManagerDictionary.ContainsKey(strProcessManagerName))
            {
                if (m_singleProcessManagerDictionary[strProcessManagerName] is ProcessManager<T> processManager)
                {
                    managerResult.GetProcessManager = processManager;
                    return managerResult;
                }
                else
                {
                    managerResult.OK = false;
                    managerResult.Error = ErrorReason.ConversionFailure;
                    return managerResult;
                }
            }
            else
            {
                managerResult.OK = false;
                managerResult.Error = ErrorReason.KeyNotFound;
                return managerResult;
            }
        }

        public static ProcessManagerResult<ProcessManager<T>> GetProcessManager<T>(string strProcessManagerName, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = new ProcessManagerResult<ProcessManager<T>>();
            if (m_multiProcessManagerDictionary.ContainsKey(strProcessManagerName))
            {
                if (m_multiProcessManagerDictionary[strProcessManagerName] is List<ProcessManager<T>> processManagers)
                {
                    if (nListIndex < 0 || nListIndex >= processManagers.Count)
                    {
                        managerResult.OK = false;
                        managerResult.Error = ErrorReason.IndexOutOfRange;
                        return managerResult;
                    }

                    managerResult.GetProcessManager = processManagers[nListIndex];
                    return managerResult;
                }
                else
                {
                    managerResult.OK = false;
                    managerResult.Error = ErrorReason.ConversionFailure;
                    return managerResult;
                }
            }
            else
            {
                managerResult.OK = false;
                managerResult.Error = ErrorReason.KeyNotFound;
                return managerResult;
            }
        }

        public static void AddProcess<T>(string strProcessManagerName, T T_object)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.AddProcess(T_object);
            }
        }

        public static void AddProcess<T>(string strProcessManagerName, T T_object, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.AddProcess(T_object);
            }
        }

        public static void AddProcessManager<T>(string strProcessManagerName)
        {
            if (m_multiProcessManagerDictionary.ContainsKey(strProcessManagerName))
            {
                if (m_multiProcessManagerDictionary[strProcessManagerName] is List<ProcessManager<T>> processManagers)
                {
                    processManagers.Add(new ProcessManager<T>());
                }
            }
        }

        public static void ReplaceProcess<T>(string strProcessManagerName, int nObjectIndex, T T_object)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.ReplaceProcess(nObjectIndex, T_object);
            }
        }

        public static void ReplaceProcess<T>(string strProcessManagerName, int nObjectIndex, T T_object, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.ReplaceProcess(nObjectIndex, T_object);
            }
        }

        public static void DeleteProcess<T>(string strProcessManagerName, int nObjectIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.DeleteProcess(nObjectIndex);
            }
        }

        public static void DeleteProcess<T>(string strProcessManagerName, int nObjectIndex, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.DeleteProcess(nObjectIndex);
            }
        }

        public static void MoveToTop<T>(string strProcessManagerName, int nObjectIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.MoveToTop(nObjectIndex);
            }
        }

        public static void MoveToTop<T>(string strProcessManagerName, int nObjectIndex, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.MoveToTop(nObjectIndex);
            }
        }

        public static void MoveToBottom<T>(string strProcessManagerName, int nObjectIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.MoveToBottom(nObjectIndex);
            }
        }

        public static void MoveToBottom<T>(string strProcessManagerName, int nObjectIndex, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.MoveToBottom(nObjectIndex);
            }
        }

        public static void MoveToPrevious<T>(string strProcessManagerName, int nObjectIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.MoveToPrevious(nObjectIndex);
            }
        }

        public static void MoveToPrevious<T>(string strProcessManagerName, int nObjectIndex, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.MoveToPrevious(nObjectIndex);
            }
        }

        public static void MoveToNext<T>(string strProcessManagerName, int nObjectIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.MoveToNext(nObjectIndex);
            }
        }

        public static void MoveToNext<T>(string strProcessManagerName, int nObjectIndex, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.MoveToNext(nObjectIndex);
            }
        }

        public static bool Save<T>(string strProcessManagerName, string strFileFullPath)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                return managerResult.GetProcessManager.Save(strFileFullPath);
            }

            return false;
        }

        public static bool Save<T>(string strProcessManagerName, string strFileFullPath, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                return managerResult.GetProcessManager.Save(strFileFullPath);
            }

            return false;
        }

        public static bool Load<T>(string strProcessManagerName, string strFileFullPath)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                return managerResult.GetProcessManager.Load(strFileFullPath);
            }

            return false;
        }

        public static bool Load<T>(string strProcessManagerName, string strFileFullPath, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                return managerResult.GetProcessManager.Load(strFileFullPath);
            }

            return false;
        }

        public static void CreateNewProcessManager<T>(string strProcessManagerName)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.CreateNewProcessManager();
            }
        }

        public static void CreateNewProcessManager<T>(string strProcessManagerName, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.CreateNewProcessManager();
            }
        }

        public static void ClearSelectProcess<T>(string strProcessManagerName)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.ClearSelectProcess();
            }
        }

        public static void ClearSelectProcess<T>(string strProcessManagerName, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.ClearSelectProcess();
            }
        }

        public static void AddSelectIndex<T>(string strProcessManagerName, int nObjectIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.AddSelectIndex(nObjectIndex);
            }
        }

        public static void AddSelectIndex<T>(string strProcessManagerName, int nObjectIndex, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.AddSelectIndex(nObjectIndex);
            }
        }

        public static void SetSelectIndex<T>(string strProcessManagerName, List<int> selectedList)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.SetSelectIndex(selectedList);
            }
        }

        public static void SetSelectIndex<T>(string strProcessManagerName, List<int> selectedList, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.SetSelectIndex(selectedList);
            }
        }

        public static ProcessManagerResult<T> GetProcessByIndex<T>(string strProcessManagerName, int nObjectIndex)
        {
            ProcessManagerResult<T> result = new ProcessManagerResult<T>();
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                result.GetProcessManager = managerResult.GetProcessManager.GetProcessByIndex(nObjectIndex);
                return result;
            }

            result.OK = false;
            result.Error = managerResult.Error;
            return result;
        }

        public static ProcessManagerResult<T> GetProcessByIndex<T>(string strProcessManagerName, int nObjectIndex, int nListIndex)
        {
            ProcessManagerResult<T> result = new ProcessManagerResult<T>();
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                result.GetProcessManager = managerResult.GetProcessManager.GetProcessByIndex(nObjectIndex);
                return result;
            }

            result.OK = false;
            result.Error = managerResult.Error;
            return result;
        }

        public static int GetSelectedProcessIndex<T>(string strProcessManagerName, int nObjectIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName);
            if (managerResult.OK)
            {
                return managerResult.GetProcessManager.GetSelectedProcessIndex(nObjectIndex);
            }

            return -1;
        }

        public static int GetSelectedProcessIndex<T>(string strProcessManagerName, int nObjectIndex, int nListIndex)
        {
            ProcessManagerResult<ProcessManager<T>> managerResult = GetProcessManager<T>(strProcessManagerName, nListIndex);
            if (managerResult.OK)
            {
                return managerResult.GetProcessManager.GetSelectedProcessIndex(nObjectIndex);
            }

            return -1;
        }
    }
}
