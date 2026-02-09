// Decompiled with JetBrains decompiler
// Type: Interaction_BreakUpFight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_BreakUpFight : Interaction
{
  public Follower follower;
  public FollowerTask currentTask;
  public new string label;

  public void Init(Follower follower)
  {
    this.follower = follower;
    this.PriorityWeight = 10f;
    follower.Interaction_FollowerInteraction.Interactable = false;
    this.currentTask = follower.Brain.CurrentTask;
    if (!((UnityEngine.Object) this.OutlineTarget == (UnityEngine.Object) null))
      return;
    this.OutlineTarget = follower.Interaction_FollowerInteraction.OutlineTarget;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.follower.Interaction_FollowerInteraction.Interactable = true;
  }

  public override void OnDestroy()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      this.EndIndicateHighlighted(player);
    base.OnDestroy();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.label = LocalizationManager.GetTranslation("FollowerInteractions/BreakUpFight");
  }

  public override void Update()
  {
    base.Update();
    if (this.follower.Brain.CurrentTask != this.currentTask)
    {
      this.follower.Interaction_FollowerInteraction.Interactable = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    }
    else
      this.follower.Interaction_FollowerInteraction.Interactable = false;
  }

  public override void GetLabel()
  {
    if (this.label == null)
      this.UpdateLocalisation();
    this.Label = this.label;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    FollowerTask_FightFollower currentTask = this.follower.Brain.CurrentTask as FollowerTask_FightFollower;
    Follower follower = this.follower;
    Follower followerById = FollowerManager.FindFollowerByID(currentTask.OtherChatTask.Brain.Info.ID);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.BreakUpFightIE(follower, followerById));
  }

  public IEnumerator BreakUpFightIE(Follower follower1, Follower follower2)
  {
    Interaction_BreakUpFight interactionBreakUpFight = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBreakUpFight.playerFarming.gameObject, 6f);
    if (follower1.Brain.CurrentTask is FollowerTask_FightFollower)
      ((FollowerTask_FightFollower) follower1.Brain.CurrentTask).Interrupt();
    if (follower2.Brain.CurrentTask is FollowerTask_FightFollower)
      ((FollowerTask_FightFollower) follower2.Brain.CurrentTask).Interrupt();
    Vector3 pos = (follower1.transform.position + follower2.transform.position) / 2f;
    interactionBreakUpFight.playerFarming.GoToAndStop(pos, GoToCallback: (System.Action) (() =>
    {
      this.playerFarming.transform.position = pos;
      this.playerFarming.state.LookAngle = this.playerFarming.state.facingAngle = 270f;
    }), maxDuration: 3f);
    yield return (object) new WaitForSeconds(0.75f);
    interactionBreakUpFight.playerFarming.CustomAnimation("reactions/react-angry" + ((double) UnityEngine.Random.value > 0.5 ? "2" : ""), false);
    yield return (object) new WaitForSeconds(0.25f);
    follower1.transform.DOMove(follower1.transform.position + (follower1.transform.position - follower2.transform.position).normalized / 2f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    follower2.transform.DOMove(follower2.transform.position + (follower2.transform.position - follower1.transform.position).normalized / 2f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    if (!(follower1.Brain.CurrentTask is FollowerTask_ManualControl))
    {
      follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      float time = 2f;
      follower1.TimedAnimation(interactionBreakUpFight.GetReactionAnim(follower1.Brain, follower2.Brain, out time), time, (System.Action) (() =>
      {
        follower1.Brain.CurrentTask?.Abort();
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
      }));
      follower1.AddBodyAnimation("idle", true, 0.0f);
    }
    if (!(follower2.Brain.CurrentTask is FollowerTask_ManualControl))
    {
      follower2.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      float time = 2f;
      follower2.TimedAnimation(interactionBreakUpFight.GetReactionAnim(follower2.Brain, follower1.Brain, out time), time, (System.Action) (() =>
      {
        follower2.Brain.CurrentTask?.Abort();
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
      }));
      follower2.AddBodyAnimation("idle", true, 0.0f);
    }
    yield return (object) new WaitForSeconds(2f);
    CultFaithManager.AddThought(Thought.Cult_BrokeUpFight);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.ClearFightingTarget();
    interactionBreakUpFight.Interactable = false;
    GameManager.GetInstance().OnConversationEnd();
    if ((double) UnityEngine.Random.value < 0.05000000074505806)
      follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_FightFollower(follower2.Brain.Info.ID, true));
  }

  public string GetReactionAnim(FollowerBrain brain, FollowerBrain otherBrain, out float time)
  {
    if (brain.Info.IsDisciple && !otherBrain.Info.IsDisciple || brain.HasTrait(FollowerTrait.TraitType.Bastard) || brain.HasTrait(FollowerTrait.TraitType.Argumentative) || brain.HasTrait(FollowerTrait.TraitType.CriminalHardened))
    {
      time = 3.33333325f;
      return "Reactions/react-laugh";
    }
    if (brain.Info.HasTrait(FollowerTrait.TraitType.Scared) || brain.Info.HasTrait(FollowerTrait.TraitType.CriminalScarred))
    {
      time = 2.9333334f;
      return "Reactions/react-feared";
    }
    if ((double) UnityEngine.Random.value < 0.25)
    {
      time = 9f;
      return "Reactions/react-cry";
    }
    if ((double) UnityEngine.Random.value < 0.25)
    {
      time = 3f;
      return "Reactions/react-embarrassed";
    }
    time = 2f;
    return "Conversations/react-mean" + UnityEngine.Random.Range(1, 4).ToString();
  }
}
