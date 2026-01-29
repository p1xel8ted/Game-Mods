// Decompiled with JetBrains decompiler
// Type: EnemyHordeMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyHordeMiniboss : UnitObject
{
  public SkeletonAnimation Spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string spawnAnimation;
  [SerializeField]
  public Vector2 timeBetweenMove;
  [SerializeField]
  public GameObject singleSpawnable;
  [SerializeField]
  public float spawnAnticipation;
  [SerializeField]
  public Vector2 timeBetweenSpawns;
  [SerializeField]
  public int maxSpawnAmount;
  [SerializeField]
  public float horizontalDistanceBetween;
  [SerializeField]
  public Vector3[] horizontalSpawnPositions = new Vector3[2];
  [SerializeField]
  public float horizontalTimeBetween;
  [SerializeField]
  public int horizontalAmount;
  [SerializeField]
  public float verticalDistanceBetween;
  [SerializeField]
  public Vector3[] verticalSpawnPositions = new Vector3[2];
  [SerializeField]
  public float verticalTimeBetween;
  [SerializeField]
  public int verticalAmount;
  [SerializeField]
  public float circleRadius;
  [SerializeField]
  public int circleAmount;
  [SerializeField]
  public float circleTimeBetween;
  public bool spawning;
  public float spawnTimestamp = -1f;
  public float moveTimestamp;
  public List<UnitObject> spawnedEnemies = new List<UnitObject>();
  public EnemyHordeMiniboss.SpawnType previousSpawnType;
  public int currentEnemies;

  public int availableEnemyCount => this.maxSpawnAmount - this.currentEnemies;

  public override void OnEnable()
  {
    base.OnEnable();
    this.spawning = false;
    this.simpleSpineFlash.FlashWhite(false);
  }

  public override void Update()
  {
    base.Update();
    if (GameManager.RoomActive && (double) this.spawnTimestamp == -1.0)
      this.spawnTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.timeBetweenSpawns.x, this.timeBetweenSpawns.y);
    if (GameManager.RoomActive)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float moveTimestamp = this.moveTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) moveTimestamp & currentTime.HasValue)
        this.Flee();
    }
    if (!this.CanSpawn())
      return;
    this.Spawn();
  }

  public bool CanSpawn()
  {
    if (GameManager.RoomActive && !this.spawning)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float spawnTimestamp = this.spawnTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) spawnTimestamp & currentTime.HasValue)
        return (double) this.spawnTimestamp != -1.0;
    }
    return false;
  }

  public void Spawn()
  {
    EnemyHordeMiniboss.SpawnType spawnType;
    do
    {
      spawnType = (EnemyHordeMiniboss.SpawnType) UnityEngine.Random.Range(0, Enum.GetNames(typeof (EnemyHordeMiniboss.SpawnType)).Length);
    }
    while (spawnType == this.previousSpawnType);
    switch (spawnType)
    {
      case EnemyHordeMiniboss.SpawnType.Horizontal1:
        this.SpawnHorizontalLine1();
        break;
      case EnemyHordeMiniboss.SpawnType.Horizontal2:
        this.SpawnHorizontalLine2();
        break;
      case EnemyHordeMiniboss.SpawnType.Vertical:
        this.SpawnVerticalLine();
        break;
      case EnemyHordeMiniboss.SpawnType.Circle:
        if ((double) Vector3.Distance(PlayerFarming.Instance.transform.position, Vector3.zero) < 4.5 - (double) this.circleRadius / 2.0)
        {
          this.SpawnCircleAroundPlayer();
          break;
        }
        break;
    }
    this.previousSpawnType = spawnType;
  }

  public void SpawnHorizontalLine1()
  {
    this.StartCoroutine((IEnumerator) this.SpawnHorizontalLineIE(this.horizontalSpawnPositions[0]));
  }

  public void SpawnHorizontalLine2()
  {
    this.StartCoroutine((IEnumerator) this.SpawnHorizontalLineIE(this.horizontalSpawnPositions[1]));
  }

  public IEnumerator SpawnHorizontalLineIE(Vector3 spawnPosition)
  {
    EnemyHordeMiniboss enemyHordeMiniboss = this;
    if (enemyHordeMiniboss.availableEnemyCount < enemyHordeMiniboss.verticalAmount)
    {
      enemyHordeMiniboss.spawning = false;
    }
    else
    {
      enemyHordeMiniboss.spawning = true;
      enemyHordeMiniboss.Spine.AnimationState.SetAnimation(0, enemyHordeMiniboss.spawnAnimation, false);
      yield return (object) new WaitForSeconds(enemyHordeMiniboss.spawnAnticipation);
      if (enemyHordeMiniboss.availableEnemyCount < enemyHordeMiniboss.horizontalAmount)
      {
        enemyHordeMiniboss.spawning = false;
      }
      else
      {
        float xStartPosition = -(float) ((double) enemyHordeMiniboss.horizontalDistanceBetween * (double) enemyHordeMiniboss.horizontalAmount / 2.0);
        for (int i = 0; i < enemyHordeMiniboss.horizontalAmount; ++i)
        {
          enemyHordeMiniboss.StartCoroutine((IEnumerator) enemyHordeMiniboss.SpawnEnemy(new Vector3(xStartPosition + spawnPosition.x, spawnPosition.y, spawnPosition.z)));
          xStartPosition += enemyHordeMiniboss.horizontalDistanceBetween;
          yield return (object) new WaitForSeconds(enemyHordeMiniboss.horizontalTimeBetween);
        }
        enemyHordeMiniboss.spawnTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyHordeMiniboss.timeBetweenSpawns.x, enemyHordeMiniboss.timeBetweenSpawns.y);
        enemyHordeMiniboss.spawning = false;
      }
    }
  }

  public void SpawnVerticalLine() => this.StartCoroutine((IEnumerator) this.SpawnVerticalLineIE());

  public IEnumerator SpawnVerticalLineIE()
  {
    EnemyHordeMiniboss enemyHordeMiniboss = this;
    if (enemyHordeMiniboss.availableEnemyCount < enemyHordeMiniboss.verticalAmount)
    {
      enemyHordeMiniboss.spawning = false;
    }
    else
    {
      enemyHordeMiniboss.spawning = true;
      enemyHordeMiniboss.Spine.AnimationState.SetAnimation(0, enemyHordeMiniboss.spawnAnimation, false);
      int spawnType = UnityEngine.Random.Range(0, 4);
      yield return (object) new WaitForSeconds(enemyHordeMiniboss.spawnAnticipation);
      if (enemyHordeMiniboss.availableEnemyCount < enemyHordeMiniboss.verticalAmount)
      {
        enemyHordeMiniboss.spawning = false;
      }
      else
      {
        float num = (float) ((double) enemyHordeMiniboss.verticalDistanceBetween * (double) enemyHordeMiniboss.verticalAmount / 2.0);
        float yStartPosition1 = -num;
        float yStartPosition2 = -num;
        int direction1 = 1;
        int direction2 = 1;
        switch (spawnType)
        {
          case 1:
            direction1 = -1;
            direction2 = -1;
            yStartPosition1 = num;
            yStartPosition2 = num;
            break;
          case 2:
            direction1 = -1;
            yStartPosition1 = num;
            break;
          case 3:
            direction2 = -1;
            yStartPosition2 = num;
            break;
        }
        for (int i = 0; i < enemyHordeMiniboss.verticalAmount * 2; i += 2)
        {
          Vector3 position1 = new Vector3(enemyHordeMiniboss.verticalSpawnPositions[0].x, enemyHordeMiniboss.verticalSpawnPositions[0].y + yStartPosition1, enemyHordeMiniboss.verticalSpawnPositions[0].z);
          Vector3 position2 = new Vector3(enemyHordeMiniboss.verticalSpawnPositions[1].x, enemyHordeMiniboss.verticalSpawnPositions[1].y + yStartPosition2, enemyHordeMiniboss.verticalSpawnPositions[1].z);
          enemyHordeMiniboss.StartCoroutine((IEnumerator) enemyHordeMiniboss.SpawnEnemy(position1));
          enemyHordeMiniboss.StartCoroutine((IEnumerator) enemyHordeMiniboss.SpawnEnemy(position2));
          yStartPosition1 += enemyHordeMiniboss.verticalDistanceBetween * (float) direction1;
          yStartPosition2 += enemyHordeMiniboss.verticalDistanceBetween * (float) direction2;
          yield return (object) new WaitForSeconds(enemyHordeMiniboss.verticalTimeBetween);
        }
        enemyHordeMiniboss.spawnTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyHordeMiniboss.timeBetweenSpawns.x, enemyHordeMiniboss.timeBetweenSpawns.y);
        enemyHordeMiniboss.spawning = false;
      }
    }
  }

  public void SpawnCircleAroundPlayer()
  {
    this.StartCoroutine((IEnumerator) this.SpawnCircleAroundPlayerIE());
  }

  public IEnumerator SpawnCircleAroundPlayerIE()
  {
    EnemyHordeMiniboss enemyHordeMiniboss = this;
    if (enemyHordeMiniboss.availableEnemyCount < enemyHordeMiniboss.verticalAmount)
    {
      enemyHordeMiniboss.spawning = false;
    }
    else
    {
      enemyHordeMiniboss.spawning = true;
      enemyHordeMiniboss.Spine.AnimationState.SetAnimation(0, enemyHordeMiniboss.spawnAnimation, false);
      yield return (object) new WaitForSeconds(enemyHordeMiniboss.spawnAnticipation);
      if (enemyHordeMiniboss.availableEnemyCount < enemyHordeMiniboss.circleAmount)
      {
        enemyHordeMiniboss.spawning = false;
      }
      else
      {
        Vector3 playerPosition = PlayerFarming.Instance.transform.position;
        float angle = 0.0f;
        float increment = (float) (360 / enemyHordeMiniboss.circleAmount);
        int direction = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        for (int i = 0; i < enemyHordeMiniboss.circleAmount * 2; i += 2)
        {
          Vector3 position = playerPosition + (Vector3) (Utils.RadianToVector2(angle * ((float) Math.PI / 180f)) * enemyHordeMiniboss.circleRadius);
          enemyHordeMiniboss.StartCoroutine((IEnumerator) enemyHordeMiniboss.SpawnEnemy(position));
          angle = Utils.Repeat(angle + increment * (float) direction, 360f);
          yield return (object) new WaitForSeconds(enemyHordeMiniboss.circleTimeBetween);
        }
        enemyHordeMiniboss.spawnTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(enemyHordeMiniboss.timeBetweenSpawns.x, enemyHordeMiniboss.timeBetweenSpawns.y);
        enemyHordeMiniboss.spawning = false;
      }
    }
  }

  public IEnumerator SpawnEnemy(Vector3 position)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyHordeMiniboss enemyHordeMiniboss = this;
    UnitObject enemy;
    EnemyStealth stealth;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      stealth.enabled = true;
      enemy.enabled = true;
      enemy.health.invincible = false;
      enemy.health.OnDie += new Health.DieAction(enemyHordeMiniboss.OnEnemyKilled);
      ++enemyHordeMiniboss.currentEnemies;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemy = UnityEngine.Object.Instantiate<GameObject>(enemyHordeMiniboss.singleSpawnable, position, Quaternion.identity, enemyHordeMiniboss.transform.parent).GetComponent<UnitObject>();
    SkeletonAnimation spine = enemy.GetComponent<EnemySwordsman>().Spine;
    spine.AnimationState.SetAnimation(0, "grave-spawn", false);
    spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    enemyHordeMiniboss.spawnedEnemies.Add(enemy);
    stealth = enemy.GetComponent<EnemyStealth>();
    stealth.enabled = false;
    enemy.enabled = false;
    enemy.health.invincible = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnEnemyKilled(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.currentEnemies;
  }

  public void Flee()
  {
    if (!(bool) (UnityEngine.Object) PlayerFarming.Instance)
      return;
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(7, 10);
      Vector3 targetLocation = PlayerFarming.Instance.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      Vector3 direction = Vector3.Normalize(targetLocation - PlayerFarming.Instance.transform.position);
      if ((UnityEngine.Object) Physics2D.CircleCast((Vector2) PlayerFarming.Instance.transform.position, 0.5f, (Vector2) direction, distance, (int) this.layerToCheck).collider == (UnityEngine.Object) null)
      {
        this.moveTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.timeBetweenMove.x, this.timeBetweenMove.y);
        this.givePath(targetLocation);
      }
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.simpleSpineFlash.FlashFillRed();
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
        Health.team2[index].DealDamage(Health.team2[index].totalHP, Attacker, AttackLocation);
      }
    }
  }

  public void OnDrawGizmos()
  {
    foreach (Vector3 horizontalSpawnPosition in this.horizontalSpawnPositions)
      Utils.DrawCircleXY(horizontalSpawnPosition, 0.5f, Color.green);
    foreach (Vector3 verticalSpawnPosition in this.verticalSpawnPositions)
      Utils.DrawCircleXY(verticalSpawnPosition, 0.5f, Color.green);
  }

  public enum SpawnType
  {
    Horizontal1,
    Horizontal2,
    Vertical,
    Circle,
  }
}
