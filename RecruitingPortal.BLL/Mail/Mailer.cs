using RecruitingPortal.DAL.Interface;
using RecruitingPortal.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace RecruitingPortal.BLL.Mail
{
    public class Mailer
    {
        static BaseService<NotificationQueue> _notificationQueue;


        public Mailer(BaseService<NotificationQueue> repository)
        {
            _notificationQueue = repository;

        }
        
        //public static bool AddNotificationQueue(string subject, string body, string emailTo, int memberId
        //                                                , int notificationTypeId, int notificationStatusTypeId, int notificationId)
        //{
        //    bool IsAdded = false;
        //    try
        //    {
        //        var notificationQueue = new NotificationQueue
        //        {
        //            Subject = subject,
        //            Body = body,
        //            CreateDate = DateTime.Now,
        //            UpdateDate = DateTime.Now,
        //            EmailFrom = ConfigurationManager.AppSettings["SmtpSenderEmail"].ToString(),
        //            EmailTo = emailTo,
        //            EmailCC = "",
        //            NumberOfAttempt = 0,
        //            IsBodyHTML = true,
        //            AttachmentFile = "",
        //            MemberId = memberId,
        //            NotificationTypeId = notificationTypeId,    //(int)NotificationType.Register,
        //            NotificationStatusTypeId = notificationStatusTypeId, //(int)NotificationStatusType.NotSent,
        //            NotificationId = notificationId    //(int)NotificationType.Register
        //            ,
        //        };

        //        _notificationQueue.Add(notificationQueue);
        //        IsAdded = true;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    return IsAdded;
        //}

        public IEnumerable<NotificationQueue> GetNotificationQueue()
        {
            IEnumerable<NotificationQueue> list = null;
            list = _notificationQueue.Where(x => x.NotificationStatusTypeId == 1);
            //list = _notificationQueue.GetAll();

            return list;
        }

        public static bool UpdateNotificationStatus(int notificationId, NotificationStatusType notificationStatusType)
        {
            //  Repository<NotificationQueue> _nq ;


            bool IsUpdated = false;
            try
            {
                var nq = _notificationQueue.Where(x => x.Id == notificationId).FirstOrDefault<NotificationQueue>();


                if (nq != null)
                {
                    nq.NotificationStatusTypeId = (int)notificationStatusType;
                    nq.UpdateDate = DateTime.Now;
                    nq.SentDate = DateTime.Now;

                    // update
                    _notificationQueue.Change(nq);
                    IsUpdated = true;

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsUpdated;
        }


        public static bool IsValidEmail(string inputEmail)
        //public bool IsValidEmail(string inputEmail)
        {
            // ref: http://stackoverflow.com/questions/16167983/best-regular-expression-for-email-validation-in-c-sharp
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return true;
            else
                return false;
        }

        public static bool SendEmail(string Subject, string Body, string AttachmentFileName
                                                , string FromEmail, string[] ToEmail
                                                , string[] CcEmail, bool IsBodyHtml, ref string errorMessage)
        {
            bool IsSent = false;

            MailMessage msgMail = new MailMessage();
            MailAddress mailAddress = new MailAddress(FromEmail, ConfigurationManager.AppSettings["SiteName"]);
            if (!string.IsNullOrEmpty(AttachmentFileName))
            {
                Attachment attachmentFile = new Attachment(AttachmentFileName);
                msgMail.Attachments.Add(attachmentFile);
            }
            if (ToEmail != null)
            {
                for (int i = 0; i < ToEmail.Length; i++)
                {
                    msgMail.To.Add(ToEmail[i].ToString());
                }
            }

            msgMail.From = mailAddress;
            msgMail.IsBodyHtml = IsBodyHtml;
            msgMail.Subject = Subject;
            msgMail.Body = Body;

            string SmtpHost = ConfigurationManager.AppSettings["SmtpHost"].ToString();
            int SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());
            string SmtpUserName = ConfigurationManager.AppSettings["SmtpUserName"].ToString();
            string SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"].ToString();

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
