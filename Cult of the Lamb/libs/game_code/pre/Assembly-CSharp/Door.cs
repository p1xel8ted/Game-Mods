// Decompiled with JetBrains decompiler
// Type: Door
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using Map;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using System.Collections;
using System.Collections.Generic;
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
  public float EntranceGoToDistance = 7f;
  private Vector2Int NextRoom;
  private bool Used;
  public GameObject VisitedIcon;
  public static List<Door> Doors = new List<Door>();

  public static Door GetEntranceDoor()
  {
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

  private void OnEnable()
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

  private void OnDisable() => Door.Doors.Remove(this);

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
            this.VisitedIcon.SetActive(!BiomeGenerator.Instance.CurrentRoom.N_Room.Room.Visited);
            break;
          }
          break;
        case Door.Direction.East:
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.E_Room.Room != null)
          {
            this.VisitedIcon.SetActive(!BiomeGenerator.Instance.CurrentRoom.E_Room.Room.Visited);
            break;
          }
          break;
        case Door.Direction.South:
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.S_Room.Room != null)
          {
            this.VisitedIcon.SetActive(!BiomeGenerator.Instance.CurrentRoom.S_Room.Room.Visited);
            break;
          }
          break;
        case Door.Direction.West:
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom.W_Room.Room != null)
          {
            this.VisitedIcon.SetActive(!BiomeGenerator.Instance.CurrentRoom.W_Room.Room.Visited);
            break;
          }
          break;
      }
      if ((UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null && GenerateRoom.Instance.LockingDoors && ConnectionType != GenerateRoom.ConnectionTypes.False && ConnectionType != GenerateRoom.ConnectionTypes.Entrance)
        this.RoomLockController.gameObject.SetActive(true);
      else if (ConnectionType == GenerateRoom.ConnectionTypes.Entrance && this.AutoLockBehind)
      {
        this.RoomLockController.gameObject.SetActive(true);
        this.RoomLockController.HideVisuals();
      }
      else
        this.RoomLockController.gameObject.SetActive(false);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player") || this.Used || PlayerFarming.Instance.GoToAndStopping || this.ConnectionType == GenerateRoom.ConnectionTypes.False || this.ConnectionType == GenerateRoom.ConnectionTypes.LeaderBoss)
      return;
    this.Used = true;
    MMTransition.StopCurrentTransition();
    switch (this.ConnectionType)
    {
      case GenerateRoom.ConnectionTypes.True:
      case GenerateRoom.ConnectionTypes.Boss:
      case GenerateRoom.ConnectionTypes.Tarot:
      case GenerateRoom.ConnectionTypes.WeaponShop:
        MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", new System.Action(this.ChangeRoom));
        break;
      case GenerateRoom.ConnectionTypes.Entrance:
      case GenerateRoom.ConnectionTypes.Exit:
        Debug.Log((object) "EXIT!~");
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
        UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
        mapMenuController.Show();
        mapMenuController.OnCancel = mapMenuController.OnCancel + (System.Action) (() =>
        {
          this.Used = false;
          if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
            return;
          PlayerFarming.Instance.GoToAndStop(this.PlayerPosition.position + this.GetDoorDirection() * this.EntranceGoToDistance, IdleOnEnd: true);
        });
        break;
      case GenerateRoom.ConnectionTypes.DoorRoom:
        MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.Completed);
        DataManager.Instance.VisitedLocations.Remove(FollowerLocation.DoorRoom);
        break;
      case GenerateRoom.ConnectionTypes.NextLayer:
        PlayerFarming.Instance.unitObject.UseDeltaTime = false;
        PlayerFarming.Instance.SpineUseDeltaTime(false);
        PlayerFarming.Instance.GoToAndStop(this.transform.position - this.GetDoorDirection() * 2f, DisableCollider: true);
        TweenerCore<Vector3, Vector3, VectorOptions> tween = PlayerFarming.Instance.transform.DOMove(this.transform.position - this.GetDoorDirection() * 2f, 1f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
        {
          PlayerFarming.Instance.unitObject.UseDeltaTime = true;
          PlayerFarming.Instance.SpineUseDeltaTime(true);
          PlayerFarming.Instance.transform.position = this.transform.position;
        }));
        if ((GameManager.SandboxDungeonEnabled || DataManager.Instance.DungeonCompleted(BiomeGenerator.Instance.DungeonLocation)) && (UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null && MapManager.Instance.CurrentNode != null && MapManager.Instance.CurrentNode.nodeType == NodeType.MiniBossFloor)
        {
          if (GameManager.SandboxDungeonEnabled)
          {
            DungeonSandboxManager.Instance.LayerIncremented();
            BiomeGenerator.Instance.Regenerate((System.Action) null);
            break;
          }
          ObjectPool.DestroyAll();
          GameManager.CurrentDungeonFloor = 1;
          ++GameManager.DungeonEndlessLevel;
          this.StartCoroutine((IEnumerator) this.NewMapRoutine());
          break;
        }
        UIAdventureMapOverlayController overlayController = MapManager.Instance.ShowMap();
        overlayController.OnCancel = overlayController.OnCancel + (System.Action) (() =>
        {
          this.Used = false;
          tween.Complete();
          PlayerFarming.Instance.GoToAndStop(this.PlayerPosition.position + this.GetDoorDirection() * this.EntranceGoToDistance * 0.5f, IdleOnEnd: true, DisableCollider: true);
        });
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

  private void ChangeRoom() => BiomeGenerator.ChangeRoom(this.NextRoom);

  private IEnumerator NewMapRoutine()
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
