// Decompiled with JetBrains decompiler
// Type: Interaction_TeleporterMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_TeleporterMap : Interaction
{
  public bool Activated;
  public GameObject InactivatedGO;
  public GameObject ActivatedGO;
  public static Interaction_TeleporterMap Instance;
  public static List<Interaction_TeleporterMap> Teleporters = new List<Interaction_TeleporterMap>();
  private string sTeleport;
  private bool activating;
  private Interaction_TeleporterMap.State CurrentState;

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sTeleport = ScriptLocalization.Interactions.Teleport;
  }

  public override void GetLabel() => this.Label = this.sTeleport;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.CurrentState = Interaction_TeleporterMap.State.Active;
    this.InactivatedGO.SetActive(false);
    this.ActivatedGO.SetActive(true);
    Interaction_TeleporterMap.Teleporters.Add(this);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_TeleporterMap.Teleporters.Remove(this);
  }

  private void InitEnemies()
  {
    if (!RoomManager.r.activeTeleporter)
      return;
    this.CurrentState = Interaction_TeleporterMap.State.Active;
    this.SetGameObjectActive();
  }

  private void SetGameObjectActive()
  {
    if (this.CurrentState == Interaction_TeleporterMap.State.Inactive)
    {
      this.InactivatedGO.SetActive(true);
      this.ActivatedGO.SetActive(false);
    }
    else
    {
      this.CurrentState = Interaction_TeleporterMap.State.Active;
      this.InactivatedGO.SetActive(false);
      this.ActivatedGO.SetActive(true);
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.activating || PlayerFarming.Instance.GoToAndStopping)
      return;
    PlayerFarming.Instance.GoToAndStop(this.transform.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut())));
  }

  private IEnumerator DoTeleportOut()
  {
    Interaction_TeleporterMap interactionTeleporterMap = this;
    interactionTeleporterMap.activating = true;
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    interactionTeleporterMap.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionTeleporterMap.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("warp-out-down", 0, false, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    Map.Node randomNodeOnLayer = MapGenerator.GetRandomNodeOnLayer(MapManager.Instance.CurrentLayer + 2);
    if (randomNodeOnLayer != null)
      yield return (object) adventureMapOverlayController.TeleportNode(randomNodeOnLayer);
    interactionTeleporterMap.activating = false;
  }

  private void Activate()
  {
    this.SetGameObjectActive();
    RoomManager.r.activeTeleporter = true;
    MiniMap.CurrentRoomShowTeleporter();
  }

  private enum State
  {
    Inactive,
    Activating,
    Active,
  }
}
