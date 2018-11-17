using LoginMVC.Manager;
using LoginMVC.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LoginMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Main(string userName)
        {
            if (userName != null)
            {
                LoginManager login = new LoginManager();
                Employee emp = login.GetCurrentEmployee(userName);
                UIDropDown uIDropDown = login.GetDropDownValues();
                ViewBag.Employee = emp;
                ViewBag.UIDropDown = uIDropDown;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        
        public ActionResult GetApplication(string id)
        {
            LoginManager login = new LoginManager();            
            return Json(null);
        }

        [HttpPost]
        public ActionResult CheckLogin(Employee emp)
        {
            LoginManager login = new LoginManager();
            return Json(login.CheckLogin(emp));
        }

        [HttpGet]
        public ActionResult DeleteApplication(string id)
        {
            LoginManager login = new LoginManager();
            return Json(login.DeleteApplication(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Application()
        {
            LoginManager login = new LoginManager();
            List<Application> applications = login.GetAllApplication();
            return Json(applications, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddApplication(Application application)
        {
            LoginManager login = new LoginManager();
            return Json(login.AddApplication(application), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RegisterUser(Employee register)
        {
            LoginManager login = new LoginManager();
            return Json(login.RegisterUser(register), JsonRequestBehavior.AllowGet);
        }
    }
}
