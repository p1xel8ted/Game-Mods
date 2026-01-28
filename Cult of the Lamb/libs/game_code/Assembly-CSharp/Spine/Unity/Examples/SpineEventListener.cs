// Decompiled with JetBrains decompiler
// Type: Spine.Unity.Examples.SpineEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Spine.Unity.Examples;

public class SpineEventListener : BaseMonoBehaviour
{
  public List<Spine.Unity.Examples.spineEventListeners> spineEventListeners = new List<Spine.Unity.Examples.spineEventListeners>();
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public bool hasSpecialBool;

  public void UpdateSpine()
  {
    if ((Object) this.spine == (Object) null)
      this.spine = this.gameObject.GetComponent<SkeletonAnimation>();
    if (!((Object) this.spine != (Object) null))
      return;
    foreach (Spine.Unity.Examples.spineEventListeners spineEventListener in this.spineEventListeners)
      spineEventListener.skeletonAnimation = this.spine;
  }

  public void Start()
  {
    foreach (Spine.Unity.Examples.spineEventListeners spineEventListener in this.spineEventListeners)
    {
      spineEventListener.hasSpecialBool = this.hasSpecialBool;
      spineEventListener.Start();
    }
  }
}
