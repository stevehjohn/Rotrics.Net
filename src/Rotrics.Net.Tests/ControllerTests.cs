﻿using Moq;
using NUnit.Framework;
using Rotrics.Net.Communications;
using Rotrics.Net.Exceptions;
using System.IO;

namespace Rotrics.Net.Tests
{
    [TestFixture]
    public class ControllerTests
    {
        private Mock<ISerialPort> _port;
        private Mock<ISerialPortEnumerator> _portEnumerator;

        private Controller _controller;

        [SetUp]
        public void SetUp()
        {
            _port = new Mock<ISerialPort>();

            _portEnumerator = new Mock<ISerialPortEnumerator>();

            var portFactory = new Mock<ISerialPortFactory>();

            portFactory.Setup(f => f.GetSerialPort())
                       .Returns(_port.Object);

            var portEnumeratorFactory = new Mock<ISerialPortEnumeratorFactory>();

            portEnumeratorFactory.Setup(f => f.GetSerialPortEnumerator())
                                 .Returns(_portEnumerator.Object);

            _controller = new Controller(portEnumeratorFactory.Object, portFactory.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [TestCase(0)]
        [TestCase(128)]
        [TestCase(255)]
        public void Passes_correct_value_to_laser_start(byte power)
        {
            SetupWorkingConnection();

            _controller.StartLaser(power);

            _port.Verify(p => p.Write($"M3 S{power}\r\n"));
        }

        [Test]
        public void StopLaser_passes_correct_command()
        {
            SetupWorkingConnection();

            _controller.StopLaser();

            _port.Verify(p => p.Write("M5\r\n"));
        }

        [Test]
        public void Throws_correct_exception_if_unable_to_connect()
        {
            _portEnumerator.Setup(pe => pe.GetPortNames())
                           .Returns(new[] { "COM3" });

            _port.Setup(p => p.Open())
                 .Throws<FileNotFoundException>();

            Assert.Throws<RotricsConnectionException>(() => _controller.Connect());
        }

        [Test]
        public void Write_throws_exception_if_not_connected()
        {
            Assert.Throws<RotricsConnectionException>(() => _controller.MoveToHome());
        }

        [Test]
        public void Throws_exception_if_command_attempted_without_MoveToHome_called_first()
        {
            _port.Setup(p => p.ReadLine())
                 .Returns("wait");

            _port.SetupGet(p => p.IsOpen)
                 .Returns(true);

            _portEnumerator.Setup(pe => pe.GetPortNames())
                           .Returns(new[] { "COM3" });

            _controller.Connect();

            Assert.Throws<RotricsCommandException>(() => _controller.MoveAbsolute(0, 300, 0));
        }

        [Test]
        public void MoveAbsolute_passes_coordinates_correctly()
        {
            SetupWorkingConnection();

            _controller.MoveAbsolute(123, 456, 789);

            _port.Verify(p => p.Write("G0 X123 Y456 Z789\r\n"));
        }

        [Test]
        public void Throws_exception_if_unable_to_locate_arm()
        {
            Assert.Throws<RotricsConnectionException>(() => _controller.Connect());
        }

        [Test]
        public void Connect_disposes_of_unused_ports()
        {
            _portEnumerator.Setup(pe => pe.GetPortNames())
                           .Returns(new[] { "COM3" });

            Assert.Throws<RotricsConnectionException>(() => _controller.Connect());

            _port.Verify(p => p.Dispose());
        }

        [Test]
        public void Grip_sends_correct_command()
        {
            SetupWorkingConnection();

            _controller.Grip();

            _port.Verify(p => p.Write("M1001\r\n"));
        }

        [Test]
        public void Release_sends_correct_command()
        {
            SetupWorkingConnection();

            _controller.Release();

            _port.Verify(p => p.Write("M1002\r\n"));
        }

        [Test]
        public void Suck_sends_correct_command()
        {
            SetupWorkingConnection();

            _controller.Suck();

            _port.Verify(p => p.Write("M1000\r\n"));
        }

        [Test]
        public void SendRaw_passes_command_through()
        {
            SetupWorkingConnection();

            _controller.SendRaw("G0 X0 Y300 Z0");

            _port.Verify(p => p.Write("G0 X0 Y300 Z0\r\n"));
        }

        [Test]
        public void ReadRaw_passes_back_received_data()
        {
            SetupWorkingConnection();

            _port.Setup(p => p.ReadLine())
                 .Returns("test");

            var response = _controller.ReadRaw();

            Assert.That(response, Is.EqualTo("test"));
        }

        private void SetupWorkingConnection()
        {
            _port.Setup(p => p.ReadLine())
                 .Returns("wait");

            _port.SetupGet(p => p.IsOpen)
                 .Returns(true);

            _portEnumerator.Setup(pe => pe.GetPortNames())
                           .Returns(new[] { "COM3" });

            _controller.Connect();

            _controller.MoveToHome();
        }
    }
}