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
    public class UserVideoController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //list Video area
        public PartialViewResult ListArea()
        {
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.videoarea = _appdb.Genres.ToList();
                return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && a.Area_id == 1 && (a.Genres.Count > 0)).ToList());
            }
            else
            {
                ViewBag.videoarea = _appdb.Genres.ToList();
                return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && (a.Genres.Count > 0)).ToList());
            }
        }

        //Video Index View
        public ActionResult VideoIndex()
        {
            DateTime today = DateTime.Today;
            // Video Genre List
            ViewBag.VideoGenre = _appdb.Genres.Where(g => g.Genre_status == "1").ToList();
            // Video Area List
            ViewBag.VideoArea = _appdb.Areas.Where(a => a.Area_status == "1").ToList();
            // Singer
            ViewBag.Singer = _appdb.Singers.Where(s => s.Singer_status == "1").ToList();
            //Count loved Video
            ViewBag.CountLoveVideo = _appdb.Video_Love_React.ToList();
            // New Release
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.VideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
            }
            else
            {
                ViewBag.VideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1").OrderByDescending(s => s.Release_date).Take(5).ToList();
            }
            //TOP LOVED VIDEOS:
            if (User.Identity.GetRole() == 2)
            {
                List<Video> toplovedvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Video_Love_React.Count()).Take(6).ToList();
                ViewBag.TopLovedVideo = toplovedvideo;
            }
            else
            {
                List<Video> toplovedvideo = _appdb.Videos.Where(s => s.Video_status == "1").OrderByDescending(s => s.Video_Love_React.Count()).Take(6).ToList();
                ViewBag.TopLovedVideo = toplovedvideo;
            }

            //HOT VIDEOS
            if (User.Identity.GetRole() == 2)
            {
                List<Video> hotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(4).ToList();
                ViewBag.HotVideo = hotvideo;
            }
            else
            {
                List<Video> hotvideo = _appdb.Videos.Where(s => s.Video_status == "1").OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(4).ToList();
                ViewBag.HotVideo = hotvideo;
            }

            //TRENDING
            if (User.Identity.GetRole() == 2)
            {
                List<Video> trendingvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).ThenByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.Trending = trendingvideo;
            }
            else
            {
                List<Video> trendingvideo = _appdb.Videos.Where(s => s.Video_status == "1").OrderByDescending(s => s.Release_date).ThenByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.Trending = trendingvideo;
            }

            //TOP 10 video OF MONTH
            if (User.Identity.GetRole() == 2)
            {
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
            }
            else
            {
                List<Video> topvideoofmonth = _appdb.Videos.Where(s => s.Video_status == "1").OrderByDescending(s => s.View_count).Take(10).ToList();
                List<Video> topvideoofmonth1 = _appdb.Videos.Where(s => s.Video_status == "1" && s.Release_date.Month == today.Month).OrderByDescending(s => s.View_count).Take(10).ToList();
                if (topvideoofmonth.Count() > 0)
                {
                    ViewBag.TopVideoOfMoth = topvideoofmonth;
                }
                else
                {
                    ViewBag.TopVideoOfMoth = topvideoofmonth1;
                }
            }
            return View();
        }

        //Top videos
        public ActionResult TopVideo()
        {
            ViewBag.TopVietnamVideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList(); ;
            if (User.Identity.GetRole() == 2)
            {
                List<Video> topvietnamvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopVietnamVideo = topvietnamvideo;
            }
            else
            {
                //Top Vietnam Video
                List<Video> topvietnamvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopVietnamVideo = topvietnamvideo;
                //Top US-UK Video
                List<Video> topusukvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 2).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopUSUKVideo = topusukvideo;
                //Top Korea Video
                List<Video> topkoreavideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 3).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopKoreaVideo = topkoreavideo;
                //Top Japan Video
                List<Video> topjapanvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 4).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopJapanVideo = topjapanvideo;
                //Top China Video
                List<Video> topchinavideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 5).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopChinaVideo = topchinavideo;
            }
            return View();
        }

        //Video new release
        public ActionResult VideoNewRelease()
        {
            // video NEW RELEASE
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                ViewBag.NewVietnamVideo = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
            }
            else
            {
                // => Vietnam
                ViewBag.NewVietnamVideo = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
                // => US-UK:
                ViewBag.NewUSUKVideo = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Release_date).Take(5).ToList();
                // => Korea:
                ViewBag.NewKoreaVideo = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Release_date).Take(5).ToList();
                // => Japan:
                ViewBag.NewJapanVideo = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Release_date).Take(5).ToList();
                // => China
                ViewBag.NewChinaVideo = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Release_date).Take(5).ToList();
            }
            return View();
        }

        //Top loved Video
        public ActionResult TopLovedVideo()
        {
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                List<Video> toplovedvnvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Video_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedVnVideo = toplovedvnvideo;
            }
            else
            {
                // => Vietnam
                List<Video> toplovedvnvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Video_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedVnVideo = toplovedvnvideo;
                // => US-UK
                List<Video> toplovedusvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Video_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedUsVideo = toplovedusvideo;
                // => Korea
                List<Video> toplovedkovideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Video_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedKoVideo = toplovedkovideo;
                // => Japan
                List<Video> toplovedjapvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Video_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedJapVideo = toplovedjapvideo;
                // => China
                List<Video> toplovedchinavideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Video_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedChinaVideo = toplovedchinavideo;
            }
            return View();
        }

        //Top hot Video
        public ActionResult HotVideo()
        {
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                List<Video> vnhotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(5).ToList();
                ViewBag.VnHotVideo = vnhotvideo;
            }
            else
            {
                // => Vietnam
                List<Video> vnhotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(5).ToList();
                ViewBag.VnHotVideo = vnhotvideo;
                // => US-UK
                List<Video> ushotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 2).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(5).ToList();
                ViewBag.UsHotVideo = ushotvideo;
                // => Korea
                List<Video> kohotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 3).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(5).ToList();
                ViewBag.KoHotVideo = kohotvideo;
                // => Japan
                List<Video> japhotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 4).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(5).ToList();
                ViewBag.JapHotVideo = japhotvideo;
                // => China
                List<Video> chinahotvideo = _appdb.Videos.Where(s => s.Video_status == "1" && s.Area_id == 5).OrderByDescending(s => (s.View_count + s.Video_Love_React.Count())).Take(5).ToList();
                ViewBag.ChinaHotVideo = chinahotvideo;
            }
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
            ViewBag.Videogenrename = _appdb.Genres.FirstOrDefault().Genre_name;
            ViewBag.GenreName = _appdb.Genres.Where(g => g.Genre_id == id).FirstOrDefault().Genre_name;
            ViewBag.ReactLoveVideo = _appdb.Video_Love_React.ToList();
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

        //search Videos
        public ActionResult SearchVideoResult(string searchvideo, int? page)
        {
            if (User.Identity.GetRole() == 2)
            {
                List<DTOofVideo> listvideoresult = (from s in _appdb.Videos
                                                  where s.Video_name.Contains(searchvideo) && s.Video_status == "1" && s.Area_id == 1
                                                  orderby s.Video_id descending
                                                  select new DTOofVideo
                                                  {
                                                      Video_name = s.Video_name,
                                                      Release_date = s.Release_date,
                                                      Videos_comment = s.Video_Comment.Count(),
                                                      Videos_love_react = s.Video_Love_React.Count(),
                                                      Singer = s.Singer.Singer_name,
                                                      Composer = s.Composer.Composer_name,
                                                      Image = s.Image
                                                  }).ToList();
                var list = _appdb.Videos.OrderByDescending(s => s.Video_id);
                ViewBag.listvideoresult = listvideoresult;
                ViewBag.ListName = "Search Video";
                //Count comment
                ViewBag.CountComment = _appdb.Video_Comment.ToList();
                //React love to video
                ViewBag.ReactLoveVideo = _appdb.Video_Love_React.ToList();

                return View("Video", GetVideo(g => g.Video_status == "1" && g.Area_id == 1 && g.Video_name.Contains(searchvideo), page));
            }
            else
            {
                List<DTOofVideo> listvideoresult = (from s in _appdb.Videos
                                                  where s.Video_name.Contains(searchvideo) && s.Video_status == "1"
                                                  orderby s.Video_id descending
                                                  select new DTOofVideo
                                                  {
                                                      Video_name = s.Video_name,
                                                      Release_date = s.Release_date,
                                                      Videos_comment = s.Video_Comment.Count(),
                                                      Videos_love_react = s.Video_Love_React.Count(),
                                                      Singer = s.Singer.Singer_name,
                                                      Composer = s.Composer.Composer_name,
                                                      Image = s.Image
                                                  }).ToList();
                var list = _appdb.Videos.OrderByDescending(s => s.Video_id);
                ViewBag.listvideoresult = listvideoresult;
                ViewBag.ListName = "Search Video";
                //Count comment
                ViewBag.CountComment = _appdb.Video_Comment.ToList();
                //React love to video
                ViewBag.ReactLoveVideo = _appdb.Video_Love_React.ToList();

                return View("Video", GetVideo(g => g.Video_status == "1" && g.Video_name.Contains(searchvideo), page));
            }
        }

        //search suggest
        public JsonResult Index(string Prefix)
        {
            var search = (from s in _appdb.Singers
                          where (s.Singer_name.Contains(Prefix))
                          select new { s.Singer_name });
            return Json(search, JsonRequestBehavior.AllowGet);
        }

        //Get video
        private IPagedList GetVideo(Expression<Func<Video, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.musicarea = _appdb.Areas.ToList();
            ViewBag.genre = _appdb.Genres.ToList();
            var listvideo = _appdb.Videos.Where(expression).OrderByDescending(s => s.Video_id).ToPagedList(pageNum, pageSize);
            return listvideo;
        }

        //Get video list in genre
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
            //React love to video
            ViewBag.ReactLoveVideo = _appdb.Video_Love_React.ToList();
            return View("Video", GetVideo(s => s.Video_status == "1", page));
        }

        //List all Video in area
        public ActionResult ListAllVideo(int? page)
        {
            if (User.Identity.GetRole() == 2)
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
                //React love to video
                ViewBag.ReactLoveVideo = _appdb.Video_Love_React.ToList();
                return View("Video", GetVideo(s => s.Video_status == "1" && s.Area_id == 1, page));
            }
            else
            {
                List<DTOofVideo> listvideo = (from s in _appdb.Videos
                                            where s.Video_status == "1"
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
                //React love to video
                ViewBag.ReactLoveVideo = _appdb.Video_Love_React.ToList();
                return View("Video", GetVideo(s => s.Video_status == "1", page));
            }
        }

        //Video Details
        public ActionResult VideoDetail(int? page, int? id)
        {
            ViewBag.VideoCommentLike = _appdb.Like_Video_Comment.ToList();
            //Video new release
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.VideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(4).ToList();
            }
            else
            {
                ViewBag.VideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1").OrderByDescending(s => s.Release_date).Take(4).ToList();
            }
            //Video details
            if (User.Identity.GetRole() == 2)
            {
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
                //count video music comment
                ViewBag.countcomment = _appdb.Video_Comment.Where(m => m.Video_id == video.Video_id).Count();
                //comment
                var comment = _appdb.Video_Comment.Where(c => c.Video_id == video.Video_id).OrderByDescending(c => c.Video_comment_id).ToList();
                ViewBag.VideoComment = comment.ToPagedList(pageNum, pageSize);
                //comment like
                ViewBag.CommentLike = _appdb.Like_Video_Comment.ToList();
                video.View_count++;
                _appdb.SaveChanges();
                return View(video);
            }
            else
            {
                int pageSize = 5;
                int pageNum = (page ?? 1);
                var video = _appdb.Videos.Where(s => s.Video_id == id && s.Video_status == "1" && s.Genre.Genre_status == "1").OrderByDescending(s => s.Video_id).ToList().FirstOrDefault();
                ViewBag.NewVideoRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1").OrderByDescending(s => s.Release_date).Take(5).ToList();
                //related Video
                List<Video> relatedvideo = _appdb.Videos.Where(s => s.Singer.Singer_id == video.Singer.Singer_id && s.Video_id != video.Video_id && s.Video_status == "1").OrderByDescending(s => s.Video_id).Take(6).ToList();
                ViewBag.RelatedVideo = relatedvideo;
                //relate genre
                List<Video> relatedVideogenre = _appdb.Videos.Where(s => s.Genre.Genre_id == video.Genre.Genre_id && s.Video_id != video.Video_id && s.Video_status == "1").OrderByDescending(s => s.Video_id).Take(6).ToList();
                ViewBag.RelatedVideoGenre = relatedVideogenre;
                //count loved Video
                ViewBag.ReactLoveVideo = _appdb.Video_Love_React.ToList();
                //count love Video
                ViewBag.CountLoveVideo = _appdb.Video_Love_React.Where(s => s.Video_id == video.Video_id).Count();
                //count video music comment
                ViewBag.countcomment = _appdb.Video_Comment.Where(m => m.Video_id == video.Video_id).Count();
                //comment
                var comment = _appdb.Video_Comment.Where(c => c.Video_id == video.Video_id).OrderByDescending(c => c.Video_comment_id).ToList();
                ViewBag.VideoComment = comment.ToPagedList(pageNum, pageSize);
                video.View_count++;
                _appdb.SaveChanges();
                return View(video);
            }
        }

        //suggest search
        [HttpPost]
        public JsonResult SuggestVideoSearch(string Prefix)
        {
            if (User.Identity.GetRole() == 2)
            {
                var searchvideo = (from s in _appdb.Videos
                                  where s.Video_status == "1" && s.Area_id == 1 && (s.Video_name.Contains(Prefix) || s.Genre.Genre_name.ToString().Contains(Prefix) || s.Genre.Area.Area_name.Contains(Prefix))
                                  orderby s.Video_name ascending
                                  select new DTOofVideo
                                  {
                                      Video_name = s.Video_name,
                                      Video_id = s.Video_id,
                                      Image = s.Image,
                                  });
                return Json(searchvideo, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var searchvideo = (from s in _appdb.Videos
                                  where s.Video_status == "1" && (s.Video_name.Contains(Prefix) || s.Genre.Genre_name.ToString().Contains(Prefix) || s.Genre.Area.Area_name.Contains(Prefix))
                                  orderby s.Video_name ascending
                                  select new DTOofVideo
                                  {
                                      Video_name = s.Video_name,
                                      Video_id = s.Video_id,
                                      Image = s.Image,
                                  });
                return Json(searchvideo, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReactLoveVideo(Video_Love_React Video_Love_React, int Video_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int User_id = User.Identity.GetUserId();
                    var video = _appdb.Video_Love_React.FirstOrDefault(m => m.Video_id == Video_id && m.User_id == User_id);
                    if (video != null)
                    {
                        _appdb.Video_Love_React.Remove(video);
                    }
                    else
                    {
                        Video_Love_React.User_id = User.Identity.GetUserId();
                        Video_Love_React.Create_date = DateTime.Now;
                        Video_Love_React.Video_id = Video_id;
                        Video_Love_React.React_love = "1";
                        _appdb.Video_Love_React.Add(Video_Love_React);
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

        [ValidateInput(false)]
        public JsonResult CommentVideo(Video_Comment commentvideo, String commentcontent)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    commentvideo.User_id = User.Identity.GetUserId();
                    commentvideo.Create_date = DateTime.Now;
                    commentvideo.Content = commentcontent;
                    _appdb.Video_Comment.Add(commentvideo);
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
        public JsonResult LikeCommentVideo(Like_Video_Comment commentvideoLikes, int videocomment_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int Userid = User.Identity.GetUserId();
                    var comment = _appdb.Like_Video_Comment.FirstOrDefault(m => m.Video_comment_id == videocomment_id && m.User_id == Userid);
                    if (comment != null)
                    {
                        _appdb.Like_Video_Comment.Remove(comment);
                    }
                    else
                    {
                        commentvideoLikes.User_id = User.Identity.GetUserId();
                        commentvideoLikes.Create_date = DateTime.Now;
                        commentvideoLikes.Video_comment_id = videocomment_id;
                        commentvideoLikes.React_like = "1";
                        _appdb.Like_Video_Comment.Add(commentvideoLikes);
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
    }
}