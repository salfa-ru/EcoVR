import cv2
from cvzone.HandTrackingModule import HandDetector

TITLE='preview'
PATH_ICON = 'handcontrol.ico'
WIN_WIDTH=3
WIN_HEIGHT=4

LM_LIST = 'lmlist'

DEFAULT_INDEX=0
DEFAULT_WIDTH=320
DEFAULT_HEIGHT=240
DEFAULT_MAX_HANDS=1
DEFAULT_DETECTION=0.8
DEFAULT_DETECT=True
DEFAULT_FLIP=False
DEFAULT_PREVIEW=True



class Capture():
    def __init__(self, 
                 index:int=DEFAULT_INDEX, 
                 width:int=DEFAULT_WIDTH, 
                 height:int=DEFAULT_HEIGHT, 
                 max_hands:int = DEFAULT_MAX_HANDS, 
                 detection:float = DEFAULT_DETECTION,
                 preview:bool=DEFAULT_PREVIEW,
                 detect:bool=DEFAULT_DETECT,
                 flip:bool=DEFAULT_FLIP,):
        self.index = index
        self.width = width
        self.height = height
        self.cap = cv2.VideoCapture(self.index)
        self.cap.set(WIN_WIDTH, width)
        self.cap.set(WIN_HEIGHT, height)
        self.detector = HandDetector(maxHands=max_hands, detectionCon=detection)
        self.preview = preview
        self.detect = detect
        self.flip = flip
        self.data = []
        self.stop = False
        self.sender = None
        self.isSend:bool = False
        self.limit_frame=[0,0,self.width,self.height]

    def start(self):
        while self.stop == False:
            img = self.__search_landmarks()
            img = cv2.rectangle(img, (self.limit_frame[0], self.limit_frame[1]), (self.limit_frame[2], self.limit_frame[3]), (0,255,255), 1)
            self.__show_preview(img)            

    def __search_landmarks(self):      
        _, img = self.cap.read()
        if self.flip: img = cv2.flip(img, 1)
        if self.detect:
            hands, img = self.detector.findHands(img)
            for hand in hands:
                self.save(hand)         
        return img


    def save(self, current):
        self.data = current
        if self.isSend and self.sender != None:
            self.sender(self.data)

    def __show_preview(self, img):
        if self.preview:
            cv2.imshow(TITLE, img)
            cv2.waitKey(1)
        else:
             cv2.destroyAllWindows()
