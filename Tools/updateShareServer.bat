@echo off
setlocal

REM 设置源文件夹和目标文件夹
set "sourceFolder=%~dp0..\Server\ShareBase\bin\Release\netstandard2.1"
set "targetFolder=%~dp0..\Assets\Plugins"

REM 创建目标文件夹（如果不存在）
if not exist "%targetFolder%" (
    mkdir "%targetFolder%"
)

REM 复制 DLL 文件到目标文件夹
xcopy "%sourceFolder%\ShareBase.dll" "%targetFolder%\" /Y

echo DLL 文件已成功复制到 %targetFolder%
pause

endlocal