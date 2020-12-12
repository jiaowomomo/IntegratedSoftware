using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Base
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class ShowerAttribute : Attribute
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public ShowerAttribute(string name)
        {
            this.Name = name;
        }
    }
}
