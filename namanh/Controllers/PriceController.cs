using namanh.DAL;
using namanh.Models;
using namanh.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace namanh.Controllers
{
    public class PriceController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult List(int? page, string name, string result = "")
        {
            ViewBag.Result = result;
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var products = _unitOfWork.PriceLangdingRepository.Get(orderBy: a => a.OrderBy(o => o.Sort));

            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(l => l.Name.Contains(name));
            }

            
            var model = new ListViewModel
            {
                Price = products.ToPagedList(pageNumber, pageSize),
                Name = name,
            };
            return View(model);
        }


        public ActionResult Price()
        {
            var model = new PriceLangding
            {
                Sort = 1,
                Active = true,
            };
            return View(model);
        }


        [HttpPost]
        public ActionResult Price(PriceLangding model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PriceLangdingRepository.Insert(model);
                _unitOfWork.Save();

                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var price = _unitOfWork.PriceLangdingRepository.GetById(id);
            if (price == null)
                return HttpNotFound();
            price.Locations = _unitOfWork.LocationRepository.Get(x => x.PriceLangdingId == id).ToList();
            return View(price);
        }

        [HttpPost]
        public ActionResult Edit(PriceLangding model)
        {
            if (ModelState.IsValid)
            {
                // lấy dữ liệu cũ
                var price = _unitOfWork.PriceLangdingRepository.GetById(model.Id);

                if (price == null)
                    return HttpNotFound();

                // update field
                price.Name = model.Name;
                price.Description = model.Description   ;
                price.Sort = model.Sort;
                price.Active = model.Active;

                // xóa location cũ
                var oldLocations = _unitOfWork.LocationRepository
                    .Get(x => x.PriceLangdingId == model.Id)
                    .ToList();

                foreach (var item in oldLocations)
                {
                    _unitOfWork.LocationRepository.Delete(item);
                }

                // thêm location mới
                if (model.Locations != null)
                {
                    foreach (var item in model.Locations)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Name))
                        {
                            var location = new Location
                            {
                                Name = item.Name,
                                PriceLangdingId = model.Id
                            };

                            _unitOfWork.LocationRepository.Insert(location);
                        }
                    }
                }

                _unitOfWork.Save();

                return RedirectToAction("Price");
            }

            return View(model);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            var item = _unitOfWork.PriceLangdingRepository.GetById(id);

            if (item == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Item not found"
                });
            }

            _unitOfWork.PriceLangdingRepository.Delete(item);
            _unitOfWork.Save();

            return Json(new
            {
                success = true,
                message = "Deleted successfully"
            });
        }
    }
}