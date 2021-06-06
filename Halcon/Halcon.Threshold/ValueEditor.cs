using CommonLibrary.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Halcon.Threshold
{
    public class ValueEditor: UITypeEditor
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
                TrackBarEditor trackBarEditor = new TrackBarEditor(0, 255);
                trackBarEditor.Value = Convert.ToInt32(value);
                edSvc.DropDownControl(trackBarEditor);
                return trackBarEditor.Value;
            }
            return value;
        }
    }
}
