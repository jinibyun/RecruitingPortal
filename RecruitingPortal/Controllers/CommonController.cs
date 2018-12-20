using AutoMapper;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.Controllers;
using RecruitingPortal.Domain;
using RecruitingPortal.Infrastructure;
using RecruitingPortal.Models;
using RecruitingPortal.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    public class CommonController : BaseController
    {
        public CommonController(PortalService<JobSeekerFile> jobSeekerFile, 
                                PortalService<JobPostingFile> jobPostingFile, 
                                PortalService<JobPosting> jobPosting,
                                PortalService<RecruitingPortal.Domain.TypeOfService> typeOfService,
                                PortalService<GuardRequest> guardRequest)
        {
            jobSeekerFileService = jobSeekerFile;
            jobPostingFileService = jobPostingFile;
            jobPostingService = jobPosting;
            typeOfServiceService = typeOfService;
            guardRequestService = guardRequest;
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeleteFile(int FileId, string fileName, string mode, string filePostType)
        {
            try
            {
                if (mode.Equals("JobSeeker", StringComparison.InvariantCultureIgnoreCase))
                {
                    var jobSeekerFiles = jobSeekerFileService.Get(x => x.Id == FileId);

                    if (jobSeekerFiles != null && jobSeekerFiles.Count() > 0)
                    {
                        var jobSeekerFile = jobSeekerFiles.SingleOrDefault<JobSeekerFile>();
                        if (jobSeekerFile != null)
                        {
                            var resumeDir = Server.MapPath(string.Format("~/FileUploaded/JobSeeker/{0}/{1}", jobSeekerFile.JobSeekerId, filePostType));
                            DirectoryInfo dir = new DirectoryInfo(resumeDir);

                            // remove record and file
                            foreach (var file in dir.GetFiles())
                            {
                                file.Delete();
                            }

                            // note: no delete but change to just empty 
                            switch (filePostType.ToLowerInvariant())
                            {
                                case "resume":
                                    jobSeekerFile.ResumeFilePath = string.Empty;
                                    break;
                                case "securitylicensecertificate":
                                    jobSeekerFile.SecurityLicenseCertificateFilePath = string.Empty;
                                    break;
                                case "cprfirstaidcertificate":
                                    jobSeekerFile.CPRFirstAidCertificateFilePath = string.Empty;
                                    break;
                                case "eligibilityfile":
                                    jobSeekerFile.EligibilityFilePath = string.Empty;
                                    break;
                                case "educationfile":
                                    jobSeekerFile.EducationFilePath = string.Empty;
                                    break;
                                //case "photopath":
                                //    break;
                            }

                            jobSeekerFileService.Change(jobSeekerFile);

                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else if (mode.Equals("JobPosting", StringComparison.InvariantCultureIgnoreCase))
                {
                    var jobPostingFiles = jobPostingFileService.Get(x => x.Id == FileId);

                    if (jobPostingFiles != null && jobPostingFiles.Count() > 0)
                    {
                        var jobPostingFile = jobPostingFiles.SingleOrDefault<JobPostingFile>();
                        if (jobPostingFile != null)
                        {
                            var jobPostingDir = Server.MapPath(string.Format("~/FileUploaded/JobPosting/{0}", jobPostingFile.JobPostingId));
                            DirectoryInfo dir = new DirectoryInfo(jobPostingDir);

                            // remove record and file
                            foreach (var file in dir.GetFiles())
                            {
                                file.Delete();
                            }

                            // note: no delete but change to just empty 
                            switch (filePostType.ToLowerInvariant())
                            {
                                case "jobposting":
                                    jobPostingFile.JobFilePath1 = string.Empty;
                                    break;

                            }

                            jobPostingFileService.Change(jobPostingFile);

                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        public PartialViewResult GetTypeOfService()
        {
            // type of service
            List<SelectListItem> typeOfServices = new List<SelectListItem>();
            typeOfServices.Add(new SelectListItem { Text = "All Service Type", Value = "0" });

            var typeServices = typeOfServiceService.GetAll();
            foreach (var member in typeServices)
            {
                typeOfServices.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
            }

            var viewModel = new TypeOfServiceViewModel();
            viewModel.ItemsInDropDown = typeOfServices;
            

            return PartialView("~/Views/Shared/_TypeOfService.cshtml", viewModel);
        }

        public PartialViewResult GetJobPostedOrNotPosted()
        {
            List<SelectListItem> typeOfJobPosted = new List<SelectListItem>();
            typeOfJobPosted.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            typeOfJobPosted.Add(new SelectListItem { Text = "ADs Posted", Value = "JP" });
            typeOfJobPosted.Add(new SelectListItem { Text = "ADs Pending", Value = "JNP" });

            return PartialView("~/Views/Shared/_JobPostedOrNotPosted.cshtml", typeOfJobPosted);
        }

        [HttpGet]
        public FileContentResult ExportToExcel(ReportType reportType)
        {
            if (reportType == ReportType.JOBPOSTING)
            {
                var reportJobPostData = new ReportJobPostData();
                var postingData = jobPostingService.Get(x => x.IsDeleted == false).OrderByDescending(y => y.Id);

                reportJobPostData.AllJobPostData = new List<AllJobPostViewModel>();
                foreach (var member in postingData)
                {
                    reportJobPostData.AllJobPostData.Add(new AllJobPostViewModel
                    {
                        Address = member.Address,
                        City = member.city != null ? member.city.City1 : string.Empty,
                        CreateDate = member.CreateDate,
                        GuardRequestId = member.GuardRequestId,
                        // Id = member.Id,
                        JobId = member.JobId,
                        PostalCode = member.PostalCode,
                        ServiceTypeName = member.TypeOfService != null ? member.TypeOfService.Name : string.Empty,
                        Title = member.Title
                    });
                }


                string[] columns = { "JobId", "Title", "City", "Address",
                                 "PostalCode", "CreateDate","GuardRequestId","ServiceTypeName" };
                byte[] filecontent = ExcelExportHelper.ExportExcel(reportJobPostData.AllJobPostData, string.Format("All Job Posting on {0} ", DateTime.Now), true, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, string.Format("All Job Posting on {0}.xlsx", DateTime.Now));
            }
            else if ( reportType == ReportType.GUARDREQUEDST)
            {
                var reportGuardRequestData = new ReportGuardRequestData();
                var guardRequestData = guardRequestService.Get(x => x.IsDeleted == false).OrderByDescending(y => y.Id);

                reportGuardRequestData.AllGuardRequestData = new List<AllGuardRequestViewModel>();
                foreach (var member in guardRequestData)
                {
                    reportGuardRequestData.AllGuardRequestData.Add(new AllGuardRequestViewModel
                    {
                        JobPostDate = (member.JobPostings != null && member.JobPostings.Any()) ? member.JobPostings.Select(x => x.CreateDate).FirstOrDefault() : null,
                        RequestDate = member.CreateDate,
                        Requestor = member.AspNetUser.Email,
                        Responder = (member.JobPostings != null && member.JobPostings.Any()) ? member.JobPostings.Select(x => x.AspNetUser.Email).FirstOrDefault() : "",
                        CreateDate = member.CreateDate,
                        //GuardRequestId = member.Id,
                        JobId = (member.JobPostings != null && member.JobPostings.Any()) ? member.JobPostings.Select(x => x.JobId).FirstOrDefault() : "",                        
                        RequestServiceTypeName = member.TypeOfService.Name,
                        RequestServiceLocation = member.ServiceLocation
                    });
                }


                string[] columns = { "JobId", "Requestor", "Responder",
                                   "CreateDate","RequestDate","JobPostDate", "RequestServiceTypeName", "RequestAddress" };
                byte[] filecontent = ExcelExportHelper.ExportExcel(reportGuardRequestData.AllGuardRequestData, string.Format("All Guard Requests on {0} ", DateTime.Now), true, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, string.Format("All Guard Requests on {0}.xlsx", DateTime.Now));
            }

            return null;
        }

        

    }
}