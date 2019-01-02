/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Loader.
 * 
 * SPV3.Loader is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Loader is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Loader.  If not, see <http://www.gnu.org/licenses/>.
 */

using NUnit.Framework;

namespace SPV3.Loader.Test
{
    [TestFixture]
    public class ParameterTest
    {
        [Test]
        public void ParseType_TogglesAreParsed_True()
        {
            var parsed = new ParametersParser().Parse("-nosound -novideo -safemode -window");

            Assert.IsTrue(
                parsed.DisableSound
                && parsed.DisableVideo
                && parsed.EnableSafeMode
                && parsed.EnableWindowMode
            );
        }

        [Test]
        public void SerialiseType_CardTypeIsCorrect_True()
        {
            Assert.AreEqual("-use14", new ParametersSerialiser().Serialise(
                new Parameters
                {
                    CardType = CardType.Shaders14Card
                })
            );
        }

        [Test]
        public void SerialiseType_CorrectVideoMode_True()
        {
            Assert.AreEqual("-vidmode 1280,720,60", new ParametersSerialiser().Serialise(
                new Parameters
                {
                    VideoWidth = 1280,
                    VideoHeight = 720,
                    VideoRefreshRate = 60
                })
            );
        }

        [Test]
        public void SerialiseType_PortsAreCorrect_True()
        {
            Assert.AreEqual("-port 2302 -cport 2303", new ParametersSerialiser().Serialise(
                new Parameters
                {
                    ServerPort = 2302,
                    ClientPort = 2303
                })
            );
        }

        [Test]
        public void SerialiseType_TogglesAreCorrect_True()
        {
            Assert.AreEqual("-nosound -safemode -console", new ParametersSerialiser().Serialise(
                new Parameters
                {
                    DisableSound = true,
                    EnableSafeMode = true,
                    EnableConsole = true
                })
            );
        }
    }
}