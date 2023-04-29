using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class APIController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Supplier index
        public ActionResult APIIndex(int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    var list_api = from a in _appdb.API_Key                                        
                                   orderby a.id descending
                                   select a;
                    return View(list_api.ToPagedList(pageNum, pageSize));
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new supplier view
        public ActionResult APICreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    return View();
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this function", "danger");
                    return RedirectToAction("APIIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new supplier code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult APICreate(API_Key api)
        {
            try
            {
                api.api_name = api.api_name;
                api.client_id = api.client_id;
                api.client_secret = api.client_secret;
                api.update_date = DateTime.Now;
                _appdb.API_Key.Add(api);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new api key: " + api.id + "", "success");
                return RedirectToAction("APIIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(api);
        }

        //Supplier Edit
        public ActionResult APIEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("APIEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var api = _appdb.API_Key.SingleOrDefault(a => a.id == id);
                if (User.Identity.Permiss_Edit() == true)
                {
                    if (api == null || id == null)
                    {
                        Notification.set_noti("This supplier is not exist: " + api.api_name + "", "warning");
                        return RedirectToAction("APIIndex");
                    }
                    return View(api);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("APIIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult APIEdit(API_Key api, string returnUrl)
        {
            try
            {
                api.api_name = api.api_name;
                api.client_id = api.client_id;
                api.client_secret = api.client_secret;
                api.update_date = DateTime.Now;
                _appdb.Entry(api).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update supplier successfully: " + api.api_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("APIIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(api);
        }

        //change status
        [HttpPost]
        public JsonResult ChangeStatus(int id, bool state = false)
        {
            bool result;
            if (User.Identity.GetRole() == 1)
            {
                API_Key api = _appdb.API_Key.FirstOrDefault(m => m.id == id);
                api.active = state;
                api.update_date = DateTime.Now;
                _appdb.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Delete supplier
        public ActionResult APIDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            API_Key api = _appdb.API_Key.Find(id);
            if (api == null)
            {
                return HttpNotFound();
            }
            return View(api);
        }

        [HttpPost, ActionName("APIDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            API_Key api = _appdb.API_Key.Find(id);
            _appdb.API_Key.Remove(api);
            _appdb.SaveChanges();
            return RedirectToAction("APIIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _appdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}