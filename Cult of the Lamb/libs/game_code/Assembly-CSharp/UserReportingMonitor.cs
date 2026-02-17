// Decompiled with JetBrains decompiler
// Type: UserReportingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

#nullable disable
public class UserReportingMonitor : MonoBehaviour
{
  public bool IsEnabled;
  public bool IsEnabledAfterTrigger;
  public bool IsHiddenWithoutDimension;
  public string MonitorName;
  public string Summary;

  public UserReportingMonitor()
  {
    this.IsEnabled = true;
    this.IsHiddenWithoutDimension = true;
    this.MonitorName = ((object) this).GetType().Name;
  }

  public void Start()
  {
    if (UnityUserReporting.CurrentClient != null)
      return;
    UnityUserReporting.Configure();
  }

  public void Trigger()
  {
    if (!this.IsEnabledAfterTrigger)
      this.IsEnabled = false;
    UnityUserReporting.CurrentClient.TakeScreenshot(2048 /*0x0800*/, 2048 /*0x0800*/, (Action<UserReportScreenshot>) (s => { }));
    UnityUserReporting.CurrentClient.TakeScreenshot(512 /*0x0200*/, 512 /*0x0200*/, (Action<UserReportScreenshot>) (s => { }));
    UnityUserReporting.CurrentClient.CreateUserReport((Action<UserReport>) (br =>
    {
      if (string.IsNullOrEmpty(br.ProjectIdentifier))
        Debug.LogWarning((object) "The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
      br.Summary = this.Summary;
      br.DeviceMetadata.Add(new UserReportNamedValue("Monitor", this.MonitorName));
      string str1 = "Unknown";
      string str2 = "0.0";
      foreach (UserReportNamedValue reportNamedValue in br.DeviceMetadata)
      {
        if (reportNamedValue.Name == "Platform")
          str1 = reportNamedValue.Value;
        if (reportNamedValue.Name == "Version")
          str2 = reportNamedValue.Value;
      }
      br.Dimensions.Add(new UserReportNamedValue("Monitor.Platform.Version", $"{this.MonitorName}.{str1}.{str2}"));
      br.Dimensions.Add(new UserReportNamedValue("Monitor", this.MonitorName));
      br.IsHiddenWithoutDimension = this.IsHiddenWithoutDimension;
      UnityUserReporting.CurrentClient.SendUserReport(br, (Action<bool, UserReport>) ((success, br2) => this.Triggered()));
    }));
  }

  public virtual void Triggered()
  {
  }

  [CompilerGenerated]
  public void \u003CTrigger\u003Eb__7_2(UserReport br)
  {
    if (string.IsNullOrEmpty(br.ProjectIdentifier))
      Debug.LogWarning((object) "The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
    br.Summary = this.Summary;
    br.DeviceMetadata.Add(new UserReportNamedValue("Monitor", this.MonitorName));
    string str1 = "Unknown";
    string str2 = "0.0";
    foreach (UserReportNamedValue reportNamedValue in br.DeviceMetadata)
    {
      if (reportNamedValue.Name == "Platform")
        str1 = reportNamedValue.Value;
      if (reportNamedValue.Name == "Version")
        str2 = reportNamedValue.Value;
    }
    br.Dimensions.Add(new UserReportNamedValue("Monitor.Platform.Version", $"{this.MonitorName}.{str1}.{str2}"));
    br.Dimensions.Add(new UserReportNamedValue("Monitor", this.MonitorName));
    br.IsHiddenWithoutDimension = this.IsHiddenWithoutDimension;
    UnityUserReporting.CurrentClient.SendUserReport(br, (Action<bool, UserReport>) ((success, br2) => this.Triggered()));
  }

  [CompilerGenerated]
  public void \u003CTrigger\u003Eb__7_3(bool success, UserReport br2) => this.Triggered();
}
