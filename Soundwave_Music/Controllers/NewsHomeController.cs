using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.DataTransferObjectives;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Soundwave_Music.Controllers
{
    public class NewsHomeController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        public ActionResult ListAllNews(int? page)
        {
            List<DTOofNews> listnews = (from n in _appdb.News
                                        where n.Status == "1"
                                        orderby n.Create_date descending
                                        select new DTOofNews
                                        {
                                            News_title = n.News_title,
                                            Create_date = n.Create_date,
                                            Image = n.Image
                                        }).ToList();
            ViewBag.ListName = "All News";
            return View("News", GetNews(s => s.Status == "1", page));
        }

        private IPagedList GetNews(Expression<Func<News, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listnews = _appdb.News.Where(expression).OrderByDescending(s => s.News_id).ToPagedList(pageNum, pageSize);
            return listnews;
        }

        //News Details
        public ActionResult NewsDetail(int? page, int? id)
        {
            ViewBag.NewsCommentLike = _appdb.Like_News_Comment.ToList();
            ViewBag.RecentNews = _appdb.News.OrderByDescending(s => s.News_id).Where(s => s.Status == "1").OrderByDescending(s => s.Create_date).Take(4).ToList();

            int pageSize = 5;
            int pageNum = (page ?? 1);
            var news = _appdb.News.Where(s => s.News_id == id && s.Status == "1").OrderByDescending(s => s.News_id).ToList().FirstOrDefault();
            //count News comment
            ViewBag.countcomment = _appdb.News_Comment.Where(m => m.News_id == news.News_id).Count();
            //comment
            var comment = _appdb.News_Comment.Where(c => c.News_id == news.News_id).OrderByDescending(c => c.News_comment_id).ToList();
            ViewBag.NewsComment = comment.ToPagedList(pageNum, pageSize);
            news.View_count++;
            _appdb.SaveChanges();
            return View(news);
        }

        [ValidateInput(false)]
        public JsonResult CommentNews(News_Comment commentnews, String commentcontent)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    commentnews.User_id = User.Identity.GetUserId();
                    commentnews.Create_date = DateTime.Now;
                    commentnews.Content = commentcontent;
                    _appdb.News_Comment.Add(commentnews);
                    _appdb.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LikeCommentNews(Like_News_Comment commentNewsLikes, int newscomment_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int Userid = User.Identity.GetUserId();
                    var comment = _appdb.Like_News_Comment.FirstOrDefault(m => m.News_comment_id == newscomment_id && m.User_id == Userid);
                    if (comment != null)
                    {
                        _appdb.Like_News_Comment.Remove(comment);
                    }
                    else
                    {
                        commentNewsLikes.User_id = User.Identity.GetUserId();
                        commentNewsLikes.Create_date = DateTime.Now;
                        commentNewsLikes.News_comment_id = newscomment_id;
                        commentNewsLikes.React_like = "1";
                        _appdb.Like_News_Comment.Add(commentNewsLikes);
                    }
                    _appdb.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //search News
        public ActionResult SearchNewsResult(string searchnews, int? page)
        {
            List<DTOofNews> listnewsresult = (from s in _appdb.News
                                              where s.News_title.Contains(searchnews) && s.Status == "1"
                                              orderby s.News_id descending
                                              select new DTOofNews
                                              {
                                                  News_title = s.News_title,
                                                  Create_date = s.Create_date,
                                                  Image = s.Image
                                              }).ToList();
            var list = _appdb.News.OrderByDescending(s => s.News_id);
            ViewBag.listnewsresult = listnewsresult;
            ViewBag.ListName = "Search News";
            //Count comment
            ViewBag.CountComment = _appdb.News_Comment.ToList();           

            return View("News", GetNews(g => g.Status == "1" && g.News_title.Contains(searchnews), page));

        }

        //suggest search
        [HttpPost]
        public JsonResult SuggestNewsSearch(string Prefix)
        {
            var searchnews = (from s in _appdb.News
                              where s.Status == "1" && (s.News_title.Contains(Prefix))
                              orderby s.News_title ascending
                              select new DTOofNews
                              {
                                  News_title = s.News_title,
                                  News_id = s.News_id,
                                  Image = s.Image,
                              });
            return Json(searchnews, JsonRequestBehavior.AllowGet);
        }
    }
}