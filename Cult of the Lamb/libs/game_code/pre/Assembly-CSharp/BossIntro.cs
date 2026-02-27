// Decompiled with JetBrains decompiler
// Type: BossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private void Start()
  {
    if (!((Object) this.BossSpine != (Object) null))
      return;
    this.BossSpine.UpdateInterval = 1;
  }

  public virtual IEnumerator PlayRoutine(bool skipped = false)
  {
    yield return (object) null;
  }
}
