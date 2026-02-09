// Decompiled with JetBrains decompiler
// Type: CraftInterface
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public interface CraftInterface
{
  bool CanCraft(
    CraftDefinition craft,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null);

  bool OnCraft(
    CraftDefinition craft,
    Item try_use_particular_item = null,
    List<string> multiquality_ids = null,
    int amount = 1,
    List<Item> override_needs = null,
    WorldGameObject other_obj_override = null);

  void OnRightClick();
}
