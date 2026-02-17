// Decompiled with JetBrains decompiler
// Type: EnemyScout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyScout : EnemySwordsman
{
  public List<Vector3> TeleportPositions;
  public Vector3 Direction;
  public RaycastHit2D Results;
  public float SpawnDelay = 0.2f;
  public int SummonedCount;
  public List<GameObject> EnemiesToSpawn = new List<GameObject>();
  public float Distance = 2f;
  public float SpawnCircleCastRadius = 1f;

  public void SpawnMoreEnemies()
  {
    if (this.GetAvailableSpawnPositions().Count <= 0)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.SpawnEnemiesRoutine());
  }

  public List<Vector3> GetAvailableSpawnPositions()
  {
    this.TeleportPositions = new List<Vector3>();
    int num = -3;
    while ((num += 2) <= 1)
    {
      this.Angle = (float) (((double) this.state.LookAngle + (double) (45 * num)) * (Math.PI / 180.0));
      this.Direction = new Vector3(Mathf.Cos(this.Angle), Mathf.Sin(this.Angle));
      this.Results = Physics2D.CircleCast((Vector2) this.transform.position, this.SpawnCircleCastRadius, (Vector2) this.Direction, this.Distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) this.Results.collider == (UnityEngine.Object) null)
        this.TeleportPositions.Add(this.transform.position + this.Direction * this.Distance);
    }
    if (this.TeleportPositions.Count <= 0)
    {
      this.Direction = new Vector3(Mathf.Cos(this.state.LookAngle), Mathf.Sin(this.state.LookAngle));
      this.Results = Physics2D.CircleCast((Vector2) this.transform.position, this.SpawnCircleCastRadius, (Vector2) this.Direction, this.Distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) this.Results.collider == (UnityEngine.Object) null)
        this.TeleportPositions.Add(this.transform.position + this.Direction * this.Distance);
    }
    return this.TeleportPositions;
  }

  public override bool CustomAttackLogic()
  {
    if ((double) (this.SpawnDelay += Time.deltaTime) <= 0.20000000298023224 || this.SummonedCount > 0)
      return false;
    this.SpawnDelay = 0.0f;
    this.SpawnMoreEnemies();
    return true;
  }

  public IEnumerator SpawnEnemiesRoutine()
  {
    EnemyScout enemyScout = this;
    enemyScout.Spine.AnimationState.SetAnimation(0, "alarm", false);
    enemyScout.Spine.AnimationState.AddAnimation(0, "dance", true, 0.0f);
    yield return (object) new WaitForSeconds(0.8333333f);
    foreach (Vector3 teleportPosition in enemyScout.TeleportPositions)
    {
      EnemySpawner.Create(teleportPosition, enemyScout.transform.parent, enemyScout.EnemiesToSpawn[UnityEngine.Random.Range(0, enemyScout.EnemiesToSpawn.Count)]).GetComponent<Health>().OnDie += new Health.DieAction(enemyScout.RemoveSpawned);
      ++enemyScout.SummonedCount;
    }
    yield return (object) new WaitForSeconds(1.26666665f);
    enemyScout.StartCoroutine((IEnumerator) enemyScout.WaitForTarget());
  }

  public void RemoveSpawned(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.SummonedCount;
    Victim.OnDie -= new Health.DieAction(this.RemoveSpawned);
  }

  public new void OnDrawGizmos()
  {
    if ((UnityEngine.Object) this.state == (UnityEngine.Object) null)
      return;
    int num = -3;
    while ((num += 2) <= 1)
    {
      float f = (float) (((double) this.state.LookAngle + (double) (45 * num)) * (Math.PI / 180.0));
      Vector3 direction = new Vector3(Mathf.Cos(f), Mathf.Sin(f));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, this.SpawnCircleCastRadius, (Vector2) direction, this.Distance, (int) this.layerToCheck);
      Color green = Color.green;
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
        Utils.DrawCircleXY((Vector3) raycastHit2D.centroid, this.SpawnCircleCastRadius, Color.red);
      else
        Utils.DrawCircleXY(this.transform.position + direction * this.Distance, this.SpawnCircleCastRadius, Color.green);
    }
  }
}
