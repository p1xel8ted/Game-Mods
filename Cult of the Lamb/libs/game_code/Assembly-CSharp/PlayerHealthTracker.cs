// Decompiled with JetBrains decompiler
// Type: PlayerHealthTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unify;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class PlayerHealthTracker : MonoBehaviour
{
  public Health bossToTrack;
  [SerializeField]
  public Enemy enemyType;
  public bool isOnPlayerHitAssigned;
  public bool failed;

  public void Start() => this.failed = false;

  public void OnEnable()
  {
    this.bossToTrack.OnDieCallback.AddListener(new UnityAction(this.OnDie));
    this.TryAssignOnPlayerHit();
  }

  public void OnDisable()
  {
    this.bossToTrack.OnDieCallback.RemoveListener(new UnityAction(this.OnDie));
    if ((Object) PlayerFarming.Instance != (Object) null)
      PlayerFarming.Instance.health.OnHitCallback.RemoveListener(new UnityAction(this.OnPlayerHit));
    else
      Debug.Log((object) "Player Farming Null!");
  }

  public void Update()
  {
    if (this.isOnPlayerHitAssigned)
      return;
    this.TryAssignOnPlayerHit();
  }

  public void OnPlayerHit()
  {
    if (!this.failed)
      Debug.Log((object) "Achievement failed to get no damage");
    this.failed = true;
  }

  public void TryAssignOnPlayerHit()
  {
    if ((Object) PlayerFarming.Instance != (Object) null)
    {
      PlayerFarming.Instance.health.OnHitCallback.AddListener(new UnityAction(this.OnPlayerHit));
      this.isOnPlayerHitAssigned = true;
    }
    else
    {
      Debug.Log((object) "Player Farming Null!");
      this.isOnPlayerHitAssigned = false;
    }
  }

  public void OnDie()
  {
    if (this.failed)
      return;
    Debug.Log((object) ("Achievement unlocked killed no damage boss: " + this.enemyType.ToString()));
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
