﻿using System.Linq;
using System.Web.Mvc;
using Dongle.Reflection;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Messaging;
using MegaSite.Api.Tools;
using MegaSite.Api.ViewModels;

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

            if (client.SelectedMediaFiles.Any())
            {
                //todo: colocar no resource as mensagens
                return Json(new Message("As fotos deste cliente já foram selecionadas", MessageType.Error), JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                client.FullName,
                client.Hash,
                client.AvailableMediaFiles,
                client.PhotoCount
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Data(string hash, string selectedMediaFilesJson)
        {
            var client = _managers.ClientManager.GetByHash(hash);
            client.SelectedMediaFilesJson = selectedMediaFilesJson;

            var vm = ObjectFiller<Client, ClientEditVm>.Fill(client);
            _managers.ClientManager.Change(vm);

            var body = "O cliente " + client.FullName + " finalizou a sua escolha de fotos\n";
            body += "Fotos:\n";

            body = client.SelectedMediaFiles.Aggregate(body, (current, selectedMediaFile) => current + ("+ " + selectedMediaFile.Title));

            Mailer.Send(client.FullName, client.Email, "Escolha de Fotos", body);

            //todo: tratar se não tiver cliente e erro de salvar
            return Json(new Message("Fotos enviadas com sucesso", MessageType.Success));
        }
    }
}
