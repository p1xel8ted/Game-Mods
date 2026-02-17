// Decompiled with JetBrains decompiler
// Type: WormBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class WormBossIntro : BossIntro
{
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "BossSpine")]
  public string introAnimation;
  [SerializeField]
  public EnemyWormBoss wormBoss;

  public new void Start()
  {
  }

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    WormBossIntro wormBossIntro = this;
    wormBossIntro.BossSpine.AnimationState.SetAnimation(0, wormBossIntro.introAnimation, false);
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
