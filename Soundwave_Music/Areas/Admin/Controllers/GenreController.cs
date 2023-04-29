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
    public class GenreController : BaseController
    {
        SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Genre index
        public ActionResult GenreIndex(string search_genre, string show_genre, int? _sizepage, int? _pagenumber, string sort_genre)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_genre_in_trash = _appdb.Genres.Count(g => g.Genre_status == "2");
                    ViewBag.Current_sort = sort_genre;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_genre == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortByGenreName = sort_genre == "genrename_asc" ? "genrename_desc" : "genrename_asc";
                    var list_genre = from g in _appdb.Genres
                                        where (g.Genre_status == "1" || g.Genre_status == "0")
                                        orderby g.Genre_id descending
                                        select g;
                    switch (sort_genre)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_genre = from g in _appdb.Genres
                                            where (g.Genre_status == "1" || g.Genre_status == "0")
                                            orderby g.Genre_id descending
                                            select g;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_genre = from g in _appdb.Genres
                                            where (g.Genre_status == "1" || g.Genre_status == "0")
                                            orderby g.Genre_id
                                            select g;
                            break;

                        case "genrename_desc":
                            ViewBag.sortname = "Sort by: Genre name (Z-A)";
                            list_genre = from g in _appdb.Genres
                                            where (g.Genre_status == "1" || g.Genre_status == "0")
                                            orderby g.Genre_name descending
                                            select g;
                            break;

                        case "genrename_asc":
                            ViewBag.sortname = "Sort by: Genre name (A-Z)";
                            list_genre = from g in _appdb.Genres
                                            where (g.Genre_status == "1" || g.Genre_status == "0")
                                            orderby g.Genre_name
                                            select g;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_genre)) return View(list_genre.ToPagedList(pageNum, pageSize));
                    switch (show_genre)
                    {
                        //case 1: search all
                        case "1":
                            list_genre = (IOrderedQueryable<Genre>)list_genre.Where(g => g.Genre_id.ToString().Contains(search_genre) ||
                                                                                            g.Genre_name.Contains(search_genre));
                            break;
                        //case 2: search by id
                        case "2":
                            list_genre = (IOrderedQueryable<Genre>)list_genre.Where(g => g.Genre_id.ToString().Contains(search_genre));
                            break;
                        //case 3: search by name
                        case "3":
                            list_genre = (IOrderedQueryable<Genre>)list_genre.Where(g => g.Genre_name.Contains(search_genre));
                            break;
                    }
                    return View(list_genre.ToPagedList(pageNum, pageSize));
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

        //Genre Trash
        public ActionResult GenreTrash(string search_genre, string show_genre, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var list_genre = from g in _appdb.Genres
                                        where g.Genre_status == "2"
                                        orderby g.Genre_id descending
                                        select g;
                    if (!string.IsNullOrEmpty(search_genre))
                    {
                        //search all
                        if (show_genre.Equals("1"))
                            list_genre = (IOrderedQueryable<Genre>)list_genre.Where(s => s.Genre_id.ToString().Contains(search_genre) || s.Genre_name.Contains(search_genre));
                        //search by id
                        else if (show_genre.Equals("2"))
                            list_genre = (IOrderedQueryable<Genre>)list_genre.Where(s => s.Genre_id.ToString().Contains(search_genre));
                        //search by full name
                        else if (show_genre.Equals("3"))
                            list_genre = (IOrderedQueryable<Genre>)list_genre.Where(a => a.Genre_name.Contains(search_genre));
                        return View("GenreTrash", list_genre.ToPagedList(pageNum, 50));
                    }
                    return View(list_genre.ToPagedList(pageNum, pageSize));
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

        //Search genre Suggestion
        [HttpPost]
        public JsonResult SuggestGenreSearch(string Prefix)
        {
            var search_genre = (from g in _appdb.Genres
                                where g.Genre_status != "2" && g.Genre_name.StartsWith(Prefix)
                                orderby g.Genre_name ascending
                                select new { g.Genre_name });
            return Json(search_genre, JsonRequestBehavior.AllowGet);
        }

        //Create new Genre view
        public ActionResult GenreCreate()
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
                    return RedirectToAction("GenreIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Genre code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenreCreate(Genre genre)
        {
            try
            {
                genre.Genre_name = genre.Genre_name;
                genre.Image = genre.Image;
                genre.Create_date = DateTime.Now;
                genre.Create_by = User.Identity.GetName();
                genre.Genre_status = genre.Genre_status;
                genre.Area_id = genre.Area_id;
                _appdb.Genres.Add(genre);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new genre: " + genre.Genre_name + "", "success");
                return RedirectToAction("GenreIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(genre);
        }

        //Genre Edit
        public ActionResult GenreEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("GenreEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var genre = _appdb.Genres.SingleOrDefault(g => g.Genre_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (genre.Genre_status == "1" || genre.Genre_status == "0"))
                {
                    if (genre == null || id == null)
                    {
                        Notification.set_noti("This genre is not exist: " + genre.Genre_name + "", "warning");
                        return RedirectToAction("GenreIndex");
                    }
                    ViewBag.ListMusicAreas = new SelectList(_appdb.Areas.Where(a => (a.Area_status == "1")).OrderBy(a => a.Area_name), "Area_id", "Area_name", 0);
                    return View(genre);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("GenreIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenreEdit(Genre genre, string returnUrl)
        {
            try
            {
                genre.Genre_name = genre.Genre_name;
                genre.Image = genre.Image;
                genre.Create_date = DateTime.Now;
                genre.Create_by = User.Identity.GetName();
                genre.Genre_status = genre.Genre_status;
                genre.Area_id = genre.Area_id;
                _appdb.Entry(genre).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update genre successfully: " + genre.Genre_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("GenreIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(genre);
        }

        //Genre Detail
        public ActionResult GenreDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_genre = (from g in _appdb.Genres
                                      where g.Genre_id == id
                                      orderby g.Create_date descending
                                      select g).FirstOrDefault();
                    if (list_genre != null && id != null) return View(list_genre);
                    Notification.set_noti("This genre is not exist: " + list_genre.Genre_name + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("GenreIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Move Genre to trash
        public ActionResult MoveGenreToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var genre = _appdb.Genres.SingleOrDefault(g => g.Genre_id == id);
                if (genre == null || id == null)
                {
                    Notification.set_noti("This genre is not exist: " + genre.Genre_name + "", "warning");
                    return RedirectToAction("GenreIndex");
                }
                genre.Genre_status = "2";
                _appdb.Entry(genre).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable genre " + genre.Genre_name + "sucessfully.", "success");
                return RedirectToAction("GenreIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("GenreIndex");
            }
        }

        //Undo Genre From Trash
        public ActionResult UndoGenreFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var genre = _appdb.Genres.SingleOrDefault(g => g.Genre_id == id);
                if (genre == null || id == null)
                {
                    Notification.set_noti("This genre is not exist: " + genre.Genre_name + "", "warning");
                    return RedirectToAction("GenreIndex");
                }
                genre.Genre_status = "1";
                _appdb.Entry(genre).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo genre " + genre.Genre_name + "sucessfully.", "success");
                return RedirectToAction("GenreTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("GenreIndex");
            }
        }

        // Delete Genre
        public ActionResult GenreDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = _appdb.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        [HttpPost, ActionName("GenreDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Genre genre = _appdb.Genres.Find(id);
            _appdb.Genres.Remove(genre);
            _appdb.SaveChanges();
            return RedirectToAction("GenreTrash");
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