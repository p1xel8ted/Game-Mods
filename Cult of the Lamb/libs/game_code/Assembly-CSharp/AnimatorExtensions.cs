// Decompiled with JetBrains decompiler
// Type: AnimatorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public static class AnimatorExtensions
{
  public static IEnumerator YieldForAnimation(this Animator animator, string stateName)
  {
    animator.Play(stateName);
    yield return (object) null;
    float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
    while ((double) (animationDuration -= Time.unscaledDeltaTime) > 0.0)
      yield return (object) null;
  }

  public static void ResetAllTriggers(this Animator animator)
  {
    foreach (AnimatorControllerParameter parameter in animator.parameters)
    {
      if (parameter.type == AnimatorControllerParameterType.Trigger)
        animator.ResetTrigger(parameter.name);
    }
  }
}
