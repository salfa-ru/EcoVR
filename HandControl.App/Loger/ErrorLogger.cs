using System;
using System.IO;

namespace HandControl.App.Loger;

public static class ErrorLoger
{
    public const string FOLDER = "log";
    
    public static void Log(string message)
    {
        CheckOrCreateFolder();
        string path = FOLDER + "\\" + GetPath();
        File.AppendAllText(path, message);
    }

    private static string GetPath()
    {
        return "log_" + DateTime.Now.ToShortDateString();
    }

    private static void CheckOrCreateFolder()
    {
        if (!Directory.Exists(FOLDER)) 
        {
            Directory.CreateDirectory(FOLDER);
        }
    }
}
