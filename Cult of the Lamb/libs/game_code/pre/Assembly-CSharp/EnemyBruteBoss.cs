// Decompiled with JetBrains decompiler
// Type: EnemyBruteBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyBruteBoss : EnemyBrute
{
  public SkeletonAnimation Spine;
  [SerializeField]
  private SimpleSpineFlash simpleSpineFlash;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  private string spawnAnimation;
  [SerializeField]
  private float hammerAttackAnticipation;
  [SerializeField]
  private float hammerAttackCooldown;
  [SerializeField]
  private int maxEnemies = 15;
  [SerializeField]
  private EnemyBruteBoss.SpawnSet[] spawnSets = new EnemyBruteBoss.SpawnSet[0];
  [SerializeField]
  private float spawnAnticipation;
  [SerializeField]
  private float spawnCooldown;
  [Space]
  [SerializeField]
  private int attacksUntilSpawn = 4;
  [SerializeField]
  private Vector2 timeBetweenAttacks;
  [SerializeField]
  private Vector2 timeBetweenSpawns;
  private bool attacking;
  private float timestamp;
  private int counter;
  private float checkPlayerTimestamp;
  private float checkPlayerInterval = 0.3f;

  public override void OnEnable()
  {
    base.OnEnable();
    this.simpleSpineFlash.FlashWhite(false);
    this.TargetWarning.gameObject.SetActive(false);
    this.attacking = false;
    this.counter = 0;
    if (!(bool) (UnityEngine.Object) PlayerFarming.Instance)
      return;
    this.givePath(PlayerFarming.Instance.transform.position);
  }

  public override void Update()
  {
    base.Update();
    if ((bool) (UnityEngine.Object) PlayerFarming.Instance)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float timestamp = this.timestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) timestamp & currentTime.HasValue && !this.attacking)
      {
        if (this.counter == this.attacksUntilSpawn - 1 && Health.team2.Count - 1 < this.maxEnemies)
          this.Spawn();
        else
          this.HammerAttack();
        ++this.counter;
        if (this.counter > this.attacksUntilSpawn)
          this.counter = 0;
      }
    }
    if (this.attacking)
      return;
    this.UpdateMoving();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health)
      {
        Health.team2[index].enabled = true;
        Health.team2[index].invincible = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position);
      }
    }
  }

  private void UpdateMoving()
  {
    if (!(bool) (UnityEngine.Object) PlayerFarming.Instance)
      return;
    float? currentTime = GameManager.GetInstance()?.CurrentTime;
    float checkPlayerTimestamp = this.checkPlayerTimestamp;
    if (!((double) currentTime.GetValueOrDefault() > (double) checkPlayerTimestamp & currentTime.HasValue) || !GameManager.RoomActive)
      return;
    this.checkPlayerTimestamp = GameManager.GetInstance().CurrentTime + this.checkPlayerInterval;
    this.givePath(PlayerFarming.Instance.transform.position);
    this.LookAtAngle(Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position));
  }

  private void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  private void HammerAttack() => this.StartCoroutine((IEnumerator) this.HammerAttackIE());

  private IEnumerator HammerAttackIE()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.TargetWarning.gameObject.SetActive(true);
    enemyBruteBoss.simpleSpineAnimator.Animate("attack-charge", 0, false);
    enemyBruteBoss.ClearPaths();
    float angle = Utils.GetAngle(enemyBruteBoss.transform.position, PlayerFarming.Instance.transform.position);
    enemyBruteBoss.LookAtAngle(angle);
    float t = 0.0f;
    while ((double) t < (double) enemyBruteBoss.hammerAttackAnticipation)
    {
      float amt = t / enemyBruteBoss.hammerAttackAnticipation;
      enemyBruteBoss.simpleSpineFlash.FlashWhite(amt);
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyBruteBoss.simpleSpineFlash.FlashWhite(false);
    enemyBruteBoss.simpleSpineAnimator.Animate("attack-impact", 0, false);
    enemyBruteBoss.simpleSpineAnimator.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(0.2f, 0.5f, 0.3f);
    enemyBruteBoss.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    if (!string.IsNullOrEmpty(enemyBruteBoss.areaAttackSoundPath))
      AudioManager.Instance.PlayOneShot(enemyBruteBoss.areaAttackSoundPath, enemyBruteBoss.transform.position);
    GameManager.GetInstance().HitStop();
    enemyBruteBoss.ParticleSystem.Play();
    enemyBruteBoss.TargetWarning.gameObject.SetActive(false);
    enemyBruteBoss.damageColliderEvents.SetActive(true);
    yield return (object) new WaitForSeconds(0.1f);
    enemyBruteBoss.damageColliderEvents.SetActive(false);
    yield return (object) new WaitForSeconds(enemyBruteBoss.hammerAttackCooldown);
    enemyBruteBoss.attacking = false;
    enemyBruteBoss.timestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyBruteBoss.timeBetweenAttacks.x, enemyBruteBoss.timeBetweenAttacks.y);
    enemyBruteBoss.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
  }

  private void Spawn() => this.StartCoroutine((IEnumerator) this.SpawnIE());

  private IEnumerator SpawnIE()
  {
    EnemyBruteBoss enemyBruteBoss = this;
    enemyBruteBoss.attacking = true;
    enemyBruteBoss.simpleSpineAnimator.Animate(enemyBruteBoss.spawnAnimation, 0, false);
    enemyBruteBoss.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    enemyBruteBoss.ClearPaths();
    yield return (object) new WaitForSeconds(enemyBruteBoss.spawnAnticipation);
    enemyBruteBoss.SpawnEnemies(enemyBruteBoss.spawnSets[UnityEngine.Random.Range(0, enemyBruteBoss.spawnSets.Length)]);
    yield return (object) new WaitForSeconds(enemyBruteBoss.spawnCooldown);
    enemyBruteBoss.attacking = false;
    enemyBruteBoss.timestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyBruteBoss.timeBetweenSpawns.x, enemyBruteBoss.timeBetweenSpawns.y);
  }

  private void SpawnEnemies(EnemyBruteBoss.SpawnSet set)
  {
    for (int index = 0; index < set.Spawnables.Length; ++index)
      EnemySpawner.Create(set.Positions[index], this.transform.parent, set.Spawnables[index].gameObject);
  }

  public override IEnumerator ChasePlayer()
  {
    yield break;
  }

  private void OnDrawGizmos()
  {
    foreach (EnemyBruteBoss.SpawnSet spawnSet in this.spawnSets)
    {
      foreach (Vector3 position in spawnSet.Positions)
        Utils.DrawCircleXY(position, 0.5f, Color.green);
    }
  }

  [Serializable]
  private class SpawnSet
  {
    public GameObject[] Spawnables;
    public Vector3[] Positions;
  }
}
