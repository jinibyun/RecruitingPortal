﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RecruitingPortal.DAL.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using RecruitingPortal.Domain;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class RecruitingPortalEntities : DbContext
    {
        public RecruitingPortalEntities()
            : base("name=RecruitingPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<BranchAddress> BranchAddresses { get; set; }
        public virtual DbSet<city> cities { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyFile> CompanyFiles { get; set; }
        public virtual DbSet<CompanyLang> CompanyLangs { get; set; }
        public virtual DbSet<CompanyTypeOfWork> CompanyTypeOfWorks { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<GuardRequest> GuardRequests { get; set; }
        public virtual DbSet<GuardRequestTypeOfWork> GuardRequestTypeOfWorks { get; set; }
        public virtual DbSet<JobAlert> JobAlerts { get; set; }
        public virtual DbSet<JobAlertFrequency> JobAlertFrequencies { get; set; }
        public virtual DbSet<JobApply> JobApplies { get; set; }
        public virtual DbSet<JobApplyStatu> JobApplyStatus { get; set; }
        public virtual DbSet<JobPosting> JobPostings { get; set; }
        public virtual DbSet<JobPostingFile> JobPostingFiles { get; set; }
        public virtual DbSet<JobPostingTypeOfWork> JobPostingTypeOfWorks { get; set; }
        public virtual DbSet<JobSeeker> JobSeekers { get; set; }
        public virtual DbSet<JobSeekerContactLog> JobSeekerContactLogs { get; set; }
        public virtual DbSet<JobSeekerFile> JobSeekerFiles { get; set; }
        public virtual DbSet<JobSeekerLang> JobSeekerLangs { get; set; }
        public virtual DbSet<JobSeekerScore> JobSeekerScores { get; set; }
        public virtual DbSet<JobSeekerSecurityExperience> JobSeekerSecurityExperiences { get; set; }
        public virtual DbSet<JobSeekerStatu> JobSeekerStatus { get; set; }
        public virtual DbSet<JobSeekerTypeOfWork> JobSeekerTypeOfWorks { get; set; }
        public virtual DbSet<JobSeekerTypeOfWorkAvailability> JobSeekerTypeOfWorkAvailabilities { get; set; }
        public virtual DbSet<Lang> Langs { get; set; }
        public virtual DbSet<NotificationQueue> NotificationQueues { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<SiteConfig> SiteConfigs { get; set; }
        public virtual DbSet<StaffTeam> StaffTeams { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TypeOfJobNotice> TypeOfJobNotices { get; set; }
        public virtual DbSet<TypeOfNotificationStatu> TypeOfNotificationStatus { get; set; }
        public virtual DbSet<TypeOfPhone> TypeOfPhones { get; set; }
        public virtual DbSet<TypeOfPosition> TypeOfPositions { get; set; }
        public virtual DbSet<TypeOfSecurityExperience> TypeOfSecurityExperiences { get; set; }
        public virtual DbSet<TypeOfService> TypeOfServices { get; set; }
        public virtual DbSet<TypeOfWork> TypeOfWorks { get; set; }
        public virtual DbSet<TypeOfWorkAvailability> TypeOfWorkAvailabilities { get; set; }
        public virtual DbSet<AspNetUsersStaffTeam> AspNetUsersStaffTeams { get; set; }
        public virtual DbSet<JobHire> JobHires { get; set; }
    
        public virtual int usp_getJobStatusStatistics()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_getJobStatusStatistics");
        }
    
        public virtual ObjectResult<usp_getAppliedCandidate_Result> usp_getAppliedCandidate(string fromDate, string toDate)
        {
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("toDate", toDate) :
                new ObjectParameter("toDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_getAppliedCandidate_Result>("usp_getAppliedCandidate", fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<usp_getJobPostStatistics_Result> usp_getJobPostStatistics(string fromDate, string toDate)
        {
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("toDate", toDate) :
                new ObjectParameter("toDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_getJobPostStatistics_Result>("usp_getJobPostStatistics", fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<usp_getRequestGuardStatistics_Result> usp_getRequestGuardStatistics(string aspNetUsersId, string fromDate, string toDate)
        {
            var aspNetUsersIdParameter = aspNetUsersId != null ?
                new ObjectParameter("aspNetUsersId", aspNetUsersId) :
                new ObjectParameter("aspNetUsersId", typeof(string));
    
            var fromDateParameter = fromDate != null ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(string));
    
            var toDateParameter = toDate != null ?
                new ObjectParameter("toDate", toDate) :
                new ObjectParameter("toDate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_getRequestGuardStatistics_Result>("usp_getRequestGuardStatistics", aspNetUsersIdParameter, fromDateParameter, toDateParameter);
        }
    }
}
