using RecruitingPortal.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class ChartViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public GoogleVisualizationDataTable DataTable { get; set; }
        public GoogleVisualizationDataTable DataTable2 { get; set; } // in some case (diff chart), passing two datatables
    }
}