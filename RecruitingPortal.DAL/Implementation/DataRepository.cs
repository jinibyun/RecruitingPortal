using RecruitingPortal.DAL.Interface;
using RecruitingPortal.DAL.Model;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.DAL.Implementation
{
    public class DataRepository : IDataRepository
    {
        private RecruitingPortalEntities context;
        public DataRepository()
        {
            context = (new DbContextFactory()).GetContext() as RecruitingPortalEntities;
        }

        public List<T> GetAppliedCandidate<T>(string fromDate, string toDate)
        {
            // http://stackoverflow.com/questions/29012920/entity-framework-return-list-from-stored-procedure
            return context.Database.SqlQuery<T>(string.Format("exec usp_getAppliedCandidate '{0}', '{1}'", fromDate, toDate)).ToList<T>();
        }

        public List<T> GetJobStatusStatistics<T>(string fromDate, string toDate)
        {
            // http://stackoverflow.com/questions/29012920/entity-framework-return-list-from-stored-procedure
            return context.Database.SqlQuery<T>(string.Format("exec usp_getJobStatusStatistics '{0}', '{1}'", fromDate, toDate)).ToList<T>();
        }

        public List<T> GetJobPostStatistics<T>(string fromDate, string toDate)
        {
            // http://stackoverflow.com/questions/29012920/entity-framework-return-list-from-stored-procedure
            return context.Database.SqlQuery<T>(string.Format("exec usp_getJobPostStatistics '{0}', '{1}'", fromDate, toDate)).ToList<T>();
        }

        public List<T> GetRequestGuardStatistics<T>(string aspNetUsersId, string fromDate, string toDate)
        {
            return context.Database.SqlQuery<T>(string.Format("exec usp_getRequestGuardStatistics '{0}', '{1}', '{2}'", aspNetUsersId, fromDate, toDate)).ToList<T>();
        }

        public List<NotificationQueue> GetNotifications(EnumNotificationType notificationType, EnumNotificationStatus notificationStatus)
        {
            if (notificationType == EnumNotificationType.All)
            {
                if(notificationStatus == EnumNotificationStatus.All)
                {
                    return context.NotificationQueues.ToList<NotificationQueue>();
                }
                return context.NotificationQueues.Where(x => x.NotificationStatusTypeId == (int)notificationStatus).ToList<NotificationQueue>();
            }
            else
            {
                if (notificationStatus == EnumNotificationStatus.All)
                {
                    return context.NotificationQueues.Where(x => x.NotificationTypeId == (int)notificationType).ToList<NotificationQueue>();
                }
                return context.NotificationQueues.Where(x => x.NotificationTypeId == (int)notificationType && x.NotificationStatusTypeId == (int)notificationStatus).ToList<NotificationQueue>();
            }          
        }

        public void UpdateNotification(int Id, EnumTypeOfNotificationStatus notificationStatypeTypeId, string exceptionMessage = "")
        {
            var notificationQueue = context.NotificationQueues.Where(x => x.Id == Id).FirstOrDefault<NotificationQueue>();

            switch(notificationStatypeTypeId)
            {
                case EnumTypeOfNotificationStatus.Sent:
                    notificationQueue.NotificationStatusTypeId = (int)notificationStatypeTypeId;
                    notificationQueue.SentDate = DateTime.Now;
                    break;
                case EnumTypeOfNotificationStatus.NotSent: // default
                    break;
                case EnumTypeOfNotificationStatus.Error:
                    notificationQueue.NotificationStatusTypeId = (int)notificationStatypeTypeId;
                    break;
            }
            
            notificationQueue.UpdateDate = DateTime.Now;

            // exception
            if(!string.IsNullOrEmpty(exceptionMessage))
            {
                notificationQueue.ExceptionDate = DateTime.Now;
                notificationQueue.ExceptionMessage = exceptionMessage;
            }


            context.SaveChanges();
        }
    }
}
