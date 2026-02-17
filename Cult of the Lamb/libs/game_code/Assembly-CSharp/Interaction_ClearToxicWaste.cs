// Decompiled with JetBrains decompiler
// Type: Interaction_ClearToxicWaste
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_ClearToxicWaste : Interaction
{
  public Waste waste;
  public bool RequireUseOthersFirst;
  public static System.Action<Waste> PlayerActivatingStart;
  public static System.Action<Waste> PlayerActivatingEnd;
  public List<PlayerFarming> activatingPlayers = new List<PlayerFarming>();
  public bool helpedFollower;
  public bool[] buttonDown = new bool[2];
  public string sString;

  public override bool InactiveAfterStopMoving => false;

  public void Start()
  {
    this.FreezeCoopPlayersOnHoldToInteract = false;
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    this.HasSecondaryInteraction = false;
    if (this.waste.StructureBrain != null)
      this.OnBrainAssigend();
    else
      this.waste.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigend);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Clean;
  }

  public override void GetLabel()
  {
    if (!this.RequireUseOthersFirst || this.RequireUseOthersFirst && DataManager.Instance.FirstTimeMine)
      this.Label = this.sString;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.activatingPlayers.Add(this._playerFarming);
    DataManager.Instance.FirstTimeMine = true;
    this._playerFarming.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.StartCoroutine((IEnumerator) this.DoClean(this._playerFarming));
  }

  public override void Update()
  {
    base.Update();
    if (this.waste.StructureInfo == null)
      return;
    foreach (PlayerFarming activatingPlayer in this.activatingPlayers)
    {
      if (InputManager.Gameplay.GetInteractButtonUp(activatingPlayer) && SettingsManager.Settings.Accessibility.HoldActions || (UnityEngine.Object) activatingPlayer != (UnityEngine.Object) null && activatingPlayer.state.CURRENT_STATE == StateMachine.State.Meditate || !SettingsManager.Settings.Accessibility.HoldActions && InputManager.Gameplay.GetAnyButtonDownExcludingMouse(activatingPlayer) && !InputManager.Gameplay.GetInteractButtonDown(activatingPlayer))
        this.buttonDown[PlayerFarming.players.IndexOf(activatingPlayer)] = false;
    }
    if (this.activatingPlayers.Count > 0 && !this.helpedFollower)
    {
      foreach (Follower follower in Follower.Followers)
      {
        if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain.CurrentTask is FollowerTask_CleanWaste currentTask && currentTask.UsingStructureID == this.waste.Structure.Structure_Info.ID && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
        {
          this.helpedFollower = true;
          break;
        }
      }
    }
    if (!this.waste.StructureInfo.Prioritised || !((UnityEngine.Object) this.playerFarming.interactor.CurrentInteraction != (UnityEngine.Object) this))
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

  public void ForceFollowersToUpdateRubble()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.CleanWaste)
        allBrain.CheckChangeTask();
    }
  }

  public void SimpleSpineAnimator_OnSpineEvent(string EventName)
  {
    Debug.Log((object) ("Eventname: " + EventName));
    if (!(EventName == "dig"))
      return;
    this.waste.StructureBrain.RemovalProgress += 6f + UpgradeSystem.Mining;
    this.waste.StructureBrain.UpdateProgress();
    this.waste.PlayerRubbleFX();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.activatingPlayers.Count > 0)
    {
      CameraManager.shakeCamera(0.3f);
      this.StopAllCoroutines();
      foreach (PlayerFarming activatingPlayer in this.activatingPlayers)
      {
        MMVibrate.StopRumble(activatingPlayer);
        activatingPlayer.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
        activatingPlayer.state.CURRENT_STATE = StateMachine.State.Idle;
      }
      if (this.helpedFollower)
        CultFaithManager.AddThought(Thought.Cult_HelpFollower);
    }
    if (!((UnityEngine.Object) this.waste != (UnityEngine.Object) null) || !((UnityEngine.Object) this.waste.Structure != (UnityEngine.Object) null))
      return;
    this.waste.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigend);
  }

  public IEnumerator DoClean(PlayerFarming player)
  {
    Interaction_ClearToxicWaste interactionClearToxicWaste = this;
    interactionClearToxicWaste.buttonDown[PlayerFarming.players.IndexOf(player)] = true;
    player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    player.state.facingAngle = Utils.GetAngle(player.state.transform.position, interactionClearToxicWaste.transform.position);
    System.Action<Waste> playerActivatingStart = Interaction_ClearToxicWaste.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionClearToxicWaste.waste);
    yield return (object) new WaitForEndOfFrame();
    player.simpleSpineAnimator.Animate("actions/dig", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    while (interactionClearToxicWaste.buttonDown[PlayerFarming.players.IndexOf(player)] && player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    player.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(interactionClearToxicWaste.SimpleSpineAnimator_OnSpineEvent);
    if (player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      player.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action onCrownReturn = player.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    System.Action<Waste> playerActivatingEnd = Interaction_ClearToxicWaste.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(interactionClearToxicWaste.waste);
    interactionClearToxicWaste.activatingPlayers.Remove(player);
  }

  public void OnBrainAssigend()
  {
    this.waste.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigend);
    this.waste.StructureBrain.OnRemovalBegin += new System.Action(this.FreeFollower);
  }

  public void FreeFollower()
  {
    Follower followerById = FollowerManager.FindFollowerByID(this.waste.StructureInfo.FollowerID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
      return;
    followerById.transform.parent = BaseLocationManager.Instance.UnitLayer;
    followerById.Brain.CompleteCurrentTask();
    ++DataManager.Instance.FollowersTrappedInToxicWaste;
  }
}
