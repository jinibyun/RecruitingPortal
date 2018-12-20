using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.DAL.Interface
{
    public interface IDataRepository
    {
        // stored proc for dashboard
        List<T> GetAppliedCandidate<T>(string fromDate, string toDate);
        List<T> GetJobStatusStatistics<T>(string fromDate, string toDate);
        List<T> GetJobPostStatistics<T>(string fromDate, string toDate);
        List<T> GetRequestGuardStatistics<T>(string aspNetUsersId, string fromDate, string toDate);

        // mail
        List<NotificationQueue> GetNotifications(EnumNotificationType notificationType,EnumNotificationStatus notificationStatus);
        void UpdateNotification(int Id, EnumTypeOfNotificationStatus notificationStatypeTypeId, string exceptionMessage = "");
    }
}
