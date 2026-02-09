// Decompiled with JetBrains decompiler
// Type: ExecutionerBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class ExecutionerBossIntro : BossIntro
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string introAnimation;
  [SerializeField]
  public EnemyBruteBoss executionerBoss;
  public string bossMusic = "event:/music/the_rot/the_rot";

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    ExecutionerBossIntro executionerBossIntro = this;
    yield return (object) null;
    executionerBossIntro.Callback?.Invoke();
  }
}
