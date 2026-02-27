// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.ShakeAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[RequireComponent(typeof (TextMeshProUGUI))]
public class ShakeAnimation : TextAnimation
{
  [SerializeField]
  [Tooltip("The library of ShakePresets that can be used by this component.")]
  private ShakeLibrary shakeLibrary;
  [SerializeField]
  [Tooltip("The name (key) of the ShakePreset this animation should use.")]
  private string shakePresetKey;
  private ShakePreset shakePreset;

  public void LoadPreset(ShakeLibrary library, string presetKey)
  {
    this.shakeLibrary = library;
    this.shakePresetKey = presetKey;
    this.shakePreset = library[presetKey];
  }

  protected override void OnEnable()
  {
    if ((Object) this.shakeLibrary != (Object) null && !string.IsNullOrEmpty(this.shakePresetKey))
      this.LoadPreset(this.shakeLibrary, this.shakePresetKey);
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
    if (this.shakePreset == null || characterIndex < this.FirstCharToAnimate || characterIndex > this.LastCharToAnimate)
      return;
    float x = Random.Range(-this.shakePreset.xPosStrength, this.shakePreset.xPosStrength);
    float y = Random.Range(-this.shakePreset.yPosStrength, this.shakePreset.yPosStrength);
    translation = new Vector2(x, y);
    rotation = Random.Range(-this.shakePreset.RotationStrength, this.shakePreset.RotationStrength);
    scale = 1f + Random.Range(-this.shakePreset.ScaleStrength, this.shakePreset.ScaleStrength);
  }
}
