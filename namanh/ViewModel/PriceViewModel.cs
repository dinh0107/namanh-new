using namanh.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace namanh.ViewModel
{
    public class ListViewModel
    {
        public PagedList.IPagedList<PriceLangding> Price { get; set; }
        public string Name { get; set; }
    }

    public class InsertPriceViewModel
    {
        public IEnumerable<Location> Locations { get; set; }
    }
}