// Decompiled with JetBrains decompiler
// Type: RoomLockController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMRoomGeneration;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RoomLockController : BaseMonoBehaviour
{
  public bool Standalone;
  public bool StandaloneDoCorrectPlayersPositions;
  public bool CanApplyUnitCorrection = true;
  public Animator animator;
  public GameObject BlockingCollider;
  public GameObject VisualsContainer;
  public static List<RoomLockController> RoomLockControllers = new List<RoomLockController>();
  public SpriteRenderer BlockedSprite;
  public bool Open = true;
  public bool Completed;
  [CompilerGenerated]
  public bool \u003CForcedLocked\u003Ek__BackingField;
  public static bool DoorsOpen = false;
  public bool inDungeon = true;
  public bool inCollision;
  public int count;

  public bool ForcedLocked
  {
    get => this.\u003CForcedLocked\u003Ek__BackingField;
    set => this.\u003CForcedLocked\u003Ek__BackingField = value;
  }

  public static event RoomLockController.RoomEvent OnRoomCleared;

  public static event RoomLockController.RoomEvent OnRoomCompleted;

  public static event RoomLockController.RoomEvent OnDoorsClosed;

  public void Start()
  {
    if (!((Object) this.BlockedSprite != (Object) null))
      return;
    this.BlockedSprite.material = new Material(this.BlockedSprite.material);
  }

  public void OnEnable()
  {
    RoomLockController.RoomLockControllers.Add(this);
    this.BlockingCollider.SetActive(!this.Open);
    if (!this.ForcedLocked)
      return;
    this.DoorUp();
  }

  public void OnDisable() => RoomLockController.RoomLockControllers.Remove(this);

  public void DoorUp()
  {
    RoomLockController.RoomEvent onDoorsClosed = RoomLockController.OnDoorsClosed;
    if (onDoorsClosed != null)
      onDoorsClosed();
    this.Open = false;
    RoomLockController.DoorsOpen = false;
    this.animator.Play("GoopWallIntro");
    this.BlockingCollider.SetActive(true);
  }

  public void DoorDown(bool forced = false)
  {
    if (!forced && GenerateRoom.Instance.LockEntranceBehindPlayer && (Object) Door.GetEntranceDoor()?.RoomLockController == (Object) this || this.ForcedLocked)
      return;
    this.Open = true;
    RoomLockController.DoorsOpen = true;
    this.animator.Play("GoopWallDown");
    this.BlockingCollider.SetActive(false);
  }

  public static void CloseAll(bool checkUnitsPlacement = true)
  {
    foreach (RoomLockController roomLockController in RoomLockController.RoomLockControllers)
    {
      if (!roomLockController.Standalone)
        roomLockController.DoorUp();
    }
    if (Health.team2.Count > 0 && (BiomeGenerator.Instance.CurrentRoom == null || string.IsNullOrEmpty(BiomeGenerator.Instance.CurrentRoom.GameObjectPath) || !BiomeGenerator.Instance.CurrentRoom.GameObjectPath.Contains("Marketplace Relic")))
      HUD_Manager.Instance.HideTopRight();
    if (!checkUnitsPlacement)
      return;
    if (CoopManager.CoopActive)
      RoomLockController.CorrectPlayersPositions();
    RoomLockController.CorrectFriendlyEnemiesPositions();
  }

  public static void OpenAll()
  {
    CoopManager.PreventWeaponSpawn = false;
    foreach (RoomLockController roomLockController in RoomLockController.RoomLockControllers)
      roomLockController.DoorDown();
  }

  public static void RoomCompleted(bool wasCombatRoom = false, bool doorsDown = true)
  {
    if ((Object) BiomeGenerator.Instance != (Object) null)
    {
      if (!BiomeGenerator.Instance.CurrentRoom.Completed)
      {
        if ((double) PlayerFleeceManager.AmountToHealOnRoomComplete() > 0.0)
        {
          for (int index = 0; index < PlayerFarming.playersCount; ++index)
          {
            PlayerFarming player = PlayerFarming.players[index];
            float healing = PlayerFleeceManager.AmountToHealOnRoomComplete() * (float) TrinketManager.GetHealthAmountMultiplier(player);
            player.health.Heal(healing);
          }
          CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        }
        if ((bool) (Object) BiomeGenerator.Instance.CurrentRoom.generateRoom)
          BiomeGenerator.Instance.CurrentRoom.generateRoom.OnRoomComplete?.Invoke();
      }
      BiomeGenerator.Instance.CurrentRoom.Completed = true;
    }
    foreach (RoomLockController roomLockController in RoomLockController.RoomLockControllers)
    {
      if (doorsDown)
        roomLockController.DoorDown();
      if (!roomLockController.ForcedLocked)
        roomLockController.Completed = true;
    }
    if (wasCombatRoom)
    {
      DeviceLightingManager.TransitionLighting(Color.yellow, Color.white, 0.6f, DeviceLightingManager.F_KEYS);
      RoomLockController.RoomEvent onRoomCleared = RoomLockController.OnRoomCleared;
      if (onRoomCleared != null)
        onRoomCleared();
    }
    RoomLockController.RoomEvent onRoomCompleted = RoomLockController.OnRoomCompleted;
    if (onRoomCompleted != null)
      onRoomCompleted();
    HUD_Manager.Instance.ShowTopRight();
  }

  public void HideVisuals()
  {
    if (!((Object) this.VisualsContainer != (Object) null))
      return;
    this.VisualsContainer.SetActive(false);
  }

  public void ShowVisuals()
  {
    if (!((Object) this.VisualsContainer != (Object) null))
      return;
    this.VisualsContainer.SetActive(true);
  }

  public void OnTriggerEnter2D(Collider2D collision) => this.count = 0;

  public void OnTriggerStay2D(Collider2D collision)
  {
    if (!GameManager.IsDungeon(PlayerFarming.Location))
    {
      Debug.Log((object) "not in dungeon");
    }
    else
    {
      if (!((Object) PlayerFarming.Instance != (Object) null) || !((Object) collision.gameObject == (Object) PlayerFarming.Instance.gameObject))
        return;
      ++this.count;
      if (PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.Moving)
      {
        if (this.Completed || this.Open || this.inCollision || this.count <= 5)
          return;
        this.inCollision = true;
        this.animator.Play("GoopWallColliding");
        AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", PlayerFarming.Instance.transform.position);
      }
      else
        this.count = 0;
    }
  }

  public static void CorrectPlayersPositions()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((Object) player.circleCollider2D != (Object) null && player.circleCollider2D.enabled)
        RoomLockController.CorrectUnitPosition(player.transform);
    }
  }

  public static void CorrectFriendlyEnemiesPositions()
  {
    foreach (Component friendlyEnemy in FriendlyEnemy.FriendlyEnemies)
      RoomLockController.CorrectUnitPosition(friendlyEnemy.transform);
  }

  public static void CorrectUnitPosition(Transform entity)
  {
    RoomLockController roomLockController1 = (RoomLockController) null;
    float num1 = float.PositiveInfinity;
    foreach (RoomLockController roomLockController2 in RoomLockController.RoomLockControllers)
    {
      if (roomLockController2.CanApplyUnitCorrection)
      {
        float num2 = Vector3.Distance(entity.position, roomLockController2.transform.position);
        if ((double) num2 < (double) num1)
        {
          roomLockController1 = roomLockController2;
          num1 = num2;
        }
      }
    }
    if ((Object) roomLockController1 == (Object) null || roomLockController1.Open)
      return;
    Vector2 normalized = (Vector2) (entity.position - roomLockController1.BlockingCollider.transform.position).normalized;
    if ((double) Vector2.Angle((Vector2) -roomLockController1.BlockingCollider.transform.up, normalized) < 90.0)
      return;
    entity.position = roomLockController1.BlockingCollider.transform.position - roomLockController1.BlockingCollider.transform.up * 0.5f;
  }

  public static bool IsPositionOutOfRoom(Vector3 position, bool isTeleport = false)
  {
    RoomLockController roomLockController1 = (RoomLockController) null;
    float num1 = float.PositiveInfinity;
    foreach (RoomLockController roomLockController2 in RoomLockController.RoomLockControllers)
    {
      if (roomLockController2.CanApplyUnitCorrection)
      {
        float num2 = Vector3.Distance(position, roomLockController2.transform.position);
        if ((double) num2 < (double) num1)
        {
          roomLockController1 = roomLockController2;
          num1 = num2;
        }
      }
    }
    if ((Object) roomLockController1 != (Object) null && !roomLockController1.Open | isTeleport)
    {
      Vector2 normalized = (Vector2) (position - roomLockController1.BlockingCollider.transform.position).normalized;
      if ((double) Vector2.Angle((Vector2) -roomLockController1.BlockingCollider.transform.up, normalized) >= 90.0)
        return true;
      Collider2D component = roomLockController1.GetComponent<Collider2D>();
      if ((Object) component != (Object) null && (double) Vector2.Angle((Vector2) -component.transform.position, normalized) >= 90.0)
        return true;
    }
    return false;
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    PlayerFarming component = collision.GetComponent<PlayerFarming>();
    if (!((Object) component != (Object) null) || !this.Open || this.Completed)
      return;
    this.inCollision = false;
    if (this.Standalone)
    {
      this.DoorUp();
      if (this.StandaloneDoCorrectPlayersPositions && CoopManager.CoopActive)
        RoomLockController.CorrectPlayersPositions();
      RoomLockController.CorrectFriendlyEnemiesPositions();
    }
    else
    {
      if ((Object) BiomeGenerator.Instance != (Object) null)
        BiomeGenerator.Instance.RoomBecameActive(component);
      RoomLockController.CloseAll();
    }
  }

  public delegate void RoomEvent();
}
