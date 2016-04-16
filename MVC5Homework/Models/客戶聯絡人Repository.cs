using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using System.Linq.Dynamic;
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
        /// 搜尋全部資料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> All(bool isAll, string keyword, string JobFunc, string sort)
        {
            if (sort == "客戶名稱" || sort == "客戶名稱 desc")
                sort = $@"客戶資料.{sort}";

            if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(JobFunc))
            {
                if (isAll)
                {
                    return base.All().Include(p => p.客戶資料).OrderBy(sort);
                }
                else
                {
                    return this.All().OrderBy(sort);
                }
            }
            else
                return Search(keyword, JobFunc, sort);
        }

        internal IQueryable<SelectListItem> GetJobFunc(string JobFunc)
        {
            return All(false).Select(p => new SelectListItem() { Text = p.職稱, Value = p.職稱, Selected = p.職稱 == JobFunc }).Distinct();
            
        }

        /// <summary>
        /// 搜尋客戶聯絡人
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="JobFunc"></param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> Search(string keyword, string JobFunc, string sort)
        {
            
            var data = All(false).Where(
                        p => p.姓名.Contains(keyword) || p.客戶資料.客戶名稱.Contains(keyword)
                        || p.電話.Contains(keyword) || p.手機.Contains(keyword)
                        || p.Email.Contains(keyword));

            if (!string.IsNullOrEmpty(JobFunc))
            {
                data = data.Where(p => p.職稱 == JobFunc);
            }

            return data.OrderBy(sort);
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