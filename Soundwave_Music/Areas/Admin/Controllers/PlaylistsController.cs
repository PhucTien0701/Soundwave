using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.DataTransferObjectives;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Soundwave_Music.DataTransferObjectives.DTOofPlaylist;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class PlaylistsController : BaseController
    {
        private SoundwaveDbContext db = new SoundwaveDbContext();

        public ActionResult PlaylistIndex(int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                int userid = User.Identity.GetUserId();
                var pageSize = (size ?? 10);
                var pageNumber = (page ?? 1);
                
                
                var list = from a in db.Playlists
                           where a.User_id == userid
                           orderby a.Playlist_id descending
                           select a;
                return View("PlaylistIndex", list.ToPagedList(pageNumber, 50));
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        public ActionResult PlaylistCreate()
        {
            if (User.Identity.IsAuthenticated)
            {                             
                var playlistsongcheck = from p in db.Songs
                                        where p.Song_status == "1"
                                        orderby p.Song_name ascending
                                        select new
                                        {
                                            p.Song_id,
                                            p.Song_name,
                                            Checked = ((from np in db.PlaylistSongs
                                                       where (np.Song_id == p.Song_id)
                                                       select np).Count() > 0)
                                        };
                
                var MyplaylistsongCheckBoxList = new List<PlaylistSongsCheckbox>();
                

                foreach (var item in playlistsongcheck)
                {
                    MyplaylistsongCheckBoxList.Add(new PlaylistSongsCheckbox
                    {
                        id = item.Song_id,
                        name = item.Song_name,
                    });
                }

                var playlistdto = new DTOofPlaylist();
                playlistdto.Songs = MyplaylistsongCheckBoxList;
                return View(playlistdto);
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaylistCreate(DTOofPlaylist playlistdto, Playlist playlist)
        {
            try
            {
                playlist.Playlist_name = playlistdto.Playlist_name;
                playlist.Create_date = DateTime.Now;
                playlist.Create_by = User.Identity.GetName();
                playlist.Update_date = DateTime.Now;
                playlist.Update_by = User.Identity.GetName();
                playlist.User_id = User.Identity.GetUserId();                
                
                foreach (var item in playlistdto.Songs)
                {
                    if (item.Checked)
                    {
                        db.PlaylistSongs.Add(new PlaylistSong()
                        {
                            Playlist_id = playlistdto.Playlist_id,
                            Song_id = item.id,
                        });
                    }
                }
                db.Playlists.Add(playlist);
                db.SaveChanges();
                Notification.set_noti("Create playlist: " + playlist.Playlist_name + " sucsessfully", "success");
                return RedirectToAction("PlaylistIndex");
            }
            catch
            {
                Notification.set_noti("Fail", "danger");
            }
            return View(playlistdto);
        }

        public ActionResult PlaylistEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null &&
                Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("PlaylistEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                Playlist playlist = db.Playlists.Where(m => m.Playlist_id == id).FirstOrDefault();
                if (playlist == null || id == null)
                {
                    Notification.set_noti("Do not exist: " + playlist.Playlist_name + "", "warning");
                    return RedirectToAction("PlaylistIndex");
                }                
                
                var playlistsongcheck = from p in db.Songs
                                        orderby p.Song_name ascending
                                        select new
                                        {
                                            p.Song_id,
                                            p.Song_name,
                                            Checked = ((from np in db.PlaylistSongs where (np.Playlist_id == id) && (np.Song_id == p.Song_id) select np).Count() > 0)
                                        };
                var playlistdto = new DTOofPlaylist();
                playlistdto.Playlist_id = id.Value;
                playlistdto.Playlist_name = playlist.Playlist_name;
                var MyplaylistproductCheckBoxList = new List<PlaylistSongsCheckbox>();
                
                foreach (var item in playlistsongcheck)
                {
                    MyplaylistproductCheckBoxList.Add(new PlaylistSongsCheckbox
                    {
                        id = item.Song_id,
                        name = item.Song_name,
                        Checked = item.Checked
                    });
                }
                playlistdto.Songs = MyplaylistproductCheckBoxList;
                return View(playlistdto);
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaylistEdit(DTOofPlaylist playlistdto, string returnUrl)
        {
            ViewBag.products = new MultiSelectList(db.Songs.ToList());
            try
            {
                var playlist = db.Playlists.Find(playlistdto.Playlist_id);
                playlist.Playlist_name = playlistdto.Playlist_name;
                playlist.Update_date = DateTime.Now;
                playlist.Update_by = User.Identity.GetEmail();
                

                foreach (var item in db.PlaylistSongs)
                {
                    if (item.Playlist_id == playlistdto.Playlist_id)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                }

                foreach (var item in playlistdto.Songs)
                {
                    if (item.Checked)
                    {
                        db.PlaylistSongs.Add(new PlaylistSong()
                        { Playlist_id = playlistdto.Playlist_id, Song_id = item.id });
                    }
                }
                db.SaveChanges();
                Notification.set_noti("Update successfully: " + playlist.Playlist_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("PlaylistIndex");
            }
            catch
            {
                Notification.set_noti("404!", "warning");
            }

            return View(playlistdto);
        }

        // Delete Playlist
        public ActionResult PlaylistDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playlist playlist = db.Playlists.Find(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            return View(playlist);
        }

        [HttpPost, ActionName("PlaylistDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Playlist playlist = db.Playlists.Find(id);
            db.Playlists.Remove(playlist);
            db.SaveChanges();
            return RedirectToAction("PlaylistIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}