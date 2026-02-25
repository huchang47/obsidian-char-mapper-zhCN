@echo off

REM 测试铁拳输入法

setlocal

echo 测试铁拳输入法功能...
echo =======================

REM 检查项目结构
echo 1. 检查项目结构...
dir /b /s TekkenInputMethod

REM 检查核心文件
echo 2. 检查核心文件...
if exist "TekkenInputMethod\TekkenInputMethod.Core\InputMapper.cs" (
    echo ✅ InputMapper.cs 存在
) else (
    echo ❌ InputMapper.cs 缺失
)

if exist "TekkenInputMethod\TekkenInputMethod.Core\KeyboardHook.cs" (
    echo ✅ KeyboardHook.cs 存在
) else (
    echo ❌ KeyboardHook.cs 缺失
)

if exist "TekkenInputMethod\TekkenInputMethod.Core\ConfigManager.cs" (
    echo ✅ ConfigManager.cs 存在
) else (
    echo ❌ ConfigManager.cs 缺失
)

if exist "TekkenInputMethod\TekkenInputMethod.App\MainForm.cs" (
    echo ✅ MainForm.cs 存在
) else (
    echo ❌ MainForm.cs 缺失
)

if exist "TekkenInputMethod\TekkenInputMethod.UI\ConfigForm.cs" (
    echo ✅ ConfigForm.cs 存在
) else (
    echo ❌ ConfigForm.cs 缺失
)

echo 3. 检查配置文件路径...
set APPDATA_DIR=%APPDATA%\TekkenInputMethod
echo 配置文件路径: %APPDATA_DIR%

if exist "%APPDATA_DIR%" (
    echo ✅ 配置目录存在
) else (
    echo ⚠️  配置目录不存在，首次运行时会自动创建
)

echo 4. 检查开机自启设置...
reg query "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v "TekkenInputMethod" 2>nul
if %errorlevel% equ 0 (
    echo ✅ 开机自启已设置
) else (
    echo ⚠️  开机自启未设置
)

echo 5. 测试完成！
echo =======================
echo 请手动运行输入法应用程序进行功能测试：
echo 1. 点击"激活输入法"按钮
2. 尝试输入 WASD 键，应该会输出方向键符号
3. 打开配置界面，修改按键映射
4. 测试预设模式切换
5. 检查系统托盘功能

echo.
echo 按任意键退出...
pause >nul

endlocal
