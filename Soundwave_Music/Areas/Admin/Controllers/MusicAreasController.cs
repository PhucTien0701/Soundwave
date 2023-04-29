using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class MusicAreasController : BaseController
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        public ActionResult MusicAreasIndex(string search_area, string show_area, int? _sizepage, int? _pagenumber, string sort_area)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_area_in_trash = _appdb.Areas.Count(a => a.Area_status == "2");
                    ViewBag.Current_sort = sort_area;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_area == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortByAreaName = sort_area == "areaname_asc" ? "areaname_desc" : "areaname_asc";
                    var list_music_area = from a in _appdb.Areas
                                          where (a.Area_status == "1" || a.Area_status == "0")
                                          orderby a.Area_id descending
                                          select a;
                    switch (sort_area)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_music_area = from a in _appdb.Areas
                                           where (a.Area_status == "1" || a.Area_status == "0")
                                           orderby a.Area_id descending
                                           select a;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_music_area = from a in _appdb.Areas
                                           where (a.Area_status == "1" || a.Area_status == "0")
                                           orderby a.Area_id
                                           select a;
                            break;

                        case "areaname_desc":
                            ViewBag.sortname = "Sort by: Music area name (Z-A)";
                            list_music_area = from a in _appdb.Areas
                                           where (a.Area_status == "1" || a.Area_status == "0")
                                           orderby a.Area_name descending
                                           select a;
                            break;

                        case "arename_asc":
                            ViewBag.sortname = "Sort by: Music area name (A-Z)";
                            list_music_area = from a in _appdb.Areas
                                           where (a.Area_status == "1" || a.Area_status == "0")
                                           orderby a.Area_name
                                           select a;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_area)) return View(list_music_area.ToPagedList(pageNum, pageSize));
                    switch (show_area)
                    {
                        //case 1: search all
                        case "1":
                            list_music_area = (IOrderedQueryable<Area>)list_music_area.Where(a => a.Area_id.ToString().Contains(search_area) ||
                                                                                             a.Area_name.Contains(search_area));
                            break;
                        //case 2: search by id
                        case "2":
                            list_music_area = (IOrderedQueryable<Area>)list_music_area.Where(a => a.Area_id.ToString().Contains(search_area));
                            break;
                        //case 3: search by name
                        case "3":
                            list_music_area = (IOrderedQueryable<Area>)list_music_area.Where(a => a.Area_name.Contains(search_area));
                            break;
                    }
                    return View(list_music_area.ToPagedList(pageNum, pageSize));
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

        //Area Trash
        public ActionResult MusicAreasTrash(string search_area, string show_area, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var list_music_area = from a in _appdb.Areas
                                          where a.Area_status == "2"
                                          orderby a.Area_id descending
                                          select a;
                    if (!string.IsNullOrEmpty(search_area))
                    {
                        //search all
                        if (show_area.Equals("1"))
                            list_music_area = (IOrderedQueryable<Area>)list_music_area.Where(a => a.Area_id.ToString().Contains(search_area) || a.Area_name.Contains(search_area));
                        //search by id
                        else if (show_area.Equals("2"))
                            list_music_area = (IOrderedQueryable<Area>)list_music_area.Where(a => a.Area_id.ToString().Contains(search_area));
                        //search by full name
                        else if (show_area.Equals("3"))
                            list_music_area = (IOrderedQueryable<Area>)list_music_area.Where(a => a.Area_name.Contains(search_area));
                        return View("MusicAreasTrash", list_music_area.ToPagedList(pageNum, 50));
                    }
                    return View(list_music_area.ToPagedList(pageNum, pageSize));
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

        //Search Suggestion
        [HttpPost]
        public JsonResult SuggestAreasSearch(string Prefix)
        {
            var search_area = (from a in _appdb.Areas
                               where a.Area_status != "2" && a.Area_name.StartsWith(Prefix)
                               orderby a.Area_name ascending
                               select new { a.Area_name });
            return Json(search_area, JsonRequestBehavior.AllowGet);
        }

        //Create new area view
        public ActionResult MusicAreasCreate()
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
                    return RedirectToAction("MusicAreasIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new area code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MusicAreasCreate(Area area)
        {
            try
            {
                area.Area_name = area.Area_name;
                area.Image = area.Image;
                area.Create_date = DateTime.Now;
                area.Create_by = User.Identity.GetName();
                area.Area_status = area.Area_status;
                area.Update_date = DateTime.Now;
                _appdb.Areas.Add(area);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new music area: " + area.Area_name + "", "success");
                return RedirectToAction("MusicAreasIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(area);
        }

        //Area Edit
        public ActionResult MusicAreasEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("MusicAreasEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var area = _appdb.Areas.SingleOrDefault(a => a.Area_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (area.Area_status == "1" || area.Area_status == "0"))
                {
                    if (area == null || id == null)
                    {
                        Notification.set_noti("This area is not exist: " + area.Area_name + "", "warning");
                        return RedirectToAction("MusicAreasIndex");
                    }
                    return View(area);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("MusicAreasIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MusicAreasEdit(Area area, string returnUrl)
        {
            try
            {
                area.Area_name = area.Area_name;
                area.Image = area.Image;
                area.Create_date = DateTime.Now;
                area.Create_by = User.Identity.GetName();
                area.Area_status = area.Area_status;
                area.Update_date = DateTime.Now;
                _appdb.Entry(area).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update music area successfully: " + area.Area_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("MusicAreasIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(area);
        }

        //Area Detail
        public ActionResult MusicAreasDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_music_area = (from a in _appdb.Areas
                                           where a.Area_id == id
                                           orderby a.Create_date descending
                                           select a).FirstOrDefault();
                    if (list_music_area != null && id != null) return View(list_music_area);
                    Notification.set_noti("This area is not exist: " + list_music_area.Area_name + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("MusicAreasIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }
        
        //Move area to trash
        public ActionResult MoveMusicAreasToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var area = _appdb.Areas.SingleOrDefault(a => a.Area_id == id);
                if (area == null || id == null)
                {
                    Notification.set_noti("This area is not exist: " + area.Area_name + "", "warning");
                    return RedirectToAction("MusicAreaIndex");
                }
                area.Area_status = "2";
                area.Update_date = DateTime.Now;
                _appdb.Entry(area).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable area " + area.Area_name + "sucessfully.", "success");
                return RedirectToAction("MusicAreasIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("MusicAreasIndex");
            }
        }

        //Undo Area From Trash
        public ActionResult UndoAreasFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var area = _appdb.Areas.SingleOrDefault(a => a.Area_id == id);
                if (area == null || id == null)
                {
                    Notification.set_noti("This area is not exist: " + area.Area_name + "", "warning");
                    return RedirectToAction("MusicAreaIndex");
                }
                area.Area_status = "1";
                area.Update_date = DateTime.Now;
                _appdb.Entry(area).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo area " + area.Area_name + "sucessfully.", "success");
                return RedirectToAction("MusicAreasTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("MusicAreasIndex");
            }
        }

        // Delete music areas
        public ActionResult MusicAreasDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = _appdb.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        [HttpPost, ActionName("MusicAreasDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Area area = _appdb.Areas.Find(id);
            _appdb.Areas.Remove(area);
            _appdb.SaveChanges();
            return RedirectToAction("MusicAreasTrash");
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