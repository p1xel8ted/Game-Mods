// Decompiled with JetBrains decompiler
// Type: FrogBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FrogBossIntro : BossIntro
{
  [SerializeField]
  public EnemyFrogBoss frogBoss;

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    FrogBossIntro frogBossIntro = this;
    frogBossIntro.BossSpine.AnimationState.SetAnimation(0, "transform", false);
    LetterBox.Instance.HideSkipPrompt();
    GameManager.GetInstance().OnConversationNext(frogBossIntro.CameraTarget, 12f);
    yield return (object) new WaitForSeconds(2.1f);
    GameManager.GetInstance().OnConversationNext(frogBossIntro.CameraTarget, 16f);
    CameraManager.instance.ShakeCameraForDuration(2f, 2.5f, 1f);
    frogBossIntro.BossSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationEnd();
    frogBossIntro.frogBoss.BeginPhase1();
    if (!DataManager.Instance.BossesEncountered.Contains(FollowerLocation.Dungeon1_2))
      DataManager.Instance.BossesEncountered.Add(FollowerLocation.Dungeon1_2);
    frogBossIntro.Callback?.Invoke();
  }
}
