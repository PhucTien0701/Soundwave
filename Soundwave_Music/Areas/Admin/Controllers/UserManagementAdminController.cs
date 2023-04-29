using PagedList;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.DataTransferObjectives;
using Soundwave_Music.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Soundwave_Music.DataTransferObjectives.DTOofRole;

namespace Soundwave_Music.Areas.Admin.Controllers
{
    public class UserManagementAdminController : BaseController
    {
        private readonly SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //List user in system
        public ActionResult AdminUserIndex(string search_user, string show_user, int? _sizepage, int? _pagenumber, string sort_user)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 10;
                    var pageNum = _pagenumber ?? 1;
                    ViewBag.List_user_role = _appdb.Roles.ToList();
                    ViewBag.Count_user_in_trash = _appdb.Users.Count(u => u.User_Status == "2");
                    ViewBag.Current_sort = sort_user;
                    ViewBag.Reset_sort = "";
                    ViewBag.SortByDate = sort_user == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.SortByFullName = sort_user == "fullname_asc" ? "fullname_desc" : "fullname_asc";
                    ViewBag.SortByEmail = sort_user == "email_asc" ? "email_desc" : "email_asc";
                    ViewBag.SortByPhoneNumber = sort_user == "phonenum_asc" ? "phonenum_desc" : "phonenum_asc";
                    var _useraccount = from u in _appdb.Users
                                       where (u.User_Status == "1" || u.User_Status == "0")
                                       orderby u.User_Status descending
                                       select u;
                    switch (sort_user)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Sort by: Newest";
                            _useraccount = from u in _appdb.Users
                                           where (u.User_Status == "1" || u.User_Status == "0")
                                           orderby u.User_id descending
                                           select u;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Sort by: Oldest";
                            _useraccount = from u in _appdb.Users
                                           where (u.User_Status == "1" || u.User_Status == "0")
                                           orderby u.User_id
                                           select u;
                            break;

                        case "fullname_desc":
                            ViewBag.sortname = "Sort by: User full name (Z-A)";
                            _useraccount = from u in _appdb.Users
                                           where (u.User_Status == "1" || u.User_Status == "0")
                                           orderby u.Full_name descending
                                           select u;
                            break;

                        case "fullname_asc":
                            ViewBag.sortname = "Sort by: User full name (A-Z)";
                            _useraccount = from u in _appdb.Users
                                           where (u.User_Status == "1" || u.User_Status == "0")
                                           orderby u.Full_name
                                           select u;
                            break;

                        case "email_desc":
                            ViewBag.sortname = "Sort by: User email (Z-A)";
                            _useraccount = from u in _appdb.Users
                                           where (u.User_Status == "1" || u.User_Status == "0")
                                           orderby u.Email descending
                                           select u;
                            break;

                        case "email_asc":
                            ViewBag.sortname = "Sort by: User email (A-Z)";
                            _useraccount = from u in _appdb.Users
                                           where (u.User_Status == "1" || u.User_Status == "0")
                                           orderby u.Email
                                           select u;
                            break;

                        case "phonenum_desc":
                            ViewBag.sortname = "Sort by: Phone number (9-0)";
                            _useraccount = from u in _appdb.Users
                                           where (u.User_Status == "1" || u.User_Status == "0")
                                           orderby u.Phone_number descending
                                           select u;
                            break;

                        case "phonenum_asc":
                            ViewBag.sortname = "Sort by: Phone number (0-9)";
                            _useraccount = from u in _appdb.Users
                                           where (u.User_Status == "1" || u.User_Status == "0")
                                           orderby u.Phone_number
                                           select u;
                            break;
                    }

                    if (string.IsNullOrEmpty(search_user)) return View(_useraccount.ToPagedList(pageNum, pageSize));
                    switch (show_user)
                    {
                        //case 1: search all
                        case "1":
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.User_id.ToString().Contains(search_user) ||
                                                                                       u.Email.Contains(search_user) ||
                                                                                       u.Full_name.Contains(search_user) ||
                                                                                       u.Phone_number.ToString().Contains(search_user) ||
                                                                                       u.User_Status.Contains(search_user));
                            break;
                        //case 2: search by id
                        case "2":
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.User_id.ToString().Contains(search_user));
                            break;
                        //case 3: search by name
                        case "3":
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.Full_name.Contains(search_user));
                            break;
                        //case 4: search by email
                        case "4":
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.Email.Contains(search_user));
                            break;
                        //case 5: search by phone number
                        case "5":
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.Phone_number.ToString().Contains(search_user));
                            break;
                        default:
                            ViewBag.search = search_user;
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.Role_id.ToString().Contains(search_user));
                            break;
                    }
                    return View("AdminUserIndex", _useraccount.ToPagedList(pageNum, pageSize));
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

        public ActionResult UserTrash(string search_user, string show_user, int? _sizepage, int? _pagenumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Edit() == true || User.Identity.Permiss_Delete() == true || User.Identity.Permiss_Update() == true)
                {
                    var pageSize = _sizepage ?? 5;
                    var pageNum = _pagenumber ?? 1;
                    var _useraccount = from u in _appdb.Users
                                       where u.User_Status == "2"
                                       orderby u.Update_date descending
                                       select u;
                    if (!string.IsNullOrEmpty(search_user))
                    {
                        //search all
                        if (show_user.Equals("1"))
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.User_id.ToString().Contains(search_user) ||
                                                                                            u.Full_name.Contains(search_user) ||
                                                                                            u.Email.Contains(search_user) ||
                                                                                            u.Phone_number.ToString().Contains(search_user) ||
                                                                                            u.User_Status.Contains(search_user));
                        //search by id
                        else if (show_user.Equals("2"))
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.User_id.ToString().Contains(search_user));
                        //search by full name
                        else if (show_user.Equals("3"))
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.Full_name.Contains(search_user));
                        //search by email
                        else if (show_user.Equals("4"))
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.Email.Contains(search_user));
                        //search by phone number
                        else if (show_user.Equals("5"))
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.Phone_number.ToString().Contains(search_user));
                        //search by user status
                        else if (show_user.Equals("6"))
                            _useraccount = (IOrderedQueryable<User>)_useraccount.Where(u => u.User_Status.Contains(search_user));
                        return View("UserTrash", _useraccount.ToPagedList(pageNum, 50));
                    }
                    return View(_useraccount.ToPagedList(pageNum, pageSize));
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

        //Search Suggestion
        [HttpPost]
        public JsonResult SuggestUserSearch(string Prefix)
        {
            var search_user = (from u in _appdb.Users
                               where u.User_Status != "2" && u.Email.StartsWith(Prefix)
                               orderby u.Email ascending
                               select new { u.Email });
            return Json(search_user, JsonRequestBehavior.AllowGet);
        }

        //User detail information
        public ActionResult UserDetail(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_View() == true)
                {
                    var _useraccount = (from u in _appdb.Users
                                        where u.User_id == id
                                        orderby u.Create_date descending
                                        select u).FirstOrDefault();
                    if (_useraccount != null && id != null) return View(_useraccount);
                    Notification.set_noti("This use is not exist: " + _useraccount.Email + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    Notification.set_noti("You do not have permission", "danger");
                    return RedirectToAction("AdminUserIndex");
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Diabled user account
        public ActionResult MoveUserToTrash(int? id, string returnUrl)
        {
            if (User.Identity.GetRole() != 1)
            {
                Notification.set_noti("You do not have permission", "danger");
                return RedirectToAction("AdminUserIndex");
            }
            else
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("MoveUserToTrash", new { returnUrl = Request.UrlReferrer.ToString() });
                }

                var _useraccount = _appdb.Users.SingleOrDefault(u => u.User_id == id);
                if (_useraccount == null || id == null)
                {
                    Notification.set_noti("This user is not exist: " + _useraccount.Email + "", "warning");
                    return RedirectToAction("AdminUserIndex");
                }

                _useraccount.User_Status = "2";
                _useraccount.Update_date = DateTime.Now;
                _appdb.Configuration.ValidateOnSaveEnabled = false;
                _appdb.Entry(_useraccount).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Disable user successfully: " + _useraccount.Email + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Dashboard");
            }
        }

        //Undo user from trash
        public ActionResult UndoUserFromTrash(int? id, string returnUrl)
        {
            if (User.Identity.GetRole() != 1)
            {
                Notification.set_noti("You do not have permission", "danger");
                return RedirectToAction("UserTrash");
            }
            else
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("UndoUserFromTrash", new { returnUrl = Request.UrlReferrer.ToString() });
                }

                var _useraccount = _appdb.Users.SingleOrDefault(u => u.User_id == id);
                if (_useraccount == null || id == null)
                {
                    Notification.set_noti("This user is not exist: " + _useraccount.Email + "", "warning");
                    return RedirectToAction("UserTrash");
                }

                _useraccount.User_Status = "1";
                _useraccount.Update_date = DateTime.Now;
                _appdb.Configuration.ValidateOnSaveEnabled = false;
                _appdb.Entry(_useraccount).State = EntityState.Modified;
                _appdb.SaveChanges();
                Notification.set_noti("Restore user successfully: " + _useraccount.Email + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("UserTrash");
            }
        }

        //Edit User Information
        public ActionResult EditUserInformation(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("EditUserInformation", new { returnUrl = Request.UrlReferrer.ToString() });
                }

                var _useraccount = _appdb.Users.Where(u => u.User_id == id).SingleOrDefault();
                if (User.Identity.GetRole() != 1)
                {
                    Notification.set_noti("You do not have permission", "danger");
                    return RedirectToAction("AdminUserIndex");
                }
                else
                {
                    if (_useraccount == null || id == null)
                    {
                        Notification.set_noti("This user is not exist: " + _useraccount.Email + "", "warning");
                        return RedirectToAction("AdminUserIndex");
                    }
                    else
                    {
                        return View(_useraccount);
                    }
                }
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInformation(User model, string returnUrl, int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var _useraccount = _appdb.Users.SingleOrDefault(u => u.User_id == id);
                try
                {
                    _useraccount.Email = model.Email;
                    _useraccount.Avatar = model.Avatar;
                    _useraccount.Full_name = model.Full_name;
                    _useraccount.Phone_number = model.Phone_number;
                    _useraccount.Gender = model.Gender;
                    _useraccount.Date_of_birth = model.Date_of_birth;
                    _useraccount.Update_date = DateTime.Now;
                    _useraccount.User_Status = model.User_Status;
                    _appdb.Configuration.ValidateOnSaveEnabled = false;
                    _appdb.SaveChanges();
                    Notification.set_noti("Update information sucessfully: " + _useraccount.Email + "", "success");

                    if (!String.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Dashboard");
                }
                catch
                {
                    Notification.set_noti("Fail to update", "danger");
                }

                return View(model);
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //ListRole      
        public ActionResult ListUserRole(int? page, int? size)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.GetRole() == 1)
                {
                    var pageSize = size ?? 11;
                    var pageNumber = page ?? 1;
                    ViewBag.rolePermission = _appdb.Role_Permissions.ToList();
                    ViewBag.roles = _appdb.Roles.OrderBy(r => r.Role_id).ToPagedList(pageNumber, pageSize);
                    var rolepermissioncheck = from p in _appdb.Permissions
                                              orderby p.Permission_id ascending
                                              select new
                                              {
                                                  p.Permission_id,
                                                  p.Permission_name,
                                                  Checked_permission = ((from rp in _appdb.Role_Permissions where (rp.Permission_id == p.Permission_id) select rp).Count() > 0)
                                              };
                    var MyrolepermissCheckBoxList = new List<CheckPermissionToRole>();
                    foreach (var item in rolepermissioncheck)
                    {
                        MyrolepermissCheckBoxList.Add(new CheckPermissionToRole { Permission_id = item.Permission_id, Permission_name = item.Permission_name });
                    }
                    DTOofRole dTOofRole = new DTOofRole();
                    dTOofRole.rolePermission = MyrolepermissCheckBoxList;
                    return View(dTOofRole);
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



        //create role
        public JsonResult CreateNewRole(DTOofRole dTOofRole, Role _userrole)
        {
            bool result;
            if (User.Identity.GetRole() == 1)
            {
                _userrole.Role_name = dTOofRole.Role_name;
                _appdb.Roles.Add(_userrole);
                _appdb.Configuration.ValidateOnSaveEnabled = false;
                _appdb.SaveChanges();
                foreach (var item in dTOofRole.rolePermission)
                {
                    if (item.Checked_permission)
                    {
                        _appdb.Role_Permissions.Add(new Role_Permission() { Role_id = _userrole.Role_id, Permission_id = item.Permission_id });
                        _appdb.SaveChanges();
                    }
                }
                Notification.set_noti("Add user role successfully '" + _userrole.Role_name + "'", "success");
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //edit user role       
        public JsonResult EditRole(DTOofRole dTOofRole)
        {
            bool result;
            if (User.Identity.GetRole() == 1)
            {
                Role _userrole = _appdb.Roles.FirstOrDefault(r => r.Role_id == dTOofRole.Role_id);
                _userrole.Role_name = dTOofRole.Role_name;
                foreach (var item in _appdb.Role_Permissions)
                {
                    if (item.Role_id == dTOofRole.Role_id)
                    {
                        _appdb.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                }
                foreach (var item in dTOofRole.rolePermission)
                {
                    if (item.Checked_permission)
                    {
                        _appdb.Role_Permissions.Add(new Role_Permission() { Role_id = _userrole.Role_id, Permission_id = item.Permission_id });
                    }
                }
                _appdb.SaveChanges();
                Notification.set_noti("Edit successfully '" + _userrole.Role_name + "'", "success");
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Admin chage user status
        [HttpPost]
        public JsonResult UserChangeStatus(int id, int role_id)
        {
            bool result;
            if (User.Identity.GetRole() == 1)
            {
                User _useraccount = _appdb.Users.SingleOrDefault(m => m.User_id == id);
                _useraccount.Update_date = DateTime.Now;
                _useraccount.Role_id = role_id;
                _appdb.Configuration.ValidateOnSaveEnabled = false;
                _appdb.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User _useraccount = _appdb.Users.Find(id);
            if (_useraccount == null)
            {
                return HttpNotFound();
            }
            return View(_useraccount);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User _useraccount = _appdb.Users.Find(id);
            _appdb.Users.Remove(_useraccount);
            _appdb.SaveChanges();
            return RedirectToAction("UserTrash");
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