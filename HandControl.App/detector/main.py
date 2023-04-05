import sys
from LandmarksDetectorModule import LandmarksDetector


def main():
    argv1 = '6000'
    argv2 = '6001'
    if len(sys.argv) == 3:
        argv1 = sys.argv[1]
        argv2 = sys.argv[2]
    det = LandmarksDetector(argv1, argv2)
    det.start()


if __name__ == "__main__":
    main()
