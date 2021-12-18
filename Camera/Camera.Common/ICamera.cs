using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.Common
{
    public abstract class ICamera
    {
        private bool m_bIsHorizonMirror = false;
        private bool m_bIsVerticalMirror = false;
        private bool m_bIsRotate = false;
        private double m_dbExposureTime = 5000;
        private int m_nGain = 1;

        public bool IsHorizonMirror
        {
            get { return m_bIsHorizonMirror; }
            set { m_bIsHorizonMirror = value; }
        }

        public bool IsVerticalMirror
        {
            get { return m_bIsVerticalMirror; }
            set { m_bIsVerticalMirror = value; }
        }

        public bool IsRotate
        {
            get { return m_bIsRotate; }
            set { m_bIsRotate = value; }
        }

        public double ExposureTime
        {
            get { return m_dbExposureTime; }
            set { m_dbExposureTime = value; }
        }

        public int Gain
        {
            get { return m_nGain; }
            set { m_nGain = value; }
        }

        /// <summary>
        /// 是否初始化成功
        /// </summary>
        /// <returns></returns>
        public abstract bool IsCameraExist();

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="showBox">展示窗体</param>
        /// <param name="nWidth">窗体宽度</param>
        /// <param name="nHeight">窗体高度</param>
        public abstract List<ICamera> InitCamera(/*IntPtr showBox, int nWidth, int nHeight*/);

        /// <summary>
        /// 初始化迈德威视相机
        /// </summary>
        /// <param name="showBox">展示窗体</param>
        /// <param name="nWidth">窗体宽度</param>
        /// <param name="nHeight">窗体高度</param>
        //void MindVisionInit(IntPtr showBox, int nWidth, int nHeight);

        /// <summary>
        /// 反初始化相机
        /// </summary>
        public abstract void UnInitCamera();

        /// <summary>
        /// 连续拍照
        /// </summary>
        public abstract void Play();

        /// <summary>
        /// 停止拍照
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="filePath"></param>
        public abstract void SaveImage(string filePath);

        /// <summary>
        /// 图像旋转
        /// </summary>
        /// <param name="nRot">表示旋转的角度（逆时针方向）（0：不旋转 1:90度 2:180度 3:270度</param>
        public abstract void SetImageRotate(int nRot);

        /// <summary>
        /// 设置增益值
        /// </summary>
        /// <param name="nAeTarget">增益值</param>
        public abstract void SetGainValue(int nAeTarget);

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        public abstract void SetExposureTime(double dbExposureTime);

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public abstract void SetImageOverlay();

        /// <summary>
        /// 设置十字线
        /// </summary>
        /// <param name="nPosX">X坐标</param>
        /// <param name="nPoxY">Y坐标</param>
        public abstract void SetCrossLine(int nPosX, int nPoxY);

        /// <summary>
        /// 保存参数
        /// </summary>
        public abstract void SaveParameter(string configPath);

        /// <summary>
        /// 加载参数
        /// </summary>
        public abstract void LoadParameter(string configPath);

        /// <summary>
        /// 设置触发帧数
        /// </summary>
        /// <param name="uCount">帧数</param>
        public abstract void SetTriggerCount(int uCount = 1);

        /// <summary>
        /// 执行一次软触发
        /// </summary>
        public abstract void SingleShoot();

        /// <summary>
        /// 设置相机触发模式
        /// </summary>
        /// <param name="nMode">0连续采集，1软件触发，2硬件触发</param>
        public abstract void SetTriggerMode(int nMode);

        /// <summary>
        /// 抓拍一张图像到缓冲区中
        /// </summary>
        public abstract void SnapToBuffer();

        /// <summary>
        /// 设置曝光模式
        /// </summary>
        /// <param name="uAeState">0停止自动曝光，1使能自动曝光</param>
        public abstract void SetAeState(uint uAeState);

        /// <summary>
        /// 获取分辨率
        /// </summary>
        /// <returns>分辨率</returns>
        public abstract string GetResolution();

        /// <summary>
        /// 将缓存区数据转换为窗体图像
        /// </summary>
        /// <returns></returns>
        public abstract Image BufferToImage();

        /// <summary>
        /// 将缓存区数据转换为halcon格式
        /// </summary>
        /// <returns></returns>
        public abstract HImage BufferToHImage();
    }
}
