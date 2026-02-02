// Decompiled with JetBrains decompiler
// Type: EnterBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class EnterBuilding : BaseMonoBehaviour
{
  public UnityEvent Trigger;
  public GameObject Player;
  public FollowerLocation Destination;
  public bool HideCanvasConstants;
  public bool ShowCanvasConstants;
  public bool placedPlayer;
  public GameObject blocker;
  [SerializeField]
  public bool overrideFacingAngle;
  [SerializeField]
  public float facingAngle;
  public PlayerFarming playerFarming;
  public float lastVisitTime;

  public virtual void OnTriggerEnter2D(Collider2D collision)
  {
    this.playerFarming = collision.GetComponent<PlayerFarming>();
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && (PlayerFarming.IsAnyPlayerCarryingBody() || PlayerFarming.IsAnyPlayerChargingSnowball() || this.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive))
      return;
    if (MMConversation.isPlaying)
      MMConversation.mmConversation.FinishCloseFadeTweenByForce();
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && this.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive || !((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null) || MMConversation.isPlaying && !MMConversation.isBark || PlayerFarming.Instance.GoToAndStopping || LetterBox.IsPlaying)
      return;
    this.placedPlayer = false;
    PlayerFarming.Instance.DropDeadFollower();
    MMTransition.StopCurrentTransition();
    if (this.Destination == FollowerLocation.Church)
      TempleDecorationsManager.Instance.SpawnDecorations((Action<GameObject>) null, true);
    else if (this.Destination == FollowerLocation.Base)
      TempleDecorationsManager.Instance.UnloadDecorations();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", (System.Action) (() => this.DoTrigger()));
    this.Player = collision.gameObject;
    if (this.HideCanvasConstants)
      CanvasConstants.instance.Hide();
    if (this.ShowCanvasConstants)
      CanvasConstants.instance.Show();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.CheckToEnsurePositioned());
    this.lastVisitTime = Time.time;
  }

  public IEnumerator CheckToEnsurePositioned()
  {
    EnterBuilding enterBuilding = this;
    yield return (object) new WaitForSeconds(1f);
    if (!enterBuilding.placedPlayer)
    {
      MMTransition.StopCurrentTransition();
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.4f, "", new System.Action(enterBuilding.DoTrigger));
    }
  }

  public virtual void DoTrigger()
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      this.playerFarming = PlayerFarming.Instance;
    PlayerFarming.SetCollidersActive(false);
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    if (this.overrideFacingAngle)
      PlayerFarming.SetFacingAngleForAll(this.facingAngle);
    else
      PlayerFarming.SetFacingAngleForAll(this.playerFarming.state.facingAngle);
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if (PlayerFarming.players[index].GoToAndStopping)
        PlayerFarming.players[index].AbortGoTo();
    }
    PlayerFarming instance = PlayerFarming.Instance;
    PlayerFarming.SetMainPlayer(this.playerFarming);
    if (LocationManager.LocationManagers.ContainsKey(this.Destination))
      LocationManager.LocationManagers[this.Destination].PositionPlayer(this.playerFarming.gameObject);
    else
      PlayerFarming.PositionAllPlayers(this.playerFarming.transform.position);
    PlayerFarming.SetMainPlayer(instance);
    GameManager.GetInstance().CameraSnapToPosition(this.playerFarming.transform.position);
    PlayerFarming.SetStateForAllPlayers();
    PlayerFarming.SetCollidersActive(true);
    PlayerFarming.ResetMainPlayer();
    this.placedPlayer = true;
    if (this.Destination == FollowerLocation.Base && !AudioManager.Instance.IsEventInstancePlaying(AudioManager.Instance.CurrentMusicInstance))
      BiomeBaseManager.Instance.InitMusic();
    this.Trigger?.Invoke();
  }

  public void Update()
  {
    if (!((UnityEngine.Object) this.blocker != (UnityEngine.Object) null))
      return;
    this.blocker.gameObject.SetActive((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && PlayerFarming.IsAnyPlayerCarryingBody() || MMConversation.isPlaying && !MMConversation.isBark || LetterBox.IsPlaying || PlayerFarming.IsAnyPlayerInInteractionWithRanchable() || PlayerFarming.IsAnyPlayerChargingSnowball());
  }

  [CompilerGenerated]
  public void \u003COnTriggerEnter2D\u003Eb__11_0() => this.DoTrigger();
}
