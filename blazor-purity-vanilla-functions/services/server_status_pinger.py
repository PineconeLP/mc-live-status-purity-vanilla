from mcstatus import MinecraftServer
from models.server_status import ServerStatus


def ping_server(ip):
    server = MinecraftServer.lookup(ip)
    info = server.status()

    online_players = info.players.online
    max_players = info.players.max

    return ServerStatus(online_players, max_players)
