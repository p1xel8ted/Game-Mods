// Decompiled with JetBrains decompiler
// Type: BubbleWidgetProgress
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class BubbleWidgetProgress : BubbleWidget<BubbleWidgetProgressData>
{
  public UI2DSprite progress_bar;
  public UI2DSprite back;

  public override void Draw(BubbleWidgetProgressData data)
  {
    if (!this.initialized)
      this.Init();
    this.data = data;
    this.ui_widget.width = this.progress_bar.width + data.offset_x * 2;
    this.ui_widget.height = this.progress_bar.height + data.offset_y * 2;
    this.UpdateWidget();
  }

  public override void UpdateWidget()
  {
    float num = this.data.progress_delegate();
    this.progress_bar.fillAmount = num;
    if ((double) num < 0.0)
    {
      this.ui_widget.alpha = 0.0f;
    }
    else
    {
      if (!this.ui_widget.alpha.EqualsTo(0.0f))
        return;
      this.ui_widget.alpha = 1f;
    }
  }
}
