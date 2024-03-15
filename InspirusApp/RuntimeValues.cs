using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Collections.Generic;


namespace InspirusApp
{
   public class RuntimeValues
    {
        //private static SystemVersions _systemVersion = SystemVersions.Invalid;

        /// <summary>
        /// The encryption key.
        /// </summary>
        public string EncryptionKey
        {
            get
            {
                // Don't store this as a constant or a literal value because it's readable in the executable
                // The encryption key is: f3132b2f37274439ba29579cf623923c
                return "" + (char)102 + (char)51 + (char)49 + (char)51 + (char)50 + (char)98 + (char)50 + (char)102 + (char)51 + (char)55 + (char)50 + (char)55 + (char)52 + (char)52 + (char)51 + (char)57 + (char)98 + (char)97 + (char)50 + (char)57 + (char)53 + (char)55 + (char)57 + (char)99 + (char)102 + (char)54 + (char)50 + (char)51 + (char)57 + (char)50 + (char)51 + (char)99;
            }
        }

        /// <summary>
        /// Returns the config file value for the specified key.
        /// </summary>
        /// <param name="key">The config file key.</param>
        /// <returns>The value for the specified key.</returns>
        public string GetConfigFileValue(string key)
        {
            string configFileValue = getConfigFileValue(key, false);
            return configFileValue;
        }

        /// <summary>
        /// Returns the config file value for the specified key.
        /// </summary>
        /// <param name="key">The config file key.</param>
        /// <param name="encrypted">True if the value is encrypted or false if unencrypted.</param>
        /// <returns>The value for the specified key.</returns>
        public string GetConfigFileValue(string key, bool encrypted)
        {
            string configFileValue = getConfigFileValue(key, encrypted);
            return configFileValue;
        }

        /// <summary>
        /// Returns the config file value for the specified key.
        /// </summary>
        /// <param name="key">The config file key.</param>
        /// <param name="encrypted">True if the value is encrypted or false if unencrypted.</param>
        /// <returns>The value for the specified key.</returns>
        private string getConfigFileValue(string key, bool encrypted)
        {
            string configFileValue = "";
            try
            {
                configFileValue = ConfigurationManager.AppSettings[key];
                if (configFileValue == null)
                    configFileValue = "";

                // If the value is encrypted
                if (encrypted)
                {
                    // Decrypt the value
                 //   Cenveo.Utilities.Encryption.CenveoEncryption encryption = new Cenveo.Utilities.Encryption.CenveoEncryption(EncryptionKey);
                //    configFileValue = encryption.DecryptData(configFileValue);
                }

            }
            catch { }

            return configFileValue;
        }


        public string GetLocalStartupPath()
        {
            string StartUpPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return StartUpPath;
        }
        
        public string GetProgramDataPath()
        {
            string programData = GetLocalStartupPath();

            if (!Directory.Exists(programData))
                Directory.CreateDirectory(programData);

            return programData;
        }

        public bool IsSystemBuildPRD()
        {
            string systemBuild = ConfigurationManager.AppSettings["SystemBuild"];

            if (systemBuild.Equals("PRD"))
                return true;
            else
                return false;
        }

        public bool IsSystemBuildQA()
        {
            string systemBuild = ConfigurationManager.AppSettings["SystemBuild"];

            if (systemBuild.Equals("QA"))
                return true;
            else
                return false;
        }

        #region Generic
       /// <summary>
       /// Creates a folder at the specified location
       /// </summary>
        public string GetValidPath(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

       /// <summary>
       /// Send in a config value to a folder. Creates and returns the folder location
       /// </summary>
        public string GetValidConfigPath(string folder)
        {
            string configPath = ConfigurationManager.AppSettings[folder];

            return GetValidPath(configPath);
        }

        public string GetLocalIncomingPath()
        {
            return GetValidConfigPath("localIncomingPath");
        }

        public string GetRelativeArchivePath(string parentPath)
        {
            FileInfo parentPathInfo = new FileInfo(parentPath);
            string archivePath = parentPath;

            if (!parentPathInfo.Extension.Equals(""))
            {
                archivePath = parentPathInfo.DirectoryName;
            }

            archivePath = Path.Combine(archivePath, "Archive");

            return GetValidPath(archivePath);
        }
        #endregion
    
  
        public string GetRemoteIncomingFilesPath()
        {
            string incomingFilesPath = ConfigurationManager.AppSettings["remoteIncomingPath"];

            return incomingFilesPath;
        }

        #region MailManager Settings

 
        #endregion

        #region Print and Proof Settings
         
        #endregion

       public Encoding GetIncomingFileEncoding()
        {
            string encodingName = ConfigurationManager.AppSettings["incomingFileEncoding"];
            return Encoding.GetEncoding(encodingName);
        }
    
        virtual public string GetWinSCPExeFilePath()
        {
            return Path.Combine(GetProgramDataPath(), "WinSCP.exe");
        }

       

        public string GetVippPrintVersion()
        {
            string version = ConfigurationManager.AppSettings["vippPrintVersion"];

            return version;
        }


        public string GetVippPrintFilePathName(int importFileBatchId, int mailBatch, int printBatch, int lineCt, string uniqueId)
        {
            string vippPath = GetVippPrintBasePathName(mailBatch, importFileBatchId, printBatch);

            vippPath = vippPath + "_" + lineCt + "_" + uniqueId + ".txt";

            return vippPath;
        }

        protected string GetVippPrintBasePathName(int  mailbatch, int batchid,int printBatch)
        {
            string vippPath = GetVippPrintWorkingBaseFolderName(batchid, mailbatch);

            if (!Directory.Exists(vippPath))
            {
                Directory.CreateDirectory(vippPath);
            }

            vippPath = Path.Combine(vippPath, "Inspirus_" + batchid + "_MBat" + mailbatch + "_PBat" + printBatch);

            return vippPath;
        }

        public string GetVIPPPrintingPath()
        {
            string vippPath = ConfigurationManager.AppSettings["vippPrintingPath"];

            //  Can't check this until we authenticate
            if (!Directory.Exists(vippPath))
            {
                Directory.CreateDirectory(vippPath);
            }

            return vippPath;
        }

         public string GetVIPPWorkingPath()
        {
            string vippWorkingPath = ConfigurationManager.AppSettings["vippWorkingPath"];

            if (!Directory.Exists(vippWorkingPath))
            {
                Directory.CreateDirectory(vippWorkingPath);
            }

            return vippWorkingPath;
        }

        // PGP 
        public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        public string DecodeBase64(string encodedString)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedString);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }

        public string GetSecretKeyringFilePath()
        {
            string secretKeyRingFilePath = GetConfigFileValue("secretKeyRingFilePath", true);
            return secretKeyRingFilePath;
        }

        public string GetPublicKeyringFilePath()
        {
            string publicKeyRingFilePath = GetConfigFileValue("publicKeyRingFilePath", true);
            return publicKeyRingFilePath;
        }

        public string GetPassPhrase()
        {
            string passPhrase = GetConfigFileValue("passPhrase", true);
            return passPhrase;
        }

       
        public string GetVippPrintWorkingBaseFolderName(int importFileBatchId, int mailBatchId)
        {
            string vippPath = GetVippWorkingPath();
            vippPath = Path.Combine(vippPath, GetVippPrintBaseSubFolderName(importFileBatchId, mailBatchId));
            return vippPath;
        }

        public string GetVippPrintBaseFolderName(int importFileBatchId, int mailBatchId)
        {
            string vippPath = GetVippPath();
            vippPath = Path.Combine(vippPath, GetVippPrintBaseSubFolderName(importFileBatchId, mailBatchId));
            return vippPath;
        }

         protected string GetVippPrintBaseSubFolderName(int importFileBatchId, int mailBatchId)
        {
            string vippPath = "Batch_" + importFileBatchId + "_MBAT_" + mailBatchId;

            return vippPath;
        }

        protected string GetVippWorkingPath()
        {
            string vippPath = ConfigurationManager.AppSettings["vippWorkingPath"];

            if (!Directory.Exists(vippPath))
            {
                Directory.CreateDirectory(vippPath);
            }

            return vippPath;
        }

        protected string GetVippPath()
        {
            string vippPath = ConfigurationManager.AppSettings["vippPrintingPath"];

            if (!Directory.Exists(vippPath))
            {
                Directory.CreateDirectory(vippPath);
            }

            return vippPath;
        }

        public string GetMailedReportPath()
        {
            return GetValidConfigPath("mailedReportPath");
        }

       

        public string GetMailDatDestFileName(int mailingBatchFK)
        {
            return "CenOR_MBAT_" + mailingBatchFK + ".zip";
        }

        public string GetJobId(int printBatchId)
        {
            string jifJobIdPrefix = ConfigurationManager.AppSettings["jifJobIdPrefix"];

            return GetJobId(jifJobIdPrefix, printBatchId);
        }

        public string GetJobId(string jifJobIdPrefix, int printBatchId)
        {
            string paddedPrintBatch = printBatchId.ToString().PadLeft(6, '0');
            string jobId = jifJobIdPrefix + paddedPrintBatch;

            return jobId;
        }

      
    }
}
