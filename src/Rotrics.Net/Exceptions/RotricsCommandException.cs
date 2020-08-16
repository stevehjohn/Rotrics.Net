using System;

namespace Rotrics.Net.Exceptions
{
    public class RotricsCommandException : Exception
    {
        public RotricsCommandException(string message) : base(message)
        {
        }
    }
}