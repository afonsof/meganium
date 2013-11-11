using System.Diagnostics.CodeAnalysis;
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


    }
}
