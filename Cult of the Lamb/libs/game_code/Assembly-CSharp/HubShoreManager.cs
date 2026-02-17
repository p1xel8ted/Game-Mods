// Decompiled with JetBrains decompiler
// Type: HubShoreManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HubShoreManager : BaseMonoBehaviour
{
  public GameObject[] ShopKeepers;
  public Interaction_LighthouseBurner lighthouseBurner;
  public GameObject GemUnlit;
  public GameObject Gem;
  public GameObject Boat;
  public GameObject Demons;
  public GameObject HideAfterShopsAppear;
  public SpriteRenderer LighthouseDoorSpriteRenderer;
  public Sprite LighthouseDoorClosed;
  public Sprite LighthouseDoorOpen;
  public EnterBuilding LighthouseEntrance;
  public GameObject colliderAndBark;
  public Transform LighthouseKeeper;
  public RoomSwapManager RoomSwapManager;
  public bool Activated;
  public bool LighthouseLit;
  public int startingDay;
  public GameObject LambPropaganda;
  public GameObject CaughtFishConvo;
  public GameObject DidntCatchFishConvo;
  public GameObject FishermanTrader;
  public GameObject FishermanConvo;
  public GameObject Fisherman;
  public GameObject GiveCrystalQuestConvo;
  public GameObject CompleteCrystalQuestConvo;
  public List<GameObject> CrystalitemsToTurnOn = new List<GameObject>();
  public GameObject Shop;

  public void GiveObjectiveFixLighthouse()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FixTheLighthouse", Objectives.CustomQuestTypes.FixTheLighthouse));
    this.CheckRequirements();
  }

  public void CompleteObjectiveFixLighthouse()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FixTheLighthouse);
    DataManager.Instance.Lighthouse_Lit = true;
    this.StartCoroutine((IEnumerator) this.GiveSkinAndOpenShop());
  }

  public IEnumerator GiveSkinAndOpenShop()
  {
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.Shop.gameObject, 6f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(0.1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(1.5f);
    this.GotItem();
  }

  public void CheckMusicRoutine() => this.StartCoroutine((IEnumerator) this.CheckMusic());

  public IEnumerator CheckMusic()
  {
    yield return (object) new WaitForSeconds(0.1f);
    if (DataManager.Instance.Lighthouse_Lit)
      AudioManager.Instance.SetMusicRoomID(6, "shore_id");
    else
      AudioManager.Instance.SetMusicRoomID(0, "shore_id");
  }

  public void GotItem()
  {
    this.OpenShop();
    this.CheckRequirements();
    GameManager.GetInstance().OnConversationEnd();
  }

  public void RevealShop()
  {
    this.OpenShop();
    Vector3 zero = Vector3.zero;
    this.Shop.transform.localPosition = new Vector3(0.0f, 0.0f, 1f);
    this.Shop.transform.DOLocalMove(zero, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    CultFaithManager.AddThought(Thought.Cult_PledgedToYou);
  }

  public void OnEnable()
  {
    this.CheckRequirements();
    this.startingDay = TimeManager.CurrentDay;
    this.CheckHasDoneTutorial();
    this.CheckMusicRoutine();
    this.GiveCrystalQuestConvo.SetActive(!DataManager.Instance.Lighthouse_QuestGiven && DataManager.Instance.Lighthouse_LitFirstConvo);
    this.CompleteCrystalQuestConvo.SetActive((DataManager.Instance.Lighthouse_QuestGiven && !DataManager.Instance.CompletedLighthouseCrystalQuest || ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LighthouseReturn)) && (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LighthouseReturn) || Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL) >= 25));
    this.FishermanConvo.gameObject.SetActive(DataManager.Instance.CompletedLighthouseCrystalQuest);
  }

  public void OpenLighthouse()
  {
    this.LighthouseEntrance.gameObject.SetActive(true);
    this.colliderAndBark.SetActive(false);
    this.LighthouseDoorSpriteRenderer.sprite = this.LighthouseDoorOpen;
  }

  public void CheckHasDoneTutorial()
  {
    this.LighthouseEntrance.gameObject.SetActive(true);
    this.colliderAndBark.SetActive(false);
  }

  public void CheckRequirements()
  {
    if (DataManager.Instance.Lighthouse_Lit)
    {
      this.RoomSwapManager.TransitionOutRoomId = 6;
      Debug.Log((object) "LighthouseLit");
      this.Gem.SetActive(true);
      this.GemUnlit.SetActive(false);
      this.SpawnShopKeeper();
      if ((UnityEngine.Object) this.HideAfterShopsAppear != (UnityEngine.Object) null)
        this.HideAfterShopsAppear.SetActive(false);
      this.OpenShop();
      this.Demons.SetActive(false);
    }
    else
    {
      Debug.Log((object) "LighthouseUnlit");
      this.HideAfterShopsAppear.SetActive(true);
      this.DisableShopKeepers();
      this.CloseShop();
      this.GemUnlit.SetActive(true);
      this.Gem.SetActive(false);
    }
    if (DataManager.Instance.CompletedLighthouseCrystalQuest)
    {
      foreach (GameObject gameObject in this.CrystalitemsToTurnOn)
        gameObject.gameObject.SetActive(true);
    }
    else
    {
      foreach (GameObject gameObject in this.CrystalitemsToTurnOn)
        gameObject.gameObject.SetActive(false);
    }
  }

  public void OpenShop() => this.Shop.SetActive(true);

  public void CloseShop() => this.Shop.SetActive(false);

  public void DisableShopKeepers()
  {
    this.LambPropaganda.SetActive(false);
    foreach (GameObject shopKeeper in this.ShopKeepers)
      shopKeeper.SetActive(false);
    this.Activated = false;
  }

  public void SpawnShopKeeper()
  {
    this.LambPropaganda.SetActive(true);
    foreach (GameObject shopKeeper in this.ShopKeepers)
      shopKeeper.SetActive(true);
    Debug.Log((object) "ActivateShopKeeper");
    this.Activated = true;
  }

  public void Update()
  {
    if (this.startingDay == TimeManager.CurrentDay)
      return;
    this.CheckRequirements();
    this.startingDay = TimeManager.CurrentDay;
  }

  public void GiveCrystalQuest()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/CrystalForLighthouse", InventoryItem.ITEM_TYPE.CRYSTAL, 25, targetLocation: FollowerLocation.Dungeon1_3)
    {
      CustomTerm = "Objectives/Custom/CrystalForLighthouse"
    });
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL) < 25)
      return;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/CrystalForLighthouse", Objectives.CustomQuestTypes.LighthouseReturn));
    this.CompleteCrystalQuestConvo.GetComponent<Interaction_SimpleConversation>().AutomaticallyInteract = false;
    this.CompleteCrystalQuestConvo.SetActive(true);
  }

  public void CompleteCrystalQuest()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.LighthouseReturn);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL, -25);
    DataManager.Instance.CompletedLighthouseCrystalQuest = true;
    this.StartCoroutine((IEnumerator) this.GiveItemsRoutine(InventoryItem.ITEM_TYPE.CRYSTAL, 25));
  }

  public IEnumerator GiveItemsRoutine(InventoryItem.ITEM_TYPE itemType, int quantity)
  {
    HubShoreManager hubShoreManager = this;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    hubShoreManager.CheckRequirements();
    GameManager.GetInstance().OnConversationNew();
    for (int i = 0; i < Mathf.Max(quantity, 10); ++i)
    {
      ResourceCustomTarget.Create(hubShoreManager.CompleteCrystalQuestConvo, PlayerFarming.Instance.transform.position, itemType, (System.Action) null);
      yield return (object) new WaitForSeconds(0.025f);
    }
    hubShoreManager.StartCoroutine((IEnumerator) hubShoreManager.GiveCrystalSkin());
  }

  public IEnumerator GiveCrystalSkin()
  {
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(0.1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    bool waiting = true;
    FollowerSkinCustomTarget.Create(this.lighthouseBurner.leader.gameObject.transform.position, PlayerFarming.Instance.gameObject.transform.position, 0.5f, "Axolotl", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForEndOfFrame();
    this.FishermanConvo.SetActive(true);
  }
}
