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
namespace MegaSite.SystemTests.Features.Importacao
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Importar Videos do Vimeo")]
    public partial class ImportarVideosDoVimeoFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ImportarVideosDoVimeo.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Importar Videos do Vimeo", "Como administrador do site\r\nGostaria de importar meus vídeos do Vimeo\r\nDe modo qu" +
                    "e os usuários finais os visualizem", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Importar Videos do Vimeo com sucesso")]
        public virtual void ImportarVideosDoVimeoComSucesso()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Importar Videos do Vimeo com sucesso", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("que estou logado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 9
 testRunner.When("entro na página \"/Admin/Import\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 10
 testRunner.And("seleciono \"Vídeos do Vimeo\" no campo \"PluginName\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 11
 testRunner.And("digito \"sonhartphotoevideo\" no campo \"username\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 12
 testRunner.And("clico no botão \"Avançar\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 13
 testRunner.And("clico no botão \"Importar tudo\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 14
 testRunner.Then("estou na página \"/Admin/Import\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line 15
 testRunner.And("deu uma mensagem de sucesso", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
