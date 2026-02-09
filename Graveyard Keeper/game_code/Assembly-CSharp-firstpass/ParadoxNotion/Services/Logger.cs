// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Services.Logger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Services;

public static class Logger
{
  public static List<Logger.LogHandler> subscribers = new List<Logger.LogHandler>();

  public static void AddListener(Logger.LogHandler callback) => Logger.subscribers.Add(callback);

  public static void RemoveListener(Logger.LogHandler callback)
  {
    Logger.subscribers.Remove(callback);
  }

  public static void Log(object message, string tag = null, object context = null)
  {
    Logger.Internal_Log(LogType.Log, message, tag, context);
  }

  public static void LogWarning(object message, string tag = null, object context = null)
  {
    Logger.Internal_Log(LogType.Warning, message, tag, context);
  }

  public static void LogError(object message, string tag = null, object context = null)
  {
    Logger.Internal_Log(LogType.Error, message, tag, context);
  }

  public static void LogException(Exception exception, string tag = null, object context = null)
  {
    Logger.Internal_Log(LogType.Exception, (object) exception, tag, context);
  }

  public static void Internal_Log(LogType type, object message, string tag, object context)
  {
    if (Logger.subscribers != null && Logger.subscribers.Count > 0)
    {
      Logger.Message message1 = new Logger.Message();
      message1.type = type;
      if (message is Exception)
      {
        Exception exception = (Exception) message;
        message1.text = $"{exception.Message}\n{((IEnumerable<string>) exception.StackTrace.Split('\n')).FirstOrDefault<string>()}";
      }
      else
        message1.text = message != null ? message.ToString() : "NULL";
      message1.tag = tag;
      message1.context = context;
      bool flag = false;
      foreach (Logger.LogHandler subscriber in Logger.subscribers)
      {
        if (subscriber(message1))
        {
          flag = true;
          break;
        }
      }
      if (flag && type != LogType.Exception)
        return;
    }
    tag = $"<b>({tag} {type.ToString()})</b>";
    Logger.ForwardToUnity(type, message, tag, context);
  }

  public static void ForwardToUnity(LogType type, object message, string tag, object context)
  {
    if (message is Exception)
      Debug.unityLogger.LogException((Exception) message);
    else
      Debug.unityLogger.Log(type, tag, message, context as UnityEngine.Object);
  }

  public struct Message
  {
    public LogType type;
    public string text;
    public string tag;
    public object context;

    public bool IsSameAs(Logger.Message b)
    {
      return this.type == b.type && this.text == b.text && this.tag == b.tag && this.context == b.context;
    }

    public bool IsValid() => !string.IsNullOrEmpty(this.text);
  }

  public delegate bool LogHandler(Logger.Message message);
}
