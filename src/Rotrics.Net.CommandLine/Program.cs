using System;
using CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using static System.Console;

namespace Rotrics.Net.CommandLine
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        private static Controller _controller;

        public static void Main(string[] arguments)
        {
            Parser.Default.ParseArguments<BoundaryScanOptions, InteractiveOptions>(arguments)
                  .WithParsed<BoundaryScanOptions>(BoundaryScan)
                  .WithParsed<InteractiveOptions>(o => { Interactive(); });
        }

        private static void Interactive()
        {
            if (! Connect())
            {
                return;
            }

            _controller.MoveToHome();

            WriteLine("Enter GCode commands or type 'quit' to exit.");
            WriteLine();

            while (true)
            {
                Write("GCode > ");

                var command = ReadLine() ?? string.Empty;

                if (command.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                _controller.SendRaw(command);

                var count = 0;

                while (true)
                {
                    Thread.Sleep(100);

                    var response = _controller.ReadRaw();

                    if (response.Equals("ok", StringComparison.InvariantCultureIgnoreCase))
                    {
                        break;
                    }

                    if (response.Equals("wait", StringComparison.InvariantCultureIgnoreCase))
                    {
                        count++;

                        if (count == 10)
                        {
                            break;
                        }

                        continue;
                    }

                    WriteLine(response);
                }
            }
        }

        private static void BoundaryScan(BoundaryScanOptions options)
        {
            if (! Connect())
            {
                return;
            }
        }

        private static bool Connect()
        {
            _controller = ControllerFactory.GetController();

            WriteLine("Attempting to connect to arm...");
            WriteLine();

            try
            {
                _controller.Connect();
            }
            catch
            {
                WriteLine("Failed to connect.");
                WriteLine();

                return false;
            }

            WriteLine("Connected.");
            WriteLine();

            return true;
        }
    }
}
