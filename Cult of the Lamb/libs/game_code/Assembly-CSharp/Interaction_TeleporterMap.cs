// Decompiled with JetBrains decompiler
// Type: Interaction_TeleporterMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Map;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_TeleporterMap : Interaction
{
  public bool Activated;
  public GameObject InactivatedGO;
  public GameObject ActivatedGO;
  public static Interaction_TeleporterMap Instance;
  public static List<Interaction_TeleporterMap> Teleporters = new List<Interaction_TeleporterMap>();
  public string sTeleport;
  public bool activating;
  public Interaction_TeleporterMap.State CurrentState;

  public void Start() => this.UpdateLocalisation();

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

  public void InitEnemies()
  {
    if (!RoomManager.r.activeTeleporter)
      return;
    this.CurrentState = Interaction_TeleporterMap.State.Active;
    this.SetGameObjectActive();
  }

  public void SetGameObjectActive()
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
    if (this.activating || this.playerFarming.GoToAndStopping)
      return;
    this.playerFarming.GoToAndStop(this.transform.position, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.DoTeleportOut())));
  }

  public IEnumerator DoTeleportOut()
  {
    Interaction_TeleporterMap interactionTeleporterMap = this;
    interactionTeleporterMap.activating = true;
    GameManager.GetInstance().OnConversationNext(interactionTeleporterMap.playerFarming.gameObject, 8f);
    interactionTeleporterMap.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/pentagram/pentagram_teleport_segment", interactionTeleporterMap.gameObject);
    interactionTeleporterMap.playerFarming.simpleSpineAnimator.AddAnimate("warp-out-down", 0, false, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    Map.Node randomNodeOnLayer = MapGenerator.GetRandomNodeOnLayer(MapManager.Instance.CurrentLayer + 2);
    if (randomNodeOnLayer != null)
      yield return (object) adventureMapOverlayController.TeleportNode(randomNodeOnLayer);
    interactionTeleporterMap.activating = false;
  }

  public void Activate()
  {
    this.SetGameObjectActive();
    RoomManager.r.activeTeleporter = true;
    MiniMap.CurrentRoomShowTeleporter();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__16_0()
  {
    this.StartCoroutine((IEnumerator) this.DoTeleportOut());
  }

  public enum State
  {
    Inactive,
    Activating,
    Active,
  }
}
