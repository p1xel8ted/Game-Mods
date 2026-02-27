// Decompiled with JetBrains decompiler
// Type: GroupProbability
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class GroupProbability
{
  public GameObject GroupObject;
  [Range(1f, 100f)]
  public int Probability = 50;

  public GroupProbability(GameObject g) => this.GroupObject = g;
}
