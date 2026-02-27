// Decompiled with JetBrains decompiler
// Type: UserReportingXRExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;
using UnityEngine.XR;

#nullable disable
public class UserReportingXRExtensions : MonoBehaviour
{
  private static bool XRIsPresent()
  {
    List<XRDisplaySubsystem> instances = new List<XRDisplaySubsystem>();
    SubsystemManager.GetInstances<XRDisplaySubsystem>((List<XRDisplaySubsystem>) instances);
    foreach (IntegratedSubsystem integratedSubsystem in instances)
    {
      if (integratedSubsystem.running)
        return true;
    }
    return false;
  }

  private void Start()
  {
    if (!UserReportingXRExtensions.XRIsPresent())
      return;
    UnityUserReporting.CurrentClient.AddDeviceMetadata("XRDeviceModel", XRSettings.loadedDeviceName);
  }

  private void Update()
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
