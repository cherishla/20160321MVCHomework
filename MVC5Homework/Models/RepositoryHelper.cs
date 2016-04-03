namespace MVC5Homework.Models
{
	public static class RepositoryHelper
	{
		public static IUnitOfWork GetUnitOfWork()
		{
			return new EFUnitOfWork();
		}		
		
		public static CustomerStatisticsRepository GetCustomerStatisticsRepository()
		{
			var repository = new CustomerStatisticsRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static CustomerStatisticsRepository GetCustomerStatisticsRepository(IUnitOfWork unitOfWork)
		{
			var repository = new CustomerStatisticsRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static 客戶分類Repository Get客戶分類Repository()
		{
			var repository = new 客戶分類Repository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static 客戶分類Repository Get客戶分類Repository(IUnitOfWork unitOfWork)
		{
			var repository = new 客戶分類Repository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static 客戶資料Repository Get客戶資料Repository()
		{
			var repository = new 客戶資料Repository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static 客戶資料Repository Get客戶資料Repository(IUnitOfWork unitOfWork)
		{
			var repository = new 客戶資料Repository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static 客戶銀行資訊Repository Get客戶銀行資訊Repository()
		{
			var repository = new 客戶銀行資訊Repository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static 客戶銀行資訊Repository Get客戶銀行資訊Repository(IUnitOfWork unitOfWork)
		{
			var repository = new 客戶銀行資訊Repository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static 客戶聯絡人Repository Get客戶聯絡人Repository()
		{
			var repository = new 客戶聯絡人Repository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static 客戶聯絡人Repository Get客戶聯絡人Repository(IUnitOfWork unitOfWork)
		{
			var repository = new 客戶聯絡人Repository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		
	}
}