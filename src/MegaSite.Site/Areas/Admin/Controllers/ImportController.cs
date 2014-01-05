using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MegaSite.Api;
using MegaSite.Api.Managers;
using MegaSite.Api.Messaging;
using MegaSite.Api.Plugins;
using MegaSite.Api.Resources;
using MegaSite.Api.Tools;
using MegaSite.Api.ViewModels;
using MegaSite.Api.Web;
using MegaSite.Plugins.ContactForm;
using MegaSite.Plugins.FacebookPhotosImporter;

namespace MegaSite.Site.Areas.Admin.Controllers
{
    [Authorize]
    public class ImportController : BaseController
    {
        private readonly IManagers _managers;
        private const string FacebookOauthUrl = "https://www.facebook.com/dialog/oauth";
        private const string FacebookFormat = "{0}?client_id={1}&redirect_uri={2}?back=true&response_type=token&scope=user_photos,manage_pages,friends_photos";
        private const string FacebookScriptRedirect = "<script>window.location='{0}?' + window.location.hash.substr(1);</script>";

        public ImportController(IManagers managers)
        {
            _managers = managers;
        }

        public ViewResult Index()
        {
            var items = GetPlugins();
            var model = new ImportVm
            {
                Items = new SelectList(items.Select(i => new { i.Name, Title = Resource.ResourceManager.GetString(i.Name) }), "Name", "Title")
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ImportVm model)
        {
            try
            {
                if (model.PluginName == "FacebookPhotos")
                {
                    return FacebookImportAlbums();
                }
                var type = GetPlugins().FirstOrDefault(p => p.Name == model.PluginName);

                var plugin = Activator.CreateInstance(type) as IImportPlugin;
                return ImportPost(plugin);
            }
            catch (ArgumentException e)
            {
                return RedirectToAction("Index", "Erro: Parâmetro não especificado: " + e.ParamName, MessageType.Error);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Erro desconhecido: " + e.Message, MessageType.Error);
            }
        }

        public ActionResult FacebookImportAlbums()
        {
            var path = VirtualPathUtility.ToAbsolute("~");
            var baseurl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
            var root = baseurl + path;
            var redirectUrl = root + Url.Action("FacebookImportAlbums").Substring(1);

            if (Request["access_token"] == null)
            {
                if (Request["back"] == null)
                {
                    var facebookClientId = _managers.LicenseManager.GetOptions().Get("FacebookId");
                    var url = String.Format(FacebookFormat, FacebookOauthUrl, facebookClientId, redirectUrl);
                    return Redirect(url);
                }
                Response.Write(String.Format(FacebookScriptRedirect, redirectUrl));
                return null;
            }
            return ImportPost(new FacebookPhotos());
        }

        public ActionResult Import(List<int> itemCheckBox)
        {
            if (itemCheckBox == null)
            {
                return RedirectToAction("Index", Resource.NothingWasImported, MessageType.Warning);
            }
            var posts = Session["posts"] as List<ImportPost>;
            var importHandler = Session["importHandler"] as ImportHandler;
            var values = Session["values"] as NameValueCollection;
            var postType = _managers.PostTypeManager.GetByImportPluginType((ImportPluginType) Session["importPluginType"]);

            var user = _managers.UserManager.GetByUserNameOrEmail(User.Identity.Name);

            if (posts != null && importHandler != null)
            {
                importHandler.ImportSelectedPosts(itemCheckBox, posts, values, _managers, user, postType);
                return RedirectToAction("Index", String.Format(Resource.XItemsWasImported, itemCheckBox.Count()), MessageType.Success);
            }
            return RedirectToAction("Index", Resource.NothingWasImported, MessageType.Warning);
        }

        #region PrivateMethods

        private static IEnumerable<Type> GetPlugins()
        {
            var items = Assembly.GetAssembly(typeof(ContactForm))
                .GetTypes()
                .Where(p => typeof(IImportPlugin).IsAssignableFrom(p));
            return items;
        }

        private ActionResult ImportPost(IImportPlugin importPlugin)
        {
            ViewBag.Title = String.Format(Resource.XImporting, Resource.ResourceManager.GetString(importPlugin.GetType().Name));

            var importHandler = new ImportHandler(importPlugin);

            var values = new NameValueCollection(Request.QueryString) { Request.Form };
            var posts = importHandler.ReadAndCheckPosts(values, _managers);

            Session["importPluginType"] = importPlugin.Type;
            Session["values"] = values;
            Session["importHandler"] = importHandler;
            Session["posts"] = posts;
            return View("Import", posts);
        }
        #endregion
    }
}