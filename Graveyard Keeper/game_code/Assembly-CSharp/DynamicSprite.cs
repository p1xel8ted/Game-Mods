// Decompiled with JetBrains decompiler
// Type: DynamicSprite
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class DynamicSprite : MonoBehaviour
{
  public bool _inited;
  public SpriteRenderer[] _sprs;
  public Color[] _sprs_colors;
  public ParticleSystem[] _prtcl_systems;
  public Color[] _prtcl_sys_colors;
  public DynamicSpritePreset preset;
  public bool inside_value_set;
  public float inside_value;

  public void Update()
  {
    if (!this._inited)
    {
      this._inited = true;
      this._sprs = this.GetComponentsInChildren<SpriteRenderer>(true);
      this._sprs_colors = new Color[this._sprs.Length];
      for (int index = 0; index < this._sprs.Length; ++index)
        this._sprs_colors[index] = this._sprs[index].color;
      this._prtcl_systems = this.GetComponentsInChildren<ParticleSystem>(true);
      this._prtcl_sys_colors = new Color[this._prtcl_systems.Length];
      for (int index = 0; index < this._prtcl_systems.Length; ++index)
        this._prtcl_sys_colors[index] = this._prtcl_systems[index].main.startColor.color;
    }
    for (int index = 0; index < this._sprs.Length; ++index)
    {
      SpriteRenderer spr = this._sprs[index];
      spr.color = spr.color with
      {
        a = !this.inside_value_set || EnvironmentEngine.me.data.state != EnvironmentEngine.State.Inside ? this.preset.EvaluateAlpha() * this._sprs_colors[index].a : this.inside_value * this._sprs_colors[index].a
      };
    }
    for (int index = 0; index < this._prtcl_systems.Length; ++index)
    {
      ParticleSystem prtclSystem = this._prtcl_systems[index];
      prtclSystem.main.startColor = (ParticleSystem.MinMaxGradient) (prtclSystem.main.startColor.color with
      {
        a = (!this.inside_value_set || EnvironmentEngine.me.data.state != EnvironmentEngine.State.Inside ? this.preset.EvaluateAlpha() * this._prtcl_sys_colors[index].a : this.inside_value * this._prtcl_sys_colors[index].a)
      });
    }
  }
}
