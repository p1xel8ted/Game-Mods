// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.CurveAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[RequireComponent(typeof (TextMeshProUGUI))]
public class CurveAnimation : TextAnimation
{
  [SerializeField]
  [Tooltip("The library of CurvePresets that can be used by this component.")]
  private CurveLibrary curveLibrary;
  [SerializeField]
  [Tooltip("The name (key) of the CurvePreset this animation should use.")]
  private string curvePresetKey;
  private CurvePreset curvePreset;
  private float timeAnimationStarted;

  public void LoadPreset(CurveLibrary library, string presetKey)
  {
    this.curveLibrary = library;
    this.curvePresetKey = presetKey;
    this.curvePreset = library[presetKey];
  }

  protected override void OnEnable()
  {
    if ((Object) this.curveLibrary != (Object) null && !string.IsNullOrEmpty(this.curvePresetKey))
      this.LoadPreset(this.curveLibrary, this.curvePresetKey);
    this.timeAnimationStarted = Time.time;
    base.OnEnable();
  }

  protected override void Animate(
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
