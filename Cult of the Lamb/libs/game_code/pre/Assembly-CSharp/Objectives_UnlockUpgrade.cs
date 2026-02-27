// Decompiled with JetBrains decompiler
// Type: Objectives_UnlockUpgrade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;

#nullable disable
[Serializable]
public class Objectives_UnlockUpgrade : ObjectivesData
{
  public UpgradeSystem.Type UnlockType;

  public override string Text
  {
    get => LocalizationManager.GetTranslation($"Objectives/Unlock/{this.UnlockType.ToString()}");
  }

  public Objectives_UnlockUpgrade()
  {
  }

  public Objectives_UnlockUpgrade(string groupId, UpgradeSystem.Type unlockType)
    : base(groupId)
  {
    this.Type = Objectives.TYPES.UNLOCK;
    this.UnlockType = unlockType;
    UpgradeSystem.OnUpgradeUnlocked += new UpgradeSystem.UnlockEvent(this.UpgradeSystem_OnUpgradeUnlocked);
  }

  public override void Init(bool initialAssigning)
  {
    UpgradeSystem.OnUpgradeUnlocked += new UpgradeSystem.UnlockEvent(this.UpgradeSystem_OnUpgradeUnlocked);
    base.Init(initialAssigning);
  }

  public override ObjectivesDataFinalized GetFinalizedData()
  {
    Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade finalizedData = new Objectives_UnlockUpgrade.FinalizedData_UnlockUpgrade();
    finalizedData.GroupId = this.GroupId;
    finalizedData.Index = this.Index;
    finalizedData.UnlockType = this.UnlockType;
    finalizedData.UniqueGroupID = this.UniqueGroupID;
    return (ObjectivesDataFinalized) finalizedData;
  }

  private void UpgradeSystem_OnUpgradeUnlocked(UpgradeSystem.Type upgradeType)
  {
    if (upgradeType != this.UnlockType)
      return;
    ObjectiveManager.UpdateObjective((ObjectivesData) this);
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.UpgradeSystem_OnUpgradeUnlocked);
  }

  protected override bool CheckComplete() => UpgradeSystem.GetUnlocked(this.UnlockType);

  [Serializable]
  public class FinalizedData_UnlockUpgrade : ObjectivesDataFinalized
  {
    public UpgradeSystem.Type UnlockType;

    public override string GetText()
    {
      return LocalizationManager.GetTranslation($"Objectives/Unlock/{this.UnlockType.ToString()}");
    }
  }
}
