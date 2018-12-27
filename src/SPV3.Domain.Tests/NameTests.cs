using System;
using NUnit.Framework;

namespace SPV3.Domain.Tests
{
    [TestFixture]
    public class NameTests
    {
        [Test]
        public void Name_ExceedsUpperBound_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Console.Write
                (
                    (Name) "In the East, allocating values above the specified upper bound would be problematic."
                );
            });
        }
    }
}