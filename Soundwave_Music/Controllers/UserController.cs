using Newtonsoft.Json;
using Soundwave_Music.Common;
using Soundwave_Music.Common.Helper;
using Soundwave_Music.Common.NotificationLib;
using Soundwave_Music.Models;
using Soundwave_Music.Models.UserManagementModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;

namespace Soundwave_Music.Controllers
{
    public class UserController : Controller
    {
        //Call SoundwaveDbContext to use models
        private readonly SoundwaveDbContext _appdb = new SoundwaveDbContext();

        //Sign In VIEW
        public ActionResult SignIn(string returnURL)
        {
            if (String.IsNullOrEmpty(returnURL) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("SignIn", new { returnURL = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(Sign_In model, string returnUrl)
        {
            //hash password
            model.Password = Crypto.Hash(model.Password);

            var Data_user = _appdb.Users.Where(m => m.User_Status == "1" && m.Email.ToLower() == model.Email && m.Password == model.Password).SingleOrDefault();
            var check_user_if_disable = _appdb.Users.Where(m => m.User_Status == "2" && m.Email.ToLower() == model.Email && m.Password == model.Password).SingleOrDefault();
            var check_user_if_activate = _appdb.Users.Where(m => m.User_Status == "0" && m.Email.ToLower() == model.Email && m.Password == model.Password).SingleOrDefault();
            bool create = false; bool view = false; bool update = false; bool delete = false; bool access = false; bool edit = false;

            if (check_user_if_disable != null)
            {
                Notification.set_noti1s("This user is disabled", "danger");
            }
            else
            if (check_user_if_activate != null)
            {
                TempData["User_id"] = check_user_if_activate.User_id;
                TempData["Email_id"] = check_user_if_activate.Email;
                String Verify_code = Guid.NewGuid().ToString();
                EmailForVerification(check_user_if_activate.Email, Verify_code, "UserVerification");
                return RedirectToAction("SendVerifyEmail", "User");
            }
            else if (Data_user != null)
            {
                List<Role_Permission> roles_permissions = Data_user.Role.Role_Permission.ToList();
                foreach (var permission in roles_permissions)
                {
                    if (permission.Permission_id == 1) { create = true; }
                    if (permission.Permission_id == 2) { edit = true; }
                    if (permission.Permission_id == 3) { delete = true; }
                    if (permission.Permission_id == 4) { view = true; }
                    if (permission.Permission_id == 5) { update = true; }
                    if (permission.Permission_id == 6) { access = true; }
                }
                //When user login sucessfull, user data will be stored
                var saved_user_data = new SignInUserSavedData
                {
                    User_id = Data_user.User_id,
                    Full_name = Data_user.Full_name,
                    Email = Data_user.Email,
                    Permission_create = create,
                    Permission_edit = edit,
                    Permission_delete = delete,
                    Permission_view = view,
                    Permission_update = update,
                    Permission_access = access,
                    Role_id = Data_user.Role.Role_id,
                    Role_name = Data_user.Role.Role_name,
                    Avatar = Data_user.Avatar,
                    Phone_number = Data_user.Phone_number,
                };
                FormsAuthentication.SetAuthCookie(JsonConvert.SerializeObject(saved_user_data), false);
                Notification.set_noti1s("Login Sucessfully", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                Notification.set_noti1s("You input the wrong email or password", "danger");
            }
            return View(model);
        }

        //Logout View
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Notification.set_noti1s("Logout Successfully", "success");
            return RedirectToAction("Index", "Home");       
        }

        //SIGN UP
        public ActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Exclude = "User_Status,Verification_code")] User _useraccount, Sign_Up model)
        {
            string fail = "";
            string success = "";
            //Check email if exist in database
            var check_existed_email = _appdb.Users.Any(m => m.Email == model.Email);
            //check number phone if exist in database
            var check_existed_phone_number = _appdb.Users.Any(m => m.Phone_number == model.Phone_number);
            if (check_existed_email)
            {
                fail = "This email has been used, please input another email";
            }
            else if (check_existed_phone_number)
            {
                fail = "This phone number has been used, please input another phone number";
            }
            else
            {
                _appdb.Configuration.ValidateOnSaveEnabled = false;
                _useraccount.Password = Crypto.Hash(model.Password.Trim());
                _useraccount.Email = model.Email;
                _useraccount.Avatar = "/Images/svg/avatars/001-boy.svg";
                _useraccount.Full_name = model.Full_name;
                if (model.Phone_number.StartsWith("84"))
                {
                    model.Phone_number = Regex.Replace(model.Phone_number, @"84", "0");
                    _useraccount.Phone_number = model.Phone_number;
                }
                else
                {
                    _useraccount.Phone_number = model.Phone_number;
                }
                _useraccount.Gender = "1";
                _useraccount.Date_of_birth = DateTime.Now;
                _useraccount.Create_date = DateTime.Now;
                _useraccount.Update_date = DateTime.Now;
                model.Verification_code = Guid.NewGuid().ToString();
                _useraccount.Verification_code = model.Verification_code;
                _useraccount.User_Status = "0";
                _useraccount.Role_id = Const.USER_id;
                _useraccount.Expired = DateTime.Now.AddMinutes(10);
                EmailForVerification(model.Email, _useraccount.Verification_code, "UserVerification");
                _appdb.Users.Add(_useraccount);
                _appdb.SaveChanges();
                TempData["User_id"] = _useraccount.User_id;
                TempData["Email_id"] = _useraccount.Email;
                return RedirectToAction("SendVerifyEmail", "User");
            }
            ViewBag.Success = success;
            ViewBag.Fail = fail;
            return View(model);
        }

        //User Verification
        [HttpGet]
        public ActionResult UserVerification(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                bool activate = false;
                {
                    _appdb.Configuration.ValidateOnSaveEnabled = false;
                    var verification = _appdb.Users.Where(u => u.Verification_code == new Guid(id).ToString()).FirstOrDefault();
                    if (verification != null && verification.Expired > DateTime.Now)
                    {
                        verification.Update_date = DateTime.Now;
                        verification.Expired = DateTime.Now;
                        verification.User_Status = "1";
                        verification.Verification_code = "";
                        _appdb.SaveChanges();
                        activate = true;
                    }
                    else
                    {
                        ViewBag.Message = "Your request is invalid";
                    }
                }
                ViewBag.Status = activate;
            }
            return View();
        }

        //Reset Password
        public ActionResult UserResetPassword(string id)
        {
            var _useraccount = _appdb.Users.Where(u => u.Verification_code == id).FirstOrDefault();
            if (_useraccount != null)
            {
                Reset_Password model = new Reset_Password();
                model.Reset_password = id;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserResetPassword(Reset_Password model)
        {
            if (ModelState.IsValid)
            {
                var _useraccount = _appdb.Users.Where(u => u.Verification_code == model.Reset_password).FirstOrDefault();
                if (_useraccount != null && _useraccount.Expired > DateTime.Now)
                {
                    _useraccount.Password = Crypto.Hash(model.Input_new_password);
                    _useraccount.Verification_code = "";
                    _useraccount.Update_date = DateTime.Now;
                    _useraccount.User_Status = "1";
                    _useraccount.Expired = DateTime.Now;
                    _appdb.Configuration.ValidateOnSaveEnabled = false;
                    _appdb.SaveChanges();
                    Notification.set_noti("Update successfully", "success");
                    return RedirectToAction("SignIn");
                }
            }
            else
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //Send verify email
        public ActionResult SendVerifyEmail()
        {
            ViewBag.User_id = TempData["User_id"];
            ViewBag.Email = TempData["Email_id"];
            if (ViewBag.User_id == null)
            {
                return RedirectToAction("SignIn", "User");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ReSendVerifyEmail(User model)
        {
            var _useraccount = _appdb.Users.FirstOrDefault(u => u.User_id == model.User_id);
            if (_useraccount != null)
            {
                string verify_code = Guid.NewGuid().ToString();
                string EmailID = _useraccount.Email;
                EmailForVerification(EmailID, verify_code, "UserVerification");
                _useraccount.Email = _useraccount.Email;
                _useraccount.Verification_code = verify_code;
                _useraccount.Update_date = DateTime.Now;
                _useraccount.Expired = DateTime.Now.AddMinutes(10);
                _appdb.Configuration.ValidateOnSaveEnabled = false;
                _appdb.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        //Forgot Password
        public ActionResult UserForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult UserForgotPassword(string Email_id)
        {
            string fail = "";
            string success = "";
            var _useraccount = _appdb.Users.Where(u => u.Email == Email_id && u.User_Status != "2").FirstOrDefault();
            if (_useraccount != null)
            {
                string send_reset_code = Guid.NewGuid().ToString();
                EmailForVerification(_useraccount.Email, send_reset_code, "UserResetPassword");
                string sendmail = _useraccount.Email;
                _useraccount.Verification_code = send_reset_code;
                _useraccount.Expired = DateTime.Now.AddMinutes(10);
                _appdb.Configuration.ValidateOnSaveEnabled = false;
                _appdb.SaveChanges();
                success = "Hello " + Email_id + " The reset password link is sent to your email. Please check and conduct to reset your password";
            }
            else
            {
                fail = "This email have not stored in database.";
            }

            ViewBag.Message1 = success;
            ViewBag.Message2 = fail;
            return View();
        }

        //User Change Password
        public ActionResult UserChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity_user_id = User.Identity.GetUserId();
                var _useraccount = _appdb.Users.Where(u => u.User_id == identity_user_id).FirstOrDefault();
                ViewBag.UserFullName = _useraccount.Full_name;
                ViewBag.Avatar = _useraccount.Avatar;
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "User");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserChangePassword(Change_Password model)
        {
            var identity_user_id = User.Identity.GetUserId();
            model.Input_old_password = Crypto.Hash(model.Input_old_password);
            string check_equal_new_password = Crypto.Hash(model.Input_new_password);
            var _useraccount = _appdb.Users.Where(u => u.User_id == identity_user_id).FirstOrDefault();
            var check_password = _appdb.Users.Any(u => u.Password == model.Input_old_password && u.User_id == identity_user_id);
            if (check_password)
            {
                if (check_equal_new_password == _useraccount.Password)
                {
                    Notification.set_noti("Your new password and old password cannot be the samw", "danger");
                }
                else
                {
                    if (_useraccount != null)
                    {
                        _appdb.Configuration.ValidateOnSaveEnabled = false;
                        _useraccount.Password = Crypto.Hash(model.Input_new_password);
                        _useraccount.Update_date = DateTime.Now;
                        _appdb.SaveChanges();
                        Notification.set_noti("Update sucessfully", "success");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Notification.set_noti("Fail to update", "danger");
                    }
                }

            }
            else
            {
                Notification.set_noti("Your old password is not correct, please try again", "danger");
            }
            return View();
        }

        //User View Details
        public ActionResult UserViewDetails()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity_user_id = User.Identity.GetUserId();
                var _useraccount = _appdb.Users.Where(u => u.User_id == identity_user_id).FirstOrDefault();
                ViewBag.Avatar = _useraccount.Avatar;
                ViewBag.UserFullName = _useraccount.Full_name;
                return View(_useraccount);
            }
            else
            {
                Notification.set_noti("Please Login To System", "danger");
                return RedirectToAction("SignIn", "User");
            }
        }

        //User Edit Profile
        public ActionResult UserEditProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity_user_id = User.Identity.GetUserId();
                var _useraccount = _appdb.Users.Where(u => u.User_id == identity_user_id).FirstOrDefault();
                ViewBag.Avatar = _useraccount.Avatar;
                ViewBag.UserFullName = _useraccount.Full_name;
                return View(_useraccount);
            }
            else
            {
                Notification.set_noti("Please Login To System", "danger");
                return RedirectToAction("SignIn", "User");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEditProfile(User model)
        {
            try
            {
                var identity_user_id = User.Identity.GetUserId();
                var _useraccount = _appdb.Users.Where(u => u.User_id == identity_user_id).FirstOrDefault();
                if (model.AvatarUpload == null)
                {
                    _useraccount.Avatar = _useraccount.Avatar;
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.AvatarUpload.FileName);
                    string extension = Path.GetExtension(model.AvatarUpload.FileName);
                    fileName = SlugGenerator.SlugGenerator.GenerateSlug(fileName) + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + extension;
                    _useraccount.Avatar = "/Images/ImagesAvatar/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/ImagesAvatar/"), fileName);
                    model.AvatarUpload.SaveAs(fileName);
                }
                _useraccount.User_id = identity_user_id;
                _useraccount.Full_name = model.Full_name;
                if (model.Phone_number.StartsWith("84"))
                {
                    model.Phone_number = Regex.Replace(model.Phone_number, @"84", "0");
                    _useraccount.Phone_number = model.Phone_number;
                }
                else
                {
                    _useraccount.Phone_number = model.Phone_number;
                }
                _useraccount.Gender = model.Gender;
                _useraccount.Date_of_birth = model.Date_of_birth;
                _useraccount.Update_date = DateTime.Now;
                _useraccount.User_Status = "1";
                _appdb.Configuration.ValidateOnSaveEnabled = false;
                _appdb.SaveChanges();
                Notification.set_noti("Update Sucessfully", "success");
                return RedirectToAction("UserEditProfile");
            }
            catch
            {
                Notification.set_noti("Fail to update", "danger");
            }
            return View();
        }

        //Send verify email for user when they finish signup, forgot password.
        //Send email for verify use account and when user forgot their password
        [NonAction]
        public void EmailForVerification(string Email_id, string Activate_code, string SendEmailFor)
        {
            var Verification_URL = "/User/" + SendEmailFor + "/" + Activate_code;
            var Email_from = new MailAddress(ApplicationEmail.UserEmail, ApplicationEmail.Name);
            var Email_to = new MailAddress(Email_id);
            var Email_password = ApplicationEmail.Password;
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/MailTemplates/") + "Mail_Verification" + ".cshtml");
            if (SendEmailFor == "UserVerification")
            {
                subject = "User Verification of" + Email_id;
                body = body.Replace("{{ViewBag.Sendmail}}", "User Verification");
                body = body.Replace("{{ViewBag.Confirmtext}}", "Activate User");
                body = body.Replace("{{ViewBag.Bodytext}}", "This link is valided within <span style='font-weight:600;'>10 minutes</span>, please click into below button to activate your user account.");
                body = body.Replace("{{ViewBag.Confirmlink}}", Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Verification_URL));
            }
            else if (SendEmailFor == "UserResetPassword")
            {
                subject = "Reset Password For" + Email_id;
                body = body.Replace("{{ViewBag.Sendmail}}", "Reset Password");
                body = body.Replace("{{ViewBag.Confirmtext}}", "Setup New Password");
                body = body.Replace("{{ViewBag.Bodytext}}", "This link is valided within <span style='font-weight:600;'>10 minutes</span>, please click into below button to activate your user account.");
                body = body.Replace("{{ViewBag.Confirmlink}}", Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Verification_URL));
            }
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Email_from.Address, Email_password)
            };
            using (var Email_message = new MailMessage(Email_from, Email_to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(Email_message);
        }

        //check user if login
        public ActionResult UserLogged()
        {
            return Json(User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }

        //Comment Song
        public ActionResult SongCommentCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity_user_id = User.Identity.GetUserId();
                var _useraccount = _appdb.Users.Where(u => u.User_id == identity_user_id).FirstOrDefault();
                ViewBag.UserFullName = _useraccount.Full_name;
                ViewBag.Avatar = _useraccount.Avatar;

                if (User.Identity.GetRole() == 2)
                {
                    ViewBag.ListSong = new SelectList(_appdb.Songs.Where(a => (a.Song_status == "1") && a.Area.Area_id == 1).OrderBy(a => a.Song_name), "Song_id", "Song_name", 0);
                }
                else
                {
                    ViewBag.ListSong = new SelectList(_appdb.Songs.Where(a => (a.Song_status == "1")).OrderBy(a => a.Song_name), "Song_id", "Song_name", 0);

                }
                return View();
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Song Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SongCommentCreate(Song_Comment songcomment)
        {
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.ListSong = new SelectList(_appdb.Songs.Where(a => (a.Song_status == "1") && a.Area.Area_id == 1).OrderBy(a => a.Song_name), "Song_id", "Song_name", 0);
            }
            else
            {
                ViewBag.ListSong = new SelectList(_appdb.Songs.Where(a => (a.Song_status == "1")).OrderBy(a => a.Song_name), "Song_id", "Song_name", 0);

            }
            try
            {
                songcomment.Content = songcomment.Content;
                songcomment.Create_date = DateTime.Now;
                songcomment.Song_id = songcomment.Song_id;
                songcomment.User_id = User.Identity.GetUserId();
                _appdb.Song_Comment.Add(songcomment);
                _appdb.SaveChanges();
                Notification.set_noti("Your comment has been created successfully", "success");
                return RedirectToAction("SongCommentCreate");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View();
        }

        //Comment Album
        public ActionResult AlbumCommentCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity_user_id = User.Identity.GetUserId();
                var _useraccount = _appdb.Users.Where(u => u.User_id == identity_user_id).FirstOrDefault();
                ViewBag.UserFullName = _useraccount.Full_name;
                ViewBag.Avatar = _useraccount.Avatar;
                if (User.Identity.GetRole() == 2)
                {
                    ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1") && a.Area.Area_id == 1).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
                }
                else
                {
                    ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);

                }
                return View();
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Album code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlbumCommentCreate(Album_Comment albumcomment)
        {
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1") && a.Area.Area_id == 1).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);
            }
            else
            {
                ViewBag.ListAlbum = new SelectList(_appdb.Albums.Where(a => (a.Album_status == "1")).OrderBy(a => a.Album_name), "Album_id", "Album_name", 0);

            }
            try
            {
                albumcomment.Content = albumcomment.Content;
                albumcomment.Create_date = DateTime.Now;
                albumcomment.Album_id = albumcomment.Album_id;
                albumcomment.User_id = User.Identity.GetUserId();
                _appdb.Album_Comment.Add(albumcomment);
                _appdb.SaveChanges();
                Notification.set_noti("Your comment has been created successfully", "success");
                return RedirectToAction("AlbumCommentCreate");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View();
        }

        //Comment Video
        public ActionResult VideoCommentCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity_user_id = User.Identity.GetUserId();
                var _useraccount = _appdb.Users.Where(u => u.User_id == identity_user_id).FirstOrDefault();
                ViewBag.UserFullName = _useraccount.Full_name;
                ViewBag.Avatar = _useraccount.Avatar;
                if (User.Identity.GetRole() == 2)
                {
                    ViewBag.ListVideo = new SelectList(_appdb.Videos.Where(a => (a.Video_status == "1") && a.Area.Area_id == 1).OrderBy(a => a.Video_name), "Video_id", "Video_name", 0);
                }
                else
                {
                    ViewBag.ListVideo = new SelectList(_appdb.Videos.Where(a => (a.Video_status == "1")).OrderBy(a => a.Video_name), "Video_id", "Video_name", 0);

                }
                return View();
            }
            else
            {
                return Redirect("~/User/SignIn");
            }
        }

        //Create new Video code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VideoCommentCreate(Video_Comment videocomment)
        {
            if (User.Identity.GetRole() == 2)
            {
                ViewBag.ListVideo = new SelectList(_appdb.Videos.Where(a => (a.Video_status == "1") && a.Area.Area_id == 1).OrderBy(a => a.Video_name), "Video_id", "Video_name", 0);
            }
            else
            {
                ViewBag.ListVideo = new SelectList(_appdb.Videos.Where(a => (a.Video_status == "1")).OrderBy(a => a.Video_name), "Video_id", "Video_name", 0);

            }
            try
            {
                videocomment.Content = videocomment.Content;
                videocomment.Create_date = DateTime.Now;
                videocomment.Video_id = videocomment.Video_id;
                videocomment.User_id = User.Identity.GetUserId();
                _appdb.Video_Comment.Add(videocomment);
                _appdb.SaveChanges();
                Notification.set_noti("Your comment has been created successfully", "success");
                return RedirectToAction("VideoCommentCreate");
            }
            catch
            {
                Notification.set_noti("Error!!!", "danger");
            }
            return View();
        }
    }
}