// Decompiled with JetBrains decompiler
// Type: CoroutineStatics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public static class CoroutineStatics
{
  public static IEnumerator WaitForScaledSeconds(float seconds, SkeletonAnimation spine)
  {
    double num1;
    double num2;
    for (float timer = seconds; (double) timer >= 0.0; timer = (float) (num1 - num2))
    {
      yield return (object) null;
      num1 = (double) timer;
      double deltaTime = (double) Time.deltaTime;
      SkeletonAnimation skeletonAnimation = spine;
      double num3 = skeletonAnimation != null ? (double) skeletonAnimation.timeScale : 1.0;
      num2 = deltaTime * num3;
    }
  }
}
