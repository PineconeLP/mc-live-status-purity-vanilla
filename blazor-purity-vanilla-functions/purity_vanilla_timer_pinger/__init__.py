import datetime
import logging
import json
import os
from ..services.server_status_pinger import ping_server

import azure.functions as func


def main(mytimer: func.TimerRequest, outMessage: func.Out[str]) -> None:
    logging.info("Executing Purity Vanilla ping.")

    host = os.environ["SERVER_HOST"]
    port = os.environ["SERVER_PORT"]

    server_status = ping_server(host, int(port))
    online_players = server_status.online
    max_players = server_status.max

    logging.info("Retrieved Purity Vanilla player data.")
    logging.info(f"Online: {online_players}")
    logging.info(f"Max: {max_players}")

    outMessage.set(json.dumps({
        'target': 'ping',
        'arguments': [online_players, max_players]
    }))
