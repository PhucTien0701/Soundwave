using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Hosting;
using System.Web.Mvc;
using Soundwave_Music.Common;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.DataTransferObjectives;
using Soundwave_Music.Models;
using PagedList;
using Syncfusion.XlsIO;
using Soundwave_Music.Common.NotificationLib;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        SoundwaveDbContext db = new SoundwaveDbContext();
        //List view đơn hàng
        public ActionResult OrderIndex(int? page, int? size, string search, string show, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
                    ViewBag.PhoneNumberSortParm = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc";
                    ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                    ViewBag.WaitingSortParm = sortOrder == "waiting" ? "waiting" : "waiting";
                    ViewBag.ProcessingSortParm = sortOrder == "processing" ? "processing" : "processing";
                    ViewBag.CompletegSortParm = sortOrder == "complete" ? "complete" : "complete";
                    ViewBag.countTrash = db.Orders.Count(a => a.Status == "0");
                    var list = from a in db.Order_Detail
                               join b in db.Orders on a.Order_id equals b.Order_id
                               group a by new { a.Order_id, b } into g
                               where g.Key.b.Status != "0"
                               orderby g.Key.b.Order_id descending
                               select new DTOofOrder
                               {
                                   order_id = g.Key.Order_id,
                                   total_price = g.Key.b.Total,
                                   status = g.Key.b.Status,
                                   order_date = g.Key.b.Order_date,
                                   update_at = g.Key.b.Update_date,
                                   Name = g.Key.b.User.Full_name,
                                   Email = g.Key.b.User.Email,
                                   Phone = g.Key.b.User.Phone_number,
                                   payment_id = g.Key.b.Payment_id,
                                   payment_transaction = g.Key.b.Payment_transaction
                               };
                    switch (sortOrder)
                    {
                        case "name_asc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.User.Full_name ascending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "name_desc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.User.Full_name descending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "price_asc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.Total ascending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "price_desc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.Total descending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "waiting":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status == "1"
                                   orderby g.Key.b.Order_id descending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "processing":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status == "2"
                                   orderby g.Key.b.Order_id descending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "complete":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status == "3"
                                   orderby g.Key.b.Order_id descending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "phone_asc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.User.Phone_number ascending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "phone_desc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.User.Phone_number descending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "date_asc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.Order_date ascending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        case "date_desc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.Order_date descending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                        default:
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.Order_id equals b.Order_id
                                   group a by new { a.Order_id, b } into g
                                   where g.Key.b.Status != "0"
                                   orderby g.Key.b.Order_id descending
                                   select new DTOofOrder
                                   {
                                       order_id = g.Key.Order_id,
                                       total_price = g.Key.b.Total,
                                       status = g.Key.b.Status,
                                       order_date = g.Key.b.Order_date,
                                       update_at = g.Key.b.Update_date,
                                       Name = g.Key.b.User.Full_name,
                                       Email = g.Key.b.User.Email,
                                       Phone = g.Key.b.User.Phone_number,
                                       payment_id = g.Key.b.Payment_id,
                                       payment_transaction = g.Key.b.Payment_transaction
                                   };
                            break;
                    }
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))
                            list = list.Where(s => s.order_id.ToString().Contains(search) || s.Name.ToString().Trim().Contains(search) || s.status.Trim().Contains(search));
                        else if (show.Equals("2"))
                            list = list.Where(s => s.order_id.ToString().Contains(search));
                        else if (show.Equals("3"))
                            list = list.Where(s => s.Name.ToString().Contains(search));
                        else if (show.Equals("4"))
                            list = list.Where(s => s.Phone.ToString().Contains(search));
                        else if (show.Equals("5"))
                            list = list.Where(s => s.status.Contains(search));
                        return View("OrderIndex", list.ToPagedList(pageNumber, 50));
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
        
        public ActionResult OrderTrash(int? page, int? size, string search, string show)
        {
            if (User.Identity.IsAuthenticated)
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var list = from a in db.Order_Detail
                               join b in db.Orders on a.Order_id equals b.Order_id
                               group a by new { a.Order_id, b } into g
                               where g.Key.b.Status == "0"
                               orderby g.Key.b.Update_date descending
                               select new DTOofOrder
                               {
                                   order_id = g.Key.Order_id,
                                   total_price = g.Key.b.Total,
                                   status = g.Key.b.Status,
                                   update_at = g.Key.b.Update_date,
                                   Name = g.Key.b.User.Full_name,
                                   Phone = g.Key.b.User.Phone_number
                               };
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))
                            list = list.Where(s => s.order_id.ToString().Contains(search) || s.Name.ToString().Trim().Contains(search) || s.Phone.ToString().Contains(search) || s.status.Trim().Contains(search));
                        else if (show.Equals("2"))
                            list = list.Where(s => s.order_id.ToString().Contains(search));
                        else if (show.Equals("3"))
                            list = list.Where(s => s.Name.ToString().Contains(search));
                        else if (show.Equals("4"))
                            list = list.Where(s => s.Phone.ToString().Contains(search));
                        else if (show.Equals("5"))
                            list = list.Where(s => s.status.Contains(search));
                        return View("OrderTrash", list.ToPagedList(pageNumber, 50));
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
        
        [HttpPost]
        public JsonResult GetOrderSearch(string Prefix)
        {
            var search = (from c in db.Orders
                          where c.Status != "0" && c.Order_id.ToString().StartsWith(Prefix)
                          orderby c.Order_id descending
                          select new { c.Order_id });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult OrderDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_View() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true)
                {
                    var order_detail = db.Order_Detail.FirstOrDefault(m => m.Order_id == id);
                    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    var order = (from a in db.Order_Detail
                                 join b in db.Orders on a.Order_id equals id
                                 join p in db.Products on a.product_id equals p.product_id
                                 group a by new { a.Order_id, b, a.product_id, p } into g
                                 where g.Key.b.Order_id == id
                                 select new DTOofOrder
                                 {
                                     user_id = g.Key.b.User_id,
                                     order_id = g.Key.Order_id,
                                     status = g.Key.b.Status,
                                     total_price = g.Key.b.Total,
                                     create_at = g.Key.b.Create_date,
                                     order_date = g.Key.b.Order_date,
                                     create_by = g.Key.b.Order_create_by,
                                     payment_id = g.Key.b.Payment_id,
                                     payment_name = g.Key.b.Payment.Payment_method,
                                     product_name = g.Key.p.product_name,
                                     Email = g.Key.b.User.Email,
                                     payment_transaction = g.Key.b.Payment_transaction,
                                     update_at = g.Key.b.Update_date,
                                     update_by = g.Key.b.Update_by,
                                     Name = g.Key.b.User.Full_name,
                                     Phone = g.Key.b.User.Phone_number,
                                 }).FirstOrDefault();
                    if (order == null || id == null)
                    {
                        Notification.set_noti("Do not exist order: " + id + ")", "warning");
                        return RedirectToAction("OrderIndex");
                    }
                    ViewBag.orderDetails = db.Order_Detail.Where(m => m.Order_id == id).ToList();
                    ViewBag.orderProduct = db.Products.ToList();
                    return View(order);
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
       
        [HttpPost]
        public JsonResult CancleOrder(string ButtonConfirmlink, int? id, string ProductOrder, string OrderTotal)
        {
            Boolean result;
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            var order = db.Orders.Where(m => m.Order_id == id).FirstOrDefault();
            var orderdetail = db.Order_Detail.Where(m => m.Order_id == order.Order_id).ToList();
            if (order.Status == "3")
            {
                result = false;
            }
            else
            {
                order.Status = "0";
                order.Update_date = DateTime.Now;
                order.Update_by = User.Identity.GetEmail();
                string emailID = order.User.Email;
                string OrderID = order.Order_id.ToString();
                string OrderPhone = order.User.Phone_number;
                string OrderName = order.User.Full_name;
                double pricesum = 0;
                foreach (var item in orderdetail)
                {
                    pricesum += (item.Price);
                    ProductOrder += "<tr style='border-bottom: 1px solid rgba(0,0,0,.05);' class='product_item'>" +
                            "<td valign='middle' width ='80%' style='text-align:left; padding:0 2.5em;'> " +
                                "<div class='product-entry'>" +
                                    "<img src='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/" + item.Product.image + "' style = 'width: 100px; max-width: 600px; height: auto; padding-bottom:5px; display: block;'>" +
                                    "<div class='text'>" +
                                        "<a href='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + item.Product.slug + "'> <div class='product_name'>" + item.Product.product_name + "</div></a>" +
                                    "</div>" +
                                "</div>" +
                            "</td>" +
                            "<td valign='middle' width='20%' style='text-align:right; padding-right: 2.5em;'>" +
                                "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.Price.ToString("#,0", cul.NumberFormat) + "₫" + "</span>" +
                            "</td>" +
                        "</tr>";
                }
                if (order.Payment_id != 1 && order.Payment_transaction == "2")
                {
                    OrderTotal = "0₫";
                }
                else
                {
                    OrderTotal = order.Total.ToString("#,0", cul.NumberFormat) + "₫";
                }
                string OrderPayment = db.Payments.Where(m => m.Payment_id == order.Payment_id).FirstOrDefault().Payment_method;
                string OrderStatus = "<span style='color:#dc3545;'>is cancelled</span>";
                ButtonConfirmlink = Request.Url.Scheme + "://" + Request.Url.Authority + "/User/order_detail/" + order.Order_id;
                SendEmailOrders(ButtonConfirmlink, ProductOrder, OrderPayment, OrderStatus, emailID, OrderID, OrderTotal,
                OrderPhone, OrderName, "CancleOrders");
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                    Notification.set_noti("Cancelled order " + id + " successfully", "success");
                }
                else
                {
                    result = false;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeWaitting(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
                {
                    var order = db.Orders.SingleOrDefault(pro => pro.Order_id == id && pro.Status != "3");
                    if (order != null)
                    {
                        order.Status = "1";
                        order.Update_date = DateTime.Now;
                        order.Update_by = User.Identity.GetEmail();
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        Notification.set_noti("Order has been changed into" + id + " in processing!", "success");
                    }
                    else
                    {
                        Notification.set_noti("Order: " + "#" + id + " is completed, cannot change into another status", "warning");
                    }
                    return RedirectToAction("OrderIndex");
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("OrderIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }
        
        public ActionResult ChangeProcessing(string ButtonConfirmlink, int? id, string ProductOrder, string OrderTotal)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                    var order = db.Orders.SingleOrDefault(pro => pro.Order_id == id);
                    var orderdetail = db.Order_Detail.Where(m => m.Order_id == order.Order_id).ToList();
                    if (order != null)
                    {
                        order.Status = "2";
                        order.Update_date = DateTime.Now;
                        order.Update_by = User.Identity.GetEmail();
                        db.Entry(order).State = EntityState.Modified;
                    }
                    string emailID = order.User.Email;
                    string OrderID = order.Order_id.ToString();
                    string OrderPhone = order.User.Phone_number;
                    string OrderName = order.User.Full_name;
                    double pricesum = 0;
                    foreach (var item in orderdetail)
                    {
                        pricesum += (item.Price);
                        ProductOrder += "<tr style='border-bottom: 1px solid rgba(0,0,0,.05);' class='product_item'>" +
                                "<td valign='middle' width ='80%' style='text-align:left; padding:0 2.5em;'> " +
                                    "<div class='product-entry'>" +
                                        "<img src='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/" + item.Product.image + "' style = 'width: 100px; max-width: 600px; height: auto; padding-bottom:5px; display: block;'>" +
                                        "<div class='text'>" +
                                            "<a href='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + item.Product.slug + "'> <div class='product_name'>" + item.Product.product_name + "</div></a>" +
                                        "</div>" +
                                    "</div>" +
                                "</td>" +
                                "<td valign='middle' width='20%' style='text-align:right; padding-right: 2.5em;'>" +
                                    "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.Price.ToString("#,0", cul.NumberFormat) + "₫" + "</span>" +
                                "</td>" +
                            "</tr>";
                    }
                    
                    if (order.Payment_id != 1 && order.Payment_transaction == "2")
                    {
                        OrderTotal = "0₫";
                    }
                    else
                    {
                        OrderTotal = order.Total.ToString("#,0", cul.NumberFormat) + "₫";
                    }
                    string OrderPayment = db.Payments.Where(m => m.Payment_id == order.Payment_id).FirstOrDefault().Payment_method;
                    string OrderStatus = "<span style='color:#17a2b8;'>Processing</span>";
                    ButtonConfirmlink = Request.Url.Scheme + "://" + Request.Url.Authority + "/User/order_detail/" + order.Order_id;
                    SendEmailOrders(ButtonConfirmlink,ProductOrder, OrderPayment, OrderStatus, emailID, OrderID, OrderTotal,
                    OrderPhone, OrderName, "ChangeProcessing");
                    db.SaveChanges();
                    Notification.set_noti("Order has been changed into: " + "#" + id + " processing!", "success");
                    return RedirectToAction("OrderIndex");
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
        //Chuyển trạng thái đơn hàng sang hoàn thành
        public ActionResult ChangeComplete(string ButtonConfirmlink, int? id, string ProductOrder, string OrderTotal)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Edit() == true)
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                    var order = db.Orders.SingleOrDefault(pro => pro.Order_id == id);
                    var orderdetail = db.Order_Detail.Where(m => m.Order_id == order.Order_id).ToList();
                    if (order != null)
                    {
                        order.Status = "3";
                        order.Update_date = DateTime.Now;
                        order.Update_by = User.Identity.GetEmail();
                        db.Entry(order).State = EntityState.Modified;
                    }

                    string emailID = order.User.Email;
                    string OrderID = order.Order_id.ToString();
                    string OrderPhone = order.User.Phone_number;
                    string OrderName = order.User.Full_name;
                    double pricesum = 0;
                    foreach (var item in orderdetail)
                    {
                        pricesum += (item.Price);
                        ProductOrder += "<tr style='border-bottom: 1px solid rgba(0,0,0,.05);' class='product_item'>" +
                                "<td valign='middle' width ='80%' style='text-align:left; padding:0 2.5em;'> " +
                                    "<div class='product-entry'>" +
                                        "<img src='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/" + item.Product.image + "' style = 'width: 100px; max-width: 600px; height: auto; padding-bottom:5px; display: block;'>" +
                                        "<div class='text'>" +
                                            "<a href='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + item.Product.slug + "'> <div class='product_name'>" + item.Product.product_name + "</div></a>" +
                                        "</div>" +
                                    "</div>" +
                                "</td>" +
                                "<td valign='middle' width='20%' style='text-align:right; padding-right: 2.5em;'>" +
                                    "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.Price.ToString("#,0", cul.NumberFormat) + "₫" + "</span>" +
                                "</td>" +
                            "</tr>";
                    }
                    if (order.Payment_id != 1 && order.Payment_transaction == "2")
                    {
                        OrderTotal = "0₫";
                    }
                    else
                    {
                        OrderTotal = order.Total.ToString("#,0", cul.NumberFormat) + "₫";
                    }
                    string OrderPayment = db.Payments.Where(m => m.Payment_id == order.Payment_id).FirstOrDefault().Payment_method;
                    string OrderStatus = "<span style='color:#28a745;'>Upgrade VIP User Successfully</span>";
                    ButtonConfirmlink = Request.Url.Scheme + "://" + Request.Url.Authority + "/User/order_detail/" + order.Order_id;
                    SendEmailOrders(ButtonConfirmlink, ProductOrder, OrderPayment, OrderStatus, emailID, OrderID, OrderTotal,
                    OrderPhone, OrderName, "ChangeComplete");
                    db.SaveChanges();
                    Notification.set_noti("Order has been changed into complete: " + id + " succesffully!", "success");
                    return RedirectToAction("OrderIndex");
                }
                else
                {
                    Notification.set_noti("You do not have permission to use this action", "danger");
                    return RedirectToAction("OrderIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }
        //Gửi Email trạng thái sản phẩm
        public void SendEmailOrders(string ButtonConfirmlink, string ProductOrder, string OrderPayment, string OrderStatus, string emailID, string OrderID, string OrderTotal,
        string OrderPhone, string OrderName, string emailFor)
        {
            var fromEmail = new MailAddress(ApplicationEmail.UserEmail, ApplicationEmail.Name); 
            var toEmail = new MailAddress(emailID);
            //nhập password của bạn
            var fromEmailPassword = ApplicationEmail.Password;
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/MailTemplates/") + "Mail_Order" + ".cshtml"); 
            if (emailFor == "ChangeProcessing")
            {
                subject = "Order #" + OrderID + " in processing";
                body = body.Replace("{{OrderId}}", OrderID);
                body = body.Replace("{{BodyContent}}", "VIP Package of order <span style='color: rgb(1, 181, 187); font-weight: 500;'>#" + OrderID + "</span> has been handling in system.");
                body = body.Replace("{{OrderStatus}}", OrderStatus);
                body = body.Replace("{{ButtonConfirm}}", "Order Management");
                body = body.Replace("{{ButtonConfirmLink}}", ButtonConfirmlink);
                body = body.Replace("{{UserEmail}}", emailID);
                body = body.Replace("{{UserName}}", OrderName);
                body = body.Replace("{{UserPhoneNumber}}", OrderPhone);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{ProductOrder}}", ProductOrder);
                body = body.Replace("{{Payment}}", OrderPayment);
            }
            else if (emailFor == "ChangeComplete")
            {
                subject = "Order #" + OrderID + " has been upgraded successfully";
                body = body.Replace("{{OrderId}}", OrderID);
                body = body.Replace("{{BodyContent}}", "Your user account <span style='color: rgb(1, 181, 187); font-weight: 500;'>#" + OrderID + "</span> has been upgraded successfully. Thank you to using our service.");
                body = body.Replace("{{ButtonConfirm}}", "Order Management");
                body = body.Replace("{{ButtonConfirmLink}}", ButtonConfirmlink);
                body = body.Replace("{{OrderStatus}}", OrderStatus);
                body = body.Replace("{{UserEmail}}", emailID);
                body = body.Replace("{{UserName}}", OrderName);
                body = body.Replace("{{UserPhoneNumber}}", OrderPhone);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{ProductOrder}}", ProductOrder);
                body = body.Replace("{{Payment}}", OrderPayment);
            }
            else if (emailFor == "CancleOrders")
            {
                subject = "Order #" + OrderID + " has been cancelled";
                body = body.Replace("{{OrderId}}", OrderID);
                body = body.Replace("{{BodyContent}}", "Your order <span style='color: rgb(1, 181, 187); font-weight: 500;'>#" + OrderID + "</span> has been cancelled.");
                body = body.Replace("{{ButtonConfirm}}", "Order Management");
                body = body.Replace("{{ButtonConfirmLink}}", ButtonConfirmlink);
                body = body.Replace("{{OrderStatus}}", OrderStatus);
                body = body.Replace("{{UserEmail}}", emailID);
                body = body.Replace("{{UserName}}", OrderName);
                body = body.Replace("{{UserPhoneNumber}}", OrderPhone);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{ProductOrder}}", ProductOrder);
                body = body.Replace("{{Payment}}", OrderPayment);
            }
            var smtp = new SmtpClient
            {
                Host = ApplicationEmail.Host,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        public ActionResult Turnover(string sortOrder, int? size, int? page)
        {
            var pageSize = size ?? 10;
            var pageNumber = page ?? 1;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.MonthSortParm = sortOrder == "total_month" ? "total_month" : "total_month";
            ViewBag.YearSortParm = sortOrder == "total_year" ? "total_year" : "total_year";
            var order = db.Orders.ToList();
            switch (sortOrder)
            {
                case "total_month":
                    order.Where(m => m.Status == "2").Sum(m => m.Total);
                    break;
                default:
                    order.Where(m => m.Status == "3").Sum(m => m.Total);
                    break;
            }
            return View(order.ToPagedList(pageNumber, pageSize));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}