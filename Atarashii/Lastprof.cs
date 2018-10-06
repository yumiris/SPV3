using System;
using System.IO;

namespace Atarashii
{
    public class Lastprof
    {
        /// <summary>
        ///     Official name of the lastprof text file.
        /// </summary>
        private const string FileName = "lastprof.txt";

        /// <summary>
        ///     Separation character which is guaranteed to be present.
        /// </summary>
        private const char Delimiter = '\\';

        /// <summary>
        ///     Position of the profile name relative to the end of the split string.
        /// </summary>
        private const int NameOffset = 0x2;

        /// <summary>
        ///     Known string that is present in the lastprof.txt.
        /// </summary>
        private const string Signature = "lam.sav";

        /// <summary>
        ///     Retrieves the profile name from a lastprof.txt string.
        /// </summary>
        /// <example>
        ///     new Lastprof.Parser().Parse(File.ReadAllText("lastprof.txt"));
        /// </example>
        /// <param name="data">
        ///     Text data from a lastprof.txt file.
        /// </param>
        /// <returns>
        ///     The profile name. In actual environments, it's the profile used in the last HCE instance.
        /// </returns>
        public string Parse(string data)
        {
            if (!data.Contains(Signature)) throw new ParserException("Invalid lastprof string.");

            var array = data.Split(Delimiter);
            return array[array.Length - NameOffset];
        }

        /// <summary>
        ///     Attempts to parse an official lastprof.txt present on the filesystem.
        /// </summary>
        /// <returns>
        ///     The profile name used in the last HCE instance.
        /// </returns>
        /// <exception cref="ParserException">
        ///     Lastprof text file was not found.
        /// </exception>
        public string Parse()
        {
            var file = Detect();
            
            if (!File.Exists(file))
                throw new ParserException("Lastprof text file not found.");

            return Parse(File.ReadAllText(file));
        }

        /// <summary>
        ///     Attempts to retrieve the path of the lastprof.txt on the filesystem.
        /// </summary>
        /// <returns>
        ///     Path if found, otherwise an empty string.
        /// </returns>
        public string Detect()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var txtFilePath = Path.Combine(myDocuments, "My Games",
                "Halo CE", FileName);

            return txtFilePath ;
        }
    }
}