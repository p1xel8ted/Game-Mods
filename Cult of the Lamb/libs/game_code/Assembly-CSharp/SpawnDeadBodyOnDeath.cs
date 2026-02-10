// Decompiled with JetBrains decompiler
// Type: SpawnDeadBodyOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpawnDeadBodyOnDeath : BaseMonoBehaviour
{
  public Health health;
  public DeadBodySliding deadBodySliding;
  public float Speed = 1700f;

  public void Awake()
  {
    if (!((Object) this.deadBodySliding != (Object) null))
      return;
    ObjectPool.CreatePool<DeadBodySliding>(this.deadBodySliding, ObjectPool.CountPooled<DeadBodySliding>(this.deadBodySliding) + 1, true);
  }

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnDisable() => this.health.OnDie -= new Health.DieAction(this.OnDie);

  public void ReEnable(float Speed)
  {
    this.enabled = true;
    this.Speed = Speed;
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((Object) this.deadBodySliding == (Object) null)
      return;
    if ((Object) this.health != (Object) null)
      this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.DropBody(Attacker);
  }

  public void DropBody(GameObject attacker)
  {
    if ((Object) this.deadBodySliding == (Object) null)
      return;
    GameObject gameObject = this.deadBodySliding.gameObject.Spawn();
    DeadBodySliding component = gameObject.GetComponent<DeadBodySliding>();
    gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    gameObject.transform.parent = this.transform.parent;
    bool explode = TrinketManager.HasTrinket(TarotCards.Card.Skull, PlayerFarming.GetPlayerFarmingComponent(attacker));
    if (!(bool) (Object) component || !(bool) (Object) attacker)
      return;
    component.Init(this.gameObject, Utils.GetAngle(attacker.transform.position, this.transform.position), this.Speed, explode);
  }
}
