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
    public class GuestAlbumController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //list Album area
        public PartialViewResult ListArea()
        {
            ViewBag.albumarea = _appdb.Genres.ToList();
            return PartialView("ListArea", _appdb.Areas.Where(a => a.Area_status == "1" && a.Area_id == 1 && (a.Genres.Count > 0)).ToList());
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
            ViewBag.AlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(5).ToList();
            //TOP LOVED ALBUMS:
            List<Album> toplovedalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Album_Love_React.Count()).Take(6).ToList();
            ViewBag.TopLovedAlbum = toplovedalbum;
            //HOT ALBUMS
            List<Album> hotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(4).ToList();
            ViewBag.HotAlbum = hotalbum;
            //TRENDING
            List<Album> trendingalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).ThenByDescending(s => s.view_count).Take(5).ToList();
            ViewBag.Trending = trendingalbum;
            //TOP 10 ALBUMS OF MONTH
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
            return View();
        }

        //Top Albums
        public ActionResult TopAlbum()
        {
            ViewBag.TopVietnamAlbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.view_count).Take(5).ToList(); ;
            //Top Vietnam album
            List<Album> topvietnamalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.view_count).Take(5).ToList();
            ViewBag.TopVietnamAlbum = topvietnamalbum;
            return View();
        }

        //Album new release
        public ActionResult AlbumNewRelease()
        {
            // ALBUMS NEW RELEASE
            // => Vietnam
            ViewBag.NewVietnamAlbum = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(5).ToList();
            return View();
        }

        //Top loved Album
        public ActionResult TopLovedAlbum()
        {
            // => Vietnam
            List<Album> toplovedvnalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Album_Love_React.Count()).Take(5).ToList();
            ViewBag.TopLovedVnAlbum = toplovedvnalbum;
            return View();
        }

        //Top hot Album
        public ActionResult HotAlbum()
        {
            // => Vietnam
            List<Album> vnhotalbum = _appdb.Albums.Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => (s.view_count + s.Album_Love_React.Count())).Take(5).ToList();
            ViewBag.VnHotAlbum = vnhotalbum;
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
            var genre = _appdb.Genres.Where(g => g.Genre_status == "1" && g.Genre_id == id && (g.Albums.Count() > 0)).FirstOrDefault();
            return View("Album", GetAlbumInGenre(g => g.Album_status == "1" && g.Genre.Genre_id == genre.Genre_id, page));
        }

        //Get Album genre
        private IPagedList GetGenre(Expression<Func<Genre, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var listgenre = _appdb.Genres.Where(expression).OrderByDescending(g => g.Genre_id).ToPagedList(pageNum, pageSize);
            return listgenre;
        }
        //Ger Album
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

        //List Album of singer
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
            return View("Album", GetAlbum(s => s.Album_status == "1", page));
        }

        //List all Album in area
        public ActionResult ListAllAlbum(int? page)
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
            ViewBag.CountViewAlbum = _appdb.Album_Love_React.ToList();
            return View("Album", GetAlbum(s => s.Album_status == "1" && s.Area_id == 1, page));
        }

        //Album Details
        public ActionResult AlbumDetail(int? page, int? id)
        {
            ViewBag.AlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(4).ToList();

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
            //loved Album
            ViewBag.ReactLoveAlbum = _appdb.Album_Love_React.ToList();
            //host Album
            ViewBag.HostAlbumComment = _appdb.Album_Comment.ToList();
            //count love Album
            ViewBag.CountLoveAlbum = _appdb.Album_Love_React.Where(s => s.Album_id == album.Album_id).Count();
            //list album
            var albumsongs = _appdb.Songs.Where(s => s.Album_id == album.Album_id).OrderByDescending(c => c.Album_id).ToList();
            ViewBag.AlbumSongs = albumsongs;
            album.view_count++;
            _appdb.SaveChanges();
            return View(album);
        }
    }
}