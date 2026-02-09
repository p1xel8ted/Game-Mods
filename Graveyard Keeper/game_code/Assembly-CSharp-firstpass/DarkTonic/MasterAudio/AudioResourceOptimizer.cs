// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AudioResourceOptimizer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class AudioResourceOptimizer
{
  public static Dictionary<string, List<AudioSource>> AudioResourceTargetsByName = new Dictionary<string, List<AudioSource>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public static Dictionary<string, AudioClip> AudioClipsByName = new Dictionary<string, AudioClip>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public static Dictionary<string, List<AudioClip>> PlaylistClipsByPlaylistName = new Dictionary<string, List<AudioClip>>(5, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public static List<string> InternetFilesStartedLoading = new List<string>();
  public static string _supportedLanguageFolder = string.Empty;

  public static void ClearAudioClips()
  {
    AudioResourceOptimizer.AudioClipsByName.Clear();
    AudioResourceOptimizer.AudioResourceTargetsByName.Clear();
  }

  public static string GetLocalizedDynamicSoundGroupFileName(
    SystemLanguage localLanguage,
    bool useLocalization,
    string resourceFileName)
  {
    if (!useLocalization)
      return resourceFileName;
    return (UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.Instance != (UnityEngine.Object) null ? AudioResourceOptimizer.GetLocalizedFileName(useLocalization, resourceFileName) : $"{localLanguage.ToString()}/{resourceFileName}";
  }

  public static string GetLocalizedFileName(bool useLocalization, string resourceFileName)
  {
    return !useLocalization ? resourceFileName : $"{AudioResourceOptimizer.SupportedLanguageFolder()}/{resourceFileName}";
  }

  public static void AddTargetForClip(string clipName, AudioSource source)
  {
    if (!AudioResourceOptimizer.AudioResourceTargetsByName.ContainsKey(clipName))
    {
      AudioResourceOptimizer.AudioResourceTargetsByName.Add(clipName, new List<AudioSource>()
      {
        source
      });
    }
    else
    {
      List<AudioSource> audioSourceList = AudioResourceOptimizer.AudioResourceTargetsByName[clipName];
      AudioClip audioClip = (AudioClip) null;
      for (int index = 0; index < audioSourceList.Count; ++index)
      {
        AudioClip clip = audioSourceList[index].clip;
        if (!((UnityEngine.Object) clip == (UnityEngine.Object) null))
        {
          audioClip = clip;
          break;
        }
      }
      if ((UnityEngine.Object) audioClip != (UnityEngine.Object) null)
      {
        source.clip = audioClip;
        SoundGroupVariation component = source.GetComponent<SoundGroupVariation>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.internetFileLoadStatus = DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loaded;
      }
      audioSourceList.Add(source);
    }
  }

  public static string SupportedLanguageFolder()
  {
    if (!string.IsNullOrEmpty(AudioResourceOptimizer._supportedLanguageFolder))
      return AudioResourceOptimizer._supportedLanguageFolder;
    SystemLanguage systemLanguage = Application.systemLanguage;
    if ((UnityEngine.Object) DarkTonic.MasterAudio.MasterAudio.Instance != (UnityEngine.Object) null)
    {
      switch (DarkTonic.MasterAudio.MasterAudio.Instance.langMode)
      {
        case DarkTonic.MasterAudio.MasterAudio.LanguageMode.SpecificLanguage:
          systemLanguage = DarkTonic.MasterAudio.MasterAudio.Instance.testLanguage;
          break;
        case DarkTonic.MasterAudio.MasterAudio.LanguageMode.DynamicallySet:
          systemLanguage = DarkTonic.MasterAudio.MasterAudio.DynamicLanguage;
          break;
      }
    }
    AudioResourceOptimizer._supportedLanguageFolder = !DarkTonic.MasterAudio.MasterAudio.Instance.supportedLanguages.Contains(systemLanguage) ? DarkTonic.MasterAudio.MasterAudio.Instance.defaultLanguage.ToString() : systemLanguage.ToString();
    return AudioResourceOptimizer._supportedLanguageFolder;
  }

  public static void ClearSupportLanguageFolder()
  {
    AudioResourceOptimizer._supportedLanguageFolder = string.Empty;
  }

  public static AudioClip PopulateResourceSongToPlaylistController(
    string controllerName,
    string songResourceName,
    string playlistName)
  {
    AudioClip playlistController = Resources.Load(songResourceName) as AudioClip;
    if ((UnityEngine.Object) playlistController == (UnityEngine.Object) null)
    {
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Resource file '{songResourceName}' could not be located from Playlist '{playlistName}'.");
      return (AudioClip) null;
    }
    if (!AudioUtil.AudioClipWillPreload(playlistController))
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Audio Clip for Resource file '{songResourceName}' from Playlist '{playlistName}' has 'Preload Audio Data' turned off, which can cause audio glitches. Resource files should always Preload Audio Data. Please turn it on.");
    AudioResourceOptimizer.FinishRecordingPlaylistClip(controllerName, playlistController);
    return playlistController;
  }

  public static void FinishRecordingPlaylistClip(string controllerName, AudioClip resAudioClip)
  {
    List<AudioClip> audioClipList;
    if (!AudioResourceOptimizer.PlaylistClipsByPlaylistName.ContainsKey(controllerName))
    {
      audioClipList = new List<AudioClip>(5);
      AudioResourceOptimizer.PlaylistClipsByPlaylistName.Add(controllerName, audioClipList);
    }
    else
      audioClipList = AudioResourceOptimizer.PlaylistClipsByPlaylistName[controllerName];
    audioClipList.Add(resAudioClip);
  }

  public static IEnumerator PopulateResourceSongToPlaylistControllerAsync(
    string songResourceName,
    string playlistName,
    PlaylistController controller,
    PlaylistController.AudioPlayType playType)
  {
    ResourceRequest asyncRes = Resources.LoadAsync(songResourceName, typeof (AudioClip));
    while (!asyncRes.isDone)
      yield return (object) DarkTonic.MasterAudio.MasterAudio.EndOfFrameDelay;
    AudioClip asset = asyncRes.asset as AudioClip;
    if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
    {
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Resource file '{songResourceName}' could not be located from Playlist '{playlistName}'.");
    }
    else
    {
      if (!AudioUtil.AudioClipWillPreload(asset))
        DarkTonic.MasterAudio.MasterAudio.LogWarning($"Audio Clip for Resource file '{songResourceName}' from Playlist '{playlistName}' has 'Preload Audio Data' turned off, which can cause audio glitches. Resource files should always Preload Audio Data. Please turn it on.");
      AudioResourceOptimizer.FinishRecordingPlaylistClip(controller.ControllerName, asset);
      controller.FinishLoadingNewSong(asset, playType);
    }
  }

  public static IEnumerator PopulateSourceWithInternetFile(
    string fileUrl,
    SoundGroupVariation variation,
    Action successAction,
    Action failureAction)
  {
    if (AudioResourceOptimizer.AudioClipsByName.ContainsKey(fileUrl))
    {
      if (successAction != null)
        successAction();
    }
    else if (!AudioResourceOptimizer.InternetFilesStartedLoading.Contains(fileUrl))
    {
      AudioResourceOptimizer.InternetFilesStartedLoading.Add(fileUrl);
      AudioClip audioClip;
      using (WWW fileRequest = new WWW(fileUrl))
      {
        yield return (object) fileRequest;
        if (fileRequest.error != null)
        {
          if (string.IsNullOrEmpty(fileUrl))
            DarkTonic.MasterAudio.MasterAudio.LogWarning($"Internet file is EMPTY for a Variation of Sound Group '{variation.ParentGroup.name}' could not be loaded.");
          else
            DarkTonic.MasterAudio.MasterAudio.LogWarning($"Internet file '{fileUrl}' in a Variation of Sound Group '{variation.ParentGroup.name}' could not be loaded. This can happen if the URL is incorrect or you are not online.");
          if (failureAction != null)
            failureAction();
          yield break;
        }
        audioClip = fileRequest.GetAudioClip();
        string[] segments = new Uri(fileUrl).Segments;
        audioClip.name = Path.GetFileNameWithoutExtension(segments[segments.Length - 1]);
      }
      if (!AudioResourceOptimizer.AudioResourceTargetsByName.ContainsKey(fileUrl))
      {
        DarkTonic.MasterAudio.MasterAudio.LogError($"No Audio Sources found to add Internet File '{fileUrl}' to.");
        if (failureAction != null)
          failureAction();
      }
      else
      {
        List<AudioSource> audioSourceList = AudioResourceOptimizer.AudioResourceTargetsByName[fileUrl];
        for (int index = 0; index < audioSourceList.Count; ++index)
        {
          audioSourceList[index].clip = audioClip;
          SoundGroupVariation component = audioSourceList[index].GetComponent<SoundGroupVariation>();
          if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
            component.internetFileLoadStatus = DarkTonic.MasterAudio.MasterAudio.InternetFileLoadStatus.Loaded;
        }
        if (!AudioResourceOptimizer.AudioClipsByName.ContainsKey(fileUrl))
          AudioResourceOptimizer.AudioClipsByName.Add(fileUrl, audioClip);
        if (successAction != null)
          successAction();
      }
    }
  }

  public static void RemoveLoadedInternetClip(string fileUrl)
  {
    if (!AudioResourceOptimizer.InternetFilesStartedLoading.Contains(fileUrl))
      return;
    AudioResourceOptimizer.InternetFilesStartedLoading.Remove(fileUrl);
    if (AudioResourceOptimizer.AudioResourceTargetsByName.ContainsKey(fileUrl))
    {
      List<AudioSource> audioSourceList = AudioResourceOptimizer.AudioResourceTargetsByName[fileUrl];
      for (int index = 0; index < audioSourceList.Count; ++index)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) audioSourceList[index].clip);
        audioSourceList[index].clip = (AudioClip) null;
      }
      AudioResourceOptimizer.AudioResourceTargetsByName.Remove(fileUrl);
    }
    if (!AudioResourceOptimizer.AudioClipsByName.ContainsKey(fileUrl))
      return;
    AudioResourceOptimizer.AudioClipsByName.Remove(fileUrl);
  }

  public static IEnumerator PopulateSourcesWithResourceClipAsync(
    string clipName,
    SoundGroupVariation variation,
    Action successAction,
    Action failureAction)
  {
    if (AudioResourceOptimizer.AudioClipsByName.ContainsKey(clipName))
    {
      if (successAction != null)
        successAction();
    }
    else
    {
      ResourceRequest asyncRes = Resources.LoadAsync(clipName, typeof (AudioClip));
      while (!asyncRes.isDone)
        yield return (object) DarkTonic.MasterAudio.MasterAudio.EndOfFrameDelay;
      AudioClip asset = asyncRes.asset as AudioClip;
      if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
      {
        DarkTonic.MasterAudio.MasterAudio.LogError($"Resource file '{clipName}' could not be located.");
        if (failureAction != null)
          failureAction();
      }
      else if (!AudioResourceOptimizer.AudioResourceTargetsByName.ContainsKey(clipName))
      {
        DarkTonic.MasterAudio.MasterAudio.LogError($"No Audio Sources found to add Resource file '{clipName}'.");
        if (failureAction != null)
          failureAction();
      }
      else
      {
        if (!AudioUtil.AudioClipWillPreload(asset))
          DarkTonic.MasterAudio.MasterAudio.LogWarning($"Audio Clip for Resource file '{clipName}' of Sound Group '{variation.ParentGroup.name}' has 'Preload Audio Data' turned off, which can cause audio glitches. Resource files should always Preload Audio Data. Please turn it on.");
        List<AudioSource> audioSourceList = AudioResourceOptimizer.AudioResourceTargetsByName[clipName];
        for (int index = 0; index < audioSourceList.Count; ++index)
          audioSourceList[index].clip = asset;
        if (!AudioResourceOptimizer.AudioClipsByName.ContainsKey(clipName))
          AudioResourceOptimizer.AudioClipsByName.Add(clipName, asset);
        if (successAction != null)
          successAction();
      }
    }
  }

  public static void UnloadPlaylistSongIfUnused(string controllerName, AudioClip clipToRemove)
  {
    if ((UnityEngine.Object) clipToRemove == (UnityEngine.Object) null || !AudioResourceOptimizer.PlaylistClipsByPlaylistName.ContainsKey(controllerName))
      return;
    List<AudioClip> audioClipList = AudioResourceOptimizer.PlaylistClipsByPlaylistName[controllerName];
    if (!audioClipList.Contains(clipToRemove))
      return;
    audioClipList.Remove(clipToRemove);
    if (audioClipList.Contains(clipToRemove))
      return;
    Resources.UnloadAsset((UnityEngine.Object) clipToRemove);
  }

  public static bool PopulateSourcesWithResourceClip(string clipName, SoundGroupVariation variation)
  {
    if (AudioResourceOptimizer.AudioClipsByName.ContainsKey(clipName))
      return true;
    AudioClip clip = Resources.Load(clipName) as AudioClip;
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"Resource file '{clipName}' could not be located.");
      return false;
    }
    if (!AudioResourceOptimizer.AudioResourceTargetsByName.ContainsKey(clipName))
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"No Audio Sources found to add Resource file '{clipName}'.");
      return false;
    }
    List<AudioSource> audioSourceList = AudioResourceOptimizer.AudioResourceTargetsByName[clipName];
    for (int index = 0; index < audioSourceList.Count; ++index)
      audioSourceList[index].clip = clip;
    if (!AudioUtil.AudioClipWillPreload(clip))
      DarkTonic.MasterAudio.MasterAudio.LogWarning($"Audio Clip for Resource file '{clipName}' of Sound Group '{variation.ParentGroup.name}' has 'Preload Audio Data' turned off, which can cause audio glitches. Resource files should always Preload Audio Data. Please turn it on.");
    AudioResourceOptimizer.AudioClipsByName.Add(clipName, clip);
    return true;
  }

  public static void DeleteAudioSourceFromList(string clipName, AudioSource source)
  {
    if (!AudioResourceOptimizer.AudioResourceTargetsByName.ContainsKey(clipName))
    {
      DarkTonic.MasterAudio.MasterAudio.LogError($"No Audio Sources found for Resource file '{clipName}'.");
    }
    else
    {
      List<AudioSource> audioSourceList = AudioResourceOptimizer.AudioResourceTargetsByName[clipName];
      audioSourceList.Remove(source);
      if (audioSourceList.Count != 0)
        return;
      AudioResourceOptimizer.AudioResourceTargetsByName.Remove(clipName);
    }
  }

  public static void UnloadClipIfUnused(string clipName)
  {
    if (!AudioResourceOptimizer.AudioClipsByName.ContainsKey(clipName))
      return;
    List<AudioSource> audioSourceList = new List<AudioSource>();
    if (AudioResourceOptimizer.AudioResourceTargetsByName.ContainsKey(clipName))
    {
      audioSourceList = AudioResourceOptimizer.AudioResourceTargetsByName[clipName];
      for (int index = 0; index < audioSourceList.Count; ++index)
      {
        if (audioSourceList[index].GetComponent<SoundGroupVariation>().IsPlaying)
          return;
      }
    }
    AudioClip assetToUnload = AudioResourceOptimizer.AudioClipsByName[clipName];
    for (int index = 0; index < audioSourceList.Count; ++index)
      audioSourceList[index].clip = (AudioClip) null;
    AudioResourceOptimizer.AudioClipsByName.Remove(clipName);
    Resources.UnloadAsset((UnityEngine.Object) assetToUnload);
  }
}
