// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.CustomBiomeRooom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
