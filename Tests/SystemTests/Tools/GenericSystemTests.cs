using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Meganium.Api.Entities;
using Meganium.Api.Web;
using Meganium.Site.Areas.Admin.Controllers;
using NUnit.Framework;

namespace Meganium.SystemTests.Tools
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class GenericSystemTests
    {
        [Test]
        public void AccessAllAdminPages()
        {
            var urls = new Dictionary<string, string>
                       {
                           {"/", ""},
                           {"/Rss", ""}
                       };
            var classes = typeof(BasicController).Assembly
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseController)) && type.Namespace.StartsWith("Meganium.Site.Areas.Admin.Controllers"))
                .Where(t => t.Name != "AccountController" && t.Name != "InstallController");

            foreach (var @class in classes)
            {
                var methods =
                    @class.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(m => !m.GetCustomAttributes(typeof(HttpPostAttribute), true).Any() && !m.Name.Contains("Import"));

                foreach (var methodInfo in methods)
                {
                    var controllerName = @class.Name.Replace("Controller", "");
                    urls.Add("/Admin/" + controllerName + "/" + methodInfo.Name, controllerName);
                }
            }
            var res = new Dictionary<string, string>();

            TestToolkit.EnsureLoggedIn();

            foreach (var url in urls)
                {
                    var urlValue = url.Key;

                    if (url.Key.Contains("Edit") || url.Key.Contains("Delete"))
                    {
                        var entityName = url.Value;
                        var entity = typeof(IHaveId).Assembly.GetTypes().FirstOrDefault(type => type.Name == entityName);
                        var obj = Activator.CreateInstance(entity, null) as IHaveId;

                        var repositoryProperty = TestToolkit.Uow.GetType().GetProperty(entityName + "Repository");
                        var repository = repositoryProperty.GetValue(TestToolkit.Uow, null);
                        var addMethod = repository.GetType().GetMethod("Add");
                        addMethod.Invoke(repository, new object[] {obj});
                        TestToolkit.Uow.Commit();

                        urlValue += "/" + obj.Id;
                    }
                    TestToolkit.Navigate(urlValue);
                    var page = TestToolkit.PageHtml();
                    if (page.Contains("Exception:"))
                    {
                        res.Add(urlValue, Regex.Match(page, @"\((\d+)\)").Value);
                    }
            }
            if (res.Count > 0)
            {
                var output = "";
                foreach (var re in res)
                {
                    output += re.Key + ":" + re.Value + "\n";
                }
                Assert.Fail(output);
            }
        }
    }
}
