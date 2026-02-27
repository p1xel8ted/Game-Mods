// Decompiled with JetBrains decompiler
// Type: SimpleSpineFacePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class SimpleSpineFacePlayer : BaseMonoBehaviour
{
  private SkeletonAnimation Spine;

  private void Start() => this.Spine = this.GetComponentInChildren<SkeletonAnimation>();

  private void Update()
  {
    if (!((Object) this.Spine != (Object) null) || !((Object) PlayerFarming.Instance != (Object) null))
      return;
    this.Spine.skeleton.ScaleX = (double) PlayerFarming.Instance.transform.position.x < (double) this.transform.position.x ? 1f : -1f;
  }
}
