/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Domain.
 * 
 * SPV3.Domain is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Domain is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Domain.  If not, see <http://www.gnu.org/licenses/>.
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