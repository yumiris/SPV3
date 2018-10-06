namespace Atarashii.Profile
{
    public partial class Lastprof
    {
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
            if (!data.Contains(Signature)) throw new ParserException("");

            var array = data.Split(Delimiter);
            return array[array.Length - NameOffset];
        }
    }
}