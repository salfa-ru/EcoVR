import configparser

NET = 'Net'
CAMERA = 'Camera'
DETECTOR='Detector'
IMAGE='Image'

HOSTNAME = 'hostname'
PORT_SEND='port_send'
PORT_RECEIVED='port_received'

INDEX='index'
WIDTH='width'
HEIGHT='height'

MAX_HANDS='max_hands'
DETECTION='detection'

DETECT='detect'
FLIP='flip'
PREVIEW='preview'

TRUE='true'
FALSE='false'

DEFAULT_HOST='localhost'
DEFAULT_PORT_RECEIVE=6000
DEFAULT_PORT_SEND=6001
DEFAULT_CAMERA_NUMBER=0
DEFAULT_CAMERA_WIDTH=320
DEFAULT_CAMERA_HEIGHT=240
DEFAULT_MAX_HANDS=1
DEFAULT_DETECTION=0.8

class Configuration():
    def __init__(self, config_file_path:str='',
                hostname:str=DEFAULT_HOST,                
                port_in:int=DEFAULT_PORT_RECEIVE,  
                port_out:int=DEFAULT_PORT_SEND,  
                index:int=DEFAULT_CAMERA_NUMBER, 
                width:int=DEFAULT_CAMERA_WIDTH, 
                height:int=DEFAULT_CAMERA_HEIGHT,
                max_hands:int=DEFAULT_MAX_HANDS,
                detection:float=DEFAULT_DETECTION):
        self.hostname:str= hostname
        self.port_received:int = port_in
        self.port_send:int = port_out
        self.index:int = index
        self.width:int = width
        self.height:int = height
        self.max_hands=max_hands
        self.detection=detection
        self.preview = True
        self.flip = True
        self.detect = True

        if config_file_path!='':
            self.parse_config(config_file_path)

    def parse_config(self, path:str):
        settings = configparser.ConfigParser()
        settings.read(path)

        self.hostname = settings[NET][HOSTNAME]
        self.port_received = int(settings[NET][PORT_RECEIVED]) 
        self.port_send = int(settings[NET][PORT_SEND]) 

        self.index = int(settings[CAMERA][INDEX])
        self.width = int(settings[CAMERA][WIDTH])
        self.height = int(settings[CAMERA][HEIGHT])
        
        self.max_hands = int(settings[DETECTOR][MAX_HANDS])
        self.detection = float(settings[DETECTOR][DETECTION])

        self.detect = settings[IMAGE].getboolean(DETECT)
        self.flip = settings[IMAGE].getboolean(FLIP)
        self.preview = settings[IMAGE].getboolean(PREVIEW)

    def save_config(self, path:str):
        settings = configparser.ConfigParser()

        settings[NET] = {}
        settings[NET][HOSTNAME] = self.hostname
        settings[NET][PORT_RECEIVED] = str(self.port_received)
        settings[NET][PORT_SEND] = str(self.port_send)

        settings[CAMERA] = {}
        settings[CAMERA][INDEX] = str(self.index)
        settings[CAMERA][WIDTH] = str(self.width)
        settings[CAMERA][HEIGHT] = str(self.height)

        settings[DETECTOR] = {}
        settings[DETECTOR][MAX_HANDS] = str(self.max_hands)
        settings[DETECTOR][DETECTION] = str(self.detection)

        settings[IMAGE] = {}
        settings[IMAGE][DETECT] = TRUE if self.detect else FALSE
        settings[IMAGE][FLIP] = TRUE if self.flip else FALSE
        settings[IMAGE][PREVIEW] = TRUE if self.preview else FALSE

        with open(path, 'w') as config_file:
            settings.write(config_file)