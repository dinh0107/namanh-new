using Helpers;
using hailinh.DAL;
using hailinh.Models;
using hailinh.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace hailinh.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];

        private IEnumerable<ArticleCategory> ArticleCategories()
        {
            if (System.Web.HttpContext.Current?.Items["_ArticleCategoriesCache"] is IEnumerable<ArticleCategory> cached)
            {
                return cached;
            }
            var list = _unitOfWork.ArticleCategoryRepository
                .Get(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort))
                .ToList();
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Items["_ArticleCategoriesCache"] = list;
            }
            return list;
        }
        [ChildActionOnly]
        [OutputCache(Duration = 1800)]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ArticleCategories = ArticleCategories().Where(a => a.ShowMenu),
                Services = _unitOfWork.CarServiceRepository.GetQuery(a => a.Active && a.Menu, o => o.OrderBy(a => a.Sort)).ToList(),
                Banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active && a.GroupId == 1 && a.Image != null).FirstOrDefault()
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 1800)]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                ArticleCategories = ArticleCategories().Where(a => a.ShowFooter)
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 300, VaryByCustom = "IsAdmin")]
        public ActionResult Index()
        {
            var banner = _unitOfWork.BannerRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort)).ToList();
            var service = _unitOfWork.CarServiceRepository.GetQuery(a => a.Active && a.Home, o => o.OrderBy(a => a.Sort)).ToList();
            var articles = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && (a.ArticleCategory.TypePost == TypePost.Article && a.Home && !a.Draft), o => o.OrderByDescending(a => a.CreateDate)).Take(6).ToList();
            
            var langdings = _unitOfWork.PriceLangdingRepository
                .GetQuery(a => a.Active, orderBy: a => a.OrderBy(b => b.Sort), includeProperties: "Locations")
                .ToList();

            foreach (var langding in langdings)
            {
                if (langding.Locations != null)
                {
                    langding.Locations = langding.Locations
                        .Where(x => x.Active)
                        .OrderBy(l => l.Sort)
                        .ToList();
                }
            }

            var model = new HomeViewModel
            {
                Banners = banner,
                Services = service,
                Articles = articles,
                ArticleCategories = ArticleCategories().Where(a => a.TypePost == TypePost.Article && a.Home),
                PriceLangdings = langdings
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
            var carService = _unitOfWork.CarServiceRepository
                .GetQuery(a => a.Slug == url, includeProperties: "Details,CarServicePrices")
                .FirstOrDefault();
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
        [ChildActionOnly]
        [OutputCache(Duration = 1800)]
        public PartialViewResult Form(bool hideCarType = false, string presetTypeCar = null)
        {
            var services = _unitOfWork.CarServiceRepository.GetQuery(a => a.Active && a.Home, o => o.OrderBy(a => a.Sort)).ToList();
            var locations = _unitOfWork.LocationRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.Hot).ThenBy(a => a.Sort)).Select(a => a.Name).Distinct().Take(8).ToList();
            ViewBag.Services = services;
            ViewBag.Locations = locations;
            ViewBag.HideCarType = hideCarType;
            ViewBag.PresetTypeCar = presetTypeCar;
            return PartialView();
        }

        public PartialViewResult FormLanding(string typeCar = "", bool serviceLayout = false)
        {
            ViewBag.TypeCar = typeCar;
            ViewBag.ServiceLayout = serviceLayout;
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
                await hailinh.Utils.EmailService.SendBookingNotificationAsync(model, ConfigSite, websiteUrl);
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

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}