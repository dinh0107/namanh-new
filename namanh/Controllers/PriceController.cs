using namanh.DAL;
using namanh.Models;
using namanh.ViewModel;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace namanh.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class PriceController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [Route("bang-gia-tuyen")]
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

        [Route("them-tuyen-bang-gia")]
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
        [Route("them-tuyen-bang-gia")]
        public ActionResult Price(PriceLangding model)
        {
            if (ModelState.IsValid)
            {
                var locations = model.Locations;
                model.Locations = null;
                _unitOfWork.PriceLangdingRepository.Insert(model);
                _unitOfWork.Save();
                SaveLocations(model.Id, locations);
                return RedirectToAction("List");
            }

            return View(model);
        }

        [Route("sua-tuyen-bang-gia")]
        public ActionResult Edit(int id)
        {
            var price = _unitOfWork.PriceLangdingRepository.GetById(id);
            if (price == null)
                return HttpNotFound();
            price.Locations = _unitOfWork.LocationRepository
                .Get(x => x.PriceLangdingId == id, orderBy: q => q.OrderBy(x => x.Sort))
                .ToList();
            return View(price);
        }

        [HttpPost]
        [Route("sua-tuyen-bang-gia")]
        public ActionResult Edit(PriceLangding model)
        {
            if (ModelState.IsValid)
            {
                var price = _unitOfWork.PriceLangdingRepository.GetById(model.Id);
                if (price == null)
                    return HttpNotFound();

                price.Name = model.Name;
                price.Description = model.Description;
                price.Sort = model.Sort;
                price.Active = model.Active;

                var oldLocations = _unitOfWork.LocationRepository
                    .Get(x => x.PriceLangdingId == model.Id)
                    .ToList();

                foreach (var item in oldLocations)
                {
                    _unitOfWork.LocationRepository.Delete(item);
                }

                SaveLocations(model.Id, model.Locations);
                _unitOfWork.Save();

                return RedirectToAction("List", new { result = "update" });
            }

            return View(model);
        }

        [HttpPost]
        [Route("xoa-tuyen-bang-gia")]
        public JsonResult Delete(int id)
        {
            var item = _unitOfWork.PriceLangdingRepository.GetById(id);
            if (item == null)
            {
                return Json(new { success = false, message = "Không tìm thấy dữ liệu" });
            }

            _unitOfWork.PriceLangdingRepository.Delete(item);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Xóa thành công" });
        }

        private void SaveLocations(int priceLangdingId, System.Collections.Generic.ICollection<Location> locations)
        {
            if (locations == null)
            {
                return;
            }

            var sort = 1;
            foreach (var item in locations)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    continue;
                }

                _unitOfWork.LocationRepository.Insert(new Location
                {
                    Name = item.Name.Trim(),
                    PriceLangdingId = priceLangdingId,
                    Price4 = item.Price4,
                    Price7 = item.Price7,
                    Price16 = item.Price16,
                    Price29 = item.Price29,
                    Price45 = item.Price45,
                    PriceLim = item.PriceLim,
                    Sort = item.Sort > 0 ? item.Sort : sort,
                    Hot = item.Hot,
                    Active = item.Active
                });
                sort++;
            }

            _unitOfWork.Save();
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
