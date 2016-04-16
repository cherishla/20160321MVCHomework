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
using System.Linq.Dynamic;
using PagedList;

namespace MVC5Homework.Controllers
{
    [Authorize(Roles = "admin")]
    [計算Action的執行時間]

    public class 客戶聯絡人Controller : BaseController
    {
        // GET: 客戶聯絡人
        public ActionResult Index(string keyword, string JobFunc, string sort = "客戶名稱", int page = 1)
        {
          
            var 客戶聯絡人 = repo_客戶聯絡人.All(false, keyword, JobFunc, sort).ToPagedList(page, 2);
            ViewBag.JobFunc = repo_客戶聯絡人.GetJobFunc("");
            ViewBag.sort = sort;
            return View(客戶聯絡人);

        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo_客戶聯絡人.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
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
                repo_客戶聯絡人.Add(客戶聯絡人);
                repo_客戶聯絡人.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo_客戶聯絡人.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
            var 客戶聯絡人 = repo_客戶聯絡人.Find(id);
            if (TryUpdateModel(客戶聯絡人, "Id,客戶Id,職稱,姓名,Email,手機,電話".Split(new char[] { ',' })))
            {
                repo_客戶聯絡人.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo_客戶聯絡人.Find(id.Value);
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
            客戶聯絡人 客戶聯絡人 = repo_客戶聯絡人.Find(id);
            repo_客戶聯絡人.Delete(客戶聯絡人);
            repo_客戶聯絡人.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo_客戶聯絡人.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ExportExcel(string keyword, string JobFunc, string sort = "客戶名稱")
        {
            XSSFWorkbook excel = new XSSFWorkbook();
            XSSFSheet sheet = excel.CreateSheet("客戶銀行資訊") as XSSFSheet;

            var data = new List<客戶聯絡人>();
          
                data = repo_客戶聯絡人.All(false, keyword,JobFunc, sort).ToList();
      
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
            var data = repo_客戶聯絡人.GetDataByID(客戶Id);
            ViewData.Model = data;
            ViewBag.客戶Id = 客戶Id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult BatchUpdate(IList<BatchUpdateContract> data, int 客戶Id)
        {
            if (data == null)
            {
                ViewData.Model = repo_客戶聯絡人.GetDataByID(客戶Id);
                TempData["UpdateMsg"] = "沒有資料可以更新!";

                return PartialView();
            }
            if (ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var 客戶聯絡人 = repo_客戶聯絡人.Find(item.Id);
                    客戶聯絡人.職稱 = item.職稱;
                    客戶聯絡人.手機 = item.手機;
                    客戶聯絡人.電話 = item.電話;
                }
                repo_客戶聯絡人.UnitOfWork.Commit();
                TempData["UpdateMsg"] = "聯絡人更新成功!";
            }
            ViewData.Model = repo_客戶聯絡人.GetDataByID(客戶Id);
            ViewBag.客戶Id = 客戶Id;

            if (TempData["UpdateMsg"] == null)
            TempData["UpdateMsg"] = "聯絡人更新失敗!";

            return PartialView();

        }
    }
}
