using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using EMailer;
using FileManipulation;
using System.Net.Mail;

namespace InspirusApp
{
    public class Logger : InspirusApp.ILogger
    {
        private string logFilePath = "";
        private RuntimeValues runtimeValues = null;

        public Logger()
        {
            logFilePath = Path.Combine(GetLogDir(), "Log.txt");
            runtimeValues = new RuntimeValues();
        }

        public string GetLogDir()
        {
            string StartUpPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return StartUpPath;
        }

        public string ErrorMessage(string errorMsg)
        {
            StreamWriter sr = null;

            if (File.Exists(logFilePath))
                sr = File.AppendText(logFilePath);
            else
                sr = File.CreateText(logFilePath);

            sr.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "***Error*** " + errorMsg);
            sr.Close();

            return errorMsg;
        }

        public string ErrorMessage(string errorMsg, Exception exc)
        {
            string errMsg = ErrorMessage(errorMsg + ". " + exc.Message + exc.StackTrace);
            return errorMsg;
        }

        public string InfoMessage(string infoMsg)
        {
            StreamWriter sr = null;

            if (File.Exists(logFilePath))
                sr = File.AppendText(logFilePath);
            else
                sr = File.CreateText(logFilePath);

            sr.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + infoMsg);
            sr.Close();

            return infoMsg;
        }

        public void ReportAndLogError(string errorMsg)
        {
            ErrorMessage(errorMsg);
            ReportErrors(errorMsg);
        }

        public void ReportAndLogError(string errorMsg, Exception exc)
        {
            ErrorMessage(errorMsg, exc);
            ReportErrors(errorMsg, exc);
        }

        public void ReportErrors(string additionalErrorMsg)
        {
            string body = "";

            try
            {
                string senderName = new RuntimeValues().GetConfigFileValue("senderName");
                string sendErrorsCCName1 = ConfigurationManager.AppSettings["sendErrorsCCNames"];

                string emailAddress = new RuntimeValues().GetConfigFileValue("sendErrorsEMail");
                string smtpServerName = new RuntimeValues().GetConfigFileValue("smtpServerName");
                string subject = "Inspirus Errors";

                if (runtimeValues.IsSystemBuildPRD())
                    subject = "**PRD Msg** " + subject;
                else if (runtimeValues.IsSystemBuildQA())
                    subject = "**QA Msg** " + subject;

                body = @"Please check the error information in the log.txt in the Inspirus program directory. ** " + additionalErrorMsg;

                if (emailAddress.Equals(""))
                {
                    ErrorMessage("Failed to send error alert. No email address setup to receive alert.");
                    return;
                }
                
                Email email = new Email();

                if (email.Send(smtpServerName, senderName, emailAddress, sendErrorsCCName1, "", subject, body) == false)
                    ErrorMessage("Failed to send error alert. ");
            }
            catch (Exception exc)
            {
                InfoMessage("Info e-mail: " + body);
                ErrorMessage("Failed to send error alert e-mail. " + exc.Message);
            }
        }

        public void InfoReportAndLog(string infoMsg)
        {
            InfoMessage(infoMsg);
            ReportInfo(infoMsg);
        }

        public void ReportErrors(string additionalErrorMsg, Exception exc)
        {
            ReportErrors(additionalErrorMsg + ". " + exc.Message + exc.StackTrace);
        }

        public void ReportInfo(string additionalInfoMsg)
        {
            string body = "";
            try
            {
                string senderName = new RuntimeValues().GetConfigFileValue("senderName");
                string smtpServerName = new RuntimeValues().GetConfigFileValue("smtpServerName");

                string emailAddress = new RuntimeValues().GetConfigFileValue("sendInfoEMail");                
                string sendErrorsCCName1 = ConfigurationManager.AppSettings["sendInfoCCNames"];

                string subject = "Inspirus Information";
                
                if (runtimeValues.IsSystemBuildPRD())
                    subject = "**PRD Msg** " + subject;
                else if (runtimeValues.IsSystemBuildQA())
                    subject = "**QA Msg** " + subject;

                body = @"Information from the Inspirus program. ** " + additionalInfoMsg;

                if (emailAddress.Equals(""))
                {
                    ErrorMessage("Failed to send information alert. No email address setup to receive information.");
                    return;
                }

                Email email = new Email();

                if (email.Send(smtpServerName, senderName, emailAddress, sendErrorsCCName1, "", subject, body) == false)
                    ErrorMessage("Failed to send information alert. ");
            }
            catch (Exception exc)
            {
                InfoMessage("Info e-mail: " + body);
                ErrorMessage("Failed to send error information e-mail. " + exc.Message);
            }
        }

        public void ReportProductionReprint(string additionalInfoMsg)
        {
            string subject = "US Renal Production Reprint Information";

            ReportProduction(additionalInfoMsg, subject);
        }

        public void ReportProduction(string additionalInfoMsg, string subject)
        {
            string body = "";
            try
            {
                string senderName = new RuntimeValues().GetConfigFileValue("senderName");
                string smtpServerName = new RuntimeValues().GetConfigFileValue("smtpServerName");

                string emailAddress = new RuntimeValues().GetConfigFileValue("sendProdEMail");

                if (runtimeValues.IsSystemBuildPRD())
                    subject = "PRD Msg Information from the Inspirus program** " + subject;
                else if (runtimeValues.IsSystemBuildQA())
                    subject = "**QA Msg Information from the Inspirus program** " + subject;

                body =  additionalInfoMsg;

                if (emailAddress.Equals(""))
                {
                    ErrorMessage("Failed to send information alert. No email address setup to receive information.");
                    return;
                }

                InfoMessage("Info e-mail: " + body);

                EMailWithExceptions email = new EMailWithExceptions();

                email.Send(smtpServerName, senderName, emailAddress, "", "", subject, body,true);
                  //  ErrorMessage("Failed to send information alert. ");

            }
            catch (Exception exc)
            {
                ErrorMessage("Failed to send Production Report e-mail. " + exc.Message);
            }
        }

        public void ReportProductionWithAttachments(string additionalInfoMsg, string additionalSubject, string[] filesToAttach)
        {
            string body = "";
            try
            {
                string senderName = new RuntimeValues().GetConfigFileValue("senderName");
                string smtpServerName = new RuntimeValues().GetConfigFileValue("smtpServerName");

                string emailAddress = new RuntimeValues().GetConfigFileValue("sendProdEMail");
                string subject = "Inspirus Production Information";

                if (!string.IsNullOrWhiteSpace(additionalSubject))
                {
                    subject = subject + " - " + additionalSubject;
                }

                if (runtimeValues.IsSystemBuildPRD())
                    subject = "**PRD Msg** " + subject;
                else if (runtimeValues.IsSystemBuildQA())
                    subject = "**QA Msg** " + subject;

                body = @"Information from the Inspirus program. ** " + additionalInfoMsg;

                if (emailAddress.Equals(""))
                {
                    ErrorMessage("Failed to send information alert. No email address setup to receive information.");
                    return;
                }

                InfoMessage("Info e-mail: " + body);

                EMailWithExceptions email = new EMailWithExceptions();

                email.Send(smtpServerName, senderName, emailAddress, "", "", subject, body, filesToAttach, false);
            }
            catch (Exception exc)
            {
                ErrorMessage("Failed to send Production Report with attachments e-mail. " + exc.Message);
            }
        }

        public string URLPrep(string url)
        {
            if (url.Contains(" "))
                url = url.Replace(" ", "%20");

            return url;
        }

        public void EMailHtml(string smtpServerName, string from, string to, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage(from, to, subject, body);
            mailMessage.IsBodyHtml = false;

            SmtpClient smtpClient = new SmtpClient(smtpServerName);
            smtpClient.Send(mailMessage);


        }

        public void VippFileNotification(string additionalInfoMsg)
        {
            string body = "";
            try
            {
                string senderName = new RuntimeValues().GetConfigFileValue("senderName");
                string smtpServerName = new RuntimeValues().GetConfigFileValue("smtpServerName");

                string emailAddress = new RuntimeValues().GetConfigFileValue("sendVippNotificationRecipients");

                string subject = "Inspirus Production Information";

                if (runtimeValues.IsSystemBuildQA())
                    subject = "**Development Msg** " + subject;
                else if (runtimeValues.IsSystemBuildQA())
                    subject = "**QA Msg** " + subject;

                body = @"Vipp File Notification from Inspirus. ** " + additionalInfoMsg;

                if (emailAddress.Equals(""))
                {
                    ErrorMessage("Failed to send Vipp file Notification. No email address setup to receive information.");
                    return;
                }

                InfoMessage("Vipp Notification e-mail: " + body);

                Email email = new Email();

                if (email.Send(smtpServerName, senderName, emailAddress, "", "", subject, body) == false)
                    ErrorMessage("Failed to send Vipp Notification. ");

            }
            catch (Exception exc)
            {
                ErrorMessage("Failed to send Vipp Notification e-mail. " + exc.Message);
            }
        }

        public string UProduceErrorMsg(string logMe, Exception exc)
        {
            return ErrorMessage(logMe, exc);
        }

        public string UProduceErrorMsg(string logMe)
        {
            return ErrorMessage(logMe);
        }

        public string UProduceInfoMsg(string logMe)
        {
            return InfoMessage(logMe);
        }
    }
}
