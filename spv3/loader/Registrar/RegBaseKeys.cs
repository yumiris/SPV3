namespace Registrar
{
    /// <summary>
    /// Defines constants used for the base keys in the registry, intended
    /// to make creating settings instances easier.
    /// </summary>
    public static class RegBaseKeys // This can definitely be made to be better, but it'll do for now.
    {
        public const string HKEY_CURRENT_USER=      nameof(HKEY_CURRENT_USER);
        public const string HKEY_CLASSES_ROOT=      nameof(HKEY_CLASSES_ROOT);
        public const string HKEY_LOCAL_MACHINE=     nameof(HKEY_LOCAL_MACHINE);
        public const string HKEY_USERS=             nameof(HKEY_USERS);
        public const string HKEY_CURRENT_CONFIG=    nameof(HKEY_CURRENT_CONFIG);
    }
}
