// Decompiled with JetBrains decompiler
// Type: TentacleBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class TentacleBossIntro : BossIntro
{
  public SkeletonAnimation SpineHuman;
  public SpriteRenderer Shadow;
  public StateMachine State;

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    TentacleBossIntro tentacleBossIntro = this;
    yield return (object) new WaitForEndOfFrame();
    tentacleBossIntro.Callback?.Invoke();
  }
}
