using System.Linq;
using System.Data.Entity;
using System.Linq.Dynamic;
namespace MVC5Homework.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Include(p => p.客戶資料).Where(p => p.是否已刪除 == false);
        }

 
        /// <summary>
        /// 搜尋全部資料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public IQueryable<客戶銀行資訊> All(bool isAll)
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
        public IQueryable<客戶銀行資訊> All(bool isAll, string keyword, string sort)
        {
            if (sort == "客戶名稱" || sort == "客戶名稱 desc")
                sort = $@"客戶資料.{sort}";

            if (string.IsNullOrEmpty(keyword))
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
                return Search(keyword, sort);
        }
        /// <summary>b
        /// 搜尋客戶銀行資料
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IQueryable<客戶銀行資訊> Search(string keyword, string sort)
        {
            return All(false).Where(
                   p => p.帳戶名稱.Contains(keyword) || p.客戶資料.客戶名稱.Contains(keyword)
                || p.分行代碼.ToString().Contains(keyword) || p.帳戶號碼.Contains(keyword)
                || p.銀行代碼.ToString().Contains(keyword) || p.銀行名稱.Contains(keyword)).OrderBy(sort); 

        }

        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = true;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}