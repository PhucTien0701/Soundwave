using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            //Base on Const file ADMIN ID = 1; USER ID = 2; VIPUser ID = 3
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/User/SignIn");
            }
            else
            {
                if (System.Web.HttpContext.Current.User.Identity.GetRole() == 2)
                {
                    System.Web.HttpContext.Current.Response.Redirect("~/Home/Index");
                }
            }
        }

        //When admin logout, system will return to home page
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Notification.set_noti("You logout sucessfully", "success");
            return Redirect("~/Home/Index");
        }

        //Change admin page to profile page
        public ActionResult ReturnProfile()
        {
            return Redirect("~/User/UserEditProfile");
        }

        //Change from Admin to homepage
        public ActionResult ReturnHome()
        {
            return Redirect("~/Home/Index");
        }
    }
}