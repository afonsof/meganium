using System.Linq;
using Meganium.Api.Entities;
using Meganium.Api.Managers;
using NUnit.Framework;

namespace Meganium.UnitTests.Api
{
    [TestFixture]
    public class FieldTest
    {
        const string FieldsJson = @"[{
			Type: 1,
			Name: ""Test"",
			Value: """",
			SelectList: [""Item1"", ""Item2"", ""Item3""],
			Order: 0
		    }]";

        const string FieldValuesJson = @"{ Test: ""abc"" }";


        [Test]
        public void TestBindNormal()
        {
            var fieldManager = new FieldManager();

            var columns = fieldManager.Bind(FieldsJson);
            Assert.AreEqual(1, columns.Count);
            Assert.AreEqual("Test", columns[0].Name);
            Assert.AreEqual("", columns[0].Value);
            Assert.AreEqual(0, columns[0].Order);
            Assert.AreEqual(FieldType.Text, columns[0].Type);
            Assert.AreEqual(3, columns[0].SelectList.Count());
            Assert.AreEqual("Item1", columns[0].SelectList[0]);
            Assert.AreEqual("Item2", columns[0].SelectList[1]);
            Assert.AreEqual("Item3", columns[0].SelectList[2]);

            columns = fieldManager.Bind(FieldsJson, FieldValuesJson);
            Assert.AreEqual("abc", columns[0].Value);
        }

        [Test]
        public void TestBindEmpty()
        {
            var fieldManager = new FieldManager();

            var columns = fieldManager.Bind("");
            Assert.AreEqual(0, columns.Count);

            columns = fieldManager.Bind("", FieldValuesJson);
            Assert.AreEqual(0, columns.Count);
        }

        [Test]
        public void TestBindNormalWithEmptyValues()
        {
            var fieldManager = new FieldManager();

            var columns = fieldManager.Bind(FieldsJson, "");
            Assert.AreEqual("", columns[0].Value);

            columns = fieldManager.Bind(FieldsJson, @"{ Other: ""abc""}");
            Assert.AreEqual("", columns[0].Value);
        }
    }
}
