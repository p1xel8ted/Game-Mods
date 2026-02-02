// Decompiled with JetBrains decompiler
// Type: src.Alerts.ClothingCustomiseAlerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace src.Alerts;

[MessagePackObject(false)]
[Serializable]
public class ClothingCustomiseAlerts : AlertCategory<FollowerClothingType>
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
