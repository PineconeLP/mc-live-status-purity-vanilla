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
        private Mock<IServerPingerSchedulerHandler> _mockSchedulerHandler;

        private ServerAddress _serverAddress = new ServerAddress(It.IsAny<string>(), 25565);

        [SetUp]
        public void SetUp()
        {
            _mockScheduler = new Mock<IServerPingerScheduler>();
            _mockSchedulerHandler = new Mock<IServerPingerSchedulerHandler>();

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
            _mockScheduler.Setup(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>())).ReturnsAsync(_mockSchedulerHandler.Object);
            await _repeatingPinger.Start(seconds);

            await _repeatingPinger.Start(seconds);

            _mockScheduler.Verify(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>()), Times.Once);
        }

        [Test]
        public async Task UpdateServerPingSecondsInterval_WithRunning_UpdatesSchedulerHandler()
        {
            double initialIntervalInSeconds = 5;
            double intervalInSeconds = 10;
            _mockScheduler.Setup(s => s.Schedule(_serverAddress, initialIntervalInSeconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>())).ReturnsAsync(_mockSchedulerHandler.Object);
            await _repeatingPinger.Start(initialIntervalInSeconds);

            await _repeatingPinger.UpdateServerPingSecondsInterval(intervalInSeconds);

            _mockSchedulerHandler.Verify(h => h.UpdatePingScheduleInterval(intervalInSeconds), Times.Once);
        }

        [Test]
        public async Task UpdateServerPingSecondsInterval_WithoutRunning_DoesNotUpdateSchedulerHandler()
        {
            double intervalInSeconds = 10;

            await _repeatingPinger.UpdateServerPingSecondsInterval(intervalInSeconds);

            _mockSchedulerHandler.Verify(h => h.UpdatePingScheduleInterval(intervalInSeconds), Times.Never);
        }

        [Test]
        public async Task Stop_WithRunning_StopsScheduler()
        {
            int seconds = 10;
            _mockScheduler.Setup(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>())).ReturnsAsync(_mockSchedulerHandler.Object);
            await _repeatingPinger.Start(seconds);

            await _repeatingPinger.Stop();

            _mockScheduler.Verify(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>()), Times.Once);
            _mockSchedulerHandler.Verify(s => s.StopPingSchedule(), Times.Once);
        }

        [Test]
        public async Task Stop_WithoutRunning_DoesNotStopScheduler()
        {
            int seconds = 10;
            _mockScheduler.Setup(s => s.Schedule(_serverAddress, seconds, It.IsAny<Action<ServerPingResponse>>(), It.IsAny<Action<Exception>>())).ReturnsAsync(_mockSchedulerHandler.Object);

            await _repeatingPinger.Stop();

            _mockSchedulerHandler.Verify(s => s.StopPingSchedule(), Times.Never);
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