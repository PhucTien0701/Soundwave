using Soundwave_Music.Common.Helper;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Soundwave_Music.Controllers
{
    public class HomeController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();
        public ActionResult Index()
        {
            // show singer
            ViewBag.Singer = _appdb.Singers.Where(s => s.Singer_status == "1").ToList();
            // show new release song
            if (User.Identity.IsAuthenticated == false)
            {
                //vietnam
                ViewBag.VNSongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(6).ToList();
            }
            else
            {
                if (User.Identity.GetRole() == 2)
                {
                    //vietnam
                    ViewBag.VNSongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(6).ToList();                    
                }
                else if (User.Identity.GetRole() == 1 || User.Identity.GetRole() == 3)
                {
                    //vietnam
                    ViewBag.VNSongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(6).ToList();
                    //US-UK
                    ViewBag.USUKSongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Release_date).Take(6).ToList();
                    //Korea
                    ViewBag.KoreaSongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Release_date).Take(6).ToList();
                    //Japan
                    ViewBag.JapanSongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Release_date).Take(6).ToList();
                    //China
                    ViewBag.ChinaSongNewRelease = _appdb.Songs.OrderByDescending(s => s.Song_id).Where(s => s.Song_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Release_date).Take(6).ToList();
                }
            }
            //show new release album
            if (User.Identity.IsAuthenticated == false)
            {
                //vietnam
                ViewBag.VNAlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(6).ToList();
            }
            else
            {
                if (User.Identity.GetRole() == 2)
                {
                    //vietnam
                    ViewBag.VNAlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(6).ToList();
                }
                else if (User.Identity.GetRole() == 1 || User.Identity.GetRole() == 3)
                {
                    //vietnam
                    ViewBag.VNAlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Create_date).Take(6).ToList();
                    //US-UK
                    ViewBag.USUKAlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Create_date).Take(6).ToList();
                    //Korea
                    ViewBag.KoreaAlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Create_date).Take(6).ToList();
                    //Japan
                    ViewBag.JapanAlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Create_date).Take(6).ToList();
                    //China
                    ViewBag.ChinaAlbumNewRelease = _appdb.Albums.OrderByDescending(s => s.Album_id).Where(s => s.Album_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Create_date).Take(6).ToList();
                }
            }
            //show new release video music
            if (User.Identity.IsAuthenticated == false)
            {
                //vietnam
                ViewBag.VNVideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(6).ToList();
            }
            else
            {
                if (User.Identity.GetRole() == 2)
                {
                    //vietnam
                    ViewBag.VNVideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(6).ToList();
                }
                else if (User.Identity.GetRole() == 1 || User.Identity.GetRole() == 3)
                {
                    //vietnam
                    ViewBag.VNVideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 1).OrderByDescending(s => s.Release_date).Take(6).ToList();
                    //US-UK
                    ViewBag.USUKVideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 2).OrderByDescending(s => s.Release_date).Take(6).ToList();
                    //Korea
                    ViewBag.KoreaVideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 3).OrderByDescending(s => s.Release_date).Take(6).ToList();
                    //Japan
                    ViewBag.JapanVideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 4).OrderByDescending(s => s.Release_date).Take(6).ToList();
                    //China
                    ViewBag.ChinaVideoNewRelease = _appdb.Videos.OrderByDescending(s => s.Video_id).Where(s => s.Video_status == "1" && s.Area_id == 5).OrderByDescending(s => s.Release_date).Take(6).ToList();
                }
            }
            return View();
        }
    }
}