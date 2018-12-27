using System;
using NUnit.Framework;

namespace SPV3.Domain.Tests
{
    [TestFixture]
    public class NameTests
    {
        [Test]
        public void ValueLength_ExceedsUpperBound_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                const string longButImportantKogasaLore =
                    "Kogasa Tatara (多々良　小傘 Tatara Kogasa) is a karakasa obake whose sole purpose is to surprise " +
                    "people. However, she hasn't been very successful so far. She encounters the heroine on their " +
                    "way to catch up with the flying object on a mission to scare her, but it doesn't work. She was " +
                    "once a normal umbrella in the outside world; forgotten by her owner, and no one else claimed " +
                    "her because the colour was unpopular at the time. As time went on, the wind swept her away to " +
                    "Gensokyo and she became a youkai. According to her profile in Undefined Fantastic Object, she " +
                    "is currently reading up on classic ghost stories in order to improve her scaring technique.";

                Console.Write((Name) longButImportantKogasaLore);
            });
        }
    }
}