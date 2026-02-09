// Decompiled with JetBrains decompiler
// Type: AnimateOnTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public bool Activated;
  public GameObject Player;
  public StateMachine state;

  public void Start()
  {
    if (this.HideBeforeTriggered)
      this.Spine.gameObject.SetActive(false);
    else
      this.Spine.AnimationState.SetAnimation(0, this.InitialAnimation, false);
  }

  public void Update()
  {
    if ((Object) (this.Player = GameObject.FindWithTag("Player")) == (Object) null || this.Activated || (double) Vector3.Distance(this.transform.position + this.ActivateOffset, this.Player.transform.position) >= (double) this.ActivateDistance)
      return;
    this.Activated = true;
    this.Spine.gameObject.SetActive(true);
    this.Spine.AnimationState.SetAnimation(0, this.TriggeredAnimation, false);
    this.Spine.AnimationState.AddAnimation(0, this.EndOnAnimation, false, 0.0f);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.green);
  }
}
