using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
}