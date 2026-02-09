// Decompiled with JetBrains decompiler
// Type: AmplifyColor.VersionInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace AmplifyColor;

[Serializable]
public class VersionInfo
{
  public const byte Major = 1;
  public const byte Minor = 6;
  public const byte Release = 6;
  public static string StageSuffix = "_dev002";
  public static string TrialSuffix = "";
  [SerializeField]
  public int m_major;
  [SerializeField]
  public int m_minor;
  [SerializeField]
  public int m_release;

  public static string StaticToString()
  {
    return $"{(ValueType) (byte) 1}.{(ValueType) (byte) 6}.{(ValueType) (byte) 6}" + VersionInfo.StageSuffix + VersionInfo.TrialSuffix;
  }

  public override string ToString()
  {
    return $"{this.m_major}.{this.m_minor}.{this.m_release}" + VersionInfo.StageSuffix + VersionInfo.TrialSuffix;
  }

  public int Number => this.m_major * 100 + this.m_minor * 10 + this.m_release;

  public VersionInfo()
  {
    this.m_major = 1;
    this.m_minor = 6;
    this.m_release = 6;
  }

  public VersionInfo(byte major, byte minor, byte release)
  {
    this.m_major = (int) major;
    this.m_minor = (int) minor;
    this.m_release = (int) release;
  }

  public static VersionInfo Current() => new VersionInfo((byte) 1, (byte) 6, (byte) 6);

  public static bool Matches(VersionInfo version)
  {
    return 1 == version.m_major && 6 == version.m_minor && 6 == version.m_release;
  }
}
