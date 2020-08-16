using System;
using CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

            _controller.Dispose();
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
            if (File.Exists(options.OutFile))
            {
                File.Delete(options.OutFile);
            }

            using var output = new StreamWriter(options.OutFile)
                               {
                                   AutoFlush = true
                               };

            if (! Connect())
            {
                return;
            }

            _controller.MoveToHome();

            Wait();

            for (var z = -110; z <= 160; z++)
            {
                WriteLine($"Scanning Z{z}");

                _controller.SendRaw($"G0 Z{z}");

                Wait();

                for (var y = 400; y > 0; y--)
                {
                    _controller.SendRaw($"G0 Y{y}");

                    var response = _controller.ReadRaw();

                    if (response.Contains("beyond", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Wait();

                        continue;
                    }

                    WriteLine($"Y limit is {y}");

                    output.WriteLine($"Z{z} Y{y}");

                    break;
                }
            }

            output.Close();
        }

        private static void Wait()
        {
            while (! _controller.ReadRaw().Equals("wait", StringComparison.InvariantCultureIgnoreCase))
            {
                Thread.Sleep(100);
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
