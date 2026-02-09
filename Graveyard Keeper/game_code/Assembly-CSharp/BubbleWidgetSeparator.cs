// Decompiled with JetBrains decompiler
// Type: BubbleWidgetSeparator
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class BubbleWidgetSeparator : BubbleWidget<BubbleWidgetSeparatorData>
{
  public override void Draw(BubbleWidgetSeparatorData data)
  {
    switch (data.alignment)
    {
      case NGUIText.Alignment.Left:
        this.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Left;
        break;
      case NGUIText.Alignment.Center:
        this.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Center;
        break;
      case NGUIText.Alignment.Right:
        this.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Right;
        break;
    }
  }
}
