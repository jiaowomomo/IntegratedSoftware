using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.ShapeMatch
{
    [Serializable]
    public class ShapeMatch : IImageHalconObject
    {
        public HShapeModel hShapeModel;
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

        public ShapeMatch()
        {
            GetDataManager.AddOutputInt("匹配数量");
            GetDataManager.AddOutputDoubleArray("位置X");
            GetDataManager.AddOutputDoubleArray("位置Y");
            GetDataManager.AddOutputDoubleArray("角度");
            GetDataManager.AddOutputDoubleArray("缩放比");
            GetDataManager.AddOutputDoubleArray("匹配度");
            GetDataManager.AddOutputDouble("轮廓中心X");
            GetDataManager.AddOutputDouble("轮廓中心Y");
        }

        public override void EditParameters()
        {
            ShapeMatchForm shapeMatchForm = new ShapeMatchForm(this);
            shapeMatchForm.ShowDialog();
            if (shapeMatchForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                hShapeModel = shapeMatchForm.hShapeModel.Clone();
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
            if (shapeMatchForm.hShapeModel != null)
            {
                shapeMatchForm.hShapeModel.Dispose();
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            double angleStart = 3.14 * AngleStart / 180.0;
            double angleExtent = 3.14 * AngleEnd / 180.0 - angleStart;
            HTuple row, column, angle, scale, score;
            if (source == null || source.Key == IntPtr.Zero)
            {
                throw new RunException(RunExceptionType.NoInputImage);
            }
            source.FindScaledShapeModel(hShapeModel, angleStart, angleExtent, ScaleMin, ScaleMax, MinScore, MatchCount, Overlap, "least_squares", Pyramid, Greediness, out row, out column, out angle, out scale, out score);
            if (score.Length == 0)
            {
                throw new RunException(RunExceptionType.TemplateLookupFailed);
            }
            GetDataManager.SetOutputInt("匹配数量", row.Length);
            GetDataManager.SetOutputDoubleArray("位置X", column.DArr.ToList());
            GetDataManager.SetOutputDoubleArray("位置Y", row.DArr.ToList());
            GetDataManager.SetOutputDoubleArray("角度", angle.DArr.ToList());
            GetDataManager.SetOutputDoubleArray("缩放比", scale.DArr.ToList());
            GetDataManager.SetOutputDoubleArray("匹配度", score.DArr.ToList());
            GetDataManager.SetOutputDouble("轮廓中心X", ModelCenterX);
            GetDataManager.SetOutputDouble("轮廓中心Y", ModelCenterY);
            HXLDCont hXLDCont = hShapeModel.GetShapeModelContours(1);
            for (int i = 0; i < score.Length; i++)
            {
                //创建二维矩阵
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                hv_HomMat2D.HomMat2dIdentity();
                //识别到的角度
                HHomMat2D transMat;
                transMat = hv_HomMat2D.HomMat2dRotate(angle.TupleSelect(i), 0, 0);
                //识别到的行列坐标
                transMat = transMat.HomMat2dTranslate(row.TupleSelect(i), column.TupleSelect(i));
                //变换
                HXLDCont findCont;
                findCont = hXLDCont.AffineTransContourXld(transMat);
                showObjects.Add(new Halcon.Functions.ShowObject(findCont.Clone(), "green"));
                findCont.Dispose();
            }
            hXLDCont.Dispose();
        }

        public override void SetParameters()
        {
            ShapeMatchForm shapeMatchForm = new ShapeMatchForm();
            shapeMatchForm.ShowDialog();
            if (shapeMatchForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                hShapeModel = shapeMatchForm.hShapeModel.Clone();
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
            if (shapeMatchForm.hShapeModel != null)
            {
                shapeMatchForm.hShapeModel.Dispose();
            }
        }

        public override string ToolDescriptText()
        {
            return "形状匹配";
        }

        public override string ToolName()
        {
            return "形状匹配";
        }

        public override string ToolType()
        {
            return "定位工具";
        }
    }
}
