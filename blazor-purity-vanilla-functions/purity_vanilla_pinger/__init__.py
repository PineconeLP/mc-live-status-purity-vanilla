from mcstatus import MinecraftServer
import datetime
import logging

import azure.functions as func


def main(mytimer: func.TimerRequest) -> None:
    server = MinecraftServer("purityvanilla.com", 25565)

    info = server.status()

    logging.info(info.raw['players'])
