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
    public class PaymentController : BaseController
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();
        public ActionResult PaymentIndex(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    ViewBag.countTrash = _appdb.Payments.Count(a => a.Status == "2");
                    var list = from a in _appdb.Payments
                               where (a.Status == "1" || a.Status == "0")
                               orderby a.Payment_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))
                            list = (IOrderedQueryable<Payment>)list.Where(s => s.Payment_id.ToString().Contains(search) || s.Payment_method.Contains(search)
                            || s.Create_by.Contains(search));
                        else if (show.Equals("2"))
                            list = (IOrderedQueryable<Payment>)list.Where(s => s.Payment_id.ToString().Contains(search));
                        else if (show.Equals("3"))
                            list = (IOrderedQueryable<Payment>)list.Where(s => s.Payment_method.ToString().Contains(search));
                        return View("PaymentIndex", list.ToPagedList(pageNumber, 50));
                    }
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
        
        public ActionResult PaymentTrash(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    var list = from a in _appdb.Payments
                               where a.Status == "2"
                               orderby a.Update_date descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))
                            list = (IOrderedQueryable<Payment>)list.Where(s => s.Payment_id.ToString().Contains(search) || s.Payment_method.Contains(search)
                            || s.Create_by.Contains(search));
                        else if (show.Equals("2"))
                            list = (IOrderedQueryable<Payment>)list.Where(s => s.Payment_id.ToString().Contains(search));
                        else if (show.Equals("3"))
                            list = (IOrderedQueryable<Payment>)list.Where(s => s.Payment_method.ToString().Contains(search));
                        return View("PaymentTrash", list.ToPagedList(pageNumber, 50));
                    }
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
        
        public ActionResult PaymentDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_View() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var payment = _appdb.Payments.SingleOrDefault(a => a.Payment_id == id);
                    if (payment == null || id == null)
                    {
                        Notification.set_noti("Do not exist: " + payment.Payment_method + "", "warning");
                        return RedirectToAction("PaymentIndex");
                    }
                    return View(payment);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("PaymentIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }
        
        public ActionResult PaymentCreate()
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
                    return RedirectToAction("PaymentIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");

            }
        }
        
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentCreate(Payment payment)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    payment.Create_date = DateTime.Now;
                    payment.Create_by = User.Identity.GetEmail();
                    payment.Update_date = DateTime.Now;
                    payment.Status = payment.Status;
                    payment.Update_by = User.Identity.GetEmail();
                    _appdb.Payments.Add(payment);
                    _appdb.SaveChanges();
                    Notification.set_noti("Payment method has been created successfully: " + payment.Payment_method + "", "success");
                    return RedirectToAction("PaymentIndex");
                }
                catch
                {
                    Notification.set_noti("Fail to create", "danger");
                }
                return View(payment);
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }
        
        public ActionResult PaymentEdit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var payment = _appdb.Payments.SingleOrDefault(a => a.Payment_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (payment.Status == "1" || payment.Status == "0"))
                {
                    if (payment == null || id == null)
                    {
                        Notification.set_noti("Do not exist: " + payment.Payment_method + "", "warning");
                        return RedirectToAction("PaymentIndex");
                    }
                    return View(payment);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("PaymentIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }
        
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentEdit(Payment payment)
        {
            try
            {
                payment.Update_date = DateTime.Now;
                payment.Update_by = User.Identity.GetEmail();

                _appdb.Entry(payment).State = EntityState.Modified;
                _appdb.SaveChanges();

                Notification.set_noti("Payment method has been updated successfully: " + payment.Payment_method + "", "success");
                return RedirectToAction("PaymentIndex");
            }
            catch
            {
                Notification.set_noti("404!", "warning");
            }

            return View(payment);
        }
        
        public ActionResult DelTrash(int? id) 
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var payment = _appdb.Payments.SingleOrDefault(a => a.Payment_id == id);
                if (payment == null || id == null)
                {
                    Notification.set_noti("Do not exist: " + payment.Payment_method + "", "warning");
                    return RedirectToAction("PaymentIndex");
                }
                payment.Status = "2";
                payment.Update_date = DateTime.Now;
                payment.Update_by = User.Identity.GetEmail();
                _appdb.Entry(payment).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Changed: " + payment.Payment_method + " into trash", "success");
                return RedirectToAction("PaymentIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("PaymentIndex");
            }
        }
        
        public ActionResult Undo(int? id) 
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var payment = _appdb.Payments.SingleOrDefault(a => a.Payment_id == id);
                if (payment == null || id == null)
                {
                    Notification.set_noti("Do not exist: " + payment.Payment_method + "", "warning");
                    return RedirectToAction("PaymentIndex");
                }
                payment.Status = "1";
                payment.Update_date = DateTime.Now;
                payment.Update_by = User.Identity.GetEmail();
                _appdb.Entry(payment).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo from trash: " + payment.Payment_method + "", "success");
                return RedirectToAction("PaymentTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("PaymentTrash");
            }
        }
        
        public ActionResult PaymentDelete(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("PaymentDelete", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                if (User.Identity.Permiss_Delete() == true)
                {
                    var payment = _appdb.Payments.SingleOrDefault(a => a.Payment_id == id);
                    if (payment == null || id == null)
                    {
                        Notification.set_noti("Do not exist! (ID = " + id + ")", "warning");
                        return RedirectToAction("PaymentTrash");
                    }
                    return View(payment);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("PaymentTrash");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }
        
        [HttpPost]
        [ActionName("PaymentDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, string returnUrl)
        {
            var payment = _appdb.Payments.SingleOrDefault(a => a.Payment_id == id);
            _appdb.Payments.Remove(payment);
            _appdb.SaveChanges();
            Notification.set_noti("Delete: " + payment.Payment_method + " sucessfully", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("PaymentIndex");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) _appdb.Dispose();
            base.Dispose(disposing);
        }
    }
}