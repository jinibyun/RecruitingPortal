using RecruitingPortal.Models;
using System.Collections.Generic;
using System.Linq;
// note: it is for SortBy method
using System.Web.UI.WebControls;

namespace RecruitingPortal.Util
{
    public class DataTabelResultSet
    {        
        public List<JobSeekerViewModel> GetJobSeeker(string search, string sortOrder, int start, int length,
                                 List<JobSeekerViewModel> dtResult)
        {
            return FilterResult<JobSeekerViewModel>(search, dtResult).SortBy<JobSeekerViewModel>(sortOrder).Skip(start).Take(length).ToList();
        }
        public List<JobApplyViewModel> GetJobApply(string search, string sortOrder, int start, int length,
                                 List<JobApplyViewModel> dtResult)
        {
            return FilterResult<JobApplyViewModel>(search, dtResult).SortBy<JobApplyViewModel>(sortOrder).Skip(start).Take(length).ToList();
        }

        public List<JobSeekerContactLogViewModel> GetJobSeekerContactLog(string search, string sortOrder, int start, int length,
                                 List<JobSeekerContactLogViewModel> dtResult)
        {
            return FilterResult<JobSeekerContactLogViewModel>(search, dtResult).SortBy<JobSeekerContactLogViewModel>(sortOrder).Skip(start).Take(length).ToList();
        }

        public List<JobAlertViewModel> GetJobAlert(string search, string sortOrder, int start, int length,
                                 List<JobAlertViewModel> dtResult)
        {
            return FilterResult<JobAlertViewModel>(search, dtResult).SortBy<JobAlertViewModel>(sortOrder).Skip(start).Take(length).ToList();
        }

        public List<GuardRequestViewModel> GetGuardRequest(string search, string sortOrder, int start, int length,
                                 List<GuardRequestViewModel> dtResult)
        {
            return FilterResult<GuardRequestViewModel>(search, dtResult).SortBy<GuardRequestViewModel>(sortOrder).Skip(start).Take(length).ToList();
        }

        public List<JobPostingViewModel> GetJobPosting(string search, string sortOrder, int start, int length,
                                 List<JobPostingViewModel> dtResult)
        {
            return FilterResult<JobPostingViewModel>(search, dtResult).SortBy<JobPostingViewModel>(sortOrder).Skip(start).Take(length).ToList();
        }        

        public int Count<T>(string search, List<T> dtResult)
        {
            return FilterResult<T>(search, dtResult).Count();
        }

        private IQueryable<T> FilterResult<T>(string search, List<T> dtResult)
        {
            IQueryable<T> results = dtResult.AsQueryable();            

            if (typeof(JobSeekerViewModel) == typeof(T))
            {
                var query = (IQueryable<JobSeekerViewModel>)results;
                query = query.Where(p => (
                                           search == null ||
                                            (
                                                   p.FirstName != null && p.FirstName.ToLower().Contains(search.ToLower())
                                                   || p.LastName != null && p.LastName.ToLower().Contains(search.ToLower())
                                                   || p.PostalCode != null && p.PostalCode.ToLower().Contains(search.ToLower())
                                                   
                                            )
                                        )
                );

                return (IQueryable<T>)query;
            }
            else if (typeof(JobApplyViewModel) == typeof(T))
            {
                var query = (IQueryable<JobApplyViewModel>)results;
                query = query.Where(p => (
                                           search == null ||
                                            (
                                                   p.JobPosting != null && p.JobPosting.Id.ToString().Contains(search.ToLower())
                                                   || p.JobPosting != null && p.JobPosting.Title.ToLower().Contains(search.ToLower())
                                                   || p.AppliedDate != null && p.AppliedDate.ToString().ToLower().Contains(search.ToLower())
                                                   || p.JobPosting != null && p.JobPosting.CreateDate.ToString().ToLower().Contains(search.ToLower())
                                                   || p.JobPosting != null && p.JobPosting.TypeOfService != null && p.JobPosting.TypeOfService.Name.ToLower().Contains(search.ToLower())
                                                   || p.JobPosting != null && p.JobPosting.city != null && p.JobPosting.city.City1.ToLower().Contains(search.ToLower())
                                                   || p.JobPosting != null && p.JobPosting.IsActive != null && p.JobPosting.IsActive.ToString().ToLower().Contains(search.ToLower())

                                            )
                                        )
                );

                return (IQueryable<T>)query;
            }

            else if (typeof(GuardRequestViewModel) == typeof(T))
            {
                var query = (IQueryable<GuardRequestViewModel>)results;
                query = query.Where(p => (
                                           search == null ||
                                            (
                                                   p.Id.ToString().Contains(search.ToLower())
                                                   || p.ServiceLocation != null && p.ServiceLocation.ToLower().Contains(search.ToLower())
                                                   || p.CreateDate != null && p.CreateDate.ToString().ToLower().Contains(search.ToLower())
                                                   || p.UpdateDate != null && p.UpdateDate.ToString().ToLower().Contains(search.ToLower())
                                                   || p.city != null && p.city.City1.ToLower().Contains(search.ToLower())
                                                   || p.Description != null && p.Description.ToLower().Contains(search.ToLower())
                                             )

                                            )
                                    
                );

                return (IQueryable<T>)query;
            }
            else if (typeof(JobPostingViewModel) == typeof(T))
            {
                var query = (IQueryable<JobPostingViewModel>)results;
                query = query.Where(p => (
                                           search == null ||
                                            (
                                                   p.Id.ToString().Contains(search.ToLower())
                                                   || p.JobId != null && p.JobId.ToLower().Contains(search.ToLower())
                                                   || p.CreateDate != null && p.CreateDate.ToString().ToLower().Contains(search.ToLower())
                                                   || p.Title != null && p.Title.ToLower().Contains(search.ToLower())
                                                   || p.cityName != null && p.cityName.ToLower().Contains(search.ToLower())
                                                   || p.typeOfServiceName != null && p.typeOfServiceName.ToLower().Contains(search.ToLower())
                                             )

                                            )

                );

                return (IQueryable<T>)query;
            }
            else if (typeof(JobSeekerContactLogViewModel) == typeof(T))
            {
                var query = (IQueryable<JobSeekerContactLogViewModel>)results;
                query = query.Where(p => (
                                           search == null ||
                                            (
                                                   p.Id.ToString().Contains(search.ToLower())
                                                   || p.JobId != null && p.JobId.ToLower().Contains(search.ToLower())
                                                   || p.CreateDate != null && p.CreateDate.ToString().ToLower().Contains(search.ToLower())
                                                   || p.ContactLog != null && p.ContactLog.ToLower().Contains(search.ToLower())
                                                   || p.Logger != null && p.Logger.ToLower().Contains(search.ToLower())
                                             )

                                            )

                );

                return (IQueryable<T>)query;
            }
            else if (typeof(JobAlertViewModel) == typeof(T))
            {
                var query = (IQueryable<JobAlertViewModel>)results;
                query = query.Where(p => (
                                           search == null ||
                                            (
                                                   p.Id.ToString().Contains(search.ToLower())
                                                   || p.Keyword != null && p.Keyword.ToLower().Contains(search.ToLower())
                                                   || p.CreateDate != null && p.CreateDate.ToString().ToLower().Contains(search.ToLower())
                                                   
                                             )

                                            )

                );

                return (IQueryable<T>)query;
            }    

            return null;            
        }
    }    
}