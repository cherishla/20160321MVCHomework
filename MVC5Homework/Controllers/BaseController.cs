using MVC5Homework.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace MVC5Homework.Controllers
{
    public abstract class BaseController : Controller
    {
        protected 客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        protected 客戶分類Repository repo_客戶分類 = RepositoryHelper.Get客戶分類Repository();
        protected 客戶銀行資訊Repository repo_銀行資訊 = RepositoryHelper.Get客戶銀行資訊Repository();
        protected 客戶聯絡人Repository repo_客戶聯絡人 = RepositoryHelper.Get客戶聯絡人Repository();
        public string TransferPwd(string pwd)
        {
            byte[] data1ToHash = (new UnicodeEncoding()).GetBytes(pwd);
            byte[] hashvalue1 = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(data1ToHash);
            string strPassword = Convert.ToBase64String(hashvalue1);

            return strPassword;
        }
    }


}