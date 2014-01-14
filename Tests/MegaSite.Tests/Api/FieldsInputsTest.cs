using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MegaSite.Api.Entities;
using MegaSite.Api.Trash;
using Moq;
using NUnit.Framework;

namespace MegaSite.Tests.Api
{
    [TestFixture]
    public class FieldsInputsTest
    {
        private readonly HtmlHelper _helper;
        private readonly string _htmlTemplate;

        public FieldsInputsTest()
        {
            var viewData = new ViewDataDictionary();
            var viewContext = new Mock<ViewContext>();
            viewContext.Setup(c => c.ViewData).Returns(viewData);
            var viewDataContainer = new Mock<IViewDataContainer>();
            viewDataContainer.Setup(c => c.ViewData).Returns(viewData);
            _helper = new HtmlHelper<object>(viewContext.Object, viewDataContainer.Object);
            _htmlTemplate =
                @"<div class=""editor-label""><label for=""Test"">Test</label></div><div class=""editor-field"">{0}</div>";
        }

        private List<Field> GetList()
        {
            return new List<Field>
                   {
                       new Field
                       {
                           Name = "Test",
                           Value = "123"
                       }
                   };
        }

        [Test]
        public void TestEmpty()
        {
            var html = _helper.FieldsInputs(new List<Field>());
            Assert.AreEqual("", html.ToString());
        }

        [Test]
        public void TestTypeBoolean()
        {
            var list = GetList();
            list[0].Type = FieldType.Boolean;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(
                String.Format(_htmlTemplate, @"<div class=""switch"" data-on-label=""Sim"" data-off-label=""Não""><input name=""Test"" type=""checkbox"" checked=""checked"" /></div>"),
                html.ToString());
        }

        [Test]
        public void TestTypeBooleanWithTrue()
        {
            var list = GetList();
            list[0].Type = FieldType.Boolean;
            list[0].Value = "true";
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(
                String.Format(_htmlTemplate, @"<div class=""switch"" data-on-label=""Sim"" data-off-label=""Não""><input name=""Test"" type=""checkbox"" checked=""checked"" /></div>"),
                html.ToString());
        }

        [Test]
        public void TestTypeBooleanWithFalse()
        {
            var list = GetList();
            list[0].Type = FieldType.Boolean;
            list[0].Value = "false";
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<div class=""switch"" data-on-label=""Sim"" data-off-label=""Não""><input name=""Test"" type=""checkbox"" /></div>"), html.ToString());
        }

        [Test]
        public void TestTypeBooleanWithNullOrEmpty()
        {
            var list = GetList();
            list[0].Type = FieldType.Boolean;
            list[0].Value = null;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<div class=""switch"" data-on-label=""Sim"" data-off-label=""Não""><input name=""Test"" type=""checkbox"" /></div>"), html.ToString());

            list[0].Value = "";
            html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<div class=""switch"" data-on-label=""Sim"" data-off-label=""Não""><input name=""Test"" type=""checkbox"" /></div>"), html.ToString());
        }

        [Test]
        public void TestTypeColor()
        {
            var list = GetList();
            list[0].Type = FieldType.Color;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input class=""colorpicker"" id=""Test"" name=""Test"" type=""text"" value=""123"" />"), html.ToString());
        }

        [Test]
        public void TestTypeDate()
        {
            var list = GetList();
            list[0].Type = FieldType.Date;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input class=""datepicker"" id=""Test"" name=""Test"" type=""text"" value=""123"" />"), html.ToString());
        }

        [Test]
        public void TestTypeDateTime()
        {
            var list = GetList();
            list[0].Type = FieldType.DateTime;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input class=""datetimepicker"" id=""Test"" name=""Test"" type=""text"" value=""123"" />"), html.ToString());
        }

        [Test]
        public void TestTypeEmail()
        {
            var list = GetList();
            list[0].Type = FieldType.Email;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input id=""Test"" name=""Test"" type=""email"" value=""123"" />"), html.ToString());
        }

        [Test]
        public void TestTypeLocation()
        {
            var list = GetList();
            list[0].Type = FieldType.Location;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input id=""Test"" name=""Test"" type=""text"" value=""123"" />"), html.ToString());
        }

        [Test]
        public void TestTypeMedia()
        {
            var list = GetList();
            list[0].Type = FieldType.Media;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input class=""media-picker-control"" id=""Test"" name=""Test"" type=""text"" value=""123"" />"), html.ToString());
        }

        [Test]
        public void TestTypeNumber()
        {
            var list = GetList();
            list[0].Type = FieldType.Number;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input id=""Test"" name=""Test"" type=""number"" value=""123"" />"), html.ToString());
        }

        [Test]
        public void TestTypeRichText()
        {
            var list = GetList();
            list[0].Type = FieldType.RichText;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<textarea class=""ckeditor"" cols=""20"" id=""Test"" name=""Test"" rows=""2"">
123</textarea>"), html.ToString());
        }

        [Test]
        public void TestTypeEmptySelectList()
        {
            var list = GetList();
            list[0].Type = FieldType.SelectList;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<select id=""Test"" name=""Test""><option value="""">Selecione</option>
</select>"), html.ToString());
        }

        [Test]
        public void TestTypeSelectList()
        {
            var list = GetList();
            list[0].Type = FieldType.SelectList;
            list[0].SelectList = new List<string>
                                  {
                                      "abc",
                                      "def",
                                      "ghi"
                                  };
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate,
                @"<select id=""Test"" name=""Test""><option value="""">Selecione</option>
<option>abc</option>
<option>def</option>
<option>ghi</option>
</select>"), html.ToString());
        }


        [Test]
        public void TestTypeSelectListSelected()
        {
            var list = GetList();
            list[0].Type = FieldType.SelectList;
            list[0].Value = "def";
            list[0].SelectList = new List<string>
                                  {
                                      "abc",
                                      "def",
                                      "ghi"
                                  };
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate,
                @"<select id=""Test"" name=""Test""><option value="""">Selecione</option>
<option>abc</option>
<option selected=""selected"">def</option>
<option>ghi</option>
</select>"), html.ToString());
        }

        [Test]
        public void TestTypeText()
        {
            var list = GetList();
            list[0].Type = FieldType.Text;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input id=""Test"" name=""Test"" type=""text"" value=""123"" />"), html.ToString());
        }

        [Test]
        public void TestTypeTextWithNullOrEmptyValue()
        {
            var list = GetList();
            list[0].Type = FieldType.Text;
            list[0].Value = null;
            var html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input id=""Test"" name=""Test"" type=""text"" value="""" />"), html.ToString());

            list[0].Type = FieldType.Text;
            list[0].Value = "";
            html = _helper.FieldsInputs(list);
            Assert.AreEqual(String.Format(_htmlTemplate, @"<input id=""Test"" name=""Test"" type=""text"" value="""" />"), html.ToString());
        }
    }
}
