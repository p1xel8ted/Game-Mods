// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.UnityUserReportParser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin;

public static class UnityUserReportParser
{
  public static UserReport ParseUserReport(string json)
  {
    return Unity.Cloud.UserReporting.Plugin.SimpleJson.SimpleJson.DeserializeObject<UserReport>(json);
  }

  public static UserReportList ParseUserReportList(string json)
  {
    return Unity.Cloud.UserReporting.Plugin.SimpleJson.SimpleJson.DeserializeObject<UserReportList>(json);
  }
}
