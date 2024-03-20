
namespace InspirusApp
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(165, 89);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(391, 37);
			this.button1.TabIndex = 0;
			this.button1.Text = "Download and Unzip Indesign Files";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.DownloadUnzipIndesignFilesBtn_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(165, 141);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(391, 34);
			this.button2.TabIndex = 1;
			this.button2.Text = "Download Data Files";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.DownloadDataFilesBtn_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(165, 192);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(391, 34);
			this.button3.TabIndex = 2;
			this.button3.Text = "Split Data Files";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.ProcessZipFiles);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(755, 407);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "Inspirus Main Form";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
	}
}

