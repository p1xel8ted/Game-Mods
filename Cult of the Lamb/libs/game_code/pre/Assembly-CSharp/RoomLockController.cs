// Decompiled with JetBrains decompiler
// Type: RoomLockController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMRoomGeneration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RoomLockController : BaseMonoBehaviour
{
  public bool Standalone;
  public Animator animator;
  public GameObject BlockingCollider;
  public GameObject VisualsContainer;
  public static List<RoomLockController> RoomLockControllers = new List<RoomLockController>();
  public SpriteRenderer BlockedSprite;
  public bool Open = true;
  public bool Completed;
  public static bool DoorsOpen = false;
  private bool inDungeon = true;
  private bool inCollision;
  private int count;

  public static event RoomLockController.RoomEvent OnRoomCleared;

  private void Start()
  {
    if (!((Object) this.BlockedSprite != (Object) null))
      return;
    this.BlockedSprite.material = new Material(this.BlockedSprite.material);
  }

  private void OnEnable()
  {
    RoomLockController.RoomLockControllers.Add(this);
    this.BlockingCollider.SetActive(!this.Open);
  }

  private void OnDisable() => RoomLockController.RoomLockControllers.Remove(this);

  public void DoorUp()
  {
    this.Open = false;
    RoomLockController.DoorsOpen = false;
    this.animator.Play("GoopWallIntro");
    this.BlockingCollider.SetActive(true);
  }

  public void DoorDown(bool forced = false)
  {
    if (!forced && GenerateRoom.Instance.LockEntranceBehindPlayer && (Object) Door.GetEntranceDoor()?.RoomLockController == (Object) this)
      return;
    this.Open = true;
    RoomLockController.DoorsOpen = true;
    this.animator.Play("GoopWallDown");
    this.BlockingCollider.SetActive(false);
  }

  public static void CloseAll()
  {
    foreach (RoomLockController roomLockController in RoomLockController.RoomLockControllers)
    {
      if (!roomLockController.Standalone)
        roomLockController.DoorUp();
    }
    if (Health.team2.Count <= 0)
      return;
    HUD_Manager.Instance.HideTopRight();
  }

  public static void OpenAll()
  {
    foreach (RoomLockController roomLockController in RoomLockController.RoomLockControllers)
      roomLockController.DoorDown();
  }

  public static void RoomCompleted(bool wasCombatRoom = false, bool doorsDown = true)
  {
    if ((Object) BiomeGenerator.Instance != (Object) null)
      BiomeGenerator.Instance.CurrentRoom.Completed = true;
    foreach (RoomLockController roomLockController in RoomLockController.RoomLockControllers)
    {
      if (doorsDown)
        roomLockController.DoorDown();
      roomLockController.Completed = true;
    }
    if (wasCombatRoom)
    {
      KeyboardLightingManager.TransitionAllKeys(Color.yellow, Color.white, 0.6f, KeyboardLightingManager.F_KEYS);
      RoomLockController.RoomEvent onRoomCleared = RoomLockController.OnRoomCleared;
      if (onRoomCleared != null)
        onRoomCleared();
    }
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

  private void OnTriggerEnter2D(Collider2D collision) => this.count = 0;

  private void OnTriggerStay2D(Collider2D collision)
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

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (!((Object) PlayerFarming.Instance != (Object) null) || !this.Open || this.Completed || !((Object) collision.gameObject == (Object) PlayerFarming.Instance.gameObject))
      return;
    this.inCollision = false;
    if (this.Standalone)
    {
      this.DoorUp();
    }
    else
    {
      if ((Object) BiomeGenerator.Instance != (Object) null)
        BiomeGenerator.Instance.RoomBecameActive();
      RoomLockController.CloseAll();
    }
  }

  public delegate void RoomEvent();
}
