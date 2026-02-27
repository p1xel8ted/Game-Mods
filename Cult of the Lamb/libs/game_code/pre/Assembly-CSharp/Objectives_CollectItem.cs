// Decompiled with JetBrains decompiler
// Type: Objectives_CollectItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public class Objectives_CollectItem : ObjectivesData
{
  public InventoryItem.ITEM_TYPE ItemType;
  public FollowerLocation TargetLocation;
  public int Target;
  public int StartingAmount = -1;
  public int Count;
  private bool countIsTotal;
  public string CustomTerm = "";

  public override string Text
  {
    get
    {
      int num = Mathf.Clamp(this.countIsTotal ? this.Count : this.Count - (this.StartingAmount == -1 ? 0 : this.StartingAmount), 0, this.Target);
      if (!string.IsNullOrEmpty(this.CustomTerm))
        return LocalizationManager.GetTranslation(this.CustomTerm) + $" {num}/{this.Target}";
      if (this.TargetLocation == FollowerLocation.Base)
        return string.Format(ScriptLocalization.Objectives.CollectItem, (object) (InventoryItem.Name(this.ItemType) + FontImageNames.GetIconByType(this.ItemType)), (object) num, (object) this.Target);
      return string.Format(ScriptLocalization.Objectives.CollectItemFromDungeon, (object) InventoryItem.Name(this.ItemType), (object) FontImageNames.GetIconByType(this.ItemType), (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"), (object) num, (object) this.Target);
    }
  }

  public Objectives_CollectItem()
  {
  }

  public Objectives_CollectItem(
    string groupId,
    InventoryItem.ITEM_TYPE itemType,
    int target,
    bool targetIsTotal = true,
    FollowerLocation targetLocation = FollowerLocation.Base,
    float expireTimestamp = -1f)
    : base(groupId, expireTimestamp)
  {
    this.Type = Objectives.TYPES.COLLECT_ITEM;
    this.TargetLocation = targetLocation;
    this.ItemType = itemType;
    this.Target = target;
    this.countIsTotal = targetIsTotal;
  }

  public override void Init(bool initialAssigning)
  {
    if (!this.initialised)
      Inventory.OnItemAddedToInventory += new Inventory.ItemAddedToInventory(this.OnItemAddedToInventory);
    if (initialAssigning)
    {
      this.Count = Inventory.GetItemQuantity((int) this.ItemType);
      if (!this.countIsTotal)
        this.StartingAmount = Inventory.GetItemQuantity((int) this.ItemType);
    }
    base.Init(initialAssigning);
    ObjectiveManager.CheckObjectives(this.Type);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    int num = Mathf.Clamp(this.countIsTotal ? this.Count : this.Count - this.StartingAmount, 0, this.Target);
    Objectives_CollectItem.FinalizedData_CollectItem finalizedData = new Objectives_CollectItem.FinalizedData_CollectItem();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.ItemType = this.ItemType;
    finalizedData.LocKey = this.TargetLocation == FollowerLocation.Base ? "Objectives/CollectItem" : "Objectives/CollectItemFromDungeon";
    finalizedData.Target = this.Target;
    finalizedData.Count = num;
    finalizedData.TargetLocation = this.TargetLocation;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    finalizedData.CustomTerm = this.CustomTerm;
    return (ObjectivesDataFinalized) finalizedData;
  }

  private void OnItemAddedToInventory(InventoryItem.ITEM_TYPE ItemType, int Delta)
  {
    if (ItemType != this.ItemType || this.TargetLocation != FollowerLocation.Base && PlayerFarming.Location != this.TargetLocation)
      return;
    this.Count += Delta;
  }

  protected override bool CheckComplete()
  {
    int count = this.Count;
    if (this.StartingAmount != -1)
      count -= this.StartingAmount;
    return count >= this.Target;
  }

  public override void Complete()
  {
    base.Complete();
    Inventory.OnItemAddedToInventory -= new Inventory.ItemAddedToInventory(this.OnItemAddedToInventory);
  }

  public override void Failed()
  {
    base.Failed();
    Inventory.OnItemAddedToInventory -= new Inventory.ItemAddedToInventory(this.OnItemAddedToInventory);
  }

  [Serializable]
  public class FinalizedData_CollectItem : ObjectivesDataFinalized
  {
    public InventoryItem.ITEM_TYPE ItemType;
    public FollowerLocation TargetLocation;
    public string LocKey;
    public int Target;
    public int Count;
    public string CustomTerm;

    public override string GetText()
    {
      if (!string.IsNullOrEmpty(this.CustomTerm))
        return LocalizationManager.GetTranslation(this.CustomTerm) + $" {this.Count}/{this.Target}";
      if (this.TargetLocation == FollowerLocation.Base)
        return string.Format(LocalizationManager.GetTranslation(this.LocKey), (object) (InventoryItem.Name(this.ItemType) + FontImageNames.GetIconByType(this.ItemType)), (object) this.Count, (object) this.Target);
      return string.Format(LocalizationManager.GetTranslation(this.LocKey), (object) InventoryItem.Name(this.ItemType), (object) FontImageNames.GetIconByType(this.ItemType), (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"), (object) this.Count, (object) this.Target);
    }
  }
}
