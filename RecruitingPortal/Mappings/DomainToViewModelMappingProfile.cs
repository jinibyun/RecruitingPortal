using AutoMapper;
using RecruitingPortal.Domain;
using RecruitingPortal.Models;
namespace RecruitingPortal.Mappings
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<JobPosting, JobPostingViewModel>()
                  .ForMember(dest => dest.EducationLevel, option => option.Ignore())
                  // .ForMember(dest => dest.TypeOfService, option => option.Ignore())
                  // .ForMember(dest => dest.TypeOfWork, option => option.Ignore())
                  // .ForMember(dest => dest.city, option => option.Ignore())                  
                  // .ForMember(dest => dest.JobApplies, option => option.Ignore())
                  .ForMember(dest => dest.JobPostingFiles, option => option.Ignore())
                  ;

            CreateMap<GuardRequest, GuardRequestViewModel>()
                //.ForMember(dest => dest.TypeOfService, option => option.Ignore())
                ;

            CreateMap<JobPostingFile, JobPostingFileViewModel>();

            CreateMap<city, CityViewModel>();

            CreateMap<Company, CompanyViewModel>();

            CreateMap<AspNetUser, AspNetUsersViewModel>();

            CreateMap<JobSeeker, JobSeekerViewModel>()
                  .ForMember(dest => dest.EducationLevel, option => option.Ignore())
                  .ForMember(dest => dest.TypeOfService, option => option.Ignore())
                  .ForMember(dest => dest.JobSeekerStatu, option => option.Ignore())
                  //.ForMember(dest => dest.JobApplies, option => option.Ignore())

                  // .ForMember(dest => dest.Member, option => option.Ignore())
                  ;
            CreateMap<JobSeekerScore, JobSeekerScoreViewModel>();

            CreateMap<JobApply, JobApplyViewModel>();
            CreateMap<JobAlert, JobAlertViewModel>();
            CreateMap<TypeOfService, TypeOfServiceViewModel>();
            CreateMap<TypeOfWork, TypeOfWorkViewModel>();
            CreateMap<TypeOfPosition, TypeOfPositionViewModel>();
            CreateMap<BranchAddress, BranchAddressViewModel>();
            CreateMap<JobSeekerContactLog, JobSeekerContactLogViewModel>();
            CreateMap<StaffTeam, StaffTeamViewModel>();
            CreateMap<GuardRequestTypeOfWork, GuardRequestTypeOfWorkViewModel>();

        }
        // ref: http://tecexplorer.blogspot.ca/2013/01/using-automapper-with-aspnet-mvc.html
        // ref: http://www.c-sharpcorner.com/UploadFile/tirthacs/using-automapper-in-mvc/
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }
    }
}