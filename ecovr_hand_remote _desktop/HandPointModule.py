import math
from typing import List


class HandPoint():
    """
    ### класс точки руки
    """

    def __init__(self, id: int, x: int, y: int):
        """
        :param id: номер точки кисти руки по классификации opencv
        :param x: положение по горизонтали
        :param y: положение по вертикали
        """
        self.id = id
        self.x = x
        self.y = y

    def get_point(self) -> tuple:
        """
        возвращает кортеж координат
        """
        return (self.x, self.y)


class HandPoints():
    """
    ### класс всех точек кисти руки
    """

    def __init__(self):
        # размер ( даже если включен детектирование нескольких рук, берем первые 21 точку)
        self.SIZE = 21
        # есть ли детектирование?
        self.is_detect = False  
        # список всех точек
        self.__hp_list: List[HandPoint] = []  
        for index in range(self.SIZE):
            self.__hp_list.append(HandPoint(index, -1, -1))
        # основание кисти
        self.wrist: HandPoint = self.__hp_list[0]  
        # внутреннее основание большого пальца
        self.thumb_cmc: HandPoint = self.__hp_list[1]
        # внешнее основание большого пальца
        self.thumb_mcp: HandPoint = self.__hp_list[2]
        # фаланга большого пальца
        self.thumb_ip: HandPoint = self.__hp_list[3]  
        # конец большого пальца
        self.thumb_tip: HandPoint = self.__hp_list[4]  
        # основание указательного пальца
        self.index_mcp: HandPoint = self.__hp_list[5]
        # середина указательного пальца
        self.index_pip: HandPoint = self.__hp_list[6]
        # фаланга указательного пальца
        self.index_dip: HandPoint = self.__hp_list[7]
        # конец указательного пальца
        self.index_tip: HandPoint = self.__hp_list[8]
        # основание среднего пальца
        self.middle_mcp: HandPoint = self.__hp_list[9]
        # середина среднего пальца
        self.middle_pip: HandPoint = self.__hp_list[10]
        # фаланга среднего пальца
        self.middle_dip: HandPoint = self.__hp_list[11]
        # конец среднего пальца
        self.middle_tip: HandPoint = self.__hp_list[12]
        # основание безымянного пальца
        self.ring_mcp: HandPoint = self.__hp_list[13]
        # середина безымянного пальца
        self.ring_pip: HandPoint = self.__hp_list[14]
        # фаланга безымянного пальца
        self.ring_dip: HandPoint = self.__hp_list[15]
        # конец безымянного пальца
        self.ring_tip: HandPoint = self.__hp_list[16]
        # основание мизинца
        self.pinky_mcp: HandPoint = self.__hp_list[17]
        # середина мизинца  
        self.pinky_pip: HandPoint = self.__hp_list[18]
        # фаланга мизинца  
        self.pinky_dip: HandPoint = self.__hp_list[19]
        # конец мизинца  
        self.pinky_tip: HandPoint = self.__hp_list[20]  

    def fill(self, landmarks: List[List[int]]):
        """
            ### заполнение точек (если список пустой координаты сбрасываются в (-1, -1))
            :param landmarks: Список точек из модуля HandTracking
        """
        self.is_detect = len(landmarks) > 0
        if self.is_detect:
            for index in range(self.SIZE):
                self.__hp_list[index].x, self.__hp_list[index].y = landmarks[index][1], landmarks[index][2]
        else:
            for index in range(self.SIZE):
                self.__hp_list[index].x, self.__hp_list[index].y = -1, -1

    def get_distance(self, hand_point_1: HandPoint, hand_point_2: HandPoint) -> int:
        """
        ### получение расстояния между точками
        :param hand_point_1: 1 точка
        :param hand_point_2: 2 точка
        """
        cathet_hor = math.fabs(hand_point_1.x - hand_point_2.x)
        cathet_ver = math.fabs(hand_point_1.y - hand_point_2.y)
        return int(math.hypot(cathet_hor, cathet_ver))

    def get_angle(self, hand_point_1: HandPoint, hand_point_2: HandPoint, hp_center: HandPoint) -> int:
        """
        ### получение угла между двумя прямыми 
        :param hand_point_1: 1 точка
        :param hand_point_2: 2 точка
        :param hp_center: вершина
        """
        AB = [hand_point_1.x - hp_center.x, hand_point_1.y - hp_center.y, ]
        AC = [hand_point_2.x - hp_center.x, hand_point_2.y - hp_center.y, ]
        scalar = AB[0] * AC[0] + AB[1] * AC[1]
        AB_length = math.sqrt(AB[0] ** 2 + AB[1] ** 2)
        AC_length = math.sqrt(AC[0] ** 2 + AC[1] ** 2)
        cos_angle = scalar / (AB_length * AC_length)
        angle_rad = math.acos(cos_angle)
        angle_deg = math.degrees(angle_rad)
        return int(angle_deg)

    def is_hand_position_right(self, min_angle: int = 25, max_angle: int = 60):
        """
        ### определение правильно ли расположена рука
        :param min_angle: минимальный лимит для угла (угол уменьшается при повороте руки вокруг своей оси)
        :param max_angle: максимальный лимит для угла (угол увеличивается при опускании кисти руки)
        """
        angle = self.get_angle(self.index_mcp, self.pinky_mcp, self.wrist)
        high_right = self.wrist.y > self.pinky_mcp.y and self.wrist.y > self.index_mcp.y
        angle_right = min_angle < angle < max_angle
        return high_right and angle_right
