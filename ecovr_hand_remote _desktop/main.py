import cv2
from HandTrackingModule import HandDetector
from MouseHandRemoteModule import MouseHandRemote


cap = cv2.VideoCapture(0)
if cap.isOpened():           
    hand_remote = MouseHandRemote() 
    detector = HandDetector(detection=0.7) 
    while True:
        _, img = cap.read()
        img = cv2.flip(img, 1) 
        img = detector.find_hands(img)  
        hand_points = detector.detect_position(img) 
        hand_remote.remote(hand_points) 
        hand_remote.displaying_info(img, detector) 
        cv2.imshow("preview", img) 
        if cv2.waitKey(1) & 0xFF == 27:  
            break
else:
    print('webcam is not available')


