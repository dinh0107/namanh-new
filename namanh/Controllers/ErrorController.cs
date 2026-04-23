using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace namanh.Controllers
{
    public class ErrorController : Controller
    {
        [Route("not-found")]
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
        [Route("general")]
        public ActionResult General()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}