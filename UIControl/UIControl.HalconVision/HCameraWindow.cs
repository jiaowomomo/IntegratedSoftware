using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera.Common;

namespace UIControl.HalconVision
{
    public partial class HCameraWindow : UserControl
    {
        private bool m_bIsStart = true;
        private bool m_bIsPause = false;

        public HCameraWindow()
        {
            InitializeComponent();
            CameraControl.Instance.OnImageChdaged += hObjectViewer1.SetImage;
            toolStripComboBox1.Items.Clear();
            for (int i = 0; i < CameraControl.Instance.GetCameras().Count; i++)
            {
                toolStripComboBox1.Items.Add(i);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //CameraControl.Instance.InitCamera(IntPtr.Zero, 512, 512);
            int index = toolStripComboBox1.SelectedIndex;
            if (index != -1)
            {
                if (CameraControl.Instance.IsCameraExist(index))
                {
                    if (m_bIsStart)
                    {
                        CameraControl.Instance.Play(index);
                        //CameraControl.Instance.SetHorizontalMirror();
                        //CameraControl.Instance.SetVerticalMirror();
                        CameraControl.Instance.SetExposureTime(index, Convert.ToDouble(numericUpDownExposure.Value));
                        SetUI(index);
                        toolStripButton1.Image = Properties.Resources.start_unenable;
                        toolStripButton2.Image = Properties.Resources.pause_enable;
                        toolStripButton1.Enabled = false;
                        toolStripButton2.Enabled = true;
                        toolStripComboBox1.Enabled = false;
                        m_bIsStart = false;
                        m_bIsPause = false;
                    }
                }
            }
        }

        private void SetUI(int index)
        {
            if (!CameraControl.Instance.GetCameras()[index].IsHorizonMirror)
            {
                toolStripButton3.Image = Properties.Resources.horizon_enable;
            }
            else
            {
                toolStripButton3.Image = Properties.Resources.horizon_unenable;
            }
            if (!CameraControl.Instance.GetCameras()[index].IsVerticalMirror)
            {
                toolStripButton4.Image = Properties.Resources.vertical_enable;
            }
            else
            {
                toolStripButton4.Image = Properties.Resources.vertical_unenable;
            }
            numericUpDownExposure.Value = Convert.ToDecimal(CameraControl.Instance.GetCameras()[index].ExposureTime);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int index = toolStripComboBox1.SelectedIndex;
            if (index != -1)
            {
                if (CameraControl.Instance.IsCameraExist(index))
                {
                    if (!m_bIsPause)
                    {
                        //pbCamera.Image = CameraControl.Instance.GetCurrentImage();
                        CameraControl.Instance.Pause(index);
                        toolStripButton1.Image = Properties.Resources.start_enable;
                        toolStripButton2.Image = Properties.Resources.pause_unenable;
                        toolStripButton1.Enabled = true;
                        toolStripButton2.Enabled = false;
                        toolStripComboBox1.Enabled = true;
                        m_bIsStart = true;
                        m_bIsPause = true;
                    }
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            int index = toolStripComboBox1.SelectedIndex;
            if (index != -1)
            {
                if (CameraControl.Instance.IsCameraExist(index))
                {
                    if (!CameraControl.Instance.GetCameras()[index].IsHorizonMirror)
                    {
                        CameraControl.Instance.GetCameras()[index].IsHorizonMirror = true;
                        //CameraControl.Instance.SetHorizontalMirror();
                        toolStripButton3.Image = Properties.Resources.horizon_unenable;
                    }
                    else
                    {
                        CameraControl.Instance.GetCameras()[index].IsHorizonMirror = false;
                        //CameraControl.Instance.SetHorizontalMirror();
                        toolStripButton3.Image = Properties.Resources.horizon_enable;
                    }
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            int index = toolStripComboBox1.SelectedIndex;
            if (index != -1)
            {
                if (CameraControl.Instance.IsCameraExist(index))
                {
                    if (!CameraControl.Instance.GetCameras()[index].IsVerticalMirror)
                    {
                        CameraControl.Instance.GetCameras()[index].IsVerticalMirror = true;
                        //CameraControl.Instance.SetVerticalMirror();
                        toolStripButton4.Image = Properties.Resources.vertical_unenable;
                    }
                    else
                    {
                        CameraControl.Instance.GetCameras()[index].IsVerticalMirror = false;
                        //CameraControl.Instance.SetVerticalMirror();
                        toolStripButton4.Image = Properties.Resources.vertical_enable;
                    }
                }
            }
        }

        public void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            CameraControl.Instance.UnInitCamera();
        }

        private void numericUpDownExposure_ValueChanged(object sender, EventArgs e)
        {
            int index = toolStripComboBox1.SelectedIndex;
            if (index != -1)
            {
                if (CameraControl.Instance.IsCameraExist(index))
                {
                    CameraControl.Instance.SetAeState(index, 0);
                    CameraControl.Instance.GetCameras()[index].ExposureTime = Convert.ToDouble(numericUpDownExposure.Value);
                    CameraControl.Instance.SetExposureTime(index, Convert.ToDouble(numericUpDownExposure.Value));
                }
            }
        }

        private void checkBoxShowCross_CheckedChanged(object sender, EventArgs e)
        {
            hObjectViewer1.IsShowCross = checkBoxShowCross.Checked;
        }

        private void checkBoxSetCross_CheckedChanged(object sender, EventArgs e)
        {
            hObjectViewer1.IsSetCross = checkBoxSetCross.Checked;
        }

        public void ReleaseRam()
        {
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = toolStripComboBox1.SelectedIndex;
            if(index!=-1)
            {
                SetUI(index);
            }
        }
    }
}
