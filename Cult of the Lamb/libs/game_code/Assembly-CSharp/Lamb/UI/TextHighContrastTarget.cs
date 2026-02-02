// Decompiled with JetBrains decompiler
// Type: Lamb.UI.TextHighContrastTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class TextHighContrastTarget : HighContrastTarget
{
  public TMP_Text _text;
  public Color _cachedColor;

  public TextHighContrastTarget(TMP_Text text, HighContrastConfiguration configuration)
    : base(configuration)
  {
    this._text = text;
  }

  public override void Apply(bool state)
  {
    if (state)
      this._text.color = this._configuration.TextColor;
    else
      this._text.color = this._cachedColor;
  }

  public override void Init() => this._cachedColor = this._text.color;
}
