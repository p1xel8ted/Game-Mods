// Decompiled with JetBrains decompiler
// Type: LambTownController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

#nullable disable
public class LambTownController : MonoBehaviour
{
  public static LambTownController Instance;
  [SerializeField]
  public DLCRebuildableShop[] shops;
  public Transform DoorStartPosition;
  public Material uberShaderParticleMat;
  [SerializeField]
  public Interaction_SimpleConversation rancherFixedShopB;
  [SerializeField]
  public Interaction_SimpleConversation rancherCompletedJob;
  [SerializeField]
  public Interaction_SimpleConversation rancherCompletedBoard;
  [SerializeField]
  public shopKeeperManager ranchingShop;
  [SerializeField]
  public GameObject grabAnimalConvo;
  [SerializeField]
  public Interaction_SimpleConversation rancherReturnConvoA;
  [SerializeField]
  public Interaction_SimpleConversation rancherReturnConvoB;
  [SerializeField]
  public Interaction_SimpleConversation noAnimalsConvoA;
  [SerializeField]
  public Interaction_SimpleConversation noAnimalsConvoB;
  [SerializeField]
  public Interaction_JobBoard ranchingJobBoard;
  [SerializeField]
  public Interaction_DropOffItem ranchdropOffItem;
  [SerializeField]
  public Interaction_DropOffItem icegoreDropOffItem;
  [SerializeField]
  public Interaction_SacrificeTable icegoreTable;
  public bool noAnimals;
  [SerializeField]
  public Interaction_SimpleConversation legendarySwordHintConvo;
  [SerializeField]
  public Interaction_SimpleConversation legendaryDaggerHintConvo;
  [SerializeField]
  public Interaction_SimpleConversation legendaryGauntletsHintConvo;
  [SerializeField]
  public Interaction_SimpleConversation legendaryBlunderbussHintConvo;
  [SerializeField]
  public Interaction_SimpleConversation legendaryAxeHintConvo;
  [SerializeField]
  public SkeletonAnimation yngyaSpine;
  [SerializeField]
  public Interaction_SimpleConversation yngyaConvo;
  public GameObject spawnedYngyaFollower;
  [Space]
  [SerializeField]
  public GameObject bigStairs;
  [SerializeField]
  public GameObject doors;
  [SerializeField]
  public Interaction revealInteraction;
  [SerializeField]
  public Interaction_RatauShrine[] shrines;
  public string animalAppearOnRachSFX = "event:/dlc/env/woolhaven/animal_appear_on_ranch";

  public Interaction_DropOffItem RanchDropOffItem => this.ranchdropOffItem;

  public Interaction_DropOffItem IcegoreDropOffItem => this.icegoreDropOffItem;

  public Interaction_JobBoard RanchingJobBoard => this.ranchingJobBoard;

  public void Awake()
  {
    LambTownController.Instance = this;
    this.grabAnimalConvo.gameObject.SetActive(false);
  }

  public void Start()
  {
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnWoolToRancher) && !DataManager.Instance.OnboardedWool)
      this.SetSaleAnimal();
    else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) < 2 && DataManager.Instance.RancherShopFixed)
    {
      bool flag = false;
      foreach (Structures_Ranch structuresRanch in StructureManager.GetAllStructuresOfType<Structures_Ranch>())
      {
        if (structuresRanch.Data.Animals.Count > 0)
        {
          foreach (StructuresData.Ranchable_Animal animal in structuresRanch.Data.Animals)
          {
            if ((animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW || animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT || animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA) && animal.State != Interaction_Ranchable.State.Dead)
              flag = true;
          }
        }
      }
      if (!flag && (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_GOAT) > 0 || Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_COW) > 0 || Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ANIMAL_LLAMA) > 0))
        flag = true;
      if (!flag)
      {
        this.SetSaleAnimal();
        this.noAnimalsConvoA.gameObject.SetActive(true);
        this.noAnimals = true;
      }
    }
    this.ranchingShop.gameObject.SetActive(DataManager.Instance.RancherShopFixed);
    this.rancherReturnConvoA.gameObject.SetActive(ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnWoolToRancher) && !DataManager.Instance.OnboardedWool);
    this.CheckLegendaryWeaponsHints();
    this.UpdateRevealInteraction();
    this.bigStairs.gameObject.SetActive(!DataManager.Instance.RevealedPostDLC);
    this.doors.gameObject.SetActive(DataManager.Instance.RevealedPostDLC);
    this.ranchingJobBoard.OnJobCompleted += new Interaction_JobBoard.JobEvent(this.RanchingJobBoard_OnJobCompleted);
    this.ranchingShop.itemSlots[1].transform.parent.gameObject.SetActive(DataManager.Instance.JobBoardsClaimedQuests.Contains(0));
    this.ranchingShop.itemSlots[2].transform.parent.gameObject.SetActive(DataManager.Instance.JobBoardsClaimedQuests.Contains(1));
  }

  public void OnDestroy()
  {
    this.ranchingJobBoard.OnJobCompleted -= new Interaction_JobBoard.JobEvent(this.RanchingJobBoard_OnJobCompleted);
  }

  public void OnEnable()
  {
    this.rancherReturnConvoA.gameObject.SetActive(ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnWoolToRancher) && !DataManager.Instance.OnboardedWool);
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnWoolToRancher) && !DataManager.Instance.OnboardedWool)
    {
      this.SetSaleAnimal();
    }
    else
    {
      this.SetShopAnimals();
      this.ranchingShop.InitDailyShop();
      this.SetQuestAnimals();
    }
    if (!LambTownExecutionerQuestManager.CanPlayExecutionerIndoctrinationSequence && DataManager.Instance.HasFinishedYngyaFlowerBasketQuest && !DataManager.Instance.UnlockedFleeces.Contains(678))
      this.SetupYngyaConvo();
    else
      this.yngyaSpine.gameObject.SetActive(false);
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForPlayer((System.Action) (() =>
    {
      if (!PlayerFarming.IsAnyPlayerInInteractionWithRanchable())
        return;
      if (ObjectiveManager.HasCustomObjective(Objectives.TYPES.GET_ANIMAL))
        LambTownController.Instance.RanchDropOffItem.SetOnState(true);
      LambTownController.Instance.RanchDropOffItem.SetDropOffState(false, false);
    })));
  }

  public void OnDisable()
  {
    this.yngyaConvo.Callback.RemoveListener(new UnityAction(this.UnlockYngyaFleece));
  }

  public void UpdateRevealInteraction()
  {
    this.revealInteraction.gameObject.SetActive(DataManager.Instance.BeatenYngya && !DataManager.Instance.RevealedPostDLC && DataManager.Instance.DLCDungeonNodesCompleted.Count >= 80 /*0x50*/);
  }

  public void Update()
  {
    if (!(bool) (UnityEngine.Object) HUD_Manager.Instance || DataManager.Instance.OnboardedLambTown)
      return;
    HUD_Manager.Instance.Hide(true);
  }

  public void RevealRanching()
  {
    MonoSingleton<UIManager>.Instance.ShowUpgradeTree((System.Action) (() => this.rancherFixedShopB.Play()), UpgradeSystem.Type.RanchingSystem, showDLCTree: true);
  }

  public void GiveAnimal()
  {
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.ranchingShop.itemSlots[0]);
      GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
    }));
    this.ranchingShop.gameObject.SetActive(true);
    this.grabAnimalConvo.gameObject.SetActive(true);
    this.SetSaleAnimal();
  }

  public IEnumerator WaitForPlayer(System.Action callback)
  {
    while (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator WaitForShop(System.Action callback)
  {
    while (!DataManager.Instance.CheckShopExists(this.ranchingShop.Location, this.ranchingShop.gameObject.name))
      yield return (object) null;
    while (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SetSaleAnimal()
  {
    this.SetShopAnimals();
    this.ranchingShop.InitDailyShop();
    if ((UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null)
      return;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForPlayer((System.Action) (() =>
    {
      MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
      this.ranchingShop.ItemsForSale[0].Bought = false;
      this.ranchingShop.ItemsForSale[0].itemToBuy = InventoryItem.ITEM_TYPE.ANIMAL_GOAT;
      this.ranchingShop.ItemsForSale[0].itemCost = DataManager.Instance.RanchingAnimalsAdded == 0 || this.noAnimals ? 0 : 2;
      if (this.ranchingShop.SoldOutSigns.ContainsKey(0))
        UnityEngine.Object.Destroy((UnityEngine.Object) this.ranchingShop.SoldOutSigns[0]);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForShop((System.Action) (() =>
      {
        ShopLocationTracker shop = DataManager.Instance.GetShop(this.ranchingShop.Location, this.ranchingShop.gameObject.name);
        Interaction_BuyItem component = this.ranchingShop.itemSlots[0].GetComponent<Interaction_BuyItem>();
        component.itemForSale = new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_GOAT, InventoryItem.ITEM_TYPE.WOOL, this.ranchingShop.ItemsForSale[0].itemCost);
        component.Activated = false;
        component.SaleIsOn = true;
        component.SaleAmount = DataManager.Instance.RanchingAnimalsAdded == 0 || this.noAnimals ? 10 : 5;
        component.saleAmountFloat = DataManager.Instance.RanchingAnimalsAdded == 0 || this.noAnimals ? 1f : 0.5f;
        component.CreatedDiscount = true;
        component.gameObject.SetActive(true);
        component.Interactable = true;
        shop.itemsForSale[0] = component.itemForSale;
        shop.itemsForSale[0].Bought = false;
        component.OnInteraction += (Interaction.InteractionEvent) (state =>
        {
          MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
          this.grabAnimalConvo.gameObject.SetActive(false);
          if (this.noAnimals)
          {
            this.noAnimals = false;
            this.noAnimalsConvoB.Play();
          }
          else if (!DataManager.Instance.OnboardedWool && DataManager.Instance.RanchingAnimalsAdded > 0)
          {
            ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnWoolToRancher);
            DataManager.Instance.OnboardedWool = true;
            this.rancherReturnConvoB.Play();
            this.rancherReturnConvoB.Callback.AddListener(new UnityAction(this.ranchingJobBoard.Reveal));
          }
          GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() =>
          {
            if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Wool))
              return;
            GameManager.GetInstance().OnConversationNew();
            MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Wool).OnHidden += (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
          }));
        });
        this.ranchingShop.itemSlots[0].GetComponent<InventoryItemDisplay>().SetImage(InventoryItem.ITEM_TYPE.ANIMAL_GOAT, false);
      })));
    })));
  }

  public void SetQuestAnimals()
  {
    Debug.Log((object) nameof (SetQuestAnimals));
    HashSet<InventoryItem.ITEM_TYPE> itemTypeSet;
    using (CollectionPool<HashSet<InventoryItem.ITEM_TYPE>, InventoryItem.ITEM_TYPE>.Get(out itemTypeSet))
    {
      foreach (GameObject itemSlot in this.ranchingShop.itemSlots)
      {
        Interaction_BuyItem component;
        if (itemSlot.TryGetComponent<Interaction_BuyItem>(out component))
          itemTypeSet.Add(component.itemForSale.itemToBuy);
      }
      List<ObjectivesData> customObjectives = ObjectiveManager.GetCustomObjectives(Objectives.TYPES.GET_ANIMAL);
      if (customObjectives.Count <= 0)
        return;
      Debug.Log((object) "Found some GetAnimal objectives...");
      if (!(customObjectives[0] is Objectives_GetAnimal objectivesGetAnimal))
        return;
      InventoryItem.ITEM_TYPE animalType = objectivesGetAnimal.AnimalType;
      Debug.Log((object) $"Found animal objective to get {animalType}");
      Interaction_BuyItem component1;
      if (itemTypeSet.Contains(animalType) || !this.ranchingShop.itemSlots[0].TryGetComponent<Interaction_BuyItem>(out component1))
        return;
      foreach (BuyEntry buyEntry in this.ranchingShop.ItemsForSale)
      {
        if (buyEntry.itemToBuy == animalType)
        {
          buyEntry.pickedForSale = true;
          if (buyEntry.quantity == 0)
            buyEntry.quantity = 1;
          component1.itemForSale = buyEntry;
          component1.GetCost();
          InventoryItemDisplay component2;
          if (this.ranchingShop.itemSlots[0].TryGetComponent<InventoryItemDisplay>(out component2) && component1.itemForSale.itemToBuy != InventoryItem.ITEM_TYPE.NONE)
            component2.SetImage(component1.itemForSale.itemToBuy);
          if (component1.itemForSale.quantity > 1)
            component1.updateQuantity();
        }
      }
    }
  }

  public void GiveRanchingObjectives()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Ranching", Objectives.CustomQuestTypes.RefineChargedRostone), true, true);
    if (DataManager.Instance.RefinedElectrifiedRotstone)
      GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.RefineChargedRostone)));
    ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/Ranching", InventoryItem.ITEM_TYPE.YEW_HOLY, 1), true, true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Ranching", Objectives.CustomQuestTypes.BuildRancherShop), true, true);
  }

  public void BuiltRanchingShop()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuildRancherShop);
    ObjectiveManager.Add((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/BuildRanch", UpgradeSystem.Type.Building_Ranch), true, true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_BuildStructure("Objectives/GroupTitles/BuildRanch", StructureBrain.TYPES.RANCH), true, true);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuildRanch", Objectives.CustomQuestTypes.PlaceAnimalInsideRanch), true, true);
  }

  public void GiveFlockadeShopObjectives()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuildShop", Objectives.CustomQuestTypes.BuildFlockadeShop), true, true);
  }

  public void BuiltFlockadeShop()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuildFlockadeShop);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Flockade", Objectives.CustomQuestTypes.PlayFlockade), true, true);
  }

  public void GiveTarotShopObjectives()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuildShop", Objectives.CustomQuestTypes.BuildTarotShop), true, true);
  }

  public void BuiltTarotShop()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuildTarotShop);
  }

  public void GiveDecoShopObjectives()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuildShop", Objectives.CustomQuestTypes.BuildDecoShop), true, true);
  }

  public void BuiltDecoShop()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuildDecoShop);
  }

  public void GiveBlacksmithShopObjectives()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuildShop", Objectives.CustomQuestTypes.BuildBlacksmithShop), true, true);
  }

  public void BuiltBlacksmithShop()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuildBlacksmithShop);
  }

  public void GiveGraveyardShopObjectives()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuildShop", Objectives.CustomQuestTypes.BuildGraveyardShop), true, true);
  }

  public void BuiltGraveyardShop()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuildGraveyardShop);
  }

  public void CheckLegendaryWeaponsHints()
  {
    if (!DataManager.Instance.OnboardedLegendaryWeapons)
      return;
    if (DataManager.Instance.RancherShopFixed && !DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Blunderbuss_Legendary) && !DataManager.Instance.LegendaryBlunderbussHinted)
    {
      this.legendaryBlunderbussHintConvo.gameObject.SetActive(true);
      this.legendaryBlunderbussHintConvo.Callback.RemoveAllListeners();
      this.legendaryBlunderbussHintConvo.Callback.AddListener((UnityAction) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/LegendaryBlunderbuss", Objectives.CustomQuestTypes.InvestigateFrozenShore), true, true)));
    }
    if (DataManager.Instance.FlockadeShopFixed && !DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Sword_Legendary) && !DataManager.Instance.LegendarySwordHinted)
    {
      this.legendarySwordHintConvo.gameObject.SetActive(true);
      this.legendarySwordHintConvo.Callback.RemoveAllListeners();
    }
    if (DataManager.Instance.TarotShopFixed && !DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Dagger_Legendary) && !DataManager.Instance.LegendaryDaggerHinted)
    {
      this.legendaryDaggerHintConvo.gameObject.SetActive(true);
      this.legendaryDaggerHintConvo.Callback.RemoveAllListeners();
    }
    if (DataManager.Instance.DecoShopFixed && !DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Gauntlet_Legendary) && !DataManager.Instance.LegendaryGauntletsHinted)
    {
      this.legendaryGauntletsHintConvo.gameObject.SetActive(true);
      this.legendaryGauntletsHintConvo.Callback.RemoveAllListeners();
    }
    if (!DataManager.Instance.GraveyardShopFixed || DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Axe_Legendary) || DataManager.Instance.LegendaryAxeHinted)
      return;
    this.legendaryAxeHintConvo.gameObject.SetActive(true);
    this.legendaryAxeHintConvo.Callback.RemoveAllListeners();
  }

  public void RevealDungeonDoors()
  {
    this.StartCoroutine((IEnumerator) this.RevealDungeonDoorsIE());
  }

  public IEnumerator RevealDungeonDoorsIE()
  {
    LambTownController lambTownController = this;
    DataManager.Instance.RevealedPostDLC = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(lambTownController.bigStairs.gameObject);
    bool waiting = true;
    PlayerFarming.Instance.GoToAndStop(new Vector3(18f, 10f), lambTownController.bigStairs, GoToCallback: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) lambTownController.StartCoroutine((IEnumerator) lambTownController.FadeIn());
    lambTownController.bigStairs.gameObject.SetActive(false);
    lambTownController.doors.gameObject.SetActive(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    yield return (object) lambTownController.StartCoroutine((IEnumerator) lambTownController.FadeOut());
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    lambTownController.revealInteraction.gameObject.SetActive(false);
  }

  public IEnumerator FadeIn()
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public IEnumerator FadeOut()
  {
    bool waitingForFade = true;
    MMTransition.ResumePlay((System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public void RanchingJobBoard_OnJobCompleted(ObjectivesData objective)
  {
    this.StartCoroutine((IEnumerator) this.RanchingJobBoard_OnJobCompletedIE(objective));
  }

  public IEnumerator RanchingJobBoard_OnJobCompletedIE(ObjectivesData objective)
  {
    ShopLocationTracker shop = DataManager.Instance.GetShop(this.ranchingShop.Location, this.ranchingShop.gameObject.name);
    Objectives_GetAnimal animalObjective = objective as Objectives_GetAnimal;
    int index = 2;
    int cost = 2;
    InventoryItem.ITEM_TYPE animalType = InventoryItem.ITEM_TYPE.ANIMAL_LLAMA;
    switch (animalObjective.AnimalType)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        animalType = InventoryItem.ITEM_TYPE.ANIMAL_LLAMA;
        index = 1;
        cost = 4;
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        animalType = InventoryItem.ITEM_TYPE.ANIMAL_CRAB;
        cost = 10;
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        animalType = InventoryItem.ITEM_TYPE.ANIMAL_SNAIL;
        cost = 10;
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        animalType = InventoryItem.ITEM_TYPE.ANIMAL_SPIDER;
        cost = 10;
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        animalType = InventoryItem.ITEM_TYPE.ANIMAL_TURTLE;
        cost = 10;
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        animalType = InventoryItem.ITEM_TYPE.ANIMAL_COW;
        cost = 8;
        break;
    }
    if (animalObjective.AnimalType == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
    {
      index = 2;
      animalType = InventoryItem.ITEM_TYPE.ANIMAL_COW;
    }
    if ((UnityEngine.Object) this.rancherCompletedJob != (UnityEngine.Object) null && !this.ranchingJobBoard.BoardCompleted)
    {
      this.rancherCompletedJob.ResetConvo();
      yield return (object) this.rancherCompletedJob.PlayIE();
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.GoToAndStopping)
          player.AbortGoTo();
      }
      yield return (object) new WaitForEndOfFrame();
    }
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.ranchingShop.itemSlots[index].gameObject, 6f);
    yield return (object) new WaitForSeconds(2f);
    this.ranchingShop.ItemsForSale[index].itemToBuy = animalType;
    Interaction_BuyItem component = this.ranchingShop.itemSlots[index].GetComponent<Interaction_BuyItem>();
    component.itemForSale = new BuyEntry(animalType, InventoryItem.ITEM_TYPE.WOOL, cost);
    component.gameObject.SetActive(true);
    component.Interactable = true;
    component.Activated = false;
    shop.itemsForSale[index] = component.itemForSale;
    shop.itemsForSale[index].Bought = false;
    shop.itemsForSale[index].itemCost = cost;
    if (this.ranchingShop.SoldOutSigns.ContainsKey(2))
    {
      this.ranchingShop.SoldOutSigns[2].gameObject.SetActive(false);
      this.ranchingShop.SoldOutSigns.Remove(2);
    }
    this.ranchingShop.itemSlots[index].GetComponent<InventoryItemDisplay>().SetImage(animalType, false);
    this.ranchingShop.itemSlots[index].transform.parent.gameObject.SetActive(true);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.ranchingShop.itemSlots[index].transform.position);
    AudioManager.Instance.PlayOneShot(this.animalAppearOnRachSFX);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationEnd();
    if (animalObjective.AnimalType == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      yield return (object) this.HideRancherJobBoardIE();
    this.SetShopAnimals();
  }

  public IEnumerator HideRancherJobBoardIE()
  {
    yield return (object) this.ranchingJobBoard.HideIE();
    if ((UnityEngine.Object) this.rancherCompletedBoard != (UnityEngine.Object) null)
    {
      this.rancherCompletedBoard.ResetConvo();
      yield return (object) this.rancherCompletedBoard.PlayIE();
    }
  }

  public void SetShopAnimals()
  {
    this.ranchingShop.ItemsForSale.Clear();
    for (int index = 0; index < 4; ++index)
      this.ranchingShop.ItemsForSale.Add(new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_GOAT, InventoryItem.ITEM_TYPE.WOOL, 2));
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(0))
    {
      for (int index = 0; index < 3; ++index)
        this.ranchingShop.ItemsForSale.Add(new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_LLAMA, InventoryItem.ITEM_TYPE.WOOL, 4));
    }
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(1))
    {
      for (int index = 0; index < 2; ++index)
        this.ranchingShop.ItemsForSale.Add(new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_COW, InventoryItem.ITEM_TYPE.WOOL, 8));
    }
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(2))
      this.ranchingShop.ItemsForSale.Add(new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_TURTLE, InventoryItem.ITEM_TYPE.WOOL, 10));
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(3))
      this.ranchingShop.ItemsForSale.Add(new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_CRAB, InventoryItem.ITEM_TYPE.WOOL, 10));
    if (DataManager.Instance.JobBoardsClaimedQuests.Contains(4))
      this.ranchingShop.ItemsForSale.Add(new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_SNAIL, InventoryItem.ITEM_TYPE.WOOL, 10));
    if (!DataManager.Instance.JobBoardsClaimedQuests.Contains(5))
      return;
    this.ranchingShop.ItemsForSale.Add(new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_SPIDER, InventoryItem.ITEM_TYPE.WOOL, 10));
  }

  public void SetPreviousLocation()
  {
    PlayerFarming.Location = FollowerLocation.DLC_ShrineRoom;
    PlayerFarming.LastLocation = FollowerLocation.Base;
  }

  public void SetupYngyaConvo()
  {
    FollowerInfo directInfoAccess = FollowerBrain.FindBrainByID(100007)._directInfoAccess;
    foreach (ConversationEntry entry in this.yngyaConvo.Entries)
      entry.CharacterName = directInfoAccess.Name;
    FollowerBrain.SetFollowerCostume(this.yngyaSpine.Skeleton, directInfoAccess, forceUpdate: true);
    this.yngyaSpine.gameObject.SetActive(true);
    this.yngyaConvo.gameObject.SetActive(true);
    this.yngyaConvo.Callback.AddListener((UnityAction) (() => this.StartCoroutine((IEnumerator) this.WaitForPlayer())));
  }

  public IEnumerator WaitForPlayer()
  {
    while (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    this.UnlockYngyaFleece();
  }

  public void UnlockYngyaFleece()
  {
    this.enabled = true;
    DataManager.Instance.UnlockedFleeces.Add(678);
    UIPlayerUpgradesMenuController upgradesMenuController1 = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate.Instantiate<UIPlayerUpgradesMenuController>();
    upgradesMenuController1.ShowNewFleecesUnlocked(new PlayerFleeceManager.FleeceType[1]
    {
      PlayerFleeceManager.FleeceType.Fleece678
    }, true);
    UIPlayerUpgradesMenuController upgradesMenuController2 = upgradesMenuController1;
    upgradesMenuController2.OnHidden = upgradesMenuController2.OnHidden + (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
  }

  public void UnblockPausing() => MonoSingleton<UIManager>.Instance.ForceBlockPause = false;

  public void BlockPausing() => MonoSingleton<UIManager>.Instance.ForceBlockPause = true;

  [CompilerGenerated]
  public void \u003CRevealRanching\u003Eb__44_0() => this.rancherFixedShopB.Play();

  [CompilerGenerated]
  public void \u003CGiveAnimal\u003Eb__45_0()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.ranchingShop.itemSlots[0]);
    GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
  }

  [CompilerGenerated]
  public void \u003CSetSaleAnimal\u003Eb__48_0()
  {
    MonoSingleton<UIManager>.Instance.ForceBlockPause = true;
    this.ranchingShop.ItemsForSale[0].Bought = false;
    this.ranchingShop.ItemsForSale[0].itemToBuy = InventoryItem.ITEM_TYPE.ANIMAL_GOAT;
    this.ranchingShop.ItemsForSale[0].itemCost = DataManager.Instance.RanchingAnimalsAdded == 0 || this.noAnimals ? 0 : 2;
    if (this.ranchingShop.SoldOutSigns.ContainsKey(0))
      UnityEngine.Object.Destroy((UnityEngine.Object) this.ranchingShop.SoldOutSigns[0]);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForShop((System.Action) (() =>
    {
      ShopLocationTracker shop = DataManager.Instance.GetShop(this.ranchingShop.Location, this.ranchingShop.gameObject.name);
      Interaction_BuyItem component = this.ranchingShop.itemSlots[0].GetComponent<Interaction_BuyItem>();
      component.itemForSale = new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_GOAT, InventoryItem.ITEM_TYPE.WOOL, this.ranchingShop.ItemsForSale[0].itemCost);
      component.Activated = false;
      component.SaleIsOn = true;
      component.SaleAmount = DataManager.Instance.RanchingAnimalsAdded == 0 || this.noAnimals ? 10 : 5;
      component.saleAmountFloat = DataManager.Instance.RanchingAnimalsAdded == 0 || this.noAnimals ? 1f : 0.5f;
      component.CreatedDiscount = true;
      component.gameObject.SetActive(true);
      component.Interactable = true;
      shop.itemsForSale[0] = component.itemForSale;
      shop.itemsForSale[0].Bought = false;
      component.OnInteraction += (Interaction.InteractionEvent) (state =>
      {
        MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
        this.grabAnimalConvo.gameObject.SetActive(false);
        if (this.noAnimals)
        {
          this.noAnimals = false;
          this.noAnimalsConvoB.Play();
        }
        else if (!DataManager.Instance.OnboardedWool && DataManager.Instance.RanchingAnimalsAdded > 0)
        {
          ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnWoolToRancher);
          DataManager.Instance.OnboardedWool = true;
          this.rancherReturnConvoB.Play();
          this.rancherReturnConvoB.Callback.AddListener(new UnityAction(this.ranchingJobBoard.Reveal));
        }
        GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() =>
        {
          if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Wool))
            return;
          GameManager.GetInstance().OnConversationNew();
          MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Wool).OnHidden += (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
        }));
      });
      this.ranchingShop.itemSlots[0].GetComponent<InventoryItemDisplay>().SetImage(InventoryItem.ITEM_TYPE.ANIMAL_GOAT, false);
    })));
  }

  [CompilerGenerated]
  public void \u003CSetSaleAnimal\u003Eb__48_1()
  {
    ShopLocationTracker shop = DataManager.Instance.GetShop(this.ranchingShop.Location, this.ranchingShop.gameObject.name);
    Interaction_BuyItem component = this.ranchingShop.itemSlots[0].GetComponent<Interaction_BuyItem>();
    component.itemForSale = new BuyEntry(InventoryItem.ITEM_TYPE.ANIMAL_GOAT, InventoryItem.ITEM_TYPE.WOOL, this.ranchingShop.ItemsForSale[0].itemCost);
    component.Activated = false;
    component.SaleIsOn = true;
    component.SaleAmount = DataManager.Instance.RanchingAnimalsAdded == 0 || this.noAnimals ? 10 : 5;
    component.saleAmountFloat = DataManager.Instance.RanchingAnimalsAdded == 0 || this.noAnimals ? 1f : 0.5f;
    component.CreatedDiscount = true;
    component.gameObject.SetActive(true);
    component.Interactable = true;
    shop.itemsForSale[0] = component.itemForSale;
    shop.itemsForSale[0].Bought = false;
    component.OnInteraction += (Interaction.InteractionEvent) (state =>
    {
      MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
      this.grabAnimalConvo.gameObject.SetActive(false);
      if (this.noAnimals)
      {
        this.noAnimals = false;
        this.noAnimalsConvoB.Play();
      }
      else if (!DataManager.Instance.OnboardedWool && DataManager.Instance.RanchingAnimalsAdded > 0)
      {
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnWoolToRancher);
        DataManager.Instance.OnboardedWool = true;
        this.rancherReturnConvoB.Play();
        this.rancherReturnConvoB.Callback.AddListener(new UnityAction(this.ranchingJobBoard.Reveal));
      }
      GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() =>
      {
        if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Wool))
          return;
        GameManager.GetInstance().OnConversationNew();
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Wool).OnHidden += (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
      }));
    });
    this.ranchingShop.itemSlots[0].GetComponent<InventoryItemDisplay>().SetImage(InventoryItem.ITEM_TYPE.ANIMAL_GOAT, false);
  }

  [CompilerGenerated]
  public void \u003CSetSaleAnimal\u003Eb__48_2(StateMachine state)
  {
    MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
    this.grabAnimalConvo.gameObject.SetActive(false);
    if (this.noAnimals)
    {
      this.noAnimals = false;
      this.noAnimalsConvoB.Play();
    }
    else if (!DataManager.Instance.OnboardedWool && DataManager.Instance.RanchingAnimalsAdded > 0)
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ReturnWoolToRancher);
      DataManager.Instance.OnboardedWool = true;
      this.rancherReturnConvoB.Play();
      this.rancherReturnConvoB.Callback.AddListener(new UnityAction(this.ranchingJobBoard.Reveal));
    }
    GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() =>
    {
      if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Wool))
        return;
      GameManager.GetInstance().OnConversationNew();
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Wool).OnHidden += (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
    }));
  }

  [CompilerGenerated]
  public void \u003CSetupYngyaConvo\u003Eb__72_0()
  {
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
  }
}
