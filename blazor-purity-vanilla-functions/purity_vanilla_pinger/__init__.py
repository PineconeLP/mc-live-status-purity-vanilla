import logging
import json
import os
from ..services.server_status_pinger import ping_server
from ..models.error_code import ErrorCode

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info("Executing Purity Vanilla ping from HTTP request.")

    ip = os.environ["SERVER_HOST"]

    try:
        server_status = ping_server(ip)
    except:
        return func.HttpResponse(
            json.dumps({
                'errors': [
                    {
                        "code": ErrorCode.SERVER_PING_FAILED,
                        "message": "Unable to ping server."
                    }
                ]
            }),
            status_code=404)
    else:
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
