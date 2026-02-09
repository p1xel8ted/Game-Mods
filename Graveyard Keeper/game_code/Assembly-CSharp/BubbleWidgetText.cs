// Decompiled with JetBrains decompiler
// Type: BubbleWidgetText
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BubbleWidgetText : BubbleWidget<BubbleWidgetTextData>
{
  [HideInInspector]
  [SerializeField]
  public UILabel _label;

  public override void Init()
  {
    this._label = this.GetComponent<UILabel>();
    base.Init();
  }

  public override void Draw(BubbleWidgetTextData data)
  {
    if (!this.initialized)
      this.Init();
    this.data = data;
    this._label.alignment = data.alignment;
    if (data.style == UITextStyles.TextStyle.None)
    {
      this._label.bitmapFont = data.font;
      this._label.fontSize = data.font.defaultSize;
      this._label.spacingY = data.line_spacing;
    }
    else
      UITextStyles.Set(this._label, data.style);
    GJL.EnsureLabelHasCorrectFont(this._label, false);
    this._label.text = data.text;
  }
}
