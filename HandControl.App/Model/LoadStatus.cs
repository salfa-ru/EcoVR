namespace HandControl.App.Model;

public class LoadStatus
{
    private const string LOAD_MODULES = "Получение модулей";
    private const string INSTALLING = "Получение модулей";
    private const string ACTIVATE = "Активация...";
    private const string DEACTIVATE = "Деактивация...";
    private const string START = "Запуск...";
    private string status = string.Empty;
    public delegate void OnStateChanger(LoadStatus status);
    public event OnStateChanger? StateChange;

    public string Status 
    { 
        get => status;
        set 
        {
            if (value.Contains("./python310/get-pip.py"))  status = LOAD_MODULES; 
            else if (value.Contains("install virtualenv")) status = INSTALLING;
            else if (value.Contains("virtualenv myenv")) status = INSTALLING;
            else if (value.Contains("activate.bat")) status = ACTIVATE;
            else if (value.Contains("deactivate.bat")) status = DEACTIVATE;
            else if (value.Contains("install opencv-python")) status = LOAD_MODULES;
            else if (value.Contains("install cvzone")) status = LOAD_MODULES;
            else if (value.Contains("install mediapipe")) status = LOAD_MODULES;
            else if (value.Contains("python main.py")) status = START;
            
            StateChange?.Invoke(this);
        }
    }
}

