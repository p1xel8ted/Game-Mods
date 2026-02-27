// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.UserReportNamedValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Unity.Cloud.UserReporting;

public struct UserReportNamedValue(string name, string value)
{
  public string Name { get; set; } = name;

  public string Value { get; set; } = value;
}
