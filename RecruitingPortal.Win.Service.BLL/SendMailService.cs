using RecruitingPortal.DAL.Interface;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.Win.Service.BLL
{
    public class SendMailService: ISendMailService
    {
        private readonly string SiteName = ConfigurationManager.AppSettings["SiteName"];
        private readonly string SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
        private readonly int SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());
        private readonly string SmtpUserName = ConfigurationManager.AppSettings["SmtpUserName"];
        private readonly string SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

        protected IDataRepository _repo;
        public SendMailService(IDataRepository repo)
        {            
            _repo = repo;
        }
        public void SendMail()
        {
            
            var notSentNotifications = _repo.GetNotifications(EnumNotificationType.All, EnumNotificationStatus.NotSent);
            if(notSentNotifications != null && notSentNotifications.Count > 0)
            {
                foreach(var member in notSentNotifications)
                {
                    try
                    {
                        var title = GetTitle(member.NotificationTypeId);
                        string errorMessage = "";

                        // NOTE: EmailTo and EmailCC are single string or semi colon separated value
                        var result = SendEmail(title, member.Body, member.AttachmentFile, member.EmailFrom,
                            member.EmailTo, member.EmailCC, true, ref errorMessage);
                        if (result)
                        {
                            _repo.UpdateNotification(member.Id, EnumTypeOfNotificationStatus.Sent);
                        }
                        else
                        {
                            _repo.UpdateNotification(member.Id, EnumTypeOfNotificationStatus.NotSent);
                        }
                    }
                    catch (Exception ex)
                    {
                        _repo.UpdateNotification(member.Id, EnumTypeOfNotificationStatus.Error, ex.Message);
                    }
                }
            }
        }

        private string GetTitle(int notificationStatusTypeId)
        {
            EnumNotificationType notificationStatusType = (EnumNotificationType)notificationStatusTypeId;
            var title = "Recruiting Portal - ";
            switch(notificationStatusType)
            {
                case EnumNotificationType.JobAlert:
                    title += "Job Alert";
                    break;
                case EnumNotificationType.JobApplied:
                    title += "Job Applied";
                    break;
                case EnumNotificationType.JobDeleted:
                    title += "Job Deleted";
                    break;
                case EnumNotificationType.JobExpired:
                    title += "Job Expired";
                    break;
                case EnumNotificationType.JobPosted:
                    title += "Job Posted";
                    break;
                case EnumNotificationType.JobRequested:
                    title += "Job Requested by service team";
                    break;
                case EnumNotificationType.SeekerHired:
                    title += "Job Applicant Hired Confirmation";
                    break;
            }

            return title;
        }

        private bool SendEmail(string Subject, string Body, string AttachmentFileName
                                                , string FromEmail, string ToEmail
                                                , string CcEmail, bool IsBodyHtml, ref string errorMessage)
        {
            bool IsSent = false;
            MailMessage msgMail = new MailMessage();
            MailAddress mailAddress = new MailAddress(FromEmail, SiteName);
            if (!string.IsNullOrEmpty(AttachmentFileName))
            {
                Attachment attachmentFile = new Attachment(AttachmentFileName);
                msgMail.Attachments.Add(attachmentFile);
            }

            if (!string.IsNullOrEmpty(ToEmail))
            {
                var toEmails = ToEmail.Split(new char[] { ';' });
                for (int i = 0; i < toEmails.Length; i++)
                {
                    msgMail.To.Add(toEmails[i].ToString());
                }
            }

            if (!string.IsNullOrEmpty(CcEmail))
            {
                var ccEmails = CcEmail.Split(new char[] { ';' });
                for (int i = 0; i < ccEmails.Length; i++)
                {
                    msgMail.CC.Add(ccEmails[i].ToString());
                }
            }

            msgMail.From = mailAddress;
            msgMail.IsBodyHtml = IsBodyHtml;
            msgMail.Subject = Subject;
            msgMail.Body = Body;            

            using (SmtpClient client = new SmtpClient(SmtpHost, SmtpPort))
            {
                client.Credentials = new NetworkCredential(SmtpUserName, SmtpPassword);

                // Use SSL when accessing Amazon SES. The SMTP session will begin on an unencrypted connection, and then
                // the client will issue a STARTTLS command to upgrade to an encrypted connection using SSL.
                client.EnableSsl = true;

                try
                {
                    client.Send(msgMail);
                    IsSent = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return IsSent;
            }
        }
    }
}
