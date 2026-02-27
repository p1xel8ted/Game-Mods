// Decompiled with JetBrains decompiler
// Type: MMBiomeGeneration.CustomBiomeRooom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
