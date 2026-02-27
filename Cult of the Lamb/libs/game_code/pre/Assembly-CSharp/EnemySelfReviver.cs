// Decompiled with JetBrains decompiler
// Type: EnemySelfReviver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (EnemyRevivable))]
public class EnemySelfReviver : BaseMonoBehaviour
{
  [SerializeField]
  private float spawnDelay = 2f;
  private Rigidbody2D rigidbody;
  private Health health;
  private EnemyRevivable revivable;
  private GameManager gm;
  private Vector3 startingPosition = Vector3.zero;
  private float spawnInitialDelay = 1f;
  private float spawnDelayTimestamp;
  private float spawnTimer;
  private float shakeSpeed = 10f;
  private float shakeAmount = 0.05f;

  private void Awake()
  {
    this.revivable = this.GetComponent<EnemyRevivable>();
    this.rigidbody = this.GetComponentInChildren<Rigidbody2D>();
    this.health = this.GetComponentInChildren<Health>();
  }

  private void OnEnable()
  {
    this.gm = GameManager.GetInstance();
    this.spawnDelayTimestamp = this.gm.CurrentTime + this.spawnInitialDelay;
    Health.team2.Add(this.health);
    Interaction_Chest.Instance?.AddEnemy(this.health);
  }

  private void OnDisable() => Health.team2.Remove(this.health);

  private void Update()
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

  private void Spawn()
  {
    Health component = EnemySpawner.Create(this.transform.position, this.transform.parent, this.revivable.Enemy).GetComponent<Health>();
    Interaction_Chest.Instance?.AddEnemy(component);
    Interaction_Chest.Instance?.OnSpawnedDie(this.gameObject, this.transform.position, this.health, Health.AttackTypes.Heavy, Health.AttackFlags.Crit);
    Object.Destroy((Object) this.gameObject);
  }
}
