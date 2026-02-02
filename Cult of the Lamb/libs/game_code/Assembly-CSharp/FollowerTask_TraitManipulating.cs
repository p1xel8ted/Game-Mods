// Decompiled with JetBrains decompiler
// Type: FollowerTask_TraitManipulating
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerTask_TraitManipulating : FollowerTask
{
  public Structures_TraitManipulator structureBrain;
  public Coroutine _dissentBubbleCoroutine;
  public Follower follower;
  public EventInstance loopingSound;

  public override FollowerTaskType Type => FollowerTaskType.TraitManipulating;

  public override FollowerLocation Location => FollowerLocation.Base;

  public override bool DisablePickUpInteraction => true;

  public override bool BlockTaskChanges => true;

  public override bool BlockReactTasks => true;

  public override bool BlockSocial => true;

  public override PriorityCategory GetPriorityCategory(
    FollowerRole FollowerRole,
    WorkerPriority WorkerPriority,
    FollowerBrain brain)
  {
    return PriorityCategory.ExtremelyUrgent;
  }

  public FollowerTask_TraitManipulating(int brainID) => this.GetBrain(brainID);

  public void GetBrain(int brainID)
  {
    foreach (Structures_TraitManipulator traitManipulator in StructureManager.GetAllStructuresOfType<Structures_TraitManipulator>())
    {
      if (traitManipulator.Data.FollowerID == brainID)
      {
        this.structureBrain = traitManipulator;
        break;
      }
    }
  }

  public override int GetSubTaskCode()
  {
    return this.structureBrain == null ? 0 : this.structureBrain.Data.ID;
  }

  public override void TaskTick(float deltaGameTime)
  {
    if (this.structureBrain != null && this._dissentBubbleCoroutine == null && (double) this.structureBrain.Data.Progress >= (double) this.structureBrain.Duration)
    {
      Follower followerById = FollowerManager.FindFollowerByID(this.Brain.Info.ID);
      if ((UnityEngine.Object) followerById != (UnityEngine.Object) null && followerById.gameObject.activeInHierarchy)
      {
        if (followerById.Spine.AnimationState.GetCurrent(1).Animation.Name != "Rituals/lobotomy-rehabilitated-idle")
          followerById.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Rituals/lobotomy-rehabilitated-idle");
        AudioManager.Instance.StopLoop(this.loopingSound);
        this._dissentBubbleCoroutine = followerById.StartCoroutine((IEnumerator) this.DissentBubbleRoutine(followerById));
      }
    }
    if (!((UnityEngine.Object) this.follower != (UnityEngine.Object) null) || !this.follower.gameObject.activeInHierarchy)
      return;
    if (this.structureBrain == null)
    {
      this.GetBrain(this.Brain.Info.ID);
      if (this.structureBrain == null)
      {
        this.End();
        return;
      }
    }
    this.follower.transform.position = this.UpdateDestination(this.follower);
  }

  public override Vector3 UpdateDestination(Follower follower)
  {
    return this.structureBrain != null && (UnityEngine.Object) this.GetBuilding() != (UnityEngine.Object) null ? this.GetBuilding().TargetFollowerPosition.transform.position : Vector3.zero;
  }

  public override void Setup(Follower follower)
  {
    base.Setup(follower);
    this.follower = follower;
    follower.LockToGround = false;
    if ((UnityEngine.Object) this.GetBuilding() != (UnityEngine.Object) null)
      follower.transform.position = this.GetBuilding().TargetFollowerPosition.transform.position;
    this.SetAnims(follower);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => this.SetAnims(follower)));
  }

  public override void Cleanup(Follower follower)
  {
    base.Cleanup(follower);
    follower.LockToGround = true;
    if (this._dissentBubbleCoroutine != null)
    {
      follower.StopCoroutine(this._dissentBubbleCoroutine);
      this._dissentBubbleCoroutine = (Coroutine) null;
    }
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    AudioManager.Instance.StopLoop(this.loopingSound);
    FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain.Info.XPLevel, follower.Brain.Info.SkinName, follower.Brain.Info.SkinColour, follower.Brain.Info.Outfit, follower.Brain.Info.Hat, follower.Brain.Info.Clothing, follower.Brain.Info.Customisation, follower.Brain.Info.Special, follower.Brain.Info.Necklace, follower.Brain.Info.ClothingVariant, follower.Brain._directInfoAccess);
  }

  public override void OnEnd()
  {
    base.OnEnd();
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override void OnAbort()
  {
    base.OnAbort();
    AudioManager.Instance.StopLoop(this.loopingSound);
  }

  public override void OnDoingBegin(Follower follower)
  {
    base.OnDoingBegin(follower);
    this.SetAnims(follower);
  }

  public void SetAnims(Follower follower)
  {
    if (this.structureBrain != null && this.structureBrain.Data != null && (double) this.structureBrain.Data.Progress >= (double) this.structureBrain.Duration)
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Rituals/lobotomy-rehabilitated-idle");
    else
      follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Rituals/lobotomy-rehabilitating");
    FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain.Info.XPLevel, follower.Brain.Info.SkinName, follower.Brain.Info.SkinColour, FollowerOutfitType.Naked, follower.Brain.Info.Hat, follower.Brain.Info.Clothing, follower.Brain.Info.Customisation, follower.Brain.Info.Special, follower.Brain.Info.Necklace, follower.Brain.Info.ClothingVariant, follower.Brain._directInfoAccess);
  }

  public Interaction_TraitManipulator GetBuilding()
  {
    foreach (Interaction_TraitManipulator traitManipulator in Interaction_TraitManipulator.TraitManipulators)
    {
      if (this.structureBrain != null && traitManipulator.StructureBrain != null && traitManipulator.StructureBrain.Data.ID == this.structureBrain.Data.ID)
        return traitManipulator;
    }
    return (Interaction_TraitManipulator) null;
  }

  public IEnumerator DissentBubbleRoutine(Follower follower)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0)
      {
        WorshipperBubble.SPEECH_TYPE Type = WorshipperBubble.SPEECH_TYPE.HELP;
        follower.WorshipperBubble.gameObject.SetActive(true);
        follower.WorshipperBubble.Play(Type);
        bubbleTimer = (float) (4 + UnityEngine.Random.Range(0, 2));
      }
      yield return (object) null;
    }
  }
}
