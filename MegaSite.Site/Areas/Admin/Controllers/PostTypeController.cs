using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dongle.Reflection;
using Dongle.Serialization;
using Dongle.System;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    [Authorize]
    public class PostTypeController : BaseController
    {
        private readonly IManagers _managers;

        public PostTypeController(IManagers managers)
        {
            _managers = managers;
        }

        public ActionResult Index()
        {
            return View(_managers.PostTypeManager.GetAll());
        }

        public ActionResult Create()
        {
            var vm = new PostTypeCreateEditVm();
            FillSelects(vm);
            return View("CreateOrEdit", vm);
        }

        public ActionResult Edit(int id)
        {
            var postType = _managers.PostTypeManager.GetById(id);
            var vm = ObjectFiller<PostType, PostTypeCreateEditVm>.Fill(postType);
            FillSelects(vm, postType);
            return View("CreateOrEdit", vm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(PostTypeCreateEditVm vm)
        {
            var postType = ObjectFiller<PostTypeCreateEditVm, PostType>.Fill(vm);
            _managers.PostTypeManager.CreateAndSave(vm, postType);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PostTypeCreateEditVm vm)
        {
            var postType = _managers.PostTypeManager.GetById(vm.Id);
            ObjectFiller<PostTypeCreateEditVm, PostType>.Merge(vm, postType);
            _managers.PostTypeManager.Change(vm, postType);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var message = _managers.PostTypeManager.Delete(id);
            return RedirectToAction("Index", message);
        }

        #region Private Methods

        private static void FillSelects(PostTypeCreateEditVm vm, PostType postType = null)
        {
            var enumItems = Enum.GetValues(typeof(PostType.BehaviorFlags)).Cast<PostType.BehaviorFlags>().Select(a => a.ToString("F"));
            List<string> selectedValues;
            if (postType != null)
            {
                selectedValues = postType.Behavior.ToString("F").Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            else
            {
                selectedValues = new List<string>();
            }
            vm.BehaviorsMultiselect = new MultiSelectList(enumItems, selectedValues);
        }

        #endregion

    }
}