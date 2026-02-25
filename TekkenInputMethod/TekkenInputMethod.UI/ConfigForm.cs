using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TekkenInputMethod.Core;
using TekkenInputMethod.Core.Models;

namespace TekkenInputMethod.UI
{
    public partial class ConfigForm : Form
    {
        private ConfigManager configManager;
        private MappingConfig currentConfig;
        
        public ConfigForm()
        {
            InitializeComponents();
            configManager = new ConfigManager();
            LoadConfig();
        }
        
        private void InitializeComponents()
        {
            this.Text = "铁拳输入法配置";
            this.Size = new System.Drawing.Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 创建标签页控件
            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };
            
            // 映射配置标签页
            var mappingsTab = new TabPage("按键映射");
            mappingsTab.Controls.Add(CreateMappingsPanel());
            
            // 预设模式标签页
            var presetsTab = new TabPage("预设模式");
            presetsTab.Controls.Add(CreatePresetsPanel());
            
            // 系统设置标签页
            var settingsTab = new TabPage("系统设置");
            settingsTab.Controls.Add(CreateSettingsPanel());
            
            tabControl.TabPages.AddRange(new TabPage[] { mappingsTab, presetsTab, settingsTab });
            
            // 底部按钮
            var saveButton = new Button
            {
                Text = "保存",
                Location = new System.Drawing.Point(400, 420),
                Size = new System.Drawing.Size(80, 30)
            };
            saveButton.Click += SaveButton_Click;
            
            var cancelButton = new Button
            {
                Text = "取消",
                Location = new System.Drawing.Point(490, 420),
                Size = new System.Drawing.Size(80, 30)
            };
            cancelButton.Click += CancelButton_Click;
            
            this.Controls.Add(tabControl);
            this.Controls.Add(saveButton);
            this.Controls.Add(cancelButton);
        }
        
        private Panel CreateMappingsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            
            var label = new Label
            {
                Text = "自定义按键映射",
                Location = new System.Drawing.Point(20, 10),
                AutoSize = true
            };
            
            var dataGridView = new DataGridView
            {
                Name = "mappingsGrid",
                Location = new System.Drawing.Point(20, 40),
                Size = new System.Drawing.Size(540, 280),
                AutoGenerateColumns = false
            };
            
            dataGridView.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "Key", HeaderText = "按键", Width = 80 },
                new DataGridViewTextBoxColumn { Name = "Value", HeaderText = "映射值", Width = 400 }
            );
            
            var addButton = new Button
            {
                Text = "添加",
                Location = new System.Drawing.Point(20, 350),
                Size = new System.Drawing.Size(80, 30)
            };
            addButton.Click += AddMapping_Click;
            
            var removeButton = new Button
            {
                Text = "删除",
                Location = new System.Drawing.Point(110, 350),
                Size = new System.Drawing.Size(80, 30)
            };
            removeButton.Click += RemoveMapping_Click;
            
            panel.Controls.Add(label);
            panel.Controls.Add(dataGridView);
            panel.Controls.Add(addButton);
            panel.Controls.Add(removeButton);
            
            return panel;
        }
        
        private Panel CreatePresetsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill
            };
            
            var label = new Label
            {
                Text = "选择预设模式",
                Location = new System.Drawing.Point(20, 10),
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft YaHei", 10, System.Drawing.FontStyle.Bold)
            };
            
            // 铁拳模式按钮
            var gamepadButton = new Button
            {
                Text = "铁拳模式",
                Location = new System.Drawing.Point(20, 40),
                Size = new System.Drawing.Size(150, 35)
            };
            gamepadButton.Click += (sender, e) => ApplyPreset(0);
            
            // 铁拳模式说明
            var gamepadDesc = new Label
            {
                Text = "专为铁拳游戏设计，包含完整的方向键和按钮映射\n" +
                       "• WASD: 方向键 (⭡⭠⭣⭢)\n" +
                       "• QEZC: 斜方向 (↖↗↙↘)\n" +
                       "• JIKL: 按钮 (❶❷❸❹)\n" +
                       "• UO: 组合按钮 (❶✚❷ ❸✚❹)\n" +
                       "• 其他: 长蹲⇩、回中✩、各种特殊状态等",
                Location = new System.Drawing.Point(20, 80),
                Size = new System.Drawing.Size(600, 100),
                Font = new System.Drawing.Font("Microsoft YaHei", 9),
                ForeColor = System.Drawing.Color.DarkBlue
            };
            
            // 方向键模式按钮
            var arrowKeysButton = new Button
            {
                Text = "方向键模式",
                Location = new System.Drawing.Point(20, 190),
                Size = new System.Drawing.Size(150, 35)
            };
            arrowKeysButton.Click += (sender, e) => ApplyPreset(1);
            
            // 方向键模式说明
            var arrowDesc = new Label
            {
                Text = "简洁的方向键映射，适合一般用途\n" +
                       "• WASD: 方向键 (↑←↓→)\n" +
                       "• QEZC: 斜方向 (↖↗↙↘)",
                Location = new System.Drawing.Point(20, 230),
                Size = new System.Drawing.Size(540, 50),
                Font = new System.Drawing.Font("Microsoft YaHei", 9),
                ForeColor = System.Drawing.Color.DarkGreen
            };
            
            // 恢复默认按钮
            var defaultButton = new Button
            {
                Text = "恢复默认",
                Location = new System.Drawing.Point(20, 290),
                Size = new System.Drawing.Size(150, 35)
            };
            defaultButton.Click += DefaultButton_Click;
            
            panel.Controls.Add(label);
            panel.Controls.Add(gamepadButton);
            panel.Controls.Add(gamepadDesc);
            panel.Controls.Add(arrowKeysButton);
            panel.Controls.Add(arrowDesc);
            panel.Controls.Add(defaultButton);
            
            return panel;
        }
        
        private Panel CreateSettingsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill
            };
            
            var label = new Label
            {
                Text = "系统设置",
                Location = new System.Drawing.Point(20, 10),
                AutoSize = true
            };
            
            var runOnStartupCheckbox = new CheckBox
            {
                Text = "开机自启",
                Location = new System.Drawing.Point(20, 40),
                AutoSize = true,
                Name = "runOnStartupCheckbox"
            };
            
            var startMinimizedCheckbox = new CheckBox
            {
                Text = "最小化启动",
                Location = new System.Drawing.Point(20, 70),
                AutoSize = true,
                Name = "startMinimizedCheckbox"
            };
            
            // 热键设置
            var hotkeyLabel = new Label
            {
                Text = "热键设置",
                Location = new System.Drawing.Point(20, 110),
                AutoSize = true,
                Font = new System.Drawing.Font(this.Font, System.Drawing.FontStyle.Bold)
            };
            
            var activateHotkeyLabel = new Label
            {
                Text = "激活热键:",
                Location = new System.Drawing.Point(20, 140),
                AutoSize = true
            };
            
            var activateHotkeyTextBox = new TextBox
            {
                Location = new System.Drawing.Point(100, 137),
                Size = new System.Drawing.Size(150, 23),
                Name = "activateHotkeyTextBox",
                ReadOnly = true
            };
            activateHotkeyTextBox.KeyDown += HotkeyTextBox_KeyDown;
            
            var hotkeyHintLabel = new Label
            {
                Text = "提示: 点击输入框后按下热键组合，用于激活铁拳输入状态",
                Location = new System.Drawing.Point(20, 170),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Gray,
                Font = new System.Drawing.Font(this.Font.FontFamily, 9)
            };
            
            panel.Controls.Add(label);
            panel.Controls.Add(runOnStartupCheckbox);
            panel.Controls.Add(startMinimizedCheckbox);
            panel.Controls.Add(hotkeyLabel);
            panel.Controls.Add(activateHotkeyLabel);
            panel.Controls.Add(activateHotkeyTextBox);
            panel.Controls.Add(hotkeyHintLabel);
            
            return panel;
        }
        
        private void HotkeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            
            if (sender is TextBox textBox)
            {
                var modifiers = new List<string>();
                if (e.Control) modifiers.Add("Ctrl");
                if (e.Alt) modifiers.Add("Alt");
                if (e.Shift) modifiers.Add("Shift");
                
                var key = e.KeyCode.ToString();
                if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Menu)
                {
                    textBox.Text = string.Join("+", modifiers);
                }
                else
                {
                    modifiers.Add(key);
                    textBox.Text = string.Join("+", modifiers);
                }
            }
        }
        
        private void LoadConfig()
        {
            currentConfig = configManager.LoadConfig();
            LoadMappingsToGrid();
            LoadSettings();
        }
        
        private void LoadMappingsToGrid()
        {
            var dataGridView = this.Controls.Find("mappingsGrid", true)[0] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
                foreach (var mapping in currentConfig.Mappings)
                {
                    dataGridView.Rows.Add(mapping.Key, mapping.Value);
                }
            }
        }
        
        private void LoadSettings()
        {
            var runOnStartupCheckbox = this.Controls.Find("runOnStartupCheckbox", true).FirstOrDefault() as CheckBox;
            var startMinimizedCheckbox = this.Controls.Find("startMinimizedCheckbox", true).FirstOrDefault() as CheckBox;
            var activateHotkeyTextBox = this.Controls.Find("activateHotkeyTextBox", true).FirstOrDefault() as TextBox;
            
            if (runOnStartupCheckbox != null)
                runOnStartupCheckbox.Checked = currentConfig.Settings.RunOnStartup;
            
            if (startMinimizedCheckbox != null)
                startMinimizedCheckbox.Checked = currentConfig.Settings.StartMinimized;
            
            if (activateHotkeyTextBox != null)
                activateHotkeyTextBox.Text = currentConfig.Hotkeys.Activate ?? "F8";
        }
        
        private void SaveConfig()
        {
            SaveMappingsFromGrid();
            SaveSettings();
            configManager.SaveConfig(currentConfig);
            MessageBox.Show("配置已保存", "成功");
        }
        
        private void SaveMappingsFromGrid()
        {
            var dataGridView = this.Controls.Find("mappingsGrid", true)[0] as DataGridView;
            if (dataGridView != null)
            {
                currentConfig.Mappings.Clear();
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {
                        string key = row.Cells[0].Value.ToString().Trim();
                        string value = row.Cells[1].Value.ToString().Trim();
                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                        {
                            currentConfig.Mappings[key] = value;
                        }
                    }
                }
            }
        }
        
        private void SaveSettings()
        {
            var runOnStartupCheckbox = this.Controls.Find("runOnStartupCheckbox", true).FirstOrDefault() as CheckBox;
            var startMinimizedCheckbox = this.Controls.Find("startMinimizedCheckbox", true).FirstOrDefault() as CheckBox;
            var activateHotkeyTextBox = this.Controls.Find("activateHotkeyTextBox", true).FirstOrDefault() as TextBox;
            
            if (runOnStartupCheckbox != null)
                currentConfig.Settings.RunOnStartup = runOnStartupCheckbox.Checked;
            
            if (startMinimizedCheckbox != null)
                currentConfig.Settings.StartMinimized = startMinimizedCheckbox.Checked;
            
            if (activateHotkeyTextBox != null && !string.IsNullOrEmpty(activateHotkeyTextBox.Text))
                currentConfig.Hotkeys.Activate = activateHotkeyTextBox.Text;
            
            // 应用开机自启设置
            var systemManager = new SystemManager();
            systemManager.IsRunOnStartup = currentConfig.Settings.RunOnStartup;
        }
        
        private void ApplyPreset(int presetId)
        {
            configManager.ApplyPreset(presetId);
            LoadConfig();
            MessageBox.Show($"已应用{PresetMappings.GetPresetName(presetId)}", "成功");
        }
        
        private void AddMapping_Click(object sender, EventArgs e)
        {
            var dataGridView = this.Controls.Find("mappingsGrid", true)[0] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.Rows.Add("", "");
            }
        }
        
        private void RemoveMapping_Click(object sender, EventArgs e)
        {
            var dataGridView = this.Controls.Find("mappingsGrid", true)[0] as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    dataGridView.Rows.Remove(row);
                }
            }
        }
        
        private void DefaultButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要恢复默认配置吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                configManager.ResetToDefault();
                LoadConfig();
                MessageBox.Show("已恢复默认配置", "成功");
            }
        }
        
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }
        
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
