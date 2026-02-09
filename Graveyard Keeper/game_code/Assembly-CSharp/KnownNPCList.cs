// Decompiled with JetBrains decompiler
// Type: KnownNPCList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class KnownNPCList
{
  public List<KnownNPC> npcs = new List<KnownNPC>();

  public KnownNPC GetOrCreateNPC(string npc_id)
  {
    ObjectDefinition dataOrNull = GameBalance.me.GetDataOrNull<ObjectDefinition>(npc_id);
    if (dataOrNull != null && !string.IsNullOrEmpty(dataOrNull.npc_alias))
      return this.GetOrCreateNPC(dataOrNull.npc_alias);
    if (dataOrNull == null && npc_id != "player")
      return new KnownNPC();
    foreach (KnownNPC npc in this.npcs)
    {
      if (npc.npc_id == npc_id)
        return npc;
    }
    Debug.Log((object) ("Adding new known npc: " + npc_id));
    KnownNPC npc1 = new KnownNPC() { npc_id = npc_id };
    this.npcs.Add(npc1);
    MainGame.me.save.achievements.CheckKeyQuests("meet_" + npc_id.Replace(" ", "_"));
    return npc1;
  }

  public void Sort()
  {
    this.npcs.Sort((Comparison<KnownNPC>) ((a, b) => a.sort_order.CompareTo(b.sort_order)));
  }

  public void RemoveNPC(string npc_id)
  {
    if (string.IsNullOrEmpty(npc_id))
      return;
    KnownNPC knownNpc = this.npcs.Find((Predicate<KnownNPC>) (p => p.npc_id == npc_id));
    if (knownNpc != null)
    {
      this.npcs.Remove(knownNpc);
      Debug.Log((object) $"Removing NPC {npc_id} from Known NPC List");
    }
    else
      Debug.Log((object) $"Can't find NPC {npc_id} in Known NPC List");
  }

  public KnownNPC GetNPC(string npc_id)
  {
    foreach (KnownNPC npc in this.npcs)
    {
      if (npc.npc_id == npc_id)
        return npc;
    }
    return (KnownNPC) null;
  }
}
