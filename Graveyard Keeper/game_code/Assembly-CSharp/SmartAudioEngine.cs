// Decompiled with JetBrains decompiler
// Type: SmartAudioEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DarkTonic.MasterAudio;
using LinqTools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

#nullable disable
public class SmartAudioEngine : MonoBehaviour
{
  public List<SmartAudioSound> sounds = new List<SmartAudioSound>();
  public static Dictionary<string, SmartAudioEngine.SoundState> _playing_sounds = new Dictionary<string, SmartAudioEngine.SoundState>();
  public AudioMixer mixer;
  public PlaylistController pl_music;
  public PlaylistController pl_music_ovr;
  public string _last_ovr_music = "";
  public float _cur_ovr_time;
  public string _stop_ovr_music_asap;
  public List<string> _ovr_musics = new List<string>();
  public float min_ovr_music_time = 20f;
  public float music_crossfade_time = 5f;
  public ObjectDefinition _cur_interaction_npc;
  public static SmartAudioEngine _me = (SmartAudioEngine) null;

  public static SmartAudioEngine me
  {
    get
    {
      if ((Object) SmartAudioEngine._me == (Object) null)
        SmartAudioEngine._me = Object.FindObjectOfType<SmartAudioEngine>();
      return SmartAudioEngine._me;
    }
  }

  public SmartAudioSound GetSoundByID(string id)
  {
    foreach (SmartAudioSound sound in this.sounds)
    {
      if (sound.id == id)
        return sound;
    }
    Debug.LogError((object) ("Smart sound not found, id = " + id));
    return (SmartAudioSound) null;
  }

  public void SetSoundWeight(string id, float weight)
  {
    this.GetSoundByID(id)?.SetSoundWeight(weight);
  }

  public void Update()
  {
    if (!MainGame.game_started)
      return;
    float deltaTime = Time.deltaTime;
    foreach (SmartAudioSound sound in this.sounds)
      sound.CustomUpdate(deltaTime);
    if (this._ovr_musics.Count <= 0)
      return;
    this._cur_ovr_time += Time.deltaTime;
    if ((double) this._cur_ovr_time <= (double) this.min_ovr_music_time || string.IsNullOrEmpty(this._stop_ovr_music_asap))
      return;
    this.StopOvrMusic(this._stop_ovr_music_asap);
  }

  public void PlaySound(string sound_group_id)
  {
    foreach (SmartAudioSound sound in this.sounds)
    {
      if (!(sound.sound_group != sound_group_id))
        sound.Play();
    }
  }

  public void PlaySoundWithFade(string sound_group_id)
  {
    foreach (SmartAudioSound sound in this.sounds)
    {
      if (!(sound.sound_group != sound_group_id))
        sound.PlayWithFade();
    }
  }

  public void SetSoundVolume(string sound_group_id, float volume)
  {
    foreach (SmartAudioSound sound in this.sounds)
    {
      if (!(sound.sound_group != sound_group_id))
        sound.SetSoundVolume(volume);
    }
  }

  public void StopSoundWithFade(string sound_group_id)
  {
    foreach (SmartAudioSound sound in this.sounds)
    {
      if (!(sound.sound_group != sound_group_id))
        sound.StopWithFade();
    }
  }

  public void SetDullMusicMode(bool dull_mode = true)
  {
    Debug.Log((object) ("SetDullMusicMode " + dull_mode.ToString()));
    this.mixer.SetFloat("music_cutoff", dull_mode ? 900f : 44000f);
    this.mixer.SetFloat("sfx_cutoff", dull_mode ? 5000f : 22000f);
    this.mixer.SetFloat("environment_cutoff", dull_mode ? 5000f : 22000f);
    if (!dull_mode)
      return;
    Sounds.OnWindowOpened();
  }

  public void SetChannelVolume(string channel_name, float volume_0_1, float add = 0.0f)
  {
    float num = Mathf.Lerp(-80f, 0.0f, (float) (((double) Mathf.Log10(volume_0_1 + 0.1f) + 1.0) * 0.95999997854232788));
    this.mixer.SetFloat("volume_" + channel_name, num + add);
  }

  public void OnOvrMusicStopped(string music_id)
  {
    if (this._ovr_musics.Count <= 0 || !(this._ovr_musics[this._ovr_musics.Count - 1] == music_id))
      return;
    this._ovr_musics.RemoveAt(this._ovr_musics.Count - 1);
  }

  public void OnOvrMusicStarted(string music_id)
  {
    if (this._ovr_musics.Contains(music_id))
      this._ovr_musics.Remove(music_id);
    this._ovr_musics.Add(music_id);
  }

  public void StopOvrMusic(string music_id = null, bool force_immediate = false)
  {
    Debug.Log((object) ("#snd# StopOvrMusic: " + music_id));
    this._stop_ovr_music_asap = (string) null;
    if (string.IsNullOrEmpty(music_id))
      this._ovr_musics.Clear();
    else if ((this._ovr_musics.Count == 0 ? (string) null : this._ovr_musics.Last<string>()) == music_id)
    {
      if ((double) this._cur_ovr_time > (double) this.min_ovr_music_time | force_immediate)
      {
        this._ovr_musics.Remove(music_id);
      }
      else
      {
        this._stop_ovr_music_asap = music_id;
        return;
      }
    }
    else
      this._ovr_musics.Remove(music_id);
    if (this._ovr_musics.Count == 0)
    {
      this.pl_music_ovr.FadeToVolume(0.0f, this.music_crossfade_time);
      this.pl_music.FadeToVolume(1f, this.music_crossfade_time);
    }
    else
    {
      this._last_ovr_music = this._ovr_musics.Last<string>();
      this.pl_music_ovr.TriggerPlaylistClip(this._last_ovr_music);
    }
  }

  public void PlayOvrMusic(string music_id)
  {
    Debug.Log((object) ("#snd# PlayOvrMusic: " + music_id));
    this._stop_ovr_music_asap = (string) null;
    string str = this._ovr_musics.Count == 0 ? (string) null : this._ovr_musics.Last<string>();
    if (music_id == str)
      return;
    if (string.IsNullOrEmpty(str))
    {
      if (!this.pl_music_ovr.ActiveAudioSource.isPlaying)
        this.pl_music_ovr.StartPlaylist("ovr_music");
      this.pl_music_ovr.FadeToVolume(1f, this.music_crossfade_time);
      this.pl_music.FadeToVolume(0.0f, this.music_crossfade_time);
    }
    if (this._last_ovr_music != music_id)
    {
      this.pl_music_ovr.TriggerPlaylistClip(music_id);
      this._last_ovr_music = music_id;
    }
    if (this._ovr_musics.Contains(music_id))
      this._ovr_musics.Remove(music_id);
    this._ovr_musics.Add(music_id);
    this._cur_ovr_time = 0.0f;
    this._stop_ovr_music_asap = (string) null;
  }

  public void OnStartNPCInteraction(ObjectDefinition npc)
  {
    if (this._cur_interaction_npc == npc)
      return;
    this._cur_interaction_npc = npc;
    if (string.IsNullOrEmpty(npc.ovr_music))
      return;
    this.PlayOvrMusic(npc.ovr_music);
  }

  public void OnEndNPCInteraction(ObjectDefinition npc = null)
  {
    if (this._cur_interaction_npc == null || npc != this._cur_interaction_npc && npc != null)
      return;
    if (!string.IsNullOrEmpty(this._cur_interaction_npc.ovr_music))
      this.StopOvrMusic(this._cur_interaction_npc.ovr_music);
    this._cur_interaction_npc = (ObjectDefinition) null;
  }

  public void TransitionToSnapshot(string snapshot_name, float time_to_reach = 0.0f)
  {
    AudioMixerSnapshot snapshot = this.mixer.FindSnapshot(snapshot_name);
    if ((Object) snapshot != (Object) null)
    {
      snapshot.TransitionTo(time_to_reach);
      Debug.Log((object) $"Snapshot {snapshot_name} transition with time: {time_to_reach.ToString()}");
    }
    else
      Debug.Log((object) $"Tried to transition to snapshot with name {snapshot_name} , but wasn't found");
  }

  public void StopAllSmartSounds()
  {
    foreach (SmartAudioSound sound in this.sounds)
    {
      sound.Stop();
      sound.CustomUpdate(Time.deltaTime);
    }
  }

  public enum SoundState
  {
    FadeIn,
    FadeOut,
    Playing,
  }
}
