using CommonLibrary.ExtensionUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.ManualCalibration
{
    public class ManualCalibration : IFormMenu
    {
        public string MainToolStrip => "标定";

        public string SubToolStrip => "手动标定";

        public void ExecuteTool()
        {
            ManualCalibrationForm manualCalibrationForm = new ManualCalibrationForm();
            manualCalibrationForm.ShowDialog();
        }
    }
}
