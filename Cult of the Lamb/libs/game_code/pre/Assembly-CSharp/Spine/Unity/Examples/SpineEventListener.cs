// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.SpineEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Spine.Unity.Examples;

public class SpineEventListener : BaseMonoBehaviour
{
  public List<Spine.Unity.Examples.spineEventListeners> spineEventListeners = new List<Spine.Unity.Examples.spineEventListeners>();
  [SerializeField]
  private SkeletonAnimation spine;

  public void UpdateSpine()
  {
    if ((Object) this.spine == (Object) null)
      this.spine = this.gameObject.GetComponent<SkeletonAnimation>();
    if (!((Object) this.spine != (Object) null))
      return;
    foreach (Spine.Unity.Examples.spineEventListeners spineEventListener in this.spineEventListeners)
      spineEventListener.skeletonAnimation = this.spine;
  }

  private void Start()
  {
    foreach (Spine.Unity.Examples.spineEventListeners spineEventListener in this.spineEventListeners)
      spineEventListener.Start();
  }
}
