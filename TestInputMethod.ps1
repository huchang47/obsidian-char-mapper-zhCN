Write-Host "测试铁拳输入法功能..." -ForegroundColor Green
Write-Host "=======================" -ForegroundColor Green

# 检查项目结构
Write-Host "1. 检查项目结构..." -ForegroundColor Cyan
get-childitem -Path "TekkenInputMethod" -Recurse -File | Select-Object FullName

# 检查核心文件
Write-Host "2. 检查核心文件..." -ForegroundColor Cyan

$coreFiles = @(
    "TekkenInputMethod\TekkenInputMethod.Core\InputMapper.cs",
    "TekkenInputMethod\TekkenInputMethod.Core\KeyboardHook.cs",
    "TekkenInputMethod\TekkenInputMethod.Core\ConfigManager.cs",
    "TekkenInputMethod\TekkenInputMethod.Core\SystemManager.cs",
    "TekkenInputMethod\TekkenInputMethod.App\MainForm.cs",
    "TekkenInputMethod\TekkenInputMethod.App\Program.cs",
    "TekkenInputMethod\TekkenInputMethod.UI\ConfigForm.cs",
    "TekkenInputMethod\TekkenInputMethod.UI\AboutForm.cs"
)

foreach ($file in $coreFiles) {
    if (Test-Path $file) {
        Write-Host "✅ $file 存在" -ForegroundColor Green
    } else {
        Write-Host "❌ $file 缺失" -ForegroundColor Red
    }
}

# 检查配置文件路径
Write-Host "3. 检查配置文件路径..." -ForegroundColor Cyan
$appDataDir = "$env:APPDATA\TekkenInputMethod"
Write-Host "配置文件路径: $appDataDir"

if (Test-Path $appDataDir) {
    Write-Host "✅ 配置目录存在" -ForegroundColor Green
} else {
    Write-Host "⚠️  配置目录不存在，首次运行时会自动创建" -ForegroundColor Yellow
}

# 检查开机自启设置
Write-Host "4. 检查开机自启设置..." -ForegroundColor Cyan
try {
    $runKey = Get-ItemProperty "HKCU:\Software\Microsoft\Windows\CurrentVersion\Run" -Name "TekkenInputMethod" -ErrorAction Stop
    Write-Host "✅ 开机自启已设置" -ForegroundColor Green
    Write-Host "值: $($runKey.TekkenInputMethod)" -ForegroundColor Gray
} catch {
    Write-Host "⚠️  开机自启未设置" -ForegroundColor Yellow
}

# 检查模型文件
Write-Host "5. 检查模型文件..." -ForegroundColor Cyan

$modelFiles = @(
    "TekkenInputMethod\TekkenInputMethod.Core\Models\MappingConfig.cs",
    "TekkenInputMethod\TekkenInputMethod.Core\Models\PresetMappings.cs"
)

foreach ($file in $modelFiles) {
    if (Test-Path $file) {
        Write-Host "✅ $file 存在" -ForegroundColor Green
    } else {
        Write-Host "❌ $file 缺失" -ForegroundColor Red
    }
}

Write-Host "6. 测试完成！" -ForegroundColor Green
Write-Host "=======================" -ForegroundColor Green
Write-Host "请手动运行输入法应用程序进行功能测试：" -ForegroundColor Yellow
Write-Host "1. 点击\"激活输入法\"按钮"
Write-Host "2. 尝试输入 WASD 键，应该会输出方向键符号"
Write-Host "3. 打开配置界面，修改按键映射"
Write-Host "4. 测试预设模式切换"
Write-Host "5. 检查系统托盘功能"

Write-Host ""
Write-Host "按任意键退出..." -ForegroundColor Gray
$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") | Out-Null
$host.UI.RawUI.FlushInputBuffer()
