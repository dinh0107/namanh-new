using ImageResizer.Plugins.Basic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Controls;
using hailinh.DAL;
using hailinh.Models;
using hailinh.ViewModel;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace hailinh.Controllers
{
    public class CustomController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult AddTrip()
        {
            var model = new QickTripViewModel
            {
                Trip = new Trip(),
                Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.CreateDate))
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddTrip(QickTripViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate));
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "Vui lòng nhập số điện thoại");
                model.Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate));
                return View(model);
            }
            var customer = _unitOfWork.CustomerRepository.GetQuery(a => a.Mobile.Contains(model.Mobile)).FirstOrDefault();
            if (customer == null)
            {
                var lastCode = _unitOfWork.CustomerRepository.GetQuery(orderBy: o => o.OrderByDescending(a => a.CreateDate))
                                                            .FirstOrDefault()?.Code;

                customer = new Customer
                {
                    Mobile = model.Mobile,
                    Name = model.Name ?? "Chưa có TT",
                    Active = true,
                    Code = GenerateNextCodeCustomer(lastCode),
                    CreateDate = DateTime.Now
                };
                _unitOfWork.CustomerRepository.Insert(customer);
                _unitOfWork.Save();
            }

            var trip = model.Trip;

            trip.CustomerId = customer.Id;
            if (!string.IsNullOrEmpty(model.Moth))
            {
                trip.ClosingMonth = new DateTime(DateTime.Now.Year, Convert.ToInt32(model.Moth), 1);
            }
            else
            {
                trip.ClosingMonth = null;
            }
            DateTime fromDate;
            DateTime toDate;
            if (!string.IsNullOrEmpty(model.FromDate) && model.FromDate.Contains("-"))
            {
                var cleaned = model.FromDate.Replace("  ", " ").Trim(); 

                var parts = cleaned.Split(new[] { '-' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2 &&
                    DateTime.TryParseExact(parts[0].Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate) &&
                    DateTime.TryParseExact(parts[1].Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
                {
                    trip.FromDate = fromDate;
                    trip.ToDate = toDate;
                }
            }


            trip.Price = ParseDecimal(model.Price);
            trip.PriceSale = ParseDecimal(model.PriceSale);
            trip.PriceChange = ParseDecimal(model.PriceChange);
            trip.Tolls = ParseDecimal(model.Tolls);
            trip.Other = ParseDecimal(model.Other);
            trip.Pile = ParseDecimal(model.Pile);

            trip.DriverId = model.DriverId;

            _unitOfWork.TripRepository.Insert(trip);
            _unitOfWork.Save();
            if (trip.TypeTrip == TypeTrip.Change)
            {
                return RedirectToAction("ListTripChange" , "Customer", new { Time = "month" });
            }
            if (trip.TypeTrip == TypeTrip.Drive)
            {
                return RedirectToAction("ListTrip" , "Customer", new { Time = "month" });
            }
            model.Drivers = _unitOfWork.DiverRepository.GetQuery(a => a.Active, o => o.OrderBy(a => a.CreateDate));
            return RedirectToAction("AddTrip");
        }
        private decimal? ParseDecimal(string input)
        {
            return decimal.TryParse(input?.Replace(".", ""), out var value) ? value : (decimal?)null;
        }
        public string GenerateNextCodeCustomer(string lastCode)
        {
            if (string.IsNullOrEmpty(lastCode))
                return "KH0001";

            string prefix = new string(lastCode.TakeWhile(char.IsLetter).ToArray());
            string numberPart = new string(lastCode.SkipWhile(char.IsLetter).ToArray());

            if (int.TryParse(numberPart, out int number))
            {
                number++;
                int numberLength = Math.Max(4, number.ToString().Length);
                return $"{prefix}{number.ToString($"D{numberLength}")}";
            }

            throw new Exception("Định dạng mã không hợp lệ");
        }

        [HttpPost]
        public JsonResult UpdateClosingMonthByFromDate(int month, int closingMonth)
        {
            try
            {
                var trips = _unitOfWork.TripRepository.GetQuery(a => a.FromDate.Month == month);


                int count = 0;
                foreach (var trip in trips)
                {
                    int year = trip.FromDate.Year;
                    trip.ClosingMonth = new DateTime(year, closingMonth, 1); 
                    _unitOfWork.TripRepository.Update(trip);
                    count++;
                }

                _unitOfWork.Save();

                return Json(new
                {
                    success = true,
                    updated = count,
                    message = $"Đã cập nhật {count} chuyến sang tháng chốt = {closingMonth}"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    updated = 0,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }



        #region

        public ActionResult ListPrice( string name = "")
        {
            var price = _unitOfWork.PriceTableRepository.GetQuery(orderBy: a => a.OrderBy(l => l.Sort));
            if (!string.IsNullOrEmpty(name))
            {
                price = price.Where(a => a.RouteDescription.Contains(name));
            }
            var model = new ListPriceTabViewModel
            {
                PriceTables = price,
                Name = name,
            };
            return View(model);
        }
        public ActionResult AddPrice()
        {
            var model = new PriceTabViewModel
            {
                PriceTable = new PriceTable
                {
                    Sort = 1
                }
            };
            return View(model);

        }
        [HttpPost]
        public ActionResult AddPrice(PriceTabViewModel model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.PriceTableRepository.Insert(model.PriceTable);
                _unitOfWork.Save();
                return RedirectToAction("ListPrice", new { result = "success" });
            }
            return View(model);
        }

        public ActionResult UpdatePrice(int id)
        {
            var price = _unitOfWork.PriceTableRepository.GetById(id);
            if (price == null)
            {
                return RedirectToAction("ListPrice");
            }
            var model = new PriceTabViewModel
            {
                PriceTable = price,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdatePrice(PriceTabViewModel model)
        {
            var price = _unitOfWork.PriceTableRepository.GetById(model.PriceTable.Id);

            if (price == null)
                return RedirectToAction("ListPrice");

            if (ModelState.IsValid)
            {
                price.Sort = model.PriceTable.Sort;
                price.Price4 = model.PriceTable.Price4;
                price.Price7 = model.PriceTable.Price7;
                price.Price16 = model.PriceTable.Price16;
                price.Price29 = model.PriceTable.Price29;
                price.Price45 = model.PriceTable.Price45;
                price.PriceLim = model.PriceTable.PriceLim;
                price.Hot = model.PriceTable.Hot;
                price.RouteDescription = model.PriceTable.RouteDescription;

                _unitOfWork.PriceTableRepository.Update(price);
                _unitOfWork.Save();

                return RedirectToAction("ListPrice", new { result = "update" });
            }

            return View(model);
        }


        [HttpPost]
        public bool DeletePrice(int catId = 0)
        {

            var category = _unitOfWork.PriceTableRepository.GetById(catId);
            if (category == null)
            {
                return false;
            }
            _unitOfWork.PriceTableRepository.Delete(category);
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