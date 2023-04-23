namespace HandControl.App.Model;

public class LoadStatus
{
    private string status = string.Empty;

    public delegate void OnStateChanger(LoadStatus status);
    public event OnStateChanger? StateChange;

    object o = new object();

    public string Status 
    { 
        get => status;
        set 
        {   
            if (value.Length > 50)
            {
                status = value.Substring(0,20) + "..." + value.Substring(value.Length - 27, 27); 
            }
            else status = value;
            
            StateChange?.Invoke(this);
        }
    }
}

