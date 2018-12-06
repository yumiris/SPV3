using System;

namespace SPV3.Loader.CLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var config = new LoaderConfiguration();
            var loader = new Loader(config);

            try
            {
                if (args.Length == 0)
                {
                    loader.Start(ExecutableFactory.Detect());
                    return;
                }

                var parameters = new ParametersParser().Parse(string.Join(" ", args));
                var executable = args[0].Contains(Executable.Name)
                    ? new Executable(args[0])
                    : ExecutableFactory.Detect();

                loader.Start(executable, parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}