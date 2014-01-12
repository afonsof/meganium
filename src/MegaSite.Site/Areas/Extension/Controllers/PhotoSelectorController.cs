﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dongle.Reflection;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Messaging;
using MegaSite.Api.Tools;
using MegaSite.Api.Trash;
using MegaSite.Api.ViewModels;
using MegaSite.Site.Areas.Extension.Models;

namespace MegaSite.Site.Areas.Extension.Controllers
{
    public class PhotoSelectorController : Controller
    {
        private readonly IManagers _managers;

        public PhotoSelectorController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Data(string hash)
        {
            var client = _managers.ClientManager.GetByHash(hash);
            if (client == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            var data = client.GetData<PhotoSelectorData>();
            var selected = data.SelectedMediaFiles ?? new List<MediaFile>();
            if (selected.Any())
            {
                //todo: colocar no resource as mensagens
                return Json(new Message("As fotos deste cliente já foram selecionadas", MessageType.Error), JsonRequestBehavior.AllowGet);
            }

            return Content(InternalJsonSerializer.Serialize(new
            {
                client.FullName,
                Hash = client.Code,
                data.AvailableMediaFiles,
                data.PhotoCount
            }));
        }

        [HttpPost]
        public ActionResult Data(string hash, string selectedMediaFilesJson)
        {
            var client = _managers.ClientManager.GetByHash(hash);
            var data = client.GetData<PhotoSelectorData>();
            data.SelectedMediaFiles = InternalJsonSerializer.Deserialize<List<MediaFile>>(selectedMediaFilesJson);

            var vm = ObjectFiller<Client, ClientEditVm>.Fill(client);
            vm.SetData(data);

            _managers.ClientManager.Change(vm);

            var body = "O cliente " + client.FullName + " finalizou a sua escolha de fotos\n";
            body += "Fotos:\n";
            body = data.SelectedMediaFiles.Aggregate(body, (current, selectedMediaFile) => current + ("\n+ " + selectedMediaFile.Title));

            Mailer.Send(client.FullName, client.Email, "Escolha de Fotos", body, _managers.License.Options.Get("PhotoSelectorEmailReporter"));

            //todo: tratar se não tiver cliente e erro de salvar
            return Json(new Message("Fotos enviadas com sucesso", MessageType.Success));
        }
    }
}
