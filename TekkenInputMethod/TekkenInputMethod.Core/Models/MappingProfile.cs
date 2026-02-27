using System.Collections.Generic;

namespace TekkenInputMethod.Core.Models
{
    /// <summary>
    /// 映射配置档案，支持多套配置切换
    /// </summary>
    public class MappingProfile
    {
        /// <summary>
        /// 档案ID
        /// </summary>
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
        
        /// <summary>
        /// 档案名称
        /// </summary>
        public string Name { get; set; } = "铁拳配置";
        
        /// <summary>
        /// 档案描述
        /// </summary>
        public string Description { get; set; } = "";
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreatedAt { get; set; } = System.DateTime.Now;
        
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public System.DateTime ModifiedAt { get; set; } = System.DateTime.Now;
        
        /// <summary>
        /// 按键映射
        /// </summary>
        public Dictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>();
        
        /// <summary>
        /// 热键配置
        /// </summary>
        public HotkeyConfig Hotkeys { get; set; } = new HotkeyConfig();
        
        /// <summary>
        /// 创建副本
        /// </summary>
        public MappingProfile Clone()
        {
            return new MappingProfile
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = this.Name + " (副本)",
                Description = this.Description,
                CreatedAt = System.DateTime.Now,
                ModifiedAt = System.DateTime.Now,
                Mappings = new Dictionary<string, string>(this.Mappings),
                Hotkeys = new HotkeyConfig
                {
                    Activate = this.Hotkeys.Activate
                }
            };
        }
    }
}
