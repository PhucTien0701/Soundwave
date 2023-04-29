using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class SongCommentController : BaseController
    {
        SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Song index
        public ActionResult SongCommentIndex(int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    
                    var list_songcomment = from s in _appdb.Song_Comment                                    
                                           orderby s.Song_comment_id descending
                                           select s;
                    
                    return View(list_songcomment.ToPagedList(pageNum, pageSize));
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        // Delete Song
        public ActionResult SongCommentDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song_Comment songcomment = _appdb.Song_Comment.Find(id);
            if (songcomment == null)
            {
                return HttpNotFound();
            }
            return View(songcomment);
        }

        [HttpPost, ActionName("SongCommentDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Song_Comment songcomment = _appdb.Song_Comment.Find(id);
            _appdb.Song_Comment.Remove(songcomment);
            _appdb.SaveChanges();
            return RedirectToAction("SongCommentIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _appdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}