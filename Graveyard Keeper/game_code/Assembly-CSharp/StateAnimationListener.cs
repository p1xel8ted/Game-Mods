// Decompiled with JetBrains decompiler
// Type: StateAnimationListener
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class StateAnimationListener : MonoBehaviour
{
  public System.Action on_entered;
  public System.Action on_exit;
  public bool destroy_after_on_exit = true;
  public bool destroyed;
  public GJTimer _workaround_timer;
  public string _error_message = "";

  public void OnEnteredState()
  {
    Debug.Log((object) ("OnEnteredState " + this.gameObject.name), (UnityEngine.Object) this);
    this.on_entered.TryInvoke();
    if (!((UnityEngine.Object) this._workaround_timer != (UnityEngine.Object) null))
      return;
    this._workaround_timer.AddTime(4f);
  }

  public void OnExitedState()
  {
    try
    {
      if ((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null)
      {
        if (this.destroyed)
        {
          Debug.LogWarning((object) ("Already destroyed, skipping. Obj = " + this.gameObject.name), (UnityEngine.Object) this);
          return;
        }
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) "OnExitedState gameObject is null");
      if ((UnityEngine.Object) this._workaround_timer != (UnityEngine.Object) null)
      {
        this._workaround_timer.Stop();
        this._workaround_timer = (GJTimer) null;
      }
      this.destroyed = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
      return;
    }
    if (this.destroyed)
    {
      Debug.LogWarning((object) ("Already destroyed, skipping. Obj = " + this.gameObject.name), (UnityEngine.Object) this);
    }
    else
    {
      Debug.Log((object) ("OnExitedState " + this.gameObject.name), (UnityEngine.Object) this);
      if ((UnityEngine.Object) this._workaround_timer != (UnityEngine.Object) null)
      {
        this._workaround_timer.Stop();
        this._workaround_timer = (GJTimer) null;
      }
      this.on_exit.TryInvoke();
      if (!this.destroy_after_on_exit)
        return;
      this.destroyed = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    }
  }

  public void AddWorkaroundTimer(float time, string error_message = "")
  {
    if (time.EqualsTo(0.0f))
      return;
    this._error_message = error_message;
    this._workaround_timer = GJTimer.AddTimer(time, (GJTimer.VoidDelegate) (() =>
    {
      Debug.Log((object) this._error_message, (UnityEngine.Object) this);
      this.OnExitedState();
    }));
  }

  [CompilerGenerated]
  public void \u003CAddWorkaroundTimer\u003Eb__8_0()
  {
    Debug.Log((object) this._error_message, (UnityEngine.Object) this);
    this.OnExitedState();
  }
}
