// Decompiled with JetBrains decompiler
// Type: SerializedTechPointDrop
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class SerializedTechPointDrop
{
  public Vector2 pos;
  public TechPointsSpawner.Type type;

  public void FromTechPointDrop(TechPointDrop drop)
  {
    if ((UnityEngine.Object) drop == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "TechPointDrop is null!");
    }
    else
    {
      this.pos = (Vector2) drop.transform.position;
      int num = TechDefinition.TECH_POINTS.IndexOf(drop.type);
      if (num < 0 || num > TechDefinition.TECH_POINTS.Count - 1)
        Debug.LogError((object) ("Wrong DropTechPoint type: " + drop.type));
      else
        this.type = (TechPointsSpawner.Type) num;
    }
  }
}
