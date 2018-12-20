using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.BLL
{
    public enum PageMode
    {
        CREATE = 1,
        EDIT = 2
    }

    public enum EnumMemberRole
    {       
        Administrator = 1,
        Company = 2,
        JobSeeker = 3,
        Staff = 4
    }

    public enum EnumLoginFrom
    {
        Internal,  // internal login
        External   // SNS login (google, facebook etc)
    }

    public enum EnumEducationLevel
    {
        HighSchool = 1,
        College = 2,
        University = 3,
        Other = 10
    }

    public enum EnumJobApplyStatus
    {
        Applied = 1,
        Holding = 2
    }

    public enum EnumJobSeekerStatus
    {
        Open = 1,
        Close = 2
    }

    public enum EnumTypeOfNotification
    {
        Email = 1,
        SMS = 2
    }


    public enum EnumLang
    {
        English = 1,
        French = 2
    }

    public enum EnumTypeOfNotificationStatus
    {
        NotSent = 1,
        Sent = 2,
        Error = 3
    }

    public enum EnumTypeOfPhone
    {
        HomePhone = 1,
        OfficePhone = 2,
        MobilePhone = 3
    }

    public enum EnumTypeOfWorkAvailability
    {
        Weekday = 1,
        Shift = 2
    }

    public enum EnumFilePostType
    {
        // for job seeker
        Resume,
        SecurityLicenseCertificate,
        CPRFirstAidCertificate,
        EligibilityFile,
        EducationFile,

        // for job posting,
        JobPosting
    }

    // it matches with NotificationStatusType table
    public enum NotificationStatusType
    {
        NotSent = 1,
        Sent = 2,
        Error = 3
    }

    public enum EnumFrequency
    {
        Daily = 1,
        Weekly = 7,
        Monthly = 30
    }

    public enum EnumDistance
    {
        Fifteen = 15,
        Fourty = 40,
        FiftyFive = 55
    }

    public enum EnumJobStatus
    {
        Requested = 1,
        Posted = 2,
        Applied = 3,
        Hired = 4,
        Removed = 5,
        Expired = 6
    }

    // note: it matches with Notification table
    public enum EnumNotificationType
    {
        JobRequested   = 1,
        JobPosted	   = 2,
        JobApplied	   = 3,
        SeekerHired	   = 4,
        JobExpired	   = 5,
        JobDeleted	   = 6,
        Register	   = 7,
        ForgotPassword = 8,
        EmailActivation= 9,
        JobAlert	   = 10
    }

    public enum CompanyEnum
    {
        // should be match with "Company" table
        Paragon = 18
    }

    public enum SearchControlDirection
    {
        Vertical = 1,
        Horizontal = 2
    }

    public static class EducationLevelExtensions
    {
        public static string ToShortString(this EnumEducationLevel target)
        {
            if (target == EnumEducationLevel.HighSchool)
                return "High School";

            return target.ToString();
        }
    }
}
