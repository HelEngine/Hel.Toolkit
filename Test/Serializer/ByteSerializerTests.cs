using System;
using System.Security.Cryptography;
using Hel.Toolkit.Encryption;
using Hel.Toolkit.Serializer;
using NUnit.Framework;

namespace Test.Serializer
{
    [TestFixture]
    public class ByteSerializerTests
    {

        private byte[] _serializedBytes;
        private ObjectSerializeModelTest _deserializedModel;
        private ObjectSerializeModelTest _originalModel;

        private byte[] key;

        [SetUp]
        public void Setup()
        {
            _originalModel = new ObjectSerializeModelTest
            {
                TestInt = 5565,
                TestString = "Hello"
            };
            
            key = HelAes.GenerateKeyFromString("This is totally a kool passwurd");
        }
        
        [Test, Order(1)]
        public void ObjectSerializesTest()
        {
            Assert.DoesNotThrow(() => _serializedBytes = ByteSerializer.ObjectToByteArray(_originalModel, key));
        }

        [Test, Order(2)]
        public void ObjectDeserializesTest()
        {
            Assert.DoesNotThrow(() => _deserializedModel = ByteSerializer.ByteArrayToObject<ObjectSerializeModelTest>(_serializedBytes, key));
            Assert.That(_deserializedModel, Is.Not.Null);
        }
        
        [Test, Order(3)]
        public void DeserializedObjectEquatable()
        {
            Assert.That(_deserializedModel, Is.EqualTo(_originalModel));
        }
        
        [Serializable]
        private class ObjectSerializeModelTest : IEquatable<ObjectSerializeModelTest>
        {
            public string TestString { get; set; }
            public int TestInt { get; set; }

            public bool Equals(ObjectSerializeModelTest other)
            {
                return TestString == other?.TestString && TestInt == other?.TestInt;
            }
        }
    }
}