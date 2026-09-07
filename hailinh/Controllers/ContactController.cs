using hailinh.DAL;
using hailinh.Models;
using hailinh.ViewModel;
using Helpers;
using PagedList;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
namespace hailinh.Controllers
{
    [Authorize, RoutePrefix("mms")]
    public class ContactController : CustomerController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private string AddNewCustomerAndTrip(Contact contact)
        {
            var lastCode = _unitOfWork.CustomerRepository.GetQuery(a => a.Active, o => o.OrderByDescending(a => a.CreateDate)).FirstOrDefault()?.Code;

            var customer = new Customer
            {
                Name = "Chưa có thông tin",
                Mobile = contact.Mobile,
                Code = GenerateNextCodeCustomer(lastCode),
                Active = true
            };

            _unitOfWork.CustomerRepository.Insert(customer);
            _unitOfWork.Save();

            AddTrip(contact, customer.Id);

            return "Khách hàng đã được thêm thành công. \nVui lòng cập nhật đầy đủ thông tin khách hàng!";
        }
        private string AddTripForExistingCustomer(Contact contact, int customerId)
        {
            AddTrip(contact, customerId);
            return "Chuyến đi đã được thêm thành công. \nVui lòng kiểm tra lại thông tin chuyến đi!";
        }

        private void AddTrip(Contact contact, int customerId)
        {
            var trip = new Trip
            {
                From = contact.From,
                To = contact.To,
                FromDate = contact.FromDate,
                ToDate = contact.ToDate,
                Active = true,
                CustomerId = customerId,
                Status = StatusTrip.Latch,
                Source = Source.Web
            };

            _unitOfWork.TripRepository.Insert(trip);
        }

        #region Contact
        [Route("danh-sach-lien-he")]
        public ActionResult ListContact(int? page, string name)
        {
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var contacts = _unitOfWork.ContactRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));

            if (!string.IsNullOrEmpty(name))
            {
                contacts = contacts.Where(l => l.Mobile.Contains(name));
            }
            var model = new ListContactViewModel
            {
                Contacts = contacts.ToPagedList(pageNumber, pageSize),
                Name = name
            };
            return View(model);
        }
        [HttpPost]
        public bool DeleteContact(int contactId = 0)
        {
            var contact = _unitOfWork.ContactRepository.GetById(contactId);
            if (contact == null)
            {
                return false;
            }
            _unitOfWork.ContactRepository.Delete(contact);
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public JsonResult UpdateStatus(int contactId = 0, int status = 0)
        {
            var contact = _unitOfWork.ContactRepository.GetById(contactId);
            if (contact == null)
            {
                return Json(new { status = false, msg = "Thông tin liên hệ không chính xác" });
            }
                
            contact.StatusContact = (StatusContact)status;
            string msg = "Cập nhật trạng thái thành công.";

            if (status == 2)
            {
                var checkMobile = _unitOfWork.CustomerRepository.GetQuery(a => a.Mobile.Contains(contact.Mobile)).FirstOrDefault();

                if (checkMobile == null)
                {
                    msg = AddNewCustomerAndTrip(contact);
                }
                else
                {
                    msg = AddTripForExistingCustomer(contact, checkMobile.Id);
                }
            }

            _unitOfWork.Save();
            return Json(new { status = true, msg = msg });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
