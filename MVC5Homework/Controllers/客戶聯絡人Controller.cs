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
    public class 客戶聯絡人Controller : Controller
    {
        private 客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        private 客戶資料Repository repo_客戶資料 = RepositoryHelper.Get客戶資料Repository();

        // GET: 客戶聯絡人
        public ActionResult Index()
        {
            var 客戶聯絡人 = repo.All(false).Include(p => p.客戶資料);
            return View(客戶聯絡人.ToList());
        }

        [HttpPost]
        public ActionResult Index(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                RedirectToAction("Index");
            }
            var data = repo.Search(keyword).Include(p => p.客戶資料);
            TempData["keyword"] = keyword;

            return View(data.ToList());
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repo_客戶資料.All(false), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repo_客戶資料.All(false), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repo_客戶資料.All(false), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repo.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repo_客戶資料.All(false), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = repo.Find(id);
            repo.Delete(客戶聯絡人);
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
        public ActionResult ExportExcel(string keyword)
        {
            XSSFWorkbook excel = new XSSFWorkbook();
            XSSFSheet sheet = excel.CreateSheet("客戶銀行資訊") as XSSFSheet;

            List<客戶聯絡人> data = new List<客戶聯絡人>();
            if (string.IsNullOrEmpty(keyword))
                data = repo.All(false).Include(p => p.客戶資料).ToList();
            else
                data = repo.Search(keyword).Include(p => p.客戶資料).ToList();

            if (data.Count() == 0)
            {
                TempData["ErrMsg"] = "沒有資料可以匯出";
                return RedirectToAction("Index");
                
            }

            sheet.CreateRow(0);
            sheet.GetRow(0).CreateCell(0).SetCellValue("職稱");
            sheet.GetRow(0).CreateCell(1).SetCellValue("姓名");
            sheet.GetRow(0).CreateCell(2).SetCellValue("Email");
            sheet.GetRow(0).CreateCell(3).SetCellValue("手機");
            sheet.GetRow(0).CreateCell(4).SetCellValue("電話");
            sheet.GetRow(0).CreateCell(5).SetCellValue("客戶名稱");

            for (int i = 1; i <= data.Count(); i++)
            {
                sheet.CreateRow(i);
                var item = data[i - 1];
                sheet.GetRow(i).CreateCell(0).SetCellValue(item.職稱);
                sheet.GetRow(i).CreateCell(1).SetCellValue(item.姓名);
                sheet.GetRow(i).CreateCell(2).SetCellValue(item.Email);
                sheet.GetRow(i).CreateCell(3).SetCellValue(item.手機);
                sheet.GetRow(i).CreateCell(4).SetCellValue(item.電話);
                sheet.GetRow(i).CreateCell(5).SetCellValue(item.客戶資料.客戶名稱);
            }

            MemoryStream ms = new MemoryStream();
            excel.Write(ms);
            excel = null;
            return File(ms.ToArray(), "application/excel", "Report.xlsx");
        }

        public ActionResult BatchUpdate(int? 客戶Id)
        {
            var data = repo.GetDataByID(客戶Id).Include(p =>p.客戶資料);
            ViewData.Model = data;
            ViewBag.客戶Id = 客戶Id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult BatchUpdate(IList<BatchUpdateContract> data, int 客戶Id)
        {
            if (data == null)
            {
                ViewData.Model = repo.GetDataByID(客戶Id).Include(p => p.客戶資料);
                TempData["UpdateMsg"] = "沒有資料可以更新!";

                return PartialView();
            }
            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var 客戶聯絡人 = repo.Find(item.Id);
                    客戶聯絡人.職稱 = item.職稱;
                    客戶聯絡人.手機 = item.手機;
                    客戶聯絡人.電話 = item.電話;
                }
                repo.UnitOfWork.Commit();
                TempData["UpdateMsg"] = "聯絡人更新成功!";
            }
            ViewData.Model = repo.GetDataByID(客戶Id).Include(p => p.客戶資料);
            ViewBag.客戶Id = 客戶Id;

            if (TempData["UpdateMsg"] == null)
            TempData["UpdateMsg"] = "聯絡人更新失敗!";

            return PartialView();

        }
    }
}
