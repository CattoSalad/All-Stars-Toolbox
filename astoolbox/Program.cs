using System;
using astoolbox.modules;

namespace astoolbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ASToolbox by Ciapa");
            Console.WriteLine("For updates, check:");
            Console.WriteLine("https://git.lewd.wtf/switch/modding/all-stars-toolbox\n\n");
            
            if (args.Length != 4)
            {
                PrintUsage();
                Environment.Exit(1);
            }

            IHandler handler = null;
            switch (args[0].ToLower())
            {
                case "har":
                    handler = new HARHandler();
                    break;
                case "nrolz":
                    handler = new NROLZHandler();
                    break;
                default:
                    PrintUsage();
                    Environment.Exit(1);
                    break;
            }

            switch (args[1].ToLower())
            {
                case "extract":
                    handler.Extract(args[2], args[3]);
                    break;
                case "compress":
                    handler.Compress(args[2], args[3]);
                    break;
                default:
                    PrintUsage();
                    Environment.Exit(1);
                    break;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("astool <FORMAT> <OPERATION> <SOURCE> <TARGET>");
        }
    }
}