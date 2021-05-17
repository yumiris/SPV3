using System.IO;
using static System.Environment;
using static HXE.Console;
using static HXE.Paths;
using Directory = System.IO.Directory;

namespace HXE.HCE
{
    /// <summary>
    ///   Object used for creating a new Player Profile.
    /// </summary>
    public static class NewProfile
    {
        /// <summary>
        ///   Generate a Player Profile and assign it as the LastProfile.
        /// </summary>
        /// <param name="pathParam"    >Inherit and pass the -path parameter.</param>
        /// <param name="lastprof"     >Inherit the LastProfile instance.</param>
        /// <param name="profile"      >Inherit the Profile instance.</param>
        /// <param name="makeScaffold" >Inherit and pass the bool indicating if the scaffold must be created.</param>
        /// <remarks>
        ///   Note: The selected profile's blam.sav will be overwritten with its data preserved. <br/>
        ///   This doesn't harm good profiles, but it will fix bad profiles.
        /// </remarks>
        public static void Create(string pathParam,
                                    LastProfile lastprof = null,
                                    Profile profile = null,
                                    bool makeScaffold = true,
                                    bool profileIsGood = true)
        {
            if (string.IsNullOrWhiteSpace(pathParam))
            {
                throw new System.ArgumentException($"'{nameof(pathParam)}' cannot be null or whitespace.", nameof(pathParam));
            }

            Core("Beginning to generate a new Player Profile");

            /** Handle Defaulted Parameters and set Exists booleans. */
            if (lastprof is null)
                lastprof = (LastProfile) Custom.LastProfile(pathParam);

            if (profile is null)
            {
                var lastUsedExists = System.IO.File.Exists(Custom.Profile(pathParam, lastprof.Profile));

                if (lastUsedExists)
                    profile = (Profile) Custom.Profile(pathParam, lastprof.Profile);
                else if (Profile.List().Count != 0)
                    profile = Profile.List(Custom.Profiles(pathParam))[0];
                else
                    profile = (Profile) Custom.Profile(pathParam, "New001");
            }

            bool lastprofExists = lastprof.Exists();
            bool profileExists = profile.Exists();
            bool savegameExists = System.IO.File.Exists(Custom.Progress(pathParam, profile.Details.Name));

            /** Double-check for existing files to determine if the
             * savegames scaffold still needs to be created.
             * We only do this when makeScaffold is true in case
             * the scaffold structure exists, but needs to be recreated.*/
            if (makeScaffold)
            {
                makeScaffold = !lastprofExists
                  || !profileExists
                  || !savegameExists
                  || !profileIsGood;
            }

            /** Load settings from existing profile */
            if (profileExists)
                profile.Load();

            lastprof.Profile = profile.Details.Name;

            if (makeScaffold)
                Scaffold(pathParam, lastprof, profile);
            else
            {
                profile.Save();
                lastprof.Save();
            }
        }

        /// <summary>
        ///   Create file and folder structure for Profile.Generate().
        /// </summary>
        /// <param name="pathParam"> -path parameter to pass to Halo and write to profiles.</param>
        /// <param name="lastprof" > Object reperesentation of LastProf.txt.</param>
        /// <param name="profile"  > Object representation of blam.sav.</param>
        private static void Scaffold(string pathParam, LastProfile lastprof, Profile profile)
        {
            if (string.IsNullOrWhiteSpace(pathParam))
            {
                throw new System.ArgumentException($"'{nameof(pathParam)}' cannot be null or empty.", nameof(pathParam));
            }

            if (lastprof is null)
            {
                throw new System.ArgumentNullException(nameof(lastprof));
            }

            if (profile is null)
            {
                throw new System.ArgumentNullException(nameof(profile));
            }

            Core("Creating Scaffold...");

            /** Set Path to the -path executable parameter */
            var file = (File) pathParam;

            RootDir();
            SavegamesDir();
            ProfileDir();
            BlamSav();
            SavegameBin();
            ProfileWaypoint();
            LastProfileTxt();

            Info("Scaffold Completed.");

            /// <summary>
            ///   Create the Scaffold root directory
            /// </summary>
            /// <example>
            ///   ".\temp\"
            /// </example>
            void RootDir()
            {
                Directory.CreateDirectory(file.Path);
                VerifyPath(file.Path);
            }

            /// <summary>
            ///   Create the Savegames directory if it doesn't exist.
            /// </summary>
            /// <example>
            ///   ".\temp\savegames\"
            /// </example>
            void SavegamesDir()
            {
                file.Path = Custom.Profiles(pathParam);
                Directory.CreateDirectory(file.Path);
                VerifyPath(file.Path);
            }

            /// <summary>
            ///  Create Profile's folder.
            /// </summary>
            void ProfileDir()
            {
                file.Path = Custom.ProfileDirectory(pathParam, profile.Details.Name);  /// e.g. ".\temp\savegames\New001\"
                Directory.CreateDirectory(file.Path);
                VerifyPath(file.Path);
            }

            /// <summary>
            ///   Create Profile's blam.sav
            /// </summary>
            /// <example>
            ///   ".\temp\savegames\New001\blam.sav"
            /// </example>
            /// <remarks>
            ///   The file is overwritten, so you must Load() existing data <br/>
            ///   if you want to keep that data.
            /// </remarks>
            void BlamSav()
            {
                CreateBlam();
                profile.Save();
                VerifyPath(profile.Path);

                /// <summary>
                ///   Produces a file identical to default_profile\00.sav <br/>
                ///   Write Input Blanks
                /// </summary>
                /// <see cref="Profile.Offset"/>
                void CreateBlam()
                {
                    using (var fs = new FileStream(profile.Path, FileMode.Create, FileAccess.ReadWrite))
                    using (var ms = new MemoryStream(8192))
                    using (var bw = new BinaryWriter(ms))
                    {
                        fs.Position = 0;
                        fs.CopyTo(ms);

                        /** Write Bindings filler (7fff) */
                        ms.Position = 0x13c;
                        while (ms.Position < 0x93a)
                        {
                            bw.Write((ushort) 0x7fff);
                        }

                        /** Write Gamepad Menu Bindings filler (ffff) */
                        ms.Position = 0x32A;
                        while (ms.Position != 0x33a)
                        {
                            bw.Write((ushort) 0xffff);
                        }

                        /** Unknown */
                        {
                            ms.Position = 0x0;
                            bw.Write((byte) 0x9);

                            ms.Position = 0x11a;
                            bw.Write((ushort) 0xffff);
                            bw.Write((ushort) 0x1);

                            ms.Position = 0x12E;
                            bw.Write((byte) 0x3);

                            ms.Position = 0x134;
                            bw.Write((ushort) 0x9);
                            bw.Write((ushort) 0xC);
                            bw.Write((ushort) 0x1B);
                            bw.Write((ushort) 0x1C);

                            ms.Position = 0x14E;
                            bw.Write((ushort) 0x12);

                            ms.Position = 0x170;
                            bw.Write((ushort) 0x3);
                            bw.Write((ushort) 0x5);
                            bw.Write((ushort) 0x13);
                            bw.Write((ushort) 0x2);
                            bw.Write((ushort) 0xD);
                            bw.Write((ushort) 0xF);
                            bw.Write((ushort) 0x10);

                            ms.Position = 0x18E;
                            bw.Write((ushort) 0x15);
                            bw.Write((ushort) 0x14);
                            bw.Write((ushort) 0x16);
                            bw.Write((ushort) 0x4);
                            bw.Write((ushort) 0x1);
                            bw.Write((ushort) 0x11);

                            ms.Position = 0x1A4;
                            bw.Write((ushort) 0x8);
                            ms.Position += 2;
                            bw.Write((ushort) 0xB);
                            bw.Write((ushort) 0xE);

                            ms.Position = 0x1BE;
                            bw.Write((ushort) 0xA);

                            ms.Position = 0x1C4;
                            bw.Write((ushort) 0x0);

                            ms.Position = 0x20E;
                            bw.Write((ushort) 0x7);
                            bw.Write((ushort) 0xB);
                            bw.Write((ushort) 0x6);

                            ms.Position = 0x21E;
                            bw.Write((ushort) 0x1A);
                            bw.Write((ushort) 0x19);
                            bw.Write((ushort) 0x17);
                            bw.Write((ushort) 0x18);

                            ms.Position = 0x93A;
                            bw.Write((ushort) 0x0000);
                            bw.Write((ushort) 0x0000);
                            bw.Write((byte) 0x80);
                            bw.Write((byte) 0x3F);
                            bw.Write((ushort) 0x0000);
                            bw.Write((byte) 0x80);
                            bw.Write((byte) 0x3F);
                            bw.Write((byte) 0xFC);
                            bw.Write((byte) 0x04);
                            bw.Write((byte) 0x41);
                            bw.Write((byte) 0x3E);
                            bw.Write((byte) 0xFC);
                            bw.Write((byte) 0x04);
                            bw.Write((byte) 0x41);
                            bw.Write((byte) 0x3E);
                            bw.Write((ushort) 0x0000);
                            bw.Write((byte) 0x00);
                            bw.Write((byte) 0x43);
                            bw.Write((ushort) 0x0000);
                            bw.Write((byte) 0x00);
                            bw.Write((byte) 0x43);
                            bw.Write((ushort) 0x0303);
                            bw.Write((ushort) 0x0303);
                            bw.Write((ushort) 0x0303);
                            bw.Write((ushort) 0x0303);
                            bw.Write((ushort) 0x0303);
                            bw.Write((ushort) 0x0000);
                            bw.Write((ushort) 0x0000);
                            bw.Write((byte) 0x40);
                            bw.Write((byte) 0x3f);
                            bw.Write((ushort) 0x0000);
                            bw.Write((byte) 0x40);
                            bw.Write((byte) 0x3f);

                            ms.Position = 0x962;
                            bw.Write((byte) 0x40);
                            bw.Write((byte) 0x3F);
                            bw.Write((byte) 0x0);
                            bw.Write((byte) 0x0);
                            bw.Write((byte) 0x40);
                            bw.Write((byte) 0x3F);

                            ms.Position = 0xA68;
                            bw.Write((byte) 0x20);
                            bw.Write((byte) 0x3);
                            bw.Write((byte) 0x58);
                            bw.Write((byte) 0x2);
                            bw.Write((byte) 0x3C);
                            bw.Write((byte) 0x0);
                            bw.Write((byte) 0x2);
                            bw.Write((byte) 0x2);
                            bw.Write((byte) 0x1);
                            bw.Write((byte) 0x1);
                            bw.Write((byte) 0x1);
                            bw.Write((byte) 0x2);
                            bw.Write((byte) 0x2);
                            bw.Write((byte) 0x2);
                            bw.Write((byte) 0x1);

                            ms.Position = 0xb78;
                            bw.Write((byte) 0xA);
                            bw.Write((byte) 0xA);
                            bw.Write((byte) 0x6);
                            bw.Write((byte) 0x0);
                            bw.Write((byte) 0x0);
                            bw.Write((byte) 0x1);
                            bw.Write((byte) 0x0);
                            bw.Write((byte) 0x2);

                            ms.Position = 0xC80;
                            bw.Write((byte) 0x3);
                            bw.Write((byte) 0x1);
                            bw.Write((byte) 0x1);
                            bw.Write((byte) 0x00);
                            bw.Write((ushort) 0x0000);
                            bw.Write((byte) 0x1);
                            bw.Write((byte) 0x1);

                            ms.Position = 0xD8C;
                            bw.Write(System.Text.Encoding.Unicode.GetBytes("Halo"));

                            ms.Position = 0xEBF;
                            bw.Write((byte) 0x3);

                            ms.Position = 0xFC0;
                            bw.Write((byte) 0x1);

                            ms.Position = 0x1002;
                            bw.Write(0xFE);
                            ms.Position = 0x1003;
                            bw.Write(0x08);
                            ms.Position = 0x1004;
                            bw.Write(0xFF);
                            ms.Position = 0x1005;
                            bw.Write(0x08);

                            ms.Position = 0x1FFC;
                            bw.Write((byte) 0x50);
                            bw.Write((byte) 0x3B);
                            bw.Write((byte) 0xA1);
                            bw.Write((byte) 0x3C);
                        }

                        ms.Position = 0;
                        ms.CopyTo(fs);
                    }
                }
            }

            /// <summary>
            ///   Create Profile's savegame.bin.
            /// </summary>
            /// <example>
            ///   ".\temp\savegames\New001\savegame.bin"
            /// </example>
            void SavegameBin()
            {
                file.Path = Custom.Progress(pathParam, profile.Details.Name);

                /** Only create a new savegame.bin if one does not already exist. */
                if (!file.Exists() || file.Size() != 0x480000)
                {
                    file.WriteAllBytes(new byte[0x480000]); /// 0x480000 == int 4718592
                }

                VerifyPath(file.Path);
            }

            /// <summary>
            ///   Create Profile's Waypoint file.
            /// </summary>
            /// <example>
            ///   ".\temp\savegames\New001\New001"
            /// </example>
            void ProfileWaypoint()
            {
                file.Path = Custom.Waypoint(pathParam, profile.Details.Name);
                file.WriteAllText(Custom.Waypoint(pathParam, profile.Details.Name));
                VerifyPath(file.Path);
            }

            /// <summary>
            ///   Save Profile to lastprof.txt
            /// </summary>
            /// <example>
            ///   ".\temp\lastprof.txt"
            /// </example>
            void LastProfileTxt()
            {
                file.Path = Custom.LastProfile(pathParam);
                lastprof.Path = file.Path;
                lastprof.Name = profile.Details.Name;
                lastprof.Save();
                VerifyPath(file.Path);
            }

            /// <summary>
            ///   Output the File.Path variable's current value. Verify its full path exists in the file system.
            /// </summary>
            void VerifyPath(string path)
            {
                bool isDir = System.IO.File.GetAttributes(path).HasFlag(FileAttributes.Directory);

                Debug(NewLine
                  + NewLine + $"Path is currently \"{path}\""
                  + NewLine + $"The Full Path is \"{Path.GetFullPath(path)}\""
                  + NewLine + $"Path points to a folder? {isDir}"
                  + NewLine + $"\"{path}\" exists?" + (isDir ? Directory.Exists(path) : System.IO.File.Exists(path))
                  );
            }
        }
    }
}