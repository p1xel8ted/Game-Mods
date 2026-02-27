// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.CustomBiomeRooom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace MMBiomeGeneration;

[Serializable]
public class CustomBiomeRooom
{
  public GameObject Prefab;
  public bool North;
  public bool East;
  public bool South;
  public bool West;

  public int NumDesiredConnections
  {
    get
    {
      int desiredConnections = 0;
      if (this.North)
        ++desiredConnections;
      if (this.East)
        ++desiredConnections;
      if (this.South)
        ++desiredConnections;
      if (this.West)
        ++desiredConnections;
      return desiredConnections;
    }
  }
}
