// Decompiled with JetBrains decompiler
// Type: SimpleSpineFacePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class SimpleSpineFacePlayer : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;

  public void Start() => this.Spine = this.GetComponentInChildren<SkeletonAnimation>();

  public void Update()
  {
    if (!((Object) this.Spine != (Object) null) || !((Object) PlayerFarming.Instance != (Object) null))
      return;
    this.Spine.skeleton.ScaleX = (double) PlayerFarming.Instance.transform.position.x < (double) this.transform.position.x ? 1f : -1f;
  }
}
