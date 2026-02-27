// Decompiled with JetBrains decompiler
// Type: EnemyHopperBurp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyHopperBurp : EnemyHopper
{
  public int ShotsToFire = 10;
  public float DetectEnemyRange = 10f;
  public GameObject projectilePrefab;
  public float LookAngle;
  [SerializeField]
  public float minTimeBetweenBurps = 6f;
  public float lastBurpedFliesTimestamp;
  public List<Projectile> activeProjectiles = new List<Projectile>();

  public override void OnEnable()
  {
    base.OnEnable();
    if (!((Object) this.gm != (Object) null))
      return;
    this.lastBurpedFliesTimestamp = this.gm.CurrentTime - Random.Range(0.0f, this.minTimeBetweenBurps);
  }

  public override void UpdateStateCharging()
  {
    this.SimpleSpineFlash.FlashMeWhite();
    if ((double) this.gm.TimeSince(this.chargingTimestamp) < (double) this.chargingDuration / (double) this.Spine[0].timeScale)
      return;
    this.BurpFlies();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public virtual void BurpFlies()
  {
    if ((Object) this.gm != (Object) null)
      this.lastBurpedFliesTimestamp = this.gm.CurrentTime;
    if (!string.IsNullOrEmpty(this.OnCroakSoundPath))
      AudioManager.Instance.PlayOneShot(this.OnCroakSoundPath, this.transform.position);
    this.SimpleSpineFlash.FlashWhite(false);
    this.StartCoroutine(this.ShootProjectileRoutine());
  }

  public virtual IEnumerator ShootProjectileRoutine()
  {
    EnemyHopperBurp enemyHopperBurp = this;
    CameraManager.shakeCamera(0.2f, enemyHopperBurp.LookAngle);
    List<float> shootAngles = new List<float>(enemyHopperBurp.ShotsToFire);
    for (int index = 0; index < enemyHopperBurp.ShotsToFire; ++index)
      shootAngles.Add(360f / (float) enemyHopperBurp.ShotsToFire * (float) index);
    shootAngles.Shuffle<float>();
    Health h = enemyHopperBurp.GetClosestTarget();
    float initAngle = Random.Range(0.0f, 360f);
    for (int i = 0; i < shootAngles.Count; ++i)
    {
      Projectile component = Object.Instantiate<GameObject>(enemyHopperBurp.projectilePrefab, enemyHopperBurp.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyHopperBurp.transform.position;
      component.Angle = initAngle + shootAngles[i];
      component.team = enemyHopperBurp.health.team;
      component.Speed += Random.Range(-0.5f, 0.5f);
      component.turningSpeed += Random.Range(-0.1f, 0.1f);
      component.angleNoiseFrequency += Random.Range(-0.1f, 0.1f);
      component.LifeTime += Random.Range(0.0f, 0.3f);
      component.Owner = enemyHopperBurp.health;
      component.SetTarget(h);
      enemyHopperBurp.activeProjectiles.Add(component);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyHopperBurp.Spine[0].timeScale) < 0.029999999329447746)
        yield return (object) null;
    }
  }

  public override bool ShouldStartCharging()
  {
    return GameManager.RoomActive && (double) this.gm.TimeSince(this.lastBurpedFliesTimestamp) >= (double) this.minTimeBetweenBurps / (double) this.Spine[0].timeScale && this.IsPlayerNearby();
  }

  public bool IsPlayerNearby()
  {
    if (!GameManager.RoomActive)
      return false;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange)
        return true;
    }
    return false;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = 0; index < this.activeProjectiles.Count; ++index)
    {
      if ((Object) this.activeProjectiles[index] != (Object) null && this.activeProjectiles[index].gameObject.activeSelf)
        this.activeProjectiles[index].DestroyWithVFX();
    }
    this.activeProjectiles.Clear();
  }
}
