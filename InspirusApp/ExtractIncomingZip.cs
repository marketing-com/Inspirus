using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspirusApp
{
	public class ExtractIncomingZip
	{
		protected ILogger logger = null;
		public RuntimeValues runtimeValues { set; get; }
		public string extractPath { set; get; }
		public string LocalInDownloadPath { set; get; }

		public string localIncomingArchivePath { set; get; }

		public ExtractIncomingZip()
		{
			LocalInDownloadPath = ConfigurationManager.AppSettings["localDesignFilesPath"];
			localIncomingArchivePath = ConfigurationManager.AppSettings["localDesignFilesArchivePath"];
			logger = new Logger();
			runtimeValues = new RuntimeValues();
			extractPath = runtimeValues.GetValidPath(ConfigurationManager.AppSettings["ZipExtractedIncomingFiles"]);

		}
		public void UnzipFileToFolder()
		{
			try
			{
				string[] FileNames = Directory.GetFiles(LocalInDownloadPath);
				foreach (string file in FileNames)
				{
					logger.InfoMessage("zip file " + file);
					if (Path.GetFileName(file).ToLower().EndsWith(".zip"))
					{
						// We can't use a relative path here. When deployed we need to go to a specific path, usually the sql server computer.
						string extractedZipPath = Path.Combine(extractPath, Path.GetFileNameWithoutExtension(file));
						if (Directory.Exists(extractedZipPath))//if folder already exist delete folder
						{
							logger.InfoMessage("Folder with same name found:" + extractedZipPath);
							Directory.Delete(extractedZipPath, true);
						}
						var path = Directory.CreateDirectory(extractedZipPath);//Create new folder for extract 
						ZipFile.ExtractToDirectory(file, extractedZipPath);//Extract zip file into created folder				
						logger.InfoMessage(" Extracted zip file available at " + extractedZipPath);
						FileManipulation.FileManip.ArchiveFile(file, localIncomingArchivePath,false);
					}
					else
					{
						logger.InfoMessage("There is no file to extract from " + LocalInDownloadPath);
					}

				}
			}
			catch (Exception ex)
			{
				logger.ReportAndLogError("Error in UnzipFileToFolder(). " + ex.Message + ex.StackTrace);
			}
		}

	}
}
