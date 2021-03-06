// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Xunit;

namespace System.Reflection.Emit.Tests
{
    public class CustomAttributeBuilderCtor4
    {
        private const string IntField = "TestInt";
        private const string StringField = "TestStringField";
        private const string GetStringField = "GetString";
        private const string GetIntField = "GetInt";
        
        private const string IntProperty = "TestInt32";
        private const string StringProperty = "TestString";
        private const string GetStringProperty = "GetOnlyString";
        private const string GetIntProperty = "GetOnlyInt32";

        [Theory]
        [InlineData(new string[] { IntProperty }, new object[0], "namedProperties, propertyValues")]
        [InlineData(new string[0], new object[] { 10 }, "namedProperties, propertyValues")]
        [InlineData(new string[] { IntProperty, StringProperty }, new object[] { "TestString", 10 }, null)]
        [InlineData(new string[] { GetIntProperty }, new object[] { "TestString" }, null)]
        [InlineData(new string[] { GetStringProperty }, new object[] { "TestString" }, null)]
        [InlineData(new string[] { IntProperty }, new object[] { "TestString" }, null)]
        public void NamedPropertyAndPropertyValuesDifferentLengths_ThrowsArgumentException(string[] propertyNames, object[] propertyValues, string paramName)
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            PropertyInfo[] namedProperties = Helpers.GetProperties(propertyNames);

            Assert.Throws<ArgumentException>(paramName, () => new CustomAttributeBuilder(constructor, new object[0], namedProperties, propertyValues, new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void StaticCtor_ThrowsArgumentException()
        {
            ConstructorInfo constructor = typeof(TestConstructor).GetConstructors(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Where(c => c.IsStatic).First();

            Assert.Throws<ArgumentException>(null, () => new CustomAttributeBuilder(constructor, new object[0], new PropertyInfo[0], new object[0], new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void PrivateCtor_ThrowsArgumentException()
        {
            ConstructorInfo constructor = typeof(TestConstructor).GetConstructors(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Where(c => c.IsPrivate).First();

            Assert.Throws<ArgumentException>(null, () => new CustomAttributeBuilder(constructor, new object[] { false }, new PropertyInfo[0], new object[0], new FieldInfo[0], new object[0]));
        }

        [Theory]
        [InlineData(new Type[] { typeof(string), typeof(int), typeof(string), typeof(int) }, new object[] { "TestString", 10 })]
        [InlineData(new Type[] { typeof(string), typeof(int) }, new object[] { 10, "TestString" })]
        public void ConstructorArgsDontMatchConstructor_ThrowsArgumentException(Type[] ctorParams, object[] constructorArgs)
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(ctorParams);

            Assert.Throws<ArgumentException>(null, () => new CustomAttributeBuilder(constructor, constructorArgs, new PropertyInfo[0], new object[0], new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void NullConstructor_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("con", () => new CustomAttributeBuilder(null, new object[0], new PropertyInfo[0], new object[0], new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void NullConstructorArgs_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("constructorArgs", () => new CustomAttributeBuilder(constructor, null, new PropertyInfo[0], new object[0], new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void NullNamedProperties_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("namedProperties", () => new CustomAttributeBuilder(constructor, new object[0], (PropertyInfo[])null, new object[0], new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void NullPropertyValues_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("propertyValues", () => new CustomAttributeBuilder(constructor, new object[0], new PropertyInfo[0], null, new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void NullObjectInPropertyValues_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("propertyValues[0]", () => new CustomAttributeBuilder(constructor, new object[0], Helpers.GetProperties(IntProperty, StringProperty), new object[] { null, 10 }, new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void NullObjectInNamedProperties_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("namedProperties[0]", () => new CustomAttributeBuilder(constructor, new object[0], Helpers.GetProperties(null, IntProperty), new object[] { "TestString", 10 }, new FieldInfo[0], new object[0]));
        }

        [Fact]
        public void NullNamedFields_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("namedFields", () => new CustomAttributeBuilder(constructor, new object[0], new PropertyInfo[0], new object[0], (FieldInfo[])null, new object[0]));
        }

        [Fact]
        public void NullFieldValues_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("fieldValues", () => new CustomAttributeBuilder(constructor, new object[0], new PropertyInfo[0], new object[0], new FieldInfo[0], null));
        }

        [Fact]
        public void NullObjectInFieldValues_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("fieldValues[0]", () => new CustomAttributeBuilder(constructor, new object[0], new PropertyInfo[0], new object[0], Helpers.GetFields(IntField, StringField), new object[] { null, 10 }));
        }

        [Fact]
        public void NullObjectInNamedFields_ThrowsArgumentNullException()
        {
            ConstructorInfo constructor = typeof(TestAttribute).GetConstructor(new Type[0]);
            Assert.Throws<ArgumentNullException>("namedFields[0]", () => new CustomAttributeBuilder(constructor, new object[0], new PropertyInfo[0], new object[0], Helpers.GetFields(null, IntField), new object[] { "TestString", 10 }));
        }
    }
}
