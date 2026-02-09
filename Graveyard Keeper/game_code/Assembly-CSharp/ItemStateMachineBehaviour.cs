// Decompiled with JetBrains decompiler
// Type: ItemStateMachineBehaviour
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ItemStateMachineBehaviour : BaseStateMachineBehaviour
{
  public ItemDefinition.ItemType item_type;
  public bool failed;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    base.OnStateEnter(animator, stateInfo, layerIndex);
    if (!this.behaviour_cached && !this.CheckAnimator(animator))
      return;
    this.behaviour.OnItemStart(this.item_type);
  }

  public override void OnLoop()
  {
    this.behaviour.OnItemLoop(this.item_type, this.failed);
    if (this.waiting_first_loop)
      this.behaviour.OnItemFirstLoop(this.item_type, this.failed);
    base.OnLoop();
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (this.behaviour_cached || this.CheckAnimator(animator))
      this.behaviour.OnItemStop(this.item_type);
    base.OnStateExit(animator, stateInfo, layerIndex);
  }
}
