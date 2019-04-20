using Library.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Library.Domain.UnitTests.ExtensionsTests
{
    public class ShapeDataTests
    {
        private class TestClass
        {
            const string const1 = "const1";
            private string field1 = "field1";
            public int Prop1 { get; set; } = 1;
            public string Prop2 { get; set; } = "Prop2";
            protected string Prop3 { get; set; } = "Prop3";
            public void Method1() { }
        }

        [Fact]
        public void ShouldGetListOfOnlyOneProperty()
        {
            const int ExpectedCountOfProperties = 1;

            var listOfTestClass = new List<TestClass>()
            {
                new TestClass()
            };

            var expectedPropertyName = nameof(TestClass.Prop1);
            var expectedPropertyValue = listOfTestClass.FirstOrDefault().Prop1;

            var result = listOfTestClass.ShapeData("prop1");
            var dictionary_object = (IDictionary<string, object>)result?.FirstOrDefault();

            Assert.Equal(expectedPropertyName, dictionary_object?.Keys?.FirstOrDefault());
            Assert.Equal(expectedPropertyValue, dictionary_object?.Values?.FirstOrDefault());
            Assert.Equal(ExpectedCountOfProperties, dictionary_object?.Count);
        }

        [Fact]
        public void ShouldGetListOfAllPublicProperties()
        {
            const int ExpectedCountOfProperties = 2;

            var listOfTestClass = new List<TestClass>()
            {
                new TestClass()
            };

            var result = listOfTestClass.ShapeData(string.Empty);
            var dictionary_object = (IDictionary<string, object>)result?.FirstOrDefault();

            Assert.Equal(ExpectedCountOfProperties, dictionary_object?.Count);
        }

        [Fact]
        public void ShouldADictionaryForEachItemInTheList()
        {
            var listOfTestClass = new List<TestClass>()
            {
                new TestClass(),
                new TestClass()
            };

            var result = listOfTestClass.ShapeData(string.Empty);

            Assert.Equal(listOfTestClass.Count, result.Count());
        }
    }
}
