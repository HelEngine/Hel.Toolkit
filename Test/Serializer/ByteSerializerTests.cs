using System;
using Hel.Toolkit.Serializer;
using NUnit.Framework;

namespace Test.Serializer
{
    [TestFixture]
    public class ByteSerializerTests
    {

        private byte[] _serializedBytes;
        private object _deserializedObject;
        private ObjectSerializeModelTest _castedModel;
        private ObjectSerializeModelTest _originalModel;

        [SetUp]
        public void Setup()
        {
            _originalModel = new ObjectSerializeModelTest
            {
                TestInt = 5565,
                TestString = "Hello"
            };
        }
        
        [Test, Order(1)]
        public void ObjectSerializesTest()
        {
            Assert.DoesNotThrow(() => _serializedBytes = ByteSerializer.ObjectToByteArray(_originalModel));
        }

        [Test, Order(2)]
        public void ObjectDeserializesTest()
        {
            Assert.DoesNotThrow(() => _deserializedObject = ByteSerializer.ByteArrayToObject(_serializedBytes));
        }

        [Test, Order(3)]
        public void DeserializedObjectCasts()
        {
            Assert.DoesNotThrow(() => _castedModel = (ObjectSerializeModelTest) _deserializedObject);
        }

        [Test, Order(4)]
        public void DeserializedObjectEquatable()
        {
            Assert.That(_castedModel, Is.EqualTo(_originalModel));
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