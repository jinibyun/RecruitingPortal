using RecruitingPortal.BLL;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.Domain;
using System.Collections.Generic;
using System.Linq;

namespace RecruitingPortal.Infrastructure
{
    public class CommonLibrary
    {
        public EnumJobStatus GetJobStatus(bool isHired, bool isRemoved, bool isExpired, bool isApplied, bool isPosted)
        {
            // 1. HIRED, REMOVED and EXPIRED is close status
            // 2. REQUESTED, POSTED and APPLIED is open status

            // Requested = 1,
            // Posted = 2,
            // Applied = 3,
            // Hired = 4,
            // Removed = 5,
            // Expired = 6

            // NOTE: order of defining is important
            var result = EnumJobStatus.Requested; // default
            if (isHired)
            {
                result = EnumJobStatus.Hired;
            }
            else
            {
                if(isRemoved)
                {
                    result = EnumJobStatus.Removed;
                }
                else
                {
                    if(isExpired)
                    {
                        result = EnumJobStatus.Expired;
                    }
                    else
                    {
                        if(isApplied)
                        {
                            result = EnumJobStatus.Applied;
                        }
                        else
                        {
                            if(isPosted)
                            {
                                result = EnumJobStatus.Posted;
                            }
                        }
                    }
                }
            }                        
            return result;
        }
    }
}