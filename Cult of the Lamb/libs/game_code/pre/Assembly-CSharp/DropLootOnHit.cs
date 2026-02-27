// Decompiled with JetBrains decompiler
// Type: DropLootOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DropLootOnHit : BaseMonoBehaviour
{
  public InventoryItem.ITEM_TYPE LootToDrop;
  [SerializeField]
  private Vector2 randomAmount;
  [SerializeField]
  private Vector2 randomForce = new Vector2(2f, 4f);
  public bool DontDropOnPlayerFullAmmo;
  private Health health;
  public bool IsNaturalResource;
  public bool dropLootOnHitWithProjectile;

  private void Start() => this.SetHealth();

  public void SetHealth()
  {
    if ((Object) this.health != (Object) null)
      return;
    this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnHit += new Health.HitAction(this.OnHit);
  }

  public void Play(Health.AttackTypes AttackType)
  {
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.NONE)
      return;
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.BLACK_SOUL && (Object) this.health != (Object) null)
    {
      if (this.DontDropOnPlayerFullAmmo && (double) FaithAmmo.Ammo >= (double) FaithAmmo.Total)
        return;
      if (!this.dropLootOnHitWithProjectile)
      {
        if (AttackType == Health.AttackTypes.Projectile)
          return;
        BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(Random.Range(this.randomAmount.x, this.randomAmount.y + 1f) * TrinketManager.GetBlackSoulsMultiplier()), this.transform.position, simulated: true);
        if (!(bool) (Object) blackSoul)
          return;
        blackSoul.SetAngle((float) Random.Range(0, 360), new Vector2(this.randomForce.x, this.randomForce.y));
      }
      else
      {
        BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(Random.Range(this.randomAmount.x, this.randomAmount.y + 1f) * TrinketManager.GetBlackSoulsMultiplier()), this.transform.position, simulated: true);
        if (!(bool) (Object) blackSoul)
          return;
        blackSoul.SetAngle((float) Random.Range(0, 360), new Vector2(this.randomForce.x, this.randomForce.y));
      }
    }
    else
    {
      int num1 = Mathf.RoundToInt(Random.Range(this.randomAmount.x, this.randomAmount.y + 1f));
      if (this.IsNaturalResource)
        num1 = num1 + TrinketManager.GetLootIncreaseModifier(this.LootToDrop) + UpgradeSystem.GetForageIncreaseModifier;
      int num2 = -1;
      while (++num2 < num1)
        InventoryItem.Spawn(this.LootToDrop, 1, this.transform.position);
    }
  }

  private void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if ((Object) Attacker != (Object) null)
    {
      Health component = Attacker.GetComponent<Health>();
      if ((Object) component != (Object) null && component.team != Health.Team.PlayerTeam)
        return;
    }
    this.Play(AttackType);
  }
}
