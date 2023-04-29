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
    public class UserAlbumController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //list album area
        public PartialViewResult ListArea()
        {
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.albumarea = _appdb.Genres.ToList();
                return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && a.Area_id == 1 && (a.Genres.Count > 0)).ToList());
            }
            else
            {
                ViewBag.albumarea = _appdb.Genres.ToList();
                return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && (a.Genres.Count > 0)).ToList());
            }
        }

        //Album Index View
        public ActionResult AlbumIndex()
        {
            DateTime today = DateTime.Today;
            // Album Genre List
            ViewBag.AlbumGenre = _appdb.Genres.Where(g => g.Genre_status == "1").ToList();
            // Album Area List
            ViewBag.AlbumArea = _appdb.Areas.Where(a => a.Area_status == "1").ToList();
            // Singer
            ViewBag.Singer = _appdb.Singers.Where(s => s.Singer_status == "1").ToList();
            //Count loved Album
            ViewBag.CountLoveAlbum = _appdb.Album_Love_React.ToList();
            // New Release
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.AlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(5).ToList();
            }
            else
            {
                ViewBag.AlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1").OrderByDescending(s => s.Create_date).Take(5).ToList();
            }
            //TOP LOVED ALBUMS:
            if (User.Identity.GetRole() == 2)
            {
                List<Album> toplovedalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Album_Love_React.Count()).Take(6).ToList();
                ViewBag.TopLovedAlbum = toplovedalbum;
            }
            else
            {
                List<Album> toplovedalbum = _appdb.Albums.Where(s => s.Album_status == "1").OrderByDescending(s => s.Album_Love_React.Count()).Take(6).ToList();
                ViewBag.TopLovedAlbum = toplovedalbum;
            }

            //HOT Album
            if (User.Identity.GetRole() == 2)
            {
                List<Album> hotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(4).ToList();
                ViewBag.HotAlbum = hotalbum;
            }
            else
            {
                List<Album> hotalbum = _appdb.Albums.Where(s => s.Album_status == "1").OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(4).ToList();
                ViewBag.HotAlbum = hotalbum;
            }

            //TRENDING
            if (User.Identity.GetRole() == 2)
            {
                List<Album> trendingalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).OrderByDescending(s => s.view_count).Take(5).ToList();
                ViewBag.Trending = trendingalbum;
            }
            else
            {
                List<Album> trendingalbum = _appdb.Albums.Where(s => s.Album_status == "1").OrderByDescending(s => s.Create_date).OrderByDescending(s => s.view_count).Take(5).ToList();
                ViewBag.Trending = trendingalbum;
            }

            //TOP 10 ALBUM OF MONTH
            if (User.Identity.GetRole() == 2)
            {
                List<Album> topalbumofmonth = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.view_count).Take(10).ToList();
                List<Album> topalbumofmonth1 = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1 && s.Create_date.Month == today.Month).OrderByDescending(s => s.view_count).Take(10).ToList();
                if (topalbumofmonth.Count() > 0)
                {
                    ViewBag.TopAlbumOfMoth = topalbumofmonth;
                }
                else
                {
                    ViewBag.TopAlbumOfMoth = topalbumofmonth1;
                }
            }
            else
            {
                List<Album> topalbumofmonth = _appdb.Albums.Where(s => s.Album_status == "1").OrderByDescending(s => s.view_count).Take(10).ToList();
                List<Album> topalbumofmonth1 = _appdb.Albums.Where(s => s.Album_status == "1" && s.Create_date.Month == today.Month).OrderByDescending(s => s.view_count).Take(10).ToList();
                if (topalbumofmonth.Count() > 0)
                {
                    ViewBag.TopAlbumOfMoth = topalbumofmonth;
                }
                else
                {
                    ViewBag.TopAlbumOfMoth = topalbumofmonth1;
                }
            }
            return View();
        }

        //Top Albums
        public ActionResult TopAlbum()
        {
            ViewBag.TopVietnamAlbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.view_count).Take(5).ToList(); ;
            if (User.Identity.GetRole() == 2)
            {
                List<Album> topvietnamalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.view_count).Take(5).ToList();
                ViewBag.TopVietnamAlbum = topvietnamalbum;
            }
            else
            {
                //Top Vietnam Album
                List<Album> topvietnamalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.view_count).Take(5).ToList();
                ViewBag.TopVietnamAlbum = topvietnamalbum;
                //Top US-UK Album
                List<Album> topusukalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 2).OrderByDescending(s => s.view_count).Take(5).ToList();
                ViewBag.TopUSUKAlbum = topusukalbum;
                //Top Korea Album
                List<Album> topkoreaalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 3).OrderByDescending(s => s.view_count).Take(5).ToList();
                ViewBag.TopKoreaAlbum = topkoreaalbum;
                //Top Japan Album
                List<Album> topjapanalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 4).OrderByDescending(s => s.view_count).Take(5).ToList();
                ViewBag.TopJapanAlbum = topjapanalbum;
                //Top China Album
                List<Album> topchinaalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 5).OrderByDescending(s => s.view_count).Take(5).ToList();
                ViewBag.TopChinaAlbum = topchinaalbum;
            }
            return View();
        }

        //Album new release
        public ActionResult AlbumNewRelease()
        {
            // ALBUM NEW RELEASE
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                ViewBag.NewVietnamAlbum = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(5).ToList();
            }
            else
            {
                // => Vietnam
                ViewBag.NewVietnamAlbum = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(5).ToList();
                // => US-UK:
                ViewBag.NewUSUKAlbum = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Create_date).Take(5).ToList();
                // => Korea:
                ViewBag.NewKoreaAlbum = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Create_date).Take(5).ToList();
                // => Japan:
                ViewBag.NewJapanAlbum = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Create_date).Take(5).ToList();
                // => China
                ViewBag.NewChinaAlbum = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Create_date).Take(5).ToList();
            }
            return View();
        }

        //Top loved album
        public ActionResult TopLovedAlbum()
        {
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                List<Album> toplovedvnalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Album_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedVnAlbum = toplovedvnalbum;
            }
            else
            {
                // => Vietnam
                List<Album> toplovedvnalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Album_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedVnAlbum = toplovedvnalbum;
                // => US-UK
                List<Album> toplovedusalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Album_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedUsAlbum = toplovedusalbum;
                // => Korea
                List<Album> toplovedkoalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Album_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedKoAlbum = toplovedkoalbum;
                // => Japan
                List<Album> toplovedjapalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Album_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedJapAlbum = toplovedjapalbum;
                // => China
                List<Album> toplovedchinaalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Album_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedChinaAlbum = toplovedchinaalbum;
            }
            return View();
        }

        //Top hot album
        public ActionResult HotAlbum()
        {
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                List<Album> vnhotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(5).ToList();
                ViewBag.VnHotAlbum = vnhotalbum;
            }
            else
            {
                // => Vietnam
                List<Album> vnhotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(5).ToList();
                ViewBag.VnHotAlbum = vnhotalbum;
                // => US-UK
                List<Album> ushotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 2).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(5).ToList();
                ViewBag.UsHotAlbum = ushotalbum;
                // => Korea
                List<Album> kohotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 3).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(5).ToList();
                ViewBag.KoHotAlbum = kohotalbum;
                // => Japan
                List<Album> japhotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 4).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(5).ToList();
                ViewBag.JapHotAlbum = japhotalbum;
                // => China
                List<Album> chinahotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 5).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(5).ToList();
                ViewBag.ChinaHotAlbum = chinahotalbum;
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

        public ActionResult Album(int? page, int? id)
        {
            ViewBag.albumgenrename = _appdb.Genres.FirstOrDefault().Genre_name;
            ViewBag.GenreName = _appdb.Genres.Where(g => g.Genre_id == id).FirstOrDefault().Genre_name;
            ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();
            var genre = _appdb.Genres.Where(g => g.Genre_status == "1" && g.Genre_id == id && (g.Albums.Count() > 0)).FirstOrDefault();
            return View("Album", GetAlbumInGenre(g => g.Album_status == "1" && g.Genre.Genre_id == genre.Genre_id, page));
        }

        //Get album genre
        private IPagedList GetGenre(Expression<Func<Genre, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listgenre = _appdb.Genres.Where(expression).OrderByDescending(g => g.Genre_id).ToPagedList(pageNum, pageSize);
            return listgenre;
        }
        //Ger album
        private IPagedList GetAlbumInGenre(Expression<Func<Album, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listalbum = _appdb.Albums.Where(expression).OrderByDescending(s => s.Album_id).ToPagedList(pageNum, pageSize);
            return listalbum;
        }
        private IPagedList GetAlbumArea(Expression<Func<Genre, bool>> expression, int? page)
        {
            int pageSize = 15;
            int pageNum = (page ?? 1);
            var list = _appdb.Genres.Where(expression).OrderByDescending(g => g.Genre_id).ToPagedList(pageNum, pageSize);
            return list;
        }

        //List album of singer
        public ActionResult SingerAlbum(int? page, int? size, int id)
        {
            var singer = _appdb.Singers.Where(s => s.Singer_id == id).FirstOrDefault();
            ViewBag.CountAlbum = singer.Albums.Count();
            ViewBag.SingerName = singer.Singer_name;
            ViewBag.SingerImage = singer.Image;
            ViewBag.ListSingerName = singer.Singer_name;
            var pageSize = size ?? 10;
            var pageNum = page ?? 1;
            var listalbum = from s in _appdb.Albums
                            join si in _appdb.Singers on s.Singer_id equals si.Singer_id
                            where si.Singer_status == "1" && si.Singer_id == id
                            orderby s.Album_id descending
                            select new DTOofAlbum
                            {
                                Album_id = s.Album_id,
                                Create_date = s.Create_date,
                                Image = s.Image,
                                Album_name = s.Album_name,
                            };
            ViewBag.CountAlbum = listalbum.Count();
            return View(listalbum.ToPagedList(pageNum, pageSize));
        }

        //search Albums
        public ActionResult SearchAlbumResult(string searchalbum, int? page)
        {
            if (User.Identity.GetRole() == 2)
            {
                List<DTOofAlbum> listalbumresult = (from s in _appdb.Albums
                                                    where s.Album_name.Contains(searchalbum) && s.Album_status == "1" && s.Area_id == 1
                                                    orderby s.Album_id descending
                                                    select new DTOofAlbum
                                                    {
                                                        Album_name = s.Album_name,
                                                        Create_date = s.Create_date,
                                                        Albums_comment = s.Album_Comment.Count(),
                                                        Albums_love_react = s.Album_Love_React.Count(),
                                                        Singer = s.Singer.Singer_name,
                                                        Image = s.Image
                                                    }).ToList();
                var list = _appdb.Albums.OrderByDescending(s => s.Album_id);
                ViewBag.listalbumresult = listalbumresult;
                ViewBag.ListName = "Search Album";
                //Count comment
                ViewBag.CountComment = _appdb.Album_Comment.ToList();
                //React love to album
                ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();

                return View("Album", GetAlbum(g => g.Album_status == "1" && g.Area_id == 1 && g.Album_name.Contains(searchalbum), page));
            }
            else
            {
                List<DTOofAlbum> listalbumresult = (from s in _appdb.Albums
                                                    where s.Album_name.Contains(searchalbum) && s.Album_status == "1"
                                                    orderby s.Album_id descending
                                                    select new DTOofAlbum
                                                    {
                                                        Album_name = s.Album_name,
                                                        Create_date = s.Create_date,
                                                        Albums_comment = s.Album_Comment.Count(),
                                                        Albums_love_react = s.Album_Love_React.Count(),
                                                        Singer = s.Singer.Singer_name,
                                                        Image = s.Image
                                                    }).ToList();
                var list = _appdb.Albums.OrderByDescending(s => s.Album_id);
                ViewBag.listalbumresult = listalbumresult;
                ViewBag.ListName = "Search Album";
                //Count comment
                ViewBag.CountComment = _appdb.Album_Comment.ToList();
                //React love to album
                ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();

                return View("Album", GetAlbum(g => g.Album_status == "1" && g.Album_name.Contains(searchalbum), page));
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

        //Get Album
        private IPagedList GetAlbum(Expression<Func<Album, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.musicarea = _appdb.Areas.ToList();
            ViewBag.genre = _appdb.Genres.ToList();
            var listalbum = _appdb.Albums.Where(expression).OrderByDescending(s => s.Album_id).ToPagedList(pageNum, pageSize);
            return listalbum;
        }

        //Get Album list in genre
        public ActionResult ListAlbumInGenre(int? page, int? id)
        {
            List<DTOofAlbum> listalbum = (from s in _appdb.Albums
                                          where s.Genre.Genre_id == id && s.Album_status == "1"
                                          orderby s.Album_id descending
                                          select new DTOofAlbum
                                          {
                                              Album_name = s.Album_name,
                                              Create_date = s.Create_date,
                                              Singer = s.Singer.Singer_name,
                                              Image = s.Image,
                                          }).ToList();
            var albumgenre = _appdb.Genres.Where(g => g.Genre_id == id && g.Genre_status == "1").FirstOrDefault();
            ViewBag.AlbumGenre = albumgenre.Genre_name;
            //React love to album
            ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();
            return View("Album", GetAlbum(s => s.Album_status == "1", page));
        }

        //List all Album in area
        public ActionResult ListAllAlbum(int? page)
        {
            if (User.Identity.GetRole() == 2)
            {
                List<DTOofAlbum> listalbum = (from s in _appdb.Albums
                                              where s.Album_status == "1" && s.Area_id == 1
                                              orderby s.Create_date descending
                                              select new DTOofAlbum
                                              {
                                                  Album_name = s.Album_name,
                                                  Create_date = s.Create_date,
                                                  Singer = s.Singer.Singer_name,
                                                  Image = s.Image
                                              }).ToList();
                ViewBag.ListName = "All Album";
                //React love to album
                ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();
                return View("Album", GetAlbum(s => s.Album_status == "1" && s.Area_id == 1, page));
            }
            else
            {
                List<DTOofAlbum> listalbum = (from s in _appdb.Albums
                                              where s.Album_status == "1"
                                              orderby s.Create_date descending
                                              select new DTOofAlbum
                                              {
                                                  Album_name = s.Album_name,
                                                  Create_date = s.Create_date,
                                                  Singer = s.Singer.Singer_name,
                                                  Image = s.Image
                                              }).ToList();
                ViewBag.ListName = "All Album";
                //React love to album
                ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();
                return View("Album", GetAlbum(s => s.Album_status == "1", page));
            }
        }

        //Album Details
        public ActionResult AlbumDetail(int? page, int? id)
        {
            ViewBag.AlbumCommentLike = _appdb.Like_Album_Comment.ToList();
            //Album new release
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.AlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(4).ToList();
            }
            else
            {
                ViewBag.AlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1").OrderByDescending(s => s.Create_date).Take(4).ToList();
            }
            //Album details
            if (User.Identity.GetRole() == 2)
            {
                int pageSize = 5;
                int pageNum = (page ?? 1);
                var album = _appdb.Albums.Where(s => s.Album_id == id && s.Album_status == "1" && s.Genre.Genre_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Album_id).ToList().FirstOrDefault();
                ViewBag.NewAlbumRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(5).ToList();
                //relate Album
                List<Album> relatedalbum = _appdb.Albums.Where(s => s.Singer.Singer_id == album.Singer.Singer_id && s.Album_id != album.Album_id && s.Album_status == "1").OrderByDescending(s => s.Album_id).Take(6).ToList();
                ViewBag.RelatedAlbum = relatedalbum;
                //relate genre
                List<Album> relatedalbumgenre = _appdb.Albums.Where(s => s.Genre.Genre_id == album.Genre.Genre_id && s.Album_id != album.Album_id && s.Album_status == "1").OrderByDescending(s => s.Album_id).Take(6).ToList();
                ViewBag.RelatedAlbumGenre = relatedalbumgenre;
                //loved album
                ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();
                //host album
                ViewBag.HostAlbumComment = _appdb.Album_Comment.ToList();
                //count love album
                ViewBag.CountLoveAlbum = _appdb.Album_Love_React.Where(s => s.Album_id == album.Album_id).Count();
                //comment
                var comment = _appdb.Album_Comment.Where(c => c.Album_id == album.Album_id).OrderByDescending(c => c.Album_comment_id).ToList();
                ViewBag.AlbumComment = comment.ToPagedList(pageNum, pageSize);
                //count video music comment
                ViewBag.countcomment = _appdb.Album_Comment.Where(m => m.Album_id == album.Album_id).Count();
                //comment like
                ViewBag.CommentLike = _appdb.Like_Album_Comment.ToList();
                //list album
                var albumsongs = _appdb.Songs.Where(s => s.Album_id == album.Album_id).OrderByDescending(c => c.Album_id).ToList();
                ViewBag.AlbumSongs = albumsongs;
                album.view_count++;
                _appdb.SaveChanges();
                return View(album);
            }
            else
            {
                int pageSize = 5;
                int pageNum = (page ?? 1);
                var album = _appdb.Albums.Where(s => s.Album_id == id && s.Album_status == "1" && s.Genre.Genre_status == "1").OrderByDescending(s => s.Album_id).ToList().FirstOrDefault();
                ViewBag.NewAlbumRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1").OrderByDescending(s => s.Create_date).Take(5).ToList();
                //related album
                List<Album> relatedalbum = _appdb.Albums.Where(s => s.Singer.Singer_id == album.Singer.Singer_id && s.Album_id != album.Album_id && s.Album_status == "1").OrderByDescending(s => s.Album_id).Take(6).ToList();
                ViewBag.RelatedAlbum = relatedalbum;
                //relate genre
                List<Album> relatedalbumgenre = _appdb.Albums.Where(s => s.Genre.Genre_id == album.Genre.Genre_id && s.Album_id != album.Album_id && s.Album_status == "1").OrderByDescending(s => s.Album_id).Take(6).ToList();
                ViewBag.RelatedAlbumGenre = relatedalbumgenre;
                //loved album
                ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();
                //count love album
                ViewBag.CountLoveAlbum = _appdb.Album_Love_React.Where(s => s.Album_id == album.Album_id).Count();
                //count video music comment
                ViewBag.countcomment = _appdb.Album_Comment.Where(m => m.Album_id == album.Album_id).Count();
                //comment
                var comment = _appdb.Album_Comment.Where(c => c.Album_id == album.Album_id).OrderByDescending(c => c.Album_comment_id).ToList();
                ViewBag.AlbumComment = comment.ToPagedList(pageNum, pageSize);
                //list album
                var albumsongs = _appdb.Songs.Where(s => s.Album_id == album.Album_id).OrderByDescending(c => c.Album_id).ToList();
                ViewBag.AlbumSongs = albumsongs;
                album.view_count++;
                _appdb.SaveChanges();
                return View(album);
            }
        }

        //suggest search
        [HttpPost]
        public JsonResult SuggestAlbumSearch(string Prefix)
        {
            if (User.Identity.GetRole() == 2)
            {
                var searchalbum = (from s in _appdb.Albums
                                   where s.Album_status == "1" && s.Area_id == 1 && (s.Album_name.Contains(Prefix) || s.Genre.Genre_name.ToString().Contains(Prefix) || s.Genre.Area.Area_name.Contains(Prefix))
                                   orderby s.Album_name ascending
                                   select new DTOofAlbum
                                   {
                                       Album_name = s.Album_name,
                                       Album_id = s.Album_id,
                                       Image = s.Image,
                                   });
                return Json(searchalbum, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var searchalbum = (from s in _appdb.Albums
                                   where s.Album_status == "1" && (s.Album_name.Contains(Prefix) || s.Genre.Genre_name.ToString().Contains(Prefix) || s.Genre.Area.Area_name.Contains(Prefix))
                                   orderby s.Album_name ascending
                                   select new DTOofAlbum
                                   {
                                       Album_name = s.Album_name,
                                       Album_id = s.Album_id,
                                       Image = s.Image,
                                   });
                return Json(searchalbum, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReactLoveAlbum(Album_Love_React Album_Love_React, int Album_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int User_id = User.Identity.GetUserId();
                    var album = _appdb.Album_Love_React.FirstOrDefault(m => m.Album_id == Album_id && m.User_id == User_id);
                    if (album != null)
                    {
                        _appdb.Album_Love_React.Remove(album);
                    }
                    else
                    {
                        Album_Love_React.User_id = User.Identity.GetUserId();
                        Album_Love_React.Create_date = DateTime.Now;
                        Album_Love_React.Album_id = Album_id;
                        Album_Love_React.React_love = "1";
                        _appdb.Album_Love_React.Add(Album_Love_React);
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
        public JsonResult CommentAlbum(Album_Comment commentalbum, String commentcontent)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    commentalbum.User_id = User.Identity.GetUserId();
                    commentalbum.Create_date = DateTime.Now;
                    commentalbum.Content = commentcontent;
                    _appdb.Album_Comment.Add(commentalbum);
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
        public JsonResult LikeCommentAlbum(Like_Album_Comment commentalbumLikes, int albumcomment_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int Userid = User.Identity.GetUserId();
                    var comment = _appdb.Like_Album_Comment.FirstOrDefault(m => m.Album_comment_id == albumcomment_id && m.User_id == Userid);
                    if (comment != null)
                    {
                        _appdb.Like_Album_Comment.Remove(comment);
                    }
                    else
                    {
                        commentalbumLikes.User_id = User.Identity.GetUserId();
                        commentalbumLikes.Create_date = DateTime.Now;
                        commentalbumLikes.Album_comment_id = albumcomment_id;
                        commentalbumLikes.React_like = "1";
                        _appdb.Like_Album_Comment.Add(commentalbumLikes);
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