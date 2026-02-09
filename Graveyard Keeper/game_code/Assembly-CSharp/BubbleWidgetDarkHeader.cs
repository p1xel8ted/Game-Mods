// Decompiled with JetBrains decompiler
// Type: BubbleWidgetDarkHeader
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class BubbleWidgetDarkHeader : BubbleWidget<BubbleWidgetDarkHeaderData>
{
  public UILabel hdr_label;

  public override void Init() => base.Init();

  public override void Draw(BubbleWidgetDarkHeaderData data)
  {
    if (!this.initialized)
      this.Init();
    this.data = data;
    this.hdr_label.text = data.text;
  }
}
