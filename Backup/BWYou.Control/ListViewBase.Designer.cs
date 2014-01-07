namespace BWYou.Control
{
    partial class ListViewBase
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.lsvMessage = new System.Windows.Forms.ListView();
            this.colDateTime = new System.Windows.Forms.ColumnHeader();
            this.colMessage = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lsvMessage
            // 
            this.lsvMessage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDateTime,
            this.colMessage});
            this.lsvMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvMessage.Location = new System.Drawing.Point(0, 0);
            this.lsvMessage.Name = "lsvMessage";
            this.lsvMessage.Size = new System.Drawing.Size(660, 339);
            this.lsvMessage.TabIndex = 78;
            this.lsvMessage.TabStop = false;
            this.lsvMessage.UseCompatibleStateImageBehavior = false;
            this.lsvMessage.View = System.Windows.Forms.View.Details;
            // 
            // colDateTime
            // 
            this.colDateTime.Text = "시간";
            this.colDateTime.Width = 110;
            // 
            // colMessage
            // 
            this.colMessage.Text = "메세지";
            this.colMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colMessage.Width = 520;
            // 
            // ListViewBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lsvMessage);
            this.Name = "ListViewBase";
            this.Size = new System.Drawing.Size(660, 339);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader colDateTime;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ListView lsvMessage;
    }
}
