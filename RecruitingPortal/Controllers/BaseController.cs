using AutoMapper;
using RecruitingPortal.BLL;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.Domain;
using RecruitingPortal.Infrastructure;
using RecruitingPortal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    public class BaseController : Controller
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string ReturnText { get; set; }
        protected const int CountryId = 43; // Canada

        protected PortalService<AspNetUser> aspNetUserService { get; set; }
        protected PortalService<JobPosting> jobPostingService { get; set; }
        protected PortalService<JobPostingTypeOfWork> jobPostingTypeOfWorkService { get; set; }
        protected PortalService<JobPostingFile> postingFileService { get; set; }
        protected PortalService<JobSeeker> jobSeekerService { get; set; }
        protected PortalService<city> cityService { get; set; }
        protected PortalService<Region> regionService { get; set; }
        protected PortalService<JobSeekerFile> jobSeekerFileService { get; set; }
        protected PortalService<JobPostingFile> jobPostingFileService { get; set; }
        protected PortalService<JobSeekerLang> jobSeekerLangService { get; set; }
        protected PortalService<JobSeekerTypeOfWork> jobSeekerTypeOfWorkService { get; set; }
        protected PortalService<JobSeekerTypeOfWorkAvailability> jobSeekerTypeOfWorkServiceAvailabilityService { get; set; }
        protected PortalService<JobApply> jobApplyService { get; set; }
        protected PortalService<JobHire> jobHireService { get; set; }
        protected PortalService<Company> companyService { get; set; }
        protected PortalService<CompanyFile> companyFileService { get; set; }
        protected PortalService<CompanyLang> companyLangService { get; set; }
        protected PortalService<CompanyTypeOfWork> companyTypeOfWorkService { get; set; }
        protected PortalService<NotificationQueue> notificationQueueService { get; set; }
        protected PortalService<JobAlert> jobAlertService { get; set; }
        protected PortalService<Domain.TypeOfService> typeOfServiceService { get; set; }
        protected PortalService<TypeOfPosition> typeOfPositionService { get; set; }
        protected PortalService<TypeOfWork> typeOfWorkServiceService { get; set; }
        protected PortalService<TypeOfSecurityExperience> typeOfSecurityExperienceService { get; set; }
        protected PortalService<JobSeekerSecurityExperience> jobSeekerSecurityExperienceService { get; set; }
        protected PortalService<TypeOfJobNotice> typeOfJobNoticeService { get; set; }
        protected PortalService<JobSeekerScore> jobSeekerScoreService { get; set; }
        protected PortalService<GuardRequest> guardRequestService { get; set; }
        protected PortalService<JobSeekerContactLog> jobSeekerContactLogService { get; set; }
        protected PortalService<BranchAddress> branchAddressService { get; set; }
        protected PortalService<StaffTeam> staffTeamService { get; set; }
        protected PortalService<GuardRequestTypeOfWork> guardRequestTypeOfWorkService { get; set; }
        protected PortalService<JobSeekerStatu> jobSeekerStatusService { get; set; }
        protected Dictionary<EnumFilePostType, HttpPostedFileBase> UploadFiles;

        protected IBusinessLayer _service;

        public BaseController()
        {            
        }
        public BaseController(IBusinessLayer service)
        {
            _service = service;
        }

        protected int PageSize
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["PageSize"]);
            }
        }

        public LoggedInUserViewModel LoggedInUser
        {
            get
            {
                return WebUtil.GetSession<LoggedInUserViewModel>("LoggedInUserViewModel") != null ? WebUtil.GetSession<LoggedInUserViewModel>("LoggedInUserViewModel") : null;
            }
        }

        protected void SetChildTable<T, U>(T viewModelParam,
                                            U entityParam,
                                            PageMode pageMode,
                                            Dictionary<EnumFilePostType, HttpPostedFileBase> UploadFiles = null
                                            )
        {
            if (entityParam is JobSeeker)
            {
                JobSeekerViewModel viewModel = (JobSeekerViewModel)(object)viewModelParam;
                JobSeeker entity = (JobSeeker)(object)entityParam;
                // JobSeekerTypeOfWork
                foreach (var m in viewModel.CheckBoxListTypeOfWork)
                {
                    var jobSeekerTypeOfWork = jobSeekerTypeOfWorkService.Get(x => x.TypeOfWorkId == m.Id && x.JobSeekerId == entity.Id).SingleOrDefault<JobSeekerTypeOfWork>();
                    if (jobSeekerTypeOfWork == null)
                    {
                        if (m.Checked) // add
                        {
                            jobSeekerTypeOfWorkService.Add(new JobSeekerTypeOfWork
                            {
                                JobSeekerId = entity.Id,
                                TypeOfWorkId = m.Id
                            });
                        }
                    }
                    else
                    {
                        if (!m.Checked) // delete
                        {
                            jobSeekerTypeOfWorkService.Delete(jobSeekerTypeOfWork);
                        }
                    }
                }

                foreach (var m in viewModel.CheckBoxListSecurityExperience)
                {
                    var jobSeekerSecurityExperience = jobSeekerSecurityExperienceService.Get(x => x.SecurityExperienceId == m.Id && x.JobSeekerId == entity.Id).SingleOrDefault<JobSeekerSecurityExperience>();
                    if (jobSeekerSecurityExperience == null)
                    {
                        if (m.Checked) // add
                        {
                            jobSeekerSecurityExperienceService.Add(new JobSeekerSecurityExperience
                            {
                                JobSeekerId = entity.Id,
                                SecurityExperienceId = m.Id
                            });
                        }
                    }
                    else
                    {
                        if (!m.Checked) // delete
                        {
                            jobSeekerSecurityExperienceService.Delete(jobSeekerSecurityExperience);
                        }
                    }
                }

                foreach (var m in viewModel.CheckBoxListSecuritySubExperience)
                {
                    var jobSeekerSecurityExperience = jobSeekerSecurityExperienceService.Get(x => x.SecurityExperienceId == m.Id && x.JobSeekerId == entity.Id).SingleOrDefault<JobSeekerSecurityExperience>();
                    if (jobSeekerSecurityExperience == null)
                    {
                        if (m.Checked) // add
                        {
                            jobSeekerSecurityExperienceService.Add(new JobSeekerSecurityExperience
                            {
                                JobSeekerId = entity.Id,
                                SecurityExperienceId = m.Id
                            });
                        }
                    }
                    else
                    {
                        if (!m.Checked) // delete
                        {
                            jobSeekerSecurityExperienceService.Delete(jobSeekerSecurityExperience);
                        }
                    }
                }


                // JobSeekerLangs
                foreach (var m in viewModel.CheckBoxListLanguages)
                {
                    var jobSeekerLang = jobSeekerLangService.Get(x => x.LangId == m.Id && x.JobSeekerId == entity.Id).SingleOrDefault<JobSeekerLang>();
                    if (jobSeekerLang == null)
                    {
                        if (m.Checked) // add
                        {
                            jobSeekerLangService.Add(new JobSeekerLang
                            {
                                JobSeekerId = entity.Id,
                                LangId = m.Id
                            });
                        }
                    }
                    else
                    {
                        if (!m.Checked) // delete
                        {
                            jobSeekerLangService.Delete(jobSeekerLang);
                        }
                    }
                }
                if (UploadFiles != null)
                    SaveFile<JobSeeker>(entity, UploadFiles);
            }
            else if (entityParam is Company)
            {
                CompanyViewModel viewModel = (CompanyViewModel)(object)viewModelParam;
                Company entity = (Company)(object)entityParam;

                // CompanyTypeOfWork
                foreach (var m in viewModel.CheckBoxListTypeOfWork)
                {
                    var companyTypeOfWork = companyTypeOfWorkService.Get(x => x.TypeOfWorkId == m.Id && x.CompanyId == entity.Id).SingleOrDefault<CompanyTypeOfWork>();
                    if (companyTypeOfWork == null)
                    {
                        if (m.Checked) // add
                        {
                            companyTypeOfWorkService.Add(new CompanyTypeOfWork
                            {
                                CompanyId = entity.Id,
                                TypeOfWorkId = m.Id
                            });
                        }
                    }
                    else
                    {
                        if (!m.Checked) // delete
                        {
                            companyTypeOfWorkService.Delete(companyTypeOfWork);
                        }
                    }
                }

                // CompanyLangs
                foreach (var m in viewModel.CheckBoxListLanguages)
                {
                    var companyLang = companyLangService.Get(x => x.LangId == m.Id && x.CompanyId == entity.Id).SingleOrDefault<CompanyLang>();
                    if (companyLang == null)
                    {
                        if (m.Checked) // add
                        {
                            companyLangService.Add(new CompanyLang
                            {
                                CompanyId = entity.Id,
                                LangId = m.Id
                            });
                        }
                    }
                    else
                    {
                        if (!m.Checked) // delete
                        {
                            companyLangService.Delete(companyLang);
                        }
                    }
                }
                if (UploadFiles != null)
                    SaveFile<Company>(entity, UploadFiles);
            }
            else if (entityParam is JobPosting)
            {
                JobPostingViewModel viewModel = (JobPostingViewModel)(object)viewModelParam;
                JobPosting entity = (JobPosting)(object)entityParam;

                foreach (var m in viewModel.CheckBoxListTypeOfWork)
                {
                    var list = jobPostingTypeOfWorkService.Get(x => x.TypeOfWorkId == m.Id && x.JobPostingId == entity.Id);
                    if (list != null && list.Count() > 0)
                    {
                        var jobPostingTypeOfWork = list.SingleOrDefault<JobPostingTypeOfWork>();
                        if (!m.Checked) // delete
                        {
                            jobPostingTypeOfWorkService.Delete(jobPostingTypeOfWork);
                        }
                    }
                    else
                    {
                        if (m.Checked) // add
                        {
                            jobPostingTypeOfWorkService.Add(new JobPostingTypeOfWork
                            {
                                JobPostingId = entity.Id,
                                TypeOfWorkId = m.Id
                            });
                        }
                    }
                }
                if (UploadFiles != null)
                    SaveFile<JobPosting>(entity, UploadFiles);
            }
            else if (entityParam is GuardRequest)
            {
                GuardRequestViewModel viewModel = (GuardRequestViewModel)(object)viewModelParam;
                GuardRequest entity = (GuardRequest)(object)entityParam;

                foreach (var m in viewModel.CheckBoxListTypeOfWork)
                {
                    var list = guardRequestTypeOfWorkService.Get(x => x.TypeOfWorkId == m.Id && x.GuardRequestId == entity.Id);
                    if (list != null && list.Count() > 0)
                    {
                        var guardRequestTypeOfWork = list.SingleOrDefault<GuardRequestTypeOfWork>();
                        if (!m.Checked) // delete
                        {
                            guardRequestTypeOfWorkService.Delete(guardRequestTypeOfWork);
                        }
                    }
                    else
                    {
                        if (m.Checked) // add
                        {
                            guardRequestTypeOfWorkService.Add(new GuardRequestTypeOfWork
                            {
                                GuardRequestId = entity.Id,
                                TypeOfWorkId = m.Id
                            });
                        }
                    }
                }
            }
        }

        private void SaveFile<T>(T entityParam, Dictionary<EnumFilePostType, HttpPostedFileBase> uploadFiles) //, HttpPostedFileBase attached)
        {
            if (entityParam is JobSeeker)
            {
                JobSeeker entity = (JobSeeker)(object)entityParam;
                IEnumerable<JobSeekerFile> jobSeekerFiles = null;
                JobSeekerFile jobSeekerFile = null;


                foreach (var m in uploadFiles)
                {
                    var existFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).Any();
                    var fileName = Path.GetFileName(m.Value.FileName);
                    var dir = Server.MapPath(string.Format("~/FileUploaded/JobSeeker/{0}/{1}", entity.Id, m.Key.ToString()));
                    DirectoryInfo fullDir = new DirectoryInfo(dir);
                    if (!fullDir.Exists)
                        fullDir.Create();
                    var fullFilePath = Path.Combine(dir + "\\", fileName);

                    // ref: http://stackoverflow.com/questions/15680629/mvc-4-razor-file-upload  
                    switch (m.Key)
                    {
                        case EnumFilePostType.Resume:
                            jobSeekerFiles = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id && !string.IsNullOrEmpty(x.ResumeFilePath));
                            var existResume = jobSeekerFiles.Any();
                            if (!existResume)
                            {
                                // add file
                                m.Value.SaveAs(fullFilePath);

                                if (existFile) // update record
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.ResumeFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                                else // add record
                                {
                                    jobSeekerFile = new JobSeekerFile();
                                    jobSeekerFile.JobSeekerId = entity.Id;
                                    jobSeekerFile.ResumeFilePath = fullFilePath;
                                    jobSeekerFileService.Add(jobSeekerFile);
                                }
                            }
                            else
                            {
                                var existSameFileName = jobSeekerFiles.Where(x => string.Equals(x.ResumeFilePath, fullFilePath, StringComparison.InvariantCultureIgnoreCase)).Any();
                                // remove only file and add file
                                foreach (var file in fullDir.GetFiles())
                                {
                                    if (string.Equals(file.Name, fileName, StringComparison.InvariantCultureIgnoreCase))
                                        file.Delete();
                                }
                                m.Value.SaveAs(fullFilePath);

                                // then update record if it is not same file name
                                if (!existSameFileName)
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.ResumeFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                            }
                            break;
                        case EnumFilePostType.SecurityLicenseCertificate:
                            jobSeekerFiles = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id && !string.IsNullOrEmpty(x.SecurityLicenseCertificateFilePath));
                            var existSecurityLicense = jobSeekerFiles.Any();
                            if (!existSecurityLicense)
                            {
                                // add file
                                m.Value.SaveAs(fullFilePath);

                                if (existFile) // update record
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.SecurityLicenseCertificateFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                                else // add record
                                {
                                    jobSeekerFile = new JobSeekerFile();
                                    jobSeekerFile.JobSeekerId = entity.Id;
                                    jobSeekerFile.SecurityLicenseCertificateFilePath = fullFilePath;
                                    jobSeekerFileService.Add(jobSeekerFile);
                                }
                            }
                            else
                            {
                                var existSameFileName = jobSeekerFiles.Where(x => string.Equals(x.SecurityLicenseCertificateFilePath, fullFilePath, StringComparison.InvariantCultureIgnoreCase)).Any();
                                // remove only file and add file
                                foreach (var file in fullDir.GetFiles())
                                {
                                    if (string.Equals(file.Name, fileName, StringComparison.InvariantCultureIgnoreCase))
                                        file.Delete();
                                }
                                m.Value.SaveAs(fullFilePath);

                                // then update record if it is not same file name
                                if (!existSameFileName)
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.SecurityLicenseCertificateFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                            }
                            break;
                        case EnumFilePostType.EligibilityFile:
                            jobSeekerFiles = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id && !string.IsNullOrEmpty(x.EligibilityFilePath));
                            var existEligibility = jobSeekerFiles.Any();
                            if (!existEligibility)
                            {
                                // add file
                                m.Value.SaveAs(fullFilePath);

                                if (existFile) // update record
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.EligibilityFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                                else // add record
                                {
                                    jobSeekerFile = new JobSeekerFile();
                                    jobSeekerFile.JobSeekerId = entity.Id;
                                    jobSeekerFile.EligibilityFilePath = fullFilePath;
                                    jobSeekerFileService.Add(jobSeekerFile);
                                }
                            }
                            else
                            {
                                var existSameFileName = jobSeekerFiles.Where(x => string.Equals(x.EligibilityFilePath, fullFilePath, StringComparison.InvariantCultureIgnoreCase)).Any();
                                // remove only file and add file
                                foreach (var file in fullDir.GetFiles())
                                {
                                    if (string.Equals(file.Name, fileName, StringComparison.InvariantCultureIgnoreCase))
                                        file.Delete();
                                }
                                m.Value.SaveAs(fullFilePath);

                                // then update record if it is not same file name
                                if (!existSameFileName)
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.EligibilityFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                            }
                            break;
                        case EnumFilePostType.CPRFirstAidCertificate:
                            jobSeekerFiles = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id && !string.IsNullOrEmpty(x.CPRFirstAidCertificateFilePath));
                            var existCPRAid = jobSeekerFiles.Any();
                            if (!existCPRAid)
                            {
                                // add file
                                m.Value.SaveAs(fullFilePath);

                                if (existFile) // update record
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.CPRFirstAidCertificateFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                                else // add record
                                {
                                    jobSeekerFile = new JobSeekerFile();
                                    jobSeekerFile.JobSeekerId = entity.Id;
                                    jobSeekerFile.CPRFirstAidCertificateFilePath = fullFilePath;
                                    jobSeekerFileService.Add(jobSeekerFile);
                                }
                            }
                            else
                            {
                                var existSameFileName = jobSeekerFiles.Where(x => string.Equals(x.CPRFirstAidCertificateFilePath, fullFilePath, StringComparison.InvariantCultureIgnoreCase)).Any();
                                // remove only file and add file
                                foreach (var file in fullDir.GetFiles())
                                {
                                    if (string.Equals(file.Name, fileName, StringComparison.InvariantCultureIgnoreCase))
                                        file.Delete();
                                }
                                m.Value.SaveAs(fullFilePath);

                                // then update record if it is not same file name
                                if (!existSameFileName)
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.CPRFirstAidCertificateFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                            }
                            break;
                        case EnumFilePostType.EducationFile:
                            jobSeekerFiles = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id && !string.IsNullOrEmpty(x.EducationFilePath));
                            var existEducation = jobSeekerFiles.Any();
                            if (!existEducation)
                            {
                                // add file
                                m.Value.SaveAs(fullFilePath);

                                if (existFile) // update record
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.EducationFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                                else // add record
                                {
                                    jobSeekerFile = new JobSeekerFile();
                                    jobSeekerFile.JobSeekerId = entity.Id;
                                    jobSeekerFile.EducationFilePath = fullFilePath;
                                    jobSeekerFileService.Add(jobSeekerFile);
                                }
                            }
                            else
                            {
                                var existSameFileName = jobSeekerFiles.Where(x => string.Equals(x.EducationFilePath, fullFilePath, StringComparison.InvariantCultureIgnoreCase)).Any();
                                // remove only file and add file
                                foreach (var file in fullDir.GetFiles())
                                {
                                    if (string.Equals(file.Name, fileName, StringComparison.InvariantCultureIgnoreCase))
                                        file.Delete();
                                }
                                m.Value.SaveAs(fullFilePath);

                                // then update record if it is not same file name
                                if (!existSameFileName)
                                {
                                    jobSeekerFile = jobSeekerFileService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerFile>();
                                    jobSeekerFile.EducationFilePath = fullFilePath;
                                    jobSeekerFileService.Change(jobSeekerFile);

                                }
                            }
                            break;
                    }

                } // end of UploadFiles 

            }
            else if (entityParam is JobPosting)
            {
                JobPosting entity = (JobPosting)(object)entityParam;
                IEnumerable<JobPostingFile> jobPostingFiles = null;
                JobPostingFile jobPostingFile = null;


                foreach (var m in uploadFiles)
                {
                    var existFile = jobPostingFileService.Get(x => x.JobPostingId == entity.Id).Any();
                    var fileName = Path.GetFileName(m.Value.FileName);
                    var dir = Server.MapPath(string.Format("~/FileUploaded/JobPosting/{0}", entity.Id));
                    DirectoryInfo fullDir = new DirectoryInfo(dir);
                    if (!fullDir.Exists)
                        fullDir.Create();
                    var fullFilePath = Path.Combine(dir + "\\", fileName);

                    // ref: http://stackoverflow.com/questions/15680629/mvc-4-razor-file-upload  
                    switch (m.Key)
                    {
                        case EnumFilePostType.JobPosting:
                            jobPostingFiles = jobPostingFileService.Get(x => x.JobPostingId == entity.Id && !string.IsNullOrEmpty(x.JobFilePath1));
                            var existJobFile1 = jobPostingFiles.Any();
                            if (!existJobFile1)
                            {
                                // add file
                                m.Value.SaveAs(fullFilePath);

                                if (existFile) // update record
                                {
                                    jobPostingFile = jobPostingFileService.Get(x => x.JobPostingId == entity.Id).FirstOrDefault<JobPostingFile>();
                                    jobPostingFile.JobFilePath1 = fullFilePath;
                                    jobPostingFileService.Change(jobPostingFile);

                                }
                                else // add record
                                {
                                    jobPostingFile = new JobPostingFile();
                                    jobPostingFile.JobPostingId = entity.Id;
                                    jobPostingFile.JobFilePath1 = fullFilePath;
                                    jobPostingFileService.Add(jobPostingFile);
                                }
                            }
                            else
                            {
                                var existSameFileName = jobPostingFiles.Where(x => string.Equals(x.JobFilePath1, fullFilePath, StringComparison.InvariantCultureIgnoreCase)).Any();
                                // remove only file and add file
                                foreach (var file in fullDir.GetFiles())
                                {
                                    if (string.Equals(file.Name, fileName, StringComparison.InvariantCultureIgnoreCase))
                                        file.Delete();
                                }
                                m.Value.SaveAs(fullFilePath);

                                // then update record if it is not same file name
                                if (!existSameFileName)
                                {
                                    jobPostingFile = jobPostingFileService.Get(x => x.JobPostingId == entity.Id).FirstOrDefault<JobPostingFile>();
                                    jobPostingFile.JobFilePath1 = fullFilePath;
                                    jobPostingFileService.Change(jobPostingFile);

                                }
                            }
                            break;
                    }

                } // end of UploadFiles 
            }
        }

        // ref: http://stackoverflow.com/questions/730699/how-can-i-present-a-file-for-download-from-an-mvc-controller
        // ref: http://www.c-sharpcorner.com/UploadFile/rahul4_saxena/download-file-in-mvc-4/
        public FileResult DownloadFile(string name, int fileId, EnumFilePostType filePostType)
        {
            string fullFilePath = string.Empty;
            if (name.Equals("JobSeekerFile", StringComparison.InvariantCultureIgnoreCase))
            {
                var jobSeekerFile = jobSeekerFileService.Get(x => x.Id == fileId).FirstOrDefault<JobSeekerFile>();
                switch (filePostType)
                {
                    case EnumFilePostType.Resume:
                        fullFilePath = jobSeekerFile.ResumeFilePath;
                        break;
                    case EnumFilePostType.SecurityLicenseCertificate:
                        fullFilePath = jobSeekerFile.SecurityLicenseCertificateFilePath;
                        break;
                    case EnumFilePostType.EligibilityFile:
                        fullFilePath = jobSeekerFile.EligibilityFilePath;
                        break;
                    case EnumFilePostType.CPRFirstAidCertificate:
                        fullFilePath = jobSeekerFile.CPRFirstAidCertificateFilePath;
                        break;
                    case EnumFilePostType.EducationFile:
                        fullFilePath = jobSeekerFile.EducationFilePath;
                        break;
                }
            }
            else if (name.Equals("JobPostingFile", StringComparison.InvariantCultureIgnoreCase))
            {
                var jobPostingFile = jobPostingFileService.Get(x => x.Id == fileId).FirstOrDefault<JobPostingFile>();
                switch (filePostType)
                {
                    case EnumFilePostType.JobPosting:
                        fullFilePath = jobPostingFile.JobFilePath1;
                        break;
                }
            }

            string contentType = WebUtil.GetMimeType(fullFilePath);
            return File(fullFilePath, contentType, Server.UrlEncode(Path.GetFileName(fullFilePath)).Replace("+", " "));
        }

        protected void ApplyFilter<T>(ref IEnumerable<T> list, FormCollection frmCollection, Dictionary<string, string> frmSearchWithPageCollection)
        {
            // note: if frmSearchWithPageCollection exist, then apply it instead of frmCollection
            if (list is IEnumerable<JobPosting>) // JobPosting Search
            {
                var data = (IEnumerable<JobPosting>)list;
                string jobNameOrContent = string.Empty;
                string Title = string.Empty;
                string Description = string.Empty;
                string Rate = string.Empty;
                string ServiceType = string.Empty;
                string WorkType = string.Empty;
                string City = string.Empty;
                string Region = string.Empty;
                string PostalCode = string.Empty;
                string distance = string.Empty;

                if (frmSearchWithPageCollection == null) // initail searching
                {
                    jobNameOrContent = frmCollection["jobNameOrContent"]; // title or description     
                    Title = frmCollection["Title"];
                    Description = frmCollection["Description"];
                    Rate = frmCollection["Rate"];
                    ServiceType = frmCollection["ServiceType"];
                    WorkType = frmCollection["WorkType"];
                    City = frmCollection["CityId"];
                    Region = frmCollection["Region"];
                    PostalCode = frmCollection["PostalCode"];
                    distance = frmCollection["distance"];
                }
                else // after initial searching (click page number)
                {
                    jobNameOrContent = frmSearchWithPageCollection.ContainsKey("JobNameOrContent") ? frmSearchWithPageCollection["JobNameOrContent"] : string.Empty; // title or description       
                    Title = frmSearchWithPageCollection.ContainsKey("Title") ? frmSearchWithPageCollection["Title"] : string.Empty;
                    Description = frmSearchWithPageCollection.ContainsKey("Description") ? frmSearchWithPageCollection["Description"] : string.Empty;
                    Rate = frmSearchWithPageCollection.ContainsKey("Rate") ? frmSearchWithPageCollection["Rate"] : string.Empty;
                    ServiceType = frmSearchWithPageCollection.ContainsKey("ServiceTypeId") ? frmSearchWithPageCollection["ServiceTypeId"] : string.Empty;
                    WorkType = frmSearchWithPageCollection.ContainsKey("WorkType") ? frmSearchWithPageCollection["WorkType"] : string.Empty;
                    City = frmSearchWithPageCollection.ContainsKey("CityId") ? frmSearchWithPageCollection["CityId"] : string.Empty;
                    Region = frmSearchWithPageCollection.ContainsKey("RegionId") ? frmSearchWithPageCollection["RegionId"] : string.Empty;
                    PostalCode = frmSearchWithPageCollection.ContainsKey("PostalCode") ? frmSearchWithPageCollection["PostalCode"] : string.Empty;
                    distance = frmSearchWithPageCollection.ContainsKey("distance") ? frmSearchWithPageCollection["distance"] : string.Empty;
                }

                if (!string.IsNullOrEmpty(jobNameOrContent))
                {
                    data = data.Where(x => x.Title.Contains(jobNameOrContent, StringComparison.InvariantCultureIgnoreCase)
                                                            || x.Description.Contains(jobNameOrContent, StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    if (!string.IsNullOrEmpty(Title))
                    {
                        data = data.Where(x => x.Title.Contains(Title, StringComparison.InvariantCultureIgnoreCase));
                    }

                    if (!string.IsNullOrEmpty(Description))
                    {
                        data = data.Where(x => x.Description.Contains(Description, StringComparison.InvariantCultureIgnoreCase));
                    }


                    decimal resultRate = 0.0M;
                    if (decimal.TryParse(Rate, out resultRate))
                    {
                        data = data.Where(x => x.Rate <= resultRate);
                    }

                    if (!string.IsNullOrEmpty(ServiceType))
                    {
                        if (int.Parse(ServiceType) > 0)
                            data = data.Where(x => x.TypeOfServiceId == int.Parse(ServiceType));
                    }

                    if (!string.IsNullOrEmpty(City))
                    {
                        if (int.Parse(City) > 0)
                            data = data.Where(x => x.CityId == int.Parse(City));
                    }

                    if (!string.IsNullOrEmpty(Region))
                    {
                        if (int.Parse(Region) > 0)
                            data = data.Where(x => x.city != null && x.city.RegionID == int.Parse(Region));
                    }
                    //  TODO: post code search
                    if (!string.IsNullOrEmpty(PostalCode) && !string.IsNullOrEmpty(distance))
                    {
                        //data = data.Where(x => x.PostalCode != null && x.PostalCode != "");
                        //List<JobPosting> DistnaceFiltered = new List<JobPosting>();
                        List<string> targetPostCodes = new List<string>();

                        foreach (var m in data.ToList<JobPosting>())
                        {
                            if (!string.IsNullOrEmpty(m.PostalCode))
                            {
                                targetPostCodes.Add(m.PostalCode);
                            }
                        }

                        if (targetPostCodes.Count > 0)
                        {
                            List<string> resultAddress = WebUtil.GetPostalCodeByDistance(PostalCode, targetPostCodes, distance);

                            // resultAddress data format is ...e.g. "Thornhill, ON L4J 5X2, Canada"
                            List<string> postalCodes = new List<string>();
                            if (resultAddress.Count > 0)
                            {
                                foreach (var p in resultAddress)
                                {
                                    string[] Address = p.Split(new char[] { ',' });
                                    postalCodes.Add(Address[1].Trim().Substring(3));
                                }
                            }

                            if (postalCodes.Count > 0)
                            {
                                data = data.Where(p => p.PostalCode != null
                                        && p.PostalCode != ""
                                        && postalCodes.Contains(p.PostalCode));
                            }

                        }
                    }
                }


                //// note: checkbox is a bit weired
                //// ref: http://stackoverflow.com/questions/18458186/html-checkbox-control-form-collection-values-are-getting-with-some-extra-true-fa
                //if (frmCollection["WorkType"].Contains("true"))
                //{
                //    for(int i = 0; i < frmCollection["WorkType"].Count(); i++ )
                //    {
                //        if(i % 2 == 0)
                //        {
                //            // frmCollection["WorkType"][i]
                //                data = data.Where(x => x.TypeOfWorkId == int.Parse(ServiceType));
                //        }

                //    }
                //}

                list = (IEnumerable<T>)data;
            }
            else if (list is IEnumerable<JobSeeker>) // JobSeeker Search
            {
                var data = (IEnumerable<JobSeeker>)list;
                string jobSeekerNameOrContent = string.Empty;
                string jobSeekerName = string.Empty;
                string ExpYears = string.Empty;
                string ScoringRange = string.Empty;
                string ContactedBy = string.Empty;
                string MinRate = string.Empty;
                string MaxRate = string.Empty;
                string SeekerServiceType = string.Empty;
                string WorkType = string.Empty;
                string City = string.Empty;
                string Region = string.Empty;

                // work availiability checkbox
                bool isWeekday = false;
                bool isShift = false;

                // work type checkbox
                bool isFulltime = false;
                bool isParttime = false;

                // available languages checkbox
                bool isEnglish = false;
                bool isFrench = false;
                string PostalCode = string.Empty;
                string distance = string.Empty;

                if (frmSearchWithPageCollection == null) // initail searching
                {
                    jobSeekerNameOrContent = frmCollection["jobSeekerNameOrContent"]; // title or description     
                    jobSeekerName = frmCollection["JobSeekerName"];
                    ExpYears = frmCollection["ExpYears"];
                    ScoringRange = frmCollection["ScoringRange"];
                    ContactedBy = frmCollection["ContactedBy"];
                    SeekerServiceType = frmCollection["SeekerServiceType"];
                    City = frmCollection["CityId"];
                    Region = frmCollection["Region"];
                    MinRate = frmCollection["MinRate"];
                    //WorkType = frmCollection["WorkType"];   

                    // work availiability
                    if (frmCollection["chkAvailabilities"] != null)
                    {
                        List<string> chkAvailabilities = frmCollection["chkAvailabilities"].Replace("true,false", "true").Split(',').ToList();
                        isWeekday = bool.Parse(chkAvailabilities[0]);
                        isShift = bool.Parse(chkAvailabilities[1]);
                    }

                    // work type
                    if (frmCollection["chkWorkType"] != null)
                    {
                        List<string> chkWorkType = frmCollection["chkWorkType"].Replace("true,false", "true").Split(',').ToList();
                        isFulltime = bool.Parse(chkWorkType[0]);
                        isParttime = bool.Parse(chkWorkType[1]);
                    }

                    // available languages 
                    if (frmCollection["chkLanguages"] != null)
                    {
                        List<string> chkLanguages = frmCollection["chkLanguages"].Replace("true,false", "true").Split(',').ToList();
                        isEnglish = bool.Parse(chkLanguages[0]);
                        isFrench = bool.Parse(chkLanguages[1]);
                    }

                    PostalCode = frmCollection["PostalCode"];
                    distance = frmCollection["distance"];
                }
                else // after initial searching, paging
                {
                    jobSeekerNameOrContent = frmSearchWithPageCollection.ContainsKey("JobSeekerNameOrContent") ? frmSearchWithPageCollection["JobSeekerNameOrContent"] : string.Empty; // title or description       
                    jobSeekerName = frmSearchWithPageCollection.ContainsKey("JobSeekerName") ? frmSearchWithPageCollection["JobSeekerName"] : string.Empty;
                    ExpYears = frmSearchWithPageCollection.ContainsKey("ExpYears") ? frmSearchWithPageCollection["ExpYears"] : string.Empty;
                    ScoringRange = frmSearchWithPageCollection.ContainsKey("ScoringRange") ? frmSearchWithPageCollection["ScoringRange"] : string.Empty;
                    ContactedBy = frmSearchWithPageCollection.ContainsKey("ContactedBy") ? frmSearchWithPageCollection["ContactedBy"] : string.Empty;
                    SeekerServiceType = frmSearchWithPageCollection.ContainsKey("ServiceTypeId") ? frmSearchWithPageCollection["ServiceTypeId"] : string.Empty;
                    City = frmSearchWithPageCollection.ContainsKey("CityId") ? frmSearchWithPageCollection["CityId"] : string.Empty;
                    Region = frmSearchWithPageCollection.ContainsKey("RegionId") ? frmSearchWithPageCollection["RegionId"] : string.Empty;
                    MinRate = frmSearchWithPageCollection.ContainsKey("MinRate") ? frmSearchWithPageCollection["MinRate"] : string.Empty;
                    //WorkType = frmSearchWithPageCollection.ContainsKey("WorkType") ? frmSearchWithPageCollection["WorkType"] : string.Empty;
                    isWeekday = frmSearchWithPageCollection.ContainsKey("isWeekday") ? bool.Parse(frmSearchWithPageCollection["isWeekday"]) : false;
                    isShift = frmSearchWithPageCollection.ContainsKey("isShift") ? bool.Parse(frmSearchWithPageCollection["isShift"]) : false;
                    isFulltime = frmSearchWithPageCollection.ContainsKey("isFulltime") ? bool.Parse(frmSearchWithPageCollection["isFulltime"]) : false;

                    isParttime = frmSearchWithPageCollection.ContainsKey("isParttime") ? bool.Parse(frmSearchWithPageCollection["isParttime"]) : false;
                    isEnglish = frmSearchWithPageCollection.ContainsKey("isEnglish") ? bool.Parse(frmSearchWithPageCollection["isEnglish"]) : false;
                    isFrench = frmSearchWithPageCollection.ContainsKey("isFrench") ? bool.Parse(frmSearchWithPageCollection["isFrench"]) : false;

                    PostalCode = frmSearchWithPageCollection.ContainsKey("PostalCode") ? frmSearchWithPageCollection["PostalCode"] : string.Empty;
                    distance = frmSearchWithPageCollection.ContainsKey("distance") ? frmSearchWithPageCollection["distance"] : string.Empty;
                }

                if (!string.IsNullOrEmpty(jobSeekerNameOrContent))
                {
                    data = data.Where(x => x.FirstName.Contains(jobSeekerNameOrContent, StringComparison.InvariantCultureIgnoreCase)
                                        || x.LastName.Contains(jobSeekerNameOrContent, StringComparison.InvariantCultureIgnoreCase)
                                        || x.Address.Contains(jobSeekerNameOrContent, StringComparison.InvariantCultureIgnoreCase)
                                        || x.CellPhone.Contains(jobSeekerNameOrContent, StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    if (!string.IsNullOrEmpty(jobSeekerName))
                    {
                        data = data.Where(x => x.FirstName.Contains(jobSeekerName, StringComparison.InvariantCultureIgnoreCase)
                                            || x.LastName.Contains(jobSeekerName, StringComparison.InvariantCultureIgnoreCase));
                    }

                    if (!string.IsNullOrEmpty(ExpYears))
                    {
                        data = data.Where(x => (int)x.YearsOfExperience >= int.Parse(ExpYears));
                    }

                    if (!string.IsNullOrEmpty(ScoringRange))
                    {
                        data = data.Where(x => x.JobSeekerScores.Where(y => y.TotalScore.HasValue && (int)y.TotalScore >= int.Parse(ScoringRange)).Any());
                    }

                    if (!string.IsNullOrEmpty(ContactedBy))
                    {
                        data = data.Where(x => x.JobSeekerContactLogs.Where(y => y.AspNetUsersId == ContactedBy).Any());
                    }

                    if (!string.IsNullOrEmpty(SeekerServiceType))
                    {
                        if (int.Parse(SeekerServiceType) > 0)
                            data = data.Where(x => x.TypeOfServiceId == int.Parse(SeekerServiceType));
                    }

                    if (!string.IsNullOrEmpty(City))
                    {
                        if (int.Parse(City) > 0)
                            data = data.Where(x => x.city.CityId == int.Parse(City));

                    }

                    if (!string.IsNullOrEmpty(Region))
                    {
                        if (int.Parse(Region) > 0)
                            data = data.Where(x => x.city != null && x.city.RegionID == int.Parse(Region));
                    }


                    if (!string.IsNullOrEmpty(MinRate))
                    {
                        decimal result = 0.0M;
                        if (decimal.TryParse(MinRate, out result))
                        {
                            if (result > 0.0M)
                                data = data.Where(x => x.RateDesiredPerHour <= result);
                        }
                    }

                    // work availiability filtering
                    if (isWeekday)
                        data = data.Where(x => x.JobSeekerTypeOfWorkAvailabilities.Where(y => y.TypeOfWorkAvailabilityId == (int)EnumTypeOfWorkAvailability.Weekday).Any());

                    // work availiability filtering
                    if (isShift)
                        data = data.Where(x => x.JobSeekerTypeOfWorkAvailabilities.Where(y => y.TypeOfWorkAvailabilityId == (int)EnumTypeOfWorkAvailability.Shift).Any());


                    // work type
                    if (isFulltime)
                        data = data.Where(x => x.JobSeekerTypeOfWorks.Where(y => y.TypeOfWorkId == 1).Any());

                    // work type
                    if (isParttime)
                        data = data.Where(x => x.JobSeekerTypeOfWorks.Where(y => y.TypeOfWorkId == 2).Any());


                    // available languages
                    if (isEnglish)
                        data = data.Where(x => x.JobSeekerLangs.Where(y => y.LangId == (int)EnumLang.English).Any());


                    // available languages
                    if (isFrench)
                        data = data.Where(x => x.JobSeekerLangs.Where(y => y.LangId == (int)EnumLang.French).Any());

                    // TODO: post code search. use GetPostalCodeByDistance rather than below later
                    //if (!string.IsNullOrEmpty(PostalCode) && !string.IsNullOrEmpty(distance))
                    //{
                    //    data = data.Where(x => x.PostalCode != null && x.PostalCode != "");
                    //    List<JobSeeker> DistnaceFiltered = new List<JobSeeker>();
                    //    foreach (var m in data.ToList<JobSeeker>())
                    //    {
                    //        if (!string.IsNullOrEmpty(m.PostalCode))
                    //        {
                    //            float distanceBetween = WebUtil.GetDistanceByPostalCode(PostalCode, m.PostalCode);

                    //            switch (distance)
                    //            {
                    //                case "15": // 15 km
                    //                    if (distanceBetween > 0.0 && distanceBetween <= 15.0)
                    //                    {
                    //                        DistnaceFiltered.Add(m);
                    //                    }
                    //                    break;
                    //                case "40": // 40 km
                    //                    if (distanceBetween > 0.0 && distanceBetween <= 40.0)
                    //                    {
                    //                        DistnaceFiltered.Add(m);
                    //                    }
                    //                    break;
                    //                case "55": // 55 km
                    //                    if (distanceBetween > 0.0 && distanceBetween <= 55.0)
                    //                    {
                    //                        DistnaceFiltered.Add(m);
                    //                    }
                    //                    break;
                    //            }
                    //        }
                    //    }

                    //    data = DistnaceFiltered as IEnumerable<JobSeeker>;

                    //}
                }

                list = (IEnumerable<T>)data;
            }
        }

        protected void SetLocation<T>(ref T viewModelParam)
        {
            // ref: http://stackoverflow.com/questions/8830648/html-validationmessagefor-did-not-work-as-expected
            var listRegion = regionService.Get(x => x.CountryId == CountryId).OrderBy(y => y.Region1).ToList<Region>();
            List<RegionViewModel> regionViewModels = new List<RegionViewModel>();
            regionViewModels.Add(new RegionViewModel { RegionId = "", Region1 = "Select Province" });
            foreach (var m in listRegion)
            {
                regionViewModels.Add(new RegionViewModel { RegionId = m.RegionId.ToString(), Region1 = m.Region1 });
            }
            var regionsSelectList = regionViewModels.Select(x => new SelectListItem { Text = x.Region1, Value = x.RegionId.ToString() }).AsEnumerable<SelectListItem>();// as IEnumerable<SelectListItem>;


            List<CityViewModel> cityViewModels = new List<CityViewModel>();
            List<city> listCity = new List<city>();

            if (viewModelParam is JobSeekerViewModel)
            {
                JobSeekerViewModel viewModel = (JobSeekerViewModel)(object)viewModelParam;

                if (viewModel.AspNetUsers != null && viewModel.city != null) // edit mode
                {
                    // TODO
                    // listCity = cityService.Where(x => x.CountryID == CountryId && x.RegionID == viewModel.City.RegionID).OrderBy(y => y.City1).ToList<city>();
                }
                else // create mode
                {
                    listCity = cityService.Get(x => x.CountryID == CountryId).OrderBy(y => y.City1).ToList<city>();
                }

                cityViewModels.Add(new CityViewModel { CityId = "", City1 = "Select City" });
                foreach (var m in listCity)
                {
                    cityViewModels.Add(new CityViewModel { CityId = m.CityId.ToString(), City1 = m.City1 });
                }

                viewModel.RegionsInDropDown = regionsSelectList;
                viewModel.CitiesInDropDown = cityViewModels.Select(x => new SelectListItem { Text = x.City1, Value = x.CityId.ToString() }).AsEnumerable<SelectListItem>();// as IEnumerable<SelectListItem>;                            
            }
            // TODO
            //else if (viewModelParam is CompanyViewModel)
            //{
            //    CompanyViewModel viewModel = (CompanyViewModel)(object)viewModelParam;

            //    if (viewModel.Members.Where(x => x.city != null).Any())
            //    {
            //        listCity = cityService.Where(x => x.CountryID == CountryId && x.RegionID == viewModel.Members.Select(y => y.city.RegionID).FirstOrDefault()).OrderBy(y => y.City1).ToList<city>();
            //    }

            //    cityViewModels.Add(new CityViewModel { CityId = "", City1 = "Select City" });
            //    foreach (var m in listCity)
            //    {
            //        cityViewModels.Add(new CityViewModel { CityId = m.CityId.ToString(), City1 = m.City1 });
            //    }

            //    viewModel.RegionsInDropDown = regionsSelectList;
            //    viewModel.CitiesInDropDown = cityViewModels.Select(x => new SelectListItem { Text = x.City1, Value = x.CityId.ToString() }).AsEnumerable<SelectListItem>();
            //}
            else if (viewModelParam is JobPostingViewModel)
            {
                JobPostingViewModel viewModel = (JobPostingViewModel)(object)viewModelParam;

                if (viewModel.city != null)
                {
                    listCity = cityService.Get(x => x.CountryID == CountryId && x.RegionID == viewModel.city.RegionID).OrderBy(y => y.City1).ToList<city>();
                }

                cityViewModels.Add(new CityViewModel { CityId = "", City1 = "Select City" });
                foreach (var m in listCity)
                {
                    cityViewModels.Add(new CityViewModel { CityId = m.CityId.ToString(), City1 = m.City1 });
                }

                viewModel.RegionsInDropDown = regionsSelectList;
                viewModel.CitiesInDropDown = cityViewModels.Select(x => new SelectListItem { Text = x.City1, Value = x.CityId.ToString() }).AsEnumerable<SelectListItem>();
            }
            else if (viewModelParam is GuardRequestViewModel)
            {
                GuardRequestViewModel viewModel = (GuardRequestViewModel)(object)viewModelParam;

                if (viewModel.city != null)
                {
                    listCity = cityService.Get(x => x.CountryID == CountryId && x.RegionID == viewModel.city.RegionID).OrderBy(y => y.City1).ToList<city>();
                }

                cityViewModels.Add(new CityViewModel { CityId = "", City1 = "Select City" });
                foreach (var m in listCity)
                {
                    cityViewModels.Add(new CityViewModel { CityId = m.CityId.ToString(), City1 = m.City1 });
                }

                viewModel.RegionsInDropDown = regionsSelectList;
                viewModel.CitiesInDropDown = cityViewModels.Select(x => new SelectListItem { Text = x.City1, Value = x.CityId.ToString() }).AsEnumerable<SelectListItem>();
            }
        }

        protected void AddNotificationQueue(string subject, string body, List<string> emailTo, string aspNetUsersId,
                                            int? guardRequestId, int? jobAlertId, int? jobPostingId,
                                            EnumNotificationType notificationType)
        {
            var notificationQueue = new NotificationQueue
            {
                Subject = subject,
                Body = body,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                SentDate = null,
                EmailFrom = ConfigurationManager.AppSettings["SmtpSenderEmail"].ToString(),
                EmailTo = string.Join(";", emailTo),
                EmailCC = string.Empty,
                NumberOfAttempt = 0,
                IsBodyHTML = true,
                AttachmentFile = string.Empty,
                AspNetUsersId = aspNetUsersId,
                GuardRequestId = guardRequestId,
                JobAlertId = jobAlertId,
                JobPostingId = jobPostingId,
                NotificationTypeId = (int)notificationType,
                NotificationStatusTypeId = (int)EnumTypeOfNotificationStatus.NotSent
            };

            notificationQueueService.Add(notificationQueue);
        }
    }
}