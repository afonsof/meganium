using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dongle.Reflection;
using Dongle.Serialization;
using MegaSite.Api;
using MegaSite.Api.Entities;
using MegaSite.Api.Messaging;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;
using MegaSite.Api.Resources;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    [Authorize]
    public class PostController : BaseController
    {
        private readonly IManagers _managers;

        public PostController(IManagers managers)
        {
            _managers = managers;
        }

        public ViewResult Index(int? postTypeId)
        {
            var postType = _managers.PostTypeManager.GetById(postTypeId);

            return View(new PostIndexVm
            {
                PostType = postType,
                PostFields = _managers.FieldManager.Bind(postType.FieldsJson),
                Posts = _managers.PostManager.GetByPostType(postType.Id)
            });
        }

        public ActionResult Create(int? postTypeId)
        {
            var postType = _managers.PostTypeManager.GetById(postTypeId);
            var post = _managers.PostManager.Create(postType);
            return ViewPost(post);
        }

        public ActionResult Edit(int id)
        {
            var post = _managers.PostManager.GetById(id);
            return ViewPost(post);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(PostCreateEditVm vm, IEnumerable<int> categoryIds, int? postTypeId, FormCollection form, int? parentId)
        {
            var postType = _managers.PostTypeManager.GetById(postTypeId);
            var fields = _managers.FieldManager.Bind(postType.FieldsJson);
            var user = _managers.UserManager.GetByUserNameOrEmail(User.Identity.Name);
            var parent = _managers.PostManager.GetById(parentId);
            var fieldValues = _managers.FieldManager.FillDictionary(form, fields);

            if (!ModelState.IsValid)
            {
                return ReturnValidationError(vm, categoryIds, parentId, postType, fieldValues);
            }
            var post = ObjectFiller<PostCreateEditVm, Post>.Fill(vm);
            _managers.PostManager.CreateAndSave(post, user, postType, categoryIds, fieldValues, parent);
            return RedirectToAction("Index", "Post", new { postTypeId = postType.Id }, Resource.ItemSuccessfullyAdd, MessageType.Success);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PostCreateEditVm vm, IEnumerable<int> categoryIds, int? postTypeId, FormCollection form, int? parentId)
        {
            var postType = _managers.PostTypeManager.GetById(postTypeId);
            var fields = _managers.FieldManager.Bind(postType.FieldsJson);
            var user = _managers.UserManager.GetByUserNameOrEmail(User.Identity.Name);
            var parent = _managers.PostManager.GetById(parentId);
            var fieldValues = _managers.FieldManager.FillDictionary(form, fields);

            if (!ModelState.IsValid)
            {
                return ReturnValidationError(vm, categoryIds, parentId, postType, fieldValues);
            }
            var post = _managers.PostManager.GetById(vm.Id);
            ObjectFiller<PostCreateEditVm, Post>.Fill(vm, post);
            _managers.PostManager.Change(post, user, postType, parent, categoryIds, fieldValues);
            return RedirectToAction("Index", "Post", new { postTypeId = postType.Id }, Resource.ItemSuccessfullySaved, MessageType.Success);
        }

        public ActionResult Delete(int id)
        {
            var postType = _managers.PostManager.GetById(id).PostType;
            _managers.PostManager.Delete(id);
            return RedirectToAction("Index", "Post", new { postTypeId = postType.Id });
        }

        #region Private Methods

        private ViewResult ReturnValidationError(PostCreateEditVm vm, IEnumerable<int> categoryIds, int? parentId, PostType postType, Dictionary<string, string> fieldValues)
        {
            vm.PostType = postType;
            vm.Fields = _managers.FieldManager.Bind(postType.FieldsJson, JsonSimpleSerializer.SerializeToString(fieldValues));
            FillSelects(postType, vm, vm.Id, parentId, categoryIds);
            SetMessage(Resource.ThereAreValidationErrors, MessageType.Error);
            return View(GetViewName(postType), vm);
        }

        private ViewResult ViewPost(Post post)
        {
            var vm = ObjectFiller<Post, PostCreateEditVm>.Fill(post);
            vm.Fields = _managers.FieldManager.Bind(post.PostType.FieldsJson, post.FieldsValuesJson);
            FillSelects(post.PostType, vm, vm.Id, post.Parent == null ? null : (int?)post.Parent.Id, post.Categories.Select(c => c.Id));
            return View(GetViewName(post.PostType), vm);
        }

        private void FillSelects(IHaveId postType, PostCreateEditVm vm, int postId, int? postParentId, IEnumerable<int> selectedIds = null)
        {
            vm.PrivacySelect = new SelectList(new Dictionary<string, string> { { "false", Resource.Private }, { "true", Resource.Public } }, "Key", "Value");
            vm.CategoriesMultiselect = new MultiSelectList(_managers.CategoryManager.GetFromType(postType.Id), "Id", "Title", selectedIds);
            vm.ParentSelect = new SelectList(_managers.PostManager.GetWithoutParentFromType(postId, postType.Id), "Id", "Title", postParentId);
        }

        private static string GetViewName(PostType postType)
        {
            if (string.IsNullOrEmpty(postType.ViewName))
            {
                return "Default";
            }
            return postType.ViewName;
        }

        #endregion
    }
}