// Decompiled with JetBrains decompiler
// Type: FMODLoopSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class FMODLoopSound : BaseMonoBehaviour
{
  public EventInstance LoopedSound;
  [EventRef]
  public string AudioSourcePath = string.Empty;
  public int ParameterSetOnStart;
  public string ParameterToSetOnStart = "";
  public bool SetParameter;
  public string ParameterToSet = "";
  public float ParameterSet;
  public bool PlayOnStart = true;
  public bool TriggerOn;
  public bool TriggeredOn;
  public bool isMusic;
  public bool restartOnEnable;
  public bool LoopStarted;
  public float Distance;
  public float MaxDistance = 20f;

  public void OnEnable()
  {
    if (this.LoopStarted || this.restartOnEnable)
      this.PlayLoop();
    this.LoopStarted = false;
  }

  public void Update()
  {
    if (!this.LoopStarted || (Object) PlayerFarming.Instance == (Object) null || !this.SetParameter)
      return;
    if (!this.TriggerOn)
    {
      this.Distance = this.gameObject.transform.position.y - PlayerFarming.Instance.transform.position.y;
      this.ParameterSet = (float) ((double) this.Distance / (double) this.MaxDistance + 1.0);
      AudioManager.Instance.SetEventInstanceParameter(this.LoopedSound, this.ParameterToSet, this.ParameterSet);
    }
    else
    {
      this.Distance = Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position);
      if ((double) this.Distance >= (double) this.MaxDistance || this.TriggeredOn)
        return;
      if (this.isMusic)
        AudioManager.Instance.SetMusicRoomID(1, this.ParameterToSet);
      else
        AudioManager.Instance.SetEventInstanceParameter(this.LoopedSound, this.ParameterToSet, 1f);
      this.TriggeredOn = true;
    }
  }

  public void Start()
  {
    if (!this.PlayOnStart)
      return;
    this.restartOnEnable = true;
    this.PlayLoop();
  }

  public IEnumerator WaitForPlayer()
  {
    while ((Object) PlayerFarming.Instance == (Object) null)
      yield return (object) null;
    AudioManager.Instance.PlayMusic(this.AudioSourcePath);
    if (!this.ParameterToSetOnStart.IsNullOrEmpty())
      AudioManager.Instance.SetMusicRoomID(this.ParameterSetOnStart, this.ParameterToSetOnStart);
    this.LoopStarted = true;
  }

  public IEnumerator WaitForPlayerLoop()
  {
    FMODLoopSound fmodLoopSound = this;
    while ((Object) PlayerFarming.Instance == (Object) null && (Object) AudioManager.Instance == (Object) null)
      yield return (object) null;
    fmodLoopSound.StopLoop();
    fmodLoopSound.LoopedSound = AudioManager.Instance.CreateLoop(fmodLoopSound.AudioSourcePath, fmodLoopSound.gameObject, true);
    if (!fmodLoopSound.ParameterToSetOnStart.IsNullOrEmpty())
      AudioManager.Instance.SetEventInstanceParameter(fmodLoopSound.LoopedSound, fmodLoopSound.ParameterToSetOnStart, (float) fmodLoopSound.ParameterSetOnStart);
    fmodLoopSound.LoopStarted = true;
  }

  public void PlayLoop()
  {
    this.StopLoop();
    if (this.AudioSourcePath.IsNullOrEmpty())
      return;
    if (this.isMusic)
      this.StartCoroutine((IEnumerator) this.WaitForPlayer());
    else
      this.StartCoroutine((IEnumerator) this.WaitForPlayerLoop());
  }

  public void StopLoop()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    this.LoopStarted = false;
  }

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.LoopedSound);
    this.LoopStarted = false;
  }

  public void OnDestroy() => this.StopLoop();

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(this.transform.position, this.MaxDistance);
  }
}
