using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.CodeSystem.Controls
{
    public partial class RenameForm : Form
    {
        public SubType SubType => (SubType)comboBoxType.SelectedIndex;
        public string MethodName => textBoxMethod.Text;

        public RenameForm()
        {
            InitializeComponent();

            comboBoxType.SelectedIndex = 0;
        }
    }
}
