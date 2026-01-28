// Decompiled with JetBrains decompiler
// Type: src.Alerts.RelicAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
