using System;

namespace Rotrics.Net.Exceptions
{
    public class RotricsConnectionException : Exception
    {
        public RotricsConnectionException(string message) : base(message)
        {
        }
    }
}