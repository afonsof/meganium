using System.Collections.Generic;
using System.Web.Mvc;
using Dongle.Reflection;
using Dongle.Serialization;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Messaging;
using MegaSite.Api.Trash;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;
using MegaSite.Api.Resources;
using MegaSite.Site.Areas.Extension.Models;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    [Authorize]
    public class ClientController : BaseController
    {
        private readonly IManagers _managers;

        public ClientController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Index()
        {
            return View(_managers.ClientManager.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ClientCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                SetMessage(Resource.ThereAreValidationErrors, MessageType.Error);
                return View(vm);
            }

            var client = ObjectFiller<ClientCreateVm, Client>.Fill(vm);
            var message = _managers.ClientManager.CreateAndSave(client);
            if (message.Type == MessageType.Error)
            {
                SetMessage(message);
                return View(vm);
            }
            return RedirectToAction("Index", message);
        }

        public ActionResult Edit(int id)
        {
            var client = _managers.ClientManager.GetById(id);
            var vm = ObjectFiller<Client, ClientEditVm>.Fill(client);

            var data = client.GetData<PhotoSelectorData>();
            vm.SelectedMediaFiles = data.SelectedMediaFiles ?? new List<MediaFile>();
            vm.AvailableMediaFilesJson = InternalJsonSerializer.Serialize(data.AvailableMediaFiles ?? new List<MediaFile>());
            vm.PhotoCount = data.PhotoCount;
            return View(vm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ClientEditVm vm)
        {
            if (!ModelState.IsValid)
            {
                SetMessage(Resource.ThereAreValidationErrors, MessageType.Error);
                return View(vm);
            }

            var photoSelectorData = new PhotoSelectorData
            {
                PhotoCount = vm.PhotoCount,
                AvailableMediaFiles =
                    InternalJsonSerializer.Deserialize<List<MediaFile>>(vm.AvailableMediaFilesJson)
            };
            vm.DataJson = InternalJsonSerializer.Serialize(photoSelectorData);

            var message = _managers.ClientManager.Change(vm);
            return RedirectToAction("Index", message);
        }

        public ActionResult Delete(int id)
        {
            var message = _managers.ClientManager.Remove(id);
            return RedirectToAction("Index", message);
        }
    }
}