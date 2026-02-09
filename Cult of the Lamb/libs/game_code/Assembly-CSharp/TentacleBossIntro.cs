// Decompiled with JetBrains decompiler
// Type: TentacleBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
