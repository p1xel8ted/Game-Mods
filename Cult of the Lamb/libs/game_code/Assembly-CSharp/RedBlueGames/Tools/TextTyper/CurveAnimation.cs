// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.CurveAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[RequireComponent(typeof (TextMeshProUGUI))]
public class CurveAnimation : TextAnimation
{
  [SerializeField]
  [Tooltip("The library of CurvePresets that can be used by this component.")]
  public CurveLibrary curveLibrary;
  [SerializeField]
  [Tooltip("The name (key) of the CurvePreset this animation should use.")]
  public string curvePresetKey;
  public CurvePreset curvePreset;
  public float timeAnimationStarted;

  public void LoadPreset(CurveLibrary library, string presetKey)
  {
    this.curveLibrary = library;
    this.curvePresetKey = presetKey;
    this.curvePreset = library[presetKey];
  }

  public override void OnEnable()
  {
    if ((Object) this.curveLibrary != (Object) null && !string.IsNullOrEmpty(this.curvePresetKey))
      this.LoadPreset(this.curveLibrary, this.curvePresetKey);
    this.timeAnimationStarted = Time.time;
    base.OnEnable();
  }

  public override void Animate(
    int characterIndex,
    out Vector2 translation,
    out float rotation,
    out float scale)
  {
    translation = Vector2.zero;
    rotation = 0.0f;
    scale = 1f;
    if (this.curvePreset == null || characterIndex < this.FirstCharToAnimate || characterIndex > this.LastCharToAnimate)
      return;
    float time = (float) ((double) Time.time - (double) this.timeAnimationStarted + (double) characterIndex * (double) this.curvePreset.timeOffsetPerChar);
    float x = this.curvePreset.xPosCurve.Evaluate(time) * this.curvePreset.xPosMultiplier;
    float y = this.curvePreset.yPosCurve.Evaluate(time) * this.curvePreset.yPosMultiplier;
    translation = new Vector2(x, y);
    rotation = this.curvePreset.rotationCurve.Evaluate(time) * this.curvePreset.rotationMultiplier;
    scale += this.curvePreset.scaleCurve.Evaluate(time) * this.curvePreset.scaleMultiplier;
  }
}
