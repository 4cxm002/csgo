using CSGO.Models;
using DotNetOpenAuth.OpenId.RelyingParty;
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
            //CSGO.Domain.Repository.LoginRepository lr = new Domain.Repository.LoginRepository();
            //lr.Login("moorgazm", "fhorn160");

            var openid = new OpenIdRelyingParty();
            var response = openid.GetResponse();

            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        // do success
                        var responseURI = response.ClaimedIdentifier.ToString();
                        //"http://steamcommunity.com/openid/id/76561197969877387"
                        // last part is steam user id
                        break;

                    case AuthenticationStatus.Canceled:
                    case AuthenticationStatus.Failed:
                        // do fail
                        break;
                }
            }
            else
            {
                using (OpenIdRelyingParty openidd = new OpenIdRelyingParty())
                {
                    IAuthenticationRequest request = openidd.CreateRequest("http://steamcommunity.com/openid");
                    request.RedirectToProvider();
                }
            }


            var model = new IndexViewModel();

            return View("Index", model);
        }

        public ActionResult Login(LoginViewModel lvm)
        {
            

            var model = new IndexViewModel();

            return View("Index", model);
        }
    }
}