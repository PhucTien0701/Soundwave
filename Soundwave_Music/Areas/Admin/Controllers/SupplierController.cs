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
    public class SupplierController : BaseController
    {
        private SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Supplier index
        public ActionResult SupplierIndex(string search_supplier, string show_supplier, int? _sizepage, int? _pagenumber, string sort_supplier)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (_sizepage ?? 10);
                    var pageNum = (_pagenumber ?? 1);
                    ViewBag.Count_supplier_in_trash = _appdb.Suppliers.Count(s => s.Supplier_status == "2");
                    ViewBag.Current_sort = sort_supplier;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_supplier == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortBySupplierName = sort_supplier == "suppliername_asc" ? "suppliername_desc" : "suppliername_asc";
                    var list_supplier = from s in _appdb.Suppliers
                                        where (s.Supplier_status == "1" || s.Supplier_status == "0")
                                        orderby s.Supplier_id descending
                                        select s;
                    switch (sort_supplier)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            list_supplier = from s in _appdb.Suppliers
                                              where (s.Supplier_status == "1" || s.Supplier_status == "0")
                                              orderby s.Supplier_id descending
                                              select s;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            list_supplier = from s in _appdb.Suppliers
                                              where (s.Supplier_status == "1" || s.Supplier_status == "0")
                                              orderby s.Supplier_id
                                              select s;
                            break;

                        case "suppliername_desc":
                            ViewBag.sortname = "Sort by: Supplier name (Z-A)";
                            list_supplier = from s in _appdb.Suppliers
                                              where (s.Supplier_status == "1" || s.Supplier_status == "0")
                                              orderby s.Supplier_name descending
                                              select s;
                            break;

                        case "suppliername_asc":
                            ViewBag.sortname = "Sort by: Supplier name (A-Z)";
                            list_supplier = from s in _appdb.Suppliers
                                              where (s.Supplier_status == "1" || s.Supplier_status == "0")
                                              orderby s.Supplier_name
                                              select s;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_supplier)) return View(list_supplier.ToPagedList(pageNum, pageSize));
                    switch (show_supplier)
                    {
                        //case 1: search all
                        case "1":
                            list_supplier = (IOrderedQueryable<Supplier>)list_supplier.Where(a => a.Supplier_id.ToString().Contains(search_supplier) ||
                                                                                              a.Supplier_name.Contains(search_supplier));
                            break;
                        //case 2: search by id
                        case "2":
                            list_supplier = (IOrderedQueryable<Supplier>)list_supplier.Where(a => a.Supplier_id.ToString().Contains(search_supplier));
                            break;
                        //case 3: search by name
                        case "3":
                            list_supplier = (IOrderedQueryable<Supplier>)list_supplier.Where(a => a.Supplier_name.Contains(search_supplier));
                            break;
                    }
                    return View(list_supplier.ToPagedList(pageNum, pageSize));
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

        //Supplier Trash
        public ActionResult SupplierTrash(string search_supplier, string show_supplier, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var list_supplier = from s in _appdb.Suppliers
                                        where s.Supplier_status == "2"
                                        orderby s.Supplier_id descending
                                        select s;
                    if (!string.IsNullOrEmpty(search_supplier))
                    {
                        //search all
                        if (show_supplier.Equals("1"))
                            list_supplier = (IOrderedQueryable<Supplier>)list_supplier.Where(s => s.Supplier_id.ToString().Contains(search_supplier) || s.Supplier_name.Contains(search_supplier));
                        //search by id
                        else if (show_supplier.Equals("2"))
                            list_supplier = (IOrderedQueryable<Supplier>)list_supplier.Where(s => s.Supplier_id.ToString().Contains(search_supplier));
                        //search by full name
                        else if (show_supplier.Equals("3"))
                            list_supplier = (IOrderedQueryable<Supplier>)list_supplier.Where(a => a.Supplier_name.Contains(search_supplier));
                        return View("SupplierTrash", list_supplier.ToPagedList(pageNum, 50));
                    }
                    return View(list_supplier.ToPagedList(pageNum, pageSize));
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

        //Search supplier Suggestion
        [HttpPost]
        public JsonResult SuggestSupplierSearch(string Prefix)
        {
            var search_supplier = (from s in _appdb.Suppliers
                               where s.Supplier_status != "2" && s.Supplier_name.StartsWith(Prefix)
                               orderby s.Supplier_name ascending
                               select new { s.Supplier_name });
            return Json(search_supplier, JsonRequestBehavior.AllowGet);
        }

        //Create new supplier view
        public ActionResult SupplierCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    return View();
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this function", "danger");
                    return RedirectToAction("SupplierIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new supplier code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupplierCreate(Supplier supplier)
        {
            try
            {
                supplier.Supplier_name = supplier.Supplier_name;
                supplier.Image = supplier.Image;
                supplier.Supplier_status = supplier.Supplier_status;
                supplier.Create_date = DateTime.Now;
                supplier.Create_by = User.Identity.GetName();
                _appdb.Suppliers.Add(supplier);
                _appdb.SaveChanges();
                Notification.set_noti("Create successfully new supplier: " + supplier.Supplier_name + "", "success");
                return RedirectToAction("SupplierIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View(supplier);
        }

        //Supplier Edit
        public ActionResult SupplierEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("SupplierEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var supplier = _appdb.Suppliers.SingleOrDefault(s => s.Supplier_id == id);
                if ((User.Identity.Permiss_Edit() == true) && (supplier.Supplier_status == "1" || supplier.Supplier_status == "0"))
                {
                    if (supplier == null || id == null)
                    {
                        Notification.set_noti("This supplier is not exist: " + supplier.Supplier_name + "", "warning");
                        return RedirectToAction("SupplierIndex");
                    }
                    return View(supplier);
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("SupplierIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupplierEdit(Supplier supplier, string returnUrl)
        {
            try
            {
                supplier.Supplier_name = supplier.Supplier_name;
                supplier.Image = supplier.Image;
                supplier.Supplier_status = supplier.Supplier_status;
                supplier.Create_date = DateTime.Now;
                supplier.Create_by = User.Identity.GetName();                
                _appdb.Entry(supplier).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Update supplier successfully: " + supplier.Supplier_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("SupplierIndex");
            }
            catch
            {
                Notification.set_noti("Error!!!", "warning");
            }
            return View(supplier);
        }

        //Supplier Detail
        public ActionResult SupplierDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Access() == true)
                {
                    var list_supplier = (from s in _appdb.Suppliers
                                         where s.Supplier_id == id
                                         orderby s.Create_date descending
                                         select s).FirstOrDefault();
                    if (list_supplier != null && id != null) return View(list_supplier);
                    Notification.set_noti("This supplier is not exist: " + list_supplier.Supplier_name + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("SupplierIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Move supplier to trash
        public ActionResult MoveSupplierToTrash(int? id, string returnUrl)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var supplier = _appdb.Suppliers.SingleOrDefault(s => s.Supplier_id == id);
                if (supplier == null || id == null)
                {
                    Notification.set_noti("This supplier is not exist: " + supplier.Supplier_name + "", "warning");
                    return RedirectToAction("SupplierIndex");
                }
                supplier.Supplier_status = "2";
                _appdb.Entry(supplier).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable supplier " + supplier.Supplier_name + "sucessfully.", "success");
                return RedirectToAction("SupplierIndex");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action", "danger");
                return RedirectToAction("SupplierIndex");
            }
        }

        //Undo Supplier From Trash
        public ActionResult UndoSupplierFromTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
            {
                var supplier = _appdb.Suppliers.SingleOrDefault(s => s.Supplier_id == id);
                if (supplier == null || id == null)
                {
                    Notification.set_noti("This supplier is not exist: " + supplier.Supplier_name + "", "warning");
                    return RedirectToAction("SupplierIndex");
                }
                supplier.Supplier_status = "1";
                _appdb.Entry(supplier).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Undo supplier " + supplier.Supplier_name + "sucessfully.", "success");
                return RedirectToAction("SupplierTrash");
            }
            else
            {
                Notification.set_noti("You do not have permission to use this action.", "danger");
                return RedirectToAction("SupplierIndex");
            }
        }

        // Delete supplier
        public ActionResult SupplierDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = _appdb.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("SupplierDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = _appdb.Suppliers.Find(id);
            _appdb.Suppliers.Remove(supplier);
            _appdb.SaveChanges();
            return RedirectToAction("SupplierTrash");
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