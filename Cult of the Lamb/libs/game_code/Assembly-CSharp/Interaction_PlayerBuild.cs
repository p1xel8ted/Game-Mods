// Decompiled with JetBrains decompiler
// Type: Interaction_PlayerBuild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_PlayerBuild : Interaction
{
  public BuildSitePlot buildSitePlot;
  public bool Activating;
  public static System.Action<BuildSitePlot> PlayerActivatingStart;
  public static System.Action<BuildSitePlot> PlayerActivatingEnd;
  public List<PlayerFarming> activatingPlayers = new List<PlayerFarming>();
  public string sBuild;
  public string sObstructed;
  public string sPrioritise;
  public string sUnprioritise;
  public string sCancel;
  public int activatingCount;
  public bool helpedFollower;

  public void Start()
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
    base.OnInteract(state);
    this.playerFarming.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.Activating = true;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.DoBuild(this.playerFarming));
    ++this.activatingCount;
  }

  public override void GetSecondaryLabel()
  {
    string str = StructuresData.GetInfoByType(this.buildSitePlot.Structure.Brain.Data.ToBuildType, 0).IsUpgrade ? string.Empty : this.sCancel;
    this.SecondaryLabel = this.Activating ? "" : str;
    this.HasSecondaryInteraction = !string.IsNullOrEmpty(str);
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    AudioManager.Instance.PlayOneShot("event:/building/finished_wood", this.transform.position);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact, this.playerFarming);
    foreach (StructuresData.ItemCost itemCost in StructuresData.GetCost(this.buildSitePlot.Structure.Brain.Data.ToBuildType))
      InventoryItem.Spawn(itemCost.CostItem, itemCost.CostValue, this.transform.position);
    this.buildSitePlot.Structure.RemoveStructure();
    GameManager.RecalculatePaths();
    foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
      locationFollower.Brain.CheckChangeTask();
  }

  public void ForceFollowersToUpdateRubble()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      Debug.Log((object) allBrain.CurrentTaskType);
      if (allBrain.CurrentTaskType == FollowerTaskType.ClearRubble || allBrain.CurrentTaskType == FollowerTaskType.ClearWeeds || allBrain.CurrentTaskType == FollowerTaskType.Build || allBrain.CurrentTaskType == FollowerTaskType.ChopTrees || allBrain.CurrentTaskType == FollowerTaskType.Forage)
        allBrain.CheckChangeTask();
    }
  }

  public override void Update()
  {
    base.Update();
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
    if (!this.buildSitePlot.StructureInfo.Prioritised || !((UnityEngine.Object) this.playerFarming.interactor.CurrentInteraction != (UnityEngine.Object) this))
      return;
    foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.color = Color.yellow;
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.color = Color.white;
    base.OnBecomeCurrent(playerFarming);
  }

  public void SimpleSpineAnimator_OnSpineEvent(string EventName)
  {
    if (!(EventName == "Chop"))
      return;
    AudioManager.Instance.PlayOneShot("event:/followers/hammering", this.transform.position);
    this.buildSitePlot.StructureBrain.BuildProgress += 5f;
    if (LetterBox.IsPlaying)
      return;
    CameraManager.shakeCamera(0.3f);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.StopAllCoroutines();
    foreach (PlayerFarming activatingPlayer in this.activatingPlayers)
    {
      activatingPlayer.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
      System.Action onCrownReturn = activatingPlayer.OnCrownReturn;
      if (onCrownReturn != null)
        onCrownReturn();
      if (!activatingPlayer.GoToAndStopping && activatingPlayer.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
        this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    if (!this.helpedFollower || this.activatingCount <= 0)
      return;
    CultFaithManager.AddThought(Thought.Cult_HelpFollower);
  }

  public IEnumerator DoBuild(PlayerFarming playerFarming)
  {
    Interaction_PlayerBuild interactionPlayerBuild = this;
    interactionPlayerBuild.activatingPlayers.Add(playerFarming);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.facingAngle = Utils.GetAngle(interactionPlayerBuild.state.transform.position, interactionPlayerBuild.transform.position);
    System.Action<BuildSitePlot> playerActivatingStart = Interaction_PlayerBuild.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionPlayerBuild.buildSitePlot);
    yield return (object) new WaitForEndOfFrame();
    playerFarming.simpleSpineAnimator.Animate("actions/hammer", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    while ((UnityEngine.Object) interactionPlayerBuild != (UnityEngine.Object) null && interactionPlayerBuild.gameObject.activeSelf && (InputManager.Gameplay.GetInteractButtonHeld(playerFarming) || !SettingsManager.Settings.Accessibility.HoldActions) && (!((UnityEngine.Object) playerFarming != (UnityEngine.Object) null) || playerFarming.state.CURRENT_STATE != StateMachine.State.Meditate) && (SettingsManager.Settings.Accessibility.HoldActions || !InputManager.Gameplay.GetAnyButtonDownExcludingMouse(playerFarming) || InputManager.Gameplay.GetInteractButtonDown(playerFarming)))
      yield return (object) null;
    playerFarming.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(interactionPlayerBuild.SimpleSpineAnimator_OnSpineEvent);
    System.Action onCrownReturn = playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (!playerFarming.GoToAndStopping && playerFarming.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionPlayerBuild.activatingPlayers.Remove(playerFarming);
    System.Action<BuildSitePlot> playerActivatingEnd = Interaction_PlayerBuild.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(interactionPlayerBuild.buildSitePlot);
    interactionPlayerBuild.Activating = false;
    --interactionPlayerBuild.activatingCount;
  }
}
