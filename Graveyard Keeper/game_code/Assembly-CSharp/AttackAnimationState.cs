// Decompiled with JetBrains decompiler
// Type: AttackAnimationState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AttackAnimationState : StateMachineBehaviour
{
  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    WorldObjectPart component = animator.gameObject.GetComponent<WorldObjectPart>();
    if ((Object) component == (Object) null)
    {
      Debug.LogError((object) "Non found WOP!");
    }
    else
    {
      component.parent.components.character.attack.AttackAnimationEnded();
      Debug.Log((object) $"[{component.name}] Attack animation ended!");
    }
  }
}
