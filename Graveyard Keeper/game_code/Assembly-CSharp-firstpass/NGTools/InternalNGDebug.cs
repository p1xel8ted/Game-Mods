// Decompiled with JetBrains decompiler
// Type: NGTools.InternalNGDebug
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using UnityEngine;

#nullable disable
namespace NGTools;

[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public class InternalNGDebug
{
  public const char MultiContextsStartChar = '\u0001';
  public const char MultiContextsEndChar = '\u0004';
  public const char MultiContextsSeparator = ';';
  public const char DataStartChar = '\u0002';
  public const char DataEndChar = '\u0004';
  public const char DataSeparator = '\n';
  public const char DataSeparatorReplace = '\u0005';
  public const char MultiTagsStartChar = '\u0003';
  public const char MultiTagsEndChar = '\u0004';
  public const char MultiTagsSeparator = ';';
  public static EventWaitHandle waitHandle;
  public static string LogPath = "NGTLogs.txt";
  public static int lastLogHash;
  public static int lastLogCounter;

  static InternalNGDebug()
  {
    InternalNGDebug.waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "578943af-6fd1-4792-b36c-1713c20a37d9");
  }

  public static void Log(object message)
  {
    Debug.Log((object) ("[NG Tools Pro] " + message?.ToString()));
  }

  public static void Log(object message, UnityEngine.Object context)
  {
    Debug.Log((object) ("[NG Tools Pro] " + message?.ToString()), context);
  }

  public static void Log(int error, string message)
  {
    Debug.Log((object) $"[NG Tools Pro] #{error.ToString()} - {message}");
  }

  public static void Log(int error, string message, UnityEngine.Object context)
  {
    Debug.Log((object) $"[NG Tools Pro] #{error.ToString()} - {message}", context);
  }

  public static void LogWarning(string message)
  {
    Debug.LogWarning((object) ("[NG Tools Pro] " + message));
  }

  public static void LogWarning(string message, UnityEngine.Object context)
  {
    Debug.LogWarning((object) ("[NG Tools Pro] " + message), context);
  }

  public static void LogWarning(int error, string message)
  {
    Debug.LogWarning((object) $"[NG Tools Pro] #{error.ToString()} - {message}");
  }

  public static void LogWarning(int error, string message, UnityEngine.Object context)
  {
    Debug.LogWarning((object) $"[NG Tools Pro] #{error.ToString()} - {message}", context);
  }

  public static void LogError(object message)
  {
    Debug.LogError((object) ("[NG Tools Pro] " + message?.ToString()));
  }

  public static void LogError(object message, UnityEngine.Object context)
  {
    Debug.LogError((object) ("[NG Tools Pro] " + message?.ToString()), context);
  }

  public static void LogError(int error, string message)
  {
    Debug.LogError((object) $"[NG Tools Pro] #{error.ToString()} - {message}");
  }

  public static void LogError(int error, string message, UnityEngine.Object context)
  {
    Debug.LogError((object) $"[NG Tools Pro] #{error.ToString()} - {message}", context);
  }

  public static void LogException(string message, Exception exception)
  {
    Debug.LogException((Exception) new InternalNGDebug.NGTools($"{exception.GetType().Name}: {message}{Environment.NewLine}{exception.Message}{Environment.NewLine}{exception.StackTrace}"));
  }

  public static void LogException(Exception exception)
  {
    Debug.LogException((Exception) new InternalNGDebug.NGTools(exception.Message + Environment.NewLine + exception.StackTrace));
  }

  public static void LogException(Exception exception, UnityEngine.Object context)
  {
    Debug.LogException((Exception) new InternalNGDebug.NGTools(exception.Message + Environment.NewLine + exception.StackTrace), context);
  }

  public static void LogException(string message, Exception exception, UnityEngine.Object context)
  {
    Debug.LogException((Exception) new InternalNGDebug.NGTools(message + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace), context);
  }

  public static void LogException(int error, Exception exception)
  {
    Debug.LogException((Exception) new InternalNGDebug.NGTools($"[E{error.ToString()}] {exception.GetType().Name}: {exception.Message}{Environment.NewLine}{exception.StackTrace}"));
  }

  public static void LogException(int error, Exception exception, UnityEngine.Object context)
  {
    Debug.LogException((Exception) new InternalNGDebug.NGTools($"[E{error.ToString()}] {exception.GetType().Name}: {exception.Message}{Environment.NewLine}{exception.StackTrace}"), context);
  }

  public static void LogException(int error, string message, Exception exception)
  {
    Debug.LogException((Exception) new InternalNGDebug.NGTools($"[E{error.ToString()}] {message}{Environment.NewLine}{exception.GetType().Name}: {exception.Message}{Environment.NewLine}{exception.StackTrace}"));
  }

  public static void LogException(int error, string message, Exception exception, UnityEngine.Object context)
  {
    Debug.LogException((Exception) new InternalNGDebug.NGTools($"[E{error.ToString()}] {message}{Environment.NewLine}{exception.GetType().Name}: {exception.Message}{Environment.NewLine}{exception.StackTrace}"), context);
  }

  public static void InternalLog(object message)
  {
    if (Conf.DebugMode == Conf.DebugModes.None)
      return;
    Debug.Log((object) ("[NG Tools Pro] " + message?.ToString()));
  }

  public static void InternalLogWarning(object message)
  {
    if (Conf.DebugMode == Conf.DebugModes.None)
      return;
    Debug.LogWarning((object) ("[NG Tools Pro] " + message?.ToString()));
  }

  public static void Assert(bool assertion, object message, UnityEngine.Object context)
  {
    if (Conf.DebugMode == Conf.DebugModes.None || assertion)
      return;
    Debug.LogError((object) ("[NG Tools Pro] " + message?.ToString()), context);
  }

  public static void Assert(bool assertion, object message)
  {
    if (Conf.DebugMode == Conf.DebugModes.None || assertion)
      return;
    Debug.LogError((object) ("[NG Tools Pro] " + message?.ToString()));
  }

  public static void LogFile(object log)
  {
    if (Conf.DebugMode == Conf.DebugModes.None)
      return;
    InternalNGDebug.waitHandle.WaitOne();
    if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
    {
      InternalNGDebug.VerboseLog(log);
      int hashCode = log.GetHashCode();
      if (hashCode != InternalNGDebug.lastLogHash)
      {
        InternalNGDebug.lastLogHash = hashCode;
        InternalNGDebug.lastLogCounter = 0;
        File.AppendAllText(InternalNGDebug.LogPath, log?.ToString() + Environment.NewLine);
      }
      else
      {
        ++InternalNGDebug.lastLogCounter;
        if (InternalNGDebug.lastLogCounter <= 2)
          File.AppendAllText(InternalNGDebug.LogPath, log?.ToString() + Environment.NewLine);
        else if (InternalNGDebug.lastLogCounter == 3)
          File.AppendAllText(InternalNGDebug.LogPath, "…" + Environment.NewLine);
      }
    }
    else
      Debug.Log((object) ("[NG Tools Pro] " + log?.ToString()));
    InternalNGDebug.waitHandle.Set();
  }

  public static void LogFileException(string message, Exception exception)
  {
    if (Conf.DebugMode == Conf.DebugModes.None)
      return;
    InternalNGDebug.waitHandle.WaitOne();
    if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
    {
      InternalNGDebug.VerboseLog((object) message);
      InternalNGDebug.VerboseLogException(exception);
      int num = message.GetHashCode() + exception.GetHashCode();
      if (num != InternalNGDebug.lastLogHash)
      {
        InternalNGDebug.lastLogHash = num;
        InternalNGDebug.lastLogCounter = 0;
        File.AppendAllText(InternalNGDebug.LogPath, message + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine);
      }
      else
      {
        ++InternalNGDebug.lastLogCounter;
        if (InternalNGDebug.lastLogCounter <= 2)
          File.AppendAllText(InternalNGDebug.LogPath, message + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine);
        else if (InternalNGDebug.lastLogCounter == 3)
          File.AppendAllText(InternalNGDebug.LogPath, "…" + Environment.NewLine);
      }
    }
    else
    {
      Debug.LogError((object) ("[NG Tools Pro] " + message));
      Debug.LogError((object) $"[NG Tools Pro] {exception.Message}{Environment.NewLine}{exception.StackTrace}");
    }
    InternalNGDebug.waitHandle.Set();
  }

  public static void LogFileException(Exception exception)
  {
    if (Conf.DebugMode == Conf.DebugModes.None)
      return;
    InternalNGDebug.waitHandle.WaitOne();
    if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
    {
      InternalNGDebug.VerboseLogException(exception);
      int hashCode = exception.GetHashCode();
      if (hashCode != InternalNGDebug.lastLogHash)
      {
        InternalNGDebug.lastLogHash = hashCode;
        InternalNGDebug.lastLogCounter = 0;
        File.AppendAllText(InternalNGDebug.LogPath, exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine);
      }
      else
      {
        ++InternalNGDebug.lastLogCounter;
        if (InternalNGDebug.lastLogCounter <= 2)
          File.AppendAllText(InternalNGDebug.LogPath, exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine);
        else if (InternalNGDebug.lastLogCounter == 3)
          File.AppendAllText(InternalNGDebug.LogPath, "…" + Environment.NewLine);
      }
    }
    else
      Debug.LogError((object) $"[NG Tools Pro] {exception.Message}{Environment.NewLine}{exception.StackTrace}");
    InternalNGDebug.waitHandle.Set();
  }

  public static void AssertFile(bool assertion, object message)
  {
    if (Conf.DebugMode == Conf.DebugModes.None || assertion)
      return;
    if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
    {
      InternalNGDebug.VerboseAssert((object) (message ?? (object) "NULL").ToString());
      InternalNGDebug.LogFile((object) (message ?? (object) "NULL").ToString());
    }
    else
      Debug.LogError((object) ("[NG Tools Pro] " + (message ?? (object) "NULL").ToString()));
  }

  public static void VerboseAssert(object message)
  {
    if (Conf.DebugMode != Conf.DebugModes.Verbose)
      return;
    InternalNGDebug.LogError(message);
  }

  public static void VerboseLog(object message)
  {
    if (Conf.DebugMode != Conf.DebugModes.Verbose)
      return;
    InternalNGDebug.Log(message);
  }

  public static void VerboseLogException(Exception exception)
  {
    if (Conf.DebugMode != Conf.DebugModes.Verbose)
      return;
    InternalNGDebug.LogException(exception);
  }

  public class NGTools(string message) : Exception(message)
  {
  }
}
