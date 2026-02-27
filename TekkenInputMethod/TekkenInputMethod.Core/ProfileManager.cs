using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TekkenInputMethod.Core.Models;

namespace TekkenInputMethod.Core
{
    /// <summary>
    /// 配置档案管理器，每个配置为独立的JSON文件
    /// </summary>
    public class ProfileManager
    {
        private string ProfilesDirectory { get; }
        private string SettingsFilePath { get; }
        private List<MappingProfile> _profiles;
        private string _activeProfileId;
        private SettingsConfig _settings;
        
        public ProfileManager()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ProfilesDirectory = Path.Combine(appDataPath, "TekkenInputMethod", "Profiles");
            SettingsFilePath = Path.Combine(ProfilesDirectory, "settings.json");
            
            if (!Directory.Exists(ProfilesDirectory))
            {
                Directory.CreateDirectory(ProfilesDirectory);
            }
            
            _profiles = new List<MappingProfile>();
            _settings = new SettingsConfig();
            
            LoadAllProfiles();
            LoadSettings();
            
            // 如果没有配置，创建默认配置
            if (_profiles.Count == 0)
            {
                CreateDefaultProfiles();
            }
            
            // 确保有激活的配置
            if (string.IsNullOrEmpty(_activeProfileId) || 
                !_profiles.Any(p => p.Id == _activeProfileId))
            {
                _activeProfileId = _profiles.First().Id;
                SaveSettings();
            }
        }
        
        /// <summary>
        /// 加载所有配置档案
        /// </summary>
        private void LoadAllProfiles()
        {
            _profiles.Clear();
            
            try
            {
                var profileFiles = Directory.GetFiles(ProfilesDirectory, "*.json")
                    .Where(f => Path.GetFileName(f) != "settings.json");
                
                foreach (var filePath in profileFiles)
                {
                    try
                    {
                        string json = File.ReadAllText(filePath);
                        var profile = JsonSerializer.Deserialize<MappingProfile>(json);
                        if (profile != null)
                        {
                            _profiles.Add(profile);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"加载配置文件失败: {filePath}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("加载配置档案列表失败", ex);
            }
        }
        
        /// <summary>
        /// 加载设置
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    if (settings != null)
                    {
                        _activeProfileId = settings.ActiveProfileId ?? "";
                        _settings = settings.Settings ?? new SettingsConfig();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("加载设置失败", ex);
            }
        }
        
        /// <summary>
        /// 保存设置
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                var settings = new AppSettings
                {
                    ActiveProfileId = _activeProfileId,
                    Settings = _settings
                };
                
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                Logger.Error("保存设置失败", ex);
            }
        }
        
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        private string GetProfileFilePath(string profileId)
        {
            return Path.Combine(ProfilesDirectory, $"{profileId}.json");
        }
        
        /// <summary>
        /// 保存单个配置到文件
        /// </summary>
        private bool SaveProfileToFile(MappingProfile profile)
        {
            try
            {
                string filePath = GetProfileFilePath(profile.Id);
                string json = JsonSerializer.Serialize(profile, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(filePath, json);
                Logger.Info($"配置已保存到: {filePath}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"保存配置文件失败: {profile.Id}", ex);
                return false;
            }
        }
        
        /// <summary>
        /// 删除配置文件
        /// </summary>
        private bool DeleteProfileFile(string profileId)
        {
            try
            {
                string filePath = GetProfileFilePath(profileId);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Logger.Info($"配置文件已删除: {filePath}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"删除配置文件失败: {profileId}", ex);
                return false;
            }
        }
        
        /// <summary>
        /// 获取所有配置档案
        /// </summary>
        public List<MappingProfile> GetAllProfiles()
        {
            return _profiles;
        }
        
        /// <summary>
        /// 获取当前激活的配置
        /// </summary>
        public MappingProfile GetActiveProfile()
        {
            return _profiles.FirstOrDefault(p => p.Id == _activeProfileId) 
                   ?? _profiles.First();
        }
        
        /// <summary>
        /// 切换配置
        /// </summary>
        public bool SwitchProfile(string profileId)
        {
            if (_profiles.Any(p => p.Id == profileId))
            {
                _activeProfileId = profileId;
                SaveSettings();
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 创建新配置
        /// </summary>
        public MappingProfile CreateProfile(string name, string description = "")
        {
            var profile = new MappingProfile
            {
                Name = name,
                Description = description,
                Mappings = new Dictionary<string, string>(PresetMappings.DefaultMapping),
                Hotkeys = new HotkeyConfig()
            };
            
            _profiles.Add(profile);
            SaveProfileToFile(profile);
            return profile;
        }
        
        /// <summary>
        /// 复制配置
        /// </summary>
        public MappingProfile DuplicateProfile(string profileId)
        {
            var source = _profiles.FirstOrDefault(p => p.Id == profileId);
            if (source == null) return null;
            
            var clone = source.Clone();
            _profiles.Add(clone);
            SaveProfileToFile(clone);
            return clone;
        }
        
        /// <summary>
        /// 删除配置
        /// </summary>
        public bool DeleteProfile(string profileId)
        {
            // 不能删除最后一个配置
            if (_profiles.Count <= 1) return false;
            
            var profile = _profiles.FirstOrDefault(p => p.Id == profileId);
            if (profile == null) return false;
            
            _profiles.Remove(profile);
            DeleteProfileFile(profileId);
            
            // 如果删除的是当前激活的配置，切换到第一个
            if (_activeProfileId == profileId)
            {
                _activeProfileId = _profiles.First().Id;
                SaveSettings();
            }
            
            return true;
        }
        
        /// <summary>
        /// 更新配置
        /// </summary>
        public bool UpdateProfile(MappingProfile profile)
        {
            if (profile == null)
            {
                Logger.Error("UpdateProfile: profile 为 null");
                return false;
            }
            
            var existing = _profiles.FirstOrDefault(p => p.Id == profile.Id);
            if (existing == null) 
            {
                Logger.Error($"UpdateProfile: 找不到ID为 {profile.Id} 的配置");
                return false;
            }
            
            // 如果 existing 和 profile 是同一个对象，直接保存
            // 如果不是同一个对象，需要复制属性
            if (!ReferenceEquals(existing, profile))
            {
                existing.Name = profile.Name;
                existing.Description = profile.Description;
                existing.Mappings = new Dictionary<string, string>(profile.Mappings);
                existing.Hotkeys = new HotkeyConfig 
                { 
                    Activate = profile.Hotkeys.Activate 
                };
            }
            existing.ModifiedAt = DateTime.Now;
            
            Logger.Info($"UpdateProfile: 正在保存配置 {existing.Name}，映射数量: {existing.Mappings.Count}");
            return SaveProfileToFile(existing);
        }
        
        /// <summary>
        /// 导出配置到文件
        /// </summary>
        public bool ExportProfile(string profileId, string filePath)
        {
            try
            {
                var profile = _profiles.FirstOrDefault(p => p.Id == profileId);
                if (profile == null) return false;
                
                var exportData = new
                {
                    Version = "1.0",
                    ExportTime = DateTime.Now,
                    Profile = profile
                };
                
                string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("导出配置失败", ex);
                return false;
            }
        }
        
        /// <summary>
        /// 从文件导入配置
        /// </summary>
        public MappingProfile ImportProfile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var exportData = JsonSerializer.Deserialize<ExportData>(json);
                
                if (exportData?.Profile == null) return null;
                
                // 生成新ID，避免冲突
                exportData.Profile.Id = Guid.NewGuid().ToString();
                exportData.Profile.Name = exportData.Profile.Name + " (导入)";
                exportData.Profile.CreatedAt = DateTime.Now;
                exportData.Profile.ModifiedAt = DateTime.Now;
                
                _profiles.Add(exportData.Profile);
                SaveProfileToFile(exportData.Profile);
                
                return exportData.Profile;
            }
            catch (Exception ex)
            {
                Logger.Error("导入配置失败", ex);
                return null;
            }
        }
        
        /// <summary>
        /// 获取应用设置
        /// </summary>
        public SettingsConfig GetSettings()
        {
            return _settings;
        }
        
        /// <summary>
        /// 保存应用设置
        /// </summary>
        public void SaveSettings(SettingsConfig settings)
        {
            _settings = settings;
            SaveSettings();
        }
        
        private void CreateDefaultProfiles()
        {
            // 创建铁拳配置
            var tekkenProfile = new MappingProfile
            {
                Name = "铁拳配置",
                Description = "专为铁拳游戏设计，包含完整的方向键和按钮映射",
                Mappings = new Dictionary<string, string>(PresetMappings.GamepadMode),
                Hotkeys = new HotkeyConfig()
            };
            
            // 创建方向键配置
            var arrowProfile = new MappingProfile
            {
                Name = "方向键配置",
                Description = "简洁的方向键映射，适合一般用途",
                Mappings = new Dictionary<string, string>(PresetMappings.ArrowKeysMode),
                Hotkeys = new HotkeyConfig()
            };
            
            _profiles.Add(tekkenProfile);
            _profiles.Add(arrowProfile);
            
            _activeProfileId = tekkenProfile.Id;
            
            SaveProfileToFile(tekkenProfile);
            SaveProfileToFile(arrowProfile);
            SaveSettings();
        }
        
        private class AppSettings
        {
            public string ActiveProfileId { get; set; } = "";
            public SettingsConfig Settings { get; set; } = new SettingsConfig();
        }
        
        private class ExportData
        {
            public string Version { get; set; } = "";
            public DateTime ExportTime { get; set; }
            public MappingProfile Profile { get; set; } = new MappingProfile();
        }
    }
}
