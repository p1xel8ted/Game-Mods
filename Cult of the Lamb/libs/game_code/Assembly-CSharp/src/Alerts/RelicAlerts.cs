// Decompiled with JetBrains decompiler
// Type: src.Alerts.RelicAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class RelicAlerts : AlertCategory<RelicType>
{
  [IgnoreMember]
  public UpgradeSystem.Type[] _allRituals;

  public RelicAlerts()
  {
    EquipmentManager.OnRelicUnlocked += new Action<RelicType>(this.OnRelicUnlocked);
  }

  void object.Finalize()
  {
    try
    {
      EquipmentManager.OnRelicUnlocked -= new Action<RelicType>(this.OnRelicUnlocked);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void OnRelicUnlocked(RelicType relicType) => this.AddOnce(relicType);
}
