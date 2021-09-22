using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.GrayMatch
{
    [Serializable]
    public class GrayMatch : IImageHalconObject
    {
        public HTemplate hTemplate;
        public double AngleStart = 0;
        public double AngleEnd = 0;
        public double ScaleMin = 0.9;
        public double ScaleMax = 1.1;
        public double MinScore = 0.5;
        public int MatchCount = 1;
        public double Overlap = 0.5;
        public int Pyramid = 5;
        public double Greediness = 0.5;
        public string ModelImage = "";
        public double ModelCenterX = 0;
        public double ModelCenterY = 0;

        public GrayMatch()
        {
            GetDataManager.AddOutputInt("匹配数量");
            GetDataManager.AddOutputDoubleArray("位置X");
            GetDataManager.AddOutputDoubleArray("位置Y");
            GetDataManager.AddOutputDoubleArray("角度");
            GetDataManager.AddOutputDoubleArray("平均偏差");
            GetDataManager.AddOutputDouble("轮廓中心X");
            GetDataManager.AddOutputDouble("轮廓中心Y");
        }

        public override void EditParameters()
        {
            GrayMatchForm shapeMatchForm = new GrayMatchForm(this);
            shapeMatchForm.ShowDialog();
            if (shapeMatchForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                hTemplate = shapeMatchForm.hTemplate.Clone();
                AngleStart = shapeMatchForm.AngleStart;
                AngleEnd = shapeMatchForm.AngleEnd;
                ScaleMin = shapeMatchForm.ScaleMin;
                ScaleMax = shapeMatchForm.ScaleMax;
                MinScore = shapeMatchForm.MinScore;
                MatchCount = shapeMatchForm.MatchCount;
                Overlap = shapeMatchForm.Overlap;
                Pyramid = shapeMatchForm.Pyramid;
                Greediness = shapeMatchForm.Greediness;
                ModelImage = shapeMatchForm.ModelImage;
                ROIList = shapeMatchForm.m_listROI;
                ModelCenterX = shapeMatchForm.ModelCenterX;
                ModelCenterY = shapeMatchForm.ModelCenterY;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
            if (shapeMatchForm.hTemplate != null)
            {
                shapeMatchForm.hTemplate.Dispose();
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            double angleStart = 3.14 * AngleStart / 180.0;
            double angleExtent = 3.14 * AngleEnd / 180.0 - angleStart;
            HTuple row, column, angle, error;
            if (source == null || source.Key == IntPtr.Zero)
            {
                throw new RunException(RunExceptionType.NoInputImage);
            }
            source.BestMatchRotMg(hTemplate, angleStart, angleExtent, 30, "false", 4, out row, out column, out angle, out error);
            if (angle.Length == 0)
            {
                throw new RunException(RunExceptionType.TemplateLookupFailed);
            }
            GetDataManager.SetOutputInt("匹配数量", row.Length);
            GetDataManager.SetOutputDoubleArray("位置X", column.DArr.ToList());
            GetDataManager.SetOutputDoubleArray("位置Y", row.DArr.ToList());
            GetDataManager.SetOutputDoubleArray("角度", angle.DArr.ToList());
            GetDataManager.SetOutputDoubleArray("平均偏差", error.DArr.ToList());
            GetDataManager.SetOutputDouble("轮廓中心X", ModelCenterX);
            GetDataManager.SetOutputDouble("轮廓中心Y", ModelCenterY);
            ROIManager rm = new ROIManager();
            rm.SetROIs(ROIList);
            HXLDCont hXLDCont = rm.GetRegions().GenContourRegionXld("border");
            for (int i = 0; i < angle.Length; i++)
            {
                //创建二维矩阵
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                hv_HomMat2D.VectorAngleToRigid(new HTuple(ModelCenterY), new HTuple(ModelCenterX), 0, row.TupleSelect(i), column.TupleSelect(i), angle.TupleSelect(i));
                //识别到的角度
                HHomMat2D transMat;
                transMat = hv_HomMat2D.HomMat2dRotate(angle.TupleSelect(i), 0, 0);
                //识别到的行列坐标
                transMat = transMat.HomMat2dTranslate(row.TupleSelect(i), column.TupleSelect(i));
                //变换
                HXLDCont findCont;
                findCont = hXLDCont.AffineTransContourXld(hv_HomMat2D);
                showObjects.Add(new Halcon.Functions.ShowObject(findCont.Clone(), "green"));
                findCont.Dispose();
            }
            hXLDCont.Dispose();
        }

        public override void SetParameters()
        {
            GrayMatchForm shapeMatchForm = new GrayMatchForm();
            shapeMatchForm.ShowDialog();
            if (shapeMatchForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                hTemplate = shapeMatchForm.hTemplate.Clone();
                AngleStart = shapeMatchForm.AngleStart;
                AngleEnd = shapeMatchForm.AngleEnd;
                ScaleMin = shapeMatchForm.ScaleMin;
                ScaleMax = shapeMatchForm.ScaleMax;
                MinScore = shapeMatchForm.MinScore;
                MatchCount = shapeMatchForm.MatchCount;
                Overlap = shapeMatchForm.Overlap;
                Pyramid = shapeMatchForm.Pyramid;
                Greediness = shapeMatchForm.Greediness;
                ModelImage = shapeMatchForm.ModelImage;
                ROIList = shapeMatchForm.m_listROI;
                ModelCenterX = shapeMatchForm.ModelCenterX;
                ModelCenterY = shapeMatchForm.ModelCenterY;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
            if (shapeMatchForm.hTemplate != null)
            {
                shapeMatchForm.hTemplate.Dispose();
            }
        }

        public override string ToolDescriptText()
        {
            return "灰度匹配";
        }

        public override string ToolName()
        {
            return "灰度匹配";
        }

        public override string ToolType()
        {
            return "定位工具";
        }
    }
}
