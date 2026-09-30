using Helpers;
using hailinh.DAL;
using hailinh.Models;
using hailinh.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        private List<CarService> GetCachedHomeServices()
        {
            var cache = System.Web.HttpContext.Current?.Cache;
            const string key = "_HomeCarServices";
            if (cache?[key] is List<CarService> cached) return cached;

            var list = _unitOfWork.CarServiceRepository
                .GetQuery(a => a.Active && a.Home, o => o.OrderBy(a => a.Sort))
                .AsNoTracking()
                .ToList();
            cache?.Insert(key, list, null, DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
            return list;
        }

        private List<CarService> GetCachedMenuServices()
        {
            var cache = System.Web.HttpContext.Current?.Cache;
            const string key = "_MenuCarServices";
            if (cache?[key] is List<CarService> cached) return cached;

            var list = _unitOfWork.CarServiceRepository
                .GetQuery(a => a.Active && a.Menu, o => o.OrderBy(a => a.Sort))
                .AsNoTracking()
                .ToList();
            cache?.Insert(key, list, null, DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
            return list;
        }

        private List<CarService> GetCachedAllServices()
        {
            var cache = System.Web.HttpContext.Current?.Cache;
            const string key = "_AllCarServices";
            if (cache?[key] is List<CarService> cached) return cached;

            var list = _unitOfWork.CarServiceRepository
                .GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort))
                .AsNoTracking()
                .ToList();
            cache?.Insert(key, list, null, DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
            return list;
        }

        private List<string> GetCachedFormLocations()
        {
            var cache = System.Web.HttpContext.Current?.Cache;
            const string key = "_FormLocations";
            if (cache?[key] is List<string> cached) return cached;

            var list = _unitOfWork.LocationRepository
                .GetQuery(a => a.Active, o => o.OrderByDescending(a => a.Hot).ThenBy(a => a.Sort))
                .AsNoTracking()
                .Select(a => a.Name)
                .Distinct()
                .Take(8)
                .ToList();
            cache?.Insert(key, list, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            return list;
        }

        public static void ClearHomeCache()
        {
            var cache = System.Web.HttpContext.Current?.Cache;
            if (cache == null) return;
            cache.Remove("_HomeCarServices");
            cache.Remove("_MenuCarServices");
            cache.Remove("_AllCarServices");
            cache.Remove("_FormLocations");
            cache.Remove("_HomeBanners");
            cache.Remove("_HomePriceLangdings");
        }

        private List<Banner> GetCachedBanners()
        {
            var cache = System.Web.HttpContext.Current?.Cache;
            const string key = "_HomeBanners";
            if (cache?[key] is List<Banner> cached) return cached;

            var list = _unitOfWork.BannerRepository
                .GetQuery(a => a.Active, o => o.OrderBy(a => a.Sort))
                .AsNoTracking()
                .ToList();
            cache?.Insert(key, list, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            return list;
        }

        private List<PriceLangding> GetCachedPriceLangdings()
        {
            var cache = System.Web.HttpContext.Current?.Cache;
            const string key = "_HomePriceLangdings";
            if (cache?[key] is List<PriceLangding> cached) return cached;

            var list = _unitOfWork.PriceLangdingRepository
                .GetQuery(a => a.Active, orderBy: a => a.OrderBy(b => b.Sort), includeProperties: "Locations")
                .AsNoTracking()
                .ToList();

            foreach (var langding in list)
            {
                if (langding.Locations != null)
                {
                    langding.Locations = langding.Locations
                        .Where(x => x.Active)
                        .OrderBy(l => l.Sort)
                        .ToList();
                }
            }
            cache?.Insert(key, list, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
            return list;
        }

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
                ArticleCategories = Enumerable.Empty<ArticleCategory>(),
                Services = GetCachedMenuServices()
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 1800)]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                Services = GetCachedAllServices()
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 300, VaryByCustom = "IsAdmin")]
        public ActionResult Index()
        {
            var banner = GetCachedBanners();
            var service = GetCachedHomeServices();
            var langdings = GetCachedPriceLangdings();

            var model = new HomeViewModel
            {
                Banners = banner,
                Services = service,
                Articles = new List<Article>(),
                ArticleCategories = Enumerable.Empty<ArticleCategory>(),
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
        [OutputCache(Duration = 300, VaryByParam = "url", VaryByCustom = "IsAdmin")]
        public ActionResult ServiceCar(string url)
        {
            var carService = _unitOfWork.CarServiceRepository
                .GetQuery(a => a.Slug == url, includeProperties: "Details,CarServicePrices")
                .AsNoTracking()
                .FirstOrDefault();
            if (carService == null)
            {
                return RedirectToActionPermanent("ErrorPage");
            }
            var banner = GetCachedBanners()
                .Where(a => a.Active && a.GroupId == 5)
                .OrderBy(a => a.Sort)
                .ToList();
            var model = new ServiceCarViewModel
            {
                Banners = banner,
                Services = Enumerable.Empty<ArticleCategory>(),
                Articles = Enumerable.Empty<Article>(),
                CarService = carService,
                ArticleCategories = Enumerable.Empty<ArticleCategory>()
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
        [OutputCache(Duration = 1800, VaryByParam = "hideCarType;presetTypeCar")]
        public PartialViewResult Form(bool hideCarType = false, string presetTypeCar = null)
        {
            ViewBag.Services = GetCachedHomeServices();
            ViewBag.Locations = GetCachedFormLocations();
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
            var langdings = GetCachedPriceLangdings();
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

        [Route("khoi-tao-dich-vu-xe")]
        [Route("init-car-services")]
        public ActionResult SeedCarServices()
        {
            try
            {
                var seedList = GetDefaultCarServices();
                int inserted = 0;
                int updated = 0;

                foreach (var item in seedList)
                {
                    var existing = _unitOfWork.CarServiceRepository
                        .GetQuery(a => a.Slug == item.Slug, includeProperties: "Details,CarServicePrices")
                        .FirstOrDefault();

                    if (existing == null)
                    {
                        _unitOfWork.CarServiceRepository.Insert(item);
                        inserted++;
                    }
                    else
                    {
                        existing.Title = item.Title;
                        existing.Car = item.Car;
                        existing.Capacity = item.Capacity;
                        existing.Firm = item.Firm;
                        existing.Type = item.Type;
                        existing.Speed = item.Speed;
                        existing.Slogan = item.Slogan;
                        existing.Description = item.Description;
                        existing.Active = true;
                        existing.Home = true;
                        existing.Menu = true;
                        existing.Sort = item.Sort;
                        existing.TitleMeta = item.TitleMeta;
                        existing.DescriptionMeta = item.DescriptionMeta;
                        if (string.IsNullOrEmpty(existing.Image)) existing.Image = item.Image;
                        if (string.IsNullOrEmpty(existing.ImageUrl)) existing.ImageUrl = item.ImageUrl;

                        if (existing.Details == null || !existing.Details.Any())
                        {
                            existing.Details = item.Details;
                        }

                        if (existing.CarServicePrices == null || !existing.CarServicePrices.Any())
                        {
                            existing.CarServicePrices = item.CarServicePrices;
                        }

                        updated++;
                    }
                }

                _unitOfWork.Save();

                return Json(new
                {
                    status = true,
                    msg = $"Khởi tạo dữ liệu thành công! Thêm mới: {inserted}, Cập nhật: {updated}.",
                    total = seedList.Count,
                    services = seedList.Select(s => new
                    {
                        s.Title,
                        s.Slug,
                        Car = s.Car,
                        s.Capacity,
                        Url = "/dich-vu/" + s.Slug
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = "Lỗi khởi tạo: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private static List<CarService> GetDefaultCarServices()
        {
            return new List<CarService>
            {
                new CarService
                {
                    Title = "Thuê xe 4 - 5 chỗ",
                    Slug = "thue-xe-4-cho",
                    Car = "Toyota Vios, Honda City, Hyundai Accent",
                    Capacity = "4 - 5 chỗ",
                    Firm = "Toyota / Honda / Hyundai",
                    Type = "Sedan 4 - 5 chỗ",
                    Speed = "100 km/h",
                    Slogan = "Tiết kiệm – Nhanh chóng – Tiện lợi",
                    Description = "Dịch vụ cho thuê xe 4 - 5 chỗ có lái chuyên nghiệp, xe đời mới sạch sẽ, điều hòa mát lạnh, tài xế lịch sự. Phù hợp công tác, đi sân bay, về quê, du lịch ngắn ngày.",
                    Image = "seed/hero-vios.webp",
                    ImageUrl = "seed/hero-vios.webp",
                    Active = true,
                    Home = true,
                    Menu = true,
                    Sort = 1,
                    TitleMeta = "Thuê xe 4 - 5 chỗ có lái giá rẻ, xe đời mới | Hà Linh",
                    DescriptionMeta = "Dịch vụ cho thuê xe 4 - 5 chỗ trọn gói đón trả tận nơi, đội xe đời mới sạch sẽ, báo giá nhanh trong 5 phút.",
                    Details = new List<CarServiceDetail>
                    {
                        new CarServiceDetail { Name = "Xe đời mới 2023 - 2024", Desciption = "Sang trọng, sạch thơm, bảo dưỡng định kỳ", CarServiceType = CarServiceType.Highlight, Sort = 1 },
                        new CarServiceDetail { Name = "Tài xế chuyên nghiệp", Desciption = "Đúng giờ, thân thiện, tay lái an toàn", CarServiceType = CarServiceType.Highlight, Sort = 2 },
                        new CarServiceDetail { Name = "Giá trọn gói minh bạch", Desciption = "Cam kết không phát sinh, không phí ẩn", CarServiceType = CarServiceType.Highlight, Sort = 3 },
                        new CarServiceDetail { Name = "Đón trả tận nơi", Desciption = "Đón đúng giờ tại nhà riêng hoặc cơ quan", CarServiceType = CarServiceType.Highlight, Sort = 4 },
                        new CarServiceDetail { Name = "Hỗ trợ 24/7", Desciption = "Tổng đài tư vấn nhiệt tình mọi lúc", CarServiceType = CarServiceType.Highlight, Sort = 5 },
                        new CarServiceDetail { Name = "Bảo hiểm hành khách đầy đủ", Desciption = "An tâm tuyệt đối trên toàn hành trình", CarServiceType = CarServiceType.Highlight, Sort = 6 },
                        new CarServiceDetail { Name = "Đặt xe nhanh 5 phút", Desciption = "Xác nhận lịch trình và nhận xe tức thì", CarServiceType = CarServiceType.Highlight, Sort = 7 },
                        new CarServiceDetail { Name = "Giá thuê xe 4 - 5 chỗ bao gồm những gì?", Body = "<p>Giá đã bao gồm lương lái xe, xăng dầu và chi phí vận hành xe theo lộ trình thỏa thuận. Phí cầu đường (BOT) và VAT nếu cần sẽ được báo cụ thể trước chuyến đi.</p>", CarServiceType = CarServiceType.Policy, Sort = 1 },
                        new CarServiceDetail { Name = "Tôi có thể hủy hoặc đổi chuyến không?", Body = "<p>Quý khách có thể đổi giờ đón hoặc báo hủy chuyến trước giờ xuất phát tối thiểu 2 tiếng hoàn toàn miễn phí.</p>", CarServiceType = CarServiceType.Policy, Sort = 2 },
                        new CarServiceDetail { Name = "Toyota Vios 2024", Desciption = "Sedan quốc dân rộng rãi, tiết kiệm", CarServiceType = CarServiceType.SupportedCar, Sort = 1 },
                        new CarServiceDetail { Name = "Honda City 2024", Desciption = "Hiện đại, thể thao, vận hành êm ái", CarServiceType = CarServiceType.SupportedCar, Sort = 2 }
                    },
                    CarServicePrices = new List<CarServicePrice>
                    {
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Sân bay Nội Bài", Price = "250.000đ", Km = "30km", Hot = true, Sort = 1 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Ninh Bình", Price = "1.100.000đ", Km = "95km", Hot = true, Sort = 2 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Hạ Long", Price = "1.500.000đ", Km = "160km", Hot = true, Sort = 3 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Hải Phòng", Price = "1.300.000đ", Km = "120km", Hot = false, Sort = 4 }
                    }
                },
                new CarService
                {
                    Title = "Thuê xe 7 chỗ",
                    Slug = "thue-xe-7-cho",
                    Car = "Toyota Innova, Fortuner, Mitsubishi Xpander",
                    Capacity = "7 chỗ",
                    Firm = "Toyota / Mitsubishi",
                    Type = "SUV / MPV 7 chỗ",
                    Speed = "100 km/h",
                    Slogan = "Rộng rãi – Êm ái – Sang trọng",
                    Description = "Dịch vụ thuê xe 7 chỗ gia đình rộng rãi, cốp chứa đồ lớn, gầm cao êm ái trên mọi cung đường. Lựa chọn lý tưởng cho du lịch gia đình, về quê, công tác liên tỉnh.",
                    Image = "seed/hero-vios.webp",
                    ImageUrl = "seed/hero-vios.webp",
                    Active = true,
                    Home = true,
                    Menu = true,
                    Sort = 2,
                    TitleMeta = "Thuê xe 7 chỗ có lái giá tốt, xe đời mới | Hà Linh",
                    DescriptionMeta = "Dịch vụ thuê xe 7 chỗ Innova, Fortuner, Xpander phục vụ du lịch gia đình, công tác tỉnh với giá trọn gói cạnh tranh.",
                    Details = new List<CarServiceDetail>
                    {
                        new CarServiceDetail { Name = "Xe đời mới 2023 - 2024", Desciption = "Gầm cao, rộng rãi, điều hòa mát rượi", CarServiceType = CarServiceType.Highlight, Sort = 1 },
                        new CarServiceDetail { Name = "Tài xế chuyên nghiệp", Desciption = "Lịch sự, chu đáo, am hiểu các tuyến đường", CarServiceType = CarServiceType.Highlight, Sort = 2 },
                        new CarServiceDetail { Name = "Giá trọn gói minh bạch", Desciption = "Báo giá nhanh, cam kết không phát sinh", CarServiceType = CarServiceType.Highlight, Sort = 3 },
                        new CarServiceDetail { Name = "Đón trả tận nơi", Desciption = "Phục vụ 24/7 đón tận cửa", CarServiceType = CarServiceType.Highlight, Sort = 4 },
                        new CarServiceDetail { Name = "Hỗ trợ 24/7", Desciption = "Tư vấn và phản hồi trong 5 phút", CarServiceType = CarServiceType.Highlight, Sort = 5 },
                        new CarServiceDetail { Name = "Bảo hiểm hành khách đầy đủ", Desciption = "Đảm bảo quyền lợi khách hàng", CarServiceType = CarServiceType.Highlight, Sort = 6 },
                        new CarServiceDetail { Name = "Đặt xe nhanh 5 phút", Desciption = "Chốt lịch trình đơn giản, nhanh chóng", CarServiceType = CarServiceType.Highlight, Sort = 7 },
                        new CarServiceDetail { Name = "Xe 7 chỗ chở được bao nhiêu hành lý?", Body = "<p>Dòng xe 7 chỗ có hàng ghế thứ 3 gập linh hoạt, thoải mái chở 5-7 người cùng 3-5 vali cỡ trung.</p>", CarServiceType = CarServiceType.Policy, Sort = 1 },
                        new CarServiceDetail { Name = "Có thể thuê xe đi nhiều ngày không?", Body = "<p>Có, Hà Linh cung cấp dịch vụ thuê xe đi tỉnh nhiều ngày theo yêu cầu với chi phí ưu đãi nhất.</p>", CarServiceType = CarServiceType.Policy, Sort = 2 }
                    },
                    CarServicePrices = new List<CarServicePrice>
                    {
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Sân bay Nội Bài", Price = "300.000đ", Km = "30km", Hot = true, Sort = 1 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Ninh Bình", Price = "1.300.000đ", Km = "95km", Hot = true, Sort = 2 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Hạ Long", Price = "1.700.000đ", Km = "160km", Hot = true, Sort = 3 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Sa Pa", Price = "3.200.000đ", Km = "315km", Hot = true, Sort = 4 }
                    }
                },
                new CarService
                {
                    Title = "Thuê xe 16 chỗ",
                    Slug = "thue-xe-16-cho",
                    Car = "Ford Transit, Hyundai Solati",
                    Capacity = "16 chỗ",
                    Firm = "Ford / Hyundai",
                    Type = "Minibus 16 chỗ",
                    Speed = "100 km/h",
                    Slogan = "Đời mới – Thoải mái – An toàn tuyệt đối",
                    Description = "Thuê xe 16 chỗ Ford Transit, Solati khoang xe cao thoáng, điều hòa sâu, ghế ngả êm ái. Thích hợp cho đoàn du lịch công ty, lễ hội, hiếu hỉ, dã ngoại.",
                    Image = "seed/hero-vios.webp",
                    ImageUrl = "seed/hero-vios.webp",
                    Active = true,
                    Home = true,
                    Menu = true,
                    Sort = 3,
                    TitleMeta = "Thuê xe 16 chỗ Ford Transit, Solati giá rẻ | Hà Linh",
                    DescriptionMeta = "Dịch vụ thuê xe 16 chỗ đời mới, lái xe an toàn, phục vụ nhiệt tình, đón trả tận nơi.",
                    Details = new List<CarServiceDetail>
                    {
                        new CarServiceDetail { Name = "Xe đời mới 2023 - 2024", Desciption = "Ghế ngả thoải mái, trần cao thoáng đãng", CarServiceType = CarServiceType.Highlight, Sort = 1 },
                        new CarServiceDetail { Name = "Tài xế chuyên nghiệp", Desciption = "Kinh nghiệm đường trường, tận tâm", CarServiceType = CarServiceType.Highlight, Sort = 2 },
                        new CarServiceDetail { Name = "Giá trọn gói minh bạch", Desciption = "Không phát sinh phụ phí ngoài hợp đồng", CarServiceType = CarServiceType.Highlight, Sort = 3 },
                        new CarServiceDetail { Name = "Đón trả tận nơi", Desciption = "Đón đúng điểm hẹn đúng thời gian", CarServiceType = CarServiceType.Highlight, Sort = 4 },
                        new CarServiceDetail { Name = "Hỗ trợ 24/7", Desciption = "Hỗ trợ đoàn chu đáo suốt hành trình", CarServiceType = CarServiceType.Highlight, Sort = 5 },
                        new CarServiceDetail { Name = "Bảo hiểm hành khách đầy đủ", Desciption = "Đầy đủ bảo hiểm theo quy định", CarServiceType = CarServiceType.Highlight, Sort = 6 },
                        new CarServiceDetail { Name = "Đặt xe nhanh 5 phút", Desciption = "Hợp đồng rõ ràng, xác nhận tức thì", CarServiceType = CarServiceType.Highlight, Sort = 7 }
                    },
                    CarServicePrices = new List<CarServicePrice>
                    {
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Sân bay Nội Bài", Price = "500.000đ", Km = "30km", Hot = true, Sort = 1 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Ninh Bình", Price = "1.800.000đ", Km = "95km", Hot = true, Sort = 2 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Hạ Long", Price = "2.300.000đ", Km = "160km", Hot = true, Sort = 3 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Sầm Sơn", Price = "2.600.000đ", Km = "170km", Hot = true, Sort = 4 }
                    }
                },
                new CarService
                {
                    Title = "Thuê xe 29 chỗ",
                    Slug = "thue-xe-29-cho",
                    Car = "Hyundai County, Thaco Town",
                    Capacity = "29 chỗ",
                    Firm = "Hyundai / Thaco",
                    Type = "Xe khách 29 chỗ",
                    Speed = "90 km/h",
                    Slogan = "Chuyên nghiệp – Tiện nghi – Du lịch tập thể",
                    Description = "Cho thuê xe 29 chỗ đời mới phục vụ đoàn đông, cơ quan, trường học, tour du lịch, khoang hành lý rộng, hệ thống giảm xóc êm ái.",
                    Image = "seed/hero-vios.webp",
                    ImageUrl = "seed/hero-vios.webp",
                    Active = true,
                    Home = true,
                    Menu = true,
                    Sort = 4,
                    TitleMeta = "Thuê xe 29 chỗ đời mới giá tốt | Hà Linh",
                    DescriptionMeta = "Dịch vụ thuê xe 29 chỗ Thaco, County phục vụ cơ quan, trường học, du lịch trọn gói.",
                    Details = new List<CarServiceDetail>
                    {
                        new CarServiceDetail { Name = "Xe đời mới chất lượng", Desciption = "Bảo dưỡng định kỳ, nội thất sạch sẽ", CarServiceType = CarServiceType.Highlight, Sort = 1 },
                        new CarServiceDetail { Name = "Tài xế chuyên tuyến", Desciption = "Lái xe cẩn trọng, phục vụ nhiệt tình", CarServiceType = CarServiceType.Highlight, Sort = 2 },
                        new CarServiceDetail { Name = "Giá trọn gói cạnh tranh", Desciption = "Hóa đơn VAT đầy đủ theo yêu cầu", CarServiceType = CarServiceType.Highlight, Sort = 3 },
                        new CarServiceDetail { Name = "Đón trả tận nơi", Desciption = "Đưa đón theo lịch trình của đoàn", CarServiceType = CarServiceType.Highlight, Sort = 4 },
                        new CarServiceDetail { Name = "Hỗ trợ 24/7", Desciption = "Tư vấn phương án tối ưu chi phí", CarServiceType = CarServiceType.Highlight, Sort = 5 },
                        new CarServiceDetail { Name = "Bảo hiểm hành khách đầy đủ", Desciption = "An tâm tuyệt đối", CarServiceType = CarServiceType.Highlight, Sort = 6 },
                        new CarServiceDetail { Name = "Đặt xe nhanh 5 phút", Desciption = "Thủ tục nhanh gọn", CarServiceType = CarServiceType.Highlight, Sort = 7 }
                    },
                    CarServicePrices = new List<CarServicePrice>
                    {
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Sân bay Nội Bài", Price = "800.000đ", Km = "30km", Hot = true, Sort = 1 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Ninh Bình", Price = "2.500.000đ", Km = "95km", Hot = true, Sort = 2 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Hạ Long", Price = "3.200.000đ", Km = "160km", Hot = true, Sort = 3 }
                    }
                },
                new CarService
                {
                    Title = "Thuê xe 45 chỗ",
                    Slug = "thue-xe-45-cho",
                    Car = "Hyundai Universe, Thaco Blue Sky",
                    Capacity = "45 chỗ",
                    Firm = "Hyundai / Thaco",
                    Type = "Xe du lịch 45 chỗ",
                    Speed = "90 km/h",
                    Slogan = "Hiện đại – Ghế ngả cao cấp – Đầy đủ tiện nghi",
                    Description = "Dòng xe du lịch 45 chỗ Universe sang trọng bậc nhất, bầu hơi êm ái, khoang chứa đồ siêu rộng, tủ lạnh mini, phục vụ tour du lịch quy mô lớn.",
                    Image = "seed/hero-vios.webp",
                    ImageUrl = "seed/hero-vios.webp",
                    Active = true,
                    Home = true,
                    Menu = true,
                    Sort = 5,
                    TitleMeta = "Thuê xe 45 chỗ Universe sang trọng giá rẻ | Hà Linh",
                    DescriptionMeta = "Cho thuê xe 45 chỗ Universe đời mới đưa đón công nhân viên, học sinh, tour du lịch lớn.",
                    Details = new List<CarServiceDetail>
                    {
                        new CarServiceDetail { Name = "Xe Universe đời mới", Desciption = "Bầu hơi êm ái, ghế ngả cao cấp", CarServiceType = CarServiceType.Highlight, Sort = 1 },
                        new CarServiceDetail { Name = "Tài xế chuẩn phong cách", Desciption = "Kinh nghiệm dày dặn trên mọi cung đường", CarServiceType = CarServiceType.Highlight, Sort = 2 },
                        new CarServiceDetail { Name = "Giá tốt cho đoàn đông", Desciption = "Hợp đồng minh bạch, đầy đủ VAT", CarServiceType = CarServiceType.Highlight, Sort = 3 },
                        new CarServiceDetail { Name = "Đón trả tận nơi", Desciption = "Đúng giờ, linh hoạt lộ trình", CarServiceType = CarServiceType.Highlight, Sort = 4 },
                        new CarServiceDetail { Name = "Hỗ trợ 24/7", Desciption = "Hỗ trợ trưởng đoàn chu đáo", CarServiceType = CarServiceType.Highlight, Sort = 5 },
                        new CarServiceDetail { Name = "Bảo hiểm hành khách đầy đủ", Desciption = "An toàn là ưu tiên số 1", CarServiceType = CarServiceType.Highlight, Sort = 6 },
                        new CarServiceDetail { Name = "Đặt xe nhanh 5 phút", Desciption = "Giữ xe ngay sau khi thỏa thuận", CarServiceType = CarServiceType.Highlight, Sort = 7 }
                    },
                    CarServicePrices = new List<CarServicePrice>
                    {
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Sân bay Nội Bài", Price = "1.200.000đ", Km = "30km", Hot = true, Sort = 1 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Ninh Bình", Price = "3.500.000đ", Km = "95km", Hot = true, Sort = 2 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Hạ Long", Price = "4.500.000đ", Km = "160km", Hot = true, Sort = 3 }
                    }
                },
                new CarService
                {
                    Title = "Thuê xe Limousine VIP",
                    Slug = "thue-xe-limousine",
                    Car = "Dcar Limousine 9 - 11 chỗ",
                    Capacity = "Limousine 9 - 11 chỗ",
                    Firm = "Dcar Limousine",
                    Type = "Limousine thương gia",
                    Speed = "100 km/h",
                    Slogan = "Đẳng cấp thương gia – Tiện nghi vượt trội",
                    Description = "Trải nghiệm dòng xe Limousine hạng thương gia với ghế massage, cổng sạc từng vị trí, wifi tốc độ cao, phục vụ chuyên gia, đối tác và khách VIP.",
                    Image = "seed/hero-vios.webp",
                    ImageUrl = "seed/hero-vios.webp",
                    Active = true,
                    Home = true,
                    Menu = true,
                    Sort = 6,
                    TitleMeta = "Thuê xe Limousine VIP 9 - 11 chỗ giá tốt | Hà Linh",
                    DescriptionMeta = "Dịch vụ thuê xe Dcar Limousine thương gia sang trọng, đẳng cấp, phục vụ VIP 24/7.",
                    Details = new List<CarServiceDetail>
                    {
                        new CarServiceDetail { Name = "Ghế da massage VIP", Desciption = "Êm ái, cổng sạc USB từng vị trí, Wifi tốc độ cao", CarServiceType = CarServiceType.Highlight, Sort = 1 },
                        new CarServiceDetail { Name = "Tài xế tác phong chuyên nghiệp", Desciption = "Lịch thiệp, chu đáo, bảo mật thông tin", CarServiceType = CarServiceType.Highlight, Sort = 2 },
                        new CarServiceDetail { Name = "Giá chuẩn thương gia", Desciption = "Trọn gói, minh bạch 100%", CarServiceType = CarServiceType.Highlight, Sort = 3 },
                        new CarServiceDetail { Name = "Đón trả tận nơi", Desciption = "Đưa đón VIP chuẩn giờ", CarServiceType = CarServiceType.Highlight, Sort = 4 },
                        new CarServiceDetail { Name = "Hỗ trợ 24/7", Desciption = "Tư vấn riêng theo yêu cầu", CarServiceType = CarServiceType.Highlight, Sort = 5 },
                        new CarServiceDetail { Name = "Bảo hiểm hành khách đầy đủ", Desciption = "Chất lượng dịch vụ chuẩn 5 sao", CarServiceType = CarServiceType.Highlight, Sort = 6 },
                        new CarServiceDetail { Name = "Đặt xe nhanh 5 phút", Desciption = "Xác nhận lịch trình tức thì", CarServiceType = CarServiceType.Highlight, Sort = 7 }
                    },
                    CarServicePrices = new List<CarServicePrice>
                    {
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Sân bay Nội Bài", Price = "600.000đ", Km = "30km", Hot = true, Sort = 1 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Ninh Bình", Price = "2.200.000đ", Km = "95km", Hot = true, Sort = 2 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Hạ Long", Price = "2.800.000đ", Km = "160km", Hot = true, Sort = 3 },
                        new CarServicePrice { RouteDescription = "Hà Nội ↔ Hải Phòng", Price = "2.500.000đ", Km = "120km", Hot = true, Sort = 4 }
                    }
                }
            };
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}