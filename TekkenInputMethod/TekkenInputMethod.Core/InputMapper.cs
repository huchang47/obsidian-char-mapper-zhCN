using System.Collections.Generic;
using TekkenInputMethod.Core.Models;

namespace TekkenInputMethod.Core
{
    public class InputMapper
    {
        private Dictionary<string, string> mappings;
        private bool isActive;
        
        public bool IsActive {
            get { return isActive; }
            set { isActive = value; }
        }
        
        public InputMapper()
        {
            mappings = new Dictionary<string, string>();
            isActive = false;
        }
        
        public void LoadMappings(Dictionary<string, string> newMappings)
        {
            mappings = newMappings;
        }
        
        public string MapKey(string key, bool isShiftPressed = false)
        {
            if (!isActive)
            {
                return null;
            }
            
            // 大小写敏感：如果按下 Shift，使用大写键
            string lookupKey = isShiftPressed ? key.ToUpper() : key.ToLower();
            
            if (mappings.ContainsKey(lookupKey))
            {
                return mappings[lookupKey];
            }
            
            // 如果没有找到大小写敏感的映射，尝试小写（向后兼容）
            string normalizedKey = key.ToLower();
            if (mappings.ContainsKey(normalizedKey))
            {
                return mappings[normalizedKey];
            }
            
            return null;
        }
        
        public void AddMapping(string key, string value)
        {
            mappings[key] = value;
        }
        
        public void RemoveMapping(string key)
        {
            mappings.Remove(key);
        }
        
        public void ClearMappings()
        {
            mappings.Clear();
        }
        
        public Dictionary<string, string> GetMappings()
        {
            return new Dictionary<string, string>(mappings);
        }
        
        public bool HasMapping(string key)
        {
            string normalizedKey = key.ToLower();
            return mappings.ContainsKey(normalizedKey);
        }
    }
}
