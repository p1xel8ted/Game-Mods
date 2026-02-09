// Decompiled with JetBrains decompiler
// Type: SkinPreset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Skin Preset")]
[Serializable]
public class SkinPreset : ScriptableObject
{
  public int head = -1;
  public int body = 300;
  public int mid;
  public int bot;
  public int backpack;

  public static SkinPreset Load(string id)
  {
    if (string.IsNullOrEmpty(id))
      return (SkinPreset) null;
    SkinPreset skinPreset = Resources.Load<SkinPreset>(id);
    if (!((UnityEngine.Object) skinPreset == (UnityEngine.Object) null))
      return skinPreset;
    Debug.LogError((object) ("Couldn't load skin preset = " + id));
    return (SkinPreset) null;
  }
}
