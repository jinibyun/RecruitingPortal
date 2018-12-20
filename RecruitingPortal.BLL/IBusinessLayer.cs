using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.BLL
{
    public interface IBusinessLayer
    {
        List<T> GetAppliedCandidate<T>(string fromDate, string toDate);
        List<T> GetJobStatusStatistics<T>(string fromDate, string toDate);
        List<T> GetJobPostStatistics<T>(string fromDate, string toDate);
        List<T> GetRequestGuardStatistics<T>(string aspNetUsersId, string fromDate, string toDate);
    }

}
