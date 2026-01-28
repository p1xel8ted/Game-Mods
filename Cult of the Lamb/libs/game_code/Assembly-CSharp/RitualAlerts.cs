// Decompiled with JetBrains decompiler
// Type: RitualAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class RitualAlerts : AlertCategory<UpgradeSystem.Type>
{
  [IgnoreMember]
  public UpgradeSystem.Type[] _allRituals;

  public RitualAlerts()
  {
    this._allRituals = UpgradeSystem.AllRituals();
    UpgradeSystem.OnAbilityUnlocked += new Action<UpgradeSystem.Type>(this.OnRitualAdded);
    UpgradeSystem.OnAbilityLocked += new Action<UpgradeSystem.Type>(this.OnRitualRemoved);
  }

  void object.Finalize()
  {
    try
    {
      this._allRituals = (UpgradeSystem.Type[]) null;
      UpgradeSystem.OnAbilityUnlocked -= new Action<UpgradeSystem.Type>(this.OnRitualAdded);
      UpgradeSystem.OnAbilityLocked -= new Action<UpgradeSystem.Type>(this.OnRitualRemoved);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnRitualAdded(UpgradeSystem.Type upgradeType)
  {
    if (!this._allRituals.Contains<UpgradeSystem.Type>(upgradeType))
      return;
    this.AddOnce(upgradeType);
  }

  public void OnRitualRemoved(UpgradeSystem.Type upgradeType) => this.Remove(upgradeType);
}
