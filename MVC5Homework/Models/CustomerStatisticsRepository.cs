using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Homework.Models
{   
	public  class CustomerStatisticsRepository : EFRepository<CustomerStatistics>, ICustomerStatisticsRepository
	{

	}

	public  interface ICustomerStatisticsRepository : IRepository<CustomerStatistics>
	{

	}
}