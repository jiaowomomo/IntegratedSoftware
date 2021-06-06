using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halcon.CoordinateConversion
{
    [Serializable]
    public class CoordinateConversion : IImageHalconObject
    {
        public string strType = "";
        public int nIndex = 0;

        public CoordinateConversion()
        {
            GetDataManager.AddInputDoubleArray("输入X像素");
            GetDataManager.AddInputDoubleArray("输入Y像素");
            GetDataManager.AddOutputDoubleArray("输出X坐标");
            GetDataManager.AddOutputDoubleArray("输出Y坐标");
        }

        public override void EditParameters()
        {
            CoordinateConversionForm coordinateConversionForm = new CoordinateConversionForm(this);
            coordinateConversionForm.ShowDialog();
            if (coordinateConversionForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                strType = coordinateConversionForm.strType;
                nIndex = coordinateConversionForm.nIndex;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            string path = Application.StartupPath + @"\" + strType + @"\calib" + nIndex.ToString() + @".data";
            HHomMat2D pixToWorld;
            HTuple tuple;
            if (File.Exists(path))
            {
                HOperatorSet.ReadTuple(path, out tuple);
                pixToWorld = new HHomMat2D(tuple);
            }
            else
            {
                pixToWorld = new HHomMat2D();
            }
            HTuple Row = new HTuple(GetDataManager.GetInputDoubleArray("输入Y像素").ToArray());
            HTuple Column = new HTuple(GetDataManager.GetInputDoubleArray("输入X像素").ToArray());
            HTuple qx, qy;
            HOperatorSet.AffineTransPoint2d(pixToWorld, Column, Row, out qx, out qy);
            GetDataManager.SetOutputDoubleArray("输出X坐标", qx.ToDArr().ToList());
            GetDataManager.SetOutputDoubleArray("输出Y坐标", qx.ToDArr().ToList());
        }

        public override void SetParameters()
        {
            CoordinateConversionForm coordinateConversionForm = new CoordinateConversionForm();
            coordinateConversionForm.ShowDialog();
            if (coordinateConversionForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                strType = coordinateConversionForm.strType;
                nIndex = coordinateConversionForm.nIndex;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "坐标系转换";
        }

        public override string ToolName()
        {
            return "坐标系转换";
        }

        public override string ToolType()
        {
            return "数学工具";
        }
    }
}
