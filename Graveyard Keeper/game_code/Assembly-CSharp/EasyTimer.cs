// Decompiled with JetBrains decompiler
// Type: EasyTimer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EasyTimer : MonoBehaviour
{
  public float _end_time;
  public EasyTimer.VoidDelegate _delegate;
  public bool _active = true;
  public bool _depends_on_scale;

  public static EasyTimer Add(float seconds, EasyTimer.VoidDelegate dlgt, bool depends_on_scale = true)
  {
    if (dlgt == null)
    {
      Debug.LogError((object) "Trying to add timer with void delegate!");
      return (EasyTimer) null;
    }
    EasyTimer easyTimer = new GameObject($"Easy timer {seconds.ToString()} s.").AddComponent<EasyTimer>();
    easyTimer.Init(seconds, dlgt, depends_on_scale);
    return easyTimer;
  }

  public void Init(float time, EasyTimer.VoidDelegate dlgt, bool depends_on_scale)
  {
    this._end_time = (depends_on_scale ? Time.time : Time.realtimeSinceStartup) + time;
    this._delegate = dlgt;
    this._active = true;
    this._depends_on_scale = depends_on_scale;
  }

  public void Stop()
  {
    if (!this._active)
      return;
    this.enabled = this._active = false;
    this.gameObject.Destroy();
  }

  public void Update()
  {
    if (!this._active || (!this._depends_on_scale || (double) Time.time < (double) this._end_time) && (this._depends_on_scale || (double) Time.realtimeSinceStartup < (double) this._end_time))
      return;
    this.OnComplete();
  }

  public void OnComplete()
  {
    if (!this._active)
      return;
    if (this._delegate != null)
      this._delegate();
    this.Stop();
  }

  public static void StopAllTimers()
  {
    foreach (EasyTimer easyTimer in Object.FindObjectsOfType<EasyTimer>())
      easyTimer.Stop();
  }

  public static void ForceAllTimersComplete()
  {
    foreach (EasyTimer easyTimer in Object.FindObjectsOfType<EasyTimer>())
      easyTimer.OnComplete();
  }

  public delegate void VoidDelegate();
}
