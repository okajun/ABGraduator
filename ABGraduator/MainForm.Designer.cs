namespace ABGraduator
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.Record = new System.Windows.Forms.Button();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.labelLatest = new System.Windows.Forms.Label();
            this.labelStore = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(188, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(57, 25);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Record
            // 
            this.Record.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Record.Location = new System.Drawing.Point(124, 2);
            this.Record.Margin = new System.Windows.Forms.Padding(4);
            this.Record.Name = "Record";
            this.Record.Size = new System.Drawing.Size(57, 25);
            this.Record.TabIndex = 1;
            this.Record.Text = "Mark";
            this.Record.UseVisualStyleBackColor = true;
            this.Record.Click += new System.EventHandler(this.mark_Click);
            // 
            // labelCurrent
            // 
            this.labelCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrent.BackColor = System.Drawing.Color.PaleGreen;
            this.labelCurrent.Location = new System.Drawing.Point(9, 31);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(72, 23);
            this.labelCurrent.TabIndex = 2;
            this.labelCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLatest
            // 
            this.labelLatest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelLatest.BackColor = System.Drawing.Color.LavenderBlush;
            this.labelLatest.Location = new System.Drawing.Point(87, 31);
            this.labelLatest.Name = "labelLatest";
            this.labelLatest.Size = new System.Drawing.Size(72, 23);
            this.labelLatest.TabIndex = 2;
            this.labelLatest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelStore
            // 
            this.labelStore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStore.BackColor = System.Drawing.Color.LightPink;
            this.labelStore.Location = new System.Drawing.Point(165, 31);
            this.labelStore.Name = "labelStore";
            this.labelStore.Size = new System.Drawing.Size(72, 23);
            this.labelStore.TabIndex = 2;
            this.labelStore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(246, 60);
            this.ControlBox = false;
            this.Controls.Add(this.labelStore);
            this.Controls.Add(this.labelLatest);
            this.Controls.Add(this.labelCurrent);
            this.Controls.Add(this.Record);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Opacity = 0.75D;
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button Record;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.Label labelLatest;
        private System.Windows.Forms.Label labelStore;
    }
}

