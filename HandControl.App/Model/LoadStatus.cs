namespace HandControl.App.Model;

public class LoadStatus
{
    private string status = string.Empty;
    public delegate void OnStateChanger(LoadStatus status);
    public event OnStateChanger? StateChange;

    public string Status 
    { 
        get => status;
        set 
        {
            if (value.Contains("./python310/get-pip.py"))  status = "Получение установщика"; 
            else if (value.Contains("install virtualenv")) status = "Установка модуля виртуального пространства";
            else if (value.Contains("virtualenv myenv")) status = "Создание виртуального пространства";
            else if (value.Contains("activate.bat")) status = "Активация виртуального пространства";
            else if (value.Contains("deactivate.bat")) status = "Деактивация виртуального пространства";
            else if (value.Contains("python --version")) status = "Получение версии";
            else if (value.Contains("install opencv-python")) status = "Получение модуля opencv";
            else if (value.Contains("install cvzone")) status = "Получение модуля cvzone";
            else if (value.Contains("install mediapipe")) status = "Получение модуля mediapipe";
            else if (value.Contains("python main.py")) status = "Запуск";
            
            StateChange?.Invoke(this);
        }
    }
}

