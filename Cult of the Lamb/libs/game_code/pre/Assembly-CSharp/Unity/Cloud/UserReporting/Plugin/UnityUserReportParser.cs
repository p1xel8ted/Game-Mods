// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.UnityUserReportParser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
