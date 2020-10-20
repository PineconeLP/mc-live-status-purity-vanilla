import sys
from get_server import get_server

server = get_server(sys.argv)

info = server.status()

print(info.raw['players'])
