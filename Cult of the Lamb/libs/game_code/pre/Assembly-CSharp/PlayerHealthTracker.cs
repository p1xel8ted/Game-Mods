// Decompiled with JetBrains decompiler
// Type: PlayerHealthTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Unify;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class PlayerHealthTracker : MonoBehaviour
{
  public Health bossToTrack;
  [SerializeField]
  private Enemy enemyType;
  private bool failed;

  private void Start() => this.failed = false;

  private void OnEnable()
  {
    this.bossToTrack.OnDieCallback.AddListener(new UnityAction(this.OnDie));
    if ((Object) PlayerFarming.Instance != (Object) null)
      PlayerFarming.Instance.health.OnHitCallback.AddListener(new UnityAction(this.OnPlayerHit));
    else
      Debug.Log((object) "Player Farming Null!");
  }

  private void OnDisable()
  {
    this.bossToTrack.OnDieCallback.RemoveListener(new UnityAction(this.OnDie));
    if ((Object) PlayerFarming.Instance != (Object) null)
      PlayerFarming.Instance.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
    else
      Debug.Log((object) "Player Farming Null!");
  }

  private void OnPlayerHit()
  {
    if (!this.failed)
      Debug.Log((object) "Achievement failed to get no damage");
    this.failed = true;
  }

  private void OnDie()
  {
    if (this.failed)
      return;
    Debug.Log((object) ("Achievement unlocked killed no damage boss: " + (object) this.enemyType));
    switch (this.enemyType)
    {
      case Enemy.WormBoss:
        AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_BOSS_1_NODAMAGE"));
        break;
      case Enemy.FrogBoss:
        AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_BOSS_2_NODAMAGE"));
        break;
      case Enemy.JellyBoss:
        AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_BOSS_3_NODAMAGE"));
        break;
      case Enemy.SpiderBoss:
        AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("KILL_BOSS_4_NODAMAGE"));
        break;
    }
  }
}
