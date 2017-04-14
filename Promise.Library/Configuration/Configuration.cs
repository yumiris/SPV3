namespace Promise.Library.Configuration
{
    public abstract class Configuration : IConfiguration
    {
        protected string GetParameterString(string parameter, bool toggle)
        {
            return toggle ? parameter : string.Empty;
        }

        public abstract string GetConfiguration();
    }
}
