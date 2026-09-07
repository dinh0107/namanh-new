using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hailinh.Models;

namespace hailinh.ViewModel
{
    public class ListPriceViewModel
    {
        public IEnumerable<CarServicePrice> CarServicePrices { get; set; }
        public string Name { get; set; }
        public CarService CarService { get; set; }
    }
    public class ListDetailViewModel
    {
        public IEnumerable<CarServiceDetail> CarServiceDetails { get; set; }
        public string Name { get; set; }
        public CarService CarService { get; set; }
        public CarServiceType? CarServiceType { get; set; }
    }
    public class PriceViewModel
    {
        public CarService CarService { get; set; }
        public CarServicePrice  CarServicePrice { get; set; }
    }
    public class DetailViewModel
    {
        public CarService CarService { get; set; }
        public CarServiceDetail CarServiceDetail { get; set; }
    }
    public class ListPriceTabViewModel
    {
        public IEnumerable<PriceTable> PriceTables { get; set; }
        public string Name { get; set; }
    }
    public class PriceTabViewModel
    {
        public PriceTable  PriceTable { get; set; }
    }
}