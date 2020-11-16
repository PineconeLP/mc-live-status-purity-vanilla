import logging
from mcstatus import MinecraftServer
import json

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info("Executing Purity Vanilla ping from HTTP request.")

    server = MinecraftServer("purityvanilla.com", 25565)
    info = server.status()
    online_players = info.players.online
    max_players = info.players.max

    logging.info("Retrieved Purity Vanilla player data.")
    logging.info(f"Online: {online_players}")
    logging.info(f"Max: {max_players}")

    return func.HttpResponse(
        json.dumps({
            'online': online_players,
            'max': max_players
        }),
        status_code=200)
