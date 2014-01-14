using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Meganium.SystemTests.Tools;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Meganium.SystemTests.Steps
{
    [Binding]
    [ExcludeFromCodeCoverage]
    public class ThenSteps
    {
        [Then(@"estou na página ""(.*)""")]
        public void EstouNaPagina(string page)
        {
            TestToolkit.AssertPageIs(page);
        }

        [Then(@"verifico que o seguinte item existe")]
        public void EntaoVerificoQueOSeguinteItemExiste(Table table)
        {
            Assert.IsTrue(TestToolkit.RowExistsInTable(table.Rows[0]));
        }

        [Then(@"verifico uma mensagem de erro ""(.*)"" para o campo ""(.*)""")]
        public void EntaoVerificoUmaMensagemDeErroParaOCampo(string message, string field)
        {
            Assert.IsTrue(TestToolkit.ErrorForFieldShowed(field));
        }

        [Then(@"deu uma mensagem de sucesso")]
        public void EntaoDeuUmaMensagemDeSucesso()
        {
            TestToolkit.AssertGlobalMessageShowed("success");
        }

        [Then(@"verifico uma mensagem de erro global ""(.*)""")]
        public void EntaoDeuUmaMensagemDeErro(string message)
        {
            TestToolkit.AssertGlobalMessageShowed("error", message);
        }

        [Then(@"o valor do campo ""(.*)"" é ""(.*)""")]
        public void EntaoOValorDoCampoE(string field, string value)
        {
            var curValue = TestToolkit.FieldValue(field);
            Assert.AreEqual(value, curValue);
        }

        [Then(@"o valor do campo ""(.*)"" está vazio")]
        public void EntaoOValorDoCampoEstaVazio(string field)
        {
            Assert.IsNull(TestToolkit.FieldValue(field));
        }

        [Then(@"os seguintes usuários não existem")]
        public void EntaoOsSeguintesUsuariosNaoExistem(Table table)
        {
            var exists = false;
            foreach (var row in table.Rows)
            {
                exists |= TestToolkit.Uow
                    .UserRepository
                    .AsQueryable()
                    .Any(u => u.UserName == row["Nome de usuário"]);
            }
            Assert.IsFalse(exists);
        }

        [Then(@"os seguintes usuários existem")]
        public void EntaoOsSeguintesUsuariosExistem(Table table)
        {
            var exists = true;
            foreach (var row in table.Rows)
            {
                exists &= TestToolkit.Uow
                    .UserRepository
                    .AsQueryable()
                    .Any(u => u.UserName == row["Nome de usuário"]);
            }
            Assert.IsTrue(exists);
        }
    }
}
