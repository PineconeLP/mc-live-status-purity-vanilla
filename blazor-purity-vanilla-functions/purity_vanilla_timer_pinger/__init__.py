from mcstatus import MinecraftServer
import datetime
import logging
import json

import azure.functions as func


def main(mytimer: func.TimerRequest, outMessage: func.Out[str]) -> None:
    logging.info("Executing Purity Vanilla ping.")

    server = MinecraftServer("purityvanilla.com", 25565)
    info = server.status()
    online_players = info.players.online
    max_players = info.players.max

    logging.info("Retrieved Purity Vanilla player data.")
    logging.info(f"Online: {online_players}")
    logging.info(f"Max: {max_players}")

    outMessage.set(json.dumps({
        'target': 'ping',
        'arguments': [online_players, max_players]
    }))
