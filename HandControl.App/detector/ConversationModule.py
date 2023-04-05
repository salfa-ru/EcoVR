import json

RESOLUTION = 'resolution'
DETECTION = 'detection'
INDEX = 'index'
WIDTH = 'width'
HEIGHT = 'height'
PREVIEW='preview'
FLIP='flip'
DETECT='detect'
DATA='data'
STATUS='status'
FALSE='false'

class Conversation():
    def __init__(self):
        self.data = {
            RESOLUTION : {
                    INDEX : 0,
                    WIDTH : 0,
                    HEIGHT : 0
            },
            DETECTION:{
                PREVIEW:FALSE,
                DETECT:FALSE,
                FLIP:FALSE,
                DATA: [],
            }           
        }
        self.connect = {
            STATUS:""
        }  

    def get_reg(self, status:str):
        self.connect[STATUS] = status
        return json.dumps(self.connect)

    def get_data(self,
                 resolution:tuple=(int,int,int), 
                 detection:tuple=(bool,bool,bool,[])):
        self.data[RESOLUTION][INDEX] = resolution[0]
        self.data[RESOLUTION][WIDTH] = resolution[1]
        self.data[RESOLUTION][HEIGHT] = resolution[2]
        self.data[DETECTION][PREVIEW] = detection[0]
        self.data[DETECTION][DETECT] = detection[1]
        self.data[DETECTION][FLIP] = detection[2]
        self.data[DETECTION][DATA] = detection[3]
        return json.dumps(self.data)

    def get_command(self, js:str):
        return json.loads(js)
        