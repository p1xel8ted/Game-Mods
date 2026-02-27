// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.HighContrastConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "High Contrast Configuration", menuName = "Massive Monster/High Contrast Configuration", order = 1)]
public class HighContrastConfiguration : ScriptableObject
{
  [Header("Selectable")]
  [SerializeField]
  public HighContrastConfiguration.HighContrastColorSet _colorTransitionSet;
  [Header("Text")]
  [SerializeField]
  public Color _textColor;

  public HighContrastConfiguration.HighContrastColorSet ColorTransitionSet
  {
    get => this._colorTransitionSet;
  }

  public Color TextColor => this._textColor;

  [Serializable]
  public struct HighContrastColorSet
  {
    public Color NormalColor;
    public Color HighlightColor;
    public Color PressedColor;
    public Color SelectedColor;
    public Color DisabledColor;

    public HighContrastColorSet(Selectable selectable)
    {
      this.NormalColor = selectable.colors.normalColor;
      this.HighlightColor = selectable.colors.highlightedColor;
      this.PressedColor = selectable.colors.pressedColor;
      this.SelectedColor = selectable.colors.selectedColor;
      this.DisabledColor = selectable.colors.disabledColor;
    }

    public HighContrastColorSet(TMP_Text text)
    {
      this.NormalColor = text.color;
      this.HighlightColor = text.color;
      this.PressedColor = text.color;
      this.SelectedColor = text.color;
      this.DisabledColor = text.color;
    }

    public HighContrastColorSet(SelectableColourProxy colourProxy)
    {
      this.NormalColor = colourProxy.Colors.normalColor;
      this.HighlightColor = colourProxy.Colors.highlightedColor;
      this.PressedColor = colourProxy.Colors.pressedColor;
      this.SelectedColor = colourProxy.Colors.selectedColor;
      this.DisabledColor = colourProxy.Colors.disabledColor;
    }

    public void Apply(Selectable selectable) => selectable.colors = this.GetColorBlock();

    public void Apply(SelectableColourProxy colourProxy)
    {
      colourProxy.Colors = this.GetColorBlock();
    }

    public ColorBlock GetColorBlock()
    {
      return new ColorBlock()
      {
        normalColor = this.NormalColor,
        highlightedColor = this.HighlightColor,
        pressedColor = this.PressedColor,
        selectedColor = this.SelectedColor,
        disabledColor = this.DisabledColor,
        colorMultiplier = 1f
      };
    }

    public void Apply(TMP_Text text) => text.color = this.NormalColor;
  }
}
