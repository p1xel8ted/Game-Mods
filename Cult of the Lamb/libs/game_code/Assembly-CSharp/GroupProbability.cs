// Decompiled with JetBrains decompiler
// Type: GroupProbability
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
