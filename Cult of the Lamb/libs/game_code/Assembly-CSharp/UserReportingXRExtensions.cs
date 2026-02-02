// Decompiled with JetBrains decompiler
// Type: UserReportingXRExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;
using UnityEngine.XR;

#nullable disable
public class UserReportingXRExtensions : MonoBehaviour
{
  public static bool XRIsPresent()
  {
    List<XRDisplaySubsystem> subsystems = new List<XRDisplaySubsystem>();
    SubsystemManager.GetInstances<XRDisplaySubsystem>((List<XRDisplaySubsystem>) subsystems);
    foreach (IntegratedSubsystem integratedSubsystem in subsystems)
    {
      if (integratedSubsystem.running)
        return true;
    }
    return false;
  }

  public void Start()
  {
    if (!UserReportingXRExtensions.XRIsPresent())
      return;
    UnityUserReporting.CurrentClient.AddDeviceMetadata("XRDeviceModel", XRSettings.loadedDeviceName);
  }

  public void Update()
  {
    if (!UserReportingXRExtensions.XRIsPresent())
      return;
    int droppedFrameCount;
    if (XRStats.TryGetDroppedFrameCount(out droppedFrameCount))
      UnityUserReporting.CurrentClient.SampleMetric("XR.DroppedFrameCount", (double) droppedFrameCount);
    int framePresentCount;
    if (XRStats.TryGetFramePresentCount(out framePresentCount))
      UnityUserReporting.CurrentClient.SampleMetric("XR.FramePresentCount", (double) framePresentCount);
    float gpuTimeLastFrame;
    if (!XRStats.TryGetGPUTimeLastFrame(out gpuTimeLastFrame))
      return;
    UnityUserReporting.CurrentClient.SampleMetric("XR.GPUTimeLastFrame", (double) gpuTimeLastFrame);
  }
}
