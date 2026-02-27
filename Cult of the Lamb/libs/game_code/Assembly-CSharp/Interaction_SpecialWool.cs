// Decompiled with JetBrains decompiler
// Type: Interaction_SpecialWool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_SpecialWool : Interaction
{
  public PickUp pickUp;
  public InventoryItem.ITEM_TYPE type;
  public ParticleSystem Particles;
  public SpriteRenderer Image;
  public static Interaction_SpecialWool Instance;
  public float Delay = 0.5f;
  [SerializeField]
  public Interaction_SpecialWool additionalWool;
  [SerializeField]
  public InventoryItem.ITEM_TYPE overrideType;
  [SerializeField]
  public UnityEvent callback;
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
  public string LabelName;
  public Vector3 BookTargetPosition;
  public float Timer;

  public bool hasAdditionalWool
  {
    get
    {
      return DataManager.Instance.PuzzleRoomsCompleted > 1 && DataManager.Instance.PuzzleRoomsCompleted != 4 && (Object) this.additionalWool != (Object) null;
    }
  }

  public int index
  {
    get
    {
      return !this.hasAdditionalWool ? DataManager.Instance.NPCRescueRoomsCompleted : DataManager.Instance.NPCRescueRoomsCompleted + 1;
    }
  }

  public void Awake()
  {
    if ((Object) this.pickUp != (Object) null)
      this.pickUp.SetIgnoreBoundsCheck();
    if (!this.hasAdditionalWool)
      return;
    this.additionalWool.gameObject.SetActive(true);
    this.transform.position = new Vector3(this.transform.position.x - 0.5f, this.transform.position.y, this.transform.position.z);
  }

  public void Start()
  {
    if (this.overrideType != InventoryItem.ITEM_TYPE.NONE)
    {
      this.type = this.overrideType;
      this.pickUp.type = this.overrideType;
      this.UpdateLocalisation();
    }
    else
    {
      this.type = this.Rewards[this.index];
      this.pickUp.type = this.type;
      this.pickUp.SetImage(this.type);
      this.UpdateLocalisation();
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.LabelName = InventoryItem.LocalizedName(this.type);
  }

  public override void GetLabel()
  {
    if ((double) this.Delay < 0.0)
      this.Label = this.LabelName;
    else
      this.Label = "";
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.Particles.Stop();
    Interaction_SpecialWool.Instance = this;
    this.StartCoroutine(this.DelayDoTween());
  }

  public IEnumerator DelayDoTween()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_SpecialWool interactionSpecialWool = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionSpecialWool.gameObject.GetComponent<PickUp>().enabled = false;
      interactionSpecialWool.Image.gameObject.transform.DOLocalMoveZ(-0.4f, 1.5f).SetLoops<TweenerCore<Vector3, Vector3, VectorOptions>>(-1, DG.Tweening.LoopType.Yoyo).SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void Update()
  {
    base.Update();
    this.Delay -= Time.deltaTime;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (!((Object) Interaction_SpecialWool.Instance == (Object) this))
      return;
    Interaction_SpecialWool.Instance = (Interaction_SpecialWool) null;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.gameObject.GetComponent<PickUp>().enabled = false;
    this.Image.gameObject.transform.DOKill();
    this.StartCoroutine(this.PlayerPickUpBook());
    if (!GameManager.IsDungeon(PlayerFarming.Location))
      return;
    PlayerReturnToBase.Disabled = true;
  }

  public IEnumerator PlayerPickUpBook()
  {
    Interaction_SpecialWool interactionSpecialWool = this;
    AudioManager.Instance.PlayOneShot("event:/dlc/music/puzzle_room/stinger_wool_get");
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionSpecialWool.transform.position);
    interactionSpecialWool.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionSpecialWool.playerFarming.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    interactionSpecialWool.state.gameObject.GetComponent<PlayerSimpleInventory>();
    interactionSpecialWool.BookTargetPosition = interactionSpecialWool.state.transform.position + new Vector3(0.0f, 0.2f, -1.2f);
    interactionSpecialWool.state.CURRENT_STATE = StateMachine.State.FoundItem;
    interactionSpecialWool.Particles.Play();
    interactionSpecialWool.Timer = 0.0f;
    while ((double) (interactionSpecialWool.Timer += Time.deltaTime) < 2.0)
    {
      interactionSpecialWool.transform.position = Vector3.Lerp(interactionSpecialWool.transform.position, interactionSpecialWool.BookTargetPosition, interactionSpecialWool.Timer / 2f);
      yield return (object) null;
    }
    interactionSpecialWool.transform.position = interactionSpecialWool.BookTargetPosition;
    Inventory.AddItem((int) interactionSpecialWool.type, 1);
    interactionSpecialWool.Particles.Stop();
    yield return (object) new WaitForSeconds(0.5f);
    interactionSpecialWool.Image.enabled = false;
    interactionSpecialWool.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    if (interactionSpecialWool.overrideType == InventoryItem.ITEM_TYPE.NONE)
    {
      ++DataManager.Instance.NPCRescueRoomsCompleted;
      if (DataManager.Instance.PuzzleRoomsCompleted == 2 && DataManager.Instance.NPCRescueRoomsCompleted >= 3 || DataManager.Instance.PuzzleRoomsCompleted <= 6 && DataManager.Instance.NPCRescueRoomsCompleted >= 10 || DataManager.Instance.PuzzleRoomsCompleted <= 5 && DataManager.Instance.NPCRescueRoomsCompleted >= 8 || DataManager.Instance.PuzzleRoomsCompleted <= 4 && DataManager.Instance.NPCRescueRoomsCompleted >= 6 || DataManager.Instance.PuzzleRoomsCompleted <= 3 && DataManager.Instance.NPCRescueRoomsCompleted >= 5 || DataManager.Instance.PuzzleRoomsCompleted <= 1)
        PuzzleController.Instance.RevealRewardPodiums();
    }
    interactionSpecialWool.callback?.Invoke();
    Object.Destroy((Object) interactionSpecialWool.gameObject);
  }
}
