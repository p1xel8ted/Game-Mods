// Decompiled with JetBrains decompiler
// Type: Interaction_BossPortal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_BossPortal : Interaction
{
  private string sTeleport;
  private bool activating;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sTeleport = ScriptLocalization.Interactions.Teleport;
  }

  public override void GetLabel() => this.Label = this.sTeleport;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.activating || PlayerFarming.Instance.GoToAndStopping)
      return;
    PlayerFarming.Instance.GoToAndStop(this.transform.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut())));
  }

  private IEnumerator DoTeleportOut()
  {
    Interaction_BossPortal interactionBossPortal = this;
    interactionBossPortal.activating = true;
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    interactionBossPortal.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionBossPortal.gameObject);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "warp-out-down", false);
    yield return (object) new WaitForSeconds(2f);
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", new System.Action(interactionBossPortal.ChangeRoom));
    DungeonModifier.SetActiveModifier((DungeonModifier) null);
    interactionBossPortal.activating = false;
  }

  private void ChangeRoom()
  {
    if (BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_1 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_2 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_3 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_4)
      BiomeGenerator.ChangeRoom(0, 1);
    else
      BiomeGenerator.ChangeRoom(BiomeGenerator.BossCoords.x, BiomeGenerator.BossCoords.y);
  }
}
