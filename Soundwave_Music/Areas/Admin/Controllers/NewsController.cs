using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.DataTransferObjectives;
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
    public class NewsController : BaseController
    {
        SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //News index
        public ActionResult NewsIndex(string search_news, string show_news, int? _sizepage, int? _pagenumber, string sort_news)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_news_in_trash = _appdb.News.Count(s => s.Status == "2");
                    ViewBag.Current_sort = sort_news;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_news == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortByNewsName = sort_news == "newsname_asc" ? "newsname_desc" : "newsname_asc";
                    var list_news = from s in _appdb.News
                                    where (s.Status == "1" || s.Status == "0")
                                    orderby s.News_id descending
                                    select s;
                    switch (sort_news)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_news = from s in _appdb.News
                                        where (s.Status == "1" || s.Status == "0")
                                        orderby s.News_id descending
                                        select s;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_news = from s in _appdb.News
                                        where (s.Status == "1" || s.Status == "0")
                                        orderby s.News_id
                                        select s;
                            break;

                        case "newsname_desc":
                            ViewBag.sortname = "Sort by: News Title (Z-A)";
                            list_news = from s in _appdb.News
                                        where (s.Status == "1" || s.Status == "0")
                                        orderby s.News_title descending
                                        select s;
                            break;

                        case "newsname_asc":
                            ViewBag.sortname = "Sort by: News Title (A-Z)";
                            list_news = from s in _appdb.News
                                        where (s.Status == "1" || s.Status == "0")
                                        orderby s.News_title
                                        select s;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_news)) return View(list_news.ToPagedList(pageNum, pageSize));
                    switch (show_news)
                    {
                        //case 1: search all
                        case "1":
                            list_news = (IOrderedQueryable<News>)list_news.Where(s => s.News_id.ToString().Contains(search_news) ||
                                                                                      s.News_title.Contains(search_news));
                            break;
                        //case 2: search by id
                        case "2":
                            list_news = (IOrderedQueryable<News>)list_news.Where(s => s.News_id.ToString().Contains(search_news));
                            break;
                        //case 3: search by name
                        case "3":
                            list_news = (IOrderedQueryable<News>)list_news.Where(s => s.News_title.Contains(search_news));
                            break;                        
                    }
                    return View(list_news.ToPagedList(pageNum, pageSize));
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

        //News trash
        public ActionResult NewsTrash(string search_news, string show_news, int? _sizepage, int? _pagenumber, string sort_news)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    ViewBag.Current_sort = sort_news;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_news == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortByNewsName = sort_news == "newsname_asc" ? "newsname_desc" : "newsname_asc";
                    var list_news = from s in _appdb.News
                                    where s.Status == "2"
                                    orderby s.News_id descending
                                    select s;
                    switch (sort_news)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_news = from s in _appdb.News
                                        where (s.Status == "1" || s.Status == "0")
                                        orderby s.News_id descending
                                        select s;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_news = from s in _appdb.News
                                        where (s.Status == "1" || s.Status == "0")
                                        orderby s.News_id
                                        select s;
                            break;

                        case "newsname_desc":
                            ViewBag.sortname = "Sort by: News Title (Z-A)";
                            list_news = from s in _appdb.News
                                        where (s.Status == "1" || s.Status == "0")
                                        orderby s.News_title descending
                                        select s;
                            break;

                        case "newsname_asc":
                            ViewBag.sortname = "Sort by: News Title (A-Z)";
                            list_news = from s in _appdb.News
                                        where (s.Status == "1" || s.Status == "0")
                                        orderby s.News_title
                                        select s;
                            break;
                    }
                    if (!string.IsNullOrEmpty(search_news))
                    {
                        //search all
                        if (show_news.Equals("1"))
                            list_news = (IOrderedQueryable<News>)list_news.Where(s => s.News_id.ToString().Contains(search_news) ||
                                                                                 s.News_title.Contains(search_news));
                        //search by id
                        else if (show_news.Equals("2"))
                            list_news = (IOrderedQueryable<News>)list_news.Where(s => s.News_id.ToString().Contains(search_news));
                        //search by name
                        else if (show_news.Equals("3"))
                            list_news = (IOrderedQueryable<News>)list_news.Where(s => s.News_title.Contains(search_news));                        
                        return View("NewsTrash", list_news.ToPagedList(pageNum, 50));
                    }
                    return View(list_news.ToPagedList(pageNum, pageSize));
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

        //Search News Suggestion
        [HttpPost]
        public JsonResult SuggestNewsSearch(string Prefix)
        {
            var search_news = (from s in _appdb.News
                               where s.Status != "2" && s.News_title.StartsWith(Prefix)
                               orderby s.News_title ascending
                               select new { s.News_title });
            return Json(search_news, JsonRequestBehavior.AllowGet);
        }

        //Create new News view
        public ActionResult NewsCreate()
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
                    return RedirectToAction("NewsIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new News code
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult NewsCreate(News news)
        {
            try
            {
                news.News_title = news.News_title;
                news.Content = news.Content;
                news.Image = news.Image;
                news.View_count = 0;                
                news.Create_date = DateTime.Now;
                news.Update_date = DateTime.Now;
                news.Status = news.Status;
                news.User_id = User.Identity.GetUserId();
                _appdb.News.Add(news);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new news: " + news.News_title + "", "success");
                return RedirectToAction("NewsIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(news);
        }

        //News Edit
        public ActionResult NewsEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("NewsEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var news = _appdb.News.SingleOrDefault(s => s.News_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (news.Status == "1" || news.Status == "0"))
                {
                    if (news == null || id == null)
                    {
                        Notification.set_noti("This news is not exist: " + news.News_title + "", "warning");
                        return RedirectToAction("NewsIndex");
                    }
                    return View(news);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("NewsIndex");
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
        public ActionResult NewsEdit(News news, string returnUrl)
        {
            try
            {
                news.News_title = news.News_title;
                news.Content = news.Content;
                news.Image = news.Image;
                //news.View_count = 0;
                //news.Create_date = DateTime.Now;
                news.Update_date = DateTime.Now;
                news.Status = news.Status;
                //news.User_id = User.Identity.GetUserId();
                _appdb.Entry(news).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update news successfully: " + news.News_title + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("NewsIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(news);
        }

        //News Detail
        public ActionResult NewsDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_news = (from s in _appdb.News
                                     where s.News_id == id
                                     orderby s.Create_date descending
                                     select s).FirstOrDefault();
                    if (list_news != null && id != null) return View(list_news);
                    Notification.set_noti("This news is not exist: " + list_news.News_title + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("NewsIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Move News to trash
        public ActionResult MoveNewsToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var news = _appdb.News.SingleOrDefault(s => s.News_id == id);
                if (news == null || id == null)
                {
                    Notification.set_noti("This news is not exist: " + news.News_title + "", "warning");
                    return RedirectToAction("NewsIndex");
                }
                news.Status = "2";
                _appdb.Entry(news).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable news " + news.News_title + "sucessfully.", "success");
                return RedirectToAction("NewsIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("NewsIndex");
            }
        }

        //Undo News From Trash
        public ActionResult UndoNewsFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var news = _appdb.News.SingleOrDefault(s => s.News_id == id);
                if (news == null || id == null)
                {
                    Notification.set_noti("This news is not exist: " + news.News_title + "", "warning");
                    return RedirectToAction("NewsIndex");
                }
                news.Status = "1";
                _appdb.Entry(news).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo news " + news.News_title + "sucessfully.", "success");
                return RedirectToAction("NewsTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("NewsIndex");
            }
        }

        // Delete News
        public ActionResult NewsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = _appdb.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [HttpPost, ActionName("NewsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = _appdb.News.Find(id);
            _appdb.News.Remove(news);
            _appdb.SaveChanges();
            return RedirectToAction("NewsTrash");
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