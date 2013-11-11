using System.Web.Mvc;
using Dongle.Reflection;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly IManagers _managers;

        public CategoryController(IManagers managers)
        {
            _managers = managers;
        }

        public ViewResult Index()
        {
            return View(_managers.CategoryManager.GetAll());
        }

        public ActionResult Create()
        {
            return ViewCategory(_managers.CategoryManager.Create());
        }
        
        public ActionResult Edit(int id)
        {
            var category = _managers.CategoryManager.GetById(id);
            return ViewCategory(category);
        }

        [HttpPost]
        public ActionResult Create(CategoryVm vm, int postTypeId)
        {
            if (!ModelState.IsValid) return ViewCategory(vm.Category);

            _managers.CategoryManager.CreateAndSave(vm.Category, postTypeId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(CategoryVm vm, int postTypeId)
        {
            if (!ModelState.IsValid) return ViewCategory(vm.Category);

            var category = _managers.CategoryManager.GetById(vm.Category.Id);
            ObjectFiller<Category, Category>.Merge(vm.Category, category);
            _managers.CategoryManager.Change(category, postTypeId);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _managers.CategoryManager.Remove(id);
            return RedirectToAction("Index");
        }

        private ActionResult ViewCategory(Category category)
        {
            var postTypes = _managers.PostTypeManager.GetWhatAllowsCategories();

            return View("CreateOrEdit", new CategoryVm
            {
                Category = category,
                PostTypeSelect = new SelectList(postTypes, "Id", "SingularName", category.PostType != null ? category.PostType.Id : 0)
            });
        }
    }
}