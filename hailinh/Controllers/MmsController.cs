using hailinh.DAL;
using hailinh.Models;
using hailinh.ViewModel;
using Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using System.Globalization;
using PagedList;

namespace hailinh.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class MmsController : Controller
    {
        public readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region Login
        [Route("dang-nhap")]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [Route("dang-nhap")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(AdminLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.Get(a => a.Username == model.Username && a.Active).SingleOrDefault();

                if (admin != null && HtmlHelpers.VerifyHash(model.Password, "SHA256", admin.Password))
                {
                    var ticket = new FormsAuthenticationTicket(1, model.Username.ToLower(), DateTime.Now, DateTime.Now.AddDays(30), true,
                       FormsAuthentication.FormsCookiePath);

                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Mms");
                }
                ModelState.AddModelError("", @"Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            return View(model);
        }
        [Route("dang-xuat")]
        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Mms");
        }
        #endregion
        [Route("")]
        public ActionResult Index()
        {
            var model = new InfoAdminViewModel
            {
                Admins = _unitOfWork.AdminRepository.GetQuery().Count(),
                Articles = _unitOfWork.ArticleRepository.GetQuery().Count(),
                Contacts = _unitOfWork.ContactRepository.GetQuery().Count(),
                Banners = _unitOfWork.BannerRepository.GetQuery().Count(),
                Customers = _unitOfWork.CustomerRepository.GetQuery().Count(),
                Trips = _unitOfWork.TripRepository.GetQuery(a => a.TypeTrip == TypeTrip.Drive).Count(),
                TripChanges = _unitOfWork.TripRepository.GetQuery(a => a.TypeTrip == TypeTrip.Change).Count(),
                Drivers = _unitOfWork.DiverRepository.GetQuery().Count(),
                Expenses = _unitOfWork.ExpenseRepository.GetQuery().Count(),
            };
            return View(model);
        }

        #region Admin
        [ChildActionOnly]
        public PartialViewResult ListAdmin()
        {
            var admins = _unitOfWork.AdminRepository.Get();
            return PartialView("ListAdmin", admins);
        }
        [Route("them-tai-khoan")]
        [AllowAnonymous]
        public ActionResult CreateAdmin(string result = "")
        {
            ViewBag.Result = result;
            return View();
        }
        [AllowAnonymous]
        [Route("them-tai-khoan")]
        [HttpPost]
        public ActionResult CreateAdmin(Admin model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                if (admin != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                }
                else
                {
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.AdminRepository.Insert(new Admin { Username = model.Username, Password = hashPass ,Active = model.Active });
                    _unitOfWork.Save();
                    return RedirectToAction("CreateAdmin", new { result = "success" });
                }
            }
            return View();
        }
        [Route("sua-tai-khoan")]
        public ActionResult EditAdmin(int adminId = 0)
        {
            var admin = _unitOfWork.AdminRepository.GetById(adminId);
            if (admin == null)
            {
                return RedirectToAction("CreateAdmin");
            }

            var model = new EditAdminViewModel
            {
                Id = admin.Id,
                Username = admin.Username,
                Active = admin.Active,
            };

            return View(model);
        }
        [Route("sua-tai-khoan")]
        [HttpPost]
        public ActionResult EditAdmin(EditAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetById(model.Id);
                if (admin == null)
                {
                    return RedirectToAction("CreateAdmin");
                }
                if (admin.Username != model.Username)
                {
                    var exists = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(model.Username)).SingleOrDefault();
                    if (exists != null)
                    {
                        ModelState.AddModelError("", @"Tên đăng nhập này có rồi");
                        return View(model);
                    }
                    admin.Username = model.Username;
                }
                admin.Active = model.Active;
                if (model.Password != null)
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                }
                _unitOfWork.Save();
                return RedirectToAction("CreateAdmin", new { result = "update" });
            }
            return View(model);
        }
        public bool DeleteAdmin(string username)
        {
            var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(username)).SingleOrDefault();
            if (admin == null)
            {
                return false;
            }
            if (username == "admin")
            {
                return false;
            }
            _unitOfWork.AdminRepository.Delete(admin);
            _unitOfWork.Save();
            return true;
        }
        [Route("doi-mat-khau")]
        public ActionResult ChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [Route("doi-mat-khau")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _unitOfWork.AdminRepository.GetQuery(a => a.Username.Equals(User.Identity.Name,
                StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (admin == null)
                {
                    return HttpNotFound();
                }
                if (HtmlHelpers.VerifyHash(model.OldPassword, "SHA256", admin.Password))
                {
                    admin.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.Save();
                    return RedirectToAction("ChangePassword", new { result = 1 });
                }
                else
                {
                    ModelState.AddModelError("", @"Mật khẩu hiện tại không đúng!");
                    return View();
                }
            }
            return View(model);
        }
        #endregion

        #region ConfigSite
        [Route("thong-tin-chung")]
        public ActionResult ConfigSite(string result = "")
        {
            ViewBag.Result = result;
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            return View(config);
        }
        [Route("thong-tin-chung")]
        [HttpPost, ValidateInput(false)]
        public ActionResult ConfigSite(ConfigSite model, FormCollection fc)
        {
            var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
            if (config == null)
            {
                _unitOfWork.ConfigSiteRepository.Insert(model);
            }
            else
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/configs/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1200, 1200, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Image")
                    {
                        config.Image = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "Favicon")
                    {
                        config.Favicon = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "AboutImage")
                    {
                        config.AboutImage = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "ImageShare")
                    {
                        config.ImageShare = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "AboutMission")
                    {
                        config.AboutMission = imgFile;
                    }
                    else if (Request.Files.Keys[i] == "FooterImage")
                    {
                        config.FooterImage = imgFile;
                    }
                }

                config.Facebook = model.Facebook;
                config.GoogleMap = model.GoogleMap;
                config.Youtube = model.Youtube;
                config.Instagram = model.Instagram;
                config.Title = model.Title;
                config.Description = model.Description;
                config.GoogleAnalytics = model.GoogleAnalytics;
                config.Hotline = model.Hotline;
                //config.Hotline2 = model.Hotline2;
                config.Email = model.Email;
                config.Messenger = model.Messenger;
                config.Place = model.Place;
                config.AboutText = model.AboutText;
                config.InfoFooter = model.InfoFooter;
                config.InfoContact = model.InfoContact;
                config.AboutUrl = model.AboutUrl;
                config.Price = model.Price;
                config.HomeFleetTitle = model.HomeFleetTitle;
                config.HomeFleetDesc = model.HomeFleetDesc;
                config.HomePriceTitle = model.HomePriceTitle;
                config.HomePriceDesc = model.HomePriceDesc;
                config.HomePriceNoteTitle = model.HomePriceNoteTitle;
                config.HomeCtaTitle = model.HomeCtaTitle;
                config.HomeCtaDesc = model.HomeCtaDesc;
                config.HomeReviewTitle = model.HomeReviewTitle;
                config.HomeNewsTitle = model.HomeNewsTitle;
                config.HomeNewsDesc = model.HomeNewsDesc;
                config.HomeFaqTitle = model.HomeFaqTitle;
                config.HomeFaqCtaTitle = model.HomeFaqCtaTitle;
                config.HomeFaqCtaDesc = model.HomeFaqCtaDesc;

                if (model.Zalo != null)
                {
                    config.Zalo = model.Zalo.Replace(" ", string.Empty);
                }

                _unitOfWork.Save();
                HttpContext.Application["ConfigSite"] = config;
                return RedirectToAction("ConfigSite", "Mms", new { result = "success" });
            }
            return View("ConfigSite", model);
        }

        [HttpPost, Route("sua-nhanh-trang-chu"), ValidateInput(false)]
        public JsonResult InlineHomeEdit(string source, string field, string value, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                return Json(new { status = false });
            }

            value = (value ?? string.Empty).Trim();

            if (string.Equals(source, "config", StringComparison.OrdinalIgnoreCase))
            {
                var config = _unitOfWork.ConfigSiteRepository.Get().FirstOrDefault();
                if (config == null)
                {
                    return Json(new { status = false });
                }

                switch (field)
                {
                    case "HomeFleetTitle": config.HomeFleetTitle = value; break;
                    case "HomeFleetDesc": config.HomeFleetDesc = value; break;
                    case "HomePriceTitle": config.HomePriceTitle = value; break;
                    case "HomePriceDesc": config.HomePriceDesc = value; break;
                    case "HomePriceNoteTitle": config.HomePriceNoteTitle = value; break;
                    case "HomeCtaTitle": config.HomeCtaTitle = value; break;
                    case "HomeCtaDesc": config.HomeCtaDesc = value; break;
                    case "HomeReviewTitle": config.HomeReviewTitle = value; break;
                    case "HomeNewsTitle": config.HomeNewsTitle = value; break;
                    case "HomeNewsDesc": config.HomeNewsDesc = value; break;
                    case "HomeFaqTitle": config.HomeFaqTitle = value; break;
                    case "HomeFaqCtaTitle": config.HomeFaqCtaTitle = value; break;
                    case "HomeFaqCtaDesc": config.HomeFaqCtaDesc = value; break;
                    default: return Json(new { status = false });
                }

                _unitOfWork.Save();
                HttpContext.Application["ConfigSite"] = config;
                return Json(new { status = true });
            }

            if (string.Equals(source, "banner", StringComparison.OrdinalIgnoreCase) && id.HasValue)
            {
                var banner = _unitOfWork.BannerRepository.GetById(id.Value);
                if (banner == null)
                {
                    return Json(new { status = false });
                }

                switch (field)
                {
                    case "BannerName": banner.BannerName = value; break;
                    case "Content": banner.Content = value; break;
                    case "Slogan": banner.Slogan = value; break;
                    default: return Json(new { status = false });
                }

                _unitOfWork.Save();
                return Json(new { status = true });
            }

            return Json(new { status = false });
        }
        #endregion

        #region Introduce
        [Route("thong-tin-gioi-thieu")]
        public ActionResult Introduce(string result = "")
        {
            ViewBag.Result = result;
            var introduce = _unitOfWork.IntroduceRepository.Get().FirstOrDefault();
            return View(introduce);
        }
        [Route("thong-tin-gioi-thieu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Introduce(Introduce model, FormCollection fc)
        {
            var introduce = _unitOfWork.IntroduceRepository.Get().FirstOrDefault();
            if (introduce == null)
            {
                _unitOfWork.IntroduceRepository.Insert(model);
            }
            else
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/introduces/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "Image")
                    {
                        introduce.Image = imgFile;
                    }
                }

                introduce.AboutText = model.AboutText;
                introduce.StaffText = model.StaffText;
                introduce.FeedbackText = model.FeedbackText;
                introduce.TitleMeta = model.TitleMeta;
                introduce.DescriptionMeta = model.DescriptionMeta;

                _unitOfWork.Save();

                return RedirectToAction("Introduce", "Mms", new { result = "success" });
            }
            return View("Introduce", model);
        }
        #endregion

        #region Driver
        [ChildActionOnly]
        public PartialViewResult ListDriver()
        {
            var drivers = _unitOfWork.DiverRepository.Get();
            return PartialView("ListDriver", drivers);
        }
        [Route("them-lai-xe")]
        public ActionResult CreateDriver(string result = "")
        {
            ViewBag.Result = result;
            return View();
        }
        [Route("them-lai-xe")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateDriver(Driver model)
        {
            if (ModelState.IsValid)
            {
                var driver = _unitOfWork.DiverRepository.GetQuery(a => a.Name.Equals(model.Name)).SingleOrDefault();
                if (driver != null)
                {
                    ModelState.AddModelError("", @"Lái xe này đã tồn tại!");
                }
                else
                {
                    _unitOfWork.DiverRepository.Insert(model);
                    _unitOfWork.Save();
                    return RedirectToAction("CreateDriver", new { result = "success" });
                }
            }
            return View();
        }
        [Route("sua-lai-xe")]
        public ActionResult EditDriver(int driverId = 0)
        {
            var driver = _unitOfWork.DiverRepository.GetById(driverId);
            if (driver == null)
            {
                return RedirectToAction("CreateDriver");
            }

            return View(driver);
        }
        [Route("sua-lai-xe")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditDriver(Driver model)
        {
            if (ModelState.IsValid)
            {
                var driver = _unitOfWork.DiverRepository.GetById(model.Id);
                if (driver == null)
                {
                    return RedirectToAction("CreateDriver");
                }
                if (driver.Name != model.Name)
                {
                    var exists = _unitOfWork.DiverRepository.GetQuery(a => a.Name.Equals(model.Name)).SingleOrDefault();
                    if (exists != null)
                    {
                        ModelState.AddModelError("", @"Lái xe này đã tồn tại!");
                        return View(model);
                    }
                    driver.Name = model.Name;
                }
                driver.Active = model.Active;

                _unitOfWork.DiverRepository.Update(driver);
                _unitOfWork.Save();
                return RedirectToAction("CreateDriver", new { result = "update" });
            }
            return View(model);
        }
        public bool DeleteDriver(int driverId = 0)
        {
            var driver = _unitOfWork.DiverRepository.GetById(driverId);
            if (driver == null)
            {
                return false;
            }

            _unitOfWork.DiverRepository.Delete(driver);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Expense
        [Route("danh-sach-khoan-chi")]
        public ActionResult ListExpense(int? page, string createDate, int driverId = 0, int status = -1, int expenditure = -1, string result = "")
        {
            var expenses = _unitOfWork.ExpenseRepository.GetQuery(orderBy: o => o.OrderByDescending(a => a.CreateDate));
            var pageNumber = page ?? 1;
            var pageSize = 20;
            ViewBag.Result = result;

            if (driverId > 0)
            {
                expenses = expenses.Where(a => a.DriverId == driverId);
            }
            if (status >= 0)
            {
                expenses = expenses.Where(a => a.Status == (StatusExpense)status);
            }
            if (expenditure >= 0)
            {
                expenses = expenses.Where(a => a.Expenditure == (Expenditure)expenditure);
            }
            DateTime today = DateTime.Today;
            DateTime fromDate = new DateTime(today.Year, today.Month, 1);
            DateTime toDate = fromDate.AddMonths(1).AddDays(-1);


            if (string.IsNullOrEmpty(createDate))
            {
                createDate = $"{fromDate.ToString("dd/MM/yyyy")} - {toDate.ToString("dd/MM/yyyy")}";
            }
            else
            {
                fromDate = Convert.ToDateTime(createDate.Split('-')[0].Trim());
                toDate = Convert.ToDateTime(createDate.Split('-')[1].Trim());
            }

            expenses = expenses.Where(a => DbFunctions.TruncateTime(a.CreateDate) >= fromDate && DbFunctions.TruncateTime(a.CreateDate) <= DbFunctions.TruncateTime(toDate));

            var model = new ListExpenseViewModel
            {
                DriverId = driverId,
                Status = status,
                Expenditure = (Expenditure)expenditure,
                CreateDate = createDate,
                Expenses = expenses.ToPagedList(pageNumber, pageSize),
                Drivers = _unitOfWork.DiverRepository.Get(a => a.Active, o => o.OrderBy(a => a.CreateDate)),
                Gasoline = expenses.Where(a => a.Expenditure == Expenditure.Gasoline).Sum(a => a.Price) ?? 0,
                ETC = expenses.Where(a => a.Expenditure == Expenditure.ETC).Sum(a => a.Price) ?? 0,
                Maintenance = expenses.Where(a => a.Expenditure == Expenditure.Maintenance).Sum(a => a.Price) ?? 0,
                CarWash = expenses.Where(a => a.Expenditure == Expenditure.CarWash).Sum(a => a.Price) ?? 0,
                Other = expenses.Where(a => a.Expenditure == Expenditure.Other).Sum(a => a.Price) ?? 0,
                Total = expenses.Sum(a => a.Price) ?? 0,
            };

            return View(model);
        }
        [Route("them-khoan-chi")]
        public ActionResult Expense()
        {
            var model = new InsertExpenseViewModel
            {
                CreateDate = DateTime.Now.ToString("dd/MM/yyyy"),
                Drivers = _unitOfWork.DiverRepository.Get(a => a.Active, o => o.OrderBy(a => a.CreateDate))
            };

            return View(model);
        }
        [Route("them-khoan-chi")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Expense(InsertExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Expense.Price = Convert.ToDecimal(model.Price.Replace(".", ""));
                model.Expense.CreateDate = Convert.ToDateTime(model.CreateDate);

                _unitOfWork.ExpenseRepository.Insert(model.Expense);
                _unitOfWork.Save();

                return RedirectToAction("ListExpense", new { result = "success" });
            }

            model.Drivers = _unitOfWork.DiverRepository.Get(a => a.Active, o => o.OrderBy(a => a.CreateDate));
            return View(model);
        }
        [Route("sua-khoan-chi")]
        public ActionResult UpdateExpense(int expenseId)
        {
            var expense = _unitOfWork.ExpenseRepository.GetById(expenseId);

            if (expense == null)
            {
                return RedirectToAction("ListExpense", new { result = "error" });
            }

            var model = new InsertExpenseViewModel
            {
                Price = expense.Price?.ToString("N0"),
                CreateDate = expense.CreateDate.ToString("dd/MM/yyyy"),
                Drivers = _unitOfWork.DiverRepository.Get(a => a.Active, o => o.OrderBy(a => a.CreateDate)),
                Expense = expense
            };

            return View(model);
        }
        [Route("sua-khoan-chi")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateExpense(InsertExpenseViewModel model)
        {
            var expense = _unitOfWork.ExpenseRepository.GetById(model.Expense.Id);

            if (expense == null)
            {
                return RedirectToAction("ListExpense", new { result = "error" });
            }

            if (ModelState.IsValid)
            {
                expense.Price = Convert.ToDecimal(model.Price.Replace(".", ""));
                expense.CreateDate = Convert.ToDateTime(model.CreateDate);
                expense.DriverId = model.Expense.DriverId;
                expense.Expenditure = model.Expense.Expenditure;
                expense.Status = model.Expense.Status;
                expense.Note = model.Expense.Note;

                _unitOfWork.Save();

                return RedirectToAction("ListExpense", new { result = "update" });
            }

            model.Drivers = _unitOfWork.DiverRepository.Get(a => a.Active, o => o.OrderBy(a => a.CreateDate));
            return View(model);
        }
        public bool DeleteExpense(int expenseId = 0)
        {
            var expense = _unitOfWork.ExpenseRepository.GetById(expenseId);

            if (expense == null)
            {
                return false;
            }

            _unitOfWork.ExpenseRepository.Delete(expense);
            _unitOfWork.Save();

            return true;
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
