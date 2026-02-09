// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Services.Threader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Threading;

#nullable disable
namespace ParadoxNotion.Services;

public static class Threader
{
  public static Thread StartAction(Action function, Action callback = null)
  {
    Thread thread = new Thread(new ThreadStart(function.Invoke));
    Threader.Begin(thread, callback);
    return thread;
  }

  public static Thread StartAction<T1>(Action<T1> function, T1 parameter1, Action callback = null)
  {
    Thread thread = new Thread((ThreadStart) (() => function(parameter1)));
    Threader.Begin(thread, callback);
    return thread;
  }

  public static Thread StartAction<T1, T2>(
    Action<T1, T2> function,
    T1 parameter1,
    T2 parameter2,
    Action callback = null)
  {
    Thread thread = new Thread((ThreadStart) (() => function(parameter1, parameter2)));
    Threader.Begin(thread, callback);
    return thread;
  }

  public static Thread StartAction<T1, T2, T3>(
    Action<T1, T2, T3> function,
    T1 parameter1,
    T2 parameter2,
    T3 parameter3,
    Action callback = null)
  {
    Thread thread = new Thread((ThreadStart) (() => function(parameter1, parameter2, parameter3)));
    Threader.Begin(thread, callback);
    return thread;
  }

  public static Thread StartFunction<TResult>(Func<TResult> function, Action<TResult> callback = null)
  {
    TResult result = default (TResult);
    Thread thread = new Thread((ThreadStart) (() => result = function()));
    Threader.Begin(thread, (Action) (() => callback(result)));
    return thread;
  }

  public static Thread StartFunction<TResult, T1>(
    Func<T1, TResult> function,
    T1 parameter1,
    Action<TResult> callback = null)
  {
    TResult result = default (TResult);
    Thread thread = new Thread((ThreadStart) (() => result = function(parameter1)));
    Threader.Begin(thread, (Action) (() => callback(result)));
    return thread;
  }

  public static Thread StartFunction<TResult, T1, T2>(
    Func<T1, T2, TResult> function,
    T1 parameter1,
    T2 parameter2,
    Action<TResult> callback = null)
  {
    TResult result = default (TResult);
    Thread thread = new Thread((ThreadStart) (() => result = function(parameter1, parameter2)));
    Threader.Begin(thread, (Action) (() => callback(result)));
    return thread;
  }

  public static Thread StartFunction<TResult, T1, T2, T3>(
    Func<T1, T2, T3, TResult> function,
    T1 parameter1,
    T2 parameter2,
    T3 parameter3,
    Action<TResult> callback = null)
  {
    TResult result = default (TResult);
    Thread thread = new Thread((ThreadStart) (() => result = function(parameter1, parameter2, parameter3)));
    Threader.Begin(thread, (Action) (() => callback(result)));
    return thread;
  }

  public static void Begin(Thread thread, Action callback)
  {
    thread.Start();
    MonoManager.current.StartCoroutine(Threader.ThreadUpdater(thread, callback));
  }

  public static IEnumerator ThreadUpdater(Thread thread, Action callback)
  {
    while (thread.IsAlive)
      yield return (object) null;
    yield return (object) null;
    if ((thread.ThreadState & ThreadState.AbortRequested) != ThreadState.AbortRequested && callback != null)
      callback();
  }
}
