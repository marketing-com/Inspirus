using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using CenveoPGPEncryption;

namespace InspirusApp
{
    public class PGPEncryptWrapper
    {
        private string pgpPublicKeyFilePath = "";
        private Logger logger = null;
        
        public PGPEncryptWrapper()
        {
            logger = new Logger();
            pgpPublicKeyFilePath = ConfigurationManager.AppSettings["pgpPublicKeyFilePath"];
        }

  

        public bool EncryptFileAsPGPExtAdded(string decryptedFilePath)
        {
            bool newFileCreated = false;

            if (!File.Exists(decryptedFilePath))
            {
                logger.ErrorMessage("File " + decryptedFilePath + " does not exist to encrypt.");
                return newFileCreated;
            }

            FileInfo decryptedFileInfo = new FileInfo(decryptedFilePath);

            if (decryptedFileInfo.Extension.ToUpper().Equals(".PGP"))
                return newFileCreated;

            string encryptedFileName = decryptedFileInfo.Name.Replace(decryptedFileInfo.Extension, "pgp");
            string newFilePath = decryptedFilePath + ".pgp";

            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

         
            newFileCreated = true;
            
            return newFileCreated;           
        }

      

      
       
     
    }
}
