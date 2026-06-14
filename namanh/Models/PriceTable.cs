using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace namanh.Models
{
    public class PriceTable
    {
        public int Id { get; set; }
        public int CarServiceId { get; set; }
        [Required]
        [Display(Name = "Lộ trình"), UIHint("TextBox")]
        public string RouteDescription { get; set; }
        [Required]
        [Display(Name = "Giá xe 4"), UIHint("TextBox")]
        public string Price4 { get; set; }
        [Required]
        [Display(Name = "Giá xe 7"), UIHint("TextBox")]
        public string Price7 { get; set; }
        [Required]
        [Display(Name = "Giá xe 16"), UIHint("TextBox")]
        public string Price16 { get; set; }
        [Required]
        [Display(Name = "Giá xe 29"), UIHint("TextBox")]
        public string Price29 { get; set; }
        [Required]
        [Display(Name = "Giá xe 45"), UIHint("TextBox")]
        public string Price45 { get; set; }
        [Required]
        [Display(Name = "Giá limousine"), UIHint("TextBox")]
        public string PriceLim { get; set; }
        public bool Hot { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
    }




    public class PriceLangding
    {
        public int Id { get; set; }
        [Display(Name = "Tên"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Mô tả"), UIHint("TextBox")]
        public string Description { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { set; get; }
        public virtual ICollection<Location> Locations { get; set; }
    }



    public class Location
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Điểm đến")]
        public string Name { get; set; }

        [Display(Name = "Giá"), UIHint("TextBox")]
        public string Price { get; set; }

        public int PriceLangdingId { get; set; }

        [ForeignKey("PriceLangdingId")]
        public virtual PriceLangding PriceLangding { get; set; }

        [Display(Name = "Giá xe 4"), UIHint("TextBox")]
        public string Price4 { get; set; }

        [Display(Name = "Giá xe 7"), UIHint("TextBox")]
        public string Price7 { get; set; }

        [Display(Name = "Giá xe 16"), UIHint("TextBox")]
        public string Price16 { get; set; }

        [Display(Name = "Giá xe 29"), UIHint("TextBox")]
        public string Price29 { get; set; }

        [Display(Name = "Giá xe 45"), UIHint("TextBox")]
        public string Price45 { get; set; }

        [Display(Name = "Giá limousine"), UIHint("TextBox")]
        public string PriceLim { get; set; }

        [Display(Name = "Thứ tự"), UIHint("NumberBox")]
        public int Sort { get; set; }

        [Display(Name = "Nổi bật")]
        public bool Hot { get; set; }

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        /// <summary>Giá hiển thị — ưu tiên cột Price; fallback cột cũ nếu dữ liệu chưa migrate.</summary>
        public string GetDisplayPrice()
        {
            if (!string.IsNullOrWhiteSpace(Price))
            {
                return Price.Trim();
            }

            var legacy = Price4 ?? Price7 ?? Price16 ?? Price29 ?? Price45 ?? PriceLim;
            return string.IsNullOrWhiteSpace(legacy) ? null : legacy.Trim();
        }
    }

}