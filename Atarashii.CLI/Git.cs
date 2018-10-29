namespace Atarashii.CLI
{
    /// <summary>
    ///     Git repository information.
    /// </summary>
    public class Git
    {
        public static string Revision => new Resource("Atarashii.CLI.REVISION").Text;
    }
}