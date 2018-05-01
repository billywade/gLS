using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace Module13BillyWade
{
    public partial class Form1 : Form
    {
        string dir = "";
        string liststring = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = "";
            dir = "";
            outputTextBox.Text = "";
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            DialogResult result = folderDlg.ShowDialog();
            pathTextBox.Text = folderDlg.SelectedPath;
            dir = folderDlg.SelectedPath;
            Environment.SpecialFolder root = folderDlg.RootFolder;

        }

        private void listButton_Click(object sender, EventArgs e)
        {
            outputTextBox.AppendText("starting...." + Environment.NewLine);
            liststring = "";
            backgroundWorker1.RunWorkerAsync();
            while (this.backgroundWorker1.IsBusy)
            {
                Application.DoEvents();
            }
        }

        private void List(string dir, DoWorkEventArgs e)
        {
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                    liststring += (f + Environment.NewLine);
                if (backgroundWorker1.CancellationPending == true)
                {
                    e.Cancel = true;
                    return; // abort work, if it's cancelled
                }
                foreach (string g in Directory.GetDirectories(dir))
                {
                    liststring += (g + Environment.NewLine);
                    List(g, e);
                }
            }
            catch (System.Exception ex)
            {
                liststring += ex.Message;
            } 
        }
 
        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                outputTextBox.AppendText("cancelled.");
                    backgroundWorker1.CancelAsync();
            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List(dir, e);
            BeginInvoke((MethodInvoker)delegate
            {
                outputTextBox.Text = liststring; 
            });
        }
    }
}

