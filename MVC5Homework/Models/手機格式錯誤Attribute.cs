using System;
using System.ComponentModel.DataAnnotations;

namespace MVC5Homework.Models
{
    public class 手機格式錯誤Attribute : DataTypeAttribute
    {
        public 手機格式錯誤Attribute():base(DataType.Text)
        {
                
        }

        public override bool IsValid(object value)
        {
            string str = (string)value;

            //長度不為10的時候，拋錯
            if (str.Length != 11)
                return false;

            for (int i = 0; i < str.Length; i++)
            {
                if (i != 4)
                {
                    if (char.IsDigit(str[i]) == false)
                        return false;
                }
                else {
                    if (str[i] != '-')
                        return false;
                }
            }


            return true;
        }
    }
}