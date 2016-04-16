using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
namespace MVC5Homework.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override IQueryable<客戶資料> All()
        {
            return base.All().Include(p=>p.客戶分類).Where(p => p.是否已刪除 == false);
        }

        internal bool CheckAccount(object value)
        {
            var data = All(false).FirstOrDefault(p => p.帳號 == value.ToString());

            if (data != null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 搜尋全部資料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public IQueryable<客戶資料> All(bool isAll, string sort, string 類別Id, string keyword)
        {
            if (sort == "類別" || sort == "類別 desc")
                sort = $@"客戶分類.{sort}";
            if (string.IsNullOrEmpty(類別Id) && string.IsNullOrEmpty(keyword))
            {
                if (isAll)
                {
                    return base.All().Include(p => p.客戶分類).OrderBy(sort);
                }
                else
                {
                    return this.All().OrderBy(sort);
                }
            }
            else
                return Search(類別Id, keyword, sort);
        }

        /// <summary>
        /// 搜尋全部資料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public IQueryable<客戶資料> All(bool isAll)
        {
            if (isAll)
            {
                return base.All().Include(p => p.客戶分類);
            }
            else
            {
                return this.All();
            }
        }

        /// <summary>
        /// 搜尋客戶資料
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IQueryable<客戶資料> Search(string 類別Id, string keyword, string sort)
        {
            var 客戶資料 = All(false).Where(p => p.客戶名稱.Contains(keyword)
                        || p.統一編號.Contains(keyword) || p.電話.Contains(keyword)
                        || p.傳真.Contains(keyword) || p.地址.Contains(keyword)
                        || p.Email.Contains(keyword));


            if (!string.IsNullOrEmpty(類別Id))
            {
               int i = int.Parse(類別Id);
               客戶資料 = 客戶資料.Where(p => p.類別Id == i);
            }

            return 客戶資料.OrderBy(sort);
             
        }

        public 客戶資料 Find(int id)
        {
            return this.All(false).FirstOrDefault(p => p.Id == id);
        }

        public 客戶資料 Find(string account, string password)
        {
            if(!string.IsNullOrEmpty(password))
                return this.All(false).FirstOrDefault(p => p.帳號 == account && p.密碼 == password); 
            else
                return this.All(false).FirstOrDefault(p => p.帳號 == account);

        }
        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}