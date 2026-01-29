// Decompiled with JetBrains decompiler
// Type: Objectives_CollectItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;
using UnityEngine;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_CollectItem : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public InventoryItem.ITEM_TYPE ItemType;
  [Key(17)]
  public FollowerLocation TargetLocation;
  [Key(18)]
  public int Target;
  [Key(19)]
  public int StartingAmount = -1;
  [Key(20)]
  public int Count;
  [Key(21)]
  public string CustomTerm = "";
  [Key(22)]
  public bool countIsTotal;

  public override string Text
  {
    get
    {
      int num = Mathf.Clamp(this.countIsTotal ? this.Count : this.Count - (this.StartingAmount == -1 ? 0 : this.StartingAmount), 0, this.Target);
      string str1 = LocalizeIntegration.ReverseText(num.ToString());
      string str2 = LocalizeIntegration.ReverseText(this.Target.ToString());
      if (this.ItemType == InventoryItem.ITEM_TYPE.YNGYA_GHOST)
        return string.Format(LocalizationManager.GetTranslation("Objectives/DepositLostSouls"), (object) InventoryItem.Name(this.ItemType), (object) FontImageNames.GetIconByType(this.ItemType), (object) DataManager.Instance.ShrineGhostJuice, (object) str2);
      if (!string.IsNullOrEmpty(this.CustomTerm))
        return $"{LocalizationManager.GetTranslation(this.CustomTerm)} {LocalizeIntegration.FormatCurrentMax(num.ToString(), this.Target.ToString())}";
      if (this.TargetLocation == FollowerLocation.Base)
        return string.Format(ScriptLocalization.Objectives.CollectItem, (object) (InventoryItem.Name(this.ItemType) + FontImageNames.GetIconByType(this.ItemType)), (object) str1, (object) str2);
      return string.Format(ScriptLocalization.Objectives.CollectItemFromDungeon, (object) InventoryItem.Name(this.ItemType), (object) FontImageNames.GetIconByType(this.ItemType), (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"), (object) str1, (object) str2);
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
      if (this.ItemType == InventoryItem.ITEM_TYPE.YNGYA_GHOST)
        this.StartingAmount += 4;
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

  public void OnItemAddedToInventory(InventoryItem.ITEM_TYPE ItemType, int Delta)
  {
    if (ItemType == InventoryItem.ITEM_TYPE.YNGYA_GHOST || ItemType != this.ItemType || this.TargetLocation != FollowerLocation.Base && PlayerFarming.Location != this.TargetLocation)
      return;
    this.Count += Delta;
  }

  public void IncreaseCount() => ++this.Count;

  public override bool CheckComplete()
  {
    int num = this.Count;
    if (this.ItemType == InventoryItem.ITEM_TYPE.YNGYA_GHOST)
      num = this.Count = DataManager.Instance.ShrineGhostJuice;
    else if (this.StartingAmount != -1)
      num -= this.StartingAmount;
    return num >= this.Target;
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

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_CollectItem : ObjectivesDataFinalized
  {
    [Key(3)]
    public InventoryItem.ITEM_TYPE ItemType;
    [Key(4)]
    public FollowerLocation TargetLocation;
    [Key(5)]
    public string LocKey;
    [Key(6)]
    public int Target;
    [Key(7)]
    public int Count;
    [Key(8)]
    public string CustomTerm;

    public override string GetText()
    {
      int count = this.Count;
      string str1 = LocalizeIntegration.ReverseText(count.ToString());
      string str2 = LocalizeIntegration.ReverseText(this.Target.ToString());
      if (this.ItemType == InventoryItem.ITEM_TYPE.YNGYA_GHOST)
        return string.Format(LocalizationManager.GetTranslation("Objectives/DepositLostSouls"), (object) InventoryItem.Name(this.ItemType), (object) FontImageNames.GetIconByType(this.ItemType), (object) str2, (object) str2);
      if (!string.IsNullOrEmpty(this.CustomTerm))
        return $"{LocalizationManager.GetTranslation(this.CustomTerm)} {LocalizeIntegration.FormatCurrentMax(count.ToString(), this.Target.ToString())}";
      if (this.TargetLocation == FollowerLocation.Base)
        return string.Format(LocalizationManager.GetTranslation(this.LocKey), (object) (InventoryItem.Name(this.ItemType) + FontImageNames.GetIconByType(this.ItemType)), (object) str1, (object) str2);
      return string.Format(LocalizationManager.GetTranslation(this.LocKey), (object) InventoryItem.Name(this.ItemType), (object) FontImageNames.GetIconByType(this.ItemType), (object) LocalizationManager.GetTranslation($"NAMES/Places/{this.TargetLocation}"), (object) str1, (object) str2);
    }
  }
}
