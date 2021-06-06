using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.Controls
{
    public partial class PropertiesViewer : UserControl
    {
        private Action m_propertyValueHandle;

        public PropertiesViewer()
        {
            InitializeComponent();
        }

        public void SetProperties(object obj)
        {
            propertyGrid1.SelectedObject = obj;
        }

        public void SetHandle(Action action)
        {
            m_propertyValueHandle = action;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            m_propertyValueHandle?.Invoke();
        }
    }
}
