using Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using hailinh.DAL;
using hailinh.Models;
using hailinh.ViewModel;

namespace hailinh.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class CarServiceController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region CarService
        [ChildActionOnly]
        public ActionResult ListCarService()
        {
            var allcats = _unitOfWork.CarServiceRepository.Get(orderBy: q => q.OrderBy(a => a.Id));
            return PartialView(allcats);
        }
        [Route("them-trang-dich-vu")]
        public ActionResult CarService(string result = "")
        {
            ViewBag.ArticleCat = result;
            return View();
        }
        [Route("them-trang-dich-vu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult CarService(CarService model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/carService/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    Request.Files[i].SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));

                    if (Request.Files.Keys[i] == "ImageUrl")
                    {
                        model.ImageUrl = imgFile;
                    }
                    if (Request.Files.Keys[i] == "ImageUrlMobile")
                    {
                        model.ImageUrlMobile = imgFile;
                    }
                    if (Request.Files.Keys[i] == "Image")
                    {
                        model.Image = imgFile;
                    }
                }

                model.Slug = HtmlHelpers.ConvertToUnSign(null, model.Slug ?? model.Title);
                model.ListImage = fc["Pictures"];
                _unitOfWork.CarServiceRepository.Insert(model);
                _unitOfWork.Save();
                return RedirectToAction("CarService", new { result = "success" });
            }
            return View(model);
        }
        [Route("sua-trang-dich-vu")]
        public ActionResult UpdateCarService(int catId = 0)
        {
            var category = _unitOfWork.CarServiceRepository.GetById(catId);
            if (category == null)
            {
                return RedirectToAction("CarService");
            }
            return View(category);
        }
        [Route("sua-trang-dich-vu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCarService(CarService category, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/carService/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    Request.Files[i].SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));

                    if (Request.Files.Keys[i] == "ImageUrl")
                    {
                        category.ImageUrl = imgFile;
                    }
                    if (Request.Files.Keys[i] == "ImageUrlMobile")
                    {
                        category.ImageUrlMobile = imgFile;
                    }
                    if (Request.Files.Keys[i] == "Image")
                    {
                        category.Image = imgFile;
                    }
                }


                var file = Request.Files["ImageUrl"];

                if (file != null && file.ContentLength == 0)
                {
                    category.ImageUrl = fc["CurrentFile"] == "" ? null : fc["CurrentFile"];
                }

                var mobile = Request.Files["ImageUrlMobile"];
                if(mobile != null && mobile.ContentLength == 0)
                {
                    category.ImageUrlMobile = fc["CurrentFileMobile"] == "" ? null : fc["CurrentFileMobile"];
                }

                var file2 = Request.Files["Image"];

                if (file2 != null && file2.ContentLength == 0)
                {
                    category.Image = fc["CurrentFileImg"] == "" ? null : fc["CurrentFileImg"];
                }
                category.ListImage = fc["Pictures"] == "" ? null : fc["Pictures"];
                category.Slug = HtmlHelpers.ConvertToUnSign(null, category.Slug ?? category.Title);
                _unitOfWork.CarServiceRepository.Update(category);
                _unitOfWork.Save();
                return RedirectToAction("CarService", new { result = "update" });
            }
            return View(category);
        }
        [HttpPost]
        public bool DeleteCarService(int catId = 0)
        {

            var category = _unitOfWork.CarServiceRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            foreach (var detail in category.Details.ToList())
            {
                _unitOfWork.CarServiceDetailRepository.Delete(detail);
            }
            foreach (var price in category.CarServicePrices.ToList())
            {
                _unitOfWork.CarServicePriceRepository.Delete(price);
            }
            _unitOfWork.CarServiceRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Price
        public ActionResult ListPrice(int id, string name = "")
        {
            var service = _unitOfWork.CarServiceRepository.GetById(id);
            if (service == null)
            {
                return RedirectToAction("CarService");
            }
            var price = _unitOfWork.CarServicePriceRepository.GetQuery(a => a.CarServiceId == service.Id, a => a.OrderBy(l => l.Sort));
            if (!string.IsNullOrEmpty(name))
            {
                price = price.Where(a => a.RouteDescription.Contains(name));
            }
            var model = new ListPriceViewModel
            {
                CarServicePrices = price,
                Name = name,
                CarService = service
            };
            return View(model);
        }
        public ActionResult AddPrice(int id)
        {
            var service = _unitOfWork.CarServiceRepository.GetById(id);
            if (service == null)
            {
                return RedirectToAction("CarService", new { id = service.Id });
            }
            var model = new PriceViewModel
            {
                CarService = service,
                CarServicePrice = new CarServicePrice
                {
                    Sort = 1
                }
            };
            return View(model);

        }
        [HttpPost]
        public ActionResult AddPrice(PriceViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.CarServicePrice.CarServiceId = model.CarService.Id;
                _unitOfWork.CarServicePriceRepository.Insert(model.CarServicePrice);
                _unitOfWork.Save();
                return RedirectToAction("ListPrice", new { id = model.CarService.Id, result = "success" });
            }
            return View(model);
        }

        public ActionResult UpdatePrice(int id)
        {
            var price = _unitOfWork.CarServicePriceRepository.GetById(id);
            if (price == null)
            {
                return RedirectToAction("CarService", new { id = price.CarService.Id });
            }
            var model = new PriceViewModel
            {
                CarServicePrice = price,
                CarService = price.CarService
            };
            return View(model);
        }
        
        [HttpPost]
        public ActionResult UpdatePrice(PriceViewModel model)
        {
            var service = _unitOfWork.CarServiceRepository.GetById(model.CarService.Id);
            if (service == null)
                return RedirectToAction("CarService");
            var price = _unitOfWork.CarServicePriceRepository.GetById(model.CarServicePrice.Id);

            if (price == null)
                return RedirectToAction("AddPrice", new { id = service.Id });

            if (ModelState.IsValid)
            {
                price.Sort = model.CarServicePrice.Sort;
                price.Price = model.CarServicePrice.Price;
                price.Hot = model.CarServicePrice.Hot;
                price.Km = model.CarServicePrice.Km;
                price.RouteDescription = model.CarServicePrice.RouteDescription;

                _unitOfWork.CarServicePriceRepository.Update(price);
                _unitOfWork.Save();

                return RedirectToAction("ListPrice", new { id = price.CarServiceId, result = "update" });
            }

            return View(model);
        }


        [HttpPost]
        public bool DeletePrice(int catId = 0)
        {

            var category = _unitOfWork.CarServicePriceRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.CarServicePriceRepository.Delete(category);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Detail 
        public ActionResult ListDetail(int id, string name = "", CarServiceType? CarServiceType = null)
        {
            var service = _unitOfWork.CarServiceRepository.GetById(id);
            if (service == null)
            {
                return RedirectToAction("CarService");
            }
            var detail = _unitOfWork.CarServiceDetailRepository.GetQuery(a => a.CarServiceId == service.Id, a => a.OrderBy(l => l.Sort));
            if (!string.IsNullOrEmpty(name))
            {
                detail = detail.Where(a => a.Name.Contains(name));
            }
            if (CarServiceType.HasValue)
            {
                detail = detail.Where(a => a.CarServiceType == CarServiceType.Value);
            }

            var model = new ListDetailViewModel
            {
                CarServiceDetails = detail,
                Name = name,
                CarServiceType = CarServiceType,
                CarService = service
            };
            return View(model);
        }

        public ActionResult Detail(int id)
        {
            var service = _unitOfWork.CarServiceRepository.GetById(id);
            if (service == null)
            {
                return RedirectToAction("CarService", new { id = service.Id });
            }
            var model = new DetailViewModel
            {
                CarService = service,
                CarServiceDetail = new CarServiceDetail
                {
                    Sort = 1
                }
            };
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Detail(DetailViewModel model, FormCollection fc)
        {
            var service = _unitOfWork.CarServiceRepository.GetById(model.CarService.Id);
            if (service == null)
            {
                return RedirectToAction("CarService", new { id = service.Id });
            }
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/detail/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "CarServiceDetail.Image")
                    {
                        model.CarServiceDetail.Image = imgFile;
                    }
                }
                model.CarServiceDetail.ListImage = fc["Pictures"];
                model.CarServiceDetail.CarServiceId = service.Id;
                _unitOfWork.CarServiceDetailRepository.Insert(model.CarServiceDetail);
                _unitOfWork.Save();
                return RedirectToAction("ListDetail", new { id = service.Id, result = "success" });

            }
            return View(model);
        }
        public ActionResult UpdateDetail(int id)
        {
            var price = _unitOfWork.CarServiceDetailRepository.GetById(id);
            if (price == null)
            {
                return RedirectToAction("ListDetail", new { id = price.CarService.Id });
            }
            var model = new DetailViewModel
            {
                CarServiceDetail = price,
                CarService = price.CarService
            };
            return View(model);
        }
        [HttpPost , ValidateInput(false)]
        public ActionResult UpdateDetail(DetailViewModel model, FormCollection fc)
        {
            var detail = _unitOfWork.CarServiceDetailRepository.GetById(model.CarServiceDetail.Id);
            if (detail == null)
            {
                return RedirectToAction("ListDetail", new { id = detail.CarService.Id });
            }
            if (ModelState.IsValid)
            {
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] == null || Request.Files[i].ContentLength <= 0) continue;
                    if (!HtmlHelpers.CheckFileExt(Request.Files[i].FileName, "jpg|jpeg|png|gif")) continue;
                    if (Request.Files[i].ContentLength > 1024 * 1024 * 4) continue;

                    var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(Request.Files[i].FileName)) +
                        "-" + DateTime.Now.Millisecond + Path.GetExtension(Request.Files[i].FileName);
                    var imgPath = "/images/detail/" + DateTime.Now.ToString("yyyy/MM/dd");
                    HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                    var imgFile = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;

                    var newImage = Image.FromStream(Request.Files[i].InputStream);
                    var fixSizeImage = HtmlHelpers.FixedSize(newImage, 1000, 1000, false);
                    HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);

                    if (Request.Files.Keys[i] == "CarServiceDetail.Image")
                    {
                        detail.Image = imgFile;
                    }
                }
                detail.ListImage = fc["Pictures"] == "" ? null : fc["Pictures"];
                detail.CarServiceId = detail.CarServiceId;
                detail.Name = model.CarServiceDetail.Name;
                detail.Sort = model.CarServiceDetail.Sort;
                detail.Desciption = model.CarServiceDetail.Desciption;
               detail.Body = model.CarServiceDetail.Body;
                _unitOfWork.Save();
                return RedirectToAction("ListDetail", new { id = detail.CarServiceId, result = "success" });
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteDetail(int id)
        {
            var category = _unitOfWork.CarServiceDetailRepository.GetById(id);
            if (category == null)
            {
                return Json(new { success = false });
            }
            _unitOfWork.CarServiceDetailRepository.Delete(category);
            _unitOfWork.Save();
            return Json(new { success = true });
        }



        #endregion
        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}