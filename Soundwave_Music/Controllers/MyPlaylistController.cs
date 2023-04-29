using Soundwave_Music.Common.Helper;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Soundwave_Music.Controllers
{
    public class MyPlaylistController : Controller
    {
        SoundwaveDbContext db = new SoundwaveDbContext();

        public ActionResult PlaylistIndex()
        {
            int userid = User.Identity.GetUserId();

            List<Playlist> getallplaylist = db.Playlists.Where(p => p.User_id == userid).ToList();
            ViewBag.GetPlaylist = getallplaylist;
            return View();
        }

        public ActionResult PlaylistDetail(int id, int? page)
        {
            int pagesize = 5;
            int cpage = page ?? 1;
            var playlist = db.Playlists.Where(m => m.Playlist_id == id).OrderByDescending(m => m.Playlist_id).ToList().FirstOrDefault();
            ViewBag.SongInPlaylist = db.PlaylistSongs.Where(m => m.Playlist_id == playlist.Playlist_id).ToList();
            return View(playlist);
        }
    }
}