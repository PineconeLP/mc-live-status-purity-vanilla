using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using MCLiveStatus.Pinger.Schedulers;
using Moq;
using NUnit.Framework;

namespace MCLiveStatus.Pinger.Tests.Pingers
{
    [TestFixture]
    public class RepeatingServerPingerTests
    {
        private RepeatingServerPinger _repeatingPinger;

        private Mock<IServerPingerScheduler> _mockScheduler;

        private ServerAddress _serverAddress = new ServerAddress(It.IsAny<string>(), 25565);

        [SetUp]
        public void SetUp()
        {
            _mockScheduler = new Mock<IServerPingerScheduler>();

            _repeatingPinger = new RepeatingServerPinger(_serverAddress, _mockScheduler.Object);
        }

        [Test]
        public async Task Start_WithoutRunning_StartsScheduler()
        {
            int seconds = 10;

            await _repeatingPinger.Start(seconds);

            _mockScheduler.Verify(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>()));
        }

        [Test]
        public async Task Start_WithRunning_DoesNotStartScheduler()
        {
            int seconds = 10;
            _mockScheduler.Setup(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>())).ReturnsAsync(() => Task.CompletedTask);

            await _repeatingPinger.Start(seconds);
            await _repeatingPinger.Start(seconds);

            _mockScheduler.Verify(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>()), Times.Once);
        }

        [Test]
        public async Task Stop_WithRunning_StopsScheduler()
        {
            bool stopped = false;
            int seconds = 10;
            _mockScheduler.Setup(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>())).ReturnsAsync(() => Task.Run(() => stopped = true));

            await _repeatingPinger.Start(seconds);
            await _repeatingPinger.Stop();

            _mockScheduler.Verify(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>()), Times.Once);
            Assert.IsTrue(stopped);
        }

        [Test]
        public async Task Stop_WithoutRunning_DoesNotStopScheduler()
        {
            bool stopped = false;
            int seconds = 10;
            _mockScheduler.Setup(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>())).ReturnsAsync(() => Task.Run(() => stopped = true));

            await _repeatingPinger.Stop();

            Assert.IsFalse(stopped);
        }

        [Test]
        public async Task Start_OnPingCompleted_RaisesPingCompletedFromScheduler()
        {
            int seconds = 10;
            ServerPingResponse expectedPingResponse = new ServerPingResponse();
            ServerPingResponse actualPingResponse = null;
            _mockScheduler.Setup(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>()))
                .Callback<ServerAddress, double, Action<ServerPingResponse>, Action<Exception>>((a, s, onPing, onEx) => onPing?.Invoke(expectedPingResponse));
            _repeatingPinger.PingCompleted += (r) => actualPingResponse = r;

            await _repeatingPinger.Start(seconds);

            Assert.IsNotNull(actualPingResponse);
            Assert.AreEqual(expectedPingResponse, actualPingResponse);
        }
    }
}