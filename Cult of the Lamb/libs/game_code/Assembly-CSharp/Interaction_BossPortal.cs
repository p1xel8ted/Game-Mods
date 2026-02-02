// Decompiled with JetBrains decompiler
// Type: Interaction_BossPortal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.DeathScreen;
using MMBiomeGeneration;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_BossPortal : Interaction
{
  public string sTeleport;
  public bool activating;

  public override void OnDisable()
  {
    base.OnDisable();
    if (!this.activating)
      return;
    GameManager.GetInstance().OnConversationEnd(false);
    this.activating = false;
  }

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sTeleport = this.Interactable ? ScriptLocalization.Interactions.Teleport : "";
  }

  public override void GetLabel() => this.Label = this.sTeleport;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.activating || this.playerFarming.GoToAndStopping)
      return;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: this.playerFarming);
    this.playerFarming.GoToAndStop(this.transform.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut())));
  }

  public IEnumerator DoTeleportOut()
  {
    Interaction_BossPortal interactionBossPortal = this;
    interactionBossPortal.activating = true;
    UIDeathScreenOverlayController.UsedBossPortal = true;
    GameManager.GetInstance().OnConversationNext(interactionBossPortal.playerFarming.gameObject, 8f);
    interactionBossPortal.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionBossPortal.gameObject);
    interactionBossPortal.playerFarming.Spine.AnimationState.SetAnimation(0, "warp-out-down", false);
    yield return (object) new WaitForSeconds(2f);
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", new System.Action(interactionBossPortal.ChangeRoom));
    DungeonModifier.SetActiveModifier((DungeonModifier) null);
    interactionBossPortal.activating = false;
  }

  public void ChangeRoom()
  {
    if ((UnityEngine.Object) Interaction_RatooRescue.Instance != (UnityEngine.Object) null && Interaction_RatooRescue.Instance.isNowFollowing)
    {
      Interaction_RatooRescue.Instance.isNowFollowing = false;
      UnityEngine.Object.Destroy((UnityEngine.Object) Interaction_RatooRescue.Instance.gameObject);
    }
    if ((UnityEngine.Object) Interaction_BaalRescue.Instance != (UnityEngine.Object) null && Interaction_BaalRescue.Instance.isNowFollowing)
    {
      Interaction_BaalRescue.Instance.isNowFollowing = false;
      UnityEngine.Object.Destroy((UnityEngine.Object) Interaction_BaalRescue.Instance.gameObject);
    }
    if (BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_1 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_2 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_3 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_4)
      BiomeGenerator.ChangeRoom(0, 1);
    else
      BiomeGenerator.ChangeRoom(BiomeGenerator.BossCoords.x, BiomeGenerator.BossCoords.y);
    GameManager.GetInstance().CachedCamTargets = new List<CameraFollowTarget.Target>();
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__6_0()
  {
    this.StartCoroutine((IEnumerator) this.DoTeleportOut());
  }
}
