using BWYou.Cloud.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SampleTest
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// 스레드 기본 테스트용
        /// </summary>
        BWYou.Base.ThreadBase thrB;

        Common.Log4net cLog = new Common.Log4net();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //frmServer fServer = new frmServer();
            //frmClient fClient = new frmClient();

            //fServer.Show();
            //fClient.Show();


        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string strMessage = "Test Error" + Environment.NewLine + "Test 1" + Environment.NewLine + "Test 2" + Environment.NewLine + "Test 3" + Environment.NewLine + "Test 4" + Environment.NewLine + "Test 5" + Environment.NewLine + "Test 6";

            BWYou.Control.TimerShow ts = new BWYou.Control.TimerShow();
            //ts.Show(this, strMessage);

            RichTextBox rtb = new RichTextBox();
            rtb.Location = btnTest.Location;
            //rtb.Dock = DockStyle.Top;
            //rtb.AutoSize = true;
            rtb.BackColor = SystemColors.Info;
            rtb.BorderStyle = BorderStyle.None;
            rtb.ReadOnly = true;
            //rtb.Enabled = false;
            rtb.Text = strMessage;
            ts.Show(this, rtb, true, 1500);

            //thrB = new BWYou.Base.ThreadBase(Guid.NewGuid().ToString());
            //thrB.Start();

            //Common cCommon = new Common("Common");

            //cCommon.ReadFreeSpaceDriveInfo("D");
            //cCommon.MessageSay += new Common.MessageSayEventHandler(lsvMessage.InvokeWriteMessage);
            //cCommon.ReadDicomFilesInFolder(@"C:\xray21\downLoadedImage", "strServerNameWorked", "strZipFilePathNameWorked");    //D:\test\
            //cCommon.MessageSay -= new Common.MessageSayEventHandler(lsvMessage.InvokeWriteMessage);

            //MessageBox.Show("테스트 종료");
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            //thrB.StopThread();
            //thrB.Dispose();
            //thrB = null;
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

            IStorage storage = new AzureStorage(txtSCP_PW.Text);
            FileInfo fi = new FileInfo(@"X:\temp\400.html");
            using (var fileStream = fi.OpenRead())
            {
                string filenamepng = storage.Upload(fileStream, "400.html", "test", string.Format(@"trsc\{0}\{1}\imgs", 1, 2));
            }


            //BWYou.Transfer.FTP ftp = new BWYou.Transfer.FTP("192.168.0.21", txtSCP_ID.Text, txtSCP_PW.Text, "2121", false);
            //ftp.Upload(".", @"C:\xray21\downLoadedImage\krh201005\13122\2477789\1.dcm");
            //ftp.Download("1.dcm", "1.dcm");

            //BWYou.Transfer.FTP ftp = new BWYou.Transfer.FTP("112.220.125.186", txtSCP_ID.Text, txtSCP_PW.Text", "4322", true);
            //ftp.Download("1.dcm", "%2F/home2/tmp/200807/16/83542/1.2.276.0.357732.201.1.dcm");    //%2F는 역슬래쉬(/)를 의미하는 Web 문자
            //ftp.Download("2.dcm", "//home2/tmp/200807/16/83542/1.2.276.0.357732.201.1.dcm");      // / 한개로 못 찾으면 // 2개 하면 되는 경우 존재함..


            //BWYou.Transfer.SCP scp = new BWYou.Transfer.SCP("211.174.182.60", txtSCP_ID.Text, txtSCP_PW.Text, 45874);
            //scp.ftEvent += new BWYou.Transfer.SCP.FileTransferEventY(WriteTransferProgress);
            //scp.Connect();
            //scp.MkdirLocal(@"D:\test\");
            //scp.Receive(@"/usr/local/trix_file/dongshin/2011/12/20/671342.zip", @"D:\test\671342.zip");
            //scp.MkdirRemote(@"/home/yousky/test/");
            //scp.Send(@"D:\test\671342.zip", @"/home/yousky/test/671342.zip");
            //scp.MkdirRemote(@"/home/yousky/test/");
            //scp.Close();
            //scp.ftEvent -= new BWYou.Transfer.SCP.FileTransferEventY(WriteTransferProgress);
            //BWYou.Transfer.SCP scp = new BWYou.Transfer.SCP("211.110.10.102", txtSCP_ID.Text, txtSCP_PW.Text, 45214);
            //scp.ftEvent += new BWYou.Transfer.SCP.FileTransferEventY(WriteTransferProgress);
            //scp.Connect();
            //scp.MkdirRemote(@"./test");
            //scp.Send(@"D:\test\1.2.840.113619.2.134.1762888262.1769.1375739026.140_6449620469.dcm", @"./test/1.2.840.113619.2.134.1762888262.1769.1375739026.140_6449620469.dcm");
            //scp.MkdirLocal(@"D:\test\");
            //scp.Receive(@"./test/1.2.840.113619.2.134.1762888262.1769.1375739026.140_6449620469.dcm", @"D:\test\1.2.840.113619.2.134.1762888262.1769.1375739026.140_6449620469.dcm.back");
            //scp.Close();
            //scp.ftEvent -= new BWYou.Transfer.SCP.FileTransferEventY(WriteTransferProgress);
        }

        private void btnTestCompress_Click(object sender, EventArgs e)
        {
            List<string> lsDecompressedFilesPathName;
            BWYou.Compress.ZIP czip = new BWYou.Compress.ZIP();
            czip.fcEvent += new BWYou.Compress.ZIP.FileCompressEvent(WriteCompressProgress);
            //czip.Compress(@"C:\TEST\test.zip", @"C:\TEST\dir1");
            czip.DeCompress(@"C:\TEST\Unzip", @"C:\TEST\test.zip", out lsDecompressedFilesPathName);
            //czip.DeCompress(@"D:\test\", @"D:\testZip\60_Backup_2011\dongshin\2011\01\15\574513.zip", out lsDecompressedFilesPathName);
            //czip.DeCompress(@"D:\test\", @"D:\Sync\프로젝트\마이그레이션\!프로그램\Mig_Decomp\236351_2009_04_06_11_40_19_677_error_무한압축해제함.ZIP", out lsDecompressedFilesPathName);
            czip.fcEvent -= new BWYou.Compress.ZIP.FileCompressEvent(WriteCompressProgress);
        }

        private void btnTestDB_Click(object sender, EventArgs e)
        {
            clsDatabase.clsOracle cDBOracle = new clsDatabase.clsOracle("192.168.0.105", 1521, "ORA92", "test", "test");
            //clsDatabase.clsOracle cDBOracle = new clsDatabase.clsOracle("192.168.0.82", 1521, "ORAYOU", "TEST", "TEST");
            if (cDBOracle.Open() == true)
            {
                cDBOracle.TEST_CRUD();
                cDBOracle.Close();
            }

            //clsDatabase.clsMysql cDBMysql = new clsDatabase.clsMysql("127.0.0.1", 3306, "trix", "trix", "#dhrxhvjtm27");
            //if (cDBMysql.Open() == true)
            //{
            //    cDBMysql.TEST_CRUD();
            //    cDBMysql.Close();
            //}
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
