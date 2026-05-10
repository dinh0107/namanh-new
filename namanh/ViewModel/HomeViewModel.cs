using namanh.Models;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace namanh.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<PriceTable>  PriceTables { get; set; }
        public IEnumerable<PriceLangding>  PriceLangdings { get; set; }
        public IEnumerable<CarService> Services { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
    }

    public class ServiceCarViewModel
    {
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Services { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public CarService  CarService { get; set; }
    }

    public class HeaderViewModel
    {
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<CarService> Services { get; set; }
        public Banner Banner { get; set; }
    }

    public class FooterViewModel
    {
        public Contact Contact { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }

    public class AllArticleViewModel
    {
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
    }

    public class ArticleCategoryViewModel
    {
        public ArticleCategory RootCategory { get; set; }
        public ArticleCategory Category { get; set; }
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
    }
    public class NavArticleViewModel
    {
        public IEnumerable<Article> Articles { get; set; }

    }
    public class ArticleViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public ArticleCategory RootCategory { get; set; }
    }
    public class ArticleDetailViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<Article> PostNews { get; set; }
    }

    public class ArticleSearchViewModel
    {
        public string Keywords { get; set; }
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
    }

    public class MenuArticleViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
    }

    public class ProductDetailViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
        public Banner Banner { get; set; }
    }

    public class CategoryProductViewModel
    {
        public ProductCategory Category { get; set; }
        public IPagedList<Product> Products { get; set; }
        public string Url { get; set; }
        public int BeginCount { get; set; }
        public int EndCount { get; set; }
    }

    public class ProductSearchViewModel
    {
        public string Keywords { get; set; }
        public IPagedList<Product> Products { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }

    public class GetProductViewModel
    {
        public string Keywords { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

    public class IntroduceViewModel
    {
        public Introduce Introduce { get; set; }
        public Banner Banner { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
    }
}
