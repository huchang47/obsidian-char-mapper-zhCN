using System.Collections.Generic;

namespace TekkenInputMethod.Core.Models
{
    public static class PresetMappings
    {
        public static Dictionary<string, string> GamepadMode {
            get {
                return new Dictionary<string, string>
                {
                    { "w", "⬆" },
                    { "a", "⬅" },
                    { "s", "⬇" },
                    { "d", "➡" },
                    { "W", "⇈" },
                    { "S", "⇊" },
                    { "q", "↖" },
                    { "e", "↗" },
                    { "z", "↙" },
                    { "c", "↘" },
                    { "x", "⇩" },
                    { "j", "❶" },
                    { "i", "❷" },
                    { "k", "❸" },
                    { "l", "❹" },
                    { "u", "❶✚❷" },
                    { "o", "❸✚❹" },
                    { " ", "▸" },
                    { "n", "✩" },
                    { "h", "Ⓗ" },
                    { "r", "Ⓡ" },
                    { "t", "Ⓣ" },
                    { "b", "Ⓑ" }
                };
            }
        }
        
        public static Dictionary<string, string> ArrowKeysMode {
            get {
                return new Dictionary<string, string>
                {
                    { "w", "↑" },
                    { "a", "←" },
                    { "s", "↓" },
                    { "d", "→" },
                    { "q", "↖" },
                    { "e", "↗" },
                    { "z", "↙" },
                    { "c", "↘" }
                };
            }
        }
        
        public static Dictionary<string, string> DefaultMapping {
            get {
                return GamepadMode;
            }
        }
        
        public static string GetPresetName(int presetId)
        {
            switch (presetId)
            {
                case 0:
                    return "铁拳模式";
                case 1:
                    return "方向键模式";
                case 2:
                    return "自定义模式";
                default:
                    return "未知模式";
            }
        }
        
        public static Dictionary<string, string> GetPresetById(int presetId)
        {
            switch (presetId)
            {
                case 0:
                    return GamepadMode;
                case 1:
                    return ArrowKeysMode;
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}
