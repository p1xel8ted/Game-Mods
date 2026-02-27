// Decompiled with JetBrains decompiler
// Type: WormBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class WormBossIntro : BossIntro
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  protected string introAnimation;
  [SerializeField]
  private EnemyWormBoss wormBoss;

  private void Start() => this.BossSpine.AnimationState.SetAnimation(0, this.introAnimation, false);

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    WormBossIntro wormBossIntro = this;
    LetterBox.Instance.HideSkipPrompt();
    if (!LetterBox.IsPlaying)
      GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(wormBossIntro.CameraTarget, 15f);
    yield return (object) new WaitForSeconds(1.25f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationNext(wormBossIntro.CameraTarget, 20f);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 1f);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationEnd();
    wormBossIntro.wormBoss.BeginPhase1();
    if (!DataManager.Instance.BossesEncountered.Contains(FollowerLocation.Dungeon1_1))
      DataManager.Instance.BossesEncountered.Add(FollowerLocation.Dungeon1_1);
    wormBossIntro.Callback?.Invoke();
  }
}
