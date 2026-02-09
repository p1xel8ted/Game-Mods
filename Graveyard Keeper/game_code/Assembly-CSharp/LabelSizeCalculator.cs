// Decompiled with JetBrains decompiler
// Type: LabelSizeCalculator
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LabelSizeCalculator : MonoBehaviour
{
  public static Dictionary<UIFont, LabelSizeCalculator> _calculators = new Dictionary<UIFont, LabelSizeCalculator>();
  [HideInInspector]
  public UILabel label;
  public UILabel _last_proceed_label;

  public void Start()
  {
    this.label = this.GetComponent<UILabel>();
    UIFont bitmapFont = this.label.bitmapFont;
    if ((Object) bitmapFont == (Object) null)
      Debug.LogError((object) "No font", (Object) this);
    else if (!LabelSizeCalculator._calculators.ContainsKey(bitmapFont))
      LabelSizeCalculator._calculators.Add(bitmapFont, this);
    else
      Debug.LogError((object) $"Label size calculator for {bitmapFont?.ToString()} already exists", (Object) this);
  }

  public Vector2 Proceed(UILabel target_label, string text)
  {
    if ((Object) this._last_proceed_label != (Object) target_label)
    {
      this._last_proceed_label = target_label;
      this.label.width = target_label.width;
      this.label.height = target_label.height;
      this.label.fontSize = target_label.fontSize;
      this.label.alignment = target_label.alignment;
      this.label.overflowMethod = target_label.overflowMethod;
      this.label.overflowWidth = target_label.overflowWidth;
      this.label.useFloatSpacing = target_label.useFloatSpacing;
      this.label.spacingX = target_label.spacingX;
      this.label.spacingY = target_label.spacingY;
    }
    GJL.EnsureLabelHasCorrectFont(this.label);
    this.label.text = text;
    this.label.ProcessText();
    return this.label.localSize;
  }

  public static Vector2 Calc(UILabel target_label, string text)
  {
    if (LabelSizeCalculator._calculators.ContainsKey(target_label.bitmapFont))
      return LabelSizeCalculator._calculators[target_label.bitmapFont].Proceed(target_label, text);
    Debug.LogError((object) ("No label size calculator for " + target_label.bitmapFont?.ToString()), (Object) target_label);
    return Vector2.zero;
  }

  public static void ApplyLanguageChange()
  {
    foreach (KeyValuePair<UIFont, LabelSizeCalculator> calculator in LabelSizeCalculator._calculators)
      GJL.EnsureLabelHasCorrectFont(calculator.Value.label);
  }
}
