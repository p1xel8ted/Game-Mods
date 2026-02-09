// Decompiled with JetBrains decompiler
// Type: GJTimer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
public class GJTimer : MonoBehaviour
{
  public float _end_time;
  public GJTimer.VoidDelegate _delegate;
  public GJTimer.VoidDelegate _on_update;
  public GJTimer.BoolDelegate _condition;
  public bool _active = true;
  public int _id;
  public bool _just_added = true;

  public static GJTimer AddTimer(float seconds, GJTimer.VoidDelegate dlgt)
  {
    if (dlgt == null)
    {
      Debug.LogError((object) "Trying to add timer with void delegate!");
      return (GJTimer) null;
    }
    GJTimer gjTimer = new GameObject($"GJTimer {seconds.ToString()} s.").AddComponent<GJTimer>();
    gjTimer.Init(seconds, dlgt);
    return gjTimer;
  }

  public static GJTimer AddConditionalChecker(
    GJTimer.BoolDelegate condition,
    GJTimer.VoidDelegate on_update,
    GJTimer.VoidDelegate on_done)
  {
    GJTimer gjTimer = new GameObject("GJTimer condition").AddComponent<GJTimer>();
    gjTimer._condition = condition;
    gjTimer._delegate = on_done;
    gjTimer._on_update = on_update;
    return gjTimer;
  }

  public void Init(float time, GJTimer.VoidDelegate dlgt)
  {
    this._end_time = Time.time + time;
    this._delegate = dlgt;
    this._active = true;
    this._just_added = true;
    this._id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
  }

  public void AddTime(float time) => this._end_time += time;

  public void Stop()
  {
    if (!this._active)
      return;
    this.enabled = false;
    NGUITools.Destroy((UnityEngine.Object) this.gameObject);
    this._active = false;
  }

  public void Update()
  {
    if (this._condition != null)
    {
      if (this._on_update != null)
        this._on_update();
      if (!this._condition())
        return;
      this.OnComplete();
    }
    else if (this._just_added)
    {
      this._just_added = false;
    }
    else
    {
      if (!this._active || (double) Time.time < (double) this._end_time)
        return;
      this.OnComplete();
    }
  }

  public void OnComplete()
  {
    if (!this._active)
      return;
    try
    {
      if (this._delegate != null)
        this._delegate();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("FATAL ERROR: Exception while OnComplete GJTimer: " + ex?.ToString()));
    }
    this.Stop();
  }

  public static void StopAllTimers()
  {
    foreach (GJTimer gjTimer in UnityEngine.Object.FindObjectsOfType<GJTimer>())
      gjTimer.Stop();
  }

  public static void ForceAllTimersComplete()
  {
    foreach (GJTimer gjTimer in UnityEngine.Object.FindObjectsOfType<GJTimer>())
      gjTimer.OnComplete();
  }

  public delegate void VoidDelegate();

  public delegate bool BoolDelegate();
}
