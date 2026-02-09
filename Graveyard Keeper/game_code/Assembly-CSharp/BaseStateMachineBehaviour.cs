// Decompiled with JetBrains decompiler
// Type: BaseStateMachineBehaviour
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BaseStateMachineBehaviour : StateMachineBehaviour
{
  public Animator cached_animator;
  public AnimatedBehaviour behaviour;
  public float normalized_time;
  public bool waiting_first_loop;
  public bool behaviour_cached;
  public int loop_count;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    base.OnStateEnter(animator, stateInfo, layerIndex);
    if (this.behaviour_cached || this.CheckAnimator(animator))
      this.behaviour.OnEnter();
    this.normalized_time = 0.0f;
    this.loop_count = 0;
    this.waiting_first_loop = true;
  }

  public override void OnStateUpdate(
    Animator animator,
    AnimatorStateInfo stateInfo,
    int layerIndex)
  {
    base.OnStateUpdate(animator, stateInfo, layerIndex);
    if (!this.behaviour_cached && !this.CheckAnimator(animator))
      return;
    float num1 = Mathf.Repeat(stateInfo.normalizedTime, 1f);
    int num2 = Mathf.FloorToInt(stateInfo.normalizedTime);
    if ((double) num1 < (double) this.normalized_time || (num1 * stateInfo.length).EqualsTo(stateInfo.length, Time.deltaTime) || num2 > this.loop_count)
    {
      this.normalized_time = 0.0f;
      ++this.loop_count;
      this.OnLoop();
    }
    else
      this.normalized_time = num1;
    this.behaviour.OnUpdate(this.normalized_time);
  }

  public virtual void OnLoop()
  {
    this.behaviour.OnLoop();
    if (!this.waiting_first_loop)
      return;
    this.behaviour.OnFirstLoop();
    this.waiting_first_loop = false;
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    base.OnStateExit(animator, stateInfo, layerIndex);
    if (this.behaviour_cached || this.CheckAnimator(animator))
      this.behaviour.OnExit();
    if (!this.waiting_first_loop)
      return;
    this.OnLoop();
  }

  public bool CheckAnimator(Animator animator)
  {
    if ((Object) this.cached_animator != (Object) animator)
    {
      this.cached_animator = animator;
      this.behaviour = (animator.GetComponent<WorldGameObject>() ?? animator.GetComponentInParent<WorldGameObject>() ?? animator.GetComponentInChildren<WorldGameObject>()).components.animated_behaviour;
    }
    this.behaviour_cached = this.behaviour != null;
    return this.behaviour_cached;
  }
}
