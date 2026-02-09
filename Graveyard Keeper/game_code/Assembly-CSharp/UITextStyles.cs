// Decompiled with JetBrains decompiler
// Type: UITextStyles
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class UITextStyles : MonoBehaviour
{
  public static UITextStyles _instance;
  public UILabel[] style_labels;

  public static UITextStyles me
  {
    get
    {
      return UITextStyles._instance ?? (UITextStyles._instance = Object.FindObjectOfType<UITextStyles>());
    }
  }

  public static void Set(UILabel some_label, UITextStyles.TextStyle style)
  {
    if (style == UITextStyles.TextStyle.None)
      return;
    UILabel styleLabel = UITextStyles.me.style_labels[(int) style];
    some_label.bitmapFont = styleLabel.bitmapFont;
    some_label.fontSize = styleLabel.fontSize;
    some_label.color = styleLabel.color;
    some_label.effectStyle = styleLabel.effectStyle;
    some_label.effectColor = styleLabel.effectColor;
    some_label.spacingY = styleLabel.spacingY;
    some_label.effectDistance = styleLabel.effectDistance;
    if (styleLabel.overflowMethod == UILabel.Overflow.ResizeFreely)
      return;
    some_label.width = styleLabel.width;
  }

  public enum TextStyle
  {
    None = -1, // 0xFFFFFFFF
    Usual = 0,
    InteractionHint = 1,
    DialogText = 2,
    HintTitle = 3,
    TinyDescription = 4,
    QualityHint = 5,
  }
}
