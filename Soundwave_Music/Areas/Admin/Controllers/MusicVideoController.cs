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
    public class MusicVideoController : BaseController
    {
        SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Video index
        public ActionResult VideoIndex(string search_video, string show_video, int? _sizepage, int? _pagenumber, string sort_video)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_video_in_trash = _appdb.Videos.Count(s => s.Video_status == "2");
                    ViewBag.Current_sort = sort_video;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_video == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortByVideoName = sort_video == "videoname_asc" ? "videoname_desc" : "videoname_asc";
                    var list_video = from s in _appdb.Videos
                                    where (s.Video_status == "1" || s.Video_status == "0")
                                    orderby s.Video_id descending
                                    select s;
                    switch (sort_video)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_video = from s in _appdb.Videos
                                        where (s.Video_status == "1" || s.Video_status == "0")
                                        orderby s.Video_id descending
                                        select s;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_video = from s in _appdb.Videos
                                        where (s.Video_status == "1" || s.Video_status == "0")
                                        orderby s.Video_id
                                        select s;
                            break;

                        case "videoname_desc":
                            ViewBag.sortname = "Sort by: Video name (Z-A)";
                            list_video = from s in _appdb.Videos
                                        where (s.Video_status == "1" || s.Video_status == "0")
                                        orderby s.Video_name descending
                                        select s;
                            break;

                        case "videoname_asc":
                            ViewBag.sortname = "Sort by: Video name (A-Z)";
                            list_video = from s in _appdb.Videos
                                        where (s.Video_status == "1" || s.Video_status == "0")
                                        orderby s.Video_name
                                        select s;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_video)) return View(list_video.ToPagedList(pageNum, pageSize));
                    switch (show_video)
                    {
                        //case 1: search all
                        case "1":
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Video_id.ToString().Contains(search_video) ||
                                                                                      s.Video_name.Contains(search_video) ||                                                                                      
                                                                                      s.Singer.Singer_name.Contains(search_video) ||
                                                                                      s.Composer.Composer_name.Contains(search_video) ||
                                                                                      s.Genre.Genre_name.Contains(search_video));
                            break;
                        //case 2: search by id
                        case "2":
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Video_id.ToString().Contains(search_video));
                            break;
                        //case 3: search by name
                        case "3":
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Video_name.Contains(search_video));
                            break;                        
                        //case 5: search by Singer Name
                        case "4":
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Singer.Singer_name.ToString().Contains(search_video));
                            break;
                        //case 6: search by Composer Name
                        case "5":
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Composer.Composer_name.ToString().Contains(search_video));
                            break;
                        //case 7: search by Genre Name
                        case "6":
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Genre.Genre_name.ToString().Contains(search_video));
                            break;
                    }
                    return View(list_video.ToPagedList(pageNum, pageSize));
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

        //Video Trash
        public ActionResult VideoTrash(string search_video, string show_video, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var list_video = from s in _appdb.Videos
                                    where s.Video_status == "2"
                                    orderby s.Video_id descending
                                    select s;
                    if (!string.IsNullOrEmpty(search_video))
                    {
                        //search all
                        if (show_video.Equals("1"))
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Video_id.ToString().Contains(search_video) ||
                                                                                 s.Video_name.Contains(search_video) ||                                                                                 
                                                                                 s.Singer.Singer_name.Contains(search_video) ||
                                                                                 s.Composer.Composer_name.Contains(search_video) ||
                                                                                 s.Genre.Genre_name.Contains(search_video));
                        //search by id
                        else if (show_video.Equals("2"))
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Video_id.ToString().Contains(search_video));
                        //search by name
                        else if (show_video.Equals("3"))
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Video_name.Contains(search_video));                        
                        //search by singer name
                        else if (show_video.Equals("4"))
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Singer.Singer_name.Contains(search_video));
                        //search by composer name
                        else if (show_video.Equals("5"))
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Composer.Composer_name.Contains(search_video));
                        //search by genre name
                        else if (show_video.Equals("6"))
                            list_video = (IOrderedQueryable<Video>)list_video.Where(s => s.Genre.Genre_name.Contains(search_video));
                        return View("VideoTrash", list_video.ToPagedList(pageNum, 50));
                    }
                    return View(list_video.ToPagedList(pageNum, pageSize));
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

        //Search Video Suggestion
        [HttpPost]
        public JsonResult SuggestVideoSearch(string Prefix)
        {
            var search_video = (from s in _appdb.Videos
                               where s.Video_status != "2" && s.Video_name.StartsWith(Prefix)
                               orderby s.Video_name ascending
                               select new { s.Video_name });
            return Json(search_video, JsonRequestBehavior.AllowGet);
        }

        //Create new Video view
        public ActionResult VideoCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
                    ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
                    ViewBag.ListComposer = new SelectList(_appdb.Composers.Where(c => (c.Composer_status == "1")).OrderBy(c => c.Composer_name), "Composer_id", "Composer_name", 0);
                    ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
                    ViewBag.ListSupplier = new SelectList(_appdb.Suppliers.Where(s => (s.Supplier_status == "1")).OrderBy(s => s.Supplier_name), "Supplier_id", "Supplier_name", 0);
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);

                    return View();
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this function", "danger");
                    return RedirectToAction("VideoIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Video code
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult VideoCreate(Video video)
        {
            ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
            ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
            ViewBag.ListComposer = new SelectList(_appdb.Composers.Where(c => (c.Composer_status == "1")).OrderBy(c => c.Composer_name), "Composer_id", "Composer_name", 0);
            ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
            ViewBag.ListSupplier = new SelectList(_appdb.Suppliers.Where(s => (s.Supplier_status == "1")).OrderBy(s => s.Supplier_name), "Supplier_id", "Supplier_name", 0);
            ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
            try
            {
                video.Video_name = video.Video_name;
                video.Image = video.Image;
                video.View_count = 0;
                video.Video_File_Upload = video.Video_File_Upload;
                video.Release_date = DateTime.Now;
                video.Lyric = video.Lyric;
                video.Create_by = User.Identity.GetName();
                video.Video_status = video.Video_status;
                video.Singer_id = video.Singer_id;
                video.Composer_id = video.Composer_id;
                video.Genre_id = video.Genre_id;
                video.Supplier_id = video.Supplier_id;
                video.Area_id = video.Area_id;
                _appdb.Videos.Add(video);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new video: " + video.Video_name + "", "success");
                return RedirectToAction("VideoIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(video);
        }

        //Video Edit
        public ActionResult VideoEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("VideoEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var video = _appdb.Videos.SingleOrDefault(s => s.Video_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (video.Video_status == "1" || video.Video_status == "0"))
                {
                    if (video == null || id == null)
                    {
                        Notification.set_noti("This video is not exist: " + video.Video_name + "", "warning");
                        return RedirectToAction("VideoIndex");
                    }
                    ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
                    ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
                    ViewBag.ListComposer = new SelectList(_appdb.Composers.Where(c => (c.Composer_status == "1")).OrderBy(c => c.Composer_name), "Composer_id", "Composer_name", 0);
                    ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
                    ViewBag.ListSupplier = new SelectList(_appdb.Suppliers.Where(s => (s.Supplier_status == "1")).OrderBy(s => s.Supplier_name), "Supplier_id", "Supplier_name", 0);
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
                    return View(video);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("VideoIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult VideoEdit(Video video, string returnUrl)
        {
            ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
            ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
            ViewBag.ListComposer = new SelectList(_appdb.Composers.Where(c => (c.Composer_status == "1")).OrderBy(c => c.Composer_name), "Composer_id", "Composer_name", 0);
            ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
            ViewBag.ListSupplier = new SelectList(_appdb.Suppliers.Where(s => (s.Supplier_status == "1")).OrderBy(s => s.Supplier_name), "Supplier_id", "Supplier_name", 0);
            ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
            try
            {
                video.Video_name = video.Video_name;
                video.Image = video.Image;
                video.Release_date = DateTime.Now;
                video.Create_by = User.Identity.GetName();
                video.Video_status = video.Video_status;
                video.Genre_id = video.Genre_id;
                video.Area_id = video.Area_id;
                _appdb.Entry(video).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update video successfully: " + video.Video_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("VideoIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(video);
        }

        //Video Detail
        public ActionResult VideoDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_video = (from s in _appdb.Videos
                                     where s.Video_id == id
                                     orderby s.Release_date descending
                                     select s).FirstOrDefault();
                    if (list_video != null && id != null) return View(list_video);
                    Notification.set_noti("This video is not exist: " + list_video.Video_name + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("VideoIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Move Video to trash
        public ActionResult MoveVideoToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var video = _appdb.Videos.SingleOrDefault(s => s.Video_id == id);
                if (video == null || id == null)
                {
                    Notification.set_noti("This video is not exist: " + video.Video_name + "", "warning");
                    return RedirectToAction("VideoIndex");
                }
                video.Video_status = "2";
                _appdb.Entry(video).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable video " + video.Video_name + "sucessfully.", "success");
                return RedirectToAction("VideoIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("VideoIndex");
            }
        }

        //Undo Video From Trash
        public ActionResult UndoVideoFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var video = _appdb.Videos.SingleOrDefault(s => s.Video_id == id);
                if (video == null || id == null)
                {
                    Notification.set_noti("This video is not exist: " + video.Video_name + "", "warning");
                    return RedirectToAction("VideoIndex");
                }
                video.Video_status = "1";
                _appdb.Entry(video).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo video " + video.Video_name + "sucessfully.", "success");
                return RedirectToAction("VideoTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("VideoIndex");
            }
        }

        // Delete Video
        public ActionResult VideoDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = _appdb.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        [HttpPost, ActionName("VideoDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = _appdb.Videos.Find(id);
            _appdb.Videos.Remove(video);
            _appdb.SaveChanges();
            return RedirectToAction("VideoTrash");
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