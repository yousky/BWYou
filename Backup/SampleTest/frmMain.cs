using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SampleTest
{
    public partial class frmMain : Form
    {
        BWYou.Base.ThreadBase thrB;

        Common.Log4net cLog = new Common.Log4net();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            thrB = new BWYou.Base.ThreadBase(Guid.NewGuid().ToString());
            thrB.Start();

            //Common cCommon = new Common("Common");

            //cCommon.ReadFreeSpaceDriveInfo("D");
            //cCommon.MessageSay += new Common.MessageSayEventHandler(lsvMessage.InvokeWriteMessage);
            //cCommon.ReadDicomFilesInFolder(@"C:\xray21\downLoadedImage", "strServerNameWorked", "strZipFilePathNameWorked");    //D:\test\
            //cCommon.MessageSay -= new Common.MessageSayEventHandler(lsvMessage.InvokeWriteMessage);

            //MessageBox.Show("테스트 종료");
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            thrB.StopThread();
            thrB.Dispose();
            thrB = null;
        }

        private void btnGC_Click(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void btnTestTransfer_Click(object sender, EventArgs e)
        {
            if (txtSCP_ID.Text.Length == 0)
            {
                txtSCP_ID.Focus();
                return;
            }

            if (txtSCP_PW.Text.Length == 0)
            {
                txtSCP_PW.Focus();
                return;
            }

            BWYou.Transfer.SCP scp = new BWYou.Transfer.SCP("211.174.182.60", txtSCP_ID.Text, txtSCP_PW.Text, 45874);
            scp.ftEvent += new BWYou.Transfer.SCP.FileTransferEventY(WriteTransferProgress);
            scp.Connect();
            scp.MkdirLocal(@"D:\test\");
            scp.Receive(@"/usr/local/trix_file/dongshin/2011/12/20/671342.zip", @"D:\test\671342.zip");
            scp.MkdirRemote(@"/home/yousky/test/");
            scp.Send(@"D:\test\671342.zip", @"/home/yousky/test/671342.zip");
            scp.MkdirRemote(@"/home/yousky/test/");
            scp.Close();
            scp.ftEvent -= new BWYou.Transfer.SCP.FileTransferEventY(WriteTransferProgress);
        }

        private void btnTestCompress_Click(object sender, EventArgs e)
        {
            List<string> lsDecompressedFilesPathName;
            BWYou.Compress.ZIP czip = new BWYou.Compress.ZIP();
            czip.fcEvent += new BWYou.Compress.ZIP.FileCompressEvent(WriteCompressProgress);
            //czip.Compress(@"D:\test\test\test.zip", @"D:\test.xml");
            czip.DeCompress(@"D:\test\", @"D:\testZip\60_Backup_2011\dongshin\2011\01\15\574513.zip", out lsDecompressedFilesPathName);
            czip.fcEvent -= new BWYou.Compress.ZIP.FileCompressEvent(WriteCompressProgress);
        }

        private void btnTestDB_Click(object sender, EventArgs e)
        {
            clsDatabase.clsMysql cDBMysql = new clsDatabase.clsMysql("127.0.0.1", 3306, "trix", "trix", "#dhrxhvjtm27");
            if (cDBMysql.Open() == true)
            {
                cDBMysql.TEST_CRUD();
                cDBMysql.Close();
            }
        }


        /// <summary>
        /// 실제 작업
        /// </summary>
        /// <param name="Message"></param>
        private void WriteTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            progTransfer.InvokeProgress(transferredBytes * 100 / totalBytes);
        }

        /// <summary>
        /// 실제 작업
        /// </summary>
        /// <param name="Message"></param>
        private void WriteCompressProgress(long workBytes, long totalBytes, string message)
        {
            if (totalBytes == 0)
            {
                progCompress.InvokeProgress(0);
            }
            else
            {
                progCompress.InvokeProgress((int)(workBytes * 100 / totalBytes));
            }
        }

        /// <summary>
        /// 실제 작업
        /// </summary>
        /// <param name="Message"></param>
        private void WriteReadDCMProgress(int workCount, int totalCount)
        {
            progReadDCM.InvokeProgress((int)(workCount * 100 / totalCount));
        }




    }
}
