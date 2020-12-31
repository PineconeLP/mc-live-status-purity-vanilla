import logging
from ..services.server_status_pinger import ping_server
import json
import os

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info("Executing Purity Vanilla ping from HTTP request.")

    host = os.environ["SERVER_HOST"]
    port = os.environ["SERVER_PORT"]

    server_status = ping_server(host, int(port))
    online_players = server_status.online
    max_players = server_status.max

    logging.info("Retrieved Purity Vanilla player data.")
    logging.info(f"Online: {online_players}")
    logging.info(f"Max: {max_players}")

    return func.HttpResponse(
        json.dumps({
            'online': online_players,
            'max': max_players
        }),
        status_code=200)
