// Decompiled with JetBrains decompiler
// Type: PoisonBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PoisonBomb : EnemyBomb
{
  [Space]
  [SerializeField]
  public GameObject poisonPrefab;
  [SerializeField]
  public GameObject splashPrefab;
  [SerializeField]
  public float poisonRadius;
  [SerializeField]
  public Vector2 posionScaleRange = new Vector2(1f, 1f);
  [SerializeField]
  public int poisonAmount;
  [SerializeField]
  public float impactDamageRadius;
  public float impactDamage = 1f;
  [CompilerGenerated]
  public float \u003CDamageMultiplier\u003Ek__BackingField = 1f;
  [CompilerGenerated]
  public float \u003CTickDurationMultiplier\u003Ek__BackingField = 1f;
  public TrailRenderer trailRenderer;

  public GameObject PoisonPrefab
  {
    get => this.poisonPrefab;
    set => this.poisonPrefab = value;
  }

  public float PoisonRadius
  {
    get => this.poisonRadius;
    set => this.poisonRadius = value;
  }

  public int PoisonAmount
  {
    get => this.poisonAmount;
    set => this.poisonAmount = value;
  }

  public float DamageMultiplier
  {
    get => this.\u003CDamageMultiplier\u003Ek__BackingField;
    set => this.\u003CDamageMultiplier\u003Ek__BackingField = value;
  }

  public float TickDurationMultiplier
  {
    get => this.\u003CTickDurationMultiplier\u003Ek__BackingField;
    set => this.\u003CTickDurationMultiplier\u003Ek__BackingField = value;
  }

  public void OnDisable()
  {
    if ((Object) this.trailRenderer == (Object) null)
      this.trailRenderer = this.GetComponentInChildren<TrailRenderer>();
    if (!((Object) this.trailRenderer != (Object) null))
      return;
    this.trailRenderer.Clear();
  }

  public override void BombLanded()
  {
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", this.gameObject);
    if ((Object) this.splashPrefab != (Object) null)
      Object.Instantiate<GameObject>(this.splashPrefab, this.transform.position, Quaternion.identity, this.transform.parent).transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360f));
    for (int index = 0; index < this.poisonAmount; ++index)
    {
      Vector2 vector2 = Random.insideUnitCircle * this.poisonRadius;
      float num = Random.Range(this.posionScaleRange.x, this.posionScaleRange.y);
      if (index == 0)
      {
        vector2 *= 0.0f;
        num = 1f;
      }
      GameObject gameObject = ObjectPool.Spawn(this.poisonPrefab, this.transform.parent, this.transform.position + (Vector3) vector2, Quaternion.identity);
      gameObject.transform.localScale = new Vector3(num, num, 1f);
      if ((bool) (Object) gameObject.GetComponent<TrapGoop>())
      {
        gameObject.GetComponent<TrapGoop>().DamageMultiplier = this.DamageMultiplier;
        gameObject.GetComponent<TrapGoop>().TickDurationMultiplier = this.TickDurationMultiplier;
      }
      RaycastHit hitInfo;
      if (Physics.Raycast(gameObject.transform.position - Vector3.forward * 2f, Vector3.forward, out hitInfo, float.PositiveInfinity) && (Object) hitInfo.collider.gameObject.GetComponent<MeshCollider>() != (Object) null)
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, hitInfo.point.z);
    }
    PlayerFarming component1 = (Object) this.GetOwner() != (Object) null ? this.GetOwner().GetComponent<PlayerFarming>() : (PlayerFarming) null;
    if ((Object) component1 != (Object) null && component1.currentCurse == EquipmentType.ProjectileAOE_ExplosiveImpact)
      GameManager.GetInstance().StartCoroutine(this.CreateExplosions(this.transform.position, Random.Range(3, 6), 1.5f));
    if ((double) this.impactDamageRadius <= 0.0)
      return;
    foreach (Component component2 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.impactDamageRadius))
    {
      Health component3 = component2.GetComponent<Health>();
      if ((bool) (Object) component3)
      {
        if (component3.team == Health.Team.Team2 || component3.IsCharmedEnemy)
        {
          if ((Object) component1 != (Object) null && component1.currentCurse == EquipmentType.ProjectileAOE_ExplosiveImpact)
            component3.DealDamage(0.0f, this.gameObject, this.transform.position, dealDamageImmediately: true);
          else if ((double) this.impactDamage > 0.0)
            component3.DealDamage(this.impactDamage, this.gameObject, this.transform.position, dealDamageImmediately: true);
          if ((Object) component1 != (Object) null && component1.currentCurse == EquipmentType.ProjectileAOE_Charm && (double) Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.ProjectileAOE_Charm).Chance)
            component3.AddCharm();
        }
        else if (component3.team == Health.Team.Neutral)
        {
          if ((Object) component1 != (Object) null && component1.currentCurse == EquipmentType.ProjectileAOE_ExplosiveImpact)
            component3.DealDamage(0.0f, this.gameObject, this.transform.position, dealDamageImmediately: true);
          else if ((double) this.impactDamage > 0.0)
            component3.DealDamage(this.impactDamage, this.gameObject, this.transform.position, dealDamageImmediately: true);
        }
      }
    }
  }

  public IEnumerator CreateExplosions(Vector3 p, int amount, float radius)
  {
    for (int i = 0; i < amount; ++i)
    {
      Explosion.CreateExplosion(i != 0 ? p + (Vector3) Random.insideUnitCircle * radius : p, Health.Team.PlayerTeam, (Health) PlayerFarming.Instance.health, 1f, this.impactDamage, this.impactDamage);
      yield return (object) new WaitForSeconds(Random.Range(0.1f, 0.2f));
    }
  }
}
