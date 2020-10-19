using System;
using System.Threading.Tasks;
using System.Timers;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using MCLiveStatus.Pinger.Schedulers;
using Moq;
using NUnit.Framework;

namespace MCLiveStatus.Pinger.Tests.Schedulers
{
    [TestFixture]
    public class TimerServerPingerSchedulerHandlerTests
    {
        private TimerServerPingerSchedulerHandler _handler;

        private Timer _timer;

        [SetUp]
        public void Setup()
        {
            _timer = new Timer();
            _timer.Start();

            _handler = new TimerServerPingerSchedulerHandler(_timer);
        }

        [Test]
        public async Task StopPingSchedule_DisposesTimer()
        {
            bool disposed = false;
            _timer.Disposed += (s, e) => disposed = true;

            await _handler.StopPingSchedule();

            Assert.IsTrue(disposed);
        }

        [Test]
        public async Task StopPingSchedule_SetsIsStoppedToTrue()
        {
            await _handler.StopPingSchedule();

            Assert.IsTrue(_handler.IsStopped);
        }
    }
}