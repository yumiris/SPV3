using System.Linq;
using System.Diagnostics;
using System;

namespace Validator
{
    class Program
    {
        //possible improvement return processId to increase integrity
        static void Main(string[] args)
        {
            try
            {
                int processId;
                if (!int.TryParse(args[0], out processId))
                    Console.WriteLine("false");

                var selectedProcess = Process.GetProcessById(processId);
                if (selectedProcess.Modules.Cast<ProcessModule>().Any(moduleName => moduleName.ModuleName == "halo1.dll"))
                {
                    Console.WriteLine("true");
                }
                else
                    Console.WriteLine("false");
            }
            catch
            {
                Console.WriteLine("false");
            }
        }
    }
}
