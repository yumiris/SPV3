/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of HCE.HCE.BalsamV.
 * 
 * HCE.HCE.BalsamV is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * HCE.HCE.BalsamV is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with HCE.HCE.BalsamV.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using HCE.BalsamV.Profile;
using HCE.BalsamV.Settings;
using NUnit.Framework;

namespace HCE.BalsamV.Tests
{
    [TestFixture]
    public class ProfileTests
    {
        private readonly Blam _blam =
            BlamFactory.GetFromStream(new MemoryStream(ProfileTestData.blam));

        [Test]
        public void InvalidName_ThrowsException_True()
        {
            var configuration = new Blam();

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => configuration.Name = "Hello from Gensokyo");
            StringAssert.Contains("Assigned name value is greater than 11 characters.", ex.Message);
        }

        [Test]
        public void InvalidResolution_ThrowsException_True()
        {
            var configuration = new Blam();

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => configuration.Video.Resolution.Width = 0);
            StringAssert.Contains("Assigned dimension value is either 0 or over 32767.", ex.Message);
        }

        [Test]
        public void InvalidSensitivity_ThrowsException_True()
        {
            var configuration = new Blam();

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => configuration.Mouse.Sensitivity.Horizontal = 11);
            StringAssert.Contains("Assigned sensitivity value is less than 1 or greater than 10.", ex.Message);
        }

        [Test]
        public void InvalidVolume_ThrowsException_True()
        {
            var configuration = new Blam();

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => configuration.Audio.Volume.Music = 15);
            StringAssert.Contains("Assigned volume value is greater than 10.", ex.Message);
        }

        [Test]
        public void ProfileParsing_AudioQualityCorrect_True()
        {
            Assert.That(_blam.Audio.Quality, Is.EqualTo(Quality.Medium));
        }

        [Test]
        public void ProfileParsing_AudioVarietyIsCorrect_True()
        {
            Assert.That(_blam.Audio.Variety, Is.EqualTo(Quality.High));
        }

        [Test]
        public void ProfileParsing_AudioVolumeIsCorrect_True()
        {
            Assert.That(_blam.Audio.Volume.Master, Is.EqualTo(10));
            Assert.That(_blam.Audio.Volume.Effects, Is.EqualTo(10));
            Assert.That(_blam.Audio.Volume.Music, Is.EqualTo(6));
        }

        [Test]
        public void ProfileParsing_ColourIsCorrect_True()
        {
            Assert.That(_blam.Colour, Is.EqualTo(Colour.White));
        }

        [Test]
        public void ProfileParsing_MouseAxisInversionIsCorrect_True()
        {
            Assert.That(_blam.Mouse.InvertVerticalAxis, Is.EqualTo(false));
        }

        [Test]
        public void ProfileParsing_MouseSensitivityIsCorrect_True()
        {
            Assert.That(_blam.Mouse.Sensitivity.Horizontal, Is.EqualTo(3));
            Assert.That(_blam.Mouse.Sensitivity.Vertical, Is.EqualTo(3));
        }

        [Test]
        public void ProfileParsing_NameIsCorrect_True()
        {
            Assert.That(_blam.Name, Is.EqualTo("New001"));
        }

        [Test]
        public void ProfileParsing_NetworkPortsAreCorrect_True()
        {
            Assert.That(_blam.Network.Port.Server, Is.EqualTo(2302));
            Assert.That(_blam.Network.Port.Client, Is.EqualTo(2303));
        }

        [Test]
        public void ProfileParsing_NetworkTypeIsCorrect_True()
        {
            Assert.That(_blam.Network.Connection, Is.EqualTo(Connection.DslLow));
        }

        [Test]
        public void ProfileParsing_VideoEffectsAreCorrect_True()
        {
            Assert.That(_blam.Video.Effects.Specular, Is.EqualTo(true));
            Assert.That(_blam.Video.Effects.Shadows, Is.EqualTo(true));
            Assert.That(_blam.Video.Effects.Decals, Is.EqualTo(true));
        }

        [Test]
        public void ProfileParsing_VideoFrameRateIsCorrect_True()
        {
            Assert.That(_blam.Video.FrameRate, Is.EqualTo(FrameRate.Fps30));
        }

        [Test]
        public void ProfileParsing_VideoParticlesIsCorrect_True()
        {
            Assert.That(_blam.Video.Particles, Is.EqualTo(Particles.High));
        }

        [Test]
        public void ProfileParsing_VideoQualityIsCorrect_True()
        {
            Assert.That(_blam.Video.Particles, Is.EqualTo(Particles.High));
        }

        [Test]
        public void ProfileParsing_VideoResolutionIsCorrect_True()
        {
            Assert.That(_blam.Video.Resolution.Width, Is.EqualTo(800));
            Assert.That(_blam.Video.Resolution.Height, Is.EqualTo(600));
        }
    }
}