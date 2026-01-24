// Decompiled with JetBrains decompiler
// Type: DisableSpineOnPerformanceMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class DisableSpineOnPerformanceMode : MonoBehaviour
{
  public IEnumerator Start()
  {
    DisableSpineOnPerformanceMode onPerformanceMode = this;
    if (SettingsManager.Settings.Game.PerformanceMode)
    {
      yield return (object) new WaitForSeconds(1f);
      SkeletonAnimationLODManager component = onPerformanceMode.GetComponent<SkeletonAnimationLODManager>();
      if ((Object) component != (Object) null)
      {
        component.DisableLODManager(true);
        component.SkeletonAnimation.timeScale = 0.0f;
      }
    }
  }
}
