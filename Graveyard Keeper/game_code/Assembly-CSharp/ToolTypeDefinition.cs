// Decompiled with JetBrains decompiler
// Type: ToolTypeDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class ToolTypeDefinition : BalanceBaseObject
{
  public ToolTypeDefinition.Flag flag;

  public bool driven_by_anim_event => this.flag == ToolTypeDefinition.Flag.None;

  public static ToolTypeDefinition Get(ItemDefinition.ItemType item_type)
  {
    string str = ((int) item_type).ToString();
    foreach (ToolTypeDefinition toolTypeDefinition in GameBalance.me.tools_data)
    {
      if (toolTypeDefinition.id == str)
        return toolTypeDefinition;
    }
    return (ToolTypeDefinition) null;
  }

  [Flags]
  public enum Flag
  {
    None = 0,
    Collider = 1,
    Work = 2,
  }
}
