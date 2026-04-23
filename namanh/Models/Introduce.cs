using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace namanh.Models
{
    public class Introduce
    {
        public int Id { get; set; }
        [Display(Name = "Ảnh giới thiệu"), StringLength(500)]
        public string Image { get; set; }
        [Display(Name = "Giới thiệu"), UIHint("EditorBox")]
        public string AboutText { get; set; }
        [Display(Name = "Đội ngũ nhân sự"), UIHint("EditorBox")]
        public string StaffText { get; set; }
        [Display(Name = "Khách hàng phản hồi"), UIHint("EditorBox")]
        public string FeedbackText { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }
    }
}