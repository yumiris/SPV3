using System;
using System.IO;
using static BalsamV.Lastprof;

namespace BalsamV
{
    /// <summary>
    ///     Creates Lastprof instances.
    /// </summary>
    public static class LastprofFactory
    {
        /// <summary>
        ///     Attempts to deserialise a Lastprof object by detecting a lastprof.txt on the file system.
        /// </summary>
        /// <returns>
        ///     Configuration instance representing a successfully detected lastprof.txt text file.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///     Attempted to detect a lastprof.txt file and none has been found on the file system.
        /// </exception>
        public static Lastprof DetectOnSystem()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var txtFilePath = Path.Combine(myDocuments, "My Games", "Halo CE", "lastprof.txt");

            if (!File.Exists(txtFilePath))
                throw new FileNotFoundException("Could not find lastprof.txt through the detection attempt.");

            return GetFromFile(txtFilePath);
        }

        /// <summary>
        ///     Attempts to deserialise a Lastprof object by reading the data of the specified lastprof.txt text.
        /// </summary>
        /// <param name="path">
        ///     Path to the blam.sav binary file.
        /// </param>
        /// <returns>
        ///     Lastprof instance representing a successfully parsed blam.sav binary.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///     Inbound blam.sav path not found.
        /// </exception>
        public static Lastprof GetFromFile(string path)
        {
            return GetFromString(File.ReadAllText(path));
        }

        /// <summary>
        ///     Deserialises a given string variable to a Lastprof instance.
        /// </summary>
        /// <param name="data">
        ///     String representation of a lastprof.txt file.
        /// </param>
        /// <returns>
        ///     Lastprof.txt object instance.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Given lastprof string lacks valid signature.
        /// </exception>
        public static Lastprof GetFromString(string data)
        {
            if (!data.Contains(Signature))
                throw new ArgumentException("Given lastprof string lacks valid signature.");

            var array = data.Split(Delimiter);

            return new Lastprof
            {
                Name = array[array.Length - NameOffset]
            };
        }
    }
}