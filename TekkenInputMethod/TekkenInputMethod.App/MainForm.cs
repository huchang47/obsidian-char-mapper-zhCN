using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TekkenInputMethod.Core;
using TekkenInputMethod.UI;

namespace TekkenInputMethod.App
{
    public partial class MainForm : Form
    {
        private KeyboardHook keyboardHook;
        private InputMapper inputMapper;
        private ProfileManager profileManager;
        private NotifyIcon trayIcon;
        private Icon appIcon;
        private HotkeyManager hotkeyManager;
        private const int HOTKEY_ACTIVATE = 1;
        
        public MainForm()
        {
            Logger.Info("应用程序启动");
            LoadApplicationIcon();
            InitializeComponents();
            InitializeKeyboardHook();
            InitializeProfileManager();
            InitializeHotkeys();
            InitializeTrayIcon();
            Logger.Info("应用程序初始化完成");
        }
        
        protected override void WndProc(ref Message m)
        {
            hotkeyManager?.ProcessHotkeyMessage(m);
            base.WndProc(ref m);
        }
        
        private void LoadApplicationIcon()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "TekkenInputMethod.TekkenInputMethod.App.tekken.png";
                
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (var bitmap = new Bitmap(stream))
                        {
                            // 创建窗体图标 (32x32)
                            using (var iconBitmap = new Bitmap(bitmap, new System.Drawing.Size(32, 32)))
                            {
                                appIcon = Icon.FromHandle(iconBitmap.GetHicon());
                                this.Icon = appIcon;
                            }
                        }
                        Logger.Info("应用程序图标加载成功");
                    }
                    else
                    {
                        Logger.Warning($"找不到嵌入资源: {resourceName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("加载应用程序图标失败", ex);
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = "铁拳输入法";
            this.Size = new System.Drawing.Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 创建控件
            var statusLabel = new Label
            {
                Name = "statusLabel",
                Text = "状态: 未激活",
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            
            var profileLabel = new Label
            {
                Name = "profileLabel",
                Text = "当前配置: 铁拳配置",
                Location = new System.Drawing.Point(20, 45),
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft YaHei", 8, System.Drawing.FontStyle.Italic),
                ForeColor = Color.DarkBlue
            };
            
            var activateButton = new Button
            {
                Text = "激活输入法",
                Location = new System.Drawing.Point(20, 70),
                Size = new System.Drawing.Size(120, 30)
            };
            activateButton.Click += ActivateButton_Click;
            
            var configButton = new Button
            {
                Text = "配置",
                Location = new System.Drawing.Point(150, 70),
                Size = new System.Drawing.Size(100, 30)
            };
            configButton.Click += ConfigButton_Click;
            
            var aboutButton = new Button
            {
                Text = "关于",
                Location = new System.Drawing.Point(20, 110),
                Size = new System.Drawing.Size(100, 30)
            };
            aboutButton.Click += AboutButton_Click;
            
            var exitButton = new Button
            {
                Text = "退出",
                Location = new System.Drawing.Point(150, 110),
                Size = new System.Drawing.Size(100, 30)
            };
            exitButton.Click += ExitButton_Click;
            
            this.Controls.Add(statusLabel);
            this.Controls.Add(profileLabel);
            this.Controls.Add(activateButton);
            this.Controls.Add(configButton);
            this.Controls.Add(aboutButton);
            this.Controls.Add(exitButton);
        }
        
        private void InitializeKeyboardHook()
        {
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyPressed += KeyboardHook_KeyPressed;
            keyboardHook.HookKeyboard();
        }
        
        private void InitializeProfileManager()
        {
            profileManager = new ProfileManager();
            inputMapper = new InputMapper();
            
            var activeProfile = profileManager.GetActiveProfile();
            inputMapper.LoadMappings(activeProfile.Mappings);
            
            UpdateProfileLabel();
        }
        
        private void UpdateProfileLabel()
        {
            var profileLabel = this.Controls.Find("profileLabel", true).FirstOrDefault() as Label;
            if (profileLabel != null)
            {
                var activeProfile = profileManager.GetActiveProfile();
                profileLabel.Text = $"当前配置: {activeProfile.Name}";
            }
        }
        
        private void InitializeHotkeys()
        {
            try
            {
                hotkeyManager = new HotkeyManager(this.Handle);
                hotkeyManager.HotkeyPressed += HotkeyManager_HotkeyPressed;
                
                var activeProfile = profileManager.GetActiveProfile();
                
                if (!string.IsNullOrEmpty(activeProfile.Hotkeys.Activate))
                {
                    if (hotkeyManager.RegisterHotkey(activeProfile.Hotkeys.Activate, HOTKEY_ACTIVATE))
                    {
                        Logger.Info($"热键注册成功: {activeProfile.Hotkeys.Activate}");
                    }
                    else
                    {
                        Logger.Warning($"热键注册失败: {activeProfile.Hotkeys.Activate}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("初始化热键失败", ex);
            }
        }
        
        private void HotkeyManager_HotkeyPressed(object sender, HotkeyEventArgs e)
        {
            if (e.Id == HOTKEY_ACTIVATE)
            {
                inputMapper.IsActive = !inputMapper.IsActive;
                UpdateStatus();
                Logger.Info($"热键切换输入法状态: {(inputMapper.IsActive ? "激活" : "停用")}");
            }
        }
        
        private void InitializeTrayIcon()
        {
            Icon trayIconImage = appIcon ?? System.Drawing.SystemIcons.Application;
            
            // 尝试加载并创建16x16的托盘图标
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "TekkenInputMethod.TekkenInputMethod.App.tekken.png";
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (var bitmap = new Bitmap(stream))
                        {
                            // 创建16x16的托盘图标
                            using (var trayBitmap = new Bitmap(16, 16))
                            {
                                using (var g = Graphics.FromImage(trayBitmap))
                                {
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    g.DrawImage(bitmap, 0, 0, 16, 16);
                                }
                                trayIconImage = Icon.FromHandle(trayBitmap.GetHicon());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("加载托盘图标失败", ex);
            }
            
            trayIcon = new NotifyIcon
            {
                Icon = trayIconImage,
                Text = "铁拳输入法",
                Visible = true
            };
            
            var contextMenu = new ContextMenuStrip();
            
            var activateMenuItem = new ToolStripMenuItem("激活");
            activateMenuItem.Click += (sender, e) => { inputMapper.IsActive = true; UpdateStatus(); };
            
            var deactivateMenuItem = new ToolStripMenuItem("停用");
            deactivateMenuItem.Click += (sender, e) => { inputMapper.IsActive = false; UpdateStatus(); };
            
            var configMenuItem = new ToolStripMenuItem("配置");
            configMenuItem.Click += (sender, e) => { ShowConfigForm(); };
            
            var exitMenuItem = new ToolStripMenuItem("退出");
            exitMenuItem.Click += (sender, e) => { Application.Exit(); };
            
            contextMenu.Items.AddRange(new ToolStripItem[] 
            {
                activateMenuItem,
                deactivateMenuItem,
                configMenuItem,
                exitMenuItem
            });
            
            trayIcon.ContextMenuStrip = contextMenu;
        }
        
        private void UpdateStatus()
        {
            string status = inputMapper.IsActive ? "已激活" : "未激活";
            foreach (Control control in this.Controls)
            {
                if (control is Label label && label.Name == "statusLabel")
                {
                    label.Text = $"状态: {status}";
                    break;
                }
            }
            
            trayIcon.Text = $"铁拳输入法 - {status}";
        }
        
        private void KeyboardHook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            string keyString = e.Key.ToString();
            
            // 处理数字键：D0-D9 转换为 0-9
            if (keyString.StartsWith("D") && keyString.Length == 2 && char.IsDigit(keyString[1]))
            {
                keyString = keyString.Substring(1);
            }
            
            if (keyString == "Space")
                keyString = " ";
            
            // 传递 Shift 键状态以支持大小写敏感
            string mappedValue = inputMapper.MapKey(keyString, e.IsShiftPressed);
            if (mappedValue != null)
            {
                // 标记为已处理，阻止原始按键传递
                e.Handled = true;
                // SendKeys 特殊字符需要转义：+ ^ % ~ ( ) { } [ ]
                string sendValue = mappedValue
                    .Replace("+", "{+}")
                    .Replace("^", "{^}")
                    .Replace("%", "{%}")
                    .Replace("~", "{~}")
                    .Replace("(", "{(}")
                    .Replace(")", "{)}")
                    .Replace("{", "{{}")
                    .Replace("}", "{}}")
                    .Replace("[", "{[}")
                    .Replace("]", "{]}");
                SendKeys.Send(sendValue);
            }
        }
        
        private void ActivateButton_Click(object sender, EventArgs e)
        {
            inputMapper.IsActive = !inputMapper.IsActive;
            UpdateStatus();
            Logger.Info($"输入法状态变更为: {(inputMapper.IsActive ? "已激活" : "未激活")}");
        }
        
        private void ConfigButton_Click(object sender, EventArgs e)
        {
            Logger.Info("打开配置界面");
            ShowConfigForm();
        }
        
        private void AboutButton_Click(object sender, EventArgs e)
        {
            Logger.Info("打开关于界面");
            ShowAboutForm();
        }
        
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Logger.Info("用户点击退出按钮");
            Application.Exit();
        }
        
        private void ShowConfigForm()
        {
            try
            {
                // 将 MainForm 的 ProfileManager 传递给 ConfigForm
                var configForm = new TekkenInputMethod.UI.ConfigForm(profileManager);
                configForm.ShowDialog();
                
                // 无论用户如何关闭配置窗口，都重新加载当前配置
                var activeProfile = profileManager.GetActiveProfile();
                inputMapper.LoadMappings(activeProfile.Mappings);
                UpdateProfileLabel();
                
                // 重新注册热键
                hotkeyManager.UnregisterHotkey(HOTKEY_ACTIVATE);
                if (!string.IsNullOrEmpty(activeProfile.Hotkeys.Activate))
                {
                    if (hotkeyManager.RegisterHotkey(activeProfile.Hotkeys.Activate, HOTKEY_ACTIVATE))
                    {
                        Logger.Info($"热键重新注册成功: {activeProfile.Hotkeys.Activate}");
                    }
                }
                
                Logger.Info("配置界面关闭");
            }
            catch (Exception ex)
            {
                Logger.Error("打开配置界面失败", ex);
                MessageBox.Show("打开配置界面失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ShowAboutForm()
        {
            try
            {
                var aboutForm = new TekkenInputMethod.UI.AboutForm();
                aboutForm.ShowDialog();
                Logger.Info("关于界面关闭");
            }
            catch (Exception ex)
            {
                Logger.Error("打开关于界面失败", ex);
                MessageBox.Show("打开关于界面失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            base.OnFormClosing(e);
        }
        
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            keyboardHook?.UnhookKeyboard();
            hotkeyManager?.UnregisterHotkey(HOTKEY_ACTIVATE);
            trayIcon?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
