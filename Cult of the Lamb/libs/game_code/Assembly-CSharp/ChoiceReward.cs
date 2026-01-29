// Decompiled with JetBrains decompiler
// Type: ChoiceReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class ChoiceReward
{
  public ChoiceReward.RewardType Type;
  public string Title;
  public string SubTitle;
  public FollowerInfo FollowerInfo;
  public int Paid;
  public int Cost;
  public InventoryItem.ITEM_TYPE Currency;
  public bool Locked;
  public InventoryItem.ITEM_TYPE ItemType;
  public int Quantity;
  public CrownAbilities.TYPE CrownAbility;
  public FollowerRole FollowerRole;

  public ChoiceReward()
  {
  }

  public ChoiceReward(
    int Cost,
    InventoryItem.ITEM_TYPE Currency,
    ChoiceReward.RewardType Type,
    FollowerInfo FollowerInfo)
  {
    this.Type = Type;
    this.FollowerInfo = FollowerInfo;
    this.Cost = Cost;
    this.Title = "New Follower";
    this.SubTitle = FollowerInfo.Name;
    this.Currency = Currency;
  }

  public ChoiceReward(
    int Cost,
    InventoryItem.ITEM_TYPE Currency,
    ChoiceReward.RewardType Type,
    InventoryItem.ITEM_TYPE ItemType,
    int Quantity)
  {
    this.Type = Type;
    this.ItemType = ItemType;
    this.Quantity = Quantity;
    this.Cost = Cost;
    this.Title = "Nature's Bounty";
    this.SubTitle = $"{InventoryItem.Name(ItemType)} x{Quantity.ToString()}";
    this.Currency = Currency;
  }

  public ChoiceReward(
    int Cost,
    InventoryItem.ITEM_TYPE Currency,
    ChoiceReward.RewardType Type,
    CrownAbilities.TYPE CrownAbility)
  {
    this.Type = Type;
    this.CrownAbility = CrownAbility;
    this.Title = "Crown Ability";
    this.SubTitle = CrownAbilities.LocalisedName(CrownAbility);
    this.Cost = Cost;
    this.Currency = Currency;
  }

  public ChoiceReward(FollowerRole FollowerRole, ChoiceReward.RewardType Type, bool Locked = false)
  {
    this.Type = Type;
    this.FollowerRole = FollowerRole;
    this.Locked = Locked;
    this.Title = FollowerRole.ToString();
    this.SubTitle = FollowerRole != FollowerRole.Worshipper ? "" : (Locked ? "Requires Shrine" : "");
    this.Cost = 0;
  }

  public enum RewardType
  {
    Follower,
    Resource,
    CrownAbility,
    FollowerRole,
  }
}
