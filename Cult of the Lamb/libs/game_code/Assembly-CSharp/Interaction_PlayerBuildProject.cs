// Decompiled with JetBrains decompiler
// Type: Interaction_PlayerBuildProject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_PlayerBuildProject : Interaction
{
  public BuildSitePlotProject buildSitePlot;
  public bool Activating;
  public static System.Action<BuildSitePlotProject> PlayerActivatingStart;
  public static System.Action<BuildSitePlotProject> PlayerActivatingEnd;
  public List<PlayerFarming> interactingPlayers = new List<PlayerFarming>();
  public string sBuild;
  public string sPrioritise;
  public string sUnprioritise;
  public int activatingCount;

  public override bool InactiveAfterStopMoving => false;

  public override bool CanMultiplePlayersInteract => true;

  public override void OnDisable()
  {
    base.OnDisable();
    if (!this.Activating)
      return;
    this.StopAllCoroutines();
    foreach (PlayerFarming interactingPlayer in this.interactingPlayers)
    {
      interactingPlayer.state.CURRENT_STATE = StateMachine.State.Idle;
      interactingPlayer.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
      System.Action onCrownReturn = interactingPlayer.OnCrownReturn;
      if (onCrownReturn != null)
        onCrownReturn();
    }
    this.interactingPlayers.Clear();
  }

  public void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    this.HasSecondaryInteraction = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sBuild = ScriptLocalization.Interactions.Build;
  }

  public override void GetLabel() => this.Label = this.sBuild;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.playerFarming.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.Activating = true;
    this.StartCoroutine((IEnumerator) this.DoBuild(this.playerFarming));
  }

  public override void GetSecondaryLabel()
  {
    this.SecondaryLabel = this.buildSitePlot.StructureInfo.Prioritised ? this.sUnprioritise : this.sPrioritise;
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (!this.buildSitePlot.StructureInfo.Prioritised)
    {
      foreach (BuildSitePlot buildSitePlot in BuildSitePlot.BuildSitePlots)
      {
        if (buildSitePlot.StructureInfo.Prioritised)
        {
          buildSitePlot.StructureInfo.Prioritised = false;
          buildSitePlot.StructureBrain.MarkObstructionsForClearing(buildSitePlot.StructureInfo.GridTilePosition, buildSitePlot.StructureInfo.Bounds, false);
          foreach (SpriteRenderer componentsInChild in buildSitePlot.gameObject.GetComponentsInChildren<SpriteRenderer>())
            componentsInChild.color = Color.white;
        }
      }
      foreach (BuildSitePlotProject buildSitePlot in BuildSitePlotProject.BuildSitePlots)
      {
        if (buildSitePlot.StructureInfo.Prioritised)
        {
          buildSitePlot.StructureInfo.Prioritised = false;
          buildSitePlot.StructureBrain.MarkObstructionsForClearing(buildSitePlot.StructureInfo.GridTilePosition, buildSitePlot.StructureInfo.Bounds, false);
          foreach (SpriteRenderer componentsInChild in buildSitePlot.gameObject.GetComponentsInChildren<SpriteRenderer>())
            componentsInChild.color = Color.white;
        }
      }
      this.buildSitePlot.StructureInfo.Prioritised = true;
      this.buildSitePlot.StructureBrain.MarkObstructionsForClearing(this.buildSitePlot.StructureInfo.GridTilePosition, this.buildSitePlot.StructureInfo.Bounds, true);
      foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
        componentsInChild.color = Color.yellow;
    }
    else
    {
      foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
        componentsInChild.color = Color.white;
      this.buildSitePlot.StructureInfo.Prioritised = false;
    }
    this.ForceFollowersToUpdateRubble();
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
    this.buildSitePlot.StructureBrain.BuildProgress += 3f;
    CameraManager.shakeCamera(0.3f);
  }

  public new void OnDestroy()
  {
    this.StopAllCoroutines();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
      System.Action onCrownReturn = player.OnCrownReturn;
      if (onCrownReturn != null)
        onCrownReturn();
      if (!player.GoToAndStopping && player.state.CURRENT_STATE == StateMachine.State.CustomAction0)
        this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
  }

  public IEnumerator DoBuild(PlayerFarming playerFarming)
  {
    Interaction_PlayerBuildProject playerBuildProject = this;
    playerBuildProject.interactingPlayers.Add(playerFarming);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.facingAngle = Utils.GetAngle(playerBuildProject.state.transform.position, playerBuildProject.transform.position);
    System.Action<BuildSitePlotProject> playerActivatingStart = Interaction_PlayerBuildProject.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(playerBuildProject.buildSitePlot);
    yield return (object) new WaitForEndOfFrame();
    playerFarming.simpleSpineAnimator.Animate("actions/hammer", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    while ((UnityEngine.Object) playerBuildProject != (UnityEngine.Object) null && playerBuildProject.gameObject.activeSelf && (InputManager.Gameplay.GetInteractButtonHeld(playerFarming) || !SettingsManager.Settings.Accessibility.HoldActions) && (!((UnityEngine.Object) playerFarming != (UnityEngine.Object) null) || playerFarming.state.CURRENT_STATE != StateMachine.State.Meditate) && (SettingsManager.Settings.Accessibility.HoldActions || !InputManager.Gameplay.GetAnyButtonDownExcludingMouse(playerFarming) || InputManager.Gameplay.GetInteractButtonDown(playerFarming)))
      yield return (object) null;
    playerBuildProject.interactingPlayers.Remove(playerFarming);
    playerFarming.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(playerBuildProject.SimpleSpineAnimator_OnSpineEvent);
    System.Action onCrownReturn = playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    if (!playerFarming.GoToAndStopping)
      playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action<BuildSitePlotProject> playerActivatingEnd = Interaction_PlayerBuildProject.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(playerBuildProject.buildSitePlot);
    playerBuildProject.Activating = false;
    --playerBuildProject.activatingCount;
  }
}
