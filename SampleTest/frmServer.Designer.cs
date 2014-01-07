namespace SampleTest
{
    partial class frmServer
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
            this.lsvMessage = new BWYou.Control.ListViewBase();
            this.btnTestEnd = new System.Windows.Forms.Button();
            this.btnTestStart = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lsvMessage
            // 
            this.lsvMessage.DateTimeFormat = "HH:mm:ss";
            this.lsvMessage.ItemsMaxCount = 100;
            this.lsvMessage.Location = new System.Drawing.Point(12, 12);
            this.lsvMessage.Name = "lsvMessage";
            this.lsvMessage.ProbBackColor = System.Drawing.Color.GreenYellow;
            this.lsvMessage.ProbForeColor = System.Drawing.Color.Red;
            this.lsvMessage.ProbPriority = BWYou.Base.MessagePriority.Debug;
            this.lsvMessage.Size = new System.Drawing.Size(760, 180);
            this.lsvMessage.TabIndex = 0;
            // 
            // btnTestEnd
            // 
            this.btnTestEnd.Location = new System.Drawing.Point(12, 327);
            this.btnTestEnd.Name = "btnTestEnd";
            this.btnTestEnd.Size = new System.Drawing.Size(165, 23);
            this.btnTestEnd.TabIndex = 75;
            this.btnTestEnd.Text = "테스트 종료";
            this.btnTestEnd.UseVisualStyleBackColor = true;
            this.btnTestEnd.Click += new System.EventHandler(this.btnTestEnd_Click);
            // 
            // btnTestStart
            // 
            this.btnTestStart.Location = new System.Drawing.Point(12, 300);
            this.btnTestStart.Name = "btnTestStart";
            this.btnTestStart.Size = new System.Drawing.Size(165, 23);
            this.btnTestStart.TabIndex = 74;
            this.btnTestStart.Text = "테스트 시작";
            this.btnTestStart.UseVisualStyleBackColor = true;
            this.btnTestStart.Click += new System.EventHandler(this.btnTestStart_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(216, 198);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(238, 96);
            this.richTextBox1.TabIndex = 76;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(460, 198);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(238, 96);
            this.richTextBox2.TabIndex = 77;
            this.richTextBox2.Text = "";
            // 
            // frmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 362);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnTestEnd);
            this.Controls.Add(this.btnTestStart);
            this.Controls.Add(this.lsvMessage);
            this.Name = "frmServer";
            this.Text = "frmServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmServer_FormClosing);
            this.Load += new System.EventHandler(this.frmServer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private BWYou.Control.ListViewBase lsvMessage;
        private System.Windows.Forms.Button btnTestEnd;
        private System.Windows.Forms.Button btnTestStart;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}