using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Homework.Models;
using NPOI.XSSF.UserModel;
using System.IO;

namespace MVC5Homework.Controllers
{
    public class 客戶銀行資訊Controller : Controller
    {
        private 客戶銀行資訊Repository repo = RepositoryHelper.Get客戶銀行資訊Repository();
        private 客戶資料Repository repo_客戶資料 = RepositoryHelper.Get客戶資料Repository();

        // GET: 客戶銀行資訊
        public ActionResult Index()
        {
             CustomerEntities db = new CustomerEntities();
            //TODO Include
            var 客戶銀行資訊 = repo.All(false);
            
            return View(客戶銀行資訊);
        }

        [HttpPost]
        public ActionResult Index(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                RedirectToAction("Index");
            }
            var data = repo.Search(keyword);
            TempData["keyword"] = keyword;

            return View(data.ToList());
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repo_客戶資料.All(false), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶銀行資訊);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repo_客戶資料.All(false), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repo_客戶資料.All(false), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repo.UnitOfWork.Context.Entry(客戶銀行資訊).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repo_客戶資料.All(false), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = repo.Find(id);
            //db.客戶銀行資訊.Remove(客戶銀行資訊);
            repo.Delete(客戶銀行資訊);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult ExportExcel(string currentKeyword)
        {
            XSSFWorkbook excel = new XSSFWorkbook();
            XSSFSheet sheet = excel.CreateSheet("客戶銀行資訊") as XSSFSheet;

            List<客戶銀行資訊> data = new List<客戶銀行資訊>();
            if (string.IsNullOrEmpty(currentKeyword))
                data = repo.All(false).ToList();
            else
                data = repo.Search(currentKeyword).ToList();

            if (data.Count() == 0)
            {
                TempData["ErrMsg"] = "沒有資料可以匯出";
                return RedirectToAction("Index");

            }
            sheet.CreateRow(0);
            sheet.GetRow(0).CreateCell(0).SetCellValue("銀行名稱");
            sheet.GetRow(0).CreateCell(1).SetCellValue("銀行代碼");
            sheet.GetRow(0).CreateCell(2).SetCellValue("分行代碼");
            sheet.GetRow(0).CreateCell(3).SetCellValue("帳戶名稱");
            sheet.GetRow(0).CreateCell(4).SetCellValue("帳戶號碼");
            sheet.GetRow(0).CreateCell(5).SetCellValue("客戶名稱");

            for (int i = 1; i <= data.Count(); i++)
            {
                sheet.CreateRow(i);
                var item = data[i - 1];
                sheet.GetRow(i).CreateCell(0).SetCellValue(item.銀行名稱);
                sheet.GetRow(i).CreateCell(1).SetCellValue(item.銀行代碼);
                sheet.GetRow(i).CreateCell(2).SetCellValue(item.分行代碼.HasValue ? Convert.ToString(item.分行代碼) : "");
                sheet.GetRow(i).CreateCell(3).SetCellValue(item.帳戶名稱);
                sheet.GetRow(i).CreateCell(4).SetCellValue(item.帳戶號碼);
                sheet.GetRow(i).CreateCell(5).SetCellValue(item.客戶資料.客戶名稱);
            }

            MemoryStream ms = new MemoryStream();
            excel.Write(ms);
            excel = null;
            return File(ms.ToArray(), "application/excel", "Report.xlsx");
        }
    }
}
