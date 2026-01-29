// Decompiled with JetBrains decompiler
// Type: RescueNPCGraveController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class RescueNPCGraveController : BaseMonoBehaviour
{
  public static RescueNPCGraveController Instance;
  [SerializeField]
  public bool isSecondary;
  [SerializeField]
  public Interaction_Generic[] npcs;
  public UnityEvent Callbacks;
  public List<InventoryItem.ITEM_TYPE> Rewards = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_LAMBWAR,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_TAROT,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_BLACKSMITH,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_DECORATION,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_6,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_7,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_8,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_9,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_10
  };
  public InventoryItem.ITEM_TYPE assignedReward;
  public bool configured;

  public int Index => DataManager.Instance.NPCRescueRoomsCompleted;

  public void OnEnable() => this.StartCoroutine((IEnumerator) this.WaitForPlayer());

  public void Awake() => RescueNPCGraveController.Instance = this;

  public IEnumerator WaitForPlayer()
  {
    while ((UnityEngine.Object) GameObject.FindGameObjectWithTag("Player") == (UnityEngine.Object) null)
      yield return (object) null;
    if (!this.configured)
    {
      this.configured = true;
      for (int index = 0; index < this.npcs.Length; ++index)
        this.npcs[index].gameObject.SetActive(this.Index + (this.isSecondary ? 1 : 0) == index);
      this.assignedReward = this.Rewards[this.Index + (this.isSecondary ? 1 : 0)];
      BiomeGenerator.Instance.CurrentRoom.Active = true;
      BlockingDoor.CloseAll();
      RoomLockController.CloseAll();
    }
  }

  public void RevealItemNPC() => this.StartCoroutine((IEnumerator) this.RevealItemRoutine());

  public IEnumerator RevealItemRoutine()
  {
    RescueNPCGraveController npcGraveController = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
    bool Wait = true;
    PlayerFarming.Instance.GoToAndStop(npcGraveController.transform.position + new Vector3(0.5f, -1f), npcGraveController.gameObject, GoToCallback: (System.Action) (() => Wait = false));
    while (Wait)
      yield return (object) null;
    PlayerFarming.Instance.CustomAnimation("actions/dig", false);
    yield return (object) new WaitForSeconds(0.466666669f);
    CameraManager.instance.ShakeCameraForDuration(1.3f, 1.4f, 0.4f);
    BiomeConstants.Instance.EmitParticleChunk(BiomeConstants.TypeOfParticle.stone, npcGraveController.transform.position, Vector3.forward * 50f, 20);
    AudioManager.Instance.PlayOneShot("event:/followers/break_free");
    yield return (object) npcGraveController.StartCoroutine((IEnumerator) npcGraveController.PlayerPickUpBook());
    ++DataManager.Instance.NPCRescueRoomsCompleted;
    GameManager.GetInstance().OnConversationEnd();
    if (DataManager.Instance.PuzzleRoomsCompleted == 2 && DataManager.Instance.NPCRescueRoomsCompleted >= 3 || DataManager.Instance.PuzzleRoomsCompleted <= 3 && DataManager.Instance.NPCRescueRoomsCompleted >= 5 || DataManager.Instance.PuzzleRoomsCompleted <= 1 || DataManager.Instance.PuzzleRoomsCompleted > 3)
      PuzzleController.Instance.RevealRewardPodiums();
  }

  public IEnumerator PlayerPickUpBook()
  {
    RescueNPCGraveController npcGraveController = this;
    PickUp pickup = (PickUp) null;
    InventoryItem.Spawn(npcGraveController.assignedReward, 1, npcGraveController.transform.position, result: (Action<PickUp>) (p => pickup = p));
    npcGraveController.Callbacks?.Invoke();
    while ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
      yield return (object) null;
    pickup.enabled = false;
    pickup.child.transform.localScale = Vector3.one;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", npcGraveController.transform.position);
    float Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    while ((double) (Timer += Time.deltaTime) < 2.0)
    {
      pickup.transform.position = Vector3.Lerp(pickup.transform.position, BookTargetPosition, 5f * Time.deltaTime);
      yield return (object) null;
    }
    pickup.transform.position = BookTargetPosition;
    Inventory.AddItem(npcGraveController.assignedReward, 1);
    yield return (object) new WaitForSeconds(0.5f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) pickup.gameObject);
  }
}
