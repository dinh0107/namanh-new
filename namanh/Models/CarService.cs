using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace namanh.Models
{
    public class CarService
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slogan { get; set; }
        [Display(Name = "Loại xe")]
        public string Car { get; set; }
        [UIHint("EditorBox")]
        public string Slug { get; set; }
        [UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name ="Ảnh banner")]
        public string ImageUrl { get; set; }
        [Display(Name = "Ảnh banner mobile")]
        public string ImageUrlMobile { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string Image { get; set; }
        //Option
        [Display(Name = "Hãng xe")]
        public string Firm { get; set; }

        [Display(Name = "Dung tích")]
        public string Capacity { get; set; }

        [Display(Name = "Loại")]
        public string Type { get; set; }

        [Display(Name = "Vận tốc")]
        public string Speed { get; set; }
        //End
        public string ListImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Trang chủ")]
        public bool Home { get; set; }
        [Display(Name = "Menu")]
        public bool Menu { get; set; }

        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên"), UIHint("NumberBox")]
        public int Sort { get; set; }
        public virtual ICollection<CarServiceDetail> Details { get; set; }
        public virtual ICollection<CarServicePrice>  CarServicePrices { get; set; }
    }

    public class CarServiceDetail
    {
        public int Id { get; set; }
        public int CarServiceId { get; set; }
        public string Image { get; set; }
        [Display(Name = "Danh sách ảnh")]
        public string ListImage { get; set; }
        [Required]
        [Display(Name = "Tên xe"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Mô tả"), UIHint("TextArea")]
        public string Desciption { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Loại")]
        public CarServiceType CarServiceType { get; set; }
        public virtual CarService CarService { get; set; }
    }
    public class CarServicePrice
    {
        public int Id { get; set; }
        public int CarServiceId { get; set; }
        [Required]
        [Display(Name = "Lộ trình"), UIHint("TextBox")]
        public string RouteDescription { get; set; }
        [Required]
        [Display(Name = "Giá"), UIHint("TextBox")]
        public string Price { get; set; }
        [Display(Name = "Km"), UIHint("TextBox")]
        public string Km { get; set; }
        public bool Hot { get; set; }
        [Display(Name = "Thứ tự"), Required(ErrorMessage = "Hãy nhập số thứ tự"), RegularExpression(@"\d+", ErrorMessage = "Chỉ nhập số nguyên dương"), UIHint("NumberBox")]
        public int Sort { get; set; }
        public virtual CarService CarService { get; set; }
    }
    public enum CarServiceType
    {
        [Display(Name = "Ưu điểm")]
        Highlight,
        [Display(Name = "Dòng xe")]
        SupportedCar,
        [Display(Name = "Quy trình")]
        ProcessStep,
        [Display(Name = "Câu hỏi")]
        Policy
    }
}