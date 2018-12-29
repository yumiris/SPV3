using System;
using System.Collections.Generic;
using NUnit.Framework;
using SPV3.Installer.Data;

namespace SPV3.Installer.Tests
{
    public class PackageTests
    {
        [Test]
        public void EntriesCount_ExceedsUpperBound_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var list = new List<Entry>();

                for (var i = 0; i < 256; i++)
                    list.Add((Entry) "Not a cat girl, but a werewolf.");

                new Package
                {
                    Entries = list
                };
            });
        }
    }
}