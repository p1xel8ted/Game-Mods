// Decompiled with JetBrains decompiler
// Type: EnemyReviver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyReviver : UnitObject
{
  public SkeletonAnimation Spine;
  [SerializeField]
  public float reviveDuration = 2f;
  [SerializeField]
  public LayerMask deadBodyMask;
  [SerializeField]
  public LayerMask enemyMask;
  public GameManager gm;
  public const float timeBetweenChecks = 0.2f;
  public float checkBodyTimestamp;
  public bool reviving;
  public List<Health> spawnedEnemies = new List<Health>();
  public GameObject currentDeadBody;
  public Coroutine spawnRoutine;

  public void Start()
  {
    this.gm = GameManager.GetInstance();
    this.Spine.transform.localPosition = this.Spine.transform.up * -2f;
    this.Spine.transform.gameObject.SetActive(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!((Object) this.currentDeadBody != (Object) null))
      return;
    this.Spine.transform.localPosition = this.Spine.transform.up * -2f;
    this.StartCoroutine(this.SpawnEnemyIE(this.currentDeadBody.GetComponentInChildren<Health>()));
  }

  public override void Update()
  {
    base.Update();
    if (this.spawnRoutine == null || !((Object) this.currentDeadBody == (Object) null))
      return;
    this.StopCoroutine(this.spawnRoutine);
    this.spawnRoutine = (Coroutine) null;
    this.StartCoroutine(this.PopOut());
  }

  public new void LateUpdate()
  {
    base.Update();
    if (!this.reviving && (double) this.gm.CurrentTime > (double) this.checkBodyTimestamp)
    {
      Health deadBody = this.FindDeadBody();
      if ((bool) (Object) deadBody)
        this.spawnRoutine = this.StartCoroutine(this.SpawnEnemyIE(deadBody));
      this.checkBodyTimestamp = this.gm.CurrentTime + 0.2f;
    }
    List<Health> team2 = Health.team2;
    team2.Remove(this.health);
    List<Health> neutralTeam = Health.neutralTeam;
    for (int index = neutralTeam.Count - 1; index >= 0; --index)
    {
      if (!(bool) (Object) neutralTeam[index].GetComponentInParent<DeadBodySliding>())
        neutralTeam.RemoveAt(index);
    }
    if (!GameManager.RoomActive || neutralTeam.Count != 0 || team2.Count != 0 || this.spawnedEnemies.Count != 0)
      return;
    this.health.MeleeAttackVulnerability = 1f;
    this.health.DamageModifier = 1f;
    this.health.untouchable = false;
    this.health.DestroyOnDeath = false;
    this.health.DealDamage(this.health.totalHP, this.gameObject, this.transform.position, dealDamageImmediately: true);
    this.health.DestroyNextFrame();
  }

  public IEnumerator SpawnEnemyIE(Health deadBody)
  {
    EnemyReviver enemyReviver = this;
    enemyReviver.reviving = true;
    enemyReviver.currentDeadBody = deadBody.gameObject;
    yield return (object) new WaitForSeconds(1f);
    enemyReviver.transform.position = deadBody.transform.position + (Vector3) Random.insideUnitCircle * 2f;
    yield return (object) enemyReviver.StartCoroutine(enemyReviver.PopIn());
    Vector3 startingPosition = deadBody.transform.position;
    float t = 0.0f;
    while ((double) t < (double) enemyReviver.reviveDuration)
    {
      float num = t / enemyReviver.reviveDuration;
      deadBody.transform.position = new Vector3(startingPosition.x + Mathf.Sin(Time.time * (10f * num)) * (0.05f * num), deadBody.transform.position.y, deadBody.transform.position.z);
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemyReviver.SpawnEnemy(deadBody.GetComponentInParent<EnemyRevivable>());
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) enemyReviver.StartCoroutine(enemyReviver.PopOut());
  }

  public IEnumerator PopIn()
  {
    this.Spine.transform.localPosition = this.Spine.transform.up * -2f;
    this.Spine.transform.gameObject.SetActive(true);
    this.Spine.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.5f);
  }

  public IEnumerator PopOut()
  {
    this.Spine.transform.DOLocalMove(this.Spine.transform.up * -2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.5f);
    this.Spine.transform.gameObject.SetActive(false);
    this.reviving = false;
  }

  public void SpawnEnemy(EnemyRevivable revivable)
  {
    Health component = EnemySpawner.Create(revivable.transform.position, this.transform.parent, revivable.Enemy).GetComponent<Health>();
    Interaction_Chest.Instance?.AddEnemy(component);
    Health.team2.Remove(revivable.GetComponentInChildren<Health>());
    Object.Destroy((Object) revivable.gameObject);
    component.OnDie += new Health.DieAction(this.EnemyDied);
    this.spawnedEnemies.Add(component);
  }

  public void EnemyDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.spawnedEnemies.Remove(Victim);
  }

  public Health FindDeadBody()
  {
    Collider2D[] collider2DArray = Physics2D.OverlapCircleAll((Vector2) this.transform.position, 50f, (int) this.deadBodyMask);
    Collider2D collider2D1 = (Collider2D) null;
    if (collider2DArray.Length != 0)
    {
      foreach (Collider2D collider2D2 in collider2DArray)
      {
        if ((bool) (Object) collider2D2.GetComponentInParent<DeadBodySliding>() && ((Object) collider2D1 == (Object) null || (double) Vector3.Distance(this.transform.position, collider2D2.transform.position) < (double) Vector3.Distance(this.transform.position, collider2D1.transform.position)))
          collider2D1 = collider2D2;
      }
    }
    return collider2D1?.GetComponentInChildren<Health>();
  }
}
