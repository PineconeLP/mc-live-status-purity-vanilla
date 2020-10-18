import sys
import json
from get_server import get_server

server = get_server(sys.argv)

info = server.status()

print(json.dumps({"online": info.players.online, "max": info.players.max}))
