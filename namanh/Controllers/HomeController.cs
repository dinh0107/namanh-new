using Helpers;
using namanh.DAL;
using namanh.Models;
using namanh.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Razor;

namespace namanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];

        private IEnumerable<ArticleCategory> ArticleCategories() =>
            _unitOfWork.ArticleCategoryRepository.Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));
        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ArticleCategories = ArticleCategories().Where(a => a.ShowMenu),
                Services = _unitOfWork.CarServiceRepository.GetQuery(a => a.Active && a.Menu, o => o.OrderBy(a => a.Sort)),
                Banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active && a.GroupId == 1 && a.Image != null).FirstOrDefault()
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                ArticleCategories = ArticleCategories().Where(a => a.ShowFooter)
            };
            return PartialView(model);
        }
        public ActionResult Index()
        {
            var banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort));
            var service = _unitOfWork.CarServiceRepository.GetQuery(a => a.Active && a.Home, o => o.OrderBy(a => a.Sort));
            var articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && (a.ArticleCategory.TypePost == TypePost.Article && a.Home && !a.Draft), o => o.OrderByDescending(a => a.CreateDate));
            
            var langdings = _unitOfWork.PriceLangdingRepository
                .GetQuery(a => a.Active, orderBy: a => a.OrderBy(b => b.Sort))
                .ToList();

            foreach (var langding in langdings)
            {
                langding.Locations = _unitOfWork.LocationRepository
                    .Get(x => x.PriceLangdingId == langding.Id && x.Active, orderBy: q => q.OrderBy(l => l.Sort))
                    .ToList();
            }

            var model = new HomeViewModel
            {
                Banners = banner,
                Services = service,
                Articles = articles.Take(6),
                ArticleCategories = ArticleCategories().Where(a => a.TypePost == TypePost.Article && a.Home),
                PriceLangdings = langdings,
                PriceTables = _unitOfWork.PriceTableRepository.GetQuery(orderBy: a => a.OrderBy(b => b.Sort))
            };
            return View(model);
        }

        public PartialViewResult GetArticle(int id)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && a.Id == id).FirstOrDefault();
            var articles = _unitOfWork.ArticleRepository.GetQuery(
                a => a.Active && !a.Draft && (a.ArticleCategoryId == category.Id || a.ArticleCategory.ParentId == category.Id),
                q => q.OrderByDescending(a => a.CreateDate)).Take(6);

            return PartialView(articles);
        }
        [Route("dich-vu/{url}")]
        public ActionResult ServiceCar(string url)
        {
            var carService = _unitOfWork.CarServiceRepository.GetQuery(a => a.Slug == url).FirstOrDefault();
            if (carService == null)
            {
                return RedirectToActionPermanent("ErrorPage");
            }
            var banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort));
            var service = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && (a.TypePost == TypePost.Service && a.Home), o => o.OrderBy(a => a.CategorySort));
            var articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && (a.ArticleCategory.TypePost == TypePost.Article && a.Home && !a.Draft), o => o.OrderByDescending(a => a.CreateDate));
            var model = new ServiceCarViewModel
            {
                Banners = banner,
                Services = service,
                Articles = articles.Take(6),
                CarService = carService,
                ArticleCategories = ArticleCategories().Where(a => a.TypePost == TypePost.Article && a.Home)
            };
            return View(model);
        }
        [Route("news/{url}", Order = 0)]
        public ActionResult ArticleCategory(string url, int? page)
        {
            var category = _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive && a.Url == url).FirstOrDefault();
            if (category == null)
            {
                return RedirectToActionPermanent("ErrorPage");
            }

            var articles = _unitOfWork.ArticleRepository.GetQuery(
                a => a.Active && !a.Draft && (a.ArticleCategoryId == category.Id || a.ArticleCategory.ParentId == category.Id),
                q => q.OrderByDescending(a => a.CreateDate));
            var pageNumber = page ?? 1;

            if (articles.Count() == 1)
            {
                var fi = articles.First();
                return RedirectToAction("ArticleDetail", new { url = fi.Url });
            }
            var model = new ArticleCategoryViewModel
            {
                Category = category,
                Articles = articles.ToPagedList(pageNumber, 11),
                Categories = ArticleCategories(),
            };

            if (category.ParentId != null)
            {
                model.RootCategory = _unitOfWork.ArticleCategoryRepository.GetById(category.ParentId);
            }
            return View(model);
        }
        [Route("{url}.html")]
        public ActionResult ArticleDetail(string url, string view = "")
        {
            var article = _unitOfWork.ArticleRepository.GetQuery(a => a.Url == url && !a.Draft).FirstOrDefault();
            if (article == null)
            {
                return RedirectToActionPermanent("ErrorPage");
            }
            if (view == "")
            {
                article.View++;
                _unitOfWork.ArticleRepository.Update(article);
                _unitOfWork.Save();
            }

            var model = new ArticleViewModel
            {
                Article = article,
                Articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && !a.Draft && (a.ArticleCategoryId == article.ArticleCategoryId && a.Id != article.Id)).OrderByDescending(a => a.CreateDate).Take(6)
            };
            if (article.ArticleCategory.ParentId != null)
            {
                model.RootCategory = _unitOfWork.ArticleCategoryRepository.GetById(article.ArticleCategory.ParentId);
            }
            return View(model);
        }
        public PartialViewResult ArticleHot()
        {
            var articles = _unitOfWork.ArticleRepository
                .GetQuery(a => a.Active && !a.Draft,
                          o => o.OrderByDescending(a => a.CreateDate))
                .ToList();

            var model = new NavArticleViewModel
            {
                Articles = articles
            };

            return PartialView(model);

        }
        public PartialViewResult Form()
        {
            var services = _unitOfWork.CarServiceRepository.GetQuery(a => a.Active && a.Home, o => o.OrderBy(a => a.Sort)).ToList();
            var locations = _unitOfWork.LocationRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.Hot).ThenBy(a => a.Sort)).Select(a => a.Name).Distinct().Take(8).ToList();
            ViewBag.Services = services;
            ViewBag.Locations = locations;
            return PartialView();
        }

        public PartialViewResult FormLanding(string typeCar = "")
        {
            ViewBag.TypeCar = typeCar;
            return PartialView();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ContactForm(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }

            if (model.ToDate != null)
            {
                model.ToDate = Convert.ToDateTime(model.ToDate);
            }

            using (var uow = new UnitOfWork())
            {
                uow.ContactRepository.Insert(model);
                uow.Save();
            }

            var websiteUrl = Request?.Url?.GetLeftPart(UriPartial.Authority) ?? "http://localhost:5000";
            Task.Run(async () =>
            {
                await namanh.Utils.EmailService.SendBookingNotificationAsync(model, ConfigSite, websiteUrl);
            });

            return Json(new { status = true, msg = "Gửi yêu cầu thành công!\nChúng tôi sẽ liên hệ báo giá tới bạn trong 3 - 5 phút." });
        }
        public ActionResult About()
        {
            return View();
        }

        [Route("lien-he")]
        public ActionResult Contact()
        {
            return View();
        }
        [Route("404")]
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
        [Route("gioi-thieu")]
        public ActionResult Introduct()
        {
            var banners = _unitOfWork.BannerRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort));
            var model = new IntroduceViewModel
            {
                Introduce = _unitOfWork.IntroduceRepository.GetQuery().FirstOrDefault(),
                Banners = banners.Where(a => a.GroupId == 6).Take(3),
                Banner = banners.Where(a => a.GroupId == 7 && a.Image != null).FirstOrDefault()
            };
            return View(model);
        }

        public PartialViewResult PriceTable()
        {
            var langdings = _unitOfWork.PriceLangdingRepository
                .GetQuery(a => a.Active, orderBy: a => a.OrderBy(b => b.Sort))
                .ToList();

            foreach (var langding in langdings)
            {
                langding.Locations = _unitOfWork.LocationRepository
                    .Get(x => x.PriceLangdingId == langding.Id && x.Active, orderBy: q => q.OrderBy(l => l.Sort))
                    .ToList();
            }

            var model = new HomeViewModel
            {
                PriceLangdings = langdings
            };
            return PartialView(model);
        }

        public JsonResult GetRoutePrices(int langdingId, string carType = null)
        {
            var locations = _unitOfWork.LocationRepository
                .Get(x => x.PriceLangdingId == langdingId && x.Active, orderBy: q => q.OrderBy(l => l.Sort))
                .ToList();

            var langding = _unitOfWork.PriceLangdingRepository.GetById(langdingId);
            var fromCity = langding?.Name ?? "";

            var result = locations.Select(x => new
            {
                id = x.Id,
                from = fromCity,
                to = x.Name,
                hot = x.Hot,
                price = GetLocationPrice(x)
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private static string GetLocationPrice(Location location)
        {
            var price = location.GetDisplayPrice();
            return string.IsNullOrWhiteSpace(price) ? "Liên hệ" : price;
        }

        public ActionResult ErrorPage()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
        public JsonResult GetPrices()
        {
            var prices = _unitOfWork.PriceTableRepository
                .GetQuery(orderBy: a => a.OrderBy(o => o.Sort))
                .ToList();
            var result = prices.Select(x => new
            {
                hanh_trinh = $"{x.RouteDescription}",
                hot = x.Hot ? true : (bool?)null,
                gia = new
                {
                    _4_cho = string.IsNullOrEmpty(x.Price4) ? "Liên hệ" : x.Price4,
                    _7_cho = string.IsNullOrEmpty(x.Price7) ? "Liên hệ" : x.Price7,
                    _16_cho = string.IsNullOrEmpty(x.Price16) ? "Liên hệ" : x.Price16,
                    _29_cho = string.IsNullOrEmpty(x.Price29) ? "Liên hệ" : x.Price29,
                    _45_cho = string.IsNullOrEmpty(x.Price45) ? "Liên hệ" : x.Price45,
                    limousine = string.IsNullOrEmpty(x.PriceLim) ? "Liên hệ" : x.PriceLim
                }
            }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult ContactForm()
        {
            return PartialView();
        }


        //[HttpPost]
        //public ActionResult ImportFromJson()
        //{
        //    try
        //    {
        //        string filePath = Server.MapPath("~/App_Start/prices.json");

        //        if (!System.IO.File.Exists(filePath))
        //            return Json(new { success = false, message = "Không tìm thấy file prices.json" });

        //        string json = System.IO.File.ReadAllText(filePath);

        //        var routes = JsonConvert.DeserializeObject<List<dynamic>>(json);
        //        var list = new List<PriceTable>();
        //        int sort = 1;

        //        foreach (var r in routes)
        //        {
        //            var hanhTrinh = (string)r.hanh_trinh;
        //            bool hot = r.hot != null && (bool)r.hot;
        //            var gia = r.gia;

        //            var item = new PriceTable
        //            {
        //                CarServiceId = 1, 
        //                RouteDescription = hanhTrinh.Replace("<i class='fa-thin fa-arrows-left-right'></i>", "↔"),
        //                Price4 = gia["4_cho"]?.ToString() ?? "Liên hệ",
        //                Price7 = gia["7_cho"]?.ToString() ?? "Liên hệ",
        //                Price16 = gia["16_cho"]?.ToString() ?? "Liên hệ",
        //                Price29 = gia["29_cho"]?.ToString() ?? "Liên hệ",
        //                Price45 = gia["45_cho"]?.ToString() ?? "Liên hệ",
        //                PriceLim = gia["limousine"]?.ToString() ?? "Liên hệ",
        //                Hot = hot,
        //                Sort = sort++
        //            };

        //            list.Add(item);
        //        }

        //        foreach (var item in list)
        //        {
        //            _unitOfWork.PriceTableRepository.Insert(item);
        //        }

        //        _unitOfWork.Save();

        //        return Json(new { success = true, message = $"Đã import {list.Count} dòng dữ liệu thành công!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}