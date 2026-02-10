// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.ShakeAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[RequireComponent(typeof (TextMeshProUGUI))]
public class ShakeAnimation : TextAnimation
{
  [SerializeField]
  [Tooltip("The library of ShakePresets that can be used by this component.")]
  public ShakeLibrary shakeLibrary;
  [SerializeField]
  [Tooltip("The name (key) of the ShakePreset this animation should use.")]
  public string shakePresetKey;
  public ShakePreset shakePreset;

  public void LoadPreset(ShakeLibrary library, string presetKey)
  {
    this.shakeLibrary = library;
    this.shakePresetKey = presetKey;
    this.shakePreset = library[presetKey];
  }

  public override void OnEnable()
  {
    if ((Object) this.shakeLibrary != (Object) null && !string.IsNullOrEmpty(this.shakePresetKey))
      this.LoadPreset(this.shakeLibrary, this.shakePresetKey);
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
    if (this.shakePreset == null || characterIndex < this.FirstCharToAnimate || characterIndex > this.LastCharToAnimate)
      return;
    float x = Random.Range(-this.shakePreset.xPosStrength, this.shakePreset.xPosStrength);
    float y = Random.Range(-this.shakePreset.yPosStrength, this.shakePreset.yPosStrength);
    translation = new Vector2(x, y);
    rotation = Random.Range(-this.shakePreset.RotationStrength, this.shakePreset.RotationStrength);
    scale = 1f + Random.Range(-this.shakePreset.ScaleStrength, this.shakePreset.ScaleStrength);
  }
}
