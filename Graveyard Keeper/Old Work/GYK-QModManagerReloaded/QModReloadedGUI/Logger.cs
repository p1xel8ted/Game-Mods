using System;
using System.IO;

namespace QModReloadedGUI;

public static class Logger
{
   private const string Log = "qmod_reloaded_log.txt";

    public static void WriteLog(string msg, bool error = false)
    {
        var dt = DateTime.Now;

        string logMessage;
        if (error)
        {
            logMessage = "-----------------------------------------\n";
            logMessage += dt.ToShortDateString() + " " + dt.ToLongTimeString() + " : [ERROR] : " + msg + "\n";
            logMessage += "-----------------------------------------";
        }
        else
        {
            logMessage = dt.ToShortDateString() + " " + dt.ToLongTimeString() + " : " + msg;
        }


        using var streamWriter = new StreamWriter(Log, append: true);
        streamWriter.WriteLine(logMessage);
    }
}
