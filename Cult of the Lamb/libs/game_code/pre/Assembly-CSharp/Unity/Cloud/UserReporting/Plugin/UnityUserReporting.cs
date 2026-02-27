// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.UnityUserReporting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Unity.Cloud.UserReporting.Client;
using UnityEngine;

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin;

public static class UnityUserReporting
{
  private static UserReportingClient currentClient;

  public static UserReportingClient CurrentClient
  {
    get
    {
      if (UnityUserReporting.currentClient == null)
        UnityUserReporting.Configure();
      return UnityUserReporting.currentClient;
    }
    private set => UnityUserReporting.currentClient = value;
  }

  public static void Configure(
    string endpoint,
    string projectIdentifier,
    IUserReportingPlatform platform,
    UserReportingClientConfiguration configuration)
  {
    UnityUserReporting.CurrentClient = new UserReportingClient(endpoint, projectIdentifier, platform, configuration);
  }

  public static void Configure(
    string endpoint,
    string projectIdentifier,
    UserReportingClientConfiguration configuration)
  {
    UnityUserReporting.CurrentClient = new UserReportingClient(endpoint, projectIdentifier, UnityUserReporting.GetPlatform(), configuration);
  }

  public static void Configure(
    string projectIdentifier,
    UserReportingClientConfiguration configuration)
  {
    UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, UnityUserReporting.GetPlatform(), configuration);
  }

  public static void Configure(string projectIdentifier)
  {
    UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, UnityUserReporting.GetPlatform(), new UserReportingClientConfiguration());
  }

  public static void Configure()
  {
    UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, UnityUserReporting.GetPlatform(), new UserReportingClientConfiguration());
  }

  public static void Configure(UserReportingClientConfiguration configuration)
  {
    UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, UnityUserReporting.GetPlatform(), configuration);
  }

  public static void Configure(
    string projectIdentifier,
    IUserReportingPlatform platform,
    UserReportingClientConfiguration configuration)
  {
    UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", projectIdentifier, platform, configuration);
  }

  public static void Configure(
    IUserReportingPlatform platform,
    UserReportingClientConfiguration configuration)
  {
    UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, platform, configuration);
  }

  public static void Configure(IUserReportingPlatform platform)
  {
    UnityUserReporting.Configure("https://userreporting.cloud.unity3d.com", Application.cloudProjectId, platform, new UserReportingClientConfiguration());
  }

  private static IUserReportingPlatform GetPlatform()
  {
    return (IUserReportingPlatform) new UnityUserReportingPlatform();
  }

  public static void Use(UserReportingClient client)
  {
    if (client == null)
      return;
    UnityUserReporting.CurrentClient = client;
  }
}
