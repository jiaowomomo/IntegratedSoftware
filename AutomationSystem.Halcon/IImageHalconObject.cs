using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HalconDotNet;

namespace AutomationSystem.Halcon
{
    [Serializable]
    public abstract class IImageHalconObject
    {
        public bool IsSelected { get; set; } = false;
        public bool IsRunOK { get; set; } = false;
        public bool IsSetupOK { get; set; } = false;
        public long lUsedTime { get; set; } = 0;
        public string ErrorMessage { get; set; } = "";

        public DataManager GetDataManager = new DataManager();
        public List<ROIBase> m_listROI = new List<ROIBase>();

        public IImageHalconObject()
        {
        }

        public abstract void SetParameters();
        public abstract void EditParameters();
        public abstract void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts);
        public abstract string ToolDescriptText();
        public abstract string ToolName();
        public abstract string ToolType();

        public /*HImage*/void ImageHandle(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            IsRunOK = false;
            try
            {
                GetDataManager.ClearData();
                Execute(ref source, ref showObjects, ref showTexts);
                IsRunOK = true;
            }
            catch (RunException he)
            {
                ErrorMessage = he.Message;
                IsRunOK = false;
            }
            //sw.Stop();
            //lUsedTime = sw.ElapsedMilliseconds;
            //return source;
        }
    }
}
