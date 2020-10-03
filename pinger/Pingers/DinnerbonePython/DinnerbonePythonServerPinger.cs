using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using Newtonsoft.Json;

namespace MCLiveStatus.Pinger.Pingers.DinnerbonePython
{
    public class DinnerbonePythonServerPinger : IServerPinger
    {
        public Task<ServerPingResponse> Ping(ServerAddress address)
        {
            return Ping(address.Host, address.Port);
        }

        public Task<ServerPingResponse> Ping(string host, int port)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("./Scripts/dist/main/main")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = $"{host} {port}"
            };

            using (Process process = Process.Start(processInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                ServerPlayers players = JsonConvert.DeserializeObject<ServerPlayers>(output);

                return Task.FromResult(new ServerPingResponse()
                {
                    OnlinePlayers = players.Online,
                    MaxPlayers = players.Max
                });
            }
        }
    }
}