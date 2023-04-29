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
    public class AlbumController : Controller
    {
        SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Album index
        public ActionResult AlbumIndex(string search_album, string show_album, int? _sizepage, int? _pagenumber, string sort_album)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_album_in_trash = _appdb.Albums.Count(a => a.Album_status == "2");
                    ViewBag.Current_sort = sort_album;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_album == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortByAlbumName = sort_album == "albumname_asc" ? "albumname_desc" : "albumname_asc";
                    var list_album = from a in _appdb.Albums
                                     where (a.Album_status == "1" || a.Album_status == "0")
                                     orderby a.Album_id descending
                                     select a;
                    switch (sort_album)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_album = from a in _appdb.Albums
                                         where (a.Album_status == "1" || a.Album_status == "0")
                                         orderby a.Album_id descending
                                         select a;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_album = from a in _appdb.Albums
                                         where (a.Album_status == "1" || a.Album_status == "0")
                                         orderby a.Album_id
                                         select a;
                            break;

                        case "albumname_desc":
                            ViewBag.sortname = "Sort by: Album name (Z-A)";
                            list_album = from a in _appdb.Albums
                                         where (a.Album_status == "1" || a.Album_status == "0")
                                         orderby a.Album_name descending
                                         select a;
                            break;

                        case "albumname_asc":
                            ViewBag.sortname = "Sort by: Album name (A-Z)";
                            list_album = from a in _appdb.Albums
                                         where (a.Album_status == "1" || a.Album_status == "0")
                                         orderby a.Album_name
                                         select a;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_album)) return View(list_album.ToPagedList(pageNum, pageSize));
                    switch (show_album)
                    {
                        //case 1: search all
                        case "1":
                            list_album = (IOrderedQueryable<Album>)list_album.Where(a => a.Album_id.ToString().Contains(search_album) ||
                                                                                         a.Album_name.Contains(search_album));
                            break;
                        //case 2: search by id
                        case "2":
                            list_album = (IOrderedQueryable<Album>)list_album.Where(a => a.Album_id.ToString().Contains(search_album));
                            break;
                        //case 3: search by name
                        case "3":
                            list_album = (IOrderedQueryable<Album>)list_album.Where(a => a.Album_name.Contains(search_album));
                            break;
                    }
                    return View(list_album.ToPagedList(pageNum, pageSize));
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

        //Album Trash
        public ActionResult AlbumTrash(string search_album, string show_album, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var list_album = from a in _appdb.Albums
                                     where a.Album_status == "2"
                                     orderby a.Album_id descending
                                     select a;
                    if (!string.IsNullOrEmpty(search_album))
                    {
                        //search all
                        if (show_album.Equals("1"))
                            list_album = (IOrderedQueryable<Album>)list_album.Where(a => a.Album_id.ToString().Contains(search_album) || a.Album_name.Contains(search_album));
                        //search by id
                        else if (show_album.Equals("2"))
                            list_album = (IOrderedQueryable<Album>)list_album.Where(a => a.Album_id.ToString().Contains(search_album));
                        //search by full name
                        else if (show_album.Equals("3"))
                            list_album = (IOrderedQueryable<Album>)list_album.Where(a => a.Album_name.Contains(search_album));
                        return View("AlbumTrash", list_album.ToPagedList(pageNum, 50));
                    }
                    return View(list_album.ToPagedList(pageNum, pageSize));
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

        //Search Album Suggestion
        [HttpPost]
        public JsonResult SuggestAlbumSearch(string Prefix)
        {
            var search_album = (from a in _appdb.Albums
                                   where a.Album_status != "2" && a.Album_name.StartsWith(Prefix)
                                   orderby a.Album_name ascending
                                   select new { a.Album_name });
            return Json(search_album, JsonRequestBehavior.AllowGet);
        }

        //Create new Album view
        public ActionResult AlbumCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
                    ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
                    ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
                    return View();
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this function", "danger");
                    return RedirectToAction("AlbumIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Album code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlbumCreate(Album album)
        {
            ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
            ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
            ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
            try
            {
                album.Album_name = album.Album_name;
                album.Image = album.Image;
                album.Create_date = DateTime.Now;
                album.Create_by = User.Identity.GetName();
                album.Album_status = album.Album_status;
                album.Genre_id = album.Genre_id;
                album.Singer_id = album.Singer_id;
                album.Area_id = album.Area_id;
                _appdb.Albums.Add(album);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new album: " + album.Album_name + "", "success");
                return RedirectToAction("AlbumIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(album);
        }

        //Album Edit
        public ActionResult AlbumEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("AlbumEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var album = _appdb.Albums.SingleOrDefault(c => c.Album_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (album.Album_status == "1" || album.Album_status == "0"))
                {
                    if (album == null || id == null)
                    {
                        Notification.set_noti("This album is not exist: " + album.Album_name + "", "warning");
                        return RedirectToAction("AlbumIndex");
                    }
                    ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
                    ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
                    return View(album);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("AlbumIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlbumEdit(Album album, string returnUrl)
        {
            ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
            ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
            ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
            try
            {
                album.Album_name = album.Album_name;
                album.Image = album.Image;
                album.Create_date = DateTime.Now;
                album.Create_by = User.Identity.GetName();
                album.Album_status = album.Album_status;
                album.Genre_id = album.Genre_id;
                album.Area_id = album.Area_id;
                _appdb.Entry(album).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update album successfully: " + album.Album_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("AlbumIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(album);
        }

        //Album Detail
        public ActionResult AlbumDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_album = (from a in _appdb.Albums
                                      where a.Album_id == id
                                      orderby a.Create_date descending
                                      select a).FirstOrDefault();
                    if (list_album != null && id != null) return View(list_album);
                    Notification.set_noti("This album is not exist: " + list_album.Album_name + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("AlbumIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Move Album to trash
        public ActionResult MoveAlbumToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var album = _appdb.Albums.SingleOrDefault(a => a.Album_id == id);
                if (album == null || id == null)
                {
                    Notification.set_noti("This album is not exist: " + album.Album_name + "", "warning");
                    return RedirectToAction("AlbumIndex");
                }
                album.Album_status = "2";
                _appdb.Entry(album).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable album " + album.Album_name + "sucessfully.", "success");
                return RedirectToAction("albumIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("albumIndex");
            }
        }

        //Undo Album From Trash
        public ActionResult UndoAlbumFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var album = _appdb.Albums.SingleOrDefault(a => a.Album_id == id);
                if (album == null || id == null)
                {
                    Notification.set_noti("This album is not exist: " + album.Album_name + "", "warning");
                    return RedirectToAction("AlbumIndex");
                }
                album.Album_status = "1";
                _appdb.Entry(album).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo album " + album.Album_name + "sucessfully.", "success");
                return RedirectToAction("AlbumTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("AlbumIndex");
            }
        }

        // Delete Album
        public ActionResult AlbumDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = _appdb.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        [HttpPost, ActionName("AlbumDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = _appdb.Albums.Find(id);
            _appdb.Albums.Remove(album);
            _appdb.SaveChanges();
            return RedirectToAction("AlbumTrash");
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