// Decompiled with JetBrains decompiler
// Type: BossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class BossIntro : BaseMonoBehaviour
{
  public SkeletonAnimation BossSpine;
  public UnityEvent Callback;
  public GameObject CameraTarget;

  public void Start()
  {
    if (!((Object) this.BossSpine != (Object) null))
      return;
    SkeletonAnimationLODManager component = this.BossSpine.GetComponent<SkeletonAnimationLODManager>();
    if ((Object) component != (Object) null)
      component.IgnoreCulling = true;
    else if ((Object) SkeletonAnimationLODGlobalManager.Instance != (Object) null)
      SkeletonAnimationLODGlobalManager.Instance.DisableCulling(this.BossSpine.transform, this.BossSpine);
    this.BossSpine.UpdateInterval = 1;
  }

  public virtual IEnumerator PlayRoutine(bool skipped = false)
  {
    yield return (object) null;
  }
}
