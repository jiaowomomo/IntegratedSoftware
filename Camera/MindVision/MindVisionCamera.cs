using Camera.Common;
using HalconDotNet;
using MVSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera.MindVision
{
    public class MindVisionCamera : ICamera
    {
        protected int m_nCameraHandle = 0;//句柄
        protected IntPtr m_imageBuffer;// 预览通道RGB图像缓存
        protected tSdkCameraCapbility m_tCameraCapability;// 相机特性描述
        protected IntPtr m_imageBufferSnapshot;// 抓拍通道RGB图像缓存
        protected tSdkFrameHead m_frameHead;//图像信息
        protected IntPtr m_uRawBuffer;//rawbuffer由SDK内部申请。应用层不要调用delete之类的释放函数
        protected CameraSdkStatus m_status;//SDK接口返回值

        private Thread m_tCaptureThread;
        private bool m_bIsStartThread = false;

        public MindVisionCamera()
        {
        }

        /// <summary>
        /// 是否初始化成功
        /// </summary>
        /// <returns></returns>
        public override bool IsCameraExist()
        {
            if (m_nCameraHandle == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="showBox">展示窗体</param>
        /// <param name="nWidth">窗体宽度</param>
        /// <param name="nHeight">窗体高度</param>
        public override List<ICamera> InitCamera(/*IntPtr showBox, int nWidth, int nHeight*/)
        {
            return MindVisionInit(IntPtr.Zero, 512, 512);
        }

        /// <summary>
        /// 初始化迈德威视相机
        /// </summary>
        /// <param name="showBox">展示窗体</param>
        /// <param name="nWidth">窗体宽度</param>
        /// <param name="nHeight">窗体高度</param>
        public List<ICamera> MindVisionInit(IntPtr showBox, int nWidth, int nHeight)
        {
            List<ICamera> result = new List<ICamera>();
            tSdkCameraDevInfo[] tCameraDevInfoList;
            if (m_nCameraHandle > 0)
            {
                //已经初始化过,返回
                return result;
            }

            m_status = MvApi.CameraEnumerateDevice(out tCameraDevInfoList);// 枚举设备，并建立设备列表
            if (m_status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
            {
                if (tCameraDevInfoList != null)//此时iCameraCounts返回了实际连接的相机个数。如果大于1，则初始化第一个相机
                {
                    for (int i = 0; i < tCameraDevInfoList.Length; i++)
                    {
                        MindVisionCamera mindVisionCamera = new MindVisionCamera();
                        m_status = MvApi.CameraInit(ref tCameraDevInfoList[i], -1, -1, ref mindVisionCamera.m_nCameraHandle);
                        if (m_status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                        {
                            //获得相机特性描述
                            MvApi.CameraGetCapability(mindVisionCamera.m_nCameraHandle, out mindVisionCamera.m_tCameraCapability);
                            mindVisionCamera.m_imageBuffer = Marshal.AllocHGlobal(mindVisionCamera.m_tCameraCapability.sResolutionRange.iWidthMax * mindVisionCamera.m_tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);
                            mindVisionCamera.m_imageBufferSnapshot = Marshal.AllocHGlobal(mindVisionCamera.m_tCameraCapability.sResolutionRange.iWidthMax * mindVisionCamera.m_tCameraCapability.sResolutionRange.iHeightMax * 3 + 1024);

                            //初始化显示模块，使用SDK内部封装好的显示接口
                            //MvApi.CameraDisplayInit(nCameraHandle, showBox);
                            //MvApi.CameraSetDisplaySize(nCameraHandle, nWidth, nHeight);

                            //设置抓拍通道的分辨率。
                            tSdkImageResolution tResolution;
                            tResolution.uSkipMode = 0;
                            tResolution.uBinAverageMode = 0;
                            tResolution.uBinSumMode = 0;
                            tResolution.uResampleMask = 0;
                            tResolution.iVOffsetFOV = 0;
                            tResolution.iHOffsetFOV = 0;
                            tResolution.iWidthFOV = mindVisionCamera.m_tCameraCapability.sResolutionRange.iWidthMax;
                            tResolution.iHeightFOV = mindVisionCamera.m_tCameraCapability.sResolutionRange.iHeightMax;
                            tResolution.iWidth = tResolution.iWidthFOV;
                            tResolution.iHeight = tResolution.iHeightFOV;
                            //tResolution.iIndex = 0xff;表示自定义分辨率,如果tResolution.iWidth和tResolution.iHeight
                            //定义为0，则表示跟随预览通道的分辨率进行抓拍。抓拍通道的分辨率可以动态更改。
                            //本例中将抓拍分辨率固定为最大分辨率。
                            tResolution.iIndex = 0xff;
                            tResolution.acDescription = new byte[32];//描述信息可以不设置
                            tResolution.iWidthZoomHd = 0;
                            tResolution.iHeightZoomHd = 0;
                            tResolution.iWidthZoomSw = 0;
                            tResolution.iHeightZoomSw = 0;

                            MvApi.CameraSetResolutionForSnap(mindVisionCamera.m_nCameraHandle, ref tResolution);

                            //让SDK来根据相机的型号动态创建该相机的配置窗口。
                            //MvApi.CameraCreateSettingPage(pCameraHandle,this.Handle,tCameraDevInfoList[0].acFriendlyName,/*SettingPageMsgCalBack*/null,/*m_iSettingPageMsgCallbackCtx*/(IntPtr)null,0);

                            mindVisionCamera.m_bIsStartThread = true;
                            mindVisionCamera.m_tCaptureThread = new Thread(new ThreadStart(mindVisionCamera.CaptureThreadProc));
                            mindVisionCamera.m_tCaptureThread.Start();

                            result.Add(mindVisionCamera);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 反初始化相机
        /// </summary>
        public override void UnInitCamera()
        {
            if (m_nCameraHandle > 0)
            {
                m_bIsStartThread = false;
                while (m_tCaptureThread != null && m_tCaptureThread.IsAlive)
                {

                    Thread.Sleep(10);
                }
                MvApi.CameraUnInit(m_nCameraHandle);//相机反初始化。释放资源。
                Marshal.FreeHGlobal(m_imageBuffer);
                Marshal.FreeHGlobal(m_imageBufferSnapshot);
                m_nCameraHandle = 0;
            }
        }

        /// <summary>
        /// 连续拍照
        /// </summary>
        public override void Play()
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraPlay(m_nCameraHandle);//让SDK进入工作模式，开始接收来自相机发送的图像数据。如果当前相机是触发模式，则需要接收到触发帧以后才会更新图像。
            }
        }

        /// <summary>
        /// 停止拍照
        /// </summary>
        public override void Pause()
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraPause(m_nCameraHandle);//让SDK进入暂停模式，不接收来自相机的图像数据，同时也会发送命令让相机暂停输出，释放传输带宽。暂停模式下，可以对相机的参数进行配置，并立即生效。  
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="filePath"></param>
        public override void SaveImage(string filePath)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSaveImage(m_nCameraHandle, filePath, m_imageBuffer, ref m_frameHead, emSdkFileType.FILE_PNG, 0);
            }
        }

        /// <summary>
        /// 左右镜像
        /// </summary>
        /// <param name="uEnable">0不镜像，1镜像</param>
        public void SetHorizontalMirror(uint uHorizon)
        {
            if (m_nCameraHandle > 0)
            {
                // 功能描述 : 设置图像镜像操作。镜像操作分为水平和垂直两个方向。
                // 参数     : hCamera  相机的句柄，由CameraInit函数获得。
                //            iDir     表示镜像的方向。0，表示水平方向；1，表示垂直方向。
                //            bEnable  TRUE，使能镜像;FALSE，禁止镜像
                // 返回值   : 成功时，返回CAMERA_STATUS_SUCCESS (0);
                MvApi.CameraSetMirror(m_nCameraHandle, 0, uHorizon);
            }
        }

        /// <summary>
        /// 上下镜像
        /// </summary>
        /// <param name="uEnable">0不镜像，1镜像</param>
        public void SetVerticalMirror(uint uVertical)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSetMirror(m_nCameraHandle, 1, uVertical);
            }
        }

        /// <summary>
        /// 设置亮度
        /// </summary>
        /// <param name="nAeTarget">亮度</param>
        public override void SetGainValue(int nAeTarget)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSetAnalogGain(m_nCameraHandle, nAeTarget);
            }
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        public override void SetExposureTime(double dbExposureTime)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSetExposureTime(m_nCameraHandle, dbExposureTime);
            }
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public override void SetImageOverlay()
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraImageOverlay(m_nCameraHandle, m_imageBuffer, ref m_frameHead);
            }
        }

        /// <summary>
        /// 设置十字线
        /// </summary>
        /// <param name="nPosX">X坐标</param>
        /// <param name="nPoxY">Y坐标</param>
        public override void SetCrossLine(int nPosX, int nPoxY)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSetCrossLine(m_nCameraHandle, 0, nPosX, nPoxY, 0, 1);
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        public override void SaveParameter(string configPath)
        {
            if (m_nCameraHandle > 0)
            {
                //文件后缀必须是config结尾
                MvApi.CameraSaveParameterToFile(m_nCameraHandle, configPath);//功能描述 : 保存当前相机参数到指定的文件中。该文件可以复制到 别的电脑上供其他相机加载，也可以做参数备份用。
            }
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        public override void LoadParameter(string configPath)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraReadParameterFromFile(m_nCameraHandle, configPath);
            }
        }

        /// <summary>
        /// 设置触发帧数
        /// </summary>
        /// <param name="uCount">帧数</param>
        public override void SetTriggerCount(int uCount = 1)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSetTriggerCount(m_nCameraHandle, uCount);
            }
        }

        /// <summary>
        /// 执行一次软触发
        /// </summary>
        public override void SingleShoot()
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSoftTrigger(m_nCameraHandle);
            }
        }

        /// <summary>
        /// 设置相机触发模式
        /// </summary>
        /// <param name="nMode">0连续采集，1软件触发，2硬件触发</param>
        public override void SetTriggerMode(int nMode)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSetTriggerMode(m_nCameraHandle, nMode);
            }
        }

        /// <summary>
        /// 抓拍一张图像到缓冲区中
        /// </summary>
        public override void SnapToBuffer()
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSnapToBuffer(m_nCameraHandle, out m_frameHead, out m_imageBuffer, 2000);
            }
        }

        /// <summary>
        /// 设置曝光模式
        /// </summary>
        /// <param name="uAeState">0停止自动曝光，1使能自动曝光</param>
        public override void SetAeState(uint uAeState)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSetAeState(m_nCameraHandle, uAeState);
            }
        }

        /// <summary>
        /// 获取分辨率
        /// </summary>
        /// <returns>分辨率</returns>
        public override string GetResolution()
        {
            string sFrameInfomation = m_frameHead.iWidth.ToString() + "*" + m_frameHead.iHeight.ToString();
            return sFrameInfomation;
        }

        private void CaptureThreadProc()
        {
            //CameraSdkStatus eStatus;
            //tSdkFrameHead FrameHead;
            //IntPtr uRawBuffer;//rawbuffer由SDK内部申请。应用层不要调用delete之类的释放函数

            while (m_bIsStartThread)
            {
                //2000毫秒超时,图像没捕获到前，线程会被挂起,释放CPU，所以该线程中无需调用sleep
                m_status = MvApi.CameraGetImageBuffer(m_nCameraHandle, out m_frameHead, out m_uRawBuffer, 2000);

                if (m_status == CameraSdkStatus.CAMERA_STATUS_SUCCESS)//如果是触发模式，则有可能超时
                {
                    //图像处理，将原始输出转换为RGB格式的位图数据，同时叠加白平衡、饱和度、LUT等ISP处理。
                    MvApi.CameraImageProcess(m_nCameraHandle, m_uRawBuffer, m_imageBuffer, ref m_frameHead);
                    if (CameraControl.Instance.OnImageChdaged != null)
                    {
                        CameraControl.Instance.OnImageChdaged(BufferToHImage(), IsHorizonMirror, IsVerticalMirror, IsRotate);
                    }
                    //叠加十字线、自动曝光窗口、白平衡窗口信息(仅叠加设置为可见状态的)。    
                    MvApi.CameraImageOverlay(m_nCameraHandle, m_imageBuffer, ref m_frameHead);
                    //调用SDK封装好的接口，显示预览图像
                    MvApi.CameraDisplayRGB24(m_nCameraHandle, m_imageBuffer, ref m_frameHead);
                    //成功调用CameraGetImageBuffer后必须释放，下次才能继续调用CameraGetImageBuffer捕获图像。
                    MvApi.CameraReleaseImageBuffer(m_nCameraHandle, m_uRawBuffer);
                }
            }
        }

        public override System.Drawing.Image BufferToImage()
        {
            MvApi.CameraSnapToBuffer(m_nCameraHandle, out m_frameHead, out m_uRawBuffer, 2000);
            //将相机输出的原始数据转换为RGB格式到内存m_ImageBufferSnapshot中
            MvApi.CameraImageProcess(m_nCameraHandle, m_uRawBuffer, m_imageBufferSnapshot, ref m_frameHead);
            //CameraSnapToBuffer成功调用后必须用CameraReleaseImageBuffer释放SDK中分配的RAW数据缓冲区
            //否则，将造成死锁现象，预览通道和抓拍通道会被一直阻塞，直到调用CameraReleaseImageBuffer释放后解锁。
            MvApi.CameraReleaseImageBuffer(m_nCameraHandle, m_uRawBuffer);
            return MvApi.CSharpImageFromFrame(m_imageBufferSnapshot, ref m_frameHead);
        }

        public override void SetImageRotate(int nRot)
        {
            if (m_nCameraHandle > 0)
            {
                MvApi.CameraSetRotate(m_nCameraHandle, nRot);
            }
        }

        public override HImage BufferToHImage()
        {
            // 数据处理回调

            // 由于黑白相机在相机打开后设置了ISP输出灰度图像
            // 因此此处pFrameBuffer=8位灰度数据
            // 否则会和彩色相机一样输出BGR24数据

            // 彩色相机ISP默认会输出BGR24图像
            // pFrameBuffer=BGR24数据
            int w = m_frameHead.iWidth;
            int h = m_frameHead.iHeight;

            HImage Image = null;
            try
            {
                if (m_frameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8)
                {
                    //HOperatorSet.GenImage1(out Image, "byte", w, h, m_ImageBuffer);
                    Image.GenImage1("byte", w, h, m_imageBuffer);
                }
                else if (m_frameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_BGR8)
                {
                    //HOperatorSet.GenImageInterleaved(out Image,
                    //    m_ImageBuffer,
                    //    "bgr",
                    //    w, h,
                    //    -1, "byte",
                    //    w, h,
                    //    0, 0, -1, 0);
                    Image.GenImageInterleaved(
                                m_imageBuffer,
                        "bgr",
                        w, h,
                        -1, "byte",
                        w, h,
                        0, 0, -1, 0);
                }
                else
                {
                    throw new HalconException("Image format is not supported!!");
                }
            }
            catch (HalconException Exc)
            {
            }
            return Image;
        }
    }
}
