// Decompiled with JetBrains decompiler
// Type: VFXParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
[RequireComponent(typeof (ParticleSystem))]
public class VFXParticle : VFXObject
{
  [SerializeField]
  public ParticleSystem _particleSystem;
  [SerializeField]
  public List<VFXParticle.ParticleEvent> _events;
  public string loopedSoundSFX;
  public EventInstance loopedSound;
  public bool createdSFX;

  public override void Init()
  {
    if (!this.Initialized)
    {
      if (!(bool) (UnityEngine.Object) this._particleSystem)
        this._particleSystem = this.GetComponent<ParticleSystem>();
      ParticleSystem.MainModule main = this._particleSystem.main with
      {
        playOnAwake = false,
        stopAction = ParticleSystemStopAction.Callback
      };
    }
    if (!this.loopedSoundSFX.IsNullOrWhitespace())
    {
      this.loopedSound = AudioManager.Instance.CreateLoop(this.loopedSoundSFX, this.gameObject, true);
      this.createdSFX = true;
    }
    base.Init();
  }

  public override void PlayVFX(float addEmissionDelay = 0.0f, PlayerFarming playerFarming = null, bool playSFX = true)
  {
    if (this._particleSystem.isPlaying)
      this._particleSystem.Stop();
    foreach (VFXParticle.ParticleEvent particleEvent in this._events)
      particleEvent.Init();
    if (!this.loopedSoundSFX.IsNullOrWhitespace() & playSFX)
    {
      if (!this.createdSFX)
        this.loopedSound = AudioManager.Instance.CreateLoop(this.loopedSoundSFX, this.gameObject, true);
      else
        AudioManager.Instance.PlayLoop(this.loopedSound);
    }
    base.PlayVFX(addEmissionDelay, playSFX: playSFX);
  }

  public override void Emit()
  {
    this._particleSystem.Play();
    base.Emit();
  }

  public override void StopVFX()
  {
    this._particleSystem.Stop();
    this.TriggerStopEvent();
    if (this.loopedSoundSFX.IsNullOrWhitespace())
      return;
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public override void CancelVFX()
  {
    this._particleSystem.Stop();
    base.CancelVFX();
    if (this.loopedSoundSFX.IsNullOrWhitespace())
      return;
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (this.loopedSoundSFX.IsNullOrWhitespace())
      return;
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public void OnDestroy()
  {
    if (this.loopedSoundSFX.IsNullOrWhitespace())
      return;
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public void LateUpdate()
  {
    if (!(bool) (UnityEngine.Object) this._particleSystem || !this._particleSystem.isPlaying)
      return;
    foreach (VFXParticle.ParticleEvent particleEvent in this._events)
    {
      float num = this._particleSystem.time % this._particleSystem.main.duration;
      if (!particleEvent.TriggersOnce && (double) num >= (double) particleEvent.TriggeredAtTime)
        particleEvent.TriggeredAtTime = -1f;
      if ((double) particleEvent.TriggeredAtTime < 0.0 && (double) particleEvent.Time > (double) num)
      {
        particleEvent.Event.Invoke();
        particleEvent.TriggeredAtTime = num;
      }
    }
  }

  public void OnParticleSystemStopped()
  {
    base.CancelVFX();
    if (this.loopedSoundSFX.IsNullOrWhitespace())
      return;
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  [Serializable]
  public class ParticleEvent
  {
    public UnityEvent Event;
    public float Time;
    public bool TriggersOnce;
    [NonSerialized]
    public float TriggeredAtTime = -1f;

    public void Init() => this.TriggeredAtTime = -1f;
  }
}
