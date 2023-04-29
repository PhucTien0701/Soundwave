using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.DataTransferObjectives;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class VIPPackController : BaseController
    {
        SoundwaveDbContext _db = new SoundwaveDbContext();

        public ActionResult VIPIndex(int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    ViewBag.countTrash = _db.Products.Count(a => a.status == "0");
                    ViewBag.countProductsAdmin = _db.Products.Count(a => a.status != "2");
                    var list = from a in _db.Products
                               where a.status == "1"
                               orderby a.product_id descending
                               select new DTOofVIP
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,                                   
                                   status = a.status,                                   
                                   product_id = a.product_id,
                                   slug = a.slug,
                               };
                    
                    return View(list.ToPagedList(pageNumber, pageSize));
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

        public ActionResult VIPTrash(int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    
                    var list = from a in _db.Products                       
                               where a.status == "0"
                               orderby a.update_date descending
                               select new DTOofVIP
                               {
                                   product_name = a.product_name,
                                   quantity = a.quantity,
                                   price = a.price,
                                   Image = a.image,
                                   update_date = a.update_date,
                                   status = a.status,
                                   product_id = a.product_id,
                               };
                    
                    return View(list.ToPagedList(pageNumber, pageSize));
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

        public ActionResult VIPDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_View() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var product = (from a in _db.Products                                   
                                   where a.product_id == id
                                   orderby a.create_date descending
                                   select new DTOofVIP
                                   {
                                       product_name = a.product_name,
                                       quantity = a.quantity,
                                       price = a.price,
                                       Image = a.image,
                                       product_id = a.product_id,
                                       description = a.description,
                                       create_date = a.create_date,
                                       create_by = a.create_by,
                                       status = a.status,
                                       buyturn = a.buyturn,
                                       update_date = a.update_date,
                                       update_by = a.update_by,
                                   }).FirstOrDefault();
                    
                    if (product == null || id == null)
                    {
                        Notification.set_noti("Do not exist: " + product.product_name + "", "warning");
                        return RedirectToAction("VIPIndex");
                    }
                    return View(product);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("VIPIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        public ActionResult VIPCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    return View();
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("VIPIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> VIPCreate(DTOofVIP Prodtos, Product product)
        {
            var slug = SlugGenerator.SlugGenerator.GenerateSlug(Prodtos.slug);
            try
            {
                if (Prodtos.slug == null)
                {
                    product.slug = SlugGenerator.SlugGenerator.GenerateSlug(Prodtos.product_name);
                }
                else
                {
                    var checkslug = _db.Products.Any(m => m.slug == slug);
                    if (checkslug)
                    {
                        product.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + new Random().Next(1, 1000);
                    }
                    else
                    {
                        product.slug = SlugGenerator.SlugGenerator.GenerateSlug(Prodtos.slug);
                    }
                }
                product.image = Prodtos.Image;
                product.status = Prodtos.status;
                product.description = Prodtos.description;
                product.buyturn = 0;
                product.create_date = DateTime.Now;
                product.update_date = DateTime.Now;
                product.create_by = User.Identity.GetEmail();
                product.update_by = User.Identity.GetEmail();
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                Notification.set_noti("Add VIP Package successfully", "success");
                return RedirectToAction("VIPIndex");
            }
            catch
            {
                Notification.set_noti("Fail to add VIP Package", "danger");
            }
            return View(Prodtos);
        }

        public ActionResult VIPEdit(int id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("VIPEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var product = _db.Products.FirstOrDefault(x => x.product_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (product.status == "1" || product.status == "0"))
                {
                    DTOofVIP productDTOs = new DTOofVIP
                    {
                        price = product.price,
                        product_id = product.product_id,
                        slug = product.slug,
                        product_name = product.product_name,
                        Image = product.image,
                        quantity = product.quantity,
                        status = product.status,
                        description = product.description
                    };
                    if (product == null)
                    {
                        Notification.set_noti("Do not exist: " + product.product_name + "", "warning");
                        return RedirectToAction("VIPIndex");
                    }
                    return View(productDTOs);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("VIPIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult VIPEdit(DTOofVIP productDtOs, string returnUrl, int id)
        {
            var product = _db.Products.SingleOrDefault(x => x.product_id == id);
            try
            {
                product.product_name = productDtOs.product_name;
                product.quantity = productDtOs.quantity;
                product.description = productDtOs.description;
                product.status = productDtOs.status;
                product.price = productDtOs.price;
                product.image = productDtOs.Image;
                product.update_date = DateTime.Now;
                product.update_by = User.Identity.GetEmail();
                _db.SaveChanges();
                Notification.set_noti("Update successfully" + id + "", "success");

                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("VIPIndex");
            }
            catch
            {
                Notification.set_noti("Fail to update", "danger");
            }
            return View(productDtOs);
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id, int state = 0)
        {
            bool result;
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                Product product = _db.Products.FirstOrDefault(m => m.product_id == id);
                int title = product.product_id;
                product.status = state.ToString();
                product.update_date = DateTime.Now;
                product.update_by = User.Identity.GetEmail();
                _db.SaveChanges();
                result = true;
                Notification.set_noti("Change status 'id " + id + "' successfully", "success");
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Delete VipPackage
        public ActionResult VIPDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product vip = _db.Products.Find(id);
            if (vip == null)
            {
                return HttpNotFound();
            }
            return View(vip);
        }

        [HttpPost, ActionName("VIPDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product vip = _db.Products.Find(id);
            _db.Products.Remove(vip);
            _db.SaveChanges();
            return RedirectToAction("VIPTrash");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}