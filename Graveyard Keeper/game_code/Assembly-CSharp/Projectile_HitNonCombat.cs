// Decompiled with JetBrains decompiler
// Type: Projectile_HitNonCombat
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Projectile_HitNonCombat : StateMachineBehaviour
{
  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    animator.gameObject.GetComponent<ProjectileObjectPart>().on_hit_non_combat.TryInvoke();
  }
}
