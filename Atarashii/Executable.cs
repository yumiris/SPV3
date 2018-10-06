using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace Atarashii
{
    public class Executable
    {
        /// <summary>
        ///     HCE executable name.
        /// </summary>
        private const string ExecutableName = @"haloce.exe";

        /// <summary>
        ///     Known file length of the 1.10 executable.
        /// </summary>
        private const int ValidLength = 0x24B000;

        /// <summary>
        ///     Default location set by the HCE installer.
        /// </summary>
        private const string DefaultInstall = @"C:\Program Files (x86)\Microsoft Games\Halo Custom Edition";

        /// <summary>
        ///     HCE registry keys location.
        /// </summary>
        private const string RegKeyLocation = @"SOFTWARE\Microsoft\Microsoft Games\Halo CE";

        /// <summary>
        ///     HCE executable path registry key name.
        /// </summary>
        private const string RegKeyIdentity = @"EXE Path";

        /// <summary>
        ///     Executes the given HCE executable.
        /// </summary>
        /// <param name="executable">
        ///     Path to the HCE executable.
        /// </param>
        /// <param name="verify">
        ///     Verify the HCE executable.
        /// </param>
        /// <exception cref="LoaderException">
        ///     The specified executable was not found.
        /// </exception>
        public void Load(string executable, bool verify = true)
        {
            if (!File.Exists(executable))
                throw new LoaderException($"The specified executable '{executable}' was not found.");

            if (verify)
                if (!Verify(executable))
                    throw new LoaderException($"The specified executable '{executable}' is deemed invalid.");

            new Process
            {
                StartInfo =
                {
                    FileName = executable,
                    WorkingDirectory = Path.GetDirectoryName(executable)
                }
            }.Start();
        }

        /// <summary>
        /// <inheritdoc cref="Load(string,bool)"/>
        /// </summary>
        /// <param name="verify"></param>
        public void Load(bool verify = true)
        {
            Load(Detect(), verify);
        }

        /// <summary>
        ///     Retrieves the path of the HCE executable on the system.
        /// </summary>
        /// <returns>
        ///     Absolute executable path if found, otherwise an empty string.
        /// </returns>
        public string Detect()
        {
            using (var view = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = view.OpenSubKey(RegKeyLocation))
            {
                var path = key?.GetValue(RegKeyIdentity);
                if (path != null) return $@"{path}\{ExecutableName}";
            }

            var fullDefaultPath = $@"{DefaultInstall}\{ExecutableName}";

            if (File.Exists(fullDefaultPath)) return fullDefaultPath;

            var currentDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), ExecutableName);
            return File.Exists(ExecutableName) ? currentDirectoryPath : string.Empty;
        }

        /// <summary>
        ///     Checks if the specified HCE executable is valid.
        /// </summary>
        /// <param name="executable">
        ///     HCE executable to verify.
        /// </param>
        /// <returns>
        ///     True on executable being deemed as valid; otherwise false.
        /// </returns>
        /// <exception cref="VerifierException">
        ///     Executable is not found.
        /// </exception>
        public bool Verify(string executable)
        {
            if (!File.Exists(executable))
                throw new VerifierException($"The specified executable '{executable}' was not found.");

            return new FileInfo(executable).Length == ValidLength;
        }

        /// <summary>
        /// <inheritdoc cref="Verify(string)"/>
        /// </summary>
        /// <returns></returns>
        public bool Verify()
        {
            return Verify(Detect());
        }
    }
}