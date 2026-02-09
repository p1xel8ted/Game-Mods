// Decompiled with JetBrains decompiler
// Type: AudioLoaderOptimizer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class AudioLoaderOptimizer
{
  public static Dictionary<string, List<GameObject>> PlayingGameObjectsByClipName = new Dictionary<string, List<GameObject>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

  public static void AddNonPreloadedPlayingClip(AudioClip clip, GameObject maHolderGameObject)
  {
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
      return;
    string name = clip.name;
    if (!AudioLoaderOptimizer.PlayingGameObjectsByClipName.ContainsKey(name))
    {
      AudioLoaderOptimizer.PlayingGameObjectsByClipName.Add(name, new List<GameObject>()
      {
        maHolderGameObject
      });
    }
    else
    {
      List<GameObject> gameObjectList = AudioLoaderOptimizer.PlayingGameObjectsByClipName[name];
      if (gameObjectList.Contains(maHolderGameObject))
        return;
      gameObjectList.Add(maHolderGameObject);
    }
  }

  public static void RemoveNonPreloadedPlayingClip(AudioClip clip, GameObject maHolderGameObject)
  {
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
      return;
    string name = clip.name;
    if (!AudioLoaderOptimizer.PlayingGameObjectsByClipName.ContainsKey(name))
      return;
    List<GameObject> gameObjectList = AudioLoaderOptimizer.PlayingGameObjectsByClipName[name];
    gameObjectList.Remove(maHolderGameObject);
    if (gameObjectList.Count != 0)
      return;
    AudioLoaderOptimizer.PlayingGameObjectsByClipName.Remove(name);
  }

  public static bool IsAnyOfNonPreloadedClipPlaying(AudioClip clip)
  {
    return !((UnityEngine.Object) clip == (UnityEngine.Object) null) && AudioLoaderOptimizer.PlayingGameObjectsByClipName.ContainsKey(clip.name);
  }
}
