using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Homework.Models
{   
	public  class 客戶分類Repository : EFRepository<客戶分類>, I客戶分類Repository
	{

	}

	public  interface I客戶分類Repository : IRepository<客戶分類>
	{

	}
}