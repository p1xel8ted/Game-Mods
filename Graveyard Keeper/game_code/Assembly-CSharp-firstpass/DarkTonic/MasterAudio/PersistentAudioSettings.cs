// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.PersistentAudioSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class PersistentAudioSettings
{
  public const string SfxVolKey = "MA_sfxVolume";
  public const string MusicVolKey = "MA_musicVolume";
  public const string SfxMuteKey = "MA_sfxMute";
  public const string MusicMuteKey = "MA_musicMute";
  public const string BusVolKey = "MA_BusVolume_";
  public const string GroupVolKey = "MA_GroupVolume_";
  public const string BusKeysKey = "MA_BusKeys";
  public const string GroupKeysKey = "MA_GroupsKeys";
  public const string Separator = ";";

  public static void SetBusVolume(string busName, float vol)
  {
    PlayerPrefs.SetFloat(PersistentAudioSettings.MakeBusKey(busName), vol);
    if ((Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (Object) null)
      return;
    if (DarkTonic.MasterAudio.MasterAudio.GrabBusByName(busName) != null)
      DarkTonic.MasterAudio.MasterAudio.SetBusVolumeByName(busName, vol);
    if (PersistentAudioSettings.BusesUpdatedKeys.Contains($";{busName};"))
      return;
    PersistentAudioSettings.BusesUpdatedKeys = $"{PersistentAudioSettings.BusesUpdatedKeys}{busName};";
  }

  public static string BusesUpdatedKeys
  {
    get
    {
      if (!PlayerPrefs.HasKey("MA_BusKeys"))
        PlayerPrefs.SetString("MA_BusKeys", ";");
      return PlayerPrefs.GetString("MA_BusKeys");
    }
    set => PlayerPrefs.SetString("MA_BusKeys", value);
  }

  public static string MakeBusKey(string busName) => "MA_BusVolume_" + busName;

  public static float? GetBusVolume(string busName)
  {
    string key = PersistentAudioSettings.MakeBusKey(busName);
    return !PlayerPrefs.HasKey(key) ? new float?() : new float?(PlayerPrefs.GetFloat(key));
  }

  public static string GetGroupKey(string groupName) => "MA_GroupVolume_" + groupName;

  public static string GroupsUpdatedKeys
  {
    get
    {
      if (!PlayerPrefs.HasKey("MA_GroupsKeys"))
        PlayerPrefs.SetString("MA_GroupsKeys", ";");
      return PlayerPrefs.GetString("MA_GroupsKeys");
    }
    set => PlayerPrefs.SetString("MA_GroupsKeys", value);
  }

  public static void SetGroupVolume(string grpName, float vol)
  {
    PlayerPrefs.SetFloat(PersistentAudioSettings.GetGroupKey(grpName), vol);
    if ((Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance == (Object) null)
      return;
    if ((Object) DarkTonic.MasterAudio.MasterAudio.GrabGroup(grpName, false) != (Object) null)
      DarkTonic.MasterAudio.MasterAudio.SetGroupVolume(grpName, vol);
    if (PersistentAudioSettings.GroupsUpdatedKeys.Contains($";{grpName};"))
      return;
    PersistentAudioSettings.GroupsUpdatedKeys = $"{PersistentAudioSettings.GroupsUpdatedKeys}{grpName};";
  }

  public static float? GetGroupVolume(string grpName)
  {
    string groupKey = PersistentAudioSettings.GetGroupKey(grpName);
    return !PlayerPrefs.HasKey(groupKey) ? new float?() : new float?(PlayerPrefs.GetFloat(groupKey));
  }

  public static bool? MixerMuted
  {
    get
    {
      return !PlayerPrefs.HasKey("MA_sfxMute") ? new bool?() : new bool?(PlayerPrefs.GetInt("MA_sfxMute") != 0);
    }
    set
    {
      if (!value.HasValue)
      {
        PlayerPrefs.DeleteKey("MA_sfxMute");
      }
      else
      {
        bool flag = value.Value;
        PlayerPrefs.SetInt("MA_sfxMute", flag ? 1 : 0);
        if (!((Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance != (Object) null))
          return;
        DarkTonic.MasterAudio.MasterAudio.MixerMuted = flag;
      }
    }
  }

  public static float? MixerVolume
  {
    get
    {
      return !PlayerPrefs.HasKey("MA_sfxVolume") ? new float?() : new float?(PlayerPrefs.GetFloat("MA_sfxVolume"));
    }
    set
    {
      if (!value.HasValue)
      {
        PlayerPrefs.DeleteKey("MA_sfxVolume");
      }
      else
      {
        float num = value.Value;
        PlayerPrefs.SetFloat("MA_sfxVolume", num);
        if (!((Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance != (Object) null))
          return;
        DarkTonic.MasterAudio.MasterAudio.MasterVolumeLevel = num;
      }
    }
  }

  public static bool? MusicMuted
  {
    get
    {
      return !PlayerPrefs.HasKey("MA_musicMute") ? new bool?() : new bool?(PlayerPrefs.GetInt("MA_musicMute") != 0);
    }
    set
    {
      if (!value.HasValue)
      {
        PlayerPrefs.DeleteKey("MA_musicMute");
      }
      else
      {
        bool flag = value.Value;
        PlayerPrefs.SetInt("MA_musicMute", flag ? 1 : 0);
        if (!((Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance != (Object) null))
          return;
        DarkTonic.MasterAudio.MasterAudio.PlaylistsMuted = flag;
      }
    }
  }

  public static float? MusicVolume
  {
    get
    {
      return !PlayerPrefs.HasKey("MA_musicVolume") ? new float?() : new float?(PlayerPrefs.GetFloat("MA_musicVolume"));
    }
    set
    {
      if (!value.HasValue)
      {
        PlayerPrefs.DeleteKey("MA_musicVolume");
      }
      else
      {
        float num = value.Value;
        PlayerPrefs.SetFloat("MA_musicVolume", num);
        if (!((Object) DarkTonic.MasterAudio.MasterAudio.SafeInstance != (Object) null))
          return;
        DarkTonic.MasterAudio.MasterAudio.PlaylistMasterVolume = num;
      }
    }
  }

  public static void RestoreMasterSettings()
  {
    if (PersistentAudioSettings.MixerVolume.HasValue)
      DarkTonic.MasterAudio.MasterAudio.MasterVolumeLevel = PersistentAudioSettings.MixerVolume.Value;
    if (PersistentAudioSettings.MixerMuted.HasValue)
      DarkTonic.MasterAudio.MasterAudio.MixerMuted = PersistentAudioSettings.MixerMuted.Value;
    if (PersistentAudioSettings.MusicVolume.HasValue)
      DarkTonic.MasterAudio.MasterAudio.PlaylistMasterVolume = PersistentAudioSettings.MusicVolume.Value;
    if (!PersistentAudioSettings.MusicMuted.HasValue)
      return;
    DarkTonic.MasterAudio.MasterAudio.PlaylistsMuted = PersistentAudioSettings.MusicMuted.Value;
  }
}
