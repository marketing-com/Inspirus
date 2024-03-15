using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InspirusApp
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void SetButtonColorRunning(object sender)
		{
			(sender as Button).BackColor = Color.Aqua;
			(sender as Button).Update();
		}

		private void ResetButtonColor(object sender)
		{
			(sender as Button).BackColor = SystemColors.Control;
			(sender as Button).UseVisualStyleBackColor = true;
			(sender as Button).Update();
		}

		private void DownloadIncomingFilesBtn_Click(object sender, EventArgs e)
		{
			SetButtonColorRunning(sender);
			MainProcessor mainProcessor = new MainProcessor();
			mainProcessor.DownloadIncomingFiles();
			ResetButtonColor(sender);
		}

		private void ProcessZipFiles(object sender, EventArgs e)
		{
			SetButtonColorRunning(sender);
			MainProcessor mainProcessor = new MainProcessor();
			mainProcessor.ProcessZipFiles();
			ResetButtonColor(sender);
		}

		private void DownloadUnzipIndesignFilesBtn_Click(object sender, EventArgs e)
		{
			SetButtonColorRunning(sender);
			MainProcessor mainProcessor = new MainProcessor();
			mainProcessor.DownloadInDesignFilesAndUnZip();
			ResetButtonColor(sender);
		}

		private void DownloadDataFilesBtn_Click(object sender, EventArgs e)
		{

		}
	}
}
