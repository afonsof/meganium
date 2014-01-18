using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Dongle.System.IO;
using Meganium.SystemTests.Tools;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace Meganium.SystemTests.Steps
{
    [Binding]
    [ExcludeFromCodeCoverage]
    class WhenSteps
    {
        [When(@"digito ""(.*)"" no campo ""(.*)""")]
        public void QuandoDigitoNoCampo(string text, string name)
        {
            TestToolkit.TypeInField(text, name);
        }

        [When(@"clico no link ""(.*)""")]
        public void QuandoClicoNoLink(string name)
        {
            TestToolkit.ClickLink(name);
        }

        [When(@"clico no botão ""(.*)""")]
        public void QuandoClicoNoBotao(string name)
        {
            TestToolkit.ClickButton(name);
        }

        [When(@"entro na página ""(.*)""")]
        public void QuandoEntroNaPagina(string page)
        {
            TestToolkit.Navigate(page);
        }

        [When(@"marco a caixa de seleção ""(.*)""")]
        public void QuandoMarcoACaixaDeSelecao(string name)
        {
            TestToolkit.CheckField(name);
        }

        [When(@"seleciono ""(.*)"" no campo ""(.*)""")]
        public void QuandoSelecionoNoCampo(string p0, string p1)
        {
            var element = new SelectElement(TestToolkit.Driver.FindElement(By.Name(p1)));
            element.SelectByText(p0);
        }

        [When(@"limpo o campo ""(.*)""")]
        public void QuandoLimpoOCampo(string name)
        {
            TestToolkit.CleanField(name);
        }

        [When(@"clico em ""(.*)"" do item ""(.*)""")]
        public void QuandoClicoEmEditarDoItem(string linkText, string item)
        {
            TestToolkit.ClickLinkInTableItem(linkText, item);
        }

        [When(@"clico OK no alerta")]
        public void QuandoClicoOkNoAlerta()
        {
            TestToolkit.Driver.SwitchTo().Alert().Accept();
        }

        [When(@"clico Cancelar no alerta")]
        public void QuandoClicoCancelarNoAlerta()
        {
            TestToolkit.Driver.SwitchTo().Alert().Dismiss();
        }

        [When(@"insiro a imagem ""(.*)""")]
        public void QuandoInsiroAImagem(string filename)
        {
            var button = TestToolkit.Driver.FindElement(By.ClassName("media-file-picker-control"));
            TestToolkit.ScrollAndClick(button);

            var modal = TestToolkit.Driver.FindElement(By.ClassName("media-picker-modal"));
            var button2 = modal.FindElement(By.ClassName("uploadifive-button"));
            var input = button2.FindElements(By.TagName("input"));

            var js = (IJavaScriptExecutor)TestToolkit.Driver;
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", input[1], "opacity: 1; position: absolute; z-index: 999; left: 0; top: 0");

            input[1].SendKeys(ApplicationPaths.RootDirectory + "\\TestData\\" + filename);
            var buttonOk = modal.FindElement(By.ClassName("btn-ok"));
            Thread.Sleep(1000);
            TestToolkit.ScrollAndClick(buttonOk);
            Thread.Sleep(1000);
        }

        [When(@"insiro a imagem ""(.+?)"" (\d+) vezes")]
        public void QuandoInsiroAImagemVezes(string filename, int times)
        {
            var dirPath = ApplicationPaths.RootDirectory + "TestData\\";
            var originalFile = dirPath + filename;
            var button = TestToolkit.Driver.FindElement(By.ClassName("uploadifive-button"));
            var inputs = button.FindElements(By.TagName("input"));
            // Mas tive que fazer assim:
            inputs[1].Click();
            SendKeys.SendWait(dirPath);
            SendKeys.SendWait("{ENTER}");
            //
            for (var i = 0; i < times; ++i)
            {
                FileSystem.CopyFile(originalFile, dirPath + i + ".jpg", true);
                // Deveria ser assim:
                //inputs[1].SendKeys(dirPath + i + ".jpg");
                // Mas tive que fazer assim:
                SendKeys.SendWait("\"" + i + ".jpg\" ");
                //
            }
            SendKeys.SendWait("{ENTER}");
        }

        [When(@"limpo a seleção da caixa de multiseleção")]
        public void QuandoLimpoASelecaoDaCaixaDeMultiselecao()
        {
            var list = TestToolkit.Driver.FindElement(By.XPath("//*[contains(@class, 'ms-selection')]/ul"));
            var elements = list.FindElements(By.TagName("li"));
            foreach (var element in elements)
            {
                if (element.Displayed)
                {
                    element.Click();
                }
            }
        }

        [When(@"seleciono os itens na caixa de multiseleção")]
        public void QuandoSelecionoOsItensNaCaixaDeMultiselecao(Table table)
        {
            var list = TestToolkit.Driver.FindElement(By.XPath("//*[contains(@class, 'ms-selectable')]/ul"));
            var elements = list.FindElements(By.TagName("li"));
            var count = 0;

            foreach (var element in elements)
            {
                if (table.Rows.Any(r => element.Text.EndsWith(r["Título"])) && element.Displayed)
                {
                    element.Click();
                    count++;
                }
            }
            Assert.AreEqual(count, table.Rows.Count);
        }

    }


}
