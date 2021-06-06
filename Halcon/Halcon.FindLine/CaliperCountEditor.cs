using CommonLibrary.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Halcon.FindLine
{
    public class CaliperCountEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                TrackBarEditor trackBarEditor = new TrackBarEditor(1, 100);
                trackBarEditor.Value = Convert.ToInt32(value);
                edSvc.DropDownControl(trackBarEditor);
                return trackBarEditor.Value;
            }
            return value;
        }
    }
}
