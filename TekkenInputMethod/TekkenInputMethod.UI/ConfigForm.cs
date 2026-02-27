using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TekkenInputMethod.Core;
using TekkenInputMethod.Core.Models;

namespace TekkenInputMethod.UI
{
    public partial class ConfigForm : Form
    {
        private ProfileManager profileManager;
        private MappingProfile currentProfile;
        
        /// <summary>
        /// 获取当前激活的配置
        /// </summary>
        public MappingProfile ActiveProfile => currentProfile;
        
        public ConfigForm(ProfileManager? existingProfileManager = null)
        {
            InitializeComponents();
            // 如果提供了现有的 ProfileManager，使用它；否则创建新的
            profileManager = existingProfileManager ?? new ProfileManager();
            LoadProfiles();
        }
        
        private void InitializeComponents()
        {
            this.Text = "铁拳输入法配置";
            this.Size = new System.Drawing.Size(650, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 创建标签页控件
            var tabControl = new TabControl
            {
                Name = "tabControl",
                Dock = DockStyle.Fill
            };
            
            // 配置管理标签页
            var profilesTab = new TabPage("配置管理");
            profilesTab.Controls.Add(CreateProfilesPanel());
            
            // 按键映射标签页
            var mappingsTab = new TabPage("按键映射");
            mappingsTab.Controls.Add(CreateMappingsPanel());
            
            // 系统设置标签页
            var settingsTab = new TabPage("系统设置");
            settingsTab.Controls.Add(CreateSettingsPanel());
            
            tabControl.TabPages.AddRange(new TabPage[] { profilesTab, mappingsTab, settingsTab });
            
            this.Controls.Add(tabControl);
        }
        
        #region 配置管理面板
        private Panel CreateProfilesPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            
            // 当前配置标签
            var currentLabel = new Label
            {
                Text = "当前配置:",
                Location = new System.Drawing.Point(20, 10),
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft YaHei", 9, System.Drawing.FontStyle.Bold)
            };
            
            var currentProfileLabel = new Label
            {
                Name = "currentProfileLabel",
                Text = "铁拳配置",
                Location = new System.Drawing.Point(100, 10),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Blue
            };
            
            // 配置列表
            var listLabel = new Label
            {
                Text = "配置列表:",
                Location = new System.Drawing.Point(20, 40),
                AutoSize = true
            };
            
            var profilesListBox = new ListBox
            {
                Name = "profilesListBox",
                Location = new System.Drawing.Point(20, 65),
                Size = new System.Drawing.Size(300, 200)
            };
            profilesListBox.SelectedIndexChanged += ProfilesListBox_SelectedIndexChanged;
            
            // 配置操作按钮
            var switchButton = new Button
            {
                Text = "切换到此配置",
                Location = new System.Drawing.Point(340, 65),
                Size = new System.Drawing.Size(120, 30)
            };
            switchButton.Click += SwitchProfile_Click;
            
            var newButton = new Button
            {
                Text = "新建配置",
                Location = new System.Drawing.Point(340, 105),
                Size = new System.Drawing.Size(120, 30)
            };
            newButton.Click += NewProfile_Click;
            
            var duplicateButton = new Button
            {
                Text = "复制配置",
                Location = new System.Drawing.Point(340, 145),
                Size = new System.Drawing.Size(120, 30)
            };
            duplicateButton.Click += DuplicateProfile_Click;
            
            var deleteButton = new Button
            {
                Text = "删除配置",
                Location = new System.Drawing.Point(340, 185),
                Size = new System.Drawing.Size(120, 30)
            };
            deleteButton.Click += DeleteProfile_Click;
            
            var editButton = new Button
            {
                Text = "编辑配置",
                Location = new System.Drawing.Point(340, 225),
                Size = new System.Drawing.Size(120, 30)
            };
            editButton.Click += EditProfile_Click;
            
            // 导入导出按钮
            var importButton = new Button
            {
                Text = "导入配置",
                Location = new System.Drawing.Point(20, 280),
                Size = new System.Drawing.Size(100, 30)
            };
            importButton.Click += ImportProfile_Click;
            
            var exportButton = new Button
            {
                Text = "导出配置",
                Location = new System.Drawing.Point(130, 280),
                Size = new System.Drawing.Size(100, 30)
            };
            exportButton.Click += ExportProfile_Click;
            
            // 配置信息
            var infoLabel = new Label
            {
                Text = "配置信息:",
                Location = new System.Drawing.Point(20, 320),
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft YaHei", 9, System.Drawing.FontStyle.Bold)
            };
            
            var profileInfoLabel = new Label
            {
                Name = "profileInfoLabel",
                Text = "",
                Location = new System.Drawing.Point(20, 345),
                Size = new System.Drawing.Size(580, 100),
                AutoSize = false
            };
            
            panel.Controls.Add(currentLabel);
            panel.Controls.Add(currentProfileLabel);
            panel.Controls.Add(listLabel);
            panel.Controls.Add(profilesListBox);
            panel.Controls.Add(switchButton);
            panel.Controls.Add(newButton);
            panel.Controls.Add(duplicateButton);
            panel.Controls.Add(deleteButton);
            panel.Controls.Add(editButton);
            panel.Controls.Add(importButton);
            panel.Controls.Add(exportButton);
            panel.Controls.Add(infoLabel);
            panel.Controls.Add(profileInfoLabel);
            
            return panel;
        }
        #endregion
        
        #region 按键映射面板
        private Panel CreateMappingsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            
            var label = new Label
            {
                Text = "当前配置的按键映射（支持大小写敏感，如 w 和 W 可映射不同符号）",
                Location = new System.Drawing.Point(20, 10),
                AutoSize = true
            };
            
            var dataGridView = new DataGridView
            {
                Name = "mappingsGrid",
                Location = new System.Drawing.Point(20, 40),
                Size = new System.Drawing.Size(580, 320),
                AutoGenerateColumns = false
            };
            
            dataGridView.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "Key", HeaderText = "按键", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "Value", HeaderText = "映射值", Width = 400 }
            );
            
            var addButton = new Button
            {
                Text = "添加",
                Location = new System.Drawing.Point(20, 370),
                Size = new System.Drawing.Size(80, 30)
            };
            addButton.Click += AddMapping_Click;
            
            var removeButton = new Button
            {
                Text = "删除",
                Location = new System.Drawing.Point(110, 370),
                Size = new System.Drawing.Size(80, 30)
            };
            removeButton.Click += RemoveMapping_Click;
            
            var saveButton = new Button
            {
                Text = "保存",
                Location = new System.Drawing.Point(420, 370),
                Size = new System.Drawing.Size(80, 30)
            };
            saveButton.Click += SaveButton_Click;
            
            var cancelButton = new Button
            {
                Text = "取消",
                Location = new System.Drawing.Point(510, 370),
                Size = new System.Drawing.Size(80, 30)
            };
            cancelButton.Click += CancelButton_Click;
            
            panel.Controls.Add(label);
            panel.Controls.Add(dataGridView);
            panel.Controls.Add(addButton);
            panel.Controls.Add(removeButton);
            panel.Controls.Add(saveButton);
            panel.Controls.Add(cancelButton);
            
            return panel;
        }
        #endregion
        
        #region 系统设置面板
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
                Text = "提示: 点击输入框后按下热键组合，用于激活/停用铁拳输入状态",
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
        #endregion
        
        #region 事件处理
        private void LoadProfiles()
        {
            currentProfile = profileManager.GetActiveProfile();
            RefreshProfilesList();
            LoadMappingsToGrid();
            LoadSettings();
        }
        
        private void RefreshProfilesList()
        {
            var listBox = this.Controls.Find("profilesListBox", true).FirstOrDefault() as ListBox;
            var currentLabel = this.Controls.Find("currentProfileLabel", true).FirstOrDefault() as Label;
            var infoLabel = this.Controls.Find("profileInfoLabel", true).FirstOrDefault() as Label;
            
            if (listBox != null)
            {
                listBox.Items.Clear();
                var profiles = profileManager.GetAllProfiles();
                foreach (var profile in profiles)
                {
                    string displayName = profile.Name;
                    if (profile.Id == currentProfile.Id)
                        displayName += " [当前]";
                    listBox.Items.Add(new ProfileListItem { Profile = profile, DisplayName = displayName });
                }
            }
            
            if (currentLabel != null)
            {
                currentLabel.Text = currentProfile.Name;
            }
            
            if (infoLabel != null)
            {
                infoLabel.Text = $"名称: {currentProfile.Name}\n" +
                                $"描述: {currentProfile.Description}\n" +
                                $"创建时间: {currentProfile.CreatedAt:yyyy-MM-dd HH:mm}\n" +
                                $"最后修改: {currentProfile.ModifiedAt:yyyy-MM-dd HH:mm}\n" +
                                $"映射数量: {currentProfile.Mappings.Count}";
            }
        }
        
        private void ProfilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;
            var infoLabel = this.Controls.Find("profileInfoLabel", true).FirstOrDefault() as Label;
            
            if (listBox?.SelectedItem is ProfileListItem item && infoLabel != null)
            {
                infoLabel.Text = $"名称: {item.Profile.Name}\n" +
                                $"描述: {item.Profile.Description}\n" +
                                $"创建时间: {item.Profile.CreatedAt:yyyy-MM-dd HH:mm}\n" +
                                $"最后修改: {item.Profile.ModifiedAt:yyyy-MM-dd HH:mm}\n" +
                                $"映射数量: {item.Profile.Mappings.Count}";
            }
        }
        
        private void SwitchProfile_Click(object sender, EventArgs e)
        {
            var listBox = this.Controls.Find("profilesListBox", true).FirstOrDefault() as ListBox;
            if (listBox?.SelectedItem is ProfileListItem item)
            {
                Logger.Info($"SwitchProfile_Click: 尝试切换到配置 {item.Profile.Name} (ID: {item.Profile.Id})");
                if (profileManager.SwitchProfile(item.Profile.Id))
                {
                    currentProfile = profileManager.GetActiveProfile();
                    Logger.Info($"SwitchProfile_Click: 切换成功，当前配置是 {currentProfile.Name}");
                    RefreshProfilesList();
                    LoadMappingsToGrid();
                    LoadSettings();
                }
                else
                {
                    Logger.Error($"SwitchProfile_Click: 切换配置失败");
                }
            }
            else
            {
                Logger.Warning("SwitchProfile_Click: 未选择任何配置");
            }
        }
        
        private void NewProfile_Click(object sender, EventArgs e)
        {
            var dialog = new NewProfileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newProfile = profileManager.CreateProfile(dialog.ProfileName, dialog.Description);
                RefreshProfilesList();
                MessageBox.Show($"已创建新配置: {newProfile.Name}", "成功");
            }
        }
        
        private void DuplicateProfile_Click(object sender, EventArgs e)
        {
            var listBox = this.Controls.Find("profilesListBox", true).FirstOrDefault() as ListBox;
            if (listBox?.SelectedItem is ProfileListItem item)
            {
                var clone = profileManager.DuplicateProfile(item.Profile.Id);
                if (clone != null)
                {
                    RefreshProfilesList();
                    MessageBox.Show($"已复制配置: {clone.Name}", "成功");
                }
            }
            else
            {
                MessageBox.Show("请先选择一个配置", "提示");
            }
        }
        
        private void DeleteProfile_Click(object sender, EventArgs e)
        {
            var listBox = this.Controls.Find("profilesListBox", true).FirstOrDefault() as ListBox;
            if (listBox?.SelectedItem is ProfileListItem item)
            {
                if (item.Profile.Id == currentProfile.Id)
                {
                    MessageBox.Show("不能删除当前正在使用的配置，请先切换到其他配置", "提示");
                    return;
                }
                
                if (MessageBox.Show($"确定要删除配置 \"{item.Profile.Name}\" 吗？", "确认", 
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (profileManager.DeleteProfile(item.Profile.Id))
                    {
                        RefreshProfilesList();
                        MessageBox.Show("配置已删除", "成功");
                    }
                    else
                    {
                        MessageBox.Show("删除失败，至少需要保留一个配置", "错误");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择一个配置", "提示");
            }
        }
        
        private void ImportProfile_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.Title = "导入配置";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var imported = profileManager.ImportProfile(openFileDialog.FileName);
                    if (imported != null)
                    {
                        RefreshProfilesList();
                        MessageBox.Show($"已成功导入配置: {imported.Name}", "成功");
                    }
                    else
                    {
                        MessageBox.Show("导入失败，请检查文件格式是否正确", "错误");
                    }
                }
            }
        }
        
        private void ExportProfile_Click(object sender, EventArgs e)
        {
            var listBox = this.Controls.Find("profilesListBox", true).FirstOrDefault() as ListBox;
            ProfileListItem itemToExport = null;
            
            if (listBox?.SelectedItem is ProfileListItem selectedItem)
            {
                itemToExport = selectedItem;
            }
            else
            {
                // 如果没有选择，导出当前配置
                itemToExport = new ProfileListItem { Profile = currentProfile, DisplayName = currentProfile.Name };
            }
            
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON files (*.json)|*.json";
                saveFileDialog.Title = "导出配置";
                saveFileDialog.FileName = $"{itemToExport.Profile.Name}.json";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (profileManager.ExportProfile(itemToExport.Profile.Id, saveFileDialog.FileName))
                    {
                        MessageBox.Show($"配置已导出到: {saveFileDialog.FileName}", "成功");
                    }
                    else
                    {
                        MessageBox.Show("导出失败", "错误");
                    }
                }
            }
        }
        
        private void EditProfile_Click(object sender, EventArgs e)
        {
            var listBox = this.Controls.Find("profilesListBox", true).FirstOrDefault() as ListBox;
            if (listBox?.SelectedItem is ProfileListItem selectedItem)
            {
                // 切换到选中的配置
                if (selectedItem.Profile.Id != currentProfile.Id)
                {
                    if (profileManager.SwitchProfile(selectedItem.Profile.Id))
                    {
                        currentProfile = profileManager.GetActiveProfile();
                        LoadMappingsToGrid();
                        RefreshProfilesList();
                    }
                }
            }
            
            // 切换到按键映射标签页
            var tabControl = this.Controls.Find("tabControl", true).FirstOrDefault() as TabControl;
            if (tabControl != null && tabControl.TabPages.Count > 1)
            {
                tabControl.SelectedIndex = 1; // 按键映射是第二个标签页
            }
        }
        
        private void LoadMappingsToGrid()
        {
            var dataGridView = this.Controls.Find("mappingsGrid", true)[0] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.Rows.Clear();
                foreach (var mapping in currentProfile.Mappings)
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
            
            var settings = profileManager.GetSettings();
            
            if (runOnStartupCheckbox != null)
                runOnStartupCheckbox.Checked = settings.RunOnStartup;
            
            if (startMinimizedCheckbox != null)
                startMinimizedCheckbox.Checked = settings.StartMinimized;
            
            if (activateHotkeyTextBox != null)
                activateHotkeyTextBox.Text = currentProfile.Hotkeys.Activate ?? "F8";
        }
        
        private void SaveConfig()
        {
            Logger.Info("SaveConfig: 开始保存配置");
            try
            {
                Logger.Info($"SaveConfig: 当前配置ID={currentProfile?.Id}, 名称={currentProfile?.Name}, 映射数量={currentProfile?.Mappings?.Count}");
                
                SaveMappingsFromGrid();
                SaveSettings();
                
                Logger.Info($"SaveConfig: 准备调用UpdateProfile，当前映射数量={currentProfile.Mappings.Count}");
                bool result = profileManager.UpdateProfile(currentProfile);
                Logger.Info($"SaveConfig: UpdateProfile返回 {result}");
                
                if (result)
                {
                    MessageBox.Show("配置已保存", "成功");
                }
                else
                {
                    MessageBox.Show("配置保存失败：无法更新配置", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("SaveConfig: 保存配置时发生异常", ex);
                MessageBox.Show($"配置保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void SaveMappingsFromGrid()
        {
            var dataGridView = this.Controls.Find("mappingsGrid", true)[0] as DataGridView;
            if (dataGridView != null)
            {
                int countBefore = currentProfile.Mappings.Count;
                currentProfile.Mappings.Clear();
                int rowCount = 0;
                
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    // 跳过新行和空行
                    if (row.IsNewRow) continue;
                    if (row.Cells[0].Value == null || row.Cells[1].Value == null) continue;
                    
                    string key = row.Cells[0].Value.ToString().Trim();
                    string value = row.Cells[1].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        currentProfile.Mappings[key] = value;
                        rowCount++;
                    }
                }
                
                Logger.Info($"SaveMappingsFromGrid: 从表格读取了 {rowCount} 行映射，当前配置共有 {currentProfile.Mappings.Count} 个映射");
            }
        }
        
        private void SaveSettings()
        {
            var runOnStartupCheckbox = this.Controls.Find("runOnStartupCheckbox", true).FirstOrDefault() as CheckBox;
            var startMinimizedCheckbox = this.Controls.Find("startMinimizedCheckbox", true).FirstOrDefault() as CheckBox;
            var activateHotkeyTextBox = this.Controls.Find("activateHotkeyTextBox", true).FirstOrDefault() as TextBox;
            
            var settings = profileManager.GetSettings();
            
            if (runOnStartupCheckbox != null)
                settings.RunOnStartup = runOnStartupCheckbox.Checked;
            
            if (startMinimizedCheckbox != null)
                settings.StartMinimized = startMinimizedCheckbox.Checked;
            
            if (activateHotkeyTextBox != null && !string.IsNullOrEmpty(activateHotkeyTextBox.Text))
                currentProfile.Hotkeys.Activate = activateHotkeyTextBox.Text;
            
            profileManager.SaveSettings(settings);
            
            // 应用开机自启设置
            var systemManager = new SystemManager();
            systemManager.IsRunOnStartup = settings.RunOnStartup;
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
            if (dataGridView == null) return;
            
            // 如果有选中的行（通过行头选择）
            if (dataGridView.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    if (!row.IsNewRow)
                        dataGridView.Rows.Remove(row);
                }
            }
            // 如果有选中的单元格，删除该单元格所在的行
            else if (dataGridView.SelectedCells.Count > 0)
            {
                var row = dataGridView.SelectedCells[0].OwningRow;
                if (!row.IsNewRow)
                    dataGridView.Rows.Remove(row);
            }
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
        
        private void SaveButton_Click(object sender, EventArgs e)
        {
            Logger.Info("SaveButton_Click: 用户点击保存按钮");
            SaveConfig();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion
        
        #region 辅助类
        private class ProfileListItem
        {
            public MappingProfile Profile { get; set; }
            public string DisplayName { get; set; }
            
            public override string ToString()
            {
                return DisplayName;
            }
        }
        #endregion
    }
}
