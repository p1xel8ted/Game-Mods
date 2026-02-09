// Decompiled with JetBrains decompiler
// Type: SmartSpeechEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class SmartSpeechEngine : MonoBehaviour
{
  public List<SmartSpeechEngine.VoiceDefinition> voices = new List<SmartSpeechEngine.VoiceDefinition>();
  public static SmartSpeechEngine _me;
  public AudioSource[] _audio_sources;
  public uint[] _audio_queue;
  public uint _cur_audio_qn;
  public bool _playing_series;
  public SmartSpeechEngine.VoiceID _playing_series_id;
  public float _playing_series_end_time;
  public System.Action _on_series_finished;

  public static SmartSpeechEngine me
  {
    get
    {
      if ((UnityEngine.Object) SmartSpeechEngine._me == (UnityEngine.Object) null)
        SmartSpeechEngine._me = UnityEngine.Object.FindObjectOfType<SmartSpeechEngine>();
      return SmartSpeechEngine._me;
    }
  }

  [ContextMenu("Init")]
  public void Init()
  {
    this._audio_sources = this.GetComponentsInChildren<AudioSource>(true);
    this._audio_queue = new uint[this._audio_sources.Length];
    this._cur_audio_qn = 0U;
  }

  public void PlayVoiceSound(SmartSpeechEngine.VoiceID voice_id, float volume = 1f)
  {
    if (this._audio_sources == null || this._audio_sources.Length == 0)
      this.Init();
    int channel_number = 0;
    uint num = this._audio_queue[0];
    for (int index = 1; index < this._audio_sources.Length; ++index)
    {
      uint audio = this._audio_queue[index];
      if (audio < num)
      {
        channel_number = index;
        num = audio;
      }
    }
    foreach (SmartSpeechEngine.VoiceDefinition voice in this.voices)
    {
      if (voice.id == voice_id)
      {
        this.PlayVoiceSoundInternal(channel_number, voice, volume);
        break;
      }
    }
  }

  public void PlayVoiceSoundInternal(
    int channel_number,
    SmartSpeechEngine.VoiceDefinition vd,
    float volume = 1f)
  {
    if (!vd.IsAvailableToPlay())
      return;
    int index = 0;
    if (vd.sounds.Length == 0)
      return;
    if (vd.sounds.Length > 1)
    {
      do
      {
        index = NGUITools.RandomRange(0, vd.sounds.Length - 1);
      }
      while (index == vd.prev_random);
      vd.prev_random = index;
    }
    SmartSpeechEngine.AudioClipDefinition sound = vd.sounds[index];
    this.PlaySound(channel_number, sound, vd.volume * volume, vd.pitch);
  }

  public void PlaySound(
    int channel_number,
    SmartSpeechEngine.AudioClipDefinition sound,
    float voice_volume = 1f,
    float pitch = 1f)
  {
    this._audio_queue[channel_number] = ++this._cur_audio_qn;
    AudioSource audioSource = this._audio_sources[channel_number];
    if (audioSource.isPlaying)
      audioSource.Stop();
    audioSource.clip = sound.audio;
    audioSource.volume = sound.volume * voice_volume;
    audioSource.pitch = pitch;
    audioSource.Play();
  }

  public void PlayVoiceSeries(
    SmartSpeechEngine.VoiceID voice_id,
    float length,
    System.Action on_series_finished = null)
  {
    this._playing_series = true;
    this._playing_series_id = voice_id;
    this._playing_series_end_time = Time.realtimeSinceStartup + length;
    this._on_series_finished = on_series_finished;
    this.PlayVoiceSound(voice_id);
  }

  public void UpdatePlayingSeries()
  {
    if ((UnityEngine.Object) SmartSpeechEngine._me == (UnityEngine.Object) null || !this._playing_series)
      return;
    this.PlayVoiceSound(this._playing_series_id);
    if ((double) this._playing_series_end_time >= (double) Time.realtimeSinceStartup)
      return;
    this._playing_series = false;
    if (this._on_series_finished == null)
      return;
    System.Action onSeriesFinished = this._on_series_finished;
    this._on_series_finished = (System.Action) null;
    onSeriesFinished();
  }

  public void Update() => this.UpdatePlayingSeries();

  public enum VoiceID
  {
    None = 0,
    Skull = 1,
    Bishop = 2,
    Inquisitor = 3,
    Actress = 4,
    Merchant = 5,
    Cultist = 6,
    Astrologer = 7,
    Guard = 8,
    Donkey = 9,
    CellPhone = 10, // 0x0000000A
    TavernKeeper = 11, // 0x0000000B
    Blacksmith = 12, // 0x0000000C
    Actor = 13, // 0x0000000D
    RedEye = 14, // 0x0000000E
    Dig = 15, // 0x0000000F
    Carpenter = 16, // 0x00000010
    FarmersSon = 17, // 0x00000011
    FarmersDaughter = 18, // 0x00000012
    Engineer = 19, // 0x00000013
    Capitan = 20, // 0x00000014
    Witch = 21, // 0x00000015
    LightKeeper = 22, // 0x00000016
    Miller = 23, // 0x00000017
    Farmer = 24, // 0x00000018
    WoodCutter = 25, // 0x00000019
    Ghost = 26, // 0x0000001A
    Zombie = 27, // 0x0000001B
    LordCommander = 28, // 0x0000001C
    Satyr = 29, // 0x0000001D
    Gypsy = 30, // 0x0000001E
    MsChain = 31, // 0x0000001F
    Gunter = 32, // 0x00000020
    Bella = 33, // 0x00000021
    Jove = 34, // 0x00000022
    Lucius = 35, // 0x00000023
    Teacher = 36, // 0x00000024
    WitchYoung = 37, // 0x00000025
    MasterAlarich = 38, // 0x00000026
    Beatris = 39, // 0x00000027
    MarquisTeodoroJr = 40, // 0x00000028
    Hunchback = 41, // 0x00000029
    GhostPriest = 42, // 0x0000002A
    WomanBlackGold = 43, // 0x0000002B
    Shepherd = 44, // 0x0000002C
    VampireCommon = 45, // 0x0000002D
    RefugeeCoffinMaker = 46, // 0x0000002E
    RefugeeCook = 47, // 0x0000002F
    RefugeeTanner = 48, // 0x00000030
    MarquisTeodoroJrAfflicted = 49, // 0x00000031
    RefugeeMoneylender = 50, // 0x00000032
    RefugeeMan = 51, // 0x00000033
    ShepherdsWife = 52, // 0x00000034
    VampireCarl = 53, // 0x00000035
    Potter = 54, // 0x00000036
    BeeKeeper = 55, // 0x00000037
    Euric = 56, // 0x00000038
    Smiler = 57, // 0x00000039
    EuricInBag = 58, // 0x0000003A
    Player = 100, // 0x00000064
    Unused = 101, // 0x00000065
    Unused2 = 102, // 0x00000066
    Unused3 = 103, // 0x00000067
    Unused4 = 104, // 0x00000068
    Unused5 = 105, // 0x00000069
  }

  [Serializable]
  public class VoiceDefinition
  {
    public SmartSpeechEngine.VoiceID id;
    [Range(0.05f, 1f)]
    public float min_period = 0.13f;
    [Range(0.0f, 1f)]
    public float volume = 1f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;
    public SmartSpeechEngine.AudioClipDefinition[] sounds;
    public float _last_time_played;
    public string _play_button_symbol = "►►";
    [NonSerialized]
    public int prev_random = -1;

    public void Play() => SmartSpeechEngine.me.PlayVoiceSound(this.id);

    public void PlaySeries()
    {
      this._play_button_symbol = "...";
      SmartSpeechEngine.me.PlayVoiceSeries(this.id, UnityEngine.Random.Range(0.4f, 1.5f), (System.Action) (() => this._play_button_symbol = "►►"));
    }

    public bool IsAvailableToPlay()
    {
      if ((double) Time.realtimeSinceStartup <= (double) this._last_time_played + (double) this.min_period / (double) this.pitch)
        return false;
      this._last_time_played = Time.realtimeSinceStartup;
      return true;
    }

    [CompilerGenerated]
    public void \u003CPlaySeries\u003Eb__2_0() => this._play_button_symbol = "►►";
  }

  [Serializable]
  public class AudioClipDefinition
  {
    public AudioClip audio;
    [Range(0.0f, 1f)]
    public float volume = 1f;

    public void Play()
    {
      float voice_volume = 1f;
      float pitch = 1f;
      foreach (SmartSpeechEngine.VoiceDefinition voice in SmartSpeechEngine.me.voices)
      {
        foreach (SmartSpeechEngine.AudioClipDefinition sound in voice.sounds)
        {
          if (sound == this)
          {
            voice_volume = voice.volume;
            pitch = voice.pitch;
          }
        }
      }
      SmartSpeechEngine.me.PlaySound(0, this, voice_volume, pitch);
    }
  }
}
