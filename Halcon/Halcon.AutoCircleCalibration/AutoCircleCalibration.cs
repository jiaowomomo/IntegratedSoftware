using CommonLibrary.ExtensionUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.AutoCircleCalibration
{
    public class AutoCircleCalibration : IFormMenu
    {
        public string MainToolStrip => "标定";

        public string SubToolStrip => "自动标定";

        public void ExecuteTool()
        {
            AutoCircleCalibrationForm autoCircleCalibration = new AutoCircleCalibrationForm();
            autoCircleCalibration.ShowDialog();
        }
    }
}
