using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace namanh.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Display(Name = "Khoản chi")]
        public Expenditure Expenditure { get; set; }
        [Display(Name = "Số tiền"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public decimal? Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Trạng thái")]
        public StatusExpense Status { get; set; }
        [Display(Name = "Ghi chú"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Note { get; set; }
        [Display(Name = "Người chi"), Required(ErrorMessage = "Hãy chọn Người chi")]
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        public Expense()
        {
            CreateDate = DateTime.Now;
        }
    }

    public enum StatusExpense
    {
        [Display(Name = "Chưa thanh toán")]
        Unpaid,
        [Display(Name = "Đã thanh toán")]
        Paid
    }

    public enum Expenditure
    {
        [Display(Name = "Xăng")]
        Gasoline,
        [Display(Name = "ETC")]
        ETC,
        [Display(Name = "Bảo dưỡng")]
        Maintenance,
        [Display(Name = "Rửa xe")]
        CarWash,
        [Display(Name = "Khác")]
        Other,
    }
}