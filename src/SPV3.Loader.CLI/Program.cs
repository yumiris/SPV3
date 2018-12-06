namespace SPV3.Loader.CLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new Loader(new LoaderConfiguration()).Start(ExecutableFactory.Detect(),
                new ParametersParser().Parse(string.Join(" ", args)));
        }
    }
}