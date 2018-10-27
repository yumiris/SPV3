namespace Atarashii.Profile
{
    public class Lastprof : IVerifiable
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

        private readonly string _data;

        public Lastprof(string data)
        {
            _data = data;
        }

        /// <inheritdoc />
        /// <returns>
        ///     False if:
        ///     - Given lastprof string lacks valid signature.
        /// </returns>
        public Verification Verify()
        {
            if (!_data.Contains(Signature))
                return new Verification(false, "Given lastprof string lacks valid signature.");

            return new Verification(true);
        }

        /// <summary>
        ///     Retrieves the profile name from a lastprof.txt string.
        /// </summary>
        /// <example>
        ///     new Lastprof.Parser().Parse(File.ReadAllText("lastprof.txt"));
        /// </example>
        /// <returns>
        ///     The profile name. In actual environments, it's the profile used in the last HCE instance.
        /// </returns>
        /// <exception cref="ProfileException">
        ///     Given lastprof string lacks valid signature..
        /// </exception>
        public string Parse()
        {
            var state = Verify();

            if (!state.IsValid)
                throw new ProfileException(state.Reason);

            var array = _data.Split(Delimiter);
            return array[array.Length - NameOffset];
        }
    }
}