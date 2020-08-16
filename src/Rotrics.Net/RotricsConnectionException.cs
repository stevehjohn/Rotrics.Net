using System;

namespace Rotrics.Net
{
    public class RotricsConnectionException : Exception
    {
        public RotricsConnectionException(string message) : base(message)
        {
        }
    }
}