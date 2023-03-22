import cv2
import mediapipe as mp
from HandPointModule import HandPoint
from HandPointModule import HandPoints
from typing import List

class HandDetector():
    """
        ## модуль детектирования руки
        входные параметры настраивают модуль mediapipe
    """

    def __init__(self, mode: bool = False, max_hands: int = 1, tracking: float = 0.5, detection: float = 0.5):
        """
        :param mode: статический режим (False)
        :param max_hands: максимально определяемое количество рук (2)
        :param tracking: точность отслеживания (0.5)
        :param detection: точность детектирования (0.5)
        """
        self.mode: bool = mode  
        self.max_hands: int = max_hands
        self.tracking: float = tracking
        self.detection: float = detection
        self.mediapipe_hands = mp.solutions.hands
        self.hands = self.mediapipe_hands.Hands(static_image_mode=self.mode,
                                                max_num_hands=self.max_hands,
                                                min_detection_confidence=self.detection,
                                                min_tracking_confidence=self.tracking)
        self.mediapipe_draw = mp.solutions.drawing_utils
        self.hand_points:HandPoints = HandPoints()

    def find_hands(self, img: cv2.Mat) -> cv2.Mat:
        """
            ### поиск рук
            :param img: исходная картинка
        """
        imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        self.results = self.hands.process(imgRGB)
        return img

    def detect_position(self, img: cv2.Mat, hand_number: int = 0) -> List[HandPoint]:
        """
        ### детектирование позиций
        :param img: исходная картинка
        :param hand_number: номер руки (первая по умолчанию, не может превышать max_hands)
        """
        landmarks: List[int] = []
        if self.results.multi_hand_landmarks:
            hand = self.results.multi_hand_landmarks[hand_number]
            for index, landmark in enumerate(hand.landmark):
                height, width, _ = img.shape
                x, y = int(landmark.x * width), int(landmark.y * height)
                landmarks.append([index, x, y])
        self.hand_points.fill(landmarks)
        return self.hand_points

    def draw_point(self, img: cv2.Mat, hand_point: HandPoint, radius: int = 2, color: tuple = (0, 0, 255)) -> None:
        """
        ### рисует заполненный круг

        :param img: исходная картинка
        :param hand_point: точка на руке
        :param radius: радиус круга (2)
        :param color: цвет заливки ((0, 0, 255) - красный в BGR)
        """
        if hand_point.x != -1 and hand_point.y != -1:
            cv2.circle(img, (hand_point.x, hand_point.y),
                       radius, color, -1, cv2.FILLED)

    def draw_text(self, img: cv2.Mat, message: str, color: tuple = (255, 255, 255), position: tuple = (10, 10)):
        """
        ### рисует текст на картинке
        :param img: исходная картинка
        :param message: текст
        :param color: цвет заливки ((255, 255, 255) - белый в BGR)
        :param position: точка отрисовки (10, 10)
        """
        cv2.putText(img, message, position,
                    cv2.FONT_HERSHEY_COMPLEX_SMALL, 0.5, color, 1)

    def draw_connection(self, img):
        """ 
        ### рисует стандартный экзоскелет кисти руки
        :param img: исходная картинка
        """
        if self.results.multi_hand_landmarks:
            for hand_landmarks in self.results.multi_hand_landmarks:
                self.mediapipe_draw.draw_landmarks(
                    img, hand_landmarks, self.mediapipe_hands.HAND_CONNECTIONS)
                
    def draw_line(self, img: cv2.Mat, hand_point_1: HandPoint, hand_point_2: HandPoint, color: tuple = (0, 255, 255), thickness: int = 1) -> None:
        """
        ### рисует линию между точками
        :param img: исходная картинка
        :param hand_point_1: 1 точка на руке
        :param hand_point_2: 2 точка на руке
        :param color: цвет заливки ((0, 0, 255) - желтый в BGR)
        :param thickness: толщина линии (1)
        """
        cv2.line(img, hand_point_1.get_point(),
                hand_point_2.get_point(), color, thickness)


def main():
    cap = cv2.VideoCapture(1)
    detector = HandDetector(max_hands=1, detection=0.7, tracking=0.5)
    while True:
        _, img = cap.read()
        img = detector.find_hands(img)
        cv2.imshow("Image", img)
        if cv2.waitKey(1) & 0xFF == 27:
            break


if __name__ == "__main__":
    main()
