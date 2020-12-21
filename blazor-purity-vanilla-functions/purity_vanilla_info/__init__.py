import logging
import os
import json

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info(
        "Executing Purity Vanilla max players excluding queue request.")

    host = os.environ["SERVER_HOST"]
    port = os.environ["SERVER_PORT"]
    name = os.environ["SERVER_NAME"]
    has_queue = os.environ["SERVER_HAS_QUEUE"]
    max_players_excluding_queue = os.environ["SERVER_MAX_PLAYERS_EXCLUDING_QUEUE"]

    logging.info(f"Host: {host}")
    logging.info(f"Port: {port}")
    logging.info(f"Name: {name}")
    logging.info(f"Has Queue: {has_queue}")
    logging.info(f"Max Players Excluding Queue: {max_players_excluding_queue}")

    return func.HttpResponse(
        json.dumps({
            'host': host,
            'port': port,
            'name': name,
            'hasQueue': has_queue,
            'maxPlayersExcludingQueue': max_players_excluding_queue,
        }),
        status_code=200)
