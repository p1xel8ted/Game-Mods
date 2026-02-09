// Decompiled with JetBrains decompiler
// Type: BubbleWidgetData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public abstract class BubbleWidgetData
{
  public BubbleWidgetData.WidgetID widget_id;

  public virtual bool IsEmpty() => false;

  public virtual void TrySetAlign(NGUIText.Alignment alignment)
  {
  }

  public enum WidgetID
  {
    None,
    HPProgress,
    CraftingItem,
    CraftingProgress,
    Fishing,
    Interaction,
    Work,
    Progress,
    Quality,
  }
}
