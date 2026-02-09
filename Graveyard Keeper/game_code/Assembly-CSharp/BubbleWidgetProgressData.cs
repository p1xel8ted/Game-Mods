// Decompiled with JetBrains decompiler
// Type: BubbleWidgetProgressData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class BubbleWidgetProgressData : BubbleWidgetData
{
  public BubbleWidgetProgressData.ProgressDelegate progress_delegate;
  public int offset_x = 4;
  public int offset_y = 4;

  public BubbleWidgetProgressData(
    BubbleWidgetProgressData.ProgressDelegate progress_delegate,
    int offset_x = 0,
    int offset_y = 2)
  {
    this.progress_delegate = progress_delegate;
    this.offset_x = offset_x;
    this.offset_y = offset_y;
    this.widget_id = BubbleWidgetData.WidgetID.Progress;
  }

  public delegate float ProgressDelegate();
}
