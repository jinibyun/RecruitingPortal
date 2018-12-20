using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    public class TinyMCEController : Controller
    {
        // An action to display your TinyMCE editor
        public ActionResult Index()
        {
            return View();
        }

        // An action that will accept your Html Content
        [HttpPost]
        public ActionResult Index(ExampleClass model)
        {
            return View();
        }
    }

    // An example class to store your values
    public class ExampleClass
    {
        // This attributes allows your HTML Content to be sent up
        [AllowHtml]
        [Required]
        public string HtmlContent { get; set; }

        public ExampleClass()
        {

        }
    }
}