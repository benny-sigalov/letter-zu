namespace Borman.ZuSharp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.uxActiveUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.uxNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.uxMenuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uxToggleEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxRebindCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.uxComboLayout = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uxCheckUseScrollLock = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uxCheckUseEscape = new System.Windows.Forms.CheckBox();
            this.uxStartWithWindows = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.uxGermanKeyboard = new System.Windows.Forms.CheckBox();
            this.uxMenuContext.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxActiveUpdateTimer
            // 
            this.uxActiveUpdateTimer.Tick += new System.EventHandler(this.uxActiveUpdateTimer_Tick);
            // 
            // uxNotifyIcon
            // 
            this.uxNotifyIcon.Text = "Letter Zu - double click to restore";
            this.uxNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.uxNotifyIconActive_MouseDoubleClick);
            // 
            // uxMenuContext
            // 
            this.uxMenuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxToggleEnable,
            this.showToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.uxMenuContext.Name = "uxMenuContext";
            this.uxMenuContext.Size = new System.Drawing.Size(202, 98);
            // 
            // uxToggleEnable
            // 
            this.uxToggleEnable.Name = "uxToggleEnable";
            this.uxToggleEnable.Size = new System.Drawing.Size(174, 22);
            this.uxToggleEnable.Text = "Перевод Включён";
            this.uxToggleEnable.Click += new System.EventHandler(this.uxToggleEnable_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.showToolStripMenuItem.Text = "Показать главное окно";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(171, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exitToolStripMenuItem.Text = "Выйти";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // uxRebindCheckTimer
            // 
            this.uxRebindCheckTimer.Enabled = true;
            this.uxRebindCheckTimer.Interval = 1000;
            this.uxRebindCheckTimer.Tick += new System.EventHandler(this.uxRebindCheckTimer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(373, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.turnOnToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.fileToolStripMenuItem.Text = "Файл";
            // 
            // turnOnToolStripMenuItem
            // 
            this.turnOnToolStripMenuItem.Name = "turnOnToolStripMenuItem";
            this.turnOnToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.turnOnToolStripMenuItem.Text = "Перевод Включён";
            this.turnOnToolStripMenuItem.Click += new System.EventHandler(this.turnOnToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(171, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.exitToolStripMenuItem1.Text = "Выйти";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.helpToolStripMenuItem.Text = "Помощь";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.manualToolStripMenuItem.Text = "Прочти меня";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.aboutToolStripMenuItem.Text = "О программе";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Привязано к раскладке:";
            // 
            // uxComboLayout
            // 
            this.uxComboLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxComboLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uxComboLayout.FormattingEnabled = true;
            this.uxComboLayout.Location = new System.Drawing.Point(168, 36);
            this.uxComboLayout.Name = "uxComboLayout";
            this.uxComboLayout.Size = new System.Drawing.Size(193, 21);
            this.uxComboLayout.TabIndex = 1;
            this.uxComboLayout.SelectedIndexChanged += new System.EventHandler(this.uxComboLayout_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Использовать Scroll Lock";
            // 
            // uxCheckUseScrollLock
            // 
            this.uxCheckUseScrollLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCheckUseScrollLock.AutoSize = true;
            this.uxCheckUseScrollLock.Location = new System.Drawing.Point(346, 63);
            this.uxCheckUseScrollLock.Name = "uxCheckUseScrollLock";
            this.uxCheckUseScrollLock.Size = new System.Drawing.Size(15, 14);
            this.uxCheckUseScrollLock.TabIndex = 3;
            this.uxCheckUseScrollLock.UseVisualStyleBackColor = true;
            this.uxCheckUseScrollLock.CheckedChanged += new System.EventHandler(this.uxCheckUseScrollLock_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Использовать Escape";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(231, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Активизировать с включением компьютера";
            // 
            // uxCheckUseEscape
            // 
            this.uxCheckUseEscape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCheckUseEscape.AutoSize = true;
            this.uxCheckUseEscape.Location = new System.Drawing.Point(346, 83);
            this.uxCheckUseEscape.Name = "uxCheckUseEscape";
            this.uxCheckUseEscape.Size = new System.Drawing.Size(15, 14);
            this.uxCheckUseEscape.TabIndex = 5;
            this.uxCheckUseEscape.UseVisualStyleBackColor = true;
            this.uxCheckUseEscape.CheckedChanged += new System.EventHandler(this.uxCheckUseEscape_CheckedChanged);
            // 
            // uxStartWithWindows
            // 
            this.uxStartWithWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxStartWithWindows.AutoSize = true;
            this.uxStartWithWindows.Location = new System.Drawing.Point(346, 104);
            this.uxStartWithWindows.Name = "uxStartWithWindows";
            this.uxStartWithWindows.Size = new System.Drawing.Size(15, 14);
            this.uxStartWithWindows.TabIndex = 5;
            this.uxStartWithWindows.UseVisualStyleBackColor = true;
            this.uxStartWithWindows.CheckedChanged += new System.EventHandler(this.uxStartWithWindows_Changed);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label5.Location = new System.Drawing.Point(12, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(229, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Пожалуйста, поддержите проект";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.linkLabel1.Location = new System.Drawing.Point(247, 158);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(113, 15);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "PayPal Donation";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "У меня немецкая клавиатура";
            // 
            // uxGermanKeyboard
            // 
            this.uxGermanKeyboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxGermanKeyboard.AutoSize = true;
            this.uxGermanKeyboard.Location = new System.Drawing.Point(346, 127);
            this.uxGermanKeyboard.Name = "uxGermanKeyboard";
            this.uxGermanKeyboard.Size = new System.Drawing.Size(15, 14);
            this.uxGermanKeyboard.TabIndex = 9;
            this.uxGermanKeyboard.UseVisualStyleBackColor = true;
            this.uxGermanKeyboard.CheckedChanged += new System.EventHandler(this.uxGermanKeyboard_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(223)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(373, 185);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.uxGermanKeyboard);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.uxStartWithWindows);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.uxCheckUseEscape);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uxComboLayout);
            this.Controls.Add(this.uxCheckUseScrollLock);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Буква Зю";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.uxMenuContext.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer uxActiveUpdateTimer;
        private System.Windows.Forms.NotifyIcon uxNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip uxMenuContext;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uxToggleEnable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Timer uxRebindCheckTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox uxComboLayout;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox uxCheckUseScrollLock;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox uxCheckUseEscape;
        private System.Windows.Forms.CheckBox uxStartWithWindows;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox uxGermanKeyboard;
    }
}

