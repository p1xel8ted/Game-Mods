// Decompiled with JetBrains decompiler
// Type: AmbientMusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

#nullable disable
public class AmbientMusicController : BaseMonoBehaviour
{
  public AudioMixerGroup audioMixerGroup;
  public List<AmbientMusicController.Track> Tracks = new List<AmbientMusicController.Track>();
  public AmbientMusicController.Track CustomCombatTrack;
  public static bool AmbientIsPlaying;
  public static AmbientMusicController _Instance;

  public AmbientMusicController.Track AmbientTrack => this.Tracks[0];

  public AmbientMusicController.Track DefaultCombatTrack => this.Tracks[1];

  public AmbientMusicController.Track NatureTrack => this.Tracks[2];

  public static AmbientMusicController Instance
  {
    get
    {
      if ((UnityEngine.Object) AmbientMusicController._Instance == (UnityEngine.Object) null)
      {
        GameObject target = UnityEngine.Object.Instantiate(Resources.Load("MMAudio/Ambient Music Controller")) as GameObject;
        AmbientMusicController._Instance = target.GetComponent<AmbientMusicController>();
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
      }
      return AmbientMusicController._Instance;
    }
  }

  public static void PlayAmbient(float FadeIn)
  {
    AmbientMusicController.Instance.AmbientTrack.Play(true, FadeIn);
    AmbientMusicController.AmbientIsPlaying = true;
  }

  public static void PlayNature(float FadeIn)
  {
    AmbientMusicController.Instance.NatureTrack.Play(false, FadeIn);
  }

  public static void PlayAmbientLayerByTag(string tag)
  {
    Debug.Log((object) "AddAmbientLayerByTrack");
    foreach (AmbientMusicController.Track layer in AmbientMusicController.Instance.AmbientTrack.Layers)
    {
      if (layer.Tag == tag)
        layer.Play(false, 0.0f);
    }
  }

  public static void PlayAmbientCombat()
  {
    AmbientMusicController.Instance.AmbientTrack.PlayLayer(0);
  }

  public static void StopAmbientCombat()
  {
    AmbientMusicController.Instance.AmbientTrack.StopLayer(0, 5f);
  }

  public static void PlayAmbientSnake()
  {
    AmbientMusicController.Instance.AmbientTrack.PlayLayer(1);
  }

  public static void StopAmbientSnake()
  {
    AmbientMusicController.Instance.AmbientTrack.StopLayer(1, 2f);
  }

  public static void StopAll()
  {
    AmbientMusicController.Instance.StopAllCoroutines();
    foreach (AmbientMusicController.Track track in AmbientMusicController.Instance.Tracks)
      track.Stop();
  }

  public static void PlayTrack(AudioClip clip, float FadeIn, bool Loop = true)
  {
    AmbientMusicController.Track.CreateAndPlay((AudioClip) null, clip, FadeIn, Loop);
    AmbientMusicController.AmbientIsPlaying = false;
  }

  public static void StopTrackAndResturnToAmbient() => AmbientMusicController.PlayAmbient(3f);

  public static void PlayCombat()
  {
    AmbientMusicController.Instance.DefaultCombatTrack.Play(true, 0.0f);
    AmbientMusicController.AmbientIsPlaying = false;
  }

  public static void PlayCombat(AudioClip PreRoll, AudioClip CombatMusic)
  {
    AmbientMusicController.Instance.CustomCombatTrack = AmbientMusicController.Track.CreateAndPlay(PreRoll, CombatMusic, 0.0f, true);
    AmbientMusicController.AmbientIsPlaying = false;
  }

  public static void StopCombat() => AmbientMusicController.PlayAmbient(4f);

  public static void FadeAll(AmbientMusicController.Track Exception)
  {
    AmbientMusicController.Instance.StopAllCoroutines();
    foreach (AmbientMusicController.Track track in AmbientMusicController.Instance.Tracks)
    {
      if (track != Exception && !track.DontFade)
        track.FadeOut(1f, true);
    }
  }

  [Serializable]
  public class Track
  {
    public string Tag = "";
    public bool Permenant;
    public bool DontFade;
    public float Volume = 1f;
    public AudioSource _audioSource;
    public AudioClip preAudioClip;
    public AudioClip audioClip;
    public List<AmbientMusicController.Track> Layers = new List<AmbientMusicController.Track>();
    public Coroutine cFadeIn;
    public Coroutine cDoPreRoll;
    public Coroutine cFadeOut;

    [HideInInspector]
    public bool isPlaying
    {
      get => (UnityEngine.Object) this.audioSource != (UnityEngine.Object) null && this.audioSource.isPlaying;
    }

    public AudioSource audioSource
    {
      get
      {
        if ((UnityEngine.Object) this._audioSource == (UnityEngine.Object) null)
        {
          this._audioSource = this.gameObject.gameObject.AddComponent<AudioSource>();
          this._audioSource.dopplerLevel = 0.0f;
          this._audioSource.outputAudioMixerGroup = AmbientMusicController._Instance.audioMixerGroup;
          this._audioSource.playOnAwake = false;
          this._audioSource.loop = true;
        }
        return this._audioSource;
      }
      set => this._audioSource = value;
    }

    public AmbientMusicController gameObject => AmbientMusicController.Instance;

    public Track(AudioClip preAudioClip, AudioClip audioClip)
    {
      this.audioClip = audioClip;
      this.preAudioClip = preAudioClip;
    }

    public void MatchTimeToTrack(AmbientMusicController.Track OtherTrack)
    {
      this.audioSource.time = OtherTrack.audioSource.time % this.audioSource.time;
    }

    public void SetVolume(float Volume) => this.audioSource.volume = Volume;

    public static AmbientMusicController.Track CreateAndPlay(
      AudioClip preAudioClip,
      AudioClip audioClip,
      float FadeIn,
      bool Loop)
    {
      AmbientMusicController.Track andPlay = new AmbientMusicController.Track(preAudioClip, audioClip);
      andPlay.Play(true, FadeIn, Loop);
      andPlay.Permenant = false;
      andPlay.gameObject.Tracks.Add(andPlay);
      return andPlay;
    }

    public static AmbientMusicController.Track Create(
      AudioClip preAudioClip,
      AudioClip audioClip,
      bool Permenant)
    {
      AmbientMusicController.Track track = new AmbientMusicController.Track(preAudioClip, audioClip)
      {
        Permenant = Permenant
      };
      track.gameObject.Tracks.Add(track);
      return track;
    }

    public void Play(bool FadeOthers, float FadeIn, bool Loop = true)
    {
      if (FadeOthers)
        AmbientMusicController.FadeAll(this);
      if (this.cFadeIn != null)
        this.gameObject.StopCoroutine(this.cFadeIn);
      this.cFadeIn = this.gameObject.StartCoroutine((IEnumerator) this.DoFadeIn(FadeIn));
      if ((UnityEngine.Object) this.preAudioClip != (UnityEngine.Object) null)
      {
        if (this.cDoPreRoll != null)
          this.gameObject.StopCoroutine(this.cDoPreRoll);
        this.cDoPreRoll = this.gameObject.StartCoroutine((IEnumerator) this.DoPreRoll());
      }
      else
      {
        if (!((UnityEngine.Object) this.audioSource.clip == (UnityEngine.Object) null) && this.audioSource.isPlaying)
          return;
        this.audioSource.clip = this.audioClip;
        this.audioSource.loop = Loop;
        this.audioSource.Play();
      }
    }

    public void PlayLayer(int Index)
    {
      if ((UnityEngine.Object) this.Layers[Index].audioSource != (UnityEngine.Object) null && (UnityEngine.Object) this.audioSource != (UnityEngine.Object) null)
        this.Layers[Index].audioSource.time = this.audioSource.time % this.Layers[Index].audioSource.time;
      this.Layers[Index].Play(false, 0.0f);
    }

    public void Stop()
    {
      this.audioSource.Stop();
      foreach (AmbientMusicController.Track layer in this.Layers)
        layer.audioSource.Stop();
    }

    public void StopLayer(int Index, float Fadeout) => this.Layers[Index].FadeOut(Fadeout, false);

    public IEnumerator DoPreRoll()
    {
      this.audioSource.clip = this.preAudioClip;
      this.audioSource.Play();
      yield return (object) new WaitForSeconds(this.preAudioClip.length);
      this.audioSource.clip = this.audioClip;
      this.audioSource.Play();
    }

    public IEnumerator DoFadeIn(float Duration)
    {
      while ((double) (this.audioSource.volume += Time.deltaTime * (1f / Duration)) < (double) this.Volume)
        yield return (object) null;
      this.audioSource.volume = this.Volume;
    }

    public void FadeOut(float Duration, bool FadeLayers)
    {
      if (this.cFadeOut != null)
        AmbientMusicController.Instance.StopCoroutine(this.cFadeOut);
      this.cFadeOut = AmbientMusicController.Instance.StartCoroutine((IEnumerator) this.DoFadeOut(Duration));
      if (!FadeLayers)
        return;
      foreach (AmbientMusicController.Track layer in this.Layers)
        layer.FadeOut(Duration, false);
    }

    public IEnumerator DoFadeOut(float Duration)
    {
      AmbientMusicController.Track track = this;
      while ((double) (track.audioSource.volume -= Time.deltaTime * (1f / Duration)) > 0.0)
        yield return (object) null;
      track.audioSource.volume = 0.0f;
      if (!track.Permenant)
      {
        track.audioSource.Stop();
        track.audioSource.clip = (AudioClip) null;
        AmbientMusicController.Instance.Tracks.Remove(track);
        UnityEngine.Object.Destroy((UnityEngine.Object) track.audioSource);
      }
    }
  }
}
