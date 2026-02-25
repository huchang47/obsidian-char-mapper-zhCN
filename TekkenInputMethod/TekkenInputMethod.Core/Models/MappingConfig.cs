using System.Collections.Generic;

namespace TekkenInputMethod.Core.Models
{
    public class MappingConfig
    {
        public Dictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>();
        
        public HotkeyConfig Hotkeys { get; set; } = new HotkeyConfig();
        
        public SettingsConfig Settings { get; set; } = new SettingsConfig();
    }
    
    public class HotkeyConfig
    {
        public string Activate { get; set; } = "F8";
    }
    
    public class SettingsConfig
    {
        public bool RunOnStartup { get; set; } = false;
        public bool ShowTrayIcon { get; set; } = true;
        public bool AutoDeactivate { get; set; } = false;
        public bool StartMinimized { get; set; } = false;
    }
}
