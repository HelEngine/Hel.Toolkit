using System;
using Hel.Toolkit.DataStructure.Arrays;
using NUnit.Framework;

namespace Test.DataStructure
{
    [TestFixture] 
    public class DynamicArrayTests
    {
        [Test]
        public void DynamicArrayExpands()
        {
            // Test expansion from 0 size
            var dynamicArray = new DynamicArray<bool>(0) {[555] = true};
            Assert.That(dynamicArray.Size, Is.EqualTo(556));
            
            // Test array size doubling
            dynamicArray[556] = true;
            Assert.That(dynamicArray.Size, Is.EqualTo(556 * 2));
            
        }

        [Test]
        public void DynamicArrayReduces()
        {
            // Test array reduction
            var dynamicArray = new DynamicArray<bool>(445) {[222] = true};
            dynamicArray.ReducePastIndex(256);
            Assert.That(dynamicArray.Size, Is.EqualTo(257));
        }
        
        [Test]
        public void DynamicArrayClears()
        {
            // Test array reduction
            var dynamicArray = new DynamicArray<bool>(445) {[323] = true};
            dynamicArray.Clear();
            Assert.That(dynamicArray[323], Is.EqualTo(false));
        }
    }
}