// Decompiled with JetBrains decompiler
// Type: BubbleWidgetTextData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BubbleWidgetTextData : BubbleWidgetData
{
  public string text;
  public UIFont font;
  public NGUIText.Alignment alignment = NGUIText.Alignment.Center;
  public int line_spacing;
  public UITextStyles.TextStyle style;
  public int max_width = -1;

  public BubbleWidgetTextData(
    string text,
    BubbleWidgetTextData.Font font,
    NGUIText.Alignment alignment = NGUIText.Alignment.Center,
    int line_spacing = -4,
    int max_width = -1)
  {
    this.text = text;
    string str;
    switch (font)
    {
      case BubbleWidgetTextData.Font.Header:
        str = "header";
        break;
      case BubbleWidgetTextData.Font.MicroFont:
        str = "micro_font";
        break;
      case BubbleWidgetTextData.Font.SmallFontBold:
        str = "small_font_bold";
        break;
      case BubbleWidgetTextData.Font.TinyFont:
        str = "tiny_font";
        break;
      default:
        str = "small_font";
        break;
    }
    this.font = Resources.Load<UIFont>("ngui_fonts/" + str);
    this.alignment = alignment;
    this.line_spacing = line_spacing;
    this.style = UITextStyles.TextStyle.None;
    this.max_width = max_width;
  }

  public BubbleWidgetTextData(
    string text,
    UITextStyles.TextStyle style = UITextStyles.TextStyle.Usual,
    NGUIText.Alignment alignment = NGUIText.Alignment.Center,
    int max_width = -1)
  {
    this.text = text;
    this.style = style;
    this.alignment = alignment;
    this.max_width = max_width;
  }

  public override bool IsEmpty() => string.IsNullOrEmpty(this.text);

  public override void TrySetAlign(NGUIText.Alignment alignment) => this.alignment = alignment;

  public enum Font
  {
    Header,
    MicroFont,
    SmallFont,
    SmallFontBold,
    TinyFont,
  }
}
