// Decompiled with JetBrains decompiler
// Type: LUTController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LUTController : MonoBehaviour
{
  public const int COLOR_EFFECTS_ON_INIT = 4;
  public List<AmplifyColorEffect> color_effects;
  public GameObject main_camera;

  public void InitLUTController()
  {
    if ((Object) this.main_camera == (Object) null)
      this.main_camera = this.gameObject;
    this.color_effects = new List<AmplifyColorEffect>();
    for (int index = 0; index < 4; ++index)
    {
      AmplifyColorEffect amplifyColorEffect = this.main_camera.AddComponent<AmplifyColorEffect>();
      amplifyColorEffect.enabled = false;
      this.color_effects.Add(amplifyColorEffect);
    }
  }

  public void UpdateColorEffects(List<LUTAtom> effects)
  {
    if (this.color_effects == null || this.color_effects.Count == 0)
      this.InitLUTController();
    while (effects.Count > this.color_effects.Count)
      this.AddColorEffect();
    for (int index = 0; index < this.color_effects.Count; ++index)
    {
      if (index >= effects.Count)
      {
        this.color_effects[index].enabled = false;
      }
      else
      {
        this.color_effects[index].enabled = true;
        if ((Object) this.color_effects[index].LutBlendTexture != (Object) effects[index].lut_texture)
          this.color_effects[index].LutBlendTexture = (Texture) effects[index].lut_texture;
        this.color_effects[index].BlendAmount = effects[index].value;
      }
    }
  }

  public void AddColorEffect()
  {
    if ((Object) this.main_camera == (Object) null)
    {
      Debug.LogError((object) "main_camera is null!");
    }
    else
    {
      AmplifyColorEffect amplifyColorEffect = this.main_camera.AddComponent<AmplifyColorEffect>();
      amplifyColorEffect.enabled = false;
      this.color_effects.Add(amplifyColorEffect);
    }
  }
}
