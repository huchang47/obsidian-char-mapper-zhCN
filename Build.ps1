# 铁拳输入法编译脚本
Write-Host "开始编译铁拳输入法..." -ForegroundColor Green

# 检查 .NET SDK 是否安装
try {
    $dotnetVersion = dotnet --version
    Write-Host ".NET SDK 版本: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "未检测到 .NET SDK，请先安装 .NET 6.0 SDK" -ForegroundColor Red
    Write-Host "下载地址: https://dotnet.microsoft.com/download/dotnet/6.0" -ForegroundColor Yellow
    exit 1
}

# 进入项目目录
Set-Location -Path "TekkenInputMethod"

# 清理之前的编译结果
Write-Host "清理之前的编译结果..." -ForegroundColor Cyan
if (Test-Path "bin") {
    Remove-Item -Path "bin" -Recurse -Force
}
if (Test-Path "obj") {
    Remove-Item -Path "obj" -Recurse -Force
}

# 还原 NuGet 包
Write-Host "还原 NuGet 包..." -ForegroundColor Cyan
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "NuGet 包还原失败" -ForegroundColor Red
    exit 1
}

# 编译项目
Write-Host "编译项目..." -ForegroundColor Cyan
dotnet build --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "编译失败" -ForegroundColor Red
    exit 1
}

Write-Host "编译成功！" -ForegroundColor Green

# 发布单文件版本
Write-Host "发布单文件版本..." -ForegroundColor Cyan
dotnet publish --configuration Release --output "..\Publish" --self-contained true --runtime win-x64
if ($LASTEXITCODE -ne 0) {
    Write-Host "发布失败" -ForegroundColor Red
    exit 1
}

Write-Host "发布成功！" -ForegroundColor Green

# 返回上级目录
Set-Location -Path ".."

# 检查生成的文件
$exePath = "Publish\TekkenInputMethod.exe"
if (Test-Path $exePath) {
    Write-Host "EXE 文件已生成: $exePath" -ForegroundColor Green
    $fileInfo = Get-Item $exePath
    Write-Host "文件大小: $($fileInfo.Length / 1MB) MB" -ForegroundColor Gray
} else {
    Write-Host "未找到生成的 EXE 文件" -ForegroundColor Red
    exit 1
}

Write-Host "" -ForegroundColor Green
Write-Host "编译完成！" -ForegroundColor Green
Write-Host "EXE 文件位置: $((Resolve-Path $exePath).Path)" -ForegroundColor Yellow
Write-Host "" -ForegroundColor Green
Write-Host "使用方法:" -ForegroundColor Cyan
Write-Host "1. 直接运行 Publish\TekkenInputMethod.exe" -ForegroundColor White
Write-Host "2. 或者将 Publish 文件夹复制到任意位置运行" -ForegroundColor White

Write-Host "" -ForegroundColor Green
Write-Host "按任意键退出..." -ForegroundColor Gray
$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") | Out-Null
