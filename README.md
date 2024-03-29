# IntegratedSoftware
视觉软件，2020年05月修整期间编写的工业集成软件框架，目前不从事该行业，因此开放出来交流学习。软件已作删减，仅保留视觉部分，需自行添加Halcon的DLL方可正常运行软件。希望对大家有帮助。

*想了解相机模块的朋友比较多，但因个人资源有限，相机代码并没有作过多编写和测试优化，害怕误导大家的学习方向。现软件上传了相机部分的代码，仅供大家参考，望见谅。

Halcon Vision software. The software has been abridged to keep only the vision part, you need to add Halcon's DLL by yourself to run the software. I hope to be helpful to everyone.

*About the camera module, because of limited personal resources, the camera code is not too much writing and testing optimization. Now the software uploads the code routines of the camera part for your reference only.

# 开发环境/Development Environment
● Visual Studio 2019

● .NET Framework 4.7.2

● Halcon 12

# 解决方案介绍/Solution Introduction

● AutomationSystem

软件运行主窗体项目。

The software runs the main form project.

● BuildDLL

所有生成的引用DLL集中放置在此处，可通过BuildDLL.sln统一管理需要编译生成的项目。

All generated reference DLLs are centrally placed here, and you can use BuildDLL.sln to unify the management of projects that need to be compiled and generated.

● Calibration

编译生成的标定DLL放置位置。

The location of the calibrated DLL placement generated by the compilation.

● CameraDLL

编译生成的相机DLL放置位置。

Placement of the camera DLL generated by the compiler.

● CommonLibrary

个人常用的工具类项目，含文件读取写入、常用变量类型扩展、控件操作等功能。

Personal tools commonly used projects , including file reading and writing , common variable type expansion , control operations and other functions .

● ExternTool

编译生成的图像处理工具DLL放置位置。

Placement of the image processing tool DLL generated by the compiler.

● Halcon

含Halcon操作窗体、Halcon图像处理工具项目。

Contains Halcon operation form, Halcon image processing tools project.

● ThirdPartyDLL

集中放置需要引用的第三方DLL。

Centralized placement of third-party DLLs that need to be referenced.

● UIControl

窗体自定义控件项目。

Forms custom control project.

# 编译生成流程/Compile Generation Process

注意：该项目不提供Halcon相关的DLL，需要自行解决，可使用Halcon 12外的其他版本。

Note: The project does not provide Halcon related DLLs, you need to solve it by yourself, you can use other versions other than Halcon 12.

① 放置Halcon软件的DLL：halcondotnet.dll、halcon.dll至ThirdPartyDLL。

  Place the Halcon software DLLs: halcondotnet.dll, halcon.dll to ThirdPartyDLL.

② 打开BuildDLL.sln，选择重新生成解决方案，选择AutomationSystem为启动项目，即可运行软件。

  Open BuildDLL.sln, select Rebuild Solution and select AutomationSystem as the startup project to run the software.

③ 如果使用的是Halcon 64位DLL，编译时请取消勾选首选32位。

  If you are using Halcon 64-bit DLL, uncheck Preferred 32-bit when compiling.
  ![image](https://user-images.githubusercontent.com/17681289/167654789-a3ce95ad-0a62-483a-a634-7fd3c6d5dae7.png)

注意：项目编译完成后，运行软件无法显示工具箱，有可能是路径原因导致以下代码无法复制粘贴相关文件到输出目录，请手动复制粘贴文件到输出目录下。

Note: After the project is compiled, the toolbox cannot be displayed when running the software, it may be due to the path that causes the following code to fail to copy and paste the relevant files to the output directory, please manually copy and paste the files to the output directory.

![image](https://github.com/jiaowomomo/IntegratedSoftware/assets/17681289/0c20534c-23fc-4466-be63-6b6e85edacdd)
