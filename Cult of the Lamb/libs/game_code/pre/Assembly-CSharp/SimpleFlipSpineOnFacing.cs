// Decompiled with JetBrains decompiler
// Type: SimpleFlipSpineOnFacing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class SimpleFlipSpineOnFacing : MonoBehaviour
{
  private StateMachine state;
  public int Dir;
  private SkeletonAnimation anim;
  public bool ReverseFacing;

  private void Start()
  {
    this.state = this.GetComponentInParent<StateMachine>();
    this.anim = this.GetComponent<SkeletonAnimation>();
  }

  private void LateUpdate()
  {
    this.Dir = ((double) this.state.facingAngle <= 90.0 || (double) this.state.facingAngle >= 270.0 ? -1 : 1) * (this.ReverseFacing ? -1 : 1);
    this.anim.skeleton.ScaleX = (float) this.Dir;
  }
}
