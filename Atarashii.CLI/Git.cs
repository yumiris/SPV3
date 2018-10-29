namespace Atarashii.CLI
{
    /// <summary>
    ///     Git repository information.
    /// </summary>
    public class Git
    {
        public static string Revision => ResourceFactory.Get(ResourceFactory.Type.Revision).Text;
    }
}