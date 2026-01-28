// Decompiled with JetBrains decompiler
// Type: SimpleFlipSpineOnFacing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class SimpleFlipSpineOnFacing : MonoBehaviour
{
  public StateMachine state;
  public int Dir;
  public SkeletonAnimation anim;
  public bool ReverseFacing;

  public void Start()
  {
    this.state = this.GetComponentInParent<StateMachine>();
    this.anim = this.GetComponent<SkeletonAnimation>();
  }

  public void LateUpdate()
  {
    this.Dir = ((double) this.state.facingAngle <= 90.0 || (double) this.state.facingAngle >= 270.0 ? -1 : 1) * (this.ReverseFacing ? -1 : 1);
    this.anim.skeleton.ScaleX = (float) this.Dir;
  }
}
