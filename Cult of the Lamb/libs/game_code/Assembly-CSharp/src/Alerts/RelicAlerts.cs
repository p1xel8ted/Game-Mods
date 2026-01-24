// Decompiled with JetBrains decompiler
// Type: src.Alerts.RelicAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
