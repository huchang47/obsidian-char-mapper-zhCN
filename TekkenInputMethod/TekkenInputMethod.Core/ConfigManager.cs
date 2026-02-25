using System;
using System.IO;
using System.Text.Json;
using TekkenInputMethod.Core.Models;

namespace TekkenInputMethod.Core
{
    public class ConfigManager
    {
        private string ConfigFilePath { get; }
        
        public ConfigManager()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "TekkenInputMethod");
            
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }
            
            ConfigFilePath = Path.Combine(appFolder, "config.json");
        }
        
        public MappingConfig LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string jsonContent = File.ReadAllText(ConfigFilePath);
                    return JsonSerializer.Deserialize<MappingConfig>(jsonContent);
                }
                else
                {
                    return CreateDefaultConfig();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading config: {ex.Message}");
                return CreateDefaultConfig();
            }
        }
        
        public void SaveConfig(MappingConfig config)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(config, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                File.WriteAllText(ConfigFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving config: {ex.Message}");
            }
        }
        
        public void ResetToDefault()
        {
            MappingConfig defaultConfig = CreateDefaultConfig();
            SaveConfig(defaultConfig);
        }
        
        public void ApplyPreset(int presetId)
        {
            MappingConfig currentConfig = LoadConfig();
            currentConfig.Mappings = PresetMappings.GetPresetById(presetId);
            SaveConfig(currentConfig);
        }
        
        private MappingConfig CreateDefaultConfig()
        {
            return new MappingConfig
            {
                Mappings = PresetMappings.DefaultMapping,
                Hotkeys = new HotkeyConfig(),
                Settings = new SettingsConfig()
            };
        }
        
        public string GetConfigPath()
        {
            return ConfigFilePath;
        }
    }
}
