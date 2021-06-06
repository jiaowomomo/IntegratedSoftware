using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    public static class DirectoryUtils
    {
        public static List<string> CreateAndReturnFailureDirectorys(IEnumerable<string> directorys)
        {
            List<string> failureDirectorys = new List<string>();
            foreach (string directory in directorys)
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch
                {
                    failureDirectorys.Add(directory);
                }
            }
            return failureDirectorys;
        }
    }
}
