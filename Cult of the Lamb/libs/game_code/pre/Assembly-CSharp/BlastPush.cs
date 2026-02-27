// Decompiled with JetBrains decompiler
// Type: BlastPush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class BlastPush : BaseMonoBehaviour
{
  [SerializeField]
  private float force = 10f;
  [SerializeField]
  private float radius = 3f;
  [SerializeField]
  private bool dealDamage = true;
  private float LifeTime = 2f;
  private float Timer;

  private void Start()
  {
    if (!this.dealDamage)
      return;
    this.StartCoroutine((IEnumerator) this.PushEnemies());
  }

  private void OnDisable() => Object.Destroy((Object) this.gameObject);

  private IEnumerator PushEnemies()
  {
    BlastPush blastPush = this;
    yield return (object) new WaitForSeconds(0.3f);
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) blastPush.transform.position, blastPush.radius))
    {
      UnitObject component1 = collider2D.GetComponent<UnitObject>();
      if ((bool) (Object) component1 && component1.health.team == Health.Team.Team2)
      {
        float num = (float) (1.0 - (double) Vector3.Distance(blastPush.transform.position, collider2D.transform.position) / (double) blastPush.radius);
        component1.DoKnockBack(blastPush.gameObject, num * blastPush.force, 1f);
        component1.health.DealDamage(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier(), blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile);
        if (DataManager.Instance.CurrentCurse == EquipmentType.EnemyBlast_Poison && (double) Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.EnemyBlast_Poison).Chance)
          component1.health.AddPoison(PlayerFarming.Instance.gameObject);
        else if (DataManager.Instance.CurrentCurse == EquipmentType.EnemyBlast_Ice && (double) Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.EnemyBlast_Ice).Chance)
          component1.health.AddIce();
      }
      else
      {
        float angle = Utils.GetAngle(blastPush.transform.position, collider2D.transform.position);
        Projectile component2 = collider2D.GetComponent<Projectile>();
        if ((bool) (Object) component2 && EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).EquipmentType == EquipmentType.EnemyBlast_DeflectsProjectiles)
        {
          if (component2.destroyOnParry)
          {
            component2.DestroyProjectile(true);
          }
          else
          {
            component2.Angle = angle;
            if ((double) component2.angleNoiseFrequency == 0.0)
              component2.Speed *= 2f;
            component2.KnockedBack = true;
            component2.team = Health.Team.PlayerTeam;
          }
        }
        else if ((bool) (Object) component2 && component2.tag == "Projectile")
          component2.DestroyProjectile(true);
        Health component3 = collider2D.GetComponent<Health>();
        if ((bool) (Object) component3 && component3.team != Health.Team.PlayerTeam)
          component3.DealDamage(10f, blastPush.gameObject, blastPush.transform.position, AttackType: Health.AttackTypes.Projectile);
      }
    }
    CameraManager.instance.ShakeCameraForDuration(0.8f, 1f, 0.1f);
  }

  private void Update()
  {
    if ((double) (this.Timer += Time.deltaTime) <= (double) this.LifeTime)
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
