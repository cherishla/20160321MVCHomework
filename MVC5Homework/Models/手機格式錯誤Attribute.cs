using System;
using System.ComponentModel.DataAnnotations;

namespace MVC5Homework.Models
{
    public class 手機格式錯誤Attribute : RegularExpressionAttribute
    {
        public 手機格式錯誤Attribute(): base(@"^\d{4}-\d{6}$")
        {
                
        }

       
    }
}