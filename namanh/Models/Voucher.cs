using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace namanh.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        [Display(Name = "Mã Voucher"), Required, StringLength(10, ErrorMessage = "Tối đa 10 ký tự"), UIHint("TextBox")]
        public string Code { get; set; }
        [StringLength(500)]
        public string QRCode { get; set; }
        public bool Active { get; set; }
        [Display(Name = "Trạng thái")]
        public Status Status { get; set; }
        [Display(Name = "Số lần sử dụng")]
        public int? CountUsed { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Khách hàng")]
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public Voucher()
        {
            CreateDate = DateTime.Now;
        }
    }

    public enum Status
    {
        [Display(Name = "Chưa dùng")]
        NotUsed,
        [Display(Name = "Đã dùng")]
        Used
    }
}