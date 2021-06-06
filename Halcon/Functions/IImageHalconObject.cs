using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HalconDotNet;

namespace Halcon.Functions
{
    [Serializable]
    public abstract class IImageHalconObject
    {
        [Browsable(false)]
        public bool IsSelected { get; set; } = false;
        [Browsable(false)]
        public bool IsRunOK { get; set; } = false;
        [Browsable(false)]
        public bool IsSetupOK { get; set; } = false;
        [Browsable(false)]
        public long UsedTime { get; set; } = 0;
        [Browsable(false)]
        public string ErrorMessage { get; set; } = string.Empty;

        public DataManager GetDataManager = new DataManager();
        public List<ROIBase> ROIList = new List<ROIBase>();

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
