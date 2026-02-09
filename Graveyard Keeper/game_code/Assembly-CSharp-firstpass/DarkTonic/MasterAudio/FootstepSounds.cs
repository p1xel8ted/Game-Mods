// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.FootstepSounds
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

[AddComponentMenu("Dark Tonic/Master Audio/Footstep Sounds")]
public class FootstepSounds : MonoBehaviour
{
  public DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode soundSpawnMode = DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.AttachToCaller;
  public FootstepSounds.FootstepTriggerMode footstepEvent;
  public List<FootstepGroup> footstepGroups = new List<FootstepGroup>();
  public EventSounds.RetriggerLimMode retriggerLimitMode;
  public int limitPerXFrm;
  public float limitPerXSec;
  public int triggeredLastFrame = -100;
  public float triggeredLastTime = -100f;
  public Transform _trans;

  public void OnTriggerEnter(Collider other)
  {
    if (this.footstepEvent != FootstepSounds.FootstepTriggerMode.OnTriggerEnter)
      return;
    this.PlaySoundsIfMatch(other.gameObject);
  }

  public void OnCollisionEnter(Collision collision)
  {
    if (this.footstepEvent != FootstepSounds.FootstepTriggerMode.OnCollision)
      return;
    this.PlaySoundsIfMatch(collision.gameObject);
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    if (this.footstepEvent != FootstepSounds.FootstepTriggerMode.OnCollision2D)
      return;
    this.PlaySoundsIfMatch(collision.gameObject);
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (this.footstepEvent != FootstepSounds.FootstepTriggerMode.OnTriggerEnter2D)
      return;
    this.PlaySoundsIfMatch(other.gameObject);
  }

  public bool CheckForRetriggerLimit()
  {
    switch (this.retriggerLimitMode)
    {
      case EventSounds.RetriggerLimMode.FrameBased:
        if (this.triggeredLastFrame > 0 && AudioUtil.FrameCount - this.triggeredLastFrame < this.limitPerXFrm)
          return false;
        break;
      case EventSounds.RetriggerLimMode.TimeBased:
        if ((double) this.triggeredLastTime > 0.0 && (double) AudioUtil.Time - (double) this.triggeredLastTime < (double) this.limitPerXSec)
          return false;
        break;
    }
    return true;
  }

  public void PlaySoundsIfMatch(GameObject go)
  {
    if (!this.CheckForRetriggerLimit())
      return;
    switch (this.retriggerLimitMode)
    {
      case EventSounds.RetriggerLimMode.FrameBased:
        this.triggeredLastFrame = AudioUtil.FrameCount;
        break;
      case EventSounds.RetriggerLimMode.TimeBased:
        this.triggeredLastTime = AudioUtil.Time;
        break;
    }
    for (int index = 0; index < this.footstepGroups.Count; ++index)
    {
      FootstepGroup footstepGroup = this.footstepGroups[index];
      if ((!footstepGroup.useLayerFilter || footstepGroup.matchingLayers.Contains(go.layer)) && (!footstepGroup.useTagFilter || footstepGroup.matchingTags.Contains(go.tag)))
      {
        float volume = footstepGroup.volume;
        float? pitch = new float?(footstepGroup.pitch);
        if (!footstepGroup.useFixedPitch)
          pitch = new float?();
        string variationName = (string) null;
        if (footstepGroup.variationType == EventSounds.VariationType.PlaySpecific)
          variationName = footstepGroup.variationName;
        switch (this.soundSpawnMode)
        {
          case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.MasterAudioLocation:
            DarkTonic.MasterAudio.MasterAudio.PlaySound(footstepGroup.soundType, volume, pitch, footstepGroup.delaySound, variationName);
            continue;
          case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.CallerLocation:
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform(footstepGroup.soundType, this.Trans, volume, pitch, footstepGroup.delaySound, variationName);
            continue;
          case DarkTonic.MasterAudio.MasterAudio.SoundSpawnLocationMode.AttachToCaller:
            DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransform(footstepGroup.soundType, this.Trans, volume, pitch, footstepGroup.delaySound, variationName);
            continue;
          default:
            continue;
        }
      }
    }
  }

  public Transform Trans
  {
    get
    {
      if ((Object) this._trans != (Object) null)
        return this._trans;
      this._trans = this.transform;
      return this._trans;
    }
  }

  public enum FootstepTriggerMode
  {
    None,
    OnCollision,
    OnTriggerEnter,
    OnCollision2D,
    OnTriggerEnter2D,
  }
}
