using System;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using Dongle.System.IO;
using Ionic.Zip;
using MegaSite.Api;
using MegaSite.Api.Entities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace MegaSite.SystemTests.Tools
{
    [ExcludeFromCodeCoverage]
    public static class TestToolkit
    {
        private static bool _logged;
        private static readonly int Port = new Random().Next(10000, 65000);
        private static Process _webserver;
        private static IWebDriver _driver;
        private static UnitOfWork _bundle;

        public static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    var dir = new DirectoryInfo(ApplicationPaths.RootDirectory);
                    var srcDir = dir.Closest("src");
                    var siteRoot = Path.Combine(srcDir.FullName, "MegaSite.Site");

                    const string process =
                        @"""C:\Program Files (x86)\Common Files\Microsoft Shared\DevServer\11.0\WebDev.WebServer40.exe""";
                    var args = @"/port:" + Port +
                               @" /nodirlist /path:""" + siteRoot + @""" /vpath:""/""";
                    _webserver = Process.Start(process, args);

                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--start-maximized");

                    var path = ApplicationPaths.RootDirectory + "\\chromedriver.exe";
                    var fileName = ApplicationPaths.RootDirectory + "\\chromedriver.zip";
                    if (!File.Exists(path))
                    {
                        var client = new WebClient();
                        client.DownloadFile("http://chromedriver.storage.googleapis.com/2.6/chromedriver_win32.zip", fileName);
                        var zip = new ZipFile(fileName);
                        zip.ExtractAll(ApplicationPaths.RootDirectory);
                    }

                    _driver = new ChromeDriver(chromeOptions);
                }
                return _driver;
            }
        }

        public static IRepositories Uow
        {
            get { return _bundle ?? (_bundle = new UnitOfWork(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)); }
        }

        public static IManagers Managers
        {
            get { return _bundle ?? (_bundle = new UnitOfWork(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)); }
        }

        private static string MountUrl(string page)
        {
            return "http://localhost:" + Port + page;
        }

        public static void Navigate(string page)
        {
            Driver.Navigate().GoToUrl(MountUrl(page));
        }

        public static string PageHtml()
        {
            return Driver.PageSource;
        }

        private static bool PageIs(string page)
        {
            var rawUrl = Driver.Url;
            var index = rawUrl.IndexOf("?", StringComparison.InvariantCulture);
            if (index > -1)
            {
                rawUrl = rawUrl.Remove(index);
            }
            var pageIs = rawUrl == MountUrl(page);

            if (!rawUrl.Contains("Login"))
            {
                _logged = true;
            }
            return pageIs;
        }

        public static void AssertPageIs(string page)
        {
            var rawUrl = Driver.Url;
            var index = rawUrl.IndexOf("?", StringComparison.InvariantCulture);
            if (index > -1)
            {
                rawUrl = rawUrl.Remove(index);
            }

            if (!rawUrl.Contains("Login"))
            {
                _logged = true;
            }
            Assert.AreEqual(rawUrl, MountUrl(page));
        }

        public static void ClickLink(string name)
        {
            var element = Driver.FindElement(By.PartialLinkText(name));
            element.Click();
        }

        public static void ClickButton(string name)
        {
            IWebElement element;
            try
            {
                element = Driver.FindElement(By.Name(name));
            }
            catch
            {
                var btns = Driver.FindElements(By.ClassName("btn"));
                element = btns.First(e => e.Text == name);
            }
            element.Click();
        }

        public static void TypeInField(string text, string name)
        {
            FindElementByNameOrLabelName(name).SendKeys(text);
        }

        private static IWebElement FindElementByNameOrLabelName(string name)
        {
            IWebElement element;
            try
            {
                element = Driver.FindElement(By.Name(name));
            }
            catch
            {
                var label = Driver.FindElement(By.XPath("//label[text()='"+ name + "']"));
                name = label.GetAttribute("for");
                element = Driver.FindElement(By.Name(name));
            }
            return element;
        }

        public static void EnsureLoggedOut()
        {
            if (!_logged)
            {
                return;
            }
            Navigate("/Admin/Account/Logout/");
        }

        public static void EnsureLoggedIn()
        {
            if (_logged)
            {
                return;
            }
            Navigate("/Admin/Account/Login");
            Driver.FindElement(By.Name("Email")).SendKeys("teste@megasiteapp.com.br");
            Driver.FindElement(By.Name("Password")).SendKeys("teste123");
            Driver.FindElement(By.TagName("form")).Submit();
            if (PageIs("/Admin"))
            {
                _logged = true;
                return;
            }
            throw new Exception("Não consegui garantir que o usuário está logado");
        }

        public static bool FieldIsVisible(string name)
        {
            return Driver.FindElement(By.Name(name)).Displayed;
        }

        public static void CheckField(string name)
        {
            var element = Driver.FindElement(By.Name(name));
            if (element.Selected == false)
            {
                element.Click();
            }
        }

        public static bool RowExistsInTable(TableRow row)
        {
            var div = Driver.FindElement(By.ClassName("row"));
            var table2 = div.FindElement(By.TagName("table"));
            var trs = table2.FindElements(By.TagName("tr"));

            var titleThs = trs[0].FindElements(By.TagName("th"));
            for (var j = 0; j < row.Count; j++)
            {
                Assert.AreEqual(row.Keys.ToList()[j], titleThs[j].Text);
            }

            var found = trs.Any(tr =>
                                {
                                    var tds = tr.FindElements(By.TagName("td"));
                                    if (tds.Count < row.Count)
                                    {
                                        return false;
                                    }
                                    var i = 0;
                                    var equal = true;
                                    foreach (var item in row)
                                    {
                                        var itemEqual = tds[i].Text == item.Value;
                                        if (!itemEqual)
                                        {
                                            try
                                            {
                                                var img = tds[i].FindElement(By.TagName("img"));
                                                itemEqual = img.GetAttribute("title") == item.Value;
                                            }
                                            catch
                                            {
                                            }
                                        }

                                        equal &= itemEqual;
                                        i++;
                                    }
                                    return equal;
                                });
            return found;
        }

        public static bool ErrorForFieldShowed(string field)
        {
            var element = FindElementByNameOrLabelName(field);
            return Driver.FindElement(By.CssSelector("span[for='" + element.GetAttribute("name") + "']")).Displayed;
        }

        public static void AssertGlobalMessageShowed(string type, string message = null)
        {
            IWebElement element = null;
            try
            {
                element = Driver.FindElement(By.ClassName("alert-" + type.ToLower()));
            }
            catch
            {
            }
            Assert.IsNotNull(element);
            Assert.IsTrue(element.Displayed, "Element must be displayed");
            if (message != null)
            {
                Assert.IsTrue(element.Text.EndsWith(message), "Message \"" + message + "\" must be found");
            }
        }

        public static void EnsureUserDoesntExist(string userName)
        {
            var user = Uow.UserRepository
                    .AsQueryable()
                    .FirstOrDefault(u => u.UserName == userName);
            if (user != null)
            {
                Uow.UserRepository.Remove(user.Id);
                Uow.Commit();
            }
        }

        public static void EnsureUserExists(string name, string userName, string email)
        {
            var user = Uow.UserRepository
                    .AsQueryable()
                    .FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                Uow.UserRepository.Add(new User
                                              {
                                                  Email = email,
                                                  UserName = userName,
                                                  FullName = name
                                              });
                Uow.Commit();
            }
        }

        public static void EnsureClientExists(string name, string email)
        {
            var client = Uow.ClientRepository
                    .AsQueryable()
                    .FirstOrDefault(u => u.FullName == name);
            if (client != null) return;
            Uow.ClientRepository.Add(new Client
                                            {
                                                Email = email,
                                                FullName = name
                                            });
            Uow.Commit();
        }

        public static void Dispose()
        {
            Uow.Dispose();
            Driver.Quit();
            _webserver.Kill();
        }

        public static string FieldValue(string field)
        {
            return Driver.FindElement(By.Name(field)).GetAttribute("Value");
        }

        public static void ClickLinkInTableItem(string linkText, string item)
        {
            var div = Driver.FindElement(By.ClassName("row"));
            var table2 = div.FindElement(By.TagName("table"));
            var trs = table2.FindElements(By.TagName("tr"));

            var trFound = trs.FirstOrDefault(tr =>
            {
                var tds = tr.FindElements(By.TagName("td"));
                if (tds.Any(td => td.Text == item))
                {
                    return true;
                }
                return false;
            });
            if (trFound != null)
            {
                var el = trFound.FindElement(By.PartialLinkText(linkText));
                el.Click();
                return;
            }
            Assert.Fail("Can't find the link named \"" + linkText + "\"");
        }

        public static void CleanField(string name)
        {
            FindElementByNameOrLabelName(name).Clear();
        }
    }
}