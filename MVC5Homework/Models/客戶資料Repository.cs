using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Homework.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override IQueryable<客戶資料> All()
        {
            return base.All().Include(p=>p.客戶分類).Where(p => p.是否已刪除 == false);
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
        public IQueryable<客戶資料> Search(string 類別Id, string keyword)
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

            return 客戶資料;
             
        }

        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
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