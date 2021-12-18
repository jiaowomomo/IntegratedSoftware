using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using HalconDotNet;
using System.IO;
using CommonLibrary.DataHelper;

namespace Camera.Common
{
    public class CameraControl
    {
        private const string ASSEMBLYNAME = "Camera";
        private static readonly string m_paramFullPath = Path.Combine(Application.StartupPath, "CtrlCard");
        private static readonly string m_cameraDLLPath = Path.Combine(Application.StartupPath, "CameraDLL");

        private static readonly Lazy<CameraControl> m_instance = new Lazy<CameraControl>(() => new CameraControl());

        private CameraType m_cameraType = CameraType.MindVision;
        private string m_strConfigPath = string.Empty;
        private List<ICamera> m_listCameras = new List<ICamera>();
        private ICamera m_camera = null;

        /// <summary>
        /// 图像改变委托
        /// </summary>
        /// <param name="hObject"></param>
        public delegate void ImageChangedEventHandler(HImage hObject, bool isHorizon = false, bool isVertical = false, bool isRotate = false);
        public ImageChangedEventHandler OnImageChdaged;

        public CameraType CameraType
        {
            get { return m_cameraType; }
            set { m_cameraType = value; }
        }

        public static CameraControl Instance { get => m_instance.Value; }

        private CameraControl()
        {
            Read();

            try
            {
                string strDLLName = $"{ASSEMBLYNAME}.{CameraType}";
                string strDLLPath = $"{Path.Combine(m_cameraDLLPath, strDLLName)}.dll";
                string strTypeName = $"{strDLLName}.{CameraType}Camera";
                m_camera = (ICamera)Assembly.LoadFile(strDLLPath).CreateInstance(strTypeName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Save()
        {
            IniHelper.AttributeToData(this, "Camera", "CameraParam.ini", m_paramFullPath);
        }

        public void Read()
        {
            IniHelper.DataToAttribute(this, "Camera", "CameraParam.ini", m_paramFullPath);
        }

        public List<ICamera> GetCameras()
        {
            return m_listCameras;
        }

        /// <summary>
        /// 是否初始化成功
        /// </summary>
        /// <returns></returns>
        public bool IsCameraExist(int index)
        {
            bool bIsExist = false;
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    bIsExist = m_listCameras[index].IsCameraExist();
                }
                else
                {
                    bIsExist = true;
                }
            }
            return bIsExist;
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        public void InitCamera()
        {
            if (m_camera != null)
            {
                m_listCameras = m_camera.InitCamera(/*showBox, nWidth, nHeight*/);
            }
        }

        /// <summary>
        /// 反初始化相机
        /// </summary>
        public void UnInitCamera()
        {
            for (int i = 0; i < m_listCameras.Count; i++)
            {
                if (m_listCameras[i] != null)
                {
                    m_listCameras[i].UnInitCamera();
                }
            }
        }

        /// <summary>
        /// 连续拍照
        /// </summary>
        public void Play(int index)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].Play();
                }
            }
        }

        /// <summary>
        /// 停止拍照
        /// </summary>
        public void Pause(int index)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].Pause();
                }
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveImage(int index, string filePath)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SaveImage(filePath);
                }
            }
        }

        /// <summary>
        /// 设置亮度
        /// </summary>
        /// <param name="nAeTarget">亮度</param>
        public void SetGainValue(int index, int nAeTarget)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SetGainValue(nAeTarget);
                }
            }
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        public void SetExposureTime(int index, double dbExposure)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SetExposureTime(dbExposure);
                }
            }
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void SetImageOverlay(int index)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SetImageOverlay();
                }
            }
        }

        /// <summary>
        /// 设置十字线
        /// </summary>
        /// <param name="nPosX">X坐标</param>
        /// <param name="nPosY">Y坐标</param>
        public void SetCrossLine(int index, int nPosX, int nPosY)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SetCrossLine(nPosX, nPosY);
                }
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        public void SaveParameter()
        {
            for (int i = 0; i < m_listCameras.Count; i++)
            {
                if (m_listCameras[i] != null)
                {
                    m_listCameras[i].SaveParameter(m_strConfigPath);
                }
            }
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        public void LoadParameter()
        {
            for (int i = 0; i < m_listCameras.Count; i++)
            {
                if (m_listCameras[i] != null)
                {
                    m_listCameras[i].LoadParameter(m_strConfigPath);
                }
            }
        }

        /// <summary>
        /// 设置触发帧数
        /// </summary>
        /// <param name="uCount">帧数</param>
        public void SetTriggerCount(int index, int uCount = 1)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SetTriggerCount(uCount);
                }
            }
        }

        /// <summary>
        /// 执行一次软触发
        /// </summary>
        public void SingleShoot(int index)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SingleShoot();
                }
            }
        }

        /// <summary>
        /// 设置相机触发模式
        /// </summary>
        /// <param name="nMode">0连续采集，1软件触发，2硬件触发</param>
        public void SetTriggerMode(int index, int nMode)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SetTriggerMode(nMode);
                }
            }
        }

        /// <summary>
        /// 抓拍一张图像到缓冲区中
        /// </summary>
        public void SnapToBuffer(int index)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SnapToBuffer();
                }
            }
        }

        /// <summary>
        /// 设置曝光模式
        /// </summary>
        /// <param name="uAeState">0停止自动曝光，1使能自动曝光</param>
        public void SetAeState(int index, uint uAeState)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SetAeState(uAeState);
                }
            }
        }

        /// <summary>
        /// 获取分辨率
        /// </summary>
        /// <returns>分辨率</returns>
        public string GetResolution(int index)
        {
            string str = string.Empty;
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    str = m_listCameras[index].GetResolution();
                }
            }
            return str;
        }

        public void SetImageRotate(int index, int nRot)
        {
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    m_listCameras[index].SetImageRotate(nRot);
                }
            }
        }

        public Image GetCurrentImage(int index)
        {
            Image image = null;
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    image = m_listCameras[index].BufferToImage();
                }
            }
            return image;
        }

        public HImage GetCurrentHImage(int index)
        {
            HImage hObject = null;
            if (m_listCameras.Count != 0)
            {
                if (m_listCameras[index] != null)
                {
                    hObject = m_listCameras[index].BufferToHImage();
                }
            }
            return hObject;
        }
    }
}
