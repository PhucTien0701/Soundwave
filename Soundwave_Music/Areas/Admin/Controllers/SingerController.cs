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
    public class SingerController : BaseController
    {
        SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Singer index
        public ActionResult SingerIndex(string search_singer, string show_singer, int? _sizepage, int? _pagenumber, string sort_singer)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_singer_in_trash = _appdb.Singers.Count(g => g.Singer_status == "2");
                    ViewBag.Current_sort = sort_singer;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_singer == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortBySingerName = sort_singer == "singername_asc" ? "singername_desc" : "singername_asc";
                    var list_singer = from s in _appdb.Singers
                                     where (s.Singer_status == "1" || s.Singer_status == "0")
                                     orderby s.Singer_id descending
                                     select s;
                    switch (sort_singer)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_singer = from s in _appdb.Singers
                                         where (s.Singer_status == "1" || s.Singer_status == "0")
                                         orderby s.Singer_id descending
                                         select s;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_singer = from s in _appdb.Singers
                                         where (s.Singer_status == "1" || s.Singer_status == "0")
                                         orderby s.Singer_id
                                         select s;
                            break;

                        case "singername_desc":
                            ViewBag.sortname = "Sort by: Singer name (Z-A)";
                            list_singer = from s in _appdb.Singers
                                         where (s.Singer_status == "1" || s.Singer_status == "0")
                                         orderby s.Singer_name descending
                                         select s;
                            break;

                        case "singername_asc":
                            ViewBag.sortname = "Sort by: Singer name (A-Z)";
                            list_singer = from s in _appdb.Singers
                                         where (s.Singer_status == "1" || s.Singer_status == "0")
                                         orderby s.Singer_name
                                         select s;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_singer)) return View(list_singer.ToPagedList(pageNum, pageSize));
                    switch (show_singer)
                    {
                        //case 1: search all
                        case "1":
                            list_singer = (IOrderedQueryable<Singer>)list_singer.Where(s => s.Singer_id.ToString().Contains(search_singer) ||
                                                                                            s.Singer_name.Contains(search_singer));
                            break;
                        //case 2: search by id
                        case "2":
                            list_singer = (IOrderedQueryable<Singer>)list_singer.Where(s => s.Singer_id.ToString().Contains(search_singer));
                            break;
                        //case 3: search by name
                        case "3":
                            list_singer = (IOrderedQueryable<Singer>)list_singer.Where(s => s.Singer_name.Contains(search_singer));
                            break;
                    }
                    return View(list_singer.ToPagedList(pageNum, pageSize));
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

        //Singer Trash
        public ActionResult SingerTrash(string search_singer, string show_singer, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var list_singer = from s in _appdb.Singers
                                     where s.Singer_status == "2"
                                     orderby s.Singer_id descending
                                     select s;
                    if (!string.IsNullOrEmpty(search_singer))
                    {
                        //search all
                        if (show_singer.Equals("1"))
                            list_singer = (IOrderedQueryable<Singer>)list_singer.Where(s => s.Singer_id.ToString().Contains(search_singer) || s.Singer_name.Contains(search_singer));
                        //search by id
                        else if (show_singer.Equals("2"))
                            list_singer = (IOrderedQueryable<Singer>)list_singer.Where(s => s.Singer_id.ToString().Contains(search_singer));
                        //search by full name
                        else if (show_singer.Equals("3"))
                            list_singer = (IOrderedQueryable<Singer>)list_singer.Where(s => s.Singer_name.Contains(search_singer));
                        return View("SingerTrash", list_singer.ToPagedList(pageNum, 50));
                    }
                    return View(list_singer.ToPagedList(pageNum, pageSize));
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

        //Search Singer Suggestion
        [HttpPost]
        public JsonResult SuggestSingerSearch(string Prefix)
        {
            var search_singer = (from s in _appdb.Singers
                                where s.Singer_status != "2" && s.Singer_name.StartsWith(Prefix)
                                orderby s.Singer_name ascending
                                select new { s.Singer_name });
            return Json(search_singer, JsonRequestBehavior.AllowGet);
        }

        //Create new Singer view
        public ActionResult SingerCreate()
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
                    return RedirectToAction("SingerIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Singer code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingerCreate(Singer singer)
        {
            try
            {
                singer.Singer_name = singer.Singer_name;
                singer.Image = singer.Image;
                singer.Create_date = DateTime.Now;
                singer.Create_by = User.Identity.GetName();
                singer.Singer_status = singer.Singer_status;
                singer.Area_id = singer.Area_id;
                _appdb.Singers.Add(singer);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new singer: " + singer.Singer_name + "", "success");
                return RedirectToAction("SingerIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(singer);
        }

        //Singer Edit
        public ActionResult SingerEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("SingerEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var singer = _appdb.Singers.SingleOrDefault(s => s.Singer_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (singer.Singer_status == "1" || singer.Singer_status == "0"))
                {
                    if (singer == null || id == null)
                    {
                        Notification.set_noti("This singer is not exist: " + singer.Singer_name + "", "warning");
                        return RedirectToAction("SingerIndex");
                    }
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
                    return View(singer);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("SingerIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingerEdit(Singer singer, string returnUrl)
        {
            try
            {
                singer.Singer_name = singer.Singer_name;
                singer.Image = singer.Image;
                singer.Create_date = DateTime.Now;
                singer.Create_by = User.Identity.GetName();
                singer.Singer_status = singer.Singer_status;
                singer.Area_id = singer.Area_id;
                _appdb.Entry(singer).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update singer successfully: " + singer.Singer_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("SingerIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(singer);
        }

        //Singer Detail
        public ActionResult SingerDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_singer = (from s in _appdb.Singers
                                      where s.Singer_id == id
                                      orderby s.Create_date descending
                                      select s).FirstOrDefault();
                    if (list_singer != null && id != null) return View(list_singer);
                    Notification.set_noti("This singer is not exist: " + list_singer.Singer_name + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("SingerIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Move Singer to trash
        public ActionResult MoveSingerToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var singer = _appdb.Singers.SingleOrDefault(s => s.Singer_id == id);
                if (singer == null || id == null)
                {
                    Notification.set_noti("This singer is not exist: " + singer.Singer_name + "", "warning");
                    return RedirectToAction("SingerIndex");
                }
                singer.Singer_status = "2";
                _appdb.Entry(singer).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable singer " + singer.Singer_name + "sucessfully.", "success");
                return RedirectToAction("SingerIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("SingerIndex");
            }
        }

        //Undo Singer From Trash
        public ActionResult UndoSingerFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var singer = _appdb.Singers.SingleOrDefault(s => s.Singer_id == id);
                if (singer == null || id == null)
                {
                    Notification.set_noti("This singer is not exist: " + singer.Singer_name + "", "warning");
                    return RedirectToAction("SingerIndex");
                }
                singer.Singer_status = "1";
                _appdb.Entry(singer).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo singer " + singer.Singer_name + "sucessfully.", "success");
                return RedirectToAction("SingerTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("SingerIndex");
            }
        }

        // Delete Singer
        public ActionResult SingerDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Singer singer = _appdb.Singers.Find(id);
            if (singer == null)
            {
                return HttpNotFound();
            }
            return View(singer);
        }

        [HttpPost, ActionName("SingerDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Singer singer = _appdb.Singers.Find(id);
            _appdb.Singers.Remove(singer);
            _appdb.SaveChanges();
            return RedirectToAction("SingerTrash");
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