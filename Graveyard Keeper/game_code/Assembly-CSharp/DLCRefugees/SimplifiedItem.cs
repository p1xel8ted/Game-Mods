// Decompiled with JetBrains decompiler
// Type: DLCRefugees.SimplifiedItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace DLCRefugees;

[Serializable]
public class SimplifiedItem
{
  public string id;
  public int count;
  public float give_energy;

  public SimplifiedItem(string id, int count, float give_energy)
  {
    this.id = id;
    this.count = count;
    this.give_energy = give_energy;
  }
}
