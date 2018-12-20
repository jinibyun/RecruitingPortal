using AutoMapper;
using RecruitingPortal.Domain;
using RecruitingPortal.Models;

namespace RecruitingPortal.Mappings
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<JobPostingViewModel, JobPosting>()
                  .ForMember(dest => dest.EducationLevel, option => option.Ignore())
                  // .ForMember(dest => dest.TypeOfService, option => option.Ignore())
                  // .ForMember(dest => dest.TypeOfWork, option => option.Ignore())
                  ;
            CreateMap<GuardRequestViewModel, GuardRequest>()
                // .ForMember(dest => dest.TypeOfService, option => option.Ignore())
                ;

            CreateMap<JobPostingFileViewModel, JobPostingFile>();

            CreateMap<CityViewModel, city>();

            CreateMap<CompanyViewModel, Company>();

            CreateMap<AspNetUsersViewModel, AspNetUser>();

            CreateMap<JobSeekerViewModel, JobSeeker>()
                  .ForMember(dest => dest.EducationLevel, option => option.Ignore())
                  .ForMember(dest => dest.TypeOfService, option => option.Ignore())
                  .ForMember(dest => dest.JobSeekerStatu, option => option.Ignore())

                 // .ForMember(dest => dest.Member, option => option.Ignore())
                 ;

            CreateMap<JobApplyViewModel, JobApply>();
            CreateMap<JobAlertViewModel, JobAlert>();
            CreateMap<TypeOfServiceViewModel, TypeOfService>();
            CreateMap<TypeOfPositionViewModel, TypeOfPosition>();
            CreateMap<TypeOfWorkViewModel, TypeOfWork>();
            CreateMap<BranchAddressViewModel, BranchAddress>();
            CreateMap<JobSeekerContactLogViewModel, JobSeekerContactLog>();
            CreateMap<StaffTeamViewModel, StaffTeam>();
            CreateMap<GuardRequestTypeOfWorkViewModel, GuardRequestTypeOfWork>();
        }
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }
    }
}