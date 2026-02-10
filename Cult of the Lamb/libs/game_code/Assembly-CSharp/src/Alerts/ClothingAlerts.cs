// Decompiled with JetBrains decompiler
// Type: src.Alerts.ClothingAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class ClothingAlerts : AlertCategory<FollowerClothingType>
{
  void object.Finalize()
  {
    try
    {
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }
}
