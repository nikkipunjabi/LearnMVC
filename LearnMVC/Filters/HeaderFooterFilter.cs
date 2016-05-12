using LearnMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearnMVC.Filters
{
    public class HeaderFooterFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //base.OnActionExecuted(filterContext);
            ViewResult v = filterContext.Result as ViewResult;
            if (v != null)
            {
                BaseViewModel bvm = v.Model as BaseViewModel;
                if (bvm != null)
                {
                    bvm.UserName = HttpContext.Current.User.Identity.Name;
                    bvm.FooterData = new FooterViewModel();
                    bvm.FooterData.CompanyName = "Horizontal India";
                    bvm.FooterData.Year = DateTime.Now.Year.ToString();
                }
            }
        }
    }
}