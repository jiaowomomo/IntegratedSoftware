# IntegratedSoftware
视觉软件，2020年05月修整期间编写的工业集成软件框架，目前不从事该行业，因此开放出来交流学习。软件已作删减，仅保留视觉部分，需自行添加Halcon的DLL方可正常运行软件。希望对大家有帮助。

*想了解相机模块的朋友比较多，但因个人资源有限，相机代码并没有作过多编写和测试优化，害怕误导大家的学习方向。现软件上传了相机部分的代码，仅供大家参考，望见谅。

# 开发环境
● Visual Studio 2019

● .NET Framework 4.7.2

● Halcon 12

# 解决方案介绍

● AutomationSystem

软件运行主窗体项目。

● BuildDLL

所有生成的引用DLL集中放置在此处，可通过BuildDLL.sln统一管理需要编译生成的项目。

● Calibration

编译生成的标定DLL放置位置。

● CameraDLL

编译生成的相机DLL放置位置。

● CommonLibrary

个人常用的工具类项目，含文件读取写入、常用变量类型扩展、控件操作等功能。

● ExternTool

编译生成的图像处理工具DLL放置位置。

● Halcon

含Halcon操作窗体、Halcon图像处理工具项目。

● ThirdPartyDLL

集中放置需要引用的第三方DLL。

● UIControl

窗体自定义控件项目。

# 编译生成流程

注意：该项目不提供Halcon相关的DLL，需要自行解决，可使用Halcon 12外的其他版本。

① 放置Halcon软件的DLL：halcondotnet.dll、halcon.dll至ThirdPartyDLL。

② 打开BuildDLL.sln，选择重新生成解决方案，选择AutomationSystem为启动项目，即可运行软件。

③ 如果使用的是Halcon 64位DLL，编译时请取消勾选首选32位。
  ![image](https://user-images.githubusercontent.com/17681289/167654789-a3ce95ad-0a62-483a-a634-7fd3c6d5dae7.png)
