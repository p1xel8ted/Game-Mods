// Decompiled with JetBrains decompiler
// Type: Interaction_PlayerBuild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_PlayerBuild : Interaction
{
  public BuildSitePlot buildSitePlot;
  private bool Activating;
  public static System.Action<BuildSitePlot> PlayerActivatingStart;
  public static System.Action<BuildSitePlot> PlayerActivatingEnd;
  private string sBuild;
  private string sObstructed;
  private string sPrioritise;
  private string sUnprioritise;
  private string sCancel;
  private bool helpedFollower;

  private void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sBuild = ScriptLocalization.Interactions.Build;
    this.sCancel = ScriptLocalization.Interactions.Cancel;
  }

  public override void GetLabel()
  {
    if (!this.buildSitePlot.StructureBrain.IsObstructed)
    {
      this.Interactable = true;
      this.Label = this.sBuild;
    }
    else
    {
      this.Interactable = false;
      this.Label = this.sObstructed;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.Activating = true;
    this.StartCoroutine((IEnumerator) this.DoBuild());
  }

  public override void GetSecondaryLabel()
  {
    string str = StructuresData.GetInfoByType(this.buildSitePlot.Structure.Brain.Data.ToBuildType, 0).IsUpgrade ? string.Empty : this.sCancel;
    this.SecondaryLabel = str;
    this.HasSecondaryInteraction = !string.IsNullOrEmpty(str);
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    AudioManager.Instance.PlayOneShot("event:/building/finished_wood", this.transform.position);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
    foreach (StructuresData.ItemCost itemCost in StructuresData.GetCost(this.buildSitePlot.Structure.Brain.Data.ToBuildType))
      InventoryItem.Spawn(itemCost.CostItem, itemCost.CostValue, this.transform.position);
    this.buildSitePlot.Structure.RemoveStructure();
    GameManager.RecalculatePaths();
    foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
      locationFollower.Brain.CheckChangeTask();
  }

  private void ForceFollowersToUpdateRubble()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      Debug.Log((object) allBrain.CurrentTaskType);
      if (allBrain.CurrentTaskType == FollowerTaskType.ClearRubble || allBrain.CurrentTaskType == FollowerTaskType.ClearWeeds || allBrain.CurrentTaskType == FollowerTaskType.Build || allBrain.CurrentTaskType == FollowerTaskType.ChopTrees || allBrain.CurrentTaskType == FollowerTaskType.Forage)
        allBrain.CheckChangeTask();
    }
  }

  private new void Update()
  {
    if (this.buildSitePlot.StructureInfo == null)
      return;
    if (this.Activating && !this.helpedFollower)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if (follower.Brain.CurrentTaskType == FollowerTaskType.Build && follower.Brain.CurrentTask.UsingStructureID == this.buildSitePlot.Structure.Structure_Info.ID && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
        {
          this.helpedFollower = true;
          break;
        }
      }
    }
    if (!this.buildSitePlot.StructureInfo.Prioritised || !((UnityEngine.Object) Interactor.CurrentInteraction != (UnityEngine.Object) this))
      return;
    foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.color = Color.yellow;
  }

  public override void OnBecomeCurrent()
  {
    foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.color = Color.white;
    base.OnBecomeCurrent();
  }

  private void SimpleSpineAnimator_OnSpineEvent(string EventName)
  {
    if (!(EventName == "Chop"))
      return;
    AudioManager.Instance.PlayOneShot("event:/followers/hammering", this.transform.position);
    this.buildSitePlot.StructureBrain.BuildProgress += 5f;
    if (LetterBox.IsPlaying)
      return;
    CameraManager.shakeCamera(0.3f);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!this.Activating)
      return;
    this.StopAllCoroutines();
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    System.Action onCrownReturn = PlayerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (!PlayerFarming.Instance.GoToAndStopping)
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    if (!this.helpedFollower)
      return;
    CultFaithManager.AddThought(Thought.Cult_HelpFollower);
  }

  private IEnumerator DoBuild()
  {
    Interaction_PlayerBuild interactionPlayerBuild = this;
    interactionPlayerBuild.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionPlayerBuild.state.facingAngle = Utils.GetAngle(interactionPlayerBuild.state.transform.position, interactionPlayerBuild.transform.position);
    System.Action<BuildSitePlot> playerActivatingStart = Interaction_PlayerBuild.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionPlayerBuild.buildSitePlot);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("actions/hammer", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    while (InputManager.Gameplay.GetInteractButtonHeld())
      yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(interactionPlayerBuild.SimpleSpineAnimator_OnSpineEvent);
    System.Action onCrownReturn = PlayerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (!PlayerFarming.Instance.GoToAndStopping)
      interactionPlayerBuild.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action<BuildSitePlot> playerActivatingEnd = Interaction_PlayerBuild.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(interactionPlayerBuild.buildSitePlot);
    interactionPlayerBuild.Activating = false;
  }
}
