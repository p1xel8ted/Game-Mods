// Decompiled with JetBrains decompiler
// Type: Interaction_PlayerClearRubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_PlayerClearRubble : Interaction
{
  public Rubble rubble;
  public bool RequireUseOthersFirst;
  public static System.Action<Rubble> PlayerActivatingStart;
  public static System.Action<Rubble> PlayerActivatingEnd;
  [SerializeField]
  public ParticleSystem _particleSystem;
  public List<PlayerFarming> activatingPlayers = new List<PlayerFarming>();
  public string sString;
  public string sMine;
  public bool helpedFollower;
  public bool[] buttonDown = new bool[2];
  public float size;

  public override bool InactiveAfterStopMoving => false;

  public void Start()
  {
    this.FreezeCoopPlayersOnHoldToInteract = false;
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    this.HasSecondaryInteraction = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ClearRubble;
    this.sMine = ScriptLocalization.Interactions.MineBloodStone;
  }

  public override void GetLabel()
  {
    if (!this.RequireUseOthersFirst || this.RequireUseOthersFirst && DataManager.Instance.FirstTimeMine)
      this.Label = $"{this.sMine} {FontImageNames.GetIconByType(this.rubble.StructureInfo.LootToDrop)}";
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.activatingPlayers.Add(this._playerFarming);
    DataManager.Instance.FirstTimeMine = true;
    this._playerFarming.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
    this.StartCoroutine((IEnumerator) this.DoBuild(this._playerFarming));
  }

  public override void Update()
  {
    base.Update();
    if (this.rubble.StructureInfo == null)
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
        if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain.CurrentTask is FollowerTask_ClearRubble currentTask && currentTask.RubbleID == this.rubble.Structure.Structure_Info.ID && follower.Brain.CurrentTask.State == FollowerTaskState.Doing)
        {
          this.helpedFollower = true;
          break;
        }
      }
    }
    if (!this.rubble.StructureInfo.Prioritised || !((UnityEngine.Object) this.playerFarming.interactor.CurrentInteraction != (UnityEngine.Object) this))
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

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (!this.rubble.StructureInfo.Prioritised)
    {
      this.rubble.StructureInfo.Prioritised = true;
      foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
        componentsInChild.color = Color.yellow;
    }
    else
    {
      foreach (SpriteRenderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
        componentsInChild.color = Color.white;
      this.rubble.StructureInfo.Prioritised = false;
    }
    this.ForceFollowersToUpdateRubble();
  }

  public void ForceFollowersToUpdateRubble()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.ClearRubble || allBrain.CurrentTaskType == FollowerTaskType.ClearWeeds || allBrain.CurrentTaskType == FollowerTaskType.Build)
        allBrain.CheckChangeTask();
    }
  }

  public void SimpleSpineAnimator_OnSpineEvent(string EventName)
  {
    Debug.Log((object) ("Eventname: " + EventName));
    if (!(EventName == "Chop"))
      return;
    if (PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom && (double) this.rubble.structureBrain.RemovalProgress + 6.0 + (double) UpgradeSystem.Mining >= (double) this.rubble.structureBrain.RemovalDurationInGameMinutes)
    {
      foreach (PlayerFarming playerFarming in new List<PlayerFarming>((IEnumerable<PlayerFarming>) this.activatingPlayers))
      {
        MMVibrate.StopRumble(playerFarming);
        playerFarming.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
        if (playerFarming.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
          playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
        System.Action<Rubble> playerActivatingEnd = Interaction_PlayerClearRubble.PlayerActivatingEnd;
        if (playerActivatingEnd != null)
          playerActivatingEnd(this.rubble);
      }
      this.activatingPlayers.Clear();
      if ((UnityEngine.Object) this.rubble._uiProgressIndicator != (UnityEngine.Object) null)
      {
        this.rubble._uiProgressIndicator.Recycle<UIProgressIndicator>();
        this.rubble._uiProgressIndicator = (UIProgressIndicator) null;
      }
      this.gameObject.SetActive(false);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.STONE, UnityEngine.Random.Range(5, 8), this.transform.position);
      this.rubble.PlayerRubbleFX();
      this.rubble.structureBrain.Data.Destroyed = true;
      AudioManager.Instance.PlayOneShot("event:/material/stone_break", this.transform.position);
    }
    else
    {
      this.rubble.structureBrain.RemovalProgress += 6f + UpgradeSystem.Mining;
      this.rubble.structureBrain.UpdateProgress();
      this.rubble.PlayerRubbleFX();
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.activatingPlayers.Count <= 0)
      return;
    if ((UnityEngine.Object) this.rubble._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this.rubble._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this.rubble._uiProgressIndicator = (UIProgressIndicator) null;
    }
    CameraManager.shakeCamera(0.3f);
    this.StopAllCoroutines();
    foreach (PlayerFarming activatingPlayer in this.activatingPlayers)
    {
      MMVibrate.StopRumble(activatingPlayer);
      activatingPlayer.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.SimpleSpineAnimator_OnSpineEvent);
      activatingPlayer.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    if (!this.helpedFollower)
      return;
    CultFaithManager.AddThought(Thought.Cult_HelpFollower);
  }

  public IEnumerator DoBuild(PlayerFarming player)
  {
    Interaction_PlayerClearRubble playerClearRubble = this;
    playerClearRubble.buttonDown[PlayerFarming.players.IndexOf(player)] = true;
    player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    player.state.facingAngle = Utils.GetAngle(player.state.transform.position, playerClearRubble.transform.position);
    System.Action<Rubble> playerActivatingStart = Interaction_PlayerClearRubble.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(playerClearRubble.rubble);
    yield return (object) new WaitForEndOfFrame();
    player.simpleSpineAnimator.Animate("actions/chop-stone", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    while (playerClearRubble.buttonDown[PlayerFarming.players.IndexOf(player)] && player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    player.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(playerClearRubble.SimpleSpineAnimator_OnSpineEvent);
    if (player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      player.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action onCrownReturn = player.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    System.Action<Rubble> playerActivatingEnd = Interaction_PlayerClearRubble.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(playerClearRubble.rubble);
    playerClearRubble.activatingPlayers.Remove(player);
  }
}
