using Connect.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetPickDataInfor
{
    public partial class frmGetPickDataInfor : Form
    {
        ConnectDB oCon = new ConnectDB();
        int flag = 0;
        public frmGetPickDataInfor()
        {
            InitializeComponent();
        }

        private void frmGetPickDataInfor_Load(object sender, EventArgs e)
        {
            lbResult.Text = "";
            SetDefault();
        }

        public void SetDefault()
        {            
            dtpStart.Value = DateTime.Now;
            dtpStop.Value = DateTime.Now;
            dtpStart.Enabled = true;
            dtpStop.Enabled = true;

            btnExcute.Enabled = true;

            ptbLoad.Visible = false;            
        }

        private void bgwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            SqlCommand sql = new SqlCommand();

            try
            {
                string tmpdtpStart = dtpStart.Text.Trim();
                string tmpdtpStop = dtpStop.Text.Trim();

                sql.CommandText = "EXEC GetPickDataInforByDate '" + tmpdtpStart.Replace("/", "") + "', '" + tmpdtpStop.Replace("/", "") + "'";
                sql.CommandTimeout = 180;
                oCon.ExecuteCommand(sql);

                flag = 1;
            }
            catch (Exception ex)
            {
                playNG();
                lbResult.ForeColor = Color.Red;
                lbResult.Text = "Result: " + ex.ToString().Trim();
                flag = 0;
            }
        }

        private void bgwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetDefault();
            if (flag == 1)
            {
                playOK();
                lbResult.ForeColor = Color.Green;
                lbResult.Text = "Result: Email sent. Please check your email again.";
            }
        }

        private void btnExcute_Click(object sender, EventArgs e)
        {           
            if (!bgwLoad.IsBusy)
            {
                lbResult.Text = "";
                setDisable();
                bgwLoad.RunWorkerAsync();
            }
        }

        public void setDisable()
        {
            ptbLoad.Visible = true;

            btnExcute.Enabled = false;

            dtpStart.Enabled = false;
            dtpStop.Enabled = false;
        }

        public void playOK()
        {
            string exePath = Application.StartupPath + "\\OK.wav";
            SoundPlayer simpleSound = new SoundPlayer(exePath);
            simpleSound.Play();
            return;
        }

        public void playNG()
        {
            string exePath = Application.StartupPath + "\\OO.wav";
            SoundPlayer simpleSound = new SoundPlayer(exePath);
            simpleSound.Play();
            return;
        }

        private void frmGetPickDataInfor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close the program ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.DoEvents();
            }
            else
            {
                e.Cancel = true;
                SetDefault();
                lbResult.Text = "";
            }
        }
    }
}
