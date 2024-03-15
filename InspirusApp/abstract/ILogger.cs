using System;

namespace InspirusApp
{
    public interface ILogger
    {
        void EMailHtml(string smtpServerName, string from, string to, string subject, string body);
        string ErrorMessage(string errorMsg);
        string ErrorMessage(string errorMsg, Exception exc);
        string GetLogDir();
        string InfoMessage(string infoMsg);
        void InfoReportAndLog(string infoMsg);
        void ReportAndLogError(string errorMsg);
        void ReportAndLogError(string errorMsg, Exception exc);
        void ReportErrors(string additionalErrorMsg);
        void ReportErrors(string additionalErrorMsg, Exception exc);
        void ReportInfo(string additionalInfoMsg);
        void ReportProduction(string additionalInfoMsg, string subject);
        void ReportProductionWithAttachments(string additionalInfoMsg, string additionalSubject, string[] filesToAttach);
        string URLPrep(string url);
    }
}
