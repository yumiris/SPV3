/**
 * Copyright (c) 2018 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

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