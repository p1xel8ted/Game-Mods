// Decompiled with JetBrains decompiler
// Type: SimpleSFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleSFX : BaseMonoBehaviour
{
  public float MinDistance = 5f;
  public float MaxDistance = 30f;
  public List<SimpleSFX.SFXAndTag> Sounds = new List<SimpleSFX.SFXAndTag>();
  public AudioSource _audioSource;
  public SimpleSFX.SFXAndTag PlayOnAwake;

  public AudioSource audioSource
  {
    get
    {
      if ((UnityEngine.Object) this._audioSource == (UnityEngine.Object) null)
      {
        this._audioSource = this.gameObject.AddComponent<AudioSource>();
        this._audioSource.playOnAwake = false;
        this._audioSource.spread = 360f;
        this._audioSource.spatialBlend = 1f;
        this._audioSource.minDistance = this.MinDistance;
        this._audioSource.maxDistance = this.MaxDistance;
      }
      return this._audioSource;
    }
  }

  public void Start()
  {
    if (this.PlayOnAwake == null)
      return;
    this.Play(this.PlayOnAwake.Tag);
  }

  public void AddSound(AudioClip clip, string tag)
  {
    this.Sounds.Add(new SimpleSFX.SFXAndTag(clip, tag));
  }

  public void Play(string Tag)
  {
    foreach (SimpleSFX.SFXAndTag sound in this.Sounds)
    {
      if (sound.Tag == Tag)
      {
        this.audioSource.clip = sound.audioClips[UnityEngine.Random.Range(0, sound.audioClips.Count)];
        this.audioSource.loop = sound.Looping;
        if (sound.Looping)
        {
          this.audioSource.Play();
          break;
        }
        this.audioSource.PlayOneShot(sound.audioClips[UnityEngine.Random.Range(0, sound.audioClips.Count)]);
        break;
      }
    }
  }

  public void Update() => this.audioSource.pitch = Time.timeScale;

  [Serializable]
  public class SFXAndTag
  {
    public string Tag;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public bool Looping;

    public SFXAndTag(AudioClip c, string t)
    {
      this.audioClips.Add(c);
      this.Tag = t;
    }

    public SFXAndTag(List<AudioClip> c, string t)
    {
      this.audioClips = c;
      this.Tag = t;
    }
  }
}
