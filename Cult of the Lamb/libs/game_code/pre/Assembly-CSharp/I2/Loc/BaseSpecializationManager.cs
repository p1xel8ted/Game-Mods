// Decompiled with JetBrains decompiler
// Type: I2.Loc.BaseSpecializationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace I2.Loc;

public class BaseSpecializationManager
{
  public string[] mSpecializations;
  public Dictionary<string, string> mSpecializationsFallbacks;

  public virtual void InitializeSpecializations()
  {
    this.mSpecializations = new string[12]
    {
      "Any",
      "PC",
      "Touch",
      "Controller",
      "VR",
      "XBox",
      "PS4",
      "OculusVR",
      "ViveVR",
      "GearVR",
      "Android",
      "IOS"
    };
    this.mSpecializationsFallbacks = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "XBox",
        "Controller"
      },
      {
        "PS4",
        "Controller"
      },
      {
        "OculusVR",
        "VR"
      },
      {
        "ViveVR",
        "VR"
      },
      {
        "GearVR",
        "VR"
      },
      {
        "Android",
        "Touch"
      },
      {
        "IOS",
        "Touch"
      }
    };
  }

  public virtual string GetCurrentSpecialization()
  {
    if (this.mSpecializations == null)
      this.InitializeSpecializations();
    return "PC";
  }

  public virtual string GetFallbackSpecialization(string specialization)
  {
    if (this.mSpecializationsFallbacks == null)
      this.InitializeSpecializations();
    string str;
    return this.mSpecializationsFallbacks.TryGetValue(specialization, out str) ? str : "Any";
  }
}
