﻿using System.Web.Mvc;
using Dongle.Reflection;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Messaging;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;
using MegaSite.Api.Resources;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IManagers _managers;

        public UserController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Index()
        {
            return View(_managers.UserManager.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                SetMessage(Resource.ThereAreValidationErrors, MessageType.Error);
                return View(vm);
            }

            var user = ObjectFiller<UserCreateVm, User>.Fill(vm);
            var message = _managers.UserManager.CreateAndSave(user);
            if (message.Type == MessageType.Error)
            {
                SetMessage(message);
                return View(vm);
            }
            return RedirectToAction("Index", message);
        }

        public ActionResult Edit(int id)
        {
            var user = _managers.UserManager.GetById(id);
            var vm = ObjectFiller<User, UserEditVm>.Fill(user);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(UserEditVm vm)
        {
            if (!ModelState.IsValid)
            {
                SetMessage(Resource.ThereAreValidationErrors, MessageType.Error);
                return View(vm);
            }
            var message = _managers.UserManager.Change(vm);
            return RedirectToAction("Index", message);
        }

        public ActionResult Delete(int id)
        {
            var message = _managers.UserManager.Remove(id);
            return RedirectToAction("Index", message);
        }
    }
}