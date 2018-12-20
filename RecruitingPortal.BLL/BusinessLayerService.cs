using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.DAL.Interface;
using RecruitingPortal.DAL.Implementation;
using System.Linq.Expressions;
using System.Data.SqlTypes;
namespace RecruitingPortal.BLL
{
    public class BusinessLayerService : IBusinessLayer
    {
        // ref: https://stackoverflow.com/questions/15157328/datetime-minvalue-and-sqldatetime-overflow
        private string sqlMinDate = SqlDateTime.MinValue.ToString();
        private string sqlMaxDate = SqlDateTime.MaxValue.ToString();

        private IDataRepository repo { get; set; }
        public BusinessLayerService()
        {
            repo = new DataRepository();
        }

        public List<T> GetJobStatusStatistics<T>(string fromDate, string toDate)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate))
                {
                    fromDate = sqlMinDate;
                }
                if (string.IsNullOrEmpty(toDate))
                {
                    toDate = sqlMaxDate;
                }
                return repo.GetJobStatusStatistics<T>(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public List<T> GetAppliedCandidate<T>(string fromDate, string toDate)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate))
                {
                    fromDate = sqlMinDate;
                }
                if (string.IsNullOrEmpty(toDate))
                {
                    toDate = sqlMaxDate;
                }
                return repo.GetAppliedCandidate<T>(fromDate, toDate);
            }
            catch (Exception ex)
            {

                throw ex;
            }            
        }

        public List<T> GetJobPostStatistics<T>(string fromDate, string toDate)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate))
                {
                    fromDate = sqlMinDate;
                }
                if (string.IsNullOrEmpty(toDate))
                {
                    toDate = sqlMaxDate;
                }
                return repo.GetJobPostStatistics<T>(fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public List<T> GetRequestGuardStatistics<T>(string aspNetUsersId, string fromDate, string toDate)
        {
            try
            {
                if (string.IsNullOrEmpty(fromDate))
                {
                    fromDate = sqlMinDate;
                }
                if (string.IsNullOrEmpty(toDate))
                {
                    toDate = sqlMaxDate;
                }
                return repo.GetRequestGuardStatistics<T>(aspNetUsersId, fromDate, toDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
