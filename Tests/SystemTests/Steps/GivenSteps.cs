using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Meganium.Api.Entities;
using Meganium.SystemTests.Tools;
using TechTalk.SpecFlow;

namespace Meganium.SystemTests.Steps
{
    [Binding]
    [ExcludeFromCodeCoverage]
    class GivenSteps
    {
        [Given(@"usuário ""(.*)"" não existe")]
        public void DadoUsuarioNaoExiste(string userEmail)
        {
            TestToolkit.EnsureUserDoesntExist(userEmail);
        }

        [Given(@"que o tipo de objeto ""(.*)"" não existe")]
        public void DadoQueOTipoDeObjetoNaoExiste(string singularName)
        {
            PostType postType = null;
            do
            {
                postType = TestToolkit.Uow.PostTypeManager.GetBySingularName(singularName);
                if (postType != null)
                {
                    TestToolkit.Uow.PostTypeRepository.Remove(postType);
                    TestToolkit.Uow.Commit();
                }
            } while (postType != null);
        }

        [Given(@"cliente ""(.*?)"" existe")]
        public void DadoClienteExiste(string name)
        {
            TestToolkit.EnsureClientExists(name, "nao@existe.com");
        }

        [Given(@"que estou logado")]
        public void DadoQueEstouLogado()
        {
            TestToolkit.EnsureLoggedIn();
        }

        [Given(@"entro na página ""(.*)""")]
        public void DadoEntroNaPagina(string page)
        {
            TestToolkit.Navigate(page);
        }

        [Given(@"que estou deslogado")]
        public void DadoQueEstouDeslogado()
        {
            TestToolkit.EnsureLoggedOut();
        }

        [Given(@"sou redirecionado para a tela de login")]
        public void DadoSouRedirecionadoParaATelaDeLogin()
        {
            TestToolkit.AssertPageIs("/Admin/Account/Login");
        }

        [Given(@"os seguintes usuários existem")]
        public void DadoOsSeguintesUsuariosExistem(Table table)
        {
            foreach (var row in table.Rows)
            {
                TestToolkit.EnsureUserExists(row["Nome"], row["Nome de usuário"], row["E-mail"]);
            }
        }

        [Given(@"o usuário ""(.*)"" tem um post")]
        public void DadoOUsuarioTemUmPost(string userName)
        {
            var user = TestToolkit.Uow.UserRepository.AsQueryable().FirstOrDefault(p => p.UserName == userName);
            if (!TestToolkit.Uow.PostRepository.AsQueryable().Any(p => p.CreatedBy == user))
            {
                TestToolkit.Uow.PostRepository.Add(new Post
                {
                    CreatedBy = user,
                    Title = "Post do " + user.FullName,
                    PostType = TestToolkit.Uow.PostTypeRepository.GetById(PostTypeIdDefault.Get())
                });
                TestToolkit.Uow.Commit();
            }
        }

        [Given(@"que o existe um tipo de objeto com todos os comportamentos")]
        public void DadoQueOExisteUmTipoDeObjetoComTodosOsComportamentos()
        {
            PostTypeIdDefault.Get();
        }

        [Given(@"as seguintes postagens existem")]
        public void DadoAsSeguintesPostagensExistem(Table table)
        {
            foreach (var row in table.Rows)
            {
                var postType = TestToolkit.Uow.PostTypeRepository.GetById(PostTypeIdDefault.Get());

                if (!TestToolkit.Uow.PostRepository.AsQueryable().Any(p => p.Title == row["Título"] && p.PostType == postType))
                {
                    TestToolkit.Uow.PostRepository.Add(new Post
                    {
                        Title = row["Título"],
                        IsFeatured = row.ContainsKey("Destaque") && row["Destaque"] == "Sim",
                        Published = true,
                        PostType = postType

                    });
                    TestToolkit.Uow.Commit();
                }
            }
        }

    }
}