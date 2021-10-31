using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartEDU_2.Controllers
{
    public class CameraController : Controller
    {
        // GET: Camera
        public bool IsLoggined()
        {
            if (Session["TAIKHOAN"] != null)
            {
          
                return true;
            }
            else
                return false;
        }

        public ActionResult Index()
        {
            Session["ReturnUrl"] = Url.Action("Index", "Camera");

            if (IsLoggined() == true)
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Account");
        }
    }
}