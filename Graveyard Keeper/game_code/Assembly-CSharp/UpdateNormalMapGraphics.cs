// Decompiled with JetBrains decompiler
// Type: UpdateNormalMapGraphics
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UpdateNormalMapGraphics : StateMachineBehaviour
{
  [SerializeField]
  public bool is_update_on_enter;
  [SerializeField]
  public bool is_update_on_exit;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.UpdateNormalMaps(animator);
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.UpdateNormalMaps(animator);
  }

  public void UpdateNormalMaps(Animator animator)
  {
    foreach (MaterialPropertyModifier componentsInChild in animator.gameObject.GetComponentsInChildren<NormalMapSprite>(true))
      componentsInChild.UpdateRenderer();
  }
}
