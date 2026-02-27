// Decompiled with JetBrains decompiler
// Type: FMODLoopSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private bool LoopStarted;
  public float Distance;
  public float MaxDistance = 20f;

  private void OnEnable()
  {
    if (this.LoopStarted)
      this.PlayLoop();
    this.LoopStarted = false;
  }

  private void Update()
  {
    if (!this.LoopStarted)
      return;
    if ((Object) PlayerFarming.Instance == (Object) null)
    {
      AudioManager.Instance.StopLoop(this.LoopedSound);
    }
    else
    {
      if (!this.SetParameter)
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
  }

  private void Start()
  {
    if (!this.PlayOnStart)
      return;
    this.PlayLoop();
  }

  private IEnumerator WaitForPlayer()
  {
    while ((Object) PlayerFarming.Instance == (Object) null)
      yield return (object) null;
    AudioManager.Instance.PlayMusic(this.AudioSourcePath);
    if (!this.ParameterToSetOnStart.IsNullOrEmpty())
      AudioManager.Instance.SetMusicRoomID(this.ParameterSetOnStart, this.ParameterToSetOnStart);
    this.LoopStarted = true;
  }

  private IEnumerator WaitForPlayerLoop()
  {
    FMODLoopSound fmodLoopSound = this;
    while ((Object) PlayerFarming.Instance == (Object) null)
      yield return (object) null;
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

  private void OnDisable() => this.StopLoop();

  private void OnDestroy() => this.StopLoop();

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(this.transform.position, this.MaxDistance);
  }
}
