import sys
import json
from mcstatus import MinecraftServer

host = sys.argv[1]
port = int(sys.argv[2])

if(port == 0):
    server = MinecraftServer.lookup(host)
else:
    server = MinecraftServer(host, port)

info = server.status()

print(json.dumps({"online": info.players.online, "max": info.players.max}))
