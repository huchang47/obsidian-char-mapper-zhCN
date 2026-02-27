using System;
using System.Windows.Forms;

namespace TekkenInputMethod.UI
{
    /// <summary>
    /// 新建配置对话框
    /// </summary>
    public class NewProfileDialog : Form
    {
        private TextBox nameTextBox;
        private TextBox descriptionTextBox;
        
        public string ProfileName { get; private set; }
        public string Description { get; private set; }
        
        public NewProfileDialog()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "新建配置";
            this.Size = new System.Drawing.Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // 名称标签和输入框
            var nameLabel = new Label
            {
                Text = "配置名称:",
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            
            nameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, 45),
                Size = new System.Drawing.Size(340, 23)
            };
            
            // 描述标签和输入框
            var descriptionLabel = new Label
            {
                Text = "配置描述 (可选):",
                Location = new System.Drawing.Point(20, 80),
                AutoSize = true
            };
            
            descriptionTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, 105),
                Size = new System.Drawing.Size(340, 23)
            };
            
            // 按钮
            var okButton = new Button
            {
                Text = "确定",
                Location = new System.Drawing.Point(200, 160),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.OK
            };
            okButton.Click += OkButton_Click;
            
            var cancelButton = new Button
            {
                Text = "取消",
                Location = new System.Drawing.Point(290, 160),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.Cancel
            };
            
            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(descriptionTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
            
            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }
        
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("请输入配置名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
            
            ProfileName = nameTextBox.Text.Trim();
            Description = descriptionTextBox.Text.Trim();
        }
    }
}
