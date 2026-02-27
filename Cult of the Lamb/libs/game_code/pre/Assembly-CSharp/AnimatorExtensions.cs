// Decompiled with JetBrains decompiler
// Type: AnimatorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
