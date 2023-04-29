using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class ComposerController : BaseController
    {
        SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Composer index
        public ActionResult ComposerIndex(string search_composer, string show_composer, int? _sizepage, int? _pagenumber, string sort_composer)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_composer_in_trash = _appdb.Composers.Count(c => c.Composer_status == "2");
                    ViewBag.Current_sort = sort_composer;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_composer == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortByComposerName = sort_composer == "composername_asc" ? "composername_desc" : "composername_asc";
                    var list_composer = from c in _appdb.Composers
                                      where (c.Composer_status == "1" || c.Composer_status == "0")
                                      orderby c.Composer_id descending
                                      select c;
                    switch (sort_composer)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_composer = from c in _appdb.Composers
                                          where (c.Composer_status == "1" || c.Composer_status == "0")
                                          orderby c.Composer_id descending
                                          select c;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_composer = from c in _appdb.Composers
                                          where (c.Composer_status == "1" || c.Composer_status == "0")
                                          orderby c.Composer_id
                                          select c;
                            break;

                        case "composername_desc":
                            ViewBag.sortname = "Sort by: Composer name (Z-A)";
                            list_composer = from c in _appdb.Composers
                                          where (c.Composer_status == "1" || c.Composer_status == "0")
                                          orderby c.Composer_name descending
                                          select c;
                            break;

                        case "composername_asc":
                            ViewBag.sortname = "Sort by: Composer name (A-Z)";
                            list_composer = from c in _appdb.Composers
                                          where (c.Composer_status == "1" || c.Composer_status == "0")
                                          orderby c.Composer_name
                                          select c;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_composer)) return View(list_composer.ToPagedList(pageNum, pageSize));
                    switch (show_composer)
                    {
                        //case 1: search all
                        case "1":
                            list_composer = (IOrderedQueryable<Composer>)list_composer.Where(c => c.Composer_id.ToString().Contains(search_composer) ||
                                                                                            c.Composer_name.Contains(search_composer));
                            break;
                        //case 2: search by id
                        case "2":
                            list_composer = (IOrderedQueryable<Composer>)list_composer.Where(c => c.Composer_id.ToString().Contains(search_composer));
                            break;
                        //case 3: search by name
                        case "3":
                            list_composer = (IOrderedQueryable<Composer>)list_composer.Where(c => c.Composer_name.Contains(search_composer));
                            break;
                    }
                    return View(list_composer.ToPagedList(pageNum, pageSize));
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

        //Composer Trash
        public ActionResult ComposerTrash(string search_composer, string show_composer, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var list_composer = from c in _appdb.Composers
                                      where c.Composer_status == "2"
                                      orderby c.Composer_id descending
                                      select c;
                    if (!string.IsNullOrEmpty(search_composer))
                    {
                        //search all
                        if (show_composer.Equals("1"))
                            list_composer = (IOrderedQueryable<Composer>)list_composer.Where(c => c.Composer_id.ToString().Contains(search_composer) || c.Composer_name.Contains(search_composer));
                        //search by id
                        else if (show_composer.Equals("2"))
                            list_composer = (IOrderedQueryable<Composer>)list_composer.Where(c => c.Composer_id.ToString().Contains(search_composer));
                        //search by full name
                        else if (show_composer.Equals("3"))
                            list_composer = (IOrderedQueryable<Composer>)list_composer.Where(c => c.Composer_name.Contains(search_composer));
                        return View("ComposerTrash", list_composer.ToPagedList(pageNum, 50));
                    }
                    return View(list_composer.ToPagedList(pageNum, pageSize));
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

        //Search Composer Suggestion
        [HttpPost]
        public JsonResult SuggestComposerSearch(string Prefix)
        {
            var search_composer = (from c in _appdb.Composers
                                 where c.Composer_status != "2" && c.Composer_name.StartsWith(Prefix)
                                 orderby c.Composer_name ascending
                                 select new { c.Composer_name });
            return Json(search_composer, JsonRequestBehavior.AllowGet);
        }

        //Create new Composer view
        public ActionResult ComposerCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
                    return View();
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this function", "danger");
                    return RedirectToAction("ComposerIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Composer code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ComposerCreate(Composer composer)
        {
            try
            {
                composer.Composer_name = composer.Composer_name;
                composer.Image = composer.Image;
                composer.Create_date = DateTime.Now;
                composer.Create_by = User.Identity.GetName();
                composer.Composer_status = composer.Composer_status;
                composer.Area_id = composer.Area_id;
                _appdb.Composers.Add(composer);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new composer: " + composer.Composer_name + "", "success");
                return RedirectToAction("ComposerIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(composer);
        }

        //Composer Edit
        public ActionResult ComposerEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("ComposerEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var composer = _appdb.Composers.SingleOrDefault(c => c.Composer_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (composer.Composer_status == "1" || composer.Composer_status == "0"))
                {
                    if (composer == null || id == null)
                    {
                        Notification.set_noti("This composer is not exist: " + composer.Composer_name + "", "warning");
                        return RedirectToAction("ComposerIndex");
                    }
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
                    return View(composer);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("ComposerIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ComposerEdit(Composer composer, string returnUrl)
        {
            try
            {
                composer.Composer_name = composer.Composer_name;
                composer.Image = composer.Image;
                composer.Create_date = DateTime.Now;
                composer.Create_by = User.Identity.GetName();
                composer.Composer_status = composer.Composer_status;
                composer.Area_id = composer.Area_id;
                _appdb.Entry(composer).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update composer successfully: " + composer.Composer_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("ComposerIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(composer);
        }

        //Composer Detail
        public ActionResult ComposerDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_composer = (from c in _appdb.Composers
                                         where c.Composer_id == id
                                         orderby c.Create_date descending
                                         select c).FirstOrDefault();
                    if (list_composer != null && id != null) return View(list_composer);
                    Notification.set_noti("This composer is not exist: " + list_composer.Composer_name + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("ComposerIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Move Composer to trash
        public ActionResult MoveComposerToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var composer = _appdb.Composers.SingleOrDefault(c => c.Composer_id == id);
                if (composer == null || id == null)
                {
                    Notification.set_noti("This composer is not exist: " + composer.Composer_name + "", "warning");
                    return RedirectToAction("ComposerIndex");
                }
                composer.Composer_status = "2";
                _appdb.Entry(composer).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable composer " + composer.Composer_name + "sucessfully.", "success");
                return RedirectToAction("ComposerIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("ComposerIndex");
            }
        }

        //Undo Composer From Trash
        public ActionResult UndoComposerFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var composer = _appdb.Composers.SingleOrDefault(c => c.Composer_id == id);
                if (composer == null || id == null)
                {
                    Notification.set_noti("This composer is not exist: " + composer.Composer_name + "", "warning");
                    return RedirectToAction("ComposerIndex");
                }
                composer.Composer_status = "1";
                _appdb.Entry(composer).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo composer " + composer.Composer_name + "sucessfully.", "success");
                return RedirectToAction("ComposerTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("ComposerIndex");
            }
        }

        // Delete Composer
        public ActionResult ComposerDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Composer composer = _appdb.Composers.Find(id);
            if (composer == null)
            {
                return HttpNotFound();
            }
            return View(composer);
        }

        [HttpPost, ActionName("ComposerDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Composer composer = _appdb.Composers.Find(id);
            _appdb.Composers.Remove(composer);
            _appdb.SaveChanges();
            return RedirectToAction("composerTrash");
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