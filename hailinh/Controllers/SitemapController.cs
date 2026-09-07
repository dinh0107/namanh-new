using hailinh.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace hailinh.Controllers
{
    public class SitemapController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [Route("sitemap.xml", Order = 0)]
        public ActionResult Index()
        {
            Response.AddHeader("Content-Type", "text/xml");
            return PartialView();
        }
        #region Sitemap - Article
        [Route("sitemap/articles.xml")]
        public ContentResult ArticleSitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var items = _unitOfWork.ArticleRepository.GetQuery(a => a.Active, q => q.OrderByDescending(a => a.Id)).Select(a => new { a.Url, catUrl = a.ArticleCategory.Url, time = a.CreateDate }).Take(100).ToList();
            var itemSitemap = (from item in items
                               select new XElement(ns + "url", new XElement(ns + "loc", Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ArticleDetail", "Home", new
                               {
                                   url = item.Url
                               })), new XElement(ns + "lastmod", item.time.ToString("yyyy-MM-dd")), new XElement(ns + "changefreq", "daily"), new XElement(ns + "priority", "0.8"))).ToList();
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset", itemSitemap));
            return Content(sitemap.ToString(), "text/xml");
            //sitemap.Save(Server.MapPath("/Sitemap/ArticleSitemap.xml"));
        }
        #endregion
        #region Sitemap - ArticleCategory
        [Route("sitemap/article-categories.xml")]
        public ContentResult ArticleCategorySitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var items = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort)).Select(a => new { a.Url }).ToList();
            var itemSitemap = (from item in items
                               select new XElement(ns + "url", new XElement(ns + "loc", Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ArticleCategory", "Home", new
                               {
                                   url = item.Url
                               })), new XElement(ns + "lastmod", DateTime.Now.ToString("yyyy-MM-dd")), new XElement(ns + "changefreq", "daily"), new XElement(ns + "priority", "0.8"))).ToList();
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset", itemSitemap));
            return Content(sitemap.ToString(), "text/xml");
            //sitemap.Save(Server.MapPath("/Sitemap/ArticleCategorySitemap.xml"));
        }
        #endregion
        [Route("sitemap/service.xml")]
        public ContentResult ServiceSitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var items = _unitOfWork.CarServiceRepository.GetQuery(a => a.Active, q => q.OrderByDescending(a => a.Id)).Select(a => new { url = a.Slug, time = a.CreatedAt }).Take(2000).ToList();
            var itemSitemap = (from item in items
                               select new XElement(ns + "url", new XElement(ns + "loc", Request.Url?.GetLeftPart(UriPartial.Authority) + Url.Action("ServiceCar", "Home", new
                               {
                                   item.url
                               })), new XElement(ns + "lastmod", item.time.ToString("yyyy-MM-dd")), new XElement(ns + "changefreq", "daily"), new XElement(ns + "priority", "0.8"))).ToList();
            var sitemap = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(ns + "urlset", itemSitemap));
            return Content(sitemap.ToString(), "text/xml");
        }
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}