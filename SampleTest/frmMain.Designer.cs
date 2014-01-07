namespace SampleTest
{
    partial class frmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMSSQLID = new System.Windows.Forms.Label();
            this.lblMSSQLPW = new System.Windows.Forms.Label();
            this.txtMSSQLID = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtMSSQLPW = new System.Windows.Forms.TextBox();
            this.lblMysqlID = new System.Windows.Forms.Label();
            this.lblSCP_ID = new System.Windows.Forms.Label();
            this.txtSCP_ID = new System.Windows.Forms.TextBox();
            this.txtSCP_PW = new System.Windows.Forms.TextBox();
            this.lblSCP_PW = new System.Windows.Forms.Label();
            this.lblMysqlPW = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtMysqlID = new System.Windows.Forms.TextBox();
            this.txtMysqlPW = new System.Windows.Forms.TextBox();
            this.btnTestTransfer = new System.Windows.Forms.Button();
            this.btnTestCompress = new System.Windows.Forms.Button();
            this.btnTestDB = new System.Windows.Forms.Button();
            this.btnTest2 = new System.Windows.Forms.Button();
            this.progReadDCM = new BWYou.Control.ProgressBarBase();
            this.progCompress = new BWYou.Control.ProgressBarBase();
            this.progTransfer = new BWYou.Control.ProgressBarBase();
            this.lsvMessage = new BWYou.Control.ListViewBase();
            this.btnGC = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMSSQLID
            // 
            this.lblMSSQLID.AutoSize = true;
            this.lblMSSQLID.Location = new System.Drawing.Point(13, 119);
            this.lblMSSQLID.Name = "lblMSSQLID";
            this.lblMSSQLID.Size = new System.Drawing.Size(88, 12);
            this.lblMSSQLID.TabIndex = 11;
            this.lblMSSQLID.Text = "MSSQL 아이디";
            // 
            // lblMSSQLPW
            // 
            this.lblMSSQLPW.AutoSize = true;
            this.lblMSSQLPW.Location = new System.Drawing.Point(13, 146);
            this.lblMSSQLPW.Name = "lblMSSQLPW";
            this.lblMSSQLPW.Size = new System.Drawing.Size(76, 12);
            this.lblMSSQLPW.TabIndex = 12;
            this.lblMSSQLPW.Text = "MSSQL 암호";
            // 
            // txtMSSQLID
            // 
            this.txtMSSQLID.Location = new System.Drawing.Point(117, 116);
            this.txtMSSQLID.Name = "txtMSSQLID";
            this.txtMSSQLID.Size = new System.Drawing.Size(100, 21);
            this.txtMSSQLID.TabIndex = 9;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 70);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(165, 23);
            this.btnTest.TabIndex = 73;
            this.btnTest.Text = "테스트 시작";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtMSSQLPW
            // 
            this.txtMSSQLPW.Location = new System.Drawing.Point(117, 143);
            this.txtMSSQLPW.Name = "txtMSSQLPW";
            this.txtMSSQLPW.Size = new System.Drawing.Size(100, 21);
            this.txtMSSQLPW.TabIndex = 10;
            this.txtMSSQLPW.UseSystemPasswordChar = true;
            // 
            // lblMysqlID
            // 
            this.lblMysqlID.AutoSize = true;
            this.lblMysqlID.Location = new System.Drawing.Point(13, 65);
            this.lblMysqlID.Name = "lblMysqlID";
            this.lblMysqlID.Size = new System.Drawing.Size(80, 12);
            this.lblMysqlID.TabIndex = 7;
            this.lblMysqlID.Text = "Mysql 아이디";
            // 
            // lblSCP_ID
            // 
            this.lblSCP_ID.AutoSize = true;
            this.lblSCP_ID.Location = new System.Drawing.Point(13, 11);
            this.lblSCP_ID.Name = "lblSCP_ID";
            this.lblSCP_ID.Size = new System.Drawing.Size(98, 12);
            this.lblSCP_ID.TabIndex = 3;
            this.lblSCP_ID.Text = "SCP 연결 아이디";
            // 
            // txtSCP_ID
            // 
            this.txtSCP_ID.Location = new System.Drawing.Point(117, 8);
            this.txtSCP_ID.Name = "txtSCP_ID";
            this.txtSCP_ID.Size = new System.Drawing.Size(100, 21);
            this.txtSCP_ID.TabIndex = 1;
            // 
            // txtSCP_PW
            // 
            this.txtSCP_PW.Location = new System.Drawing.Point(117, 35);
            this.txtSCP_PW.Name = "txtSCP_PW";
            this.txtSCP_PW.Size = new System.Drawing.Size(100, 21);
            this.txtSCP_PW.TabIndex = 2;
            this.txtSCP_PW.UseSystemPasswordChar = true;
            // 
            // lblSCP_PW
            // 
            this.lblSCP_PW.AutoSize = true;
            this.lblSCP_PW.Location = new System.Drawing.Point(13, 38);
            this.lblSCP_PW.Name = "lblSCP_PW";
            this.lblSCP_PW.Size = new System.Drawing.Size(86, 12);
            this.lblSCP_PW.TabIndex = 4;
            this.lblSCP_PW.Text = "SCP 연결 암호";
            // 
            // lblMysqlPW
            // 
            this.lblMysqlPW.AutoSize = true;
            this.lblMysqlPW.Location = new System.Drawing.Point(13, 92);
            this.lblMysqlPW.Name = "lblMysqlPW";
            this.lblMysqlPW.Size = new System.Drawing.Size(68, 12);
            this.lblMysqlPW.TabIndex = 8;
            this.lblMysqlPW.Text = "Mysql 암호";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblMSSQLID);
            this.panel1.Controls.Add(this.lblMSSQLPW);
            this.panel1.Controls.Add(this.txtMSSQLID);
            this.panel1.Controls.Add(this.txtMSSQLPW);
            this.panel1.Controls.Add(this.lblMysqlID);
            this.panel1.Controls.Add(this.lblMysqlPW);
            this.panel1.Controls.Add(this.txtMysqlID);
            this.panel1.Controls.Add(this.txtMysqlPW);
            this.panel1.Controls.Add(this.lblSCP_ID);
            this.panel1.Controls.Add(this.lblSCP_PW);
            this.panel1.Controls.Add(this.txtSCP_ID);
            this.panel1.Controls.Add(this.txtSCP_PW);
            this.panel1.Location = new System.Drawing.Point(435, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 173);
            this.panel1.TabIndex = 74;
            // 
            // txtMysqlID
            // 
            this.txtMysqlID.Location = new System.Drawing.Point(117, 62);
            this.txtMysqlID.Name = "txtMysqlID";
            this.txtMysqlID.Size = new System.Drawing.Size(100, 21);
            this.txtMysqlID.TabIndex = 5;
            // 
            // txtMysqlPW
            // 
            this.txtMysqlPW.Location = new System.Drawing.Point(117, 89);
            this.txtMysqlPW.Name = "txtMysqlPW";
            this.txtMysqlPW.Size = new System.Drawing.Size(100, 21);
            this.txtMysqlPW.TabIndex = 6;
            this.txtMysqlPW.UseSystemPasswordChar = true;
            // 
            // btnTestTransfer
            // 
            this.btnTestTransfer.Location = new System.Drawing.Point(12, 212);
            this.btnTestTransfer.Name = "btnTestTransfer";
            this.btnTestTransfer.Size = new System.Drawing.Size(87, 23);
            this.btnTestTransfer.TabIndex = 79;
            this.btnTestTransfer.Text = "전송 테스트";
            this.btnTestTransfer.UseVisualStyleBackColor = true;
            this.btnTestTransfer.Click += new System.EventHandler(this.btnTestTransfer_Click);
            // 
            // btnTestCompress
            // 
            this.btnTestCompress.Location = new System.Drawing.Point(105, 212);
            this.btnTestCompress.Name = "btnTestCompress";
            this.btnTestCompress.Size = new System.Drawing.Size(87, 23);
            this.btnTestCompress.TabIndex = 80;
            this.btnTestCompress.Text = "압축 테스트";
            this.btnTestCompress.UseVisualStyleBackColor = true;
            this.btnTestCompress.Click += new System.EventHandler(this.btnTestCompress_Click);
            // 
            // btnTestDB
            // 
            this.btnTestDB.Location = new System.Drawing.Point(198, 212);
            this.btnTestDB.Name = "btnTestDB";
            this.btnTestDB.Size = new System.Drawing.Size(87, 23);
            this.btnTestDB.TabIndex = 81;
            this.btnTestDB.Text = "DB 테스트";
            this.btnTestDB.UseVisualStyleBackColor = true;
            this.btnTestDB.Click += new System.EventHandler(this.btnTestDB_Click);
            // 
            // btnTest2
            // 
            this.btnTest2.Location = new System.Drawing.Point(12, 97);
            this.btnTest2.Name = "btnTest2";
            this.btnTest2.Size = new System.Drawing.Size(165, 23);
            this.btnTest2.TabIndex = 73;
            this.btnTest2.Text = "테스트 종료";
            this.btnTest2.UseVisualStyleBackColor = true;
            this.btnTest2.Click += new System.EventHandler(this.btnTest2_Click);
            // 
            // progReadDCM
            // 
            this.progReadDCM.Location = new System.Drawing.Point(12, 37);
            this.progReadDCM.Name = "progReadDCM";
            this.progReadDCM.Size = new System.Drawing.Size(650, 19);
            this.progReadDCM.TabIndex = 85;
            // 
            // progCompress
            // 
            this.progCompress.Location = new System.Drawing.Point(344, 12);
            this.progCompress.Name = "progCompress";
            this.progCompress.Size = new System.Drawing.Size(318, 19);
            this.progCompress.TabIndex = 84;
            // 
            // progTransfer
            // 
            this.progTransfer.Location = new System.Drawing.Point(12, 12);
            this.progTransfer.Name = "progTransfer";
            this.progTransfer.Size = new System.Drawing.Size(326, 19);
            this.progTransfer.TabIndex = 83;
            // 
            // lsvMessage
            // 
            this.lsvMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvMessage.DateTimeFormat = "HH:mm:ss";
            this.lsvMessage.ItemsMaxCount = 100;
            this.lsvMessage.Location = new System.Drawing.Point(12, 251);
            this.lsvMessage.Name = "lsvMessage";
            this.lsvMessage.ProbBackColor = System.Drawing.Color.GreenYellow;
            this.lsvMessage.ProbForeColor = System.Drawing.Color.Red;
            this.lsvMessage.ProbPriority = BWYou.Base.MessagePriority.Debug;
            this.lsvMessage.Size = new System.Drawing.Size(883, 350);
            this.lsvMessage.TabIndex = 82;
            // 
            // btnGC
            // 
            this.btnGC.Location = new System.Drawing.Point(183, 97);
            this.btnGC.Name = "btnGC";
            this.btnGC.Size = new System.Drawing.Size(165, 23);
            this.btnGC.TabIndex = 73;
            this.btnGC.Text = "GC";
            this.btnGC.UseVisualStyleBackColor = true;
            this.btnGC.Click += new System.EventHandler(this.btnGC_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 613);
            this.Controls.Add(this.progReadDCM);
            this.Controls.Add(this.progCompress);
            this.Controls.Add(this.progTransfer);
            this.Controls.Add(this.lsvMessage);
            this.Controls.Add(this.btnTestDB);
            this.Controls.Add(this.btnTestCompress);
            this.Controls.Add(this.btnTestTransfer);
            this.Controls.Add(this.btnGC);
            this.Controls.Add(this.btnTest2);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.panel1);
            this.Name = "frmMain";
            this.Text = "SampleTest";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMSSQLID;
        private System.Windows.Forms.Label lblMSSQLPW;
        private System.Windows.Forms.TextBox txtMSSQLID;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox txtMSSQLPW;
        private System.Windows.Forms.Label lblMysqlID;
        private System.Windows.Forms.Label lblSCP_ID;
        private System.Windows.Forms.TextBox txtSCP_ID;
        private System.Windows.Forms.TextBox txtSCP_PW;
        private System.Windows.Forms.Label lblSCP_PW;
        private System.Windows.Forms.Label lblMysqlPW;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtMysqlID;
        private System.Windows.Forms.TextBox txtMysqlPW;
        private System.Windows.Forms.Button btnTestTransfer;
        private System.Windows.Forms.Button btnTestCompress;
        private System.Windows.Forms.Button btnTestDB;
        private BWYou.Control.ListViewBase lsvMessage;
        private BWYou.Control.ProgressBarBase progTransfer;
        private BWYou.Control.ProgressBarBase progCompress;
        private BWYou.Control.ProgressBarBase progReadDCM;
        private System.Windows.Forms.Button btnTest2;
        private System.Windows.Forms.Button btnGC;
    }
}

