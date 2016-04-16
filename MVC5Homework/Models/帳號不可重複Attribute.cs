using System;
using System.ComponentModel.DataAnnotations;

namespace MVC5Homework.Models
{
    internal class 帳號不可重複Attribute : DataTypeAttribute
    {
        public 帳號不可重複Attribute() : base(DataType.Text)
        {

        }

        public override bool IsValid(object value)
        {
            var repo = RepositoryHelper.Get客戶資料Repository();
            return repo.CheckAccount(value);
        }
    }
}