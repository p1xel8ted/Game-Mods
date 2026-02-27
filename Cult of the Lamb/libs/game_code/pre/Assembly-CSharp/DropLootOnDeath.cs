// Decompiled with JetBrains decompiler
// Type: DropLootOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  public bool OnlyPlayer = true;
  public bool OverrideBlackSoulsNumToDrop;
  private Health health;
  private float rotateSpeedY;
  private float rotateY;
  public bool IsNaturalResource;
  public float RotationToCamera = -60f;
  private float hp = -1f;

  public bool GiveXP { get; set; } = true;

  public void SetHealth()
  {
    if ((Object) this.health == (Object) null)
      this.health = this.GetComponent<Health>();
    if (!((Object) this.health != (Object) null))
      return;
    this.hp = this.health.totalHP;
  }

  private void OnEnable()
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

  private void OnDisable()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void Play()
  {
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.NONE)
      return;
    if (this.LootToDrop == InventoryItem.ITEM_TYPE.BLACK_SOUL && (Object) this.health != (Object) null)
    {
      this.MaxLoot = 10;
      if (this.OverrideBlackSoulsNumToDrop)
        InventoryItem.SpawnBlackSoul(Mathf.RoundToInt((float) Mathf.Min(this.MaxLoot, this.NumToDrop) * TrinketManager.GetBlackSoulsMultiplier()), this.transform.position, this.GiveXP);
      else
        InventoryItem.SpawnBlackSoul(Mathf.RoundToInt((float) Mathf.Min(this.MaxLoot, (int) this.hp) * TrinketManager.GetBlackSoulsMultiplier()), this.transform.position, this.GiveXP);
    }
    else
    {
      int num1 = this.DropLootDataSource == DropLootOnDeath.DataSource.StructureData ? 1 : 0;
      InventoryItem.ITEM_TYPE itemType = num1 != 0 ? this.Structure.Structure_Info.LootToDrop : this.LootToDrop;
      int num2 = num1 != 0 ? this.Structure.Structure_Info.LootCountToDrop : this.NumToDrop;
      if (this.IsNaturalResource)
        num2 = num2 + TrinketManager.GetLootIncreaseModifier(itemType) + UpgradeSystem.GetForageIncreaseModifier;
      int num3 = -1;
      while (++num3 < num2)
        InventoryItem.Spawn(itemType, 1, this.transform.position);
    }
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if ((Object) Attacker != (Object) null)
    {
      Health component = Attacker.GetComponent<Health>();
      if ((Object) component != (Object) null && component.team != Health.Team.PlayerTeam)
        return;
    }
    this.Play();
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public enum DataSource
  {
    Local,
    StructureData,
  }
}
