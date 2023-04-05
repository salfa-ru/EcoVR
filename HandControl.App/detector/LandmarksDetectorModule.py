import threading
import time
from ConfigurationModule import Configuration
from SocketUdpModule import SocketUdp
from CaptureModule import Capture
from ConversationModule import Conversation

PATH = "settings.ini"

SLEEP_TIME = 0.2

STATUS_STOPPED = 'stopped'
STATUS_CAPTURED = 'captured'
STATUS_PREPARE = 'prepare'
STATUS_LISTEN = 'listen'

JSON_STATUS = 'Status'
JSON_STATUS_MAIN = 'main'
JSON_STATUS_CAMERA = 'camera'
JSON_STATUS_DATA = 'data'
JSON_STATUS_DRAW = 'draw'

JSON_COMMAND = 'Command'
JSON_COMMAND_KILL = 'kill'
JSON_COMMAND_STOP = 'stop'
JSON_COMMAND_START = 'start'
JSON_COMMAND_RESTART = 'restart'
JSON_COMMAND_RESOLUTION = 'resolution'
JSON_COMMAND_FLAGS = 'flags'
JSON_COMMAND_START_SEND = 'start_send'
JSON_COMMAND_STOP_SEND = 'stop_send'
JSON_COMMAND_RECT = 'rectangle'
JSON_COMMAND_CIRCLE = 'circle'
JSON_COMMAND_LINE = 'line'


JSON_ARGS = 'MessageArgs'
JSON_ARGS_IS_PREVIEW = 'IsPreview'
JSON_ARGS_IS_FLIP = 'IsFlip'
JSON_ARGS_IS_DETECT = 'IsDetect'
JSON_ARGS_INDEX = 'Index'
JSON_ARGS_WIDTH = 'Width'
JSON_ARGS_HEIGHT = 'Height'
JSON_ARGS_LEFT = 'Left'
JSON_ARGS_RIGHT = 'Right'
JSON_ARGS_TOP = 'Top'
JSON_ARGS_BOTTOM = 'Bottom'




class LandmarksDetector():
    def __init__(self, first, second):
        self.socket_udp: SocketUdp = None
        self.config = Configuration(PATH)
        self.config.port_received = int(first)
        self.config.port_send = int(second)
        self.socket_udp = SocketUdp(self.config.hostname, self.config.port_received, self.config.port_send)
        self.capture: Capture = None
        self.conversation = Conversation()
        self.status: str = STATUS_STOPPED

    def listener(self):
        while True:
            data = self.socket_udp.listen()
            cm = self.conversation.get_command(data)
            print(cm)
            if cm[JSON_STATUS] == JSON_STATUS_MAIN \
                    and cm[JSON_COMMAND] == JSON_COMMAND_KILL:
                break
            if cm[JSON_STATUS] == JSON_STATUS_MAIN \
                    and cm[JSON_COMMAND] == JSON_COMMAND_STOP \
                    and self.status == STATUS_CAPTURED:
                self.stop_detect()
                continue
            if cm[JSON_STATUS] == JSON_STATUS_MAIN \
                    and cm[JSON_COMMAND] == JSON_COMMAND_START \
                    and self.status == STATUS_STOPPED:
                self.start_detect(cm)
                continue
            if cm[JSON_STATUS] == JSON_STATUS_CAMERA \
                    and cm[JSON_COMMAND] == JSON_COMMAND_RESOLUTION:
                self.set_resolution(cm)
                continue
            if cm[JSON_STATUS] == JSON_STATUS_CAMERA \
                    and cm[JSON_COMMAND] == JSON_COMMAND_FLAGS \
                    and self.status == STATUS_CAPTURED:
                self.set_frame(cm)
                continue
            if cm[JSON_STATUS] == JSON_STATUS_DATA \
                    and cm[JSON_COMMAND] == JSON_COMMAND_START_SEND \
                    and self.status == STATUS_CAPTURED:
                self.capture.isSend = True
            if cm[JSON_STATUS] == JSON_STATUS_DATA \
                    and cm[JSON_COMMAND] == JSON_COMMAND_STOP_SEND \
                    and self.status == STATUS_CAPTURED:
                self.capture.isSend = False
            if cm[JSON_STATUS] == JSON_STATUS_DRAW \
                and cm[JSON_COMMAND] == JSON_COMMAND_RECT \
                and self.status == STATUS_CAPTURED:
                self.set_frame_limits(cm)

    def set_frame_limits(self, cm):
        self.capture.limit_frame[0] = cm[JSON_ARGS][JSON_ARGS_LEFT]
        self.capture.limit_frame[1] = cm[JSON_ARGS][JSON_ARGS_TOP]
        self.capture.limit_frame[2] = cm[JSON_ARGS][JSON_ARGS_RIGHT]
        self.capture.limit_frame[3] = cm[JSON_ARGS][JSON_ARGS_BOTTOM]


    def send_data(self, data):
        mess = self.conversation.get_data(
            (
                self.capture.index,
                self.capture.width,
                self.capture.height
            ),
            (
                self.capture.preview,
                self.capture.detect,
                self.capture.flip,
                data))
        self.socket_udp.send(str(mess))

    def stop_detect(self):
        self.capture.preview = False
        time.sleep(SLEEP_TIME)
        self.capture.stop = True
        self.capture = None
        self.send_status(STATUS_STOPPED)

    def send_status(self, status):
        self.status = status
        self.socket_udp.send(self.conversation.get_reg(self.status))

    def set_frame(self, cm):
        self.capture.preview = cm[JSON_ARGS][JSON_ARGS_IS_PREVIEW]
        self.config.preview = cm[JSON_ARGS][JSON_ARGS_IS_PREVIEW]
        self.capture.flip = cm[JSON_ARGS][JSON_ARGS_IS_FLIP]
        self.config.flip = cm[JSON_ARGS][JSON_ARGS_IS_FLIP]
        self.capture.detect = cm[JSON_ARGS][JSON_ARGS_IS_DETECT]
        self.config.detect = cm[JSON_ARGS][JSON_ARGS_IS_DETECT]

    def set_resolution(self, cm):
        self.stop_detect()
        self.start_detect(cm)

    def detection(self):
        self.send_status(STATUS_PREPARE)
        self.capture = Capture(
            index=self.config.index,
            width=self.config.width,
            height=self.config.height,
            max_hands=self.config.max_hands,
            detection=self.config.detection,
            preview=self.config.preview,
            detect=self.config.detect,
            flip=self.config.flip)
        self.capture.sender = lambda data: self.socket_udp.send(
            str(self.conversation.get_data((
                self.capture.index,
                self.capture.width,
                self.capture.height),
                (self.capture.preview,
                 self.capture.detect,
                 self.capture.flip, data))))
        self.send_status(STATUS_CAPTURED)
        self.capture.start()

    def start(self):
        self.send_status(STATUS_STOPPED)
        self.listener()
        self.config.save_config(PATH)

    def start_detect(self, cm):
        self.config.index = cm[JSON_ARGS][JSON_ARGS_INDEX]
        self.config.width = cm[JSON_ARGS][JSON_ARGS_WIDTH]
        self.config.height = cm[JSON_ARGS][JSON_ARGS_HEIGHT]
        detect_thread = threading.Thread(target=self.detection, args=())
        detect_thread.daemon = True
        detect_thread.start()
