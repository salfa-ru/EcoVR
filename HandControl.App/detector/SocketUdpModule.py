import socket

ENCODE = 'ascii'
SIZE = 1024

class SocketUdp():
    def __init__(self, hostname:str, port_received:int, port_send:int):
        self.hostname = hostname
        self.port_received = port_received
        self.port_send = port_send
        self.server_socket_send = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.server_socket_received = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.server_socket_received.bind((self.hostname, self.port_received))
                                 
    def send(self, message:str):
        self.server_socket_send.sendto(str.encode(message), (self.hostname, self.port_send))
       
    def listen(self):
        data, _ = self.server_socket_received.recvfrom(SIZE)
        return  data.decode(ENCODE)