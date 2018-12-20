using RecruitingPortal.DAL.Interface;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.Win.Service.BLL
{
    public interface ISendMailService
    {
        void SendMail();        
    }
}
