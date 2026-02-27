// Decompiled with JetBrains decompiler
// Type: Structures_HarvestTotem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Structures_HarvestTotem : StructureBrain
{
  public Action<int> OnSoulsGained;

  public int SoulMax => 3;

  public float TimeBetweenSouls => 1200f;

  public int SoulCount
  {
    get => this.Data.SoulCount;
    set
    {
      int soulCount = this.SoulCount;
      this.Data.SoulCount = Mathf.Clamp(value, 0, this.SoulMax);
      if (this.SoulCount <= soulCount)
        return;
      Action<int> onSoulsGained = this.OnSoulsGained;
      if (onSoulsGained == null)
        return;
      onSoulsGained(this.SoulCount - soulCount);
    }
  }
}
