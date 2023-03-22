import cv2
import mouse
from HandTrackingModule import HandDetector
from HandPointModule import HandPoint
from HandPointModule import HandPoints


class MouseHandRemote():
    """
    ## модуль управления курсором
    """

    def __init__(self,
                 non_sensitivity_zone_hor: float = 0.1,
                 non_sensitivity_zone_ver: float = 0.2,
                 non_sensitivity_zone_left_click: float = 0.4,
                 non_sensitivity_zone_right_click: float = 0.4,
                 non_sensitivity_zone_wheel: float = 0.4,
                 mouse_wheel_speed=0.35):
        """
            #### параметры зависящие от калибровочного замера
                - `non_sensitivity_zone_hor` -> зона не чувствительности для горизонтального перемещения курсора;
                - `non_sensitivity_zone_ver` -> зона не чувствительности для вертикального перемещения курсора;
                - `non_sensitivity_zone_left_click` -> зона не чувствительности для сработки левого клика;
                - `non_sensitivity_zone_right_click` -> зона не чувствительности для сработки правого клика;
                - `non_sensitivity_zone_wheel` -> зона не чувствительности для сработки скроллинга
                - `mouse_wheel_speed` -> скорость скроллинга колесом мыши
        """
        self.non_sensitivity_zone_hor = non_sensitivity_zone_hor
        self.non_sensitivity_zone_ver = non_sensitivity_zone_ver
        self.non_sensitivity_zone_left_click = non_sensitivity_zone_left_click
        self.non_sensitivity_zone_right_click = non_sensitivity_zone_right_click
        self.non_sensitivity_zone_wheel = non_sensitivity_zone_wheel
        self.mouse_wheel_speed: float = mouse_wheel_speed
        self.__dif_x = 0
        self.__dif_y = 0        
        self.__lim_x = 0
        self.__lim_y = 0
        self.__dif_wheel = 0
        self.__mouse_x = 0
        self.__mouse_y = 0
        self.__calibration_distance = 0
        self.__left_click_limit = 0
        self.__left_click_distance = 0
        self.__right_click_limit = 0
        self.__right_click_distance = 0
        self.is_left_down = False
        self.is_right_down = False
        self.__left_down = False
        self.__right_down = False

    def remote(self, hand_points: HandPoints, smooth: int = 5):
        """
            #### управление курсором

            входные параметры:
            - `hand_points:HandPoints` -> задетектированные точки руки
            - `smooth:int` -> сглаживание

            1. если ничего не задетектировано -> выход
            2. делаем калибровочный замер
            3. если положение руки верно включаем управление
            4. если поднят мизинец - включаем перемещение
                1. двигаем курсор по горизонтали по положению большого пальца
                2. двигаем курсор по вертикали по положению среднего пальца
            5. если режим перемещения выключен
                1. измеряем расстояние между средним и большим пальцем - при малом вызываем правый клик
                2. двигаем скрол по положению указательного пальца
            6. вне зависисмости от режима перемещения
                1. измеряем расстояние между указательным и большим пальцем - при малом вызываем левый клик
        """
        self.hand_points: HandPoints = hand_points

        if self.hand_points.is_detect:
            self.__calibration_distance = self.hand_points.get_distance(self.hand_points.wrist, self.hand_points.index_mcp)
            is_right = self.hand_points.is_hand_position_right()
            if is_right:
                # РАЗРЕШЕНИЕ НА ПЕРЕМЕЩЕНИЕ -> мизинец вверх
                move_mode = self.hand_points.pinky_tip.y < self.hand_points.pinky_pip.y
                if move_mode:
                    self.__mouse_x, self.__mouse_y = mouse.get_position()
                    # горизонтальное перемещение - положение большого пальца
                    self.__dif_x = self.hand_points.thumb_tip.x - self.hand_points.thumb_mcp.x
                    self.__dif_x = (int(self.__dif_x) // smooth) * smooth
                    self.__lim_x = int(self.__calibration_distance * self.non_sensitivity_zone_hor // smooth) * smooth
                    if abs(self.__dif_x) > self.__calibration_distance * self.non_sensitivity_zone_hor:
                        if self.__dif_x < 0:
                            self.__dif_x += self.__calibration_distance * self.non_sensitivity_zone_hor
                        else:
                            self.__dif_x -= self.__calibration_distance * self.non_sensitivity_zone_hor
                        self.__mouse_x += self.__dif_x
                    # вертикальное перемещение - положение среднего пальца
                    self.__dif_y = self.hand_points.middle_tip.y - self.hand_points.middle_pip.y
                    self.__dif_y = (int(self.__dif_y) // smooth) * smooth
                    self.__lim_y = int(self.__calibration_distance * self.non_sensitivity_zone_ver // smooth) * smooth
                    if abs(self.__dif_y) > self.__calibration_distance * self.non_sensitivity_zone_ver:
                        if self.__dif_y < 0:
                            self.__dif_y += self.__calibration_distance * self.non_sensitivity_zone_ver
                        else:
                            self.__dif_y -= self.__calibration_distance * self.non_sensitivity_zone_ver
                        self.__mouse_y += self.__dif_y
                    mouse.move(self.__mouse_x, self.__mouse_y)
                else:
                    # РЕЖИМ без движения
                    # RIGHT CLICK --> касаемся средним пальцем большого
                    self.__right_click_distance = self.hand_points.get_distance(self.hand_points.middle_tip, self.hand_points.thumb_tip)
                    self.__right_click_distance = (int(self.__right_click_distance) // smooth) * smooth
                    self.__right_click_limit = ((self.__calibration_distance * self.non_sensitivity_zone_right_click) // smooth) * smooth
                    if self.__right_click_distance < self.__right_click_limit:
                        mouse.hold(button=mouse.RIGHT)
                        self.__right_down = True
                    elif self.__right_down:
                        mouse.release(button=mouse.RIGHT)
                        self.__right_down = False

                    # MOUSE WHEEL --> положение указательного пальца
                    self.__dif_wheel = self.hand_points.index_tip.y - self.hand_points.index_pip.y
                    if abs(self.__dif_wheel) > self.__calibration_distance * self.non_sensitivity_zone_wheel:
                        if self.__dif_wheel > 0:
                            self.__dif_wheel = -self.mouse_wheel_speed
                        else:
                            self.__dif_wheel = self.mouse_wheel_speed
                        mouse.wheel(delta=self.__dif_wheel)

                # LEFT CLICK -> касаемся указательным пальцем большого вне зависимости от режима
                self.__left_click_distance = self.hand_points.get_distance(self.hand_points.index_tip, self.hand_points.thumb_tip)
                self.__left_click_distance = (int(self.__left_click_distance) // smooth) * smooth
                self.__left_click_limit = ((self.__calibration_distance * self.non_sensitivity_zone_left_click) // smooth) * smooth
                if self.__left_click_distance < self.__left_click_limit:
                    mouse.hold(button=mouse.LEFT)
                    self.__left_down = True
                elif self.__left_down:
                    mouse.release(button=mouse.LEFT)
                    self.__left_down = False

    def displaying_info(self, img: cv2.Mat, detector: HandDetector):
        """
        ### вывод отладочной информации
        1. `calibration distance` - калибровочное расстояние
        2. `horizontal` - горизонтальное перемещение сигнал - величина зоны не чувствительности
        3. `vertical` - вертикальное перемещение сигнал - величина зоны не чувствительности
        4. `mouse` - позиция курсора на экране
        5. `right click distance` - дистанция для детектировании правого клика - величина зоны не чувствительности
        6. `left click distance` - дистанция для детектировании левого клика - величина зоны не чувствительности
        7. `wheel distance` - расстояние для сработки скролла
        """
        detector.draw_line(img, self.hand_points.wrist, self.hand_points.index_mcp)
        detector.draw_line(img, self.hand_points.wrist, self.hand_points.pinky_mcp)
        detector.draw_point(img, self.hand_points.wrist)
        detector.draw_point(img, self.hand_points.index_mcp)
        detector.draw_point(img, self.hand_points.pinky_mcp)
        detector.draw_text(img, f'calibration distance{self.__calibration_distance}', position=(10, 10))

        detector.draw_line(img, self.hand_points.thumb_tip, self.hand_points.thumb_mcp)
        detector.draw_point(img, self.hand_points.thumb_tip)
        detector.draw_point(img, self.hand_points.thumb_mcp)

        detector.draw_line(img, self.hand_points.middle_tip, self.hand_points.middle_pip)
        detector.draw_point(img, self.hand_points.middle_tip)
        detector.draw_point(img, self.hand_points.middle_pip)

        detector.draw_text(img, f'horizontal {int(self.__dif_x)} - {int(self.__lim_x)}', position=(10, 20))
        detector.draw_text(img, f'vertical   {int(self.__dif_y)} - {int(self.__lim_y)}', position=(10, 30))        
        detector.draw_text( img, f'mouse: {int(self.__mouse_x)}x{int(self.__mouse_y)}', position=(10, 40))

        detector.draw_line(img, self.hand_points.thumb_tip, self.hand_points.middle_tip)
        detector.draw_point(img, self.hand_points.middle_tip)
        detector.draw_point(img, self.hand_points.thumb_tip)
        detector.draw_text( img, F'right click distance: {self.__right_click_distance} - {self.__right_click_limit}', position=(10, 50))

        detector.draw_line(img, self.hand_points.thumb_tip, self.hand_points.index_tip)
        detector.draw_point(img, self.hand_points.index_tip)
        detector.draw_point(img, self.hand_points.thumb_tip)
        detector.draw_text( img, F'left click distance: {self.__left_click_distance} - {self.__left_click_limit}', position=(10, 60))

        detector.draw_line(img, self.hand_points.index_tip, self.hand_points.index_pip)
        detector.draw_point(img, self.hand_points.index_pip)
        detector.draw_point(img, self.hand_points.index_tip)
        detector.draw_text(img, F'wheel distance: {self.__dif_wheel}', position=(10, 70))


def main():
    mhr = MouseHandRemote()
    cap = cv2.VideoCapture(0)
    detector = HandDetector(max_hands=1, detection=0.7, tracking=0.5)
    while True:
        _, img = cap.read()
        img = cv2.flip(img, 1)
        img = detector.find_hands(img)
        hand_points = detector.detect_position(img)
        mhr.remote(hand_points)
        mhr.displaying_info(img, detector)
        cv2.imshow("preview", img)
        if cv2.waitKey(1) & 0xFF == 27:
            break


if __name__ == "__main__":
    main()
