// Decompiled with JetBrains decompiler
// Type: PoisonBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class PoisonBomb : EnemyBomb
{
  [Space]
  [SerializeField]
  private GameObject poisonPrefab;
  [SerializeField]
  private GameObject splashPrefab;
  [SerializeField]
  private float poisonRadius;
  [SerializeField]
  private Vector2 posionScaleRange = new Vector2(1f, 1f);
  [SerializeField]
  private int poisonAmount;
  [SerializeField]
  private float impactDamageRadius;
  public float impactDamage = 1f;

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

  public float DamageMultiplier { get; set; } = 1f;

  public float TickDurationMultiplier { get; set; } = 1f;

  protected override void BombLanded()
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
      GameObject gameObject = Object.Instantiate<GameObject>(this.poisonPrefab, this.transform.position + (Vector3) vector2, Quaternion.identity, this.transform.parent);
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
    if (DataManager.Instance.CurrentCurse == EquipmentType.ProjectileAOE_ExplosiveImpact)
    {
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.CreateExplosions(this.transform.position, Random.Range(3, 6), 1.5f));
    }
    else
    {
      foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, this.impactDamageRadius))
      {
        Health component2 = component1.GetComponent<Health>();
        if ((bool) (Object) component2 && component2.team == Health.Team.Team2)
        {
          if ((double) this.impactDamage > 0.0)
            component2.DealDamage(this.impactDamage, this.gameObject, this.transform.position);
          if (DataManager.Instance.CurrentCurse == EquipmentType.ProjectileAOE_Charm && (double) Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.ProjectileAOE_Charm).Chance)
            component2.AddCharm();
        }
      }
    }
  }

  private IEnumerator CreateExplosions(Vector3 p, int amount, float radius)
  {
    for (int i = 0; i < amount; ++i)
    {
      Explosion.CreateExplosion(i != 0 ? p + (Vector3) Random.insideUnitCircle * radius : p, Health.Team.PlayerTeam, PlayerFarming.Instance.health, 1f, this.impactDamage, this.impactDamage);
      yield return (object) new WaitForSeconds(Random.Range(0.1f, 0.2f));
    }
  }
}
