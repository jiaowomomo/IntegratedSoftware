using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Halcon.Functions
{
    public class ViewerToolEditor : UITypeEditor
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
                CheckedListBox checkedListBox = new CheckedListBox();
                checkedListBox.CheckOnClick = true;
                string[] items = Enum.GetNames(typeof(ViewerTools));
                List<bool> checkedList = value as List<bool>;
                for (int i = 0; i < items.Length; i++)
                {
                    if (i < checkedList.Count)
                    {
                        checkedListBox.Items.Add(items[i], checkedList[i]);
                    }
                    else
                    {
                        checkedListBox.Items.Add(items[i], true);
                    }
                }
                edSvc.DropDownControl(checkedListBox);
                checkedList = new List<bool>();
                for (int i = 0; i < checkedListBox.Items.Count; i++)
                {
                    checkedList.Add(checkedListBox.GetItemChecked(i));
                }
                return checkedList;
            }
            return value;
        }
    }
}
