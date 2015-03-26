using CSGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSGO.Controllers
{
    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            return View("Index", model);
        }

        public ActionResult Login(LoginViewModel lvm)
        {
            //CSGO.Domain.Repository.LoginRepository lr = new Domain.Repository.LoginRepository();
            //lr.Login("moorgazm", "fhorn160");

            var model = new IndexViewModel();

            return View("Index", model);
        }
    }
}