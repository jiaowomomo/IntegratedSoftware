using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.DataHelper
{
    public class RegisteredHelper
    {
        public static bool RegisteredKey(string _strKeyName, string _strValueName, object _vallue, RegistryValueKind _registryValueKind)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Registry.ClassesRoot.CreateSubKey(_strKeyName);
                registryKey.SetValue(_strValueName, _vallue, _registryValueKind);
                return true;
            }
            catch
            {
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
            return false;
        }

        public static bool DeleteKey(string _strKeyName)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Registry.ClassesRoot;
                registryKey.DeleteSubKey(_strKeyName);
                return true;
            }
            catch
            {
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
            return false;
        }

        public static bool IsRegistryKeyExist(string _strKeyName)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Registry.ClassesRoot;
                string[] keyNames = registryKey.GetSubKeyNames();
                return keyNames.Contains(_strKeyName);
            }
            catch
            {
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
            return false;
        }

        public static bool IsRegistryValueNameExist(string _strKeyName, string _strValueName)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Registry.ClassesRoot.CreateSubKey(_strKeyName);
                string[] valueNames = registryKey.GetValueNames();
                return valueNames.Contains(_strValueName);
            }
            catch
            {
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
            return false;
        }

        public static object GetKeyValue(string _strKeyName, string _strValueName)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Registry.ClassesRoot.CreateSubKey(_strKeyName);
                return registryKey.GetValue(_strValueName);
            }
            catch
            {
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Close();
                }
            }
            return null;
        }
    }
}
