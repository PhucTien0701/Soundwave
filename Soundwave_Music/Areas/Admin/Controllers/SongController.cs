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
    public class SongController : BaseController
    {
        SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Song index
        public ActionResult SongIndex(string search_song, string show_song, int? _sizepage, int? _pagenumber, string sort_song)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_song_in_trash = _appdb.Songs.Count(s => s.Song_status == "2");
                    ViewBag.Current_sort = sort_song;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_song == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortBySongName = sort_song == "songname_asc" ? "songname_desc" : "songname_asc";
                    var list_song = from s in _appdb.Songs
                                    where (s.Song_status == "1" || s.Song_status == "0")
                                    orderby s.Song_id descending
                                    select s;
                    switch (sort_song)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_song = from s in _appdb.Songs
                                        where (s.Song_status == "1" || s.Song_status == "0")
                                        orderby s.Song_id descending
                                        select s;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_song = from s in _appdb.Songs
                                        where (s.Song_status == "1" || s.Song_status == "0")
                                        orderby s.Song_id
                                        select s;
                            break;

                        case "songname_desc":
                            ViewBag.sortname = "Sort by: Song name (Z-A)";
                            list_song = from s in _appdb.Songs
                                        where (s.Song_status == "1" || s.Song_status == "0")
                                        orderby s.Song_name descending
                                        select s;
                            break;

                        case "songname_asc":
                            ViewBag.sortname = "Sort by: Song name (A-Z)";
                            list_song = from s in _appdb.Songs
                                        where (s.Song_status == "1" || s.Song_status == "0")
                                        orderby s.Song_name
                                        select s;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_song)) return View(list_song.ToPagedList(pageNum, pageSize));
                    switch (show_song)
                    {
                        //case 1: search all
                        case "1":
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Song_id.ToString().Contains(search_song) ||
                                                                                      s.Song_name.Contains(search_song) ||
                                                                                      s.Album.Album_name.Contains(search_song) ||
                                                                                      s.Singer.Singer_name.Contains(search_song) ||
                                                                                      s.Composer.Composer_name.Contains(search_song) ||
                                                                                      s.Genre.Genre_name.Contains(search_song));
                            break;
                        //case 2: search by id
                        case "2":
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Song_id.ToString().Contains(search_song));
                            break;
                        //case 3: search by name
                        case "3":
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Song_name.Contains(search_song));
                            break;
                        //case 4: search by album name
                        case "4":
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Album.Album_name.ToString().Contains(search_song));
                            break;
                        //case 5: search by Singer Name
                        case "5":
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Singer.Singer_name.ToString().Contains(search_song));
                            break;
                        //case 6: search by Composer Name
                        case "6":
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Composer.Composer_name.ToString().Contains(search_song));
                            break;
                        //case 7: search by Genre Name
                        case "7":
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Genre.Genre_name.ToString().Contains(search_song));
                            break;
                    }
                    return View(list_song.ToPagedList(pageNum, pageSize));
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

        //Song Trash
        public ActionResult SongTrash(string search_song, string show_song, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var list_song = from s in _appdb.Songs
                                    where s.Song_status == "2"
                                    orderby s.Song_id descending
                                    select s;
                    if (!string.IsNullOrEmpty(search_song))
                    {
                        //search all
                        if (show_song.Equals("1"))
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Song_id.ToString().Contains(search_song) ||
                                                                                 s.Song_name.Contains(search_song) ||
                                                                                 s.Album.Album_name.Contains(search_song) ||
                                                                                 s.Singer.Singer_name.Contains(search_song) ||
                                                                                 s.Composer.Composer_name.Contains(search_song) ||
                                                                                 s.Genre.Genre_name.Contains(search_song));
                        //search by id
                        else if (show_song.Equals("2"))
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Song_id.ToString().Contains(search_song));
                        //search by name
                        else if (show_song.Equals("3"))
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Song_name.Contains(search_song));
                        //search by album name
                        else if (show_song.Equals("4"))
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Album.Album_name.Contains(search_song));
                        //search by singer name
                        else if (show_song.Equals("5"))
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Singer.Singer_name.Contains(search_song));
                        //search by composer name
                        else if (show_song.Equals("6"))
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Composer.Composer_name.Contains(search_song));
                        //search by genre name
                        else if (show_song.Equals("7"))
                            list_song = (IOrderedQueryable<Song>)list_song.Where(s => s.Genre.Genre_name.Contains(search_song));
                        return View("SongTrash", list_song.ToPagedList(pageNum, 50));
                    }
                    return View(list_song.ToPagedList(pageNum, pageSize));
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

        //Search Song Suggestion
        [HttpPost]
        public JsonResult SuggestSongSearch(string Prefix)
        {
            var search_song = (from s in _appdb.Songs
                               where s.Song_status != "2" && s.Song_name.StartsWith(Prefix)
                               orderby s.Song_name ascending
                               select new { s.Song_name });
            return Json(search_song, JsonRequestBehavior.AllowGet);
        }

        //Create new Song view
        public ActionResult SongCreate()
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
                    return RedirectToAction("SongIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Song code
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SongCreate(Song song)
        {
            ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
            ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
            ViewBag.ListComposer = new SelectList(_appdb.Composers.Where(c => (c.Composer_status == "1")).OrderBy(c => c.Composer_name), "Composer_id", "Composer_name", 0);
            ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
            ViewBag.ListSupplier = new SelectList(_appdb.Suppliers.Where(s => (s.Supplier_status == "1")).OrderBy(s => s.Supplier_name), "Supplier_id", "Supplier_name", 0);
            ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
            try
            {
                song.Song_name = song.Song_name;
                song.Image = song.Image;
                song.View_count = 0;
                song.Music_File_Upload = song.Music_File_Upload;
                song.Release_date = DateTime.Now;
                song.Lyric = song.Lyric;
                song.Create_by = User.Identity.GetName();
                song.Song_status = song.Song_status;
                song.Album_id = song.Album_id;
                song.Singer_id = song.Singer_id;
                song.Composer_id = song.Composer_id;
                song.Genre_id = song.Genre_id;
                song.Supplier_id = song.Supplier_id;
                song.Area_id = song.Area_id;
                _appdb.Songs.Add(song);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new song: " + song.Song_name + "", "success");
                return RedirectToAction("SongIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(song);
        }

        //Song Edit
        public ActionResult SongEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("SongEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var song = _appdb.Songs.SingleOrDefault(s => s.Song_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (song.Song_status == "1" || song.Song_status == "0"))
                {
                    if (song == null || id == null)
                    {
                        Notification.set_noti("This song is not exist: " + song.Song_name + "", "warning");
                        return RedirectToAction("SongIndex");
                    }
                    ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
                    ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
                    ViewBag.ListComposer = new SelectList(_appdb.Composers.Where(c => (c.Composer_status == "1")).OrderBy(c => c.Composer_name), "Composer_id", "Composer_name", 0);
                    ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
                    ViewBag.ListSupplier = new SelectList(_appdb.Suppliers.Where(s => (s.Supplier_status == "1")).OrderBy(s => s.Supplier_name), "Supplier_id", "Supplier_name", 0);
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
                    return View(song);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("SongIndex");
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
        public ActionResult SongEdit(Song song, string returnUrl)
        {
            ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
            ViewBag.ListSinger = new SelectList(_appdb.Singers.Where(s => (s.Singer_status == "1")).OrderBy(s => s.Singer_name), "Singer_id", "Singer_name", 0);
            ViewBag.ListComposer = new SelectList(_appdb.Composers.Where(c => (c.Composer_status == "1")).OrderBy(c => c.Composer_name), "Composer_id", "Composer_name", 0);
            ViewBag.ListGenre = new SelectList(_appdb.Genres.Where(g => (g.Genre_status == "1")).OrderBy(g => g.Genre_name), "Genre_id", "Genre_name", 0);
            ViewBag.ListSupplier = new SelectList(_appdb.Suppliers.Where(s => (s.Supplier_status == "1")).OrderBy(s => s.Supplier_name), "Supplier_id", "Supplier_name", 0);
            ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
            try
            {
                song.Song_name = song.Song_name;
                song.Image = song.Image;
                song.Release_date = DateTime.Now;
                song.Create_by = User.Identity.GetName();
                song.Song_status = song.Song_status;
                song.Genre_id = song.Genre_id;
                song.Area_id = song.Area_id;
                _appdb.Entry(song).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update song successfully: " + song.Song_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("SongIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(song);
        }

        //Song Detail
        public ActionResult SongDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_song = (from s in _appdb.Songs
                                      where s.Song_id == id
                                      orderby s.Release_date descending
                                      select s).FirstOrDefault();
                    if (list_song != null && id != null) return View(list_song);
                    Notification.set_noti("This song is not exist: " + list_song.Song_name + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("SongIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Move Song to trash
        public ActionResult MoveSongToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var song = _appdb.Songs.SingleOrDefault(s => s.Song_id == id);
                if (song == null || id == null)
                {
                    Notification.set_noti("This song is not exist: " + song.Song_name + "", "warning");
                    return RedirectToAction("SongIndex");
                }
                song.Song_status = "2";
                _appdb.Entry(song).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable song " + song.Song_name + "sucessfully.", "success");
                return RedirectToAction("SongIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("SongIndex");
            }
        }

        //Undo Song From Trash
        public ActionResult UndoSongFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var song = _appdb.Songs.SingleOrDefault(s => s.Song_id == id);
                if (song == null || id == null)
                {
                    Notification.set_noti("This song is not exist: " + song.Song_name + "", "warning");
                    return RedirectToAction("SongIndex");
                }
                song.Song_status = "1";
                _appdb.Entry(song).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo song " + song.Song_name + "sucessfully.", "success");
                return RedirectToAction("SongTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("SongIndex");
            }
        }

        // Delete Song
        public ActionResult SongDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = _appdb.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        [HttpPost, ActionName("SongDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Song song = _appdb.Songs.Find(id);
            _appdb.Songs.Remove(song);
            _appdb.SaveChanges();
            return RedirectToAction("SongTrash");
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