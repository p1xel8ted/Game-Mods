// Decompiled with JetBrains decompiler
// Type: FrogBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FrogBossIntro : BossIntro
{
  [SerializeField]
  private EnemyFrogBoss frogBoss;

  public override IEnumerator PlayRoutine(bool skipped = false)
  {
    FrogBossIntro frogBossIntro = this;
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
