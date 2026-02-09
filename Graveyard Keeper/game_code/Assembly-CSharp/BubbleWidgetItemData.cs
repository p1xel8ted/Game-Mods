// Decompiled with JetBrains decompiler
// Type: BubbleWidgetItemData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class BubbleWidgetItemData : BubbleWidgetData
{
  public string item_id;
  public string icon_id;
  public bool show_back;
  public bool show_quality;
  public int counter = 1;
  public bool cap_limit;
  public bool is_gratitude;
  public bool is_enough_gratitude;
  public bool infinity_counter;

  public BubbleWidgetItemData()
  {
  }

  public BubbleWidgetItemData(
    string item_id,
    bool show_back = true,
    bool show_quality = true,
    int counter = 1,
    bool infinity_counter = false)
  {
    this.item_id = item_id;
    this.show_back = show_back;
    this.show_quality = show_quality;
    this.counter = counter;
    this.infinity_counter = infinity_counter;
  }

  public override bool IsEmpty()
  {
    return string.IsNullOrEmpty(this.item_id) && string.IsNullOrEmpty(this.icon_id);
  }
}
