// Decompiled with JetBrains decompiler
// Type: Interaction_PlayerBuildProject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_PlayerBuildProject : Interaction
{
  public BuildSitePlotProject buildSitePlot;
  private bool Activating;
  public static System.Action<BuildSitePlotProject> PlayerActivatingStart;
  public static System.Action<BuildSitePlotProject> PlayerActivatingEnd;
  private string sBuild;
  private string sPrioritise;
  private string sUnprioritise;

  private void Start()
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
    if (this.Activating)
      return;
    base.OnInteract(state);
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.Activating = true;
    this.StartCoroutine((IEnumerator) this.DoBuild());
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

  private void ForceFollowersToUpdateRubble()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      Debug.Log((object) allBrain.CurrentTaskType);
      if (allBrain.CurrentTaskType == FollowerTaskType.ClearRubble || allBrain.CurrentTaskType == FollowerTaskType.ClearWeeds || allBrain.CurrentTaskType == FollowerTaskType.Build || allBrain.CurrentTaskType == FollowerTaskType.ChopTrees || allBrain.CurrentTaskType == FollowerTaskType.Forage)
        allBrain.CheckChangeTask();
    }
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
    this.buildSitePlot.StructureBrain.BuildProgress += 3f;
    CameraManager.shakeCamera(0.3f);
  }

  private new void OnDestroy()
  {
    if (!this.Activating)
      return;
    this.StopAllCoroutines();
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    if (PlayerFarming.Instance.GoToAndStopping)
      return;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  private IEnumerator DoBuild()
  {
    Interaction_PlayerBuildProject playerBuildProject = this;
    playerBuildProject.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    playerBuildProject.state.facingAngle = Utils.GetAngle(playerBuildProject.state.transform.position, playerBuildProject.transform.position);
    System.Action<BuildSitePlotProject> playerActivatingStart = Interaction_PlayerBuildProject.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(playerBuildProject.buildSitePlot);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("actions/hammer", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    while (InputManager.Gameplay.GetInteractButtonHeld())
      yield return (object) null;
    PlayerFarming.Instance.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(playerBuildProject.SimpleSpineAnimator_OnSpineEvent);
    if (!PlayerFarming.Instance.GoToAndStopping)
      playerBuildProject.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action<BuildSitePlotProject> playerActivatingEnd = Interaction_PlayerBuildProject.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(playerBuildProject.buildSitePlot);
    playerBuildProject.Activating = false;
  }
}
