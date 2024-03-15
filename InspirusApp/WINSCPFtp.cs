using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSCP;
using System.Configuration;
using System.IO;

namespace InspirusApp
{
	public class WINSCPFtp
	{
		private string sshHostKeyFingerprint = "";
		private string hostName = "";
		private string user = "";
		private string userPw = "";
		private string uploadPath = "";
		public string remoteDownloadPath = "";
		private string winSCPExeFilePath = "";
		private RuntimeValues runtimeValues = null;
		public Logger logger;

		public WINSCPFtp()
		{
			runtimeValues = new RuntimeValues();
			hostName = ConfigurationManager.AppSettings["sftpColorArtHostName"];
			user = ConfigurationManager.AppSettings["sftpColorArtUserName"];
			userPw = ConfigurationManager.AppSettings["sftpColorArtPassword"];
			winSCPExeFilePath = runtimeValues.GetWinSCPExeFilePath();
			remoteDownloadPath = ConfigurationManager.AppSettings["sftpRemoteDownloadPath"];
			logger = new Logger();
		}

		/// <summary>
		/// Full path to local file or directory to upload. 
		/// Filename in the path can be replaced with Windows wildcard1) to select multiple files. 
		/// When file name is omitted (path ends with backslash), all files and subdirectories in the local directory are uploaded. 
		/// Example of localPath: C:\Data\Source\Princess Cunard CFL\TestBed\TestSend\*
		/// Throws exception on failure
		/// </summary>
		public List<string> UploadFiles(string localPath, string uploadPath)
		{
			// Setup session options
			SessionOptions sessionOptions = new SessionOptions
			{
				Protocol = Protocol.Sftp,
				HostName = hostName,
				UserName = user,
				Password = userPw,
				GiveUpSecurityAndAcceptAnySshHostKey = true
			};

			using (Session session = new Session())
			{
				session.ExecutablePath = winSCPExeFilePath;
				// Connect
				session.Open(sessionOptions);

				// Upload files
				TransferOptions transferOptions = new TransferOptions();
				transferOptions.TransferMode = TransferMode.Automatic;
				transferOptions.PreserveTimestamp = false;
				TransferOperationResult transferResult;
				transferResult = session.PutFiles(localPath, uploadPath, false, transferOptions);
				RemoteDirectoryInfo dirInfo = session.ListDirectory(uploadPath);

				//// Throw on any error
				transferResult.Check();

				List<string> resultList = new List<string>();

				// return results
				foreach (TransferEventArgs transfer in transferResult.Transfers)
				{
					resultList.Add("Upload of " + transfer.FileName + "succeeded.");
				}

				return resultList;
			}

		}

		public void UploadPDFFilesToClientSftp(string localPath, string uploadFullFilename)
		{
			hostName = ConfigurationManager.AppSettings["PdfUploadsftpRemoteHostName"];
			user = ConfigurationManager.AppSettings["PdfUploasftpRemoteUser"];
			userPw = ConfigurationManager.AppSettings["PdfUploasftpRemotePW"];


			// Setup session options
			SessionOptions sessionOptions = new SessionOptions
			{
				Protocol = Protocol.Sftp,
				HostName = hostName,
				UserName = user,
				Password = userPw,
				SshHostKeyPolicy = SshHostKeyPolicy.GiveUpSecurityAndAcceptAny
			};

			using (Session session = new Session())
			{
				session.ExecutablePath = winSCPExeFilePath;
				// Connect
				session.Open(sessionOptions);

				// Upload files
				TransferOptions transferOptions = new TransferOptions();
				transferOptions.TransferMode = TransferMode.Automatic;
				transferOptions.PreserveTimestamp = false;
				TransferOperationResult transferResult;
				transferResult = session.PutFiles(localPath, uploadFullFilename, true, transferOptions);
				RemoteDirectoryInfo dirInfo = session.ListDirectory(uploadFullFilename);

				//// Throw on any error
				transferResult.Check();

				List<string> resultList = new List<string>();

				// return results
				foreach (TransferEventArgs transfer in transferResult.Transfers)
				{
					resultList.Add("Upload of " + transfer.FileName + " succeeded.");
				}

				//return resultList;
			}

		}

		/// <summary>
		///  When file name is omitted (path ends with backslash), all files and subdirectories in the local directory are downloaded. 
		/// </summary>
		public List<string> DownloadFiles(string localPath, bool remove,string hostName,string User,string UserPw)
		{
			// Setup session options
			SessionOptions sessionOptions = new SessionOptions
			{
				Protocol = Protocol.Sftp,
				HostName = hostName,
				UserName = User,
				Password = UserPw,
				GiveUpSecurityAndAcceptAnySshHostKey = true
			};

			using (Session session = new Session())
			{
				session.ExecutablePath = winSCPExeFilePath;
				// Connect
				session.Open(sessionOptions);

				// Upload files
				TransferOptions transferOptions = new TransferOptions();
				transferOptions.TransferMode = TransferMode.Automatic;


				RemoteDirectoryInfo dirInfo = session.ListDirectory(remoteDownloadPath);

				TransferOperationResult transferResult;
				// transferResult = session.GetFiles(remoteDownloadPath + "*.csv", localPath, false);
				transferResult = session.GetFiles(remoteDownloadPath + "*.txt", localPath + "\\*", true);   // gets all files with passed in extension and copies to local path
				transferResult = session.GetFiles(remoteDownloadPath + "*.csv", localPath + "\\*", true);   // gets all files with passed in extension and copies to local path
				transferResult = session.GetFiles(remoteDownloadPath + "*.zip", localPath + "\\*", true);   // gets all files with passed in extension and copies to local path
				transferResult = session.GetFiles(remoteDownloadPath + "*.pdf", localPath + "\\*", true);   // gets all files with passed in extension and copies to local path
				transferResult = session.GetFiles(remoteDownloadPath + "*.xlsx", localPath + "\\*", true);   // gets all files with passed in extension and copies to local path
				transferResult = session.GetFiles(remoteDownloadPath + "*.xls", localPath + "\\*", true);   // gets all files with passed in extension and copies to local path

				//transferResult = session.GetFiles(remoteDownloadPath , localPath, false);


				foreach (RemoteFileInfo filePath in dirInfo.Files)
				{
					FileInfo filePathInfo = new FileInfo(filePath.Name);

					if (filePathInfo.Extension.Equals(".csv") || filePathInfo.Extension.Equals(".txt") || filePathInfo.Extension.Equals(".zip") || filePathInfo.Extension.Equals(".xlsx") || filePathInfo.Extension.Equals(".xls") || filePathInfo.Extension.Equals(".pdf"))
					{
						string sourceFilePath = remoteDownloadPath + filePath.Name;
						string destFilePath = remoteDownloadPath + "/Archive/" + filePath.Name;
						session.MoveFile(sourceFilePath, destFilePath);
					}

				}

				// Throw on any error
				transferResult.Check();

				List<string> resultList = new List<string>();

				foreach (TransferEventArgs transfer in transferResult.Transfers)
				{
					//string sourceFilePath = transfer.FileName;
					//string destFilePath = remoteDownloadPath + "Archive/" + Path.GetFileName(transfer.FileName);
					//session.MoveFile(sourceFilePath, destFilePath);
					logger.InfoMessage("Download of " + transfer.FileName + " succeeded.");
				}
				return resultList;
			}

		}



		public List<string> DownloadReprintFiles(string localPath, string remotePath)
		{
			
			// Setup session options
			SessionOptions sessionOptions = new SessionOptions
			{
				Protocol = Protocol.Sftp,
				HostName = hostName,
				UserName = user,
				Password = userPw,
				GiveUpSecurityAndAcceptAnySshHostKey = true
			};

			using (Session session = new Session())
			{
				session.ExecutablePath = winSCPExeFilePath;
				// Connect
				session.Open(sessionOptions);

				// Upload files
				TransferOptions transferOptions = new TransferOptions();
				transferOptions.TransferMode = TransferMode.Automatic;
				RemoteDirectoryInfo dirInfo = session.ListDirectory(remotePath);
				TransferOperationResult transferResult;
				// transferResult = session.GetFiles(remoteDownloadPath + "*.csv", localPath, false);
				transferResult = session.GetFiles(remotePath + "*.xlsx", localPath + "\\*", true);   // gets all files with passed in extension and copies to local path
				transferResult = session.GetFiles(remotePath + "*.pdf", localPath + "\\*", true);   // gets all files with passed in extension and copies to local path
																											// Throw on any error
				transferResult.Check();
				List<string> resultList = new List<string>();

				return resultList;
			}

		}


		/// <summary>
		/// Full path to local file or directory to upload. 
		/// Filename in the path can be replaced with Windows wildcard1) to select multiple files. 
		/// When file name is omitted (path ends with backslash), all files and subdirectories in the local directory are uploaded. 
		/// Example of localPath: C:\Data\Source\Princess Cunard CFL\TestBed\TestSend\*
		/// Throws exception on failure
		/// </summary>
		public List<string> UploadFileTestCon(string localPath)
		{
			// Setup session options
			SessionOptions sessionOptions = new SessionOptions
			{
				Protocol = Protocol.Sftp,
				HostName = hostName,
				UserName = user,
				Password = userPw,
				SshHostKeyFingerprint = sshHostKeyFingerprint
			};

			using (Session session = new Session())
			{
				session.ExecutablePath = winSCPExeFilePath;
				// Connect
				session.Open(sessionOptions);

				// Upload files
				TransferOptions transferOptions = new TransferOptions();
				transferOptions.TransferMode = TransferMode.Ascii;

				//TransferOperationResult transferResult;
				//transferResult = session.PutFiles(localPath, uploadPath, false, transferOptions);
				RemoteDirectoryInfo dirInfo = session.ListDirectory(uploadPath);

				// Throw on any error
				//transferResult.Check();

				List<string> resultList = new List<string>();

				return resultList;
			}

		}

	}
}
