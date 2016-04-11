using MVC5Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5Homework.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        string userData = "";

        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
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

            return RedirectToAction("Edit", "客戶資料");

        }

        private bool CheckLogin(CustomLoginViewModel data)
        {
            var customData = new 客戶資料();
            string password = string.Empty;
            if (data.Password != null)
            {
                password = data.Password;

            }
            Byte[] data1ToHash = (new UnicodeEncoding()).GetBytes(password);
            byte[] hashvalue1 = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(data1ToHash);
            string strPassword = Convert.ToBase64String(hashvalue1);
            customData = repo.Find(data.Account, strPassword);
            if (customData != null)
            {
                userData = "member";
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