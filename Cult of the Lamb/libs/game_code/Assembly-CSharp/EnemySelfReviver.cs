// Decompiled with JetBrains decompiler
// Type: EnemySelfReviver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (EnemyRevivable))]
public class EnemySelfReviver : BaseMonoBehaviour
{
  [SerializeField]
  public float spawnDelay = 2f;
  public Rigidbody2D rigidbody;
  public Health health;
  public EnemyRevivable revivable;
  public GameManager gm;
  public Vector3 startingPosition = Vector3.zero;
  public float spawnInitialDelay = 1f;
  public float spawnDelayTimestamp;
  public float spawnTimer;
  public float shakeSpeed = 10f;
  public float shakeAmount = 0.05f;

  public void Awake()
  {
    this.revivable = this.GetComponent<EnemyRevivable>();
    this.rigidbody = this.GetComponentInChildren<Rigidbody2D>();
    this.health = this.GetComponentInChildren<Health>();
  }

  public void OnEnable()
  {
    this.gm = GameManager.GetInstance();
    this.spawnDelayTimestamp = this.gm.CurrentTime + this.spawnInitialDelay;
    Health.team2.Add(this.health);
    Interaction_Chest.Instance?.AddEnemy(this.health);
  }

  public void OnDisable() => Health.team2.Remove(this.health);

  public void Update()
  {
    if (!(bool) (Object) this.gm || (double) this.gm.CurrentTime <= (double) this.spawnDelayTimestamp || (double) this.rigidbody.velocity.magnitude > 0.10000000149011612)
      return;
    if (this.startingPosition == Vector3.zero)
      this.startingPosition = this.transform.position;
    this.spawnTimer += Time.deltaTime;
    float num = this.spawnTimer / this.spawnDelay;
    this.transform.position = new Vector3(this.startingPosition.x + Mathf.Sin(Time.time * (this.shakeSpeed * num)) * (this.shakeAmount * num), this.transform.position.y, this.transform.position.z);
    if ((double) num <= 1.0)
      return;
    this.Spawn();
  }

  public void Spawn()
  {
    Health component = EnemySpawner.Create(this.transform.position, this.transform.parent, this.revivable.Enemy).GetComponent<Health>();
    Interaction_Chest.Instance?.AddEnemy(component);
    Interaction_Chest.Instance?.OnSpawnedDie(this.gameObject, this.transform.position, this.health, Health.AttackTypes.Heavy, Health.AttackFlags.Crit);
    Object.Destroy((Object) this.gameObject);
  }
}
