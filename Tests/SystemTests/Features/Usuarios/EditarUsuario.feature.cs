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
namespace Meganium.SystemTests.Features.Usuarios
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Editar Usuário")]
    public partial class EditarUsuarioFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "EditarUsuario.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Editar Usuário", "Como administrador\r\nGostaria de editar um usuário\r\nDe modo que outra pessoa possa" +
                    " me ajudar administrar o site", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Editar usuário com sucesso")]
        public virtual void EditarUsuarioComSucesso()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editar usuário com sucesso", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("que estou logado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nome",
                        "Nome de usuário",
                        "E-mail"});
            table1.AddRow(new string[] {
                        "João",
                        "joao",
                        "joao@gmail.com"});
#line 9
 testRunner.And("os seguintes usuários existem", ((string)(null)), table1, "E ");
#line 12
 testRunner.When("entro na página \"/Admin/User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 13
 testRunner.And("clico em \"Editar\" do item \"joao\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 14
 testRunner.And("limpo o campo \"FullName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 15
 testRunner.And("digito \"João Editado\" no campo \"FullName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 16
 testRunner.And("clico no botão \"Salvar\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 17
 testRunner.Then("estou na página \"/Admin/User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line 18
 testRunner.And("deu uma mensagem de sucesso", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nome",
                        "Nome de usuário",
                        "E-mail"});
            table2.AddRow(new string[] {
                        "João Editado",
                        "joao",
                        "joao@gmail.com"});
#line 19
 testRunner.And("verifico que o seguinte item existe", ((string)(null)), table2, "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Editar usuario com campos vazios")]
        public virtual void EditarUsuarioComCamposVazios()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Editar usuario com campos vazios", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 24
 testRunner.Given("que estou logado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Nome",
                        "Nome de usuário",
                        "E-mail"});
            table3.AddRow(new string[] {
                        "João",
                        "joao",
                        "joao@gmail.com"});
#line 25
 testRunner.And("os seguintes usuários existem", ((string)(null)), table3, "E ");
#line 28
 testRunner.When("entro na página \"/Admin/User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 29
 testRunner.And("clico em \"Editar\" do item \"joao\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 30
 testRunner.And("limpo o campo \"FullName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 31
 testRunner.And("clico no botão \"Salvar\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 32
 testRunner.Then("verifico uma mensagem de erro \"Campo obrigatório\" para o campo \"FullName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
