// Decompiled with JetBrains decompiler
// Type: ToolActions
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ToolActions
{
  public List<ItemDefinition.ItemType> action_tools = new List<ItemDefinition.ItemType>();
  public List<float> action_k = new List<float>();

  public bool no_actions => this.action_tools.Count == 0;

  public bool GetToolK(ItemDefinition.ItemType item_type, out float k)
  {
    k = 0.0f;
    int index = this.action_tools.IndexOf(item_type);
    if (index == -1)
      return false;
    k = this.action_k[index];
    return true;
  }

  public bool HasToolK(ItemDefinition.ItemType item_type)
  {
    return this.action_tools.IndexOf(item_type) != -1;
  }
}
