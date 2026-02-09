// Decompiled with JetBrains decompiler
// Type: BonusStatueBlackSoulDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class BonusStatueBlackSoulDamage : BaseMonoBehaviour
{
  public Health health;

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
  }

  public void OnDisable() => this.health.OnHit -= new Health.HitAction(this.Health_OnHit);

  public void Health_OnHit(
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

  public IEnumerator GiveBlackSoulsRoutine(Health AttackerHealth, GameObject Target)
  {
    BonusStatueBlackSoulDamage statueBlackSoulDamage = this;
    yield return (object) new WaitForSeconds(0.25f);
    AttackerHealth.DealDamage(1f, statueBlackSoulDamage.gameObject, Vector3.Lerp(statueBlackSoulDamage.transform.position, AttackerHealth.transform.position, 0.8f));
    yield return (object) new WaitForSeconds(1f);
    float SoulsToGive = 0.0f;
    while ((double) ++SoulsToGive <= 20.0 && !((UnityEngine.Object) Target == (UnityEngine.Object) null))
    {
      if ((!GameManager.HasUnlockAvailable() ? 0 : (!DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
        SoulCustomTarget.Create(Target, statueBlackSoulDamage.transform.position, Color.black, new System.Action(statueBlackSoulDamage.GiveDevotion), 0.2f);
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, statueBlackSoulDamage.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      yield return (object) new WaitForSeconds((float) (0.20000000298023224 - 0.20000000298023224 * ((double) SoulsToGive / 20.0)));
    }
  }

  public void GiveDevotion() => Inventory.AddItem(30, 1);
}
