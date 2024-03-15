using CenveoPGPEncryption;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspirusApp
{
	public class MainProcessor
	{

		private Logger logger = null;

		public MainProcessor()
		{
			logger = new Logger();
		}

		// Nevada
		public void AllProcessingPart1()
		{
			try
			{
				DownloadIncomingFiles();
			}
			catch (Exception exc)
			{
				logger.ReportAndLogError("Error in AllProcessingPart1(). " + exc.Message + exc.StackTrace);
			}
		}

		public void DownloadInDesignFilesAndUnZip()
		{
			try
			{
				WINSCPFtp winSCPFtp = new WINSCPFtp();
				string localPath = ConfigurationManager.AppSettings["localDesignFilesPath"] + "\\*";
				string hostName = ConfigurationManager.AppSettings["sftpRemoteHostName1"];
				string user = ConfigurationManager.AppSettings["sftpRemoteUser1"];
				string userPw = ConfigurationManager.AppSettings["sftpRemotePW1"];

				List<string> results = winSCPFtp.DownloadFiles(localPath, false, hostName, user, userPw);    // do not remove, they are archived on the ftp site

				ExtractIncomingZip extractIncomingZip = new ExtractIncomingZip();
				extractIncomingZip.UnzipFileToFolder();

			}
			catch (Exception exc)
			{
				logger.ReportAndLogError("Failed to download incoming files . " + exc.Message + exc.StackTrace);
			}
		}

		public void DownloadIncomingFiles()
		{
			try
			{
				WINSCPFtp winSCPFtp = new WINSCPFtp();
				string localPath = ConfigurationManager.AppSettings["localIncomingPath"] + "\\*";
				string hostName = ConfigurationManager.AppSettings["sftpRemoteHostName2"];
				string user = ConfigurationManager.AppSettings["sftpRemoteUser2"];
				string userPw = ConfigurationManager.AppSettings["sftpRemotePW2"];
				string pgpSecretKeyFilePath = ConfigurationManager.AppSettings["pgpSecretKeyFilePath"];
				string pgpPassPhrase = ConfigurationManager.AppSettings["pgpPassPhrase"];
				string ArchiveincomingfilePath = ConfigurationManager.AppSettings["localIncomingArchivePath"];

				List<string> results = winSCPFtp.DownloadFiles(localPath, false, hostName, user, userPw);    // do not remove, they are archived on the ftp site
				string[] dataFiles = Directory.GetFiles(localPath);
				PGPEncrypter pgpEncrypter = new PGPEncrypter();
				foreach (string file in dataFiles)
				{
					pgpEncrypter.DecryptFile(file, localPath, pgpSecretKeyFilePath, pgpPassPhrase);
					FileManipulation.FileManip.ArchiveFile(file, ArchiveincomingfilePath, false);
				}
			}
			catch (Exception exc)
			{
				logger.ReportAndLogError("Failed to download incoming files . " + exc.Message + exc.StackTrace);
			}
		}
		
		public void ProcessZipFiles()
		{
			try
			{
				ProcessIncomingFiles processIncomingFiles = new ProcessIncomingFiles();
				processIncomingFiles.SplitIncomingFile();
			}
			catch (Exception exc)
			{
				logger.ReportAndLogError("Failed to ProcessZipFiles. " + exc.Message + exc.StackTrace);
			}
		}


	}
}
