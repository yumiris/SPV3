using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SPV3.Installer.Test
{
    [TestFixture]
    public class PersistenceTests
    {
        private static readonly string XmlData = Persistence.ToXml(new Installer
        {
            Backup = new Backup
            {
                Directory = new Directory
                {
                    Name = new Name
                    {
                        Value = Guid.NewGuid().ToString()
                    }
                }
            },
            Packages = new List<Package>
            {
                new Package
                {
                    Name = new Name
                    {
                        Value = "core.pkg"
                    },
                    Files = new List<File>
                    {
                        new File
                        {
                            Name = new Name
                            {
                                Value = "spv3.exe"
                            },
                            Description = new Description
                            {
                                Value = "SPV3 Executable"
                            }
                        },
                        new File
                        {
                            Name = new Name
                            {
                                Value = "haloce.exe"
                            },
                            Description = new Description
                            {
                                Value = "HCE Executable"
                            }
                        }
                    }
                },
                new Package
                {
                    Name = new Name
                    {
                        Value = "maps.pkg"
                    },
                    Files = new List<File>
                    {
                        new File
                        {
                            Name = new Name
                            {
                                Value = "loc.map"
                            },
                            Description = new Description
                            {
                                Value = "Localisation map file"
                            }
                        },
                        new File
                        {
                            Name = new Name
                            {
                                Value = "sounds.map"
                            },
                            Description = new Description
                            {
                                Value = "Shared sounds map file"
                            }
                        }
                    }
                }
            }
        });

        [Test]
        public void AssertXml_ContainsString_HceExecutable_True()
        {
            StringAssert.Contains("haloce.exe", XmlData);
        }

        [Test]
        public void AssertXml_ContainsString_Localisation_True()
        {
            StringAssert.Contains("Localisation", XmlData);
        }

        [Test]
        public void AssertXml_ContainsString_Sounds_True()
        {
            StringAssert.Contains("sounds.map", XmlData);
        }

        [Test]
        public void AssertXml_ContainsString_XmlHeader_True()
        {
            StringAssert.Contains("xml", XmlData);
        }
    }
}