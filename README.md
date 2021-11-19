# IntegratedSoftware
视觉软件，2020年05月修整期间编写的工业集成软件框架，目前不从事该行业，因此开放出来交流学习。软件已作删减，仅保留视觉部分，需自行添加Halcon的DLL方可正常运行软件。希望对大家有帮助。

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

注意：该项目不提供Halcon相关的DLL，需要自行解决。

① 放置Halcon软件的DLL：halcondotnet.dll至ThirdPartyDLL。

② 打开BuildDLL.sln，选择重新生成解决方案。

③ 打开AutomationSystem.sln，选择重新生成解决方案。

④ 将Calibration、ExternTool文件夹以及Halcon软件的DLL：halcon.dll，复制粘贴到AutomationSystem的生成输出路径，即可运行软件。
