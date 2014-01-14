﻿using System.Web.Mvc;
using System.Web.Security;
using Meganium.Api.Managers;
using Meganium.Api.Resources;
using Meganium.Api.ViewModels;
using Meganium.Api.Web;

namespace Meganium.Site.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IManagers _managers;

        public AccountController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Login(string returnUrl)
        {
            var vm = new AccountLoginVm { ReturnUrl = returnUrl };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Login(AccountLoginVm vm)
        {
            var user = _managers.UserManager.GetFromUsernameAndPassword(vm.Email, vm.Password);

            if (user == null)
            {
                ModelState.AddModelError("", Resource.UserNameOrPasswordProvidedIsIncorrect);
                return View(vm);
            }
            Session["licenseId"] = null;
            var url = vm.ReturnUrl ?? Url.Content("~/Admin");
            FormsAuthentication.SetAuthCookie(user.UserName, false);
            if (string.IsNullOrEmpty(url) && !Url.IsLocalUrl(url))
            {
                return RedirectToAction("Index", "Post");
            }
            return Redirect(url);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["licenseId"] = null;
            Response.Clear();
            var url = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Content("~/Admin");
            return Redirect(url);
        }

        //Refactor: Revisar isto aqui
        public JsonResult Ping()
        {
            try
            {
                if (User.Identity.Name != null)
                {
                    return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { message = "expired" }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
    }
}