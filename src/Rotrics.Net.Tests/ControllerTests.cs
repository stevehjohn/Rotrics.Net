using NUnit.Framework;
using Rotrics.Net.Exceptions;

namespace Rotrics.Net.Tests
{
    [TestFixture]
    public class ControllerTests
    {
        private Controller _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new Controller();
        }

        [Test]
        [Explicit]
        public void Can_connect_to_arm()
        {
            Assert.DoesNotThrow(() => _controller.Connect());
        }

        [Test]
        [Explicit]
        public void Throws_exception_when_unable_to_connect()
        {
            Assert.Throws<RotricsConnectionException>(() => _controller.Connect());
        }
    }
}