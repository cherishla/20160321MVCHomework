using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Homework.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Include(p=>p.客戶資料).Where(p => p.是否已刪除 == false);
        }

        /// <summary>
        /// 搜尋全部資料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> All(bool isAll)
        {
            if (isAll)
            {
                return base.All().Include(p => p.客戶資料);
            }
            else
            {
                return this.All();
            }
        }

        /// <summary>
        /// 搜尋客戶聯絡人
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return All(false);
            }
            else
            {
                return All(false).Where(
                       p => p.姓名.Contains(keyword) || p.客戶資料.客戶名稱.Contains(keyword)
                    || p.電話.Contains(keyword) || p.手機.Contains(keyword)
                    || p.職稱.Contains(keyword) || p.Email.Contains(keyword));
            }
        }

        /// <summary>
        /// 搜尋客戶聯絡人(By 客戶ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> GetDataByID(int? id)
        {
            if (!id.HasValue)
            {
                return All(false);
            }
            else
            {
                return All(false).Where(p=> p.客戶Id == id);
            }
        }

        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}