using System;
using System.Windows.Forms;

namespace TekkenInputMethod.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "关于铁拳输入法";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // 创建标题标签
            var titleLabel = new Label
            {
                Text = "铁拳输入法",
                Font = new System.Drawing.Font("Microsoft YaHei", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            
            // 创建版本标签
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var versionLabel = new Label
            {
                Text = $"版本 {version?.ToString(3) ?? "1.0.1"}",
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };
            
            // 创建描述文本
            var descriptionLabel = new Label
            {
                Text = "一款专为铁拳游戏玩家设计的Windows输入法，\n帮助玩家快速输入出招指令符号。",
                Location = new System.Drawing.Point(20, 90),
                AutoSize = true
            };
            
            // 创建作者标签
            var authorLabel = new Label
            {
                Text = "作者：会飞的猪，胡里胡涂",
                Location = new System.Drawing.Point(20, 140),
                AutoSize = true
            };
            
            // 创建版权标签
            var copyrightLabel = new Label
            {
                Text = "© 2026 铁拳输入法",
                Location = new System.Drawing.Point(20, 170),
                AutoSize = true
            };
            
            // 创建许可证标签
            var licenseLabel = new Label
            {
                Text = "许可证：MIT License",
                Location = new System.Drawing.Point(20, 200),
                AutoSize = true
            };
            
            // 创建确定按钮
            var okButton = new Button
            {
                Text = "确定",
                Location = new System.Drawing.Point(300, 230),
                Size = new System.Drawing.Size(80, 30)
            };
            okButton.Click += OkButton_Click;
            
            // 添加控件
            this.Controls.Add(titleLabel);
            this.Controls.Add(versionLabel);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(authorLabel);
            this.Controls.Add(copyrightLabel);
            this.Controls.Add(licenseLabel);
            this.Controls.Add(okButton);
        }
        
        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
