// Decompiled with JetBrains decompiler
// Type: GroupProbability
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
