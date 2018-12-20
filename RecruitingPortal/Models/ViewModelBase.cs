using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class ViewModelBase<T>
    {
        public virtual string PageTitle { get; set; }
        public virtual string ViewHeading { get; set; }

        // public virtual ViewModel ViewMode { get; set; }
        public virtual IList<T> ItemList { get; set; }
        public virtual T DetailItem { get; set; }
    }
}