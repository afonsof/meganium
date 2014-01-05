using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MegaSite.Api.Entities;
using MegaSite.SystemTests.Tools;
using TechTalk.SpecFlow;

namespace MegaSite.SystemTests.Steps
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
                    Title = "Post do " + user.FullName
                });
                TestToolkit.Uow.Commit();
            }
        }

        [Given(@"que o existe um tipo de objeto com todos os comportamentos")]
        public void DadoQueOExisteUmTipoDeObjetoComTodosOsComportamentos()
        {
            if (!TestToolkit.Uow.PostTypeRepository.AsQueryable().Any(p => p.SingularName == "Receita"))
            {
                var postType = new PostType
                {
                    SingularName = "Receita",
                    PluralName = "Receitas",
                    IconId = "book"
                };
                postType.SetBehavior(
                    PostType.BehaviorFlags.AllowCategories |
                    PostType.BehaviorFlags.AllowComments |
                    PostType.BehaviorFlags.AllowDescription |
                    PostType.BehaviorFlags.AllowExternalId |
                    PostType.BehaviorFlags.AllowHash |
                    PostType.BehaviorFlags.AllowLocation |
                    PostType.BehaviorFlags.AllowMarkAsFeatured |
                    PostType.BehaviorFlags.AllowPhotos |
                    PostType.BehaviorFlags.AllowTimeBox |
                    PostType.BehaviorFlags.AllowTree |
                    PostType.BehaviorFlags.AllowVideo);

                TestToolkit.Uow.PostTypeRepository.Add(postType);
                TestToolkit.Uow.Commit();

                TestToolkit.Managers.LicenseManager.GetOptions().Set("DefaultPostTypeId", postType.Id);
            }
        }

        [Given(@"as seguintes postagens existem")]
        public void DadoAsSeguintesPostagensExistem(Table table)
        {
            foreach (var row in table.Rows)
            {
                if (!TestToolkit.Uow.PostRepository.AsQueryable().Any(p => p.Title == row["Título"]))
                {
                    TestToolkit.Uow.PostRepository.Add(new Post
                    {
                        Title = row["Título"],
                        IsFeatured = row.ContainsKey("Destaque") && row["Destaque"] == "Sim",
                        Published = true
                    });
                    TestToolkit.Uow.Commit();
                }
            }
        }

    }
}