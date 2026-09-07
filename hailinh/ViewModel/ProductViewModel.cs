using hailinh.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace hailinh.ViewModel
{
    public class InsertProductCatViewModel
    {
        [Display(Name = "Danh mục cha")]
        public int? ParentId { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }

    public class ListProductViewModel
    {
        public int? RootId { get; set; }
        public int? ChildId { get; set; }
        public string Name { get; set; }
        public string Sort { get; set; }
        public PagedList.IPagedList<Product> Products { get; set; }
        public SelectList SelectCategories { get; set; }
        public SelectList ChildCategoryList { get; set; }

        public ListProductViewModel()
        {
            ChildCategoryList = new SelectList(new List<ProductCategory>(), "Id", "CategoryName");
        }
    }

    public class InsertProductViewModel
    {
        public int CategoryId { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Giá niêm yết"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string Price { get; set; }
        [Display(Name = "Giá khuyến mãi"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string PriceSale { get; set; }
    }
}
