using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5Homework.Models;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Linq.Dynamic;
using PagedList;


namespace MVC5Homework.Controllers
{
    [計算Action的執行時間]
    public class 客戶資料Controller : BaseController
    {   
        // GET: 客戶資料
        [Authorize(Roles = "admin")]
        public ActionResult Index(string 類別Id, string keyword, string sort= "客戶名稱", int page=1)
        {
            ViewBag.sort = sort;
            var data = repo.All(false, sort, 類別Id, keyword).ToPagedList(page,2);
            
            return View(data);

        }

        [Authorize(Roles = "admin")]
        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [Authorize(Roles = "admin")]    
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,帳號,密碼,電話,傳真,地址,Email,類別Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                客戶資料.密碼 = TransferPwd(客戶資料.密碼);
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        [Authorize]
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
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            客戶資料.密碼 = TransferPwd(客戶資料.密碼);
            if (TryUpdateModel(客戶資料, "Id,客戶名稱,統一編號,帳號,密碼,電話,傳真,地址,Email,類別Id".Split(new char[] { ',' })))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        [Authorize(Roles = "admin")]
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


        // GET: 客戶資料/Delete/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            //db.客戶資料.Remove(客戶資料);
            repo.Delete(客戶資料);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "admin")]
        public ActionResult CustomerStatisticsList()
        {
            var repo_CustomerView = RepositoryHelper.GetCustomerStatisticsRepository();
            var data = repo_CustomerView.All();
            return View(data);
        }

        //[Authorize(Roles = "admin")]
        public ActionResult ExportExcel(string 類別Id, string keyword, string sort = "客戶名稱")
        {
            XSSFWorkbook excel = new XSSFWorkbook();
            XSSFSheet sheet = excel.CreateSheet("客戶資料") as XSSFSheet;

            List<客戶資料> data = new List<客戶資料>();
            
            data = repo.All(false, sort, 類別Id, keyword).ToList();
            
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);

        }
    }
}
