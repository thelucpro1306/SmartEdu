using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEDU_2.Models;

namespace SmartEDU_2.Controllers
{
    public class AdminController : Controller
    {
        LinQDataContext data = new LinQDataContext();

        public bool IsLoggined()
        {
            if (Session["ADMIN"] != null)
                return true;
            else
                return false;
        }

        public ActionResult LoginAdmin(FormCollection collection)
        {
            var taikhoanad = collection.Get("Name");
            var matkhauad = collection.Get("Password");

            

            ADMIN ad = data.ADMINs.SingleOrDefault(n => n.TaiKhoanAdmin == taikhoanad && n.MatKhauAdmin == matkhauad);
            if (ad != null)
            {
                Session["ADMIN"] = ad;
                return RedirectToAction("Index", "Admin");
            }
            else


                return View();
        }
        
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, TAIKHOAN tk)
        {

            var hoten = collection["name"];
            var tendangnhap = collection["username"];
            var matkhau = collection["pass"];
            var sodienthoai = collection["sdt"];
            tk.TENDANGNHAP = tendangnhap;
            tk.HOTEN = hoten;
            tk.MATKHAU = matkhau;
            tk.SDT = sodienthoai;
            data.TAIKHOANs.InsertOnSubmit(tk);
            data.SubmitChanges();
            Session["SDT"] = sodienthoai;
            Session["Message"] = "TK SmartEdu của bạn là : "+ " TK: " +tendangnhap + " MK : "+ matkhau;
            ViewBag.Notifi = "Đăng kí thành công";
            return RedirectToAction("ListAccount");
        }


        public ActionResult Index()
        {
            
            if (IsLoggined()==true)
            {
                return View();
            }
            else
                return RedirectToAction("LoginAdmin");        
        }
        private bool ChangeStatus(int id)
        {
            var khoahoc = data.KHOAHOCs.First(c => c.MAKHOAHOC == id);
            khoahoc.TRANGTHAI = !khoahoc.TRANGTHAI;
            UpdateModel(khoahoc);
            data.SubmitChanges();
            return (bool)!khoahoc.TRANGTHAI;
        }
        [HttpPost]
        public JsonResult TrangThaiKhoaHoc(int id)
        {
            var khoahoc = data.KHOAHOCs.First(c => c.MAKHOAHOC == id);
            khoahoc.TRANGTHAI = !khoahoc.TRANGTHAI;
            UpdateModel(khoahoc);
            data.SubmitChanges();
            var result = khoahoc.TRANGTHAI;
            return Json(new
            {
                status = result
            });

        }
        [HttpGet]
        public ActionResult ListCourse()
        {
            if (IsLoggined() == true)
            {
                var khoahoc = data.KHOAHOCs.Select(a => a);
                return View(khoahoc);
            }
            else
                return RedirectToAction("LoginAdmin");
           

           
        }
        public ActionResult ListAccount()
        {
            if (IsLoggined() == true)
            {
                var taikhoan = data.TAIKHOANs.Select(a => a);
                return View(taikhoan);
            }
            else
                return RedirectToAction("LoginAdmin");
           

        }

        public ActionResult ListNotification()
        {
            if (IsLoggined() == true)
            {
                var thongbao = data.THONGBAOs.Select(a => a);
                return View(thongbao);
            }
            else
                return RedirectToAction("LoginAdmin");


        }
        public ActionResult GoToSchool()
        {
            if (IsLoggined() == true)
            {
                var phieudk = data.PHIEUDANGKYHOCs.Select(a => a);
                return View(phieudk);
            }
            else
                return RedirectToAction("LoginAdmin");
        }
        public ActionResult Menu()
        {
            if (IsLoggined() == true)
            {
                var menu = data.THUCDONs.Select(a => a);
                return View(menu);
            }
            else
                return RedirectToAction("LoginAdmin");
        }

        public ActionResult CheckTeacher()
        {
            if (IsLoggined() == true)
            {
                var gv = data.GIAOVIENs.Select(a => a);
                return View(gv);
            }
            else
                return RedirectToAction("LoginAdmin");
        }

        [HttpGet]
        public ActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCourse(KHOAHOC kh, HttpPostedFileBase fileUpload)
        {
   
            //luu tên file
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn hình ảnh!!!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //kiểm tra hình ảnh có tồn tại?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại!!!";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    kh.ANHBIA = fileName;

                    data.KHOAHOCs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                }
                return RedirectToAction("ListCourse"); 
            }

        }
        //lay doi tuong
        public ActionResult DetailCourse(int id)
        {
            KHOAHOC kh = data.KHOAHOCs.SingleOrDefault(n => n.MAKHOAHOC == id);
            ViewBag.MAKHOAHOC = kh.MAKHOAHOC ;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        public ActionResult DelCourse(int id)
        {
            //lấy đối tượng :
            KHOAHOC kh = data.KHOAHOCs.SingleOrDefault(n => n.MAKHOAHOC == id);
            ViewBag.MAKHOAHOC= kh.MAKHOAHOC;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("DelCourse")]
        public ActionResult ApllyDelCourse(int id)
        {
            //lấy ra đối tượng cần xóa theo mã:
             KHOAHOC kh = data.KHOAHOCs.SingleOrDefault(n => n.MAKHOAHOC == id);
            ViewBag.MAKHOAHOC = kh.MAKHOAHOC;
      
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHOAHOCs.DeleteOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("ListCourse");
        }

        public ActionResult DelAccount(int id)
        {
           
            TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.ID == id);
            ViewBag.ID = tk.ID;
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tk);
        }

        [HttpPost, ActionName("DelAccount")]
        public ActionResult ApllyDelAccount(int id)
        {
           
            TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.ID == id);
            ViewBag.ID = tk.ID;

            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.TAIKHOANs.DeleteOnSubmit(tk);
            data.SubmitChanges();
            return RedirectToAction("ListAccount");
        }
        [HttpGet]
       
        public ActionResult EditCourse(int id)
        {
            var coures = data.KHOAHOCs.First(k => k.MAKHOAHOC == id);
            return View(coures);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCourse(int id, FormCollection collection, HttpPostedFileBase fileupload)
        {
            KHOAHOC kh = data.KHOAHOCs.First(k => k.MAKHOAHOC == id);

            if (ModelState.IsValid)
            {

                try
                {
                    if (fileupload != null)
                    {
                        //Luu ten file
                        var fileName = Path.GetFileName(fileupload.FileName);
                        //Luu duong dan File
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);
                        //Kiem tra hinh da ton tai chua\
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                            fileupload.SaveAs(path);//Luu file vao duong dan

                        kh.ANHBIA= fileName;
                        data.KHOAHOCs.InsertOnSubmit(kh);
                        data.SubmitChanges();
                    }

                }
                catch (Exception)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh cho sản phẩm";
                }

            }
            string tenKh = collection["TENKHOAHOC"];
            decimal hocPhi = decimal.Parse(collection["HOCPHI"]);
            string moTa = collection["MOTA"];
            kh.TENKHOAHOC = tenKh;
            kh.HOCPHI = hocPhi;
            kh.MOTA = moTa;

            UpdateModel(kh);
            data.SubmitChanges();

            return RedirectToAction("DetailCourse", new { id = id });
        }
        public ActionResult EditAccount(int id)
        {
            var acc = data.TAIKHOANs.First(a => a.ID == id);
            return View(acc);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAccount(int id, FormCollection collection)
        {
            TAIKHOAN tk = data.TAIKHOANs.First(a => a.ID == id);


            string tenDN = collection["TENDANGNHAP"];
            string hoTen = collection["HOTEN"];
            string matKhau = collection["MATKHAU"];

            tk.TENDANGNHAP = tenDN;
            tk.HOTEN = hoTen;
            tk.MATKHAU = matKhau;

            UpdateModel(tk);
            data.SubmitChanges();

            return RedirectToAction("ListAccount", new { id = id });
        }
        [HttpGet]

        public ActionResult EditNotification(int id)
        {
            var thongbao = data.THONGBAOs.First(k => k.MATHONGBAO == id);
            return View(thongbao);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNotification(int id, FormCollection collection, HttpPostedFileBase fileupload)
        {
            THONGBAO tb = data.THONGBAOs.First(k => k.MATHONGBAO == id);

            if (ModelState.IsValid)
            {

                try
                {
                    if (fileupload != null)
                    {
                        //Luu ten file
                        var fileName = Path.GetFileName(fileupload.FileName);
                        //Luu duong dan File
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);
                        //Kiem tra hinh da ton tai chua\
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                            fileupload.SaveAs(path);//Luu file vao duong dan

                        tb.HINHANH = fileName;
                        data.THONGBAOs.InsertOnSubmit(tb);
                        data.SubmitChanges();
                    }

                }
                catch (Exception)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh cho sản phẩm";
                }

            }
            string tieuDe = collection["TIEUDE"];
         
            string noiDung = collection["NOIDUNG"];
            DateTime ngayThongBao = DateTime.Parse(collection["NGAYTHONGBAO"]);
            tb.TIEUDE = tieuDe;
            tb.NOIDUNG = noiDung;
            tb.NGAYTHONGBAO = ngayThongBao;

            UpdateModel(tb);
            data.SubmitChanges();

            return RedirectToAction("ListNotification", new { id = id });
        }
        public ActionResult DelNotification(int id)
        {

            THONGBAO tb = data.THONGBAOs.SingleOrDefault(n => n.MATHONGBAO == id);
            ViewBag.MATHONGBAO = tb.MATHONGBAO;
            if (tb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tb);
        }

        [HttpPost, ActionName("DelNotification")]
        public ActionResult ApllyDelNotification(int id)
        {

            THONGBAO tb = data.THONGBAOs.SingleOrDefault(n => n.MATHONGBAO == id);
            ViewBag.MATHONGBAO = tb.MATHONGBAO;

            if (tb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.THONGBAOs.DeleteOnSubmit(tb);
            data.SubmitChanges();
            return RedirectToAction("ListNotification");
        }
        public ActionResult DetailNotification(int id)
        {
            THONGBAO tb = data.THONGBAOs.SingleOrDefault(n => n.MATHONGBAO == id);
            ViewBag.MATHONGBAO = tb.MATHONGBAO;
            if (tb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tb);
        }
    }
   
}

