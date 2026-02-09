// Decompiled with JetBrains decompiler
// Type: ObjectGroupDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ObjectGroupDefinition : BalanceBaseObject
{
  public List<string> aura_emitters = new List<string>();
  public List<string> aura_receivers = new List<string>();
  public string tech_icon = "";
  [NonSerialized]
  public List<ObjectDefinition> objects = new List<ObjectDefinition>();

  public static void LinkObjectsToGroups()
  {
    foreach (ObjectGroupDefinition objectGroup in GameBalance.me.object_groups)
    {
      objectGroup.objects = new List<ObjectDefinition>();
      foreach (ObjectDefinition objectDefinition in GameBalance.me.objs_data)
      {
        if (objectDefinition.DoesBelongToGroup(objectGroup.id))
          objectGroup.objects.Add(objectDefinition);
      }
    }
  }
}
