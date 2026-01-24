// Decompiled with JetBrains decompiler
// Type: DropLootOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DropLootOnHit : BaseMonoBehaviour
{
  public InventoryItem.ITEM_TYPE LootToDrop;
  [SerializeField]
  public Vector2 randomAmount;
  [SerializeField]
  public Vector2 randomForce = new Vector2(2f, 4f);
  public bool DontDropOnPlayerFullAmmo;
  public Health health;
  public bool IsNaturalResource;
  public bool dropLootOnHitWithProjectile;

  public void Start() => this.SetHealth();

  public void SetHealth()
  {
    if ((Object) this.health != (Object) null)
      return;
    this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnHit += new Health.HitAction(this.OnHit);
  }

  public void Play(Health.AttackTypes AttackType, PlayerFarming playerFarming)
  {
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.NONE)
      return;
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.BLACK_SOUL && (Object) this.health != (Object) null && (Object) playerFarming != (Object) null)
    {
      if (this.DontDropOnPlayerFullAmmo && (double) playerFarming.playerSpells.faithAmmo.Ammo >= (double) playerFarming.playerSpells.faithAmmo.Total)
        return;
      if (!this.dropLootOnHitWithProjectile)
      {
        if (AttackType == Health.AttackTypes.Projectile)
          return;
        BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(Random.Range(this.randomAmount.x, this.randomAmount.y + 1f) * TrinketManager.GetBlackSoulsMultiplier(playerFarming)), this.transform.position, simulated: true);
        if (!(bool) (Object) blackSoul)
          return;
        blackSoul.SetAngle((float) Random.Range(0, 360), new Vector2(this.randomForce.x, this.randomForce.y));
      }
      else
      {
        BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(Random.Range(this.randomAmount.x, this.randomAmount.y + 1f) * TrinketManager.GetBlackSoulsMultiplier(playerFarming)), this.transform.position, simulated: true);
        if (!(bool) (Object) blackSoul)
          return;
        blackSoul.SetAngle((float) Random.Range(0, 360), new Vector2(this.randomForce.x, this.randomForce.y));
      }
    }
    else
    {
      int num1 = Mathf.RoundToInt(Random.Range(this.randomAmount.x, this.randomAmount.y + 1f));
      if (this.IsNaturalResource)
      {
        if ((Object) playerFarming != (Object) null)
          num1 += TrinketManager.GetLootIncreaseModifier(this.LootToDrop, playerFarming);
        num1 += UpgradeSystem.GetForageIncreaseModifier;
      }
      int num2 = -1;
      while (++num2 < num1)
        InventoryItem.Spawn(this.LootToDrop, 1, this.transform.position);
    }
  }

  public void OnHit(
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
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(Attacker);
    this.Play(AttackType, farmingComponent);
  }
}
