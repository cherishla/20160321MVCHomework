namespace MVC5Homework.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(CustomerStatisticsMetaData))]
    public partial class CustomerStatistics
    {
    }
    
    public partial class CustomerStatisticsMetaData
    {
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 客戶名稱 { get; set; }
        public Nullable<int> 銀行帳戶資料 { get; set; }
        public Nullable<int> 聯絡人數量 { get; set; }
    }
}
