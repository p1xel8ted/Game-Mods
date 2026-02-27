// Decompiled with JetBrains decompiler
// Type: DropLootOnHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
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

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnHit -= new Health.HitAction(this.OnHit);
  }

  public void SetHealth()
  {
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
      return;
    this.health = this.GetComponent<Health>();
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnHit += new Health.HitAction(this.OnHit);
  }

  public void Play(Health.AttackTypes AttackType, PlayerFarming playerFarming)
  {
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.NONE)
      return;
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.BLACK_SOUL && (UnityEngine.Object) this.health != (UnityEngine.Object) null && (UnityEngine.Object) playerFarming != (UnityEngine.Object) null)
    {
      if (this.DontDropOnPlayerFullAmmo && (double) playerFarming.playerSpells.faithAmmo.Ammo >= (double) playerFarming.playerSpells.faithAmmo.Total)
        return;
      if (!this.dropLootOnHitWithProjectile)
      {
        if (AttackType == Health.AttackTypes.Projectile)
          return;
        BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(UnityEngine.Random.Range(this.randomAmount.x, this.randomAmount.y + 1f) * TrinketManager.GetBlackSoulsMultiplier(playerFarming)), this.transform.position, simulated: true);
        if (!(bool) (UnityEngine.Object) blackSoul)
          return;
        blackSoul.SetAngle((float) UnityEngine.Random.Range(0, 360), new Vector2(this.randomForce.x, this.randomForce.y));
      }
      else
      {
        BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(Mathf.RoundToInt(UnityEngine.Random.Range(this.randomAmount.x, this.randomAmount.y + 1f) * TrinketManager.GetBlackSoulsMultiplier(playerFarming)), this.transform.position, simulated: true);
        if (!(bool) (UnityEngine.Object) blackSoul)
          return;
        blackSoul.SetAngle((float) UnityEngine.Random.Range(0, 360), new Vector2(this.randomForce.x, this.randomForce.y));
      }
    }
    else
    {
      int num = Mathf.RoundToInt(UnityEngine.Random.Range(this.randomAmount.x, this.randomAmount.y + 1f));
      if (this.IsNaturalResource)
      {
        if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) null)
          num += TrinketManager.GetLootIncreaseModifier(this.LootToDrop, playerFarming);
        num += UpgradeSystem.GetForageIncreaseModifier;
      }
      Vector3 position = Vector3.zero;
      if ((UnityEngine.Object) this.transform != (UnityEngine.Object) null)
        position = this.transform.position;
      for (int index = 0; index < num; ++index)
        InventoryItem.Spawn(this.LootToDrop, 1, position);
    }
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
    {
      Health component = Attacker.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.team != Health.Team.PlayerTeam)
        return;
    }
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(Attacker);
    try
    {
      this.Play(AttackType, farmingComponent);
    }
    catch (NullReferenceException ex)
    {
      Debug.LogError((object) ex.Message);
    }
  }
}
