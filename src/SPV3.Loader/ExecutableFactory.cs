using System;
using System.IO;
using Microsoft.Win32;

namespace SPV3.Loader
{
    /// <summary>
    ///     Creates Executable-type instances.
    /// </summary>
    public static class ExecutableFactory
    {
        /// <summary>
        ///     Default location set by the HCE installer on 64-bit systems.
        /// </summary>
        private const string DefaultInstall64 = @"C:\Program Files (x86)\Microsoft Games\Halo Custom Edition";

        /// <summary>
        ///     Default location set by the HCE installer on 32-bit systems.
        /// </summary>
        private const string DefaultInstall32 = @"C:\Program Files\Microsoft Games\Halo Custom Edition";

        /// <summary>
        ///     HCE registry keys location.
        /// </summary>
        private const string RegKeyLocation = @"SOFTWARE\Microsoft\Microsoft Games\Halo CE";

        /// <summary>
        ///     HCE executable path registry key name.
        /// </summary>
        private const string RegKeyIdentity = @"EXE Path";

        /// <summary>
        ///     Instantiate an Executable type.
        /// </summary>
        /// <returns>
        ///     Executable instance.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///     Attempted to detect an executable and none has been found on the file system.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Invalid enum value.
        /// </exception>
        public static Executable Detect()
        {
            var currentDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), Executable.Name);
            if (File.Exists(currentDirectoryPath)) return new Executable(currentDirectoryPath);

            var fullDefaultPath64 = $@"{DefaultInstall64}\{Executable.Name}";
            if (File.Exists(fullDefaultPath64)) return new Executable(fullDefaultPath64);

            var fullDefaultPath32 = $@"{DefaultInstall32}\{Executable.Name}";
            if (File.Exists(fullDefaultPath32)) return new Executable(fullDefaultPath32);

            using (var view = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = view.OpenSubKey(RegKeyLocation))
            {
                var path = key?.GetValue(RegKeyIdentity);
                if (path != null) return new Executable($@"{path}\{Executable.Name}");
            }

            using (var view = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            using (var key = view.OpenSubKey(RegKeyLocation))
            {
                var path = key?.GetValue(RegKeyIdentity);
                if (path != null) return new Executable($@"{path}\{Executable.Name}");
            }

            throw new FileNotFoundException("Could not find a legal executable through the detection attempt.");
        }
    }
}