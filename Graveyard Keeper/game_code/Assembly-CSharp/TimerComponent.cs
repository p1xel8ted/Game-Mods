// Decompiled with JetBrains decompiler
// Type: TimerComponent
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TimerComponent : WorldGameObjectComponent
{
  [RuntimeValue(true)]
  [SerializeField]
  public bool _started;
  [RuntimeValue(true)]
  [SerializeField]
  public float _target_time;
  [RuntimeValue(true)]
  [SerializeField]
  public float _played_time;
  [RuntimeValue(true)]
  [SerializeField]
  public float _normalized_time;
  public List<float> _delays;
  public List<GJCommons.VoidDelegate> _delegates;

  public event GJCommons.VoidDelegate on_loop;

  public event TimerComponent.OnUpdate on_update;

  public void Play(float time)
  {
    if (this._started)
      Debug.LogError((object) "Timer is allready started", (Object) this.wgo);
    else if (time.EqualsTo(0.0f))
    {
      Debug.LogError((object) "Cannot start timer with 0 time", (Object) this.wgo);
    }
    else
    {
      this._started = true;
      this._target_time = time;
      this._played_time = this._normalized_time = 0.0f;
    }
  }

  public void ClearTimer()
  {
    this._started = false;
    this._target_time = this._played_time = this._normalized_time = 0.0f;
  }

  public void SetCallbacks(TimerComponent.OnUpdate update, GJCommons.VoidDelegate loop)
  {
    if (!this._started)
      Debug.LogWarning((object) "Registering callbacks for not started timer", (Object) this.wgo);
    this.on_update += update;
    this.on_loop += loop;
  }

  public void RemoveCallbacks(TimerComponent.OnUpdate update, GJCommons.VoidDelegate loop)
  {
    this.on_update -= update;
    this.on_loop -= loop;
  }

  public override bool HasUpdate() => true;

  public override void UpdateComponent(float delta_time)
  {
    if (this._delays != null)
    {
      for (int index = 0; index < this._delays.Count; ++index)
      {
        this._delays[index] -= delta_time;
        if ((double) this._delays[index] <= 0.0)
        {
          this._delegates[index].TryInvoke();
          this._delays.RemoveAt(index);
          this._delegates.RemoveAt(index);
          --index;
        }
      }
    }
    if (!this._started)
      return;
    this._played_time += delta_time;
    this._normalized_time = this._played_time / this._target_time;
    if ((double) this._normalized_time < 1.0)
    {
      if (this.on_update == null)
        return;
      this.on_update(this._normalized_time);
    }
    else
    {
      this.on_loop.TryInvoke();
      if (this.on_update != null)
        this.on_update(1f);
      this._started = false;
      this._target_time = this._played_time = this._normalized_time = 0.0f;
    }
  }

  public override int GetExecutionOrder() => -1;

  public delegate void OnUpdate(float normalized_time);
}
