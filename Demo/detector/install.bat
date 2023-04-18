start /WAIT ./python310/python.exe ./python310/get-pip.py



start /WAIT ./python310/python.exe -m pip install virtualenv


start /WAIT ./python310/python.exe -m virtualenv myenv



call ./myenv/Scripts/activate.bat

@echo off
timeout 5 > NUL
@echo on

python --version

@echo off
timeout 5 > NUL
@echo on

pip install opencv-python
pip install cvzone
pip install mediapipe


pause

