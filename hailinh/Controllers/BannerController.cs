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
    public class BannerController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [Route("danh-sach-banner")]
        public ActionResult ListBanner(int? page, int groupId = 0, string result = "")
        {
            ViewBag.Banner = result;
            var pageNumber = page ?? 1;
            const int pageSize = 10;
            var banners = _unitOfWork.BannerRepository.GetQuery(orderBy: q => q.OrderBy(a => a.GroupId).ThenBy(a => a.Sort));
            if (groupId > 0)
            {
                banners = banners.Where(a => a.GroupId == groupId);
            }
            var model = new ListBannerViewModel
            {
                Banners = banners.ToPagedList(pageNumber, pageSize),
            };
            return View(model);
        }
        [Route("them-banner")]
        public ActionResult Banner()
        {
            var model = new BannerViewModel()
            {
                Banner = new Banner() { Sort = 1, Active = true }
            };
            return View(model);
        }
        [Route("them-banner")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Banner(BannerViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var file = Request.Files["Banner.Image"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif|svg|mp4"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg, svg, mp4");
                        isPost = false;
                    }
                    else
                    {
                        if (file.ContentLength > 100000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 100MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/banners/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                            var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(file.FileName)) +
                                "-" + DateTime.Now.Millisecond + Path.GetExtension(file.FileName);

                            model.Banner.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                        }
                    }
                }
                if (isPost)
                {
                    model.Banner.ListImage = fc["Pictures"];
                    _unitOfWork.BannerRepository.Insert(model.Banner);
                    _unitOfWork.Save();

                    return RedirectToAction("ListBanner", new { result = "success" });
                }
            }
            return View(model);
        }
        [Route("sua-banner")]
        public ActionResult EditBanner(int bannerId = 0)
        {
            var banner = _unitOfWork.BannerRepository.GetById(bannerId);
            if (banner == null)
            {
                return RedirectToAction("ListBanner");
            }
            var model = new BannerViewModel
            {
                Banner = banner,
            };
            return View(model);
        }
        [Route("sua-banner")]
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBanner(BannerViewModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;

                var banner = _unitOfWork.BannerRepository.GetById(model.Banner.Id);

                var file = Request.Files["Banner.Image"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif|svg|mp4"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg, svg, mp4");
                        isPost = false;
                    }
                    else
                    {
                        if (file.ContentLength > 100000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 100MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/banners/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = HtmlHelpers.ConvertToUnSign(null, Path.GetFileNameWithoutExtension(file.FileName)) +
                                "-" + DateTime.Now.Millisecond + Path.GetExtension(file.FileName);

                            banner.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                        }
                    }
                }

                if (isPost)
                {
                    banner.ListImage = fc["Pictures"] == "" ? null : fc["Pictures"];
                    banner.GroupId = model.Banner.GroupId;
                    banner.BannerName = model.Banner.BannerName;
                    banner.Slogan = model.Banner.Slogan;
                    banner.Sort = model.Banner.Sort;
                    banner.Active = model.Banner.Active;
                    banner.Url = model.Banner.Url;
                    banner.Content = model.Banner.Content;

                    _unitOfWork.BannerRepository.Update(banner);
                    _unitOfWork.Save();

                    return RedirectToAction("ListBanner", new { result = "update" });
                }
            }
            return View(model);
        }
        [HttpPost]
        public bool DeleteBanner(int bannerId = 0)
        {
            var banner = _unitOfWork.BannerRepository.GetById(bannerId);
            if (banner == null)
            {
                return false;
            }
            HtmlHelpers.DeleteFile(Server.MapPath("/images/banners/" + banner.Image));
            _unitOfWork.BannerRepository.Delete(banner);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateBannerQuick(int sort = 1, bool active = false, int bannerId = 0)
        {
            var banner = _unitOfWork.BannerRepository.GetById(bannerId);
            if (banner == null)
            {
                return false;
            }
            banner.Sort = sort;
            banner.Active = active;

            _unitOfWork.Save();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
