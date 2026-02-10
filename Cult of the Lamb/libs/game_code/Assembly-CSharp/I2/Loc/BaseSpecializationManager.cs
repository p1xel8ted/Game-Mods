// Decompiled with JetBrains decompiler
// Type: I2.Loc.BaseSpecializationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
