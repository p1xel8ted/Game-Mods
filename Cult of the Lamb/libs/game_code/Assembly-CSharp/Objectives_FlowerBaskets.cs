// Decompiled with JetBrains decompiler
// Type: Objectives_FlowerBaskets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class Objectives_FlowerBaskets : ObjectivesData
{
  [Key(16 /*0x10*/)]
  public int FilledPots;
  [Key(17)]
  public int PotsToFill;
  [IgnoreMember]
  public bool complete;

  public override string Text
  {
    get
    {
      this.FilledPots = DataManager.GetNumberOfFullFlowerPots();
      this.PotsToFill = 10;
      return string.Format(LocalizationManager.GetTranslation("Objectives/FlowerBaskets"), (object) this.FilledPots, (object) this.PotsToFill);
    }
  }

  public Objectives_FlowerBaskets()
  {
  }

  public Objectives_FlowerBaskets(string groupId, float questExpireDuration = -1f)
    : base(groupId, questExpireDuration)
  {
    this.Type = Objectives.TYPES.FLOWER_BASKETS;
    this.FilledPots = DataManager.GetNumberOfFullFlowerPots();
    this.PotsToFill = 10;
    this.IsWinterObjective = true;
  }

  public override void Init(bool initialAssigning)
  {
    base.Init(initialAssigning);
    this.complete = false;
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_FlowerBaskets.FinalizedData_FlowerBaskets finalizedData = new Objectives_FlowerBaskets.FinalizedData_FlowerBaskets();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.FilledPots = this.FilledPots;
    finalizedData.PotsToFill = this.PotsToFill;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  public void PotFillCheck()
  {
    if (DataManager.GetNumberOfFullFlowerPots() < 10)
      return;
    this.complete = true;
  }

  public override bool CheckComplete()
  {
    this.PotFillCheck();
    return this.complete;
  }

  public override void Complete()
  {
    base.Complete();
    DataManager.Instance.HasFinishedYngyaFlowerBasketQuest = true;
  }

  [MessagePackObject(false)]
  [Serializable]
  public class FinalizedData_FlowerBaskets : ObjectivesDataFinalized
  {
    [Key(3)]
    public int FilledPots;
    [Key(4)]
    public int PotsToFill;

    public override string GetText()
    {
      return string.Format(LocalizationManager.GetTranslation("Objectives/FlowerBaskets"), (object) this.FilledPots, (object) this.PotsToFill);
    }
  }
}
