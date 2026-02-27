// Decompiled with JetBrains decompiler
// Type: BonusStatueBlackSoulDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class BonusStatueBlackSoulDamage : BaseMonoBehaviour
{
  private Health health;

  private void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
  }

  private void OnDisable() => this.health.OnHit -= new Health.HitAction(this.Health_OnHit);

  private void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    Health component = Attacker.GetComponent<Health>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.StartCoroutine((IEnumerator) this.GiveBlackSoulsRoutine(component, Attacker));
  }

  private IEnumerator GiveBlackSoulsRoutine(Health AttackerHealth, GameObject Target)
  {
    BonusStatueBlackSoulDamage statueBlackSoulDamage = this;
    yield return (object) new WaitForSeconds(0.25f);
    AttackerHealth.DealDamage(1f, statueBlackSoulDamage.gameObject, Vector3.Lerp(statueBlackSoulDamage.transform.position, AttackerHealth.transform.position, 0.8f));
    yield return (object) new WaitForSeconds(1f);
    float SoulsToGive = 0.0f;
    while ((double) ++SoulsToGive <= 20.0 && !((UnityEngine.Object) Target == (UnityEngine.Object) null))
    {
      if (GameManager.HasUnlockAvailable())
        SoulCustomTarget.Create(Target, statueBlackSoulDamage.transform.position, Color.black, new System.Action(statueBlackSoulDamage.GiveDevotion), 0.2f);
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, statueBlackSoulDamage.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      yield return (object) new WaitForSeconds((float) (0.20000000298023224 - 0.20000000298023224 * ((double) SoulsToGive / 20.0)));
    }
  }

  private void GiveDevotion() => Inventory.AddItem(30, 1);
}
