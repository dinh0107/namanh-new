using System;
using System.ComponentModel.DataAnnotations;

namespace namanh.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Display(Name = "Điểm đi"), Required(ErrorMessage = "Điểm đi không được bỏ trống"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string From { get; set; }
        [Display(Name = "Điểm đến"), Required(ErrorMessage = "Điểm đến không được bỏ trống"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string To { get; set; }
        [Display(Name = "Ngày đi")]
        public DateTime FromDate { get; set; }
        [Display(Name = "Ngày về")]
        public DateTime? ToDate { get; set; }
        [Display(Name = "Số điện thoại"), RegularExpression(@"^\(?(09|03|07|08|05)\)?[-. ]?([0-9]{8})$", ErrorMessage = "Số điện thoại không đúng định dạng!"), Required(ErrorMessage = "Số điện thoại không được bỏ trống"), StringLength(10, ErrorMessage = "Tối đa 10 ký tự"), UIHint("TextBox")]
        public string Mobile { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Tình trạng")]
        public StatusContact StatusContact { get; set; }
        [Display(Name = "Loại xe"), StringLength(10)]
        public string TypeCar { get; set; }

        public Contact()
        {
            CreateDate = DateTime.Now;
        }
    }

    public enum StatusContact
    {
        [Display(Name = "Tiếp nhận")]
        Reception,
        [Display(Name = "Liên hệ")]
        Contact,
        [Display(Name = "Chốt")]
        Latch,
        [Display(Name = "Hủy")]
        Fail
    }
}