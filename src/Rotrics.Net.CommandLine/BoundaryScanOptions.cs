using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace Rotrics.Net.CommandLine
{
    [ExcludeFromCodeCoverage]
    [Verb("boundary-scan", HelpText = "Find all the boundaries of the arm and output them to a file.")]
    public class BoundaryScanOptions
    {
        // ReSharper disable once StringLiteralTypo
        [Option('o', "outfile", Required = false, HelpText = "File to save the results of the scan to.", Default = ".\\boundaries.txt")]
        public string OutFile { get; set; }
    }
}