using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEDU_2.Models;

namespace SmartEDU_2.Controllers
{
    public class AccountController : Controller
    {
        LinQDataContext data = new LinQDataContext();
        // GET: Login
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection.Get("id");
            var matkhau = collection.Get("pass");
            var luu = collection.Get("");
            if (tendn == null || matkhau == null)
            {
                ViewBag.Notifi = "";
            }
            else
            {
                
                TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.TENDANGNHAP == tendn && n.MATKHAU == matkhau);
                
                if (tk != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công!!!";
                    Session["TAIKHOAN"] = tk;
                    var returnUrl = Session["ReturnUrl"];
                    if(returnUrl != null) { return Redirect(returnUrl.ToString()); }
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Notifi = "Sai tài khoản hoặc mật khẩu";
            }
            return View();
        }
       
        public ActionResult LoginPartial()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["Taikhoan"];
            if (tk != null)
                ViewBag.TenNguoiDung = tk.HOTEN;
            return PartialView();
        }
        public ActionResult OutLogin()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "Home");
        }
    }    
}
