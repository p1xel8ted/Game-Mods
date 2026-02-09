// Decompiled with JetBrains decompiler
// Type: OnPulpitPrayAnimationState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class OnPulpitPrayAnimationState : StateMachineBehaviour
{
  public OnPulpitPrayAnimationState.Type type;

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    ChurchPulpit component = animator.gameObject.GetComponent<ChurchPulpit>();
    if ((Object) component == (Object) null)
    {
      Debug.LogError((object) ("Not found ChurchPulpit for " + animator.gameObject.name), (Object) animator);
    }
    else
    {
      switch (this.type)
      {
        case OnPulpitPrayAnimationState.Type.EndOfAnimation:
          component.OnPrayAnimationDone();
          break;
        case OnPulpitPrayAnimationState.Type.MiddleOfAnimation:
          component.OnMiddleAnimation();
          break;
      }
    }
  }

  public enum Type
  {
    EndOfAnimation,
    MiddleOfAnimation,
  }
}
