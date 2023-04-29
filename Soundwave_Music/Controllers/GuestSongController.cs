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
    public class GuestSongController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //list song area
        public PartialViewResult ListArea()
        {
            ViewBag.songarea = _appdb.Genres.ToList();
            return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && a.Area_id == 1 && (a.Genres.Count > 0)).ToList());
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
            ViewBag.SongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();            
            //TOP LOVED SONGS:
            List<Song> toplovedsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Song_Love_React.Count()).Take(6).ToList();
            ViewBag.TopLovedSong = toplovedsong;
            //HOT SONGS
            List<Song> hotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(4).ToList();
            ViewBag.HotSong = hotsong;
            //TRENDING
            List<Song> trendingsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).ThenByDescending(s => s.Release_date).Take(5).ToList();
            ViewBag.Trending = trendingsong;
            //TOP 10 SONG OF MONTH
            List<Song> topsongofmonth = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList();
            List<Song> topsongofmonth1 = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1 && s.Release_date.Month == today.Month).OrderByDescending(s => s.View_count).Take(10).ToList();
            if (topsongofmonth.Count() > 0)
            {
                ViewBag.TopSongOfMoth = topsongofmonth;
            }
            else
            {
                ViewBag.TopSongOfMoth = topsongofmonth1;
            }
            return View();
        }

        //Top songs
        public ActionResult TopSong()
        {
            ViewBag.TopVietnamSong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList(); ;
            //Top Vietnam song
            List<Song> topvietnamsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.View_count).Take(5).ToList();
            ViewBag.TopVietnamSong = topvietnamsong;
            return View();
        }

        //Song new release
        public ActionResult SongNewRelease()
        {
            // SONG NEW RELEASE
            // => Vietnam
            ViewBag.NewVietnamSong = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(5).ToList();
            return View();
        }

        //Top loved song
        public ActionResult TopLovedSong()
        {
            // => Vietnam
            List<Song> toplovedvnsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Song_Love_React.Count()).Take(5).ToList();
            ViewBag.TopLovedVnSong = toplovedvnsong;
            return View();
        }

        //Top hot song
        public ActionResult HotSong()
        {
            // => Vietnam
            List<Song> vnhotsong = _appdb.Songs.Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.View_count + s.Song_Love_React.Count())).Take(5).ToList();
            ViewBag.VnHotSong = vnhotsong;
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
            return View("Song", GetSong(s => s.Song_status == "1", page));
        }

        //List all song in area
        public ActionResult ListAllSong(int? page)
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
            ViewBag.CountViewSong = _appdb.Song_Love_React.ToList();
            return View("Song", GetSong(s => s.Song_status == "1" && s.Area_id == 1, page));
        }

        //Song Details
        public ActionResult SongDetail(int? page, int? id)
        {
            ViewBag.SongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(4).ToList();

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
            //loved song
            ViewBag.ReactLoveSong = _appdb.Song_Love_React.ToList();
            //host song
            ViewBag.HostSongComment = _appdb.Song_Comment.ToList();
            //count love song
            ViewBag.CountLoveSong = _appdb.Song_Love_React.Where(s => s.Song_id == song.Song_id).Count();
            song.View_count++;
            _appdb.SaveChanges();
            return View(song);
        }

        //search song
        public ActionResult SearchSongResult(string searchsongguest, int? page)
        {
            List<DTOofSong> listsongresult = (from s in _appdb.Songs
                                              where s.Song_name.Contains(searchsongguest) && s.Song_status == "1" && s.Area_id == 1
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

            return View("Song", GetSong(g => g.Song_status == "1" && g.Area_id == 1 && g.Song_name.Contains(searchsongguest), page));
        }

        //suggest search
        [HttpPost]
        public JsonResult SuggestSongSearch(string Prefix)
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
    }
}