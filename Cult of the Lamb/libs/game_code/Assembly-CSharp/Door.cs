// Decompiled with JetBrains decompiler
// Type: Door
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using Map;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Door : BaseMonoBehaviour
{
  public Door.Direction direction;
  public GenerateRoom.ConnectionTypes ConnectionType;
  public Transform PlayerPosition;
  public RoomLockController RoomLockController;
  public bool AutoLockBehind = true;
  public bool ForceReturnToBase;
  public bool ForceReturnToDoorRoom;
  public bool ForceReturnToLambTown;
  public bool ForceReturnToGraveyard;
  public float EntranceGoToDistance = 7f;
  public Vector2Int NextRoom;
  public bool Used;
  public GameObject VisitedIcon;
  public static List<Door> Doors = new List<Door>();
  [CompilerGenerated]
  public bool \u003CRequiresUnlocking\u003Ek__BackingField;
  public bool initialised;
  public bool isLoadingAssets;

  public bool RequiresUnlocking
  {
    get => this.\u003CRequiresUnlocking\u003Ek__BackingField;
    set => this.\u003CRequiresUnlocking\u003Ek__BackingField = value;
  }

  public static Door GetEntranceDoor(Door.Direction prioritisedDirection = Door.Direction.South)
  {
    foreach (Door door in Door.Doors)
    {
      if (door.ConnectionType == GenerateRoom.ConnectionTypes.Entrance && door.direction == prioritisedDirection)
        return door;
    }
    foreach (Door door in Door.Doors)
    {
      if (door.ConnectionType == GenerateRoom.ConnectionTypes.Entrance)
        return door;
    }
    return (Door) null;
  }

  public static Door GetFirstNonEntranceDoor()
  {
    foreach (Door door in Door.Doors)
    {
      if (door.ConnectionType != GenerateRoom.ConnectionTypes.Entrance)
        return door;
    }
    return (Door) null;
  }

  public void OnEnable()
  {
    this.Used = false;
    Door.Doors.Add(this);
    switch (this.direction)
    {
      case Door.Direction.North:
        this.tag = "North Door";
        this.NextRoom = new Vector2Int(0, 1);
        break;
      case Door.Direction.East:
        this.tag = "East Door";
        this.NextRoom = new Vector2Int(1, 0);
        break;
      case Door.Direction.South:
        this.tag = "South Door";
        this.NextRoom = new Vector2Int(0, -1);
        break;
      case Door.Direction.West:
        this.tag = "West Door";
        this.NextRoom = new Vector2Int(-1, 0);
        break;
    }
  }

  public void OnDisable() => Door.Doors.Remove(this);

  public void Init(GenerateRoom.ConnectionTypes ConnectionType)
  {
    this.ConnectionType = ConnectionType;
    if (ConnectionType == GenerateRoom.ConnectionTypes.False)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.gameObject.SetActive(true);
      foreach (Renderer componentsInChild in this.GetComponentsInChildren<SpriteRenderer>())
        componentsInChild.enabled = true;
      if ((UnityEngine.Object) this.RoomLockController == (UnityEngine.Object) null)
        return;
      this.VisitedIcon.SetActive(false);
      switch (this.direction)
      {
        case Door.Direction.North:
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.N_Room.Room != null)
          {
            this.VisitedIcon.SetActive(!BiomeGenerator.Instance.CurrentRoom.N_Room.Room.Visited && !BiomeGenerator.Instance.CurrentRoom.N_Room.Room.Hidden);
            break;
          }
          break;
        case Door.Direction.East:
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.E_Room.Room != null)
          {
            this.VisitedIcon.SetActive(!BiomeGenerator.Instance.CurrentRoom.E_Room.Room.Visited && !BiomeGenerator.Instance.CurrentRoom.E_Room.Room.Hidden);
            break;
          }
          break;
        case Door.Direction.South:
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.S_Room.Room != null)
          {
            this.VisitedIcon.SetActive(!BiomeGenerator.Instance.CurrentRoom.S_Room.Room.Visited && !BiomeGenerator.Instance.CurrentRoom.S_Room.Room.Hidden);
            break;
          }
          break;
        case Door.Direction.West:
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.W_Room.Room != null)
          {
            this.VisitedIcon.SetActive(!BiomeGenerator.Instance.CurrentRoom.W_Room.Room.Visited && !BiomeGenerator.Instance.CurrentRoom.W_Room.Room.Hidden);
            break;
          }
          break;
      }
      if ((UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null && GenerateRoom.Instance.LockingDoors && ConnectionType != GenerateRoom.ConnectionTypes.False && ConnectionType != GenerateRoom.ConnectionTypes.Entrance && ConnectionType != GenerateRoom.ConnectionTypes.LoreStoneRoom)
        this.RoomLockController.gameObject.SetActive(true);
      else if (ConnectionType == GenerateRoom.ConnectionTypes.Entrance && this.AutoLockBehind)
      {
        this.RoomLockController.gameObject.SetActive(true);
        this.RoomLockController.HideVisuals();
      }
      else
        this.RoomLockController.gameObject.SetActive(false);
      this.initialised = true;
    }
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.isLoadingAssets)
      return;
    PlayerFarming playerFarming = collision.gameObject.GetComponent<PlayerFarming>();
    if (!(bool) (UnityEngine.Object) playerFarming || MMTransition.IsPlaying || this.Used || playerFarming.GoToAndStopping || this.ConnectionType == GenerateRoom.ConnectionTypes.False || this.ConnectionType == GenerateRoom.ConnectionTypes.LeaderBoss)
      return;
    PlayerFarming.SetCollidersActive(false);
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    PlayerFarming.SetFacingAngleForAll(playerFarming.state.facingAngle);
    CoopManager.Instance.ForceCoopWeapons();
    this.Used = true;
    MMTransition.StopCurrentTransition();
    if (DungeonSandboxManager.Active && this.ConnectionType == GenerateRoom.ConnectionTypes.DoorRoom && MapManager.Instance.CurrentMap.GetFinalBossNode() != MapManager.Instance.CurrentNode)
      this.ConnectionType = GenerateRoom.ConnectionTypes.NextLayer;
    switch (this.ConnectionType)
    {
      case GenerateRoom.ConnectionTypes.True:
      case GenerateRoom.ConnectionTypes.Boss:
      case GenerateRoom.ConnectionTypes.Tarot:
      case GenerateRoom.ConnectionTypes.WeaponShop:
      case GenerateRoom.ConnectionTypes.RelicShop:
      case GenerateRoom.ConnectionTypes.LoreStoneRoom:
        MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", new System.Action(this.ChangeRoom));
        break;
      case GenerateRoom.ConnectionTypes.Entrance:
      case GenerateRoom.ConnectionTypes.Exit:
        Debug.Log((object) "EXIT!~");
        MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
        if (this.ForceReturnToBase)
        {
          MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Base Biome 1", 1f, "", (System.Action) null);
          break;
        }
        if (this.ForceReturnToDoorRoom)
        {
          DataManager.ResetRunData();
          MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Base Biome 1", 1f, "", (System.Action) null);
          break;
        }
        if (this.ForceReturnToLambTown)
        {
          MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Lamb Town", 1f, "", (System.Action) null);
          break;
        }
        if (this.ForceReturnToGraveyard)
        {
          MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "Graveyard", 1f, "", (System.Action) null);
          break;
        }
        if (GameManager.IsDungeon(PlayerFarming.Location))
          break;
        this.isLoadingAssets = true;
        this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadWorldMapAssets(), (System.Action) (() =>
        {
          this.isLoadingAssets = false;
          UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
          mapMenuController.Show();
          mapMenuController.OnCancel = mapMenuController.OnCancel + (System.Action) (() =>
          {
            this.Used = false;
            if (!((UnityEngine.Object) playerFarming != (UnityEngine.Object) null))
              return;
            playerFarming.GoToAndStop(this.PlayerPosition.position + this.GetDoorDirection() * this.EntranceGoToDistance, IdleOnEnd: true, GoToCallback: (System.Action) (() =>
            {
              PlayerFarming.SetCollidersActive(true);
              PlayerFarming.SetStateForAllPlayers();
            }));
          });
        })));
        break;
      case GenerateRoom.ConnectionTypes.DoorRoom:
        MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.Completed);
        DataManager.Instance.VisitedLocations.Remove(FollowerLocation.DoorRoom);
        break;
      case GenerateRoom.ConnectionTypes.NextLayer:
        MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
        playerFarming.unitObject.UseDeltaTime = false;
        playerFarming.SpineUseDeltaTime(false);
        playerFarming.GoToAndStop(this.transform.position - this.GetDoorDirection() * 2f, DisableCollider: true);
        TweenerCore<Vector3, Vector3, VectorOptions> tween = playerFarming.transform.DOMove(this.transform.position - this.GetDoorDirection() * 2f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
        {
          playerFarming.unitObject.UseDeltaTime = true;
          playerFarming.SpineUseDeltaTime(true);
          playerFarming.transform.position = this.transform.position;
        }));
        if (DataManager.Instance.DungeonCompleted(BiomeGenerator.Instance.DungeonLocation) && !GameManager.SandboxDungeonEnabled && (UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && MapManager.Instance.CurrentNode != null && MapManager.Instance.CurrentNode.nodeType == Map.NodeType.MiniBossFloor)
        {
          ObjectPool.DestroyAll();
          BlackSoulUpdater.Instance.Clear();
          GameManager.CurrentDungeonFloor = 1;
          ++GameManager.DungeonEndlessLevel;
          this.StartCoroutine((IEnumerator) this.NewMapRoutine());
          break;
        }
        bool cancelled = false;
        bool hidden = false;
        UIAdventureMapOverlayController overlayController = MapManager.Instance.ShowMap();
        overlayController.OnCancel = overlayController.OnCancel + (System.Action) (() =>
        {
          this.Used = false;
          cancelled = true;
          tween.Complete();
          playerFarming.GoToAndStop(this.PlayerPosition.position + this.GetDoorDirection() * this.EntranceGoToDistance * 0.5f, IdleOnEnd: true, DisableCollider: true, GoToCallback: (System.Action) (() =>
          {
            PlayerFarming.SetCollidersActive(true, true);
            PlayerFarming.SetStateForAllPlayers();
          }));
        });
        overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => hidden = true);
        break;
      case GenerateRoom.ConnectionTypes.DungeonFirstRoom:
        MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", (System.Action) (() =>
        {
          BiomeGenerator.Instance.FirstArrival = true;
          BiomeGenerator.Instance.DoFirstArrivalRoutine = true;
          BiomeGenerator.ChangeRoom(BiomeGenerator.Instance.RoomEntrance.x, BiomeGenerator.Instance.RoomEntrance.y);
        }));
        break;
    }
  }

  public Vector3 GetDoorDirection()
  {
    Vector3 doorDirection = Vector3.zero;
    if (this.direction == Door.Direction.East)
      doorDirection = Vector3.left;
    else if (this.direction == Door.Direction.West)
      doorDirection = Vector3.right;
    else if (this.direction == Door.Direction.North)
      doorDirection = Vector3.down;
    else if (this.direction == Door.Direction.South)
      doorDirection = Vector3.up;
    return doorDirection;
  }

  public void PlayerFinishedEnteringDoor()
  {
    if (this.AutoLockBehind || this.ConnectionType != GenerateRoom.ConnectionTypes.Entrance || !(bool) (UnityEngine.Object) this.GetComponent<Collider2D>())
      return;
    this.ConnectionType = GenerateRoom.ConnectionTypes.True;
    this.GetComponent<Collider2D>().isTrigger = false;
    this.RoomLockController.gameObject.SetActive(true);
    this.RoomLockController.ShowVisuals();
    foreach (PlayerDistanceMovement componentsInChild in this.GetComponentsInChildren<PlayerDistanceMovement>())
    {
      componentsInChild.ForceReset();
      componentsInChild.enabled = false;
    }
  }

  public void ChangeRoom() => BiomeGenerator.ChangeRoom(this.NextRoom);

  public IEnumerator NewMapRoutine()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.RegenerateMapRoutine();
  }

  public enum Direction
  {
    North,
    East,
    South,
    West,
  }
}
