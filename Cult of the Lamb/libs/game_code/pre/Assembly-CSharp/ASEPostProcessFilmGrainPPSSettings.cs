// Decompiled with JetBrains decompiler
// Type: ASEPostProcessFilmGrainPPSSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

#nullable disable
[PostProcess(typeof (ASEPostProcessFilmGrainPPSRenderer), PostProcessEvent.AfterStack, "ASEPostProcessFilmGrain", true)]
[Serializable]
public sealed class ASEPostProcessFilmGrainPPSSettings : PostProcessEffectSettings
{
  [Tooltip("Strength")]
  public FloatParameter _Strength;
  [Tooltip("ColorMaskRange")]
  [Range(0.0f, 1f)]
  public FloatParameter _ColorMaskRange;
  [Tooltip("ColorMaskFuzziness")]
  [Range(0.0f, 1f)]
  public FloatParameter _ColorMaskFuzziness;
  [Tooltip("ColorToMask")]
  public ColorParameter _ColorToMask;

  public ASEPostProcessFilmGrainPPSSettings()
  {
    FloatParameter floatParameter1 = new FloatParameter();
    floatParameter1.value = 15f;
    this._Strength = floatParameter1;
    FloatParameter floatParameter2 = new FloatParameter();
    floatParameter2.value = 0.1f;
    this._ColorMaskRange = floatParameter2;
    FloatParameter floatParameter3 = new FloatParameter();
    floatParameter3.value = 0.1f;
    this._ColorMaskFuzziness = floatParameter3;
    ColorParameter colorParameter = new ColorParameter();
    colorParameter.value = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    this._ColorToMask = colorParameter;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }
}
