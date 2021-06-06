using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonLibrary.DataHelper
{
    public static class AccessHelper
    {
        public static void CreateDataBase(string _strSqlCmd, string _strConnectionCmd)
        {
            try
            {
                string strCreateDBCmd = _strConnectionCmd + "Jet OLEDB:Engine Type=5";
                ADOX.Catalog catalog = new ADOX.Catalog();
                catalog.Create(strCreateDBCmd);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog.ActiveConnection);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog);
                ExecuteNonQuery(_strSqlCmd, _strConnectionCmd);
            }
            catch (Exception ex)
            {
            }
        }

        public static int ExecuteNonQuery(string _strSqlCmd, string _strConnectionCmd)
        {
            try
            {
                string strConnectionCmd = _strConnectionCmd + "Persist Security Info=False";
                using (OleDbConnection connection = new OleDbConnection(strConnectionCmd))
                {
                    using (OleDbCommand cmd = new OleDbCommand(_strSqlCmd, connection))
                    {
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static DataTable QuerySQL(string _strSqlCmd, string _strConnectionCmd)
        {
            string strConnectionCmd = _strConnectionCmd + "Persist Security Info=False";
            try
            {
                using (OleDbConnection connection = new OleDbConnection(strConnectionCmd))
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (OleDbCommand cmd = new OleDbCommand(_strSqlCmd, connection))
                    {
                        OleDbDataReader odr = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(odr);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataTable Adapter(string _strSqlCmd, string _strConnectionCmd, params OleDbParameter[] param)
        {
            DataTable dt = new DataTable();
            using (OleDbDataAdapter oda = new OleDbDataAdapter(_strSqlCmd, _strConnectionCmd))
            {
                if (param != null && param.Length != 0)
                {
                    oda.SelectCommand.Parameters.AddRange(param);
                }
                if (new OleDbConnection().State == ConnectionState.Closed)
                {
                    new OleDbConnection(_strConnectionCmd).Open();
                }
                oda.Fill(dt);
            }
            return dt;
        }

        public static int CheckAccessVersion()
        {
            int version = 0;
            RegistryKey rk = Registry.LocalMachine;
            //office 97
            RegistryKey v97 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\8.0\Access\InstallRoot\");
            //office 2000
            RegistryKey v2000 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\9.0\Access\InstallRoot\");
            //office xp
            RegistryKey vxp = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\10.0\Access\InstallRoot\");
            //office 2003
            RegistryKey v2003 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\11.0\Access\InstallRoot\");
            //查询2007
            RegistryKey v2007 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\12.0\Access\InstallRoot\");
            //查询2010
            RegistryKey v2010 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\14.0\Access\InstallRoot\");
            //查询2013
            RegistryKey v2013 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\15.0\Access\InstallRoot\");
            //查询2016
            RegistryKey v2016 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\16.0\Access\InstallRoot\");
            if (v97 != null)
            {
                string file97 = v97.GetValue("Path").ToString();
                if (File.Exists(file97 + "MSACCESS.EXE"))
                {
                    version = 1997;
                    goto Finish;
                }
            }
            if (v2000 != null)
            {
                string file2000 = v2000.GetValue("Path").ToString();
                if (File.Exists(file2000 + "MSACCESS.EXE"))
                {
                    version = 2000;
                    goto Finish;
                }
            }
            if (vxp != null)
            {
                string filexp = vxp.GetValue("Path").ToString();
                if (File.Exists(filexp + "MSACCESS.EXE"))
                {
                    version = 2001;
                    goto Finish;
                }
            }
            if (v2003 != null)
            {
                string file03 = v2003.GetValue("Path").ToString();
                if (File.Exists(file03 + "MSACCESS.EXE"))
                {
                    version = 2003;
                    goto Finish;
                }
            }
            if (v2007 != null)
            {
                string office07 = v2007.GetValue("Path").ToString();
                if (File.Exists(office07 + "MSACCESS.EXE"))
                {
                    version = 2007;
                    goto Finish;
                }
            }
            if (v2010 != null)
            {
                string office10 = v2010.GetValue("Path").ToString();
                if (File.Exists(office10 + "MSACCESS.EXE"))
                {
                    version = 2010;
                    goto Finish;
                }
            }
            if (v2013 != null)
            {
                string office13 = v2013.GetValue("Path").ToString();
                if (File.Exists(office13 + "MSACCESS.EXE"))
                {
                    version = 2013;
                    goto Finish;
                }
            }
            if (v2016 != null)
            {
                string office16 = v2016.GetValue("Path").ToString();
                if (File.Exists(office16 + "MSACCESS.EXE"))
                {
                    version = 2016;
                    goto Finish;
                }
            }
        Finish:
            {
                if (rk != null)
                {
                    rk.Close();
                }
                if (v97 != null)
                {
                    v97.Close();
                }
                if (v2000 != null)
                {
                    v2000.Close();
                }
                if (vxp != null)
                {
                    vxp.Close();
                }
                if (v2003 != null)
                {
                    v2003.Close();
                }
                if (v2007 != null)
                {
                    v2007.Close();
                }
                if (v2010 != null)
                {
                    v2010.Close();
                }
                if (v2013 != null)
                {
                    v2013.Close();
                }
                if (v2016 != null)
                {
                    v2016.Close();
                }
                return version;
            }
        }
    }
}
