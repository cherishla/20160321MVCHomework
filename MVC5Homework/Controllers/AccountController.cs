using MVC5Homework.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5Homework.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        string userData = "";
        string customID = "";
        [AllowAnonymous]
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(CustomLoginViewModel data)
        {
            if (CheckLogin(data))
            {
                //一般都用session ，所以cookie設定為false
                FormsAuthentication.RedirectFromLoginPage(data.Account, false);
            }

            bool isPersistent = false;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
              data.Account,
              DateTime.Now,
              DateTime.Now.AddMinutes(30),
              isPersistent,
              userData,
              FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            return RedirectToAction("Edit", "客戶資料", new { id= customID });

        }

        private bool CheckLogin(CustomLoginViewModel data)
        {
            var customData = new 客戶資料();
            string password = string.Empty;
            if (data.Password != null)
            {
                password = data.Password;

            }
            
            string strPassword = TransferPwd(password);
            customData = repo.Find(data.Account, strPassword);
            if (customData != null)
            {
                if (customData.帳號 == "admin")
                {
                    userData = "admin";
                }
                else
                {
                    userData = "member";
                }
                customID = customData.Id.ToString();

                return true;
            }
            else
                return false;
            
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel data)
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }

    }
}