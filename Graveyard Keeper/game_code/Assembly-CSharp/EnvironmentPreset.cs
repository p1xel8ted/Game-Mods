// Decompiled with JetBrains decompiler
// Type: EnvironmentPreset
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Environment Preset")]
[Serializable]
public class EnvironmentPreset : ScriptableObject
{
  public Texture lut;
  public float lut_morph_speed = 4f;
  public bool disable_timeofday_lut = true;
  public string music_id = "";
  [Space(5f)]
  public bool light_override;
  public Gradient light_grd = new Gradient();
  [Space(5f)]
  public bool ambient_light_override;
  public Gradient ambient_grd = new Gradient();
  [Space(5f)]
  public bool light_sprites_override;
  public Gradient light_sprites = new Gradient();
  [Space(5f)]
  public bool force_static_time;
  public float static_time_value;
  [Space(5f)]
  public bool force_shadows_alpha;
  public float shadows_alpha = 1f;
  [Space(5f)]
  public bool force_light_intensity;
  public float light_intensity = 1f;
  [Space(5f)]
  public bool force_global_shadows_alpha;
  public float global_shadows_alpha = 1f;

  public static EnvironmentPreset Load(string id)
  {
    if (string.IsNullOrEmpty(id))
      return (EnvironmentPreset) null;
    EnvironmentPreset environmentPreset = Resources.Load<EnvironmentPreset>("Environment presets/" + id);
    if (!((UnityEngine.Object) environmentPreset == (UnityEngine.Object) null))
      return environmentPreset;
    Debug.LogError((object) ("Couldn't load env.preset = " + id));
    return (EnvironmentPreset) null;
  }
}
