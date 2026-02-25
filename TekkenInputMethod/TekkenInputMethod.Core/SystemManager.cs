using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace TekkenInputMethod.Core
{
    public class SystemManager
    {
        private const string StartupKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "TekkenInputMethod";
        
        public bool IsElevated
        {
            get
            {
                using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
                {
                    var principal = new System.Security.Principal.WindowsPrincipal(identity);
                    return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
                }
            }
        }
        
        public bool IsRunOnStartup
        {
            get
            {
                using (var key = Registry.CurrentUser.OpenSubKey(StartupKeyPath))
                {
                    return key?.GetValue(AppName) != null;
                }
            }
            set
            {
                using (var key = Registry.CurrentUser.OpenSubKey(StartupKeyPath, true))
                {
                    if (value)
                    {
                        string exePath = Process.GetCurrentProcess().MainModule.FileName;
                        key?.SetValue(AppName, exePath);
                    }
                    else
                    {
                        key?.DeleteValue(AppName, false);
                    }
                }
            }
        }
        
        public void RestartAsAdmin()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                Verb = "runas",
                UseShellExecute = true
            };
            
            try
            {
                Process.Start(startInfo);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restarting as admin: {ex.Message}");
            }
        }
        
        public string GetAppDataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
        }
        
        public string GetExePath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }
        
        public void CreateShortcut(string shortcutPath)
        {
            try
            {
                string exePath = GetExePath();
                string shortcutDir = Path.GetDirectoryName(shortcutPath);
                
                if (!Directory.Exists(shortcutDir))
                {
                    Directory.CreateDirectory(shortcutDir);
                }
                
                // 使用Windows Script Host创建快捷方式
                Type shellType = Type.GetTypeFromProgID("WScript.Shell");
                dynamic shell = Activator.CreateInstance(shellType);
                dynamic shortcut = shell.CreateShortcut(shortcutPath);
                
                shortcut.TargetPath = exePath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(exePath);
                shortcut.Description = "铁拳输入法";
                shortcut.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating shortcut: {ex.Message}");
            }
        }
        
        public void AddToStartup()
        {
            IsRunOnStartup = true;
        }
        
        public void RemoveFromStartup()
        {
            IsRunOnStartup = false;
        }
    }
}
