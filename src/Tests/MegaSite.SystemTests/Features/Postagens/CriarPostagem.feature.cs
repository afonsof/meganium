﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34003
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace MegaSite.SystemTests.Features.Postagens
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Criar postagem")]
    public partial class CriarPostagemFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CriarPostagem.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Criar postagem", "Como administrador do site\r\nGostaria de criar um objeto\r\nDe modo que os usuários " +
                    "finais o visualize", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Criar postagem inserindo imagem com sucesso")]
        public virtual void CriarPostagemInserindoImagemComSucesso()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Criar postagem inserindo imagem com sucesso", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("que estou logado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 9
 testRunner.And("que o existe um tipo de objeto com todos os comportamentos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 10
 testRunner.When("entro na página \"/Admin/Post/Create\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 11
 testRunner.And("digito \"Receita de bala de coco\" no campo \"Título\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 12
 testRunner.And("insiro a imagem \"bala-de-coco.jpg\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 13
 testRunner.And("clico no botão \"Salvar\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 14
 testRunner.Then("estou na página \"/Admin\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line 15
 testRunner.And("deu uma mensagem de sucesso", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "",
                        "Título",
                        "Autor"});
            table1.AddRow(new string[] {
                        "bala-de-coco.jpg",
                        "Receita de bala de coco",
                        "Usuário de Teste"});
#line 16
 testRunner.And("verifico que o seguinte item existe", ((string)(null)), table1, "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Criar postagem com campos vazios")]
        public virtual void CriarPostagemComCamposVazios()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Criar postagem com campos vazios", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
 testRunner.Given("que estou logado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 22
 testRunner.And("que o existe um tipo de objeto com todos os comportamentos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 23
 testRunner.When("entro na página \"/Admin/Post/Create\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 24
 testRunner.And("clico no botão \"Salvar\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 25
 testRunner.Then("estou na página \"/Admin/Post/Create\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line 26
 testRunner.And("verifico uma mensagem de erro \"Campo obrigatório\" para o campo \"Título\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
