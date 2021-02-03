import logging
import os
import json

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info(
        "Executing Purity Vanilla max players excluding queue request.")

    ip = os.environ["SERVER_IP"]
    name = os.environ["SERVER_NAME"]
    has_queue = os.environ["SERVER_HAS_QUEUE"]
    max_players_excluding_queue = os.environ["SERVER_MAX_PLAYERS_EXCLUDING_QUEUE"]

    logging.info(f"IP: {ip}")
    logging.info(f"Name: {name}")
    logging.info(f"Has Queue: {has_queue}")
    logging.info(f"Max Players Excluding Queue: {max_players_excluding_queue}")

    return func.HttpResponse(
        json.dumps({
            'ip': ip,
            'name': name,
            'hasQueue': has_queue,
            'maxPlayersExcludingQueue': max_players_excluding_queue,
        }),
        status_code=200)
