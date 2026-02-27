// Decompiled with JetBrains decompiler
// Type: AnimateOnTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class AnimateOnTrigger : BaseMonoBehaviour
{
  public float ActivateDistance = 4f;
  public Vector3 ActivateOffset = Vector3.zero;
  public SkeletonAnimation Spine;
  public bool HideBeforeTriggered = true;
  [SpineAnimation("", "Spine", true, false)]
  public string InitialAnimation;
  [SpineAnimation("", "Spine", true, false)]
  public string TriggeredAnimation;
  [SpineAnimation("", "Spine", true, false)]
  public string EndOnAnimation;
  private bool Activated;
  private GameObject Player;
  private StateMachine state;

  private void Start()
  {
    if (this.HideBeforeTriggered)
      this.Spine.gameObject.SetActive(false);
    else
      this.Spine.AnimationState.SetAnimation(0, this.InitialAnimation, false);
  }

  private void Update()
  {
    if ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null || this.Activated || (double) Vector3.Distance(this.transform.position + this.ActivateOffset, this.Player.transform.position) >= (double) this.ActivateDistance)
      return;
    this.Activated = true;
    this.Spine.gameObject.SetActive(true);
    this.Spine.AnimationState.SetAnimation(0, this.TriggeredAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.EndOnAnimation, false, 0.0f);
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.green);
  }
}
