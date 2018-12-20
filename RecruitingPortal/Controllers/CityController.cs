using RecruitingPortal.BLL.Service;
using RecruitingPortal.Domain;
using RecruitingPortal.Models;
using System.Linq;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    public class CityController : BaseController
    {
        public CityController(PortalService<city> service)
        {
            cityService = service;
        }
        // GET: City
        public PartialViewResult List(int CityId = 0)
        {
            var viewModel = new CityViewModel();
            var list = cityService.Get(x => x.CountryID == 43).OrderByDescending(y => y.City1); // 43: Canada

            viewModel.ItemsInDropDown = list.Select(x => new SelectListItem { Text = x.City1, Value = x.CityId.ToString() }).AsEnumerable<SelectListItem>();// as IEnumerable<SelectListItem>;            
            return PartialView("_City", viewModel);
        }
    }
}