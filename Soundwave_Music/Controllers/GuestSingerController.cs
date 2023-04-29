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
    public class GuestSingerController : Controller
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        public ActionResult ListAllSinger(int? page)
        {
            List<DTOofSinger> listsinger = (from s in _appdb.Singers
                                            where s.Singer_status == "1" && s.Area_id == 1
                                            orderby s.Singer_id descending
                                            select new DTOofSinger
                                            {
                                                Singer_name = s.Singer_name,
                                                Image = s.Image
                                            }).ToList();
            ViewBag.ListName = "All Singer";
            return View("Singer", GetSinger(s => s.Singer_status == "1" && s.Area_id == 1, page));
        }

        private IPagedList GetSinger(Expression<Func<Singer, bool>> expression, int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            ViewBag.musicarea = _appdb.Areas.ToList();
            var listsinger = _appdb.Singers.Where(expression).OrderByDescending(s => s.Singer_id).ToPagedList(pageNum, pageSize);
            return listsinger;
        }

        //Singer Details
        public ActionResult SingerDetail(int? page, int? id)
        {

            int pageSize = 5;
            int pageNum = (page ?? 1);
            var singer = _appdb.Singers.Where(s => s.Singer_id == id && s.Singer_status == "1").OrderByDescending(s => s.Singer_id).ToList().FirstOrDefault();
            //list singer songs
            var singersongs = _appdb.Songs.Where(s => s.Singer_id == singer.Singer_id).OrderByDescending(c => c.Singer_id).ToList();
            ViewBag.SingerSongs = singersongs;
            _appdb.SaveChanges();
            return View(singer);
        }

        //search Singer
        public ActionResult SearchSingerResult(string searchsinger, int? page)
        {
            List<DTOofSinger> listsingerresult = (from s in _appdb.Singers
                                                  where s.Singer_name.Contains(searchsinger) && s.Singer_status == "1" && s.Area_id == 1
                                                  orderby s.Singer_id descending
                                                  select new DTOofSinger
                                                  {
                                                      Singer_name = s.Singer_name,
                                                      Image = s.Image
                                                  }).ToList();
            var list = _appdb.Singers.OrderByDescending(s => s.Singer_id);
            ViewBag.listsingerresult = listsingerresult;
            ViewBag.ListName = "Search Singer";

            return View("Singer", GetSinger(g => g.Singer_status == "1" && g.Area_id == 1 && g.Singer_name.Contains(searchsinger), page));

        }

        //suggest search
        [HttpPost]
        public JsonResult SuggestSingerSearch(string Prefix)
        {
            var searchsinger = (from s in _appdb.Singers
                                where s.Singer_status == "1" && s.Area_id == 1 && (s.Singer_name.Contains(Prefix))
                                orderby s.Singer_name ascending
                                select new DTOofSinger
                                {
                                    Singer_name = s.Singer_name,
                                    Singer_id = s.Singer_id,
                                    Image = s.Image,
                                });
            return Json(searchsinger, JsonRequestBehavior.AllowGet);
        
        }
    }
}