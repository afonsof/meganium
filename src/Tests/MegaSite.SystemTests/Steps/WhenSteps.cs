using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Mvc;
using Dongle.System.IO;
using MegaSite.SystemTests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace MegaSite.SystemTests.Steps
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
            button.Click();

            var modal = TestToolkit.Driver.FindElement(By.ClassName("media-picker-modal"));
            var button2 = modal.FindElement(By.ClassName("uploadifive-button"));
            var input = button2.FindElements(By.TagName("input"));

            var js = (IJavaScriptExecutor)TestToolkit.Driver;
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", input[1], "opacity: 1; position: absolute; z-index: 999; left: 0; top: 0");
        
            input[1].SendKeys(ApplicationPaths.RootDirectory + "\\TestData\\" + filename);
            var buttonOk = modal.FindElement(By.ClassName("btn-ok"));
            Thread.Sleep(1000);
            buttonOk.Click();
            Thread.Sleep(1000);
        }
    }
}
