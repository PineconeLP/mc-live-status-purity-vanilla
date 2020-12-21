import logging
import os
import json

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info(
        "Executing Purity Vanilla max players excluding queue request.")

    max_players_excluding_queue = os.environ["MAX_PLAYERS_EXCLUDING_QUEUE"]
    logging.info(f"Max Players Excluding Queue: {max_players_excluding_queue}")

    return func.HttpResponse(
        json.dumps({
            'maxPlayersExcludingQueue': max_players_excluding_queue
        }),
        status_code=200)
