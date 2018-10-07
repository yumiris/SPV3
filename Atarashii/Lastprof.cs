using Atarashii.Exceptions;

namespace Atarashii
{
    public class Lastprof
    {
        /// <summary>
        ///     Official name of the lastprof text file.
        /// </summary>
        public const string Name = "lastprof.txt";

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

        public Lastprof(string data)
        {
            Data = data;
        }

        /// <summary>
        ///     Latprof.txt data.
        /// </summary>
        public string Data { get; }

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
        /// <exception cref="ParserException">
        ///     Given Lastprof data is deemed as invalid.
        /// </exception>
        public string Parse()
        {
            if (!Data.Contains(Signature)) throw new ParserException("Invalid lastprof string.");

            var array = Data.Split(Delimiter);
            return array[array.Length - NameOffset];
        }
    }
}