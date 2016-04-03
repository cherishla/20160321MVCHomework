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
    public class 客戶資料Controller : Controller
    {   
         客戶資料Repository repo= RepositoryHelper.Get客戶資料Repository();
        客戶分類Repository repo_客戶分類 = RepositoryHelper.Get客戶分類Repository();
        // GET: 客戶資料
        public ActionResult Index()
        {
            var data = repo.All(false).Include(p => p.客戶分類);
            ViewBag.類別Id = new SelectList(repo_客戶分類.All(), "Id", "類別");
            return View(data.ToList());
        }

        [HttpPost]
        public ActionResult Index(string 類別Id, string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                RedirectToAction("Index");
            }
            var data = repo.Search(類別Id, keyword).Include(p => p.客戶分類);
            ViewBag.類別Id = new SelectList(repo_客戶分類.All(), "Id", "類別",  類別Id);
            TempData["keyword"] = keyword;
            TempData["Category"] = 類別Id;
            return View(data.ToList());
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value); 
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            ViewBag.類別Id = new SelectList(repo_客戶分類.All(), "Id", "類別");
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,類別Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.類別Id = new SelectList(repo_客戶分類.All(), "Id","類別", 客戶資料.類別Id);
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.類別Id = new SelectList(repo_客戶分類.All(), "Id", "類別", 客戶資料.類別Id);
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,類別Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.類別Id = new SelectList(repo_客戶分類.All(), "Id", "類別", 客戶資料.類別Id);
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            //db.客戶資料.Remove(客戶資料);
            repo.Delete(客戶資料);
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
        public ActionResult CustomerStatisticsList()
        {
            var repo_CustomerView = RepositoryHelper.GetCustomerStatisticsRepository();
            var data = repo_CustomerView.All();
            return View(data);
        }

        public ActionResult ExportExcel(string keyword, string categoryID)
        {
            XSSFWorkbook excel = new XSSFWorkbook();
            XSSFSheet sheet = excel.CreateSheet("客戶資料") as XSSFSheet;

            List<客戶資料> data = new List<客戶資料>();
            if(string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(categoryID))
                data = repo.All(false).Include(p=>p.客戶分類).ToList();
            else
                data = repo.Search(categoryID, keyword).Include(p => p.客戶分類).ToList();

            if (data.Count() == 0)
            {
                TempData["ErrMsg"] = "沒有資料可以匯出";
                return RedirectToAction("Index");

            }
            sheet.CreateRow(0);
            sheet.GetRow(0).CreateCell(0).SetCellValue("客戶名稱");
            sheet.GetRow(0).CreateCell(1).SetCellValue("類別");
            sheet.GetRow(0).CreateCell(2).SetCellValue("統一編號");
            sheet.GetRow(0).CreateCell(3).SetCellValue("電話");
            sheet.GetRow(0).CreateCell(4).SetCellValue("傳真");
            sheet.GetRow(0).CreateCell(5).SetCellValue("地址");
            sheet.GetRow(0).CreateCell(6).SetCellValue("Email");

            for (int i = 1; i <= data.Count(); i++)
            {
                sheet.CreateRow(i);
                var item = data[i-1];
                sheet.GetRow(i).CreateCell(0).SetCellValue(item.客戶名稱);
                sheet.GetRow(i).CreateCell(1).SetCellValue(item.客戶分類.類別);
                sheet.GetRow(i).CreateCell(2).SetCellValue(item.統一編號);
                sheet.GetRow(i).CreateCell(3).SetCellValue(item.電話);
                sheet.GetRow(i).CreateCell(4).SetCellValue(item.傳真);
                sheet.GetRow(i).CreateCell(5).SetCellValue(item.地址);
                sheet.GetRow(i).CreateCell(6).SetCellValue(item.Email);
            }

            MemoryStream ms = new MemoryStream();
            excel.Write(ms);
            excel = null;
            return File(ms.ToArray(), "application/excel", "Report.xlsx");
        }
    }
}
