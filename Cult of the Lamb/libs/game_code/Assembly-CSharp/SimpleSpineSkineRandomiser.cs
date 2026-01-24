// Decompiled with JetBrains decompiler
// Type: SimpleSpineSkineRandomiser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleSpineSkineRandomiser : BaseMonoBehaviour
{
  public List<SimpleSpineSkineRandomiser.SkinAndChance> Skins = new List<SimpleSpineSkineRandomiser.SkinAndChance>();
  public ISkeletonAnimation skeletonAnimation;

  public void Start()
  {
    this.skeletonAnimation = this.GetComponent<ISkeletonAnimation>();
    int[] weights = new int[this.Skins.Count];
    int index = -1;
    while (++index < this.Skins.Count)
      weights[index] = this.Skins[index].Probability;
    this.skeletonAnimation.Skeleton.SetSkin(this.Skins[Utils.GetRandomWeightedIndex(weights)].SkinName);
  }

  [Serializable]
  public class SkinAndChance
  {
    public string SkinName;
    [Range(0.0f, 100f)]
    public int Probability = 50;
  }
}
