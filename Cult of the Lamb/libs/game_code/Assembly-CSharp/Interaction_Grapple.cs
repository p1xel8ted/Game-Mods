// Decompiled with JetBrains decompiler
// Type: Interaction_Grapple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class Interaction_Grapple : Interaction
{
  public SkeletonAnimation Spine;
  public Interaction_Grapple TargetGrapple;
  public Transform BoneTarget;
  public LineRenderer lineRenderer;
  public string sGrapple;
  public string sRequiresGrapple;

  public override void GetLabel()
  {
    if (CrownAbilities.CrownAbilityUnlocked(CrownAbilities.TYPE.Abilities_GrappleHook))
    {
      this.Interactable = true;
      this.Label = this.sGrapple;
    }
    else
    {
      this.Interactable = false;
      this.Label = this.sRequiresGrapple;
    }
  }

  public void Start()
  {
    this.UpdateLocalisation();
    if ((Object) this.TargetGrapple == (Object) null)
      return;
    this.lineRenderer.SetPosition(0, this.BoneTarget.position);
    this.lineRenderer.SetPosition(1, this.TargetGrapple.BoneTarget.position);
    if ((double) this.TargetGrapple.transform.position.x < (double) this.transform.position.x)
      this.lineRenderer.gameObject.SetActive(false);
    switch (Utils.GetAngleDirection(Utils.GetAngle(this.transform.position, this.TargetGrapple.transform.position)))
    {
      case Utils.Direction.Up:
        this.Spine.skeleton.SetSkin("up");
        break;
      case Utils.Direction.Down:
        this.Spine.skeleton.SetSkin("down");
        break;
      case Utils.Direction.Left:
        this.Spine.skeleton.SetSkin("left");
        break;
      case Utils.Direction.Right:
        this.Spine.skeleton.SetSkin("right");
        break;
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sRequiresGrapple = "Requires grapple hook - needs loc";
  }

  public void BecomeTarget()
  {
    this.Spine.AnimationState.SetAnimation(0, "hit", false);
    this.Spine.AnimationState.AddAnimation(0, "target", true, 0.0f);
  }

  public void BecomeOrigin()
  {
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    state.transform.position = this.transform.position;
    state.gameObject.GetComponent<PlayerController>().DoGrapple(this.TargetGrapple);
  }

  public new void OnDrawGizmos()
  {
    if (!((Object) this.TargetGrapple != (Object) null))
      return;
    Utils.DrawLine(this.transform.position, this.TargetGrapple.transform.position, Color.yellow);
  }
}
