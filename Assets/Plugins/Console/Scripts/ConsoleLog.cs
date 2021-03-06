using UnityEngine;
using System.Collections;

public class ConsoleLog
{
    /// <summary>
    /// The current instance
    /// </summary>
    private static ConsoleLog instance;

    /// <summary>
    /// Getter/setter to make sure we always have only one instance
    /// Automaticly creates new instance if one doesn't exist
    /// </summary>
    public static ConsoleLog Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConsoleLog();
                Application.RegisterLogCallback(HandleUnityDebug);
            }
            return instance;
        }
    }

    /// <summary>
    /// The current log
    /// </summary>
    public string log = "";

    /// <summary>
    /// Log a new message
    /// </summary>
    /// <param name="message">The message to log</param>
    public void Log(string message)
    {
        log += message + "\n";
    }

    public static void HandleUnityDebug(string message, string stackTrace, LogType type)
    {
        string typeString = type.ToString();
        ConsoleLog.Instance.Log(string.Format("[{0}] {1}", typeString, message));
    }
}
