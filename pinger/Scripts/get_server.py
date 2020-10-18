from mcstatus import MinecraftServer


def get_server(args):
    host = args[1]

    has_host_only = args.__len__() == 2

    if(has_host_only):
        return MinecraftServer.lookup(host)
    else:
        port = int(args[2])
        return MinecraftServer(host, port)
