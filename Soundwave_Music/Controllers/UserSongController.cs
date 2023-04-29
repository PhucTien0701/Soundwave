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
    public class UserSongController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //list song area
        public PartialViewResult ListArea()
        {
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.songarea = _appdb.Genres.ToList();
                return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && a.Area_id == 1 && (a.Genres.Count > 0)).ToList());
            }
            else
            {
                ViewBag.songarea = _appdb.Genres.ToList();
                return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && (a.Genres.Count > 0)).ToList());
            }
        }

        //Song Index View
        public ActionResult SongIndex()
        {
            DateTime today = DateTime.Today;
            // Song Genre List
            ViewBag.SongGenre = _appdb.Genres.Where(g => g.Genre_status == "1").ToList();
            // Song Area List
            ViewBag.SongArea = _appdb.Areas.Where(a => a.Area_status == "1").ToList();
            // Singer
            ViewBag.Singer = _appdb.Singers.Where(s => s.Singer_status == "1").ToList();
            //Count loved song
            ViewBag.CountLoveSong = _appdb.Song_Love_React.ToList();
            // New Release
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.SongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
            }
            else
            {
                ViewBag.SongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1").OrderByDescending(s => s.Release_date).Take(5).ToList();
            }
            //TOP LOVED SONGS:
            if (User.Identity.GetRole() == 2)
            {
                List<Song> toplovedsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Song_Love_React.Count()).Take(6).ToList();
                ViewBag.TopLovedSong = toplovedsong;
            }
            else
            {
                List<Song> toplovedsong = _appdb.Songs.Where(s => s.Song_status == "1").OrderByDescending(s => s.Song_Love_React.Count()).Take(6).ToList();
                ViewBag.TopLovedSong = toplovedsong;
            }

            //HOT SONGS
            if (User.Identity.GetRole() == 2)
            {
                List<Song> hotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(4).ToList();
                ViewBag.HotSong = hotsong;
            }
            else
            {
                List<Song> hotsong = _appdb.Songs.Where(s => s.Song_status == "1").OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(4).ToList();
                ViewBag.HotSong = hotsong;
            }

            //TRENDING
            if (User.Identity.GetRole() == 2)
            {
                List<Song> trendingsong = _appdb.Songs.OrderByDescending(s => s.View_count).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
                ViewBag.Trending = trendingsong;
            }
            else
            {
                List<Song> trendingsong = _appdb.Songs.OrderByDescending(s => s.View_count).Where(s => s.Song_status == "1").OrderByDescending(s => s.Release_date).Take(5).ToList();
                ViewBag.Trending = trendingsong;
            }

            //TOP 10 SONG OF MONTH
            if (User.Identity.GetRole() == 2)
            {
                List<Song> topsongofmonth = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(10).ToList();
                List<Song> topsongofmonth1 = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1 && s.Release_date.Month == today.Month).OrderByDescending(s => s.View_count).Take(10).ToList();
                if (topsongofmonth.Count() > 0)
                {
                    ViewBag.TopSongOfMoth = topsongofmonth;
                }
                else
                {
                    ViewBag.TopSongOfMoth = topsongofmonth1;
                }
            }
            else
            {
                List<Song> topsongofmonth = _appdb.Songs.Where(s => s.Song_status == "1").OrderByDescending(s => s.View_count).Take(10).ToList();
                List<Song> topsongofmonth1 = _appdb.Songs.Where(s => s.Song_status == "1" && s.Release_date.Month == today.Month).OrderByDescending(s => s.View_count).Take(10).ToList();
                if (topsongofmonth.Count() > 0)
                {
                    ViewBag.TopSongOfMoth = topsongofmonth;
                }
                else
                {
                    ViewBag.TopSongOfMoth = topsongofmonth1;
                }
            }
            return View();
        }

        //Top songs
        public ActionResult TopSong()
        {
            ViewBag.TopVietnamSong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList(); ;
            if (User.Identity.GetRole() == 2)
            {
                List<Song> topvietnamsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopVietnamSong = topvietnamsong;
            }
            else
            {
                //Top Vietnam Song
                List<Song> topvietnamsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopVietnamSong = topvietnamsong;
                //Top US-UK Song
                List<Song> topusuksong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 2).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopUSUKSong = topusuksong;
                //Top Korea Song
                List<Song> topkoreasong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 3).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopKoreaSong = topkoreasong;
                //Top Japan Song
                List<Song> topjapansong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 4).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopJapanSong = topjapansong;
                //Top China Song
                List<Song> topchinasong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 5).OrderByDescending(s => s.View_count).Take(5).ToList();
                ViewBag.TopChinaSong = topchinasong;
            }
            return View();
        }

        //Song new release
        public ActionResult SongNewRelease()
        {
            // SONG NEW RELEASE
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                ViewBag.NewVietnamSong = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
            }
            else
            {
                // => Vietnam
                ViewBag.NewVietnamSong = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
                // => US-UK:
                ViewBag.NewUSUKSong = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Release_date).Take(5).ToList();
                // => Korea:
                ViewBag.NewKoreaSong = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Release_date).Take(5).ToList();
                // => Japan:
                ViewBag.NewJapanSong = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Release_date).Take(5).ToList();
                // => China
                ViewBag.NewChinaSong = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Release_date).Take(5).ToList();
            }
            return View();
        }

        //Top loved song
        public ActionResult TopLovedSong()
        {
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                List<Song> toplovedvnsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Song_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedVnSong = toplovedvnsong;
            }
            else
            {
                // => Vietnam
                List<Song> toplovedvnsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Song_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedVnSong = toplovedvnsong;
                // => US-UK
                List<Song> toplovedussong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Song_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedUsSong = toplovedussong;
                // => Korea
                List<Song> toplovedkosong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Song_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedKoSong = toplovedkosong;
                // => Japan
                List<Song> toplovedjapsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Song_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedJapSong = toplovedjapsong;
                // => China
                List<Song> toplovedchinasong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Song_Love_React.Count()).Take(5).ToList();
                ViewBag.TopLovedChinaSong = toplovedchinasong;
            }
            return View();
        }

        //Top hot song
        public ActionResult HotSong()
        {
            if (User.Identity.GetRole() == 2)
            {
                // => Vietnam
                List<Song> vnhotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(5).ToList();
                ViewBag.VnHotSong = vnhotsong;
            }
            else
            {
                // => Vietnam
                List<Song> vnhotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(5).ToList();
                ViewBag.VnHotSong = vnhotsong;
                // => US-UK
                List<Song> ushotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 2).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(5).ToList();
                ViewBag.UsHotSong = ushotsong;
                // => Korea
                List<Song> kohotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 3).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(5).ToList();
                ViewBag.KoHotSong = kohotsong;
                // => Japan
                List<Song> japhotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 4).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(5).ToList();
                ViewBag.JapHotSong = japhotsong;
                // => China
                List<Song> chinahotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 5).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(5).ToList();
                ViewBag.ChinaHotSong = chinahotsong;
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

        public ActionResult Song(int? page, int? id)
        {
            ViewBag.songgenrename = _appdb.Genres.FirstOrDefault().Genre_name;
            ViewBag.GenreName = _appdb.Genres.Where(g => g.Genre_id == id).FirstOrDefault().Genre_name;
            ViewBag.ReactLoveSong = _appdb.Song_Love_React.ToList();
            var genre = _appdb.Genres.Where(g => g.Genre_status == "1" && g.Genre_id == id && (g.Songs.Count() > 0)).FirstOrDefault();
            return View("Song", GetSongInGenre(g => g.Song_status == "1" && g.Genre.Genre_id == genre.Genre_id, page));
        }

        //Get song genre
        private IPagedList GetGenre(Expression<Func<Genre, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listgenre = _appdb.Genres.Where(expression).OrderByDescending(g => g.Genre_id).ToPagedList(pageNum, pageSize);
            return listgenre;
        }
        //Ger song
        private IPagedList GetSongInGenre(Expression<Func<Song, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listsong = _appdb.Songs.Where(expression).OrderByDescending(s => s.Song_id).ToPagedList(pageNum, pageSize);
            return listsong;
        }
        private IPagedList GetSongArea(Expression<Func<Genre, bool>> expression, int? page)
        {
            int pageSize = 15;
            int pageNum = (page ?? 1);
            var list = _appdb.Genres.Where(expression).OrderByDescending(g => g.Genre_id).ToPagedList(pageNum, pageSize);
            return list;
        }

        //List song of singer
        public ActionResult SingerSong(int? page, int? size, int id)
        {
            var singer = _appdb.Singers.Where(s => s.Singer_id == id).FirstOrDefault();
            ViewBag.CountSong = singer.Songs.Count();
            ViewBag.SingerName = singer.Singer_name;
            ViewBag.SingerImage = singer.Image;
            ViewBag.ListSingerName = singer.Singer_name;
            var pageSize = size ?? 10;
            var pageNum = page ?? 1;
            var listsong = from s in _appdb.Songs
                           join si in _appdb.Singers on s.Singer_id equals si.Singer_id
                           where si.Singer_status == "1" && si.Singer_id == id
                           orderby s.Song_id descending
                           select new DTOofSong
                           {
                               Song_id = s.Song_id,
                               Release_date = s.Release_date,
                               Image = s.Image,
                               Song_name = s.Song_name,
                           };
            ViewBag.CountSong = listsong.Count();
            return View(listsong.ToPagedList(pageNum, pageSize));
        }

        //List song of composer
        public ActionResult ComposerSong(int? page, int? size, int id)
        {
            var composer = _appdb.Composers.Where(s => s.Composer_id == id).FirstOrDefault();
            ViewBag.CountSong = composer.Songs.Count();
            ViewBag.ComposerName = composer.Composer_name;
            ViewBag.ComposerImage = composer.Image;
            ViewBag.ListComposerName = composer.Composer_name;
            var pageSize = size ?? 10;
            var pageNum = page ?? 1;
            var listsong = from s in _appdb.Songs
                           join co in _appdb.Composers on s.Composer_id equals co.Composer_id
                           where co.Composer_status == "1" && co.Composer_id == id
                           orderby s.Song_id descending
                           select new DTOofSong
                           {
                               Song_id = s.Song_id,
                               Release_date = s.Release_date,
                               Image = s.Image,
                               Song_name = s.Song_name,
                           };
            ViewBag.CountSong = listsong.Count();
            return View(listsong.ToPagedList(pageNum, pageSize));
        }

        //search songs
        public ActionResult SearchSongResult(string searchsong, int? page)
        {
            if (User.Identity.GetRole() == 2)
            {
                List<DTOofSong> listsongresult = (from s in _appdb.Songs
                                                  where s.Song_name.Contains(searchsong) && s.Song_status == "1" && s.Area_id == 1
                                                  orderby s.Song_id descending
                                                  select new DTOofSong
                                                  {
                                                      Song_name = s.Song_name,
                                                      Release_date = s.Release_date,
                                                      songs_comment = s.Song_Comment.Count(),
                                                      songs_love_react = s.Song_Love_React.Count(),
                                                      Singer = s.Singer.Singer_name,
                                                      Composer = s.Composer.Composer_name,
                                                      Image = s.Image
                                                  }).ToList();
                var list = _appdb.Songs.OrderByDescending(s => s.Song_id);
                ViewBag.listsongresult = listsongresult;
                ViewBag.ListName = "Search Song";
                //Count comment
                ViewBag.CountComment = _appdb.Song_Comment.ToList();
                //React love to song
                ViewBag.ReactLoveSong = _appdb.Song_Love_React.ToList();

                return View("Song", GetSong(g => g.Song_status == "1" && g.Area_id == 1 && g.Song_name.Contains(searchsong), page));
            }
            else
            {
                List<DTOofSong> listsongresult = (from s in _appdb.Songs
                                                  where s.Song_name.Contains(searchsong) && s.Song_status == "1"
                                                  orderby s.Song_id descending
                                                  select new DTOofSong
                                                  {
                                                      Song_name = s.Song_name,
                                                      Release_date = s.Release_date,
                                                      songs_comment = s.Song_Comment.Count(),
                                                      songs_love_react = s.Song_Love_React.Count(),
                                                      Singer = s.Singer.Singer_name,
                                                      Composer = s.Composer.Composer_name,
                                                      Image = s.Image
                                                  }).ToList();
                var list = _appdb.Songs.OrderByDescending(s => s.Song_id);
                ViewBag.listsongresult = listsongresult;
                ViewBag.ListName = "Search Song";
                //Count comment
                ViewBag.CountComment = _appdb.Song_Comment.ToList();
                //React love to song
                ViewBag.ReactLoveSong = _appdb.Song_Love_React.ToList();

                return View("Song", GetSong(g => g.Song_status == "1" && g.Song_name.Contains(searchsong), page));
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

        //Get song
        private IPagedList GetSong(Expression<Func<Song, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.musicarea = _appdb.Areas.ToList();
            ViewBag.genre = _appdb.Genres.ToList();
            var listsong = _appdb.Songs.Where(expression).OrderByDescending(s => s.Song_id).ToPagedList(pageNum, pageSize);
            return listsong;
        }

        //Get song list in genre
        public ActionResult ListSongInGenre(int? page, int? id)
        {
            List<DTOofSong> listsong = (from s in _appdb.Songs
                                        where s.Genre.Genre_id == id && s.Song_status == "1"
                                        orderby s.Song_id descending
                                        select new DTOofSong
                                        {
                                            Song_name = s.Song_name,
                                            Release_date = s.Release_date,
                                            Composer = s.Composer.Composer_name,
                                            Singer = s.Singer.Singer_name,
                                            Image = s.Image,
                                        }).ToList();
            var songgenre = _appdb.Genres.Where(g => g.Genre_id == id && g.Genre_status == "1").FirstOrDefault();
            ViewBag.SongGenre = songgenre.Genre_name;
            ViewBag.ReactLoveSong = _appdb.Song_Love_React.ToList();
            return View("Song", GetSong(s => s.Song_status == "1", page));
        }
        
        //List all song in area
        public ActionResult ListAllSong(int? page)
        {
            if (User.Identity.GetRole() == 2)
            {
                List<DTOofSong> listsong = (from s in _appdb.Songs
                                            where s.Song_status == "1" && s.Area_id == 1
                                            orderby s.Release_date descending
                                            select new DTOofSong
                                            {
                                                Song_name = s.Song_name,
                                                Release_date = s.Release_date,
                                                Composer = s.Composer.Composer_name,
                                                Singer = s.Singer.Singer_name,
                                                Image = s.Image
                                            }).ToList();
                ViewBag.ListName = "All Song";
                ViewBag.ReactLoveSong = _appdb.Song_Love_React.ToList();
                return View("Song", GetSong(s => s.Song_status == "1" && s.Area_id == 1, page));
            }
            else
            {
                List<DTOofSong> listsong = (from s in _appdb.Songs
                                            where s.Song_status == "1"
                                            orderby s.Release_date descending
                                            select new DTOofSong
                                            {
                                                Song_name = s.Song_name,
                                                Release_date = s.Release_date,
                                                Composer = s.Composer.Composer_name,
                                                Singer = s.Singer.Singer_name,
                                                Image = s.Image
                                            }).ToList();
                ViewBag.ListName = "All Song";
                ViewBag.ReactLoveSong = _appdb.Song_Love_React.ToList();
                return View("Song", GetSong(s => s.Song_status == "1", page));
            }            
        }

        //Song Details
        public ActionResult SongDetail(int? page, int? id)
        {
            ViewBag.SongCommentLike = _appdb.Like_Song_Comment.ToList();
            //song new release
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.SongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(4).ToList();
            }
            else
            {
                ViewBag.SongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1").OrderByDescending(s => s.Release_date).Take(4).ToList();
            }
            //song details
            if (User.Identity.GetRole() == 2)
            {
                int pageSize = 5;
                int pageNum = (page ?? 1);
                var song = _appdb.Songs.Where(s => s.Song_id == id && s.Song_status == "1" && s.Genre.Genre_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Song_id).ToList().FirstOrDefault();
                ViewBag.NewSongRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
                //relate song
                List<Song> relatedsong = _appdb.Songs.Where(s => s.Singer.Singer_id == song.Singer.Singer_id && s.Song_id != song.Song_id && s.Song_status == "1").OrderByDescending(s => s.Song_id).Take(6).ToList();
                ViewBag.RelatedSong = relatedsong;
                //relate genre
                List<Song> relatedsonggenre = _appdb.Songs.Where(s => s.Genre.Genre_id == song.Genre.Genre_id && s.Song_id != song.Song_id && s.Song_status == "1").OrderByDescending(s => s.Song_id).Take(6).ToList();
                ViewBag.RelatedSongGenre = relatedsonggenre;
                //host song
                ViewBag.HostSongComment = _appdb.Song_Comment.ToList();
                //count love song
                ViewBag.CountLoveSong = _appdb.Song_Love_React.Where(s => s.Song_id == song.Song_id).Count();
                //count song comment
                ViewBag.countcomment = _appdb.Song_Comment.Where(m => m.Song_id == song.Song_id).Count();
                //comment
                var comment = _appdb.Song_Comment.Where(c => c.Song_id == song.Song_id).OrderByDescending(c => c.Song_comment_id).ToList();
                ViewBag.SongComment = comment.ToPagedList(pageNum, pageSize);
                //comment like
                ViewBag.CommentLike = _appdb.Like_Song_Comment.ToList();
                song.View_count++;
                _appdb.SaveChanges();
                return View(song);
            }
            else
            {
                int pageSize = 5;
                int pageNum = (page ?? 1);
                var song = _appdb.Songs.Where(s => s.Song_id == id && s.Song_status == "1" && s.Genre.Genre_status == "1").OrderByDescending(s => s.Song_id).ToList().FirstOrDefault();
                ViewBag.NewSongRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1").OrderByDescending(s => s.Release_date).Take(5).ToList();
                //related song
                List<Song> relatedsong = _appdb.Songs.Where(s => s.Singer.Singer_id == song.Singer.Singer_id && s.Song_id != song.Song_id && s.Song_status == "1").OrderByDescending(s => s.Song_id).Take(6).ToList();
                ViewBag.RelatedSong = relatedsong;
                //relate genre
                List<Song> relatedsonggenre = _appdb.Songs.Where(s => s.Genre.Genre_id == song.Genre.Genre_id && s.Song_id != song.Song_id && s.Song_status == "1").OrderByDescending(s => s.Song_id).Take(6).ToList();
                ViewBag.RelatedSongGenre = relatedsonggenre;
                //count love song
                ViewBag.CountLoveSong = _appdb.Song_Love_React.Where(s => s.Song_id == song.Song_id).Count();
                //count like song comment
                ViewBag.countcomment = _appdb.Song_Comment.Where(m => m.Song_id == song.Song_id).Count();
                //comment
                var comment = _appdb.Song_Comment.Where(c => c.Song_id == song.Song_id).OrderByDescending(c => c.Song_comment_id).ToList();
                ViewBag.SongComment = comment.ToPagedList(pageNum, pageSize);
                song.View_count++;
                _appdb.SaveChanges();
                return View(song);
            }
        }

        //suggest search
        [HttpPost]
        public JsonResult SuggestSongSearch(string Prefix)
        {
            if (User.Identity.GetRole() == 2)
            {
                var searchsong = (from s in _appdb.Songs
                              where s.Song_status == "1" && s.Area_id == 1 && (s.Song_name.Contains(Prefix) || s.Genre.Genre_name.ToString().Contains(Prefix) || s.Genre.Area.Area_name.Contains(Prefix))
                              orderby s.Song_name ascending
                              select new DTOofSong
                              {
                                  Song_name = s.Song_name,
                                  Song_id = s.Song_id,
                                  Image = s.Image,
                              });
                return Json(searchsong, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var searchsong = (from s in _appdb.Songs
                              where s.Song_status == "1" && (s.Song_name.Contains(Prefix) || s.Genre.Genre_name.ToString().Contains(Prefix) || s.Genre.Area.Area_name.Contains(Prefix))
                              orderby s.Song_name ascending
                              select new DTOofSong
                              {
                                  Song_name = s.Song_name,
                                  Song_id = s.Song_id,
                                  Image = s.Image,
                              });
                return Json(searchsong, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReactLoveSong(Song_Love_React Song_Love_React, int Song_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int User_id = User.Identity.GetUserId();
                    var song = _appdb.Song_Love_React.FirstOrDefault(m => m.Song_id == Song_id && m.User_id == User_id);
                    if (song != null)
                    {
                        _appdb.Song_Love_React.Remove(song);
                    }
                    else
                    {
                        Song_Love_React.User_id = User.Identity.GetUserId();
                        Song_Love_React.Create_date = DateTime.Now;
                        Song_Love_React.Song_id = Song_id;
                        Song_Love_React.React_love = "1";
                        _appdb.Song_Love_React.Add(Song_Love_React);
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
        public JsonResult CommentSong(Song_Comment commentsong, String commentcontent)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    commentsong.User_id = User.Identity.GetUserId();
                    commentsong.Create_date = DateTime.Now;
                    commentsong.Content = commentcontent;
                    _appdb.Song_Comment.Add(commentsong);
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
        public JsonResult LikeCommentSong(Like_Song_Comment commentsongLikes, int songcomment_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int Userid = User.Identity.GetUserId();
                    var comment = _appdb.Like_Song_Comment.FirstOrDefault(m => m.Song_comment_id == songcomment_id && m.User_id == Userid);
                    if (comment != null)
                    {
                        _appdb.Like_Song_Comment.Remove(comment);
                    }
                    else
                    {
                        commentsongLikes.User_id = User.Identity.GetUserId();
                        commentsongLikes.Create_date = DateTime.Now;
                        commentsongLikes.Song_comment_id = songcomment_id;
                        commentsongLikes.React_like = "1";
                        _appdb.Like_Song_Comment.Add(commentsongLikes);
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