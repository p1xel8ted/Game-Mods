// Decompiled with JetBrains decompiler
// Type: VFXTimeline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Playables;

#nullable disable
[RequireComponent(typeof (PlayableDirector))]
public class VFXTimeline : VFXObject, INotificationReceiver
{
  [SerializeField]
  public PlayableDirector _playableDirector;

  public override void Init()
  {
    if (!this.Initialized)
    {
      if (!(bool) (UnityEngine.Object) this._playableDirector)
        this._playableDirector = this.GetComponent<PlayableDirector>();
      this._playableDirector.playOnAwake = false;
    }
    base.Init();
  }

  public override void PlayVFX(float addEmissionDelay = 0.0f, PlayerFarming playerFarming = null, bool playSFX = true)
  {
    if (this.Playing)
      this._playableDirector.Stop();
    this._playableDirector.stopped -= new Action<PlayableDirector>(this.OnPlayableDirectorStopped);
    this._playableDirector.stopped += new Action<PlayableDirector>(this.OnPlayableDirectorStopped);
    base.PlayVFX(addEmissionDelay, playSFX: playSFX);
  }

  public override void Emit()
  {
    this._playableDirector.Play();
    base.Emit();
  }

  public void OnPlayableDirectorStopped(PlayableDirector director)
  {
    this._playableDirector.stopped -= new Action<PlayableDirector>(this.OnPlayableDirectorStopped);
    this.CancelVFX();
  }

  public override void StopVFX()
  {
    this._playableDirector.Stop();
    this.TriggerStopEvent();
  }

  public void OnNotify(Playable origin, INotification notification, object context)
  {
  }

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) this._playableDirector != (UnityEngine.Object) null))
      return;
    this._playableDirector.stopped -= new Action<PlayableDirector>(this.OnPlayableDirectorStopped);
  }
}
