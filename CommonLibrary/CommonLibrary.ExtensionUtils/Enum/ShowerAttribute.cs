using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class ShowerAttribute : Attribute
    {
        private string m_strName;

        public string Name { get => m_strName; }

        public ShowerAttribute(string name)
        {
            m_strName = name;
        }
    }
}
