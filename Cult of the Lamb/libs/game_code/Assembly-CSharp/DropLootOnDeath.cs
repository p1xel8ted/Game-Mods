// Decompiled with JetBrains decompiler
// Type: DropLootOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Health))]
public class DropLootOnDeath : BaseMonoBehaviour
{
  public DropLootOnDeath.DataSource DropLootDataSource;
  public InventoryItem.ITEM_TYPE LootToDrop;
  public int NumToDrop = 1;
  public int MaxLoot = 15;
  public Structure Structure;
  public bool ShakeOnHit = true;
  public bool AllowTwitchExtraItems = true;
  public bool AllowTarotExtraItems = true;
  public bool OnlyPlayer = true;
  public bool OverrideBlackSoulsNumToDrop;
  public Health health;
  public float rotateSpeedY;
  public float rotateY;
  public bool IsNaturalResource;
  public float RotationToCamera = -60f;
  [CompilerGenerated]
  public bool \u003CGiveXP\u003Ek__BackingField = true;
  public float hp = -1f;

  public bool GiveXP
  {
    get => this.\u003CGiveXP\u003Ek__BackingField;
    set => this.\u003CGiveXP\u003Ek__BackingField = value;
  }

  public void SetHealth()
  {
    if ((Object) this.health == (Object) null)
      this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.hp = this.health.totalHP;
  }

  public void OnEnable()
  {
    if ((Object) this.health == (Object) null)
      this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie += new Health.DieAction(this.OnDie);
    if ((double) this.hp != -1.0)
      return;
    this.hp = this.health.totalHP;
  }

  public void OnDisable()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void Play(GameObject attacker)
  {
    if (this.AllowTwitchExtraItems)
      this.TryApplyTwitchManipulation();
    if (this.AllowTarotExtraItems)
      this.TrySpawnTarotDrops();
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.NONE)
      return;
    PlayerFarming playerFarming = attacker.GetComponent<PlayerFarming>();
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.BLACK_SOUL && (Object) this.health != (Object) null)
    {
      this.MaxLoot = 10;
      if (this.OverrideBlackSoulsNumToDrop)
        InventoryItem.SpawnBlackSoul(Mathf.RoundToInt((float) Mathf.Min(this.MaxLoot, this.NumToDrop) * TrinketManager.GetBlackSoulsMultiplier(playerFarming)), this.transform.position, this.GiveXP);
      else
        InventoryItem.SpawnBlackSoul(Mathf.RoundToInt((float) Mathf.Min(this.MaxLoot, (int) this.hp) * TrinketManager.GetBlackSoulsMultiplier(playerFarming)), this.transform.position, this.GiveXP);
    }
    else
    {
      int num1 = this.DropLootDataSource == DropLootOnDeath.DataSource.StructureData ? 1 : 0;
      InventoryItem.ITEM_TYPE itemType = num1 != 0 ? this.Structure.Structure_Info.LootToDrop : this.LootToDrop;
      int num2 = num1 != 0 ? this.Structure.Structure_Info.LootCountToDrop : this.NumToDrop;
      if (this.IsNaturalResource)
        num2 = num2 + TrinketManager.GetLootIncreaseModifier(itemType, playerFarming) + UpgradeSystem.GetForageIncreaseModifier;
      int num3 = -1;
      while (++num3 < num2)
        InventoryItem.Spawn(itemType, 1, this.transform.position);
    }
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.Play(Attacker);
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void SetAllowExtraItems(bool allow)
  {
    this.AllowTwitchExtraItems = allow;
    this.AllowTarotExtraItems = allow;
  }

  public void TrySpawnTarotDrops()
  {
    if (!TrinketManager.HasTrinket(TarotCards.Card.MutatedDropRotburn) || !((Object) this.health != (Object) null) || this.health.team != Health.Team.Team2 || DungeonSandboxManager.Active)
      return;
    DropLootOnDeath component = this.GetComponent<DropLootOnDeath>();
    if (!(bool) (Object) component || !component.GiveXP)
      return;
    int num = Random.Range(0, 3);
    for (int index = 0; index < num; ++index)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MAGMA_STONE, 1, this.transform.position);
    if (num <= 0)
      return;
    AudioManager.Instance.PlayOneShot("event:/dlc/tarot/rotburn_pop", this.transform.position);
  }

  public void TryApplyTwitchManipulation()
  {
    if (!DataManager.Instance.EnemiesDropGoldDuringRun || !((Object) this.health != (Object) null) || this.health.team != Health.Team.Team2)
      return;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, Random.Range(1, 3), this.transform.position);
  }

  public enum DataSource
  {
    Local,
    StructureData,
  }
}
