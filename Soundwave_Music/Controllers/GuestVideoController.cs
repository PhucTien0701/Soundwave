using PagedList;
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
    public class GuestVideoController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //list Video area
        public PartialViewResult ListArea()
        {
            ViewBag.videoarea = _appdb.Genres.ToList();
            return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && a.Area_id == 1 && (a.Genres.Count > 0)).ToList());
        }

        //Video Index View
        public ActionResult VideoIndex()
        {
            DateTime today = DateTime.Today;
            // Video Genre List
            ViewBag.VideoGenre = _appdb.Genres.Where(g => g.Genre_status == "1").ToList();
            // Video Area List
            ViewBag.videoArea = _appdb.Areas.Where(a => a.Area_status == "1").ToList();
            // Singer
            ViewBag.Singer = _appdb.Singers.Where(s => s.Singer_status == "1").ToList();
            //Count loved Video
            ViewBag.CountLoveVideo = _appdb.Video_Love_React.ToList();
            // New Release
            ViewBag.VideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
            //TOP LOVED VIDEOS:
            List<Video> toplovedvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Video_Love_React.Count()).Take(6).ToList();
            ViewBag.TopLovedVideo = toplovedvideo;
            //HOT videoS
            List<Video> hotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(4).ToList();
            ViewBag.HotVideo = hotvideo;
            //TRENDING
            List<Video> trendingvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).ThenByDescending(s => s.View_count).Take(5).ToList();
            ViewBag.Trending = trendingvideo;
            //TOP 10 VIDEOS OF MONTH
            List<Video> topvideoofmonth = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(10).ToList();
            List<Video> topvideoofmonth1 = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1 && s.Release_date.Month == today.Month).OrderByDescending(s => s.View_count).Take(10).ToList();
            if (topvideoofmonth.Count() > 0)
            {
                ViewBag.TopVideoOfMoth = topvideoofmonth;
            }
            else
            {
                ViewBag.TopVideoOfMoth = topvideoofmonth1;
            }
            return View();
        }

        //Top Videos
        public ActionResult TopVideo()
        {
            ViewBag.TopVietnamVideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList(); ;
            //Top Vietnam Video
            List<Video> topvietnamvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList();
            ViewBag.TopVietnamVideo = topvietnamvideo;
            return View();
        }

        //Video new release
        public ActionResult VideoNewRelease()
        {
            // VIDEO NEW RELEASE
            // => Vietnam
            ViewBag.NewVietnamVideo = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
            return View();
        }

        //Top loved Video
        public ActionResult TopLovedVideo()
        {
            // => Vietnam
            List<Video> toplovedvnvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Video_Love_React.Count()).Take(5).ToList();
            ViewBag.TopLovedVnVideo = toplovedvnvideo;
            return View();
        }

        //Top hot Video
        public ActionResult HotVideo()
        {
            // => Vietnam
            List<Video> vnhotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(5).ToList();
            ViewBag.VnHotVideo = vnhotvideo;
            return View();
        }

        //List genres in area
        public ActionResult Genre(int? page, int? id)
        {
            ViewBag.AreaImage = _appdb.Areas.Where(a => a.Area_id == id).FirstOrDefault().Image;
            ViewBag.AreaName = _appdb.Areas.Where(a => a.Area_id == id).FirstOrDefault().Area_name;
            var area = _appdb.Areas.Where(a => a.Area_status == "1" && a.Area_id == id && (a.Genres.Count() > 0)).FirstOrDefault();
            return View("Genre", GetGenre(a => a.Genre_status == "1" && a.Area.Area_id == area.Area_id, page));
        }

        public ActionResult Video(int? page, int? id)
        {
            ViewBag.videogenrename = _appdb.Genres.FirstOrDefault().Genre_name;
            ViewBag.GenreName = _appdb.Genres.Where(g => g.Genre_id == id).FirstOrDefault().Genre_name;
            var genre = _appdb.Genres.Where(g => g.Genre_status == "1" && g.Genre_id == id && (g.Videos.Count() > 0)).FirstOrDefault();
            return View("Video", GetVideoInGenre(g => g.Video_status == "1" && g.Genre.Genre_id == genre.Genre_id, page));
        }

        //Get Video genre
        private IPagedList GetGenre(Expression<Func<Genre, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listgenre = _appdb.Genres.Where(expression).OrderByDescending(g => g.Genre_id).ToPagedList(pageNum, pageSize);
            return listgenre;
        }
        //Ger Video
        private IPagedList GetVideoInGenre(Expression<Func<Video, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listvideo = _appdb.Videos.Where(expression).OrderByDescending(s => s.Video_id).ToPagedList(pageNum, pageSize);
            return listvideo;
        }
        private IPagedList GetVideoArea(Expression<Func<Genre, bool>> expression, int? page)
        {
            int pageSize = 15;
            int pageNum = (page ?? 1);
            var list = _appdb.Genres.Where(expression).OrderByDescending(g => g.Genre_id).ToPagedList(pageNum, pageSize);
            return list;
        }

        //List Video of singer
        public ActionResult SingerVideo(int? page, int? size, int id)
        {
            var singer = _appdb.Singers.Where(s => s.Singer_id == id).FirstOrDefault();
            ViewBag.CountVideo = singer.Videos.Count();
            ViewBag.SingerName = singer.Singer_name;
            ViewBag.SingerImage = singer.Image;
            ViewBag.ListSingerName = singer.Singer_name;
            var pageSize = size ?? 10;
            var pageNum = page ?? 1;
            var listvideo = from s in _appdb.Videos
                           join si in _appdb.Singers on s.Singer_id equals si.Singer_id
                           where si.Singer_status == "1" && si.Singer_id == id
                           orderby s.Video_id descending
                           select new DTOofVideo
                           {
                               Video_id = s.Video_id,
                               Release_date = s.Release_date,
                               Image = s.Image,
                               Video_name = s.Video_name,
                           };
            ViewBag.CountVideo = listvideo.Count();
            return View(listvideo.ToPagedList(pageNum, pageSize));
        }

        //List Video of composer
        public ActionResult ComposerVideo(int? page, int? size, int id)
        {
            var composer = _appdb.Composers.Where(s => s.Composer_id == id).FirstOrDefault();
            ViewBag.CountVideo = composer.Videos.Count();
            ViewBag.ComposerName = composer.Composer_name;
            ViewBag.ComposerImage = composer.Image;
            ViewBag.ListComposerName = composer.Composer_name;
            var pageSize = size ?? 10;
            var pageNum = page ?? 1;
            var listvideo = from s in _appdb.Videos
                           join co in _appdb.Composers on s.Composer_id equals co.Composer_id
                           where co.Composer_status == "1" && co.Composer_id == id
                           orderby s.Video_id descending
                           select new DTOofVideo
                           {
                               Video_id = s.Video_id,
                               Release_date = s.Release_date,
                               Image = s.Image,
                               Video_name = s.Video_name,
                           };
            ViewBag.CountVideo = listvideo.Count();
            return View(listvideo.ToPagedList(pageNum, pageSize));
        }

        //search suggest
        public JsonResult Index(string Prefix)
        {
            var search = (from s in _appdb.Singers
                          where (s.Singer_name.Contains(Prefix))
                          select new { s.Singer_name });
            return Json(search, JsonRequestBehavior.AllowGet);
        }

        //Get Video
        private IPagedList GetVideo(Expression<Func<Video, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.musicarea = _appdb.Areas.ToList();
            ViewBag.genre = _appdb.Genres.ToList();
            var listvideo = _appdb.Videos.Where(expression).OrderByDescending(s => s.Video_id).ToPagedList(pageNum, pageSize);
            return listvideo;
        }

        //Get Video list in genre
        public ActionResult ListVideoInGenre(int? page, int? id)
        {
            List<DTOofVideo> listvideo = (from s in _appdb.Videos
                                        where s.Genre.Genre_id == id && s.Video_status == "1"
                                        orderby s.Video_id descending
                                        select new DTOofVideo
                                        {
                                            Video_name = s.Video_name,
                                            Release_date = s.Release_date,
                                            Composer = s.Composer.Composer_name,
                                            Singer = s.Singer.Singer_name,
                                            Image = s.Image,
                                        }).ToList();
            var videogenre = _appdb.Genres.Where(g => g.Genre_id == id && g.Genre_status == "1").FirstOrDefault();
            ViewBag.VideoGenre = videogenre.Genre_name;
            return View("Video", GetVideo(s => s.Video_status == "1", page));
        }

        //List all Video in area
        public ActionResult ListAllVideo(int? page)
        {
            List<DTOofVideo> listvideo = (from s in _appdb.Videos
                                        where s.Video_status == "1" && s.Area_id == 1
                                        orderby s.Release_date descending
                                        select new DTOofVideo
                                        {
                                            Video_name = s.Video_name,
                                            Release_date = s.Release_date,
                                            Composer = s.Composer.Composer_name,
                                            Singer = s.Singer.Singer_name,
                                            Image = s.Image
                                        }).ToList();
            ViewBag.ListName = "All Video";
            ViewBag.CountViewVideo = _appdb.Video_Love_React.ToList();
            return View("Video", GetVideo(s => s.Video_status == "1" && s.Area_id == 1, page));
        }

        //Video Details
        public ActionResult VideoDetail(int? page, int? id)
        {
            ViewBag.VideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(4).ToList();

            int pageSize = 5;
            int pageNum = (page ?? 1);
            var video = _appdb.Videos.Where(s => s.Video_id == id && s.Video_status == "1" && s.Genre.Genre_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Video_id).ToList().FirstOrDefault();
            ViewBag.NewVideoRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
            //relate Video
            List<Video> relatedvideo = _appdb.Videos.Where(s => s.Singer.Singer_id == video.Singer.Singer_id && s.Video_id != video.Video_id && s.Video_status == "1").OrderByDescending(s => s.Video_id).Take(6).ToList();
            ViewBag.RelatedVideo = relatedvideo;
            //relate genre
            List<Video> relatedvideogenre = _appdb.Videos.Where(s => s.Genre.Genre_id == video.Genre.Genre_id && s.Video_id != video.Video_id && s.Video_status == "1").OrderByDescending(s => s.Video_id).Take(6).ToList();
            ViewBag.RelatedVideoGenre = relatedvideogenre;
            //loved Video
            ViewBag.ReactLoveVideo = _appdb.Video_Love_React.ToList();
            //host Video
            ViewBag.HostVideoComment = _appdb.Video_Comment.ToList();
            //count love Video
            ViewBag.CountLoveVideo = _appdb.Video_Love_React.Where(s => s.Video_id == video.Video_id).Count();
            video.View_count++;
            _appdb.SaveChanges();
            return View(video);
        }
    }
}