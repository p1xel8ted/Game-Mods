// Decompiled with JetBrains decompiler
// Type: Interaction_Pub
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Pub : Interaction
{
  public static List<Interaction_Pub> Pubs = new List<Interaction_Pub>();
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public Interaction_FoodStorage foodStorage;
  [SerializeField]
  public GameObject followerPosiiton;
  [Space]
  [SerializeField]
  public GameObject drinkProgressContainer;
  [SerializeField]
  public SpriteRenderer drinkRadialProgress;
  [SerializeField]
  public InventoryItemDisplay itemDisplay;
  [Space]
  [SerializeField]
  public BellTower bell;
  [SerializeField]
  public GameObject drinkingActive;
  [SerializeField]
  public GameObject drinkingInactive;
  [Space]
  [SerializeField]
  public Interaction_ReserveDrink[] seats;
  public Structures_Pub structureBrain;
  public string sLabel;
  public InventoryItem.ITEM_TYPE currentMeal;
  public bool givenOutfit;

  public Structure Structure => this.structure;

  public GameObject FollowerPosiiton => this.followerPosiiton;

  public Structures_Pub Brain => this.structureBrain;

  public void Awake()
  {
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null)
      this.OnBrainAssigned();
    this.UpdateLocalisation();
    Interaction_Pub.Pubs.Add(this);
  }

  public void Start()
  {
    this.drinkRadialProgress.material.SetFloat("_Angle", 90f);
    this.SetSpriteAtlasArcCenterOffset();
  }

  public void OnBrainAssigned()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structureBrain = this.structure.Brain as Structures_Pub;
    this.structureBrain.OnDrinkFinishedBrewing += new Structures_Pub.BrewEvent(this.UpdateCurrentDrink);
    Structure.OnItemDeposited += new Structure.StructureInventoryChanged(this.OnItemDeposited);
    this.UpdateCurrentDrink();
    if (!this.structureBrain.IsContainingFoodStorage)
    {
      this.structureBrain.FoodStorage = new Structures_FoodStorage(0);
      this.structureBrain.FoodStorage.Data = StructuresData.GetInfoByType(StructureBrain.TYPES.FOOD_STORAGE, 0);
      this.structureBrain.FoodStorage.Data.QueuedMeals = new List<Interaction_Kitchen.QueuedMeal>();
      foreach (InventoryItem.ITEM_TYPE queuedResource in this.structureBrain.Data.QueuedResources)
        this.structureBrain.FoodStorage.DepositItemUnstacked(queuedResource);
    }
    this.foodStorage.Structure.Brain = (StructureBrain) this.structureBrain.FoodStorage;
    this.foodStorage.UpdateFoodDisplayed();
    for (int seat = 0; seat < this.seats.Length; ++seat)
      this.seats[seat].Configure(this, seat);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this.structure != (UnityEngine.Object) null)
      this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.structureBrain != null)
    {
      this.structureBrain.OnDrinkFinishedBrewing -= new Structures_Pub.BrewEvent(this.UpdateCurrentDrink);
      Structure.OnItemDeposited -= new Structure.StructureInventoryChanged(this.OnItemDeposited);
    }
    Interaction_Pub.Pubs.Remove(this);
  }

  public void OnItemDeposited(Structure structure, InventoryItem item)
  {
    if (!((UnityEngine.Object) structure == (UnityEngine.Object) this.structure))
      return;
    this.UpdateCurrentDrink();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = LocalizationManager.GetTranslation("Interactions/Brew");
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = this.sLabel;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    GameManager.GetInstance().OnConversationNew();
    HUD_Manager.Instance.Hide(false, 0);
    Time.timeScale = 0.0f;
    UIPubMenuController menuController = MonoSingleton<UIManager>.Instance.PubMenuControllerTemplate.Instantiate<UIPubMenuController>();
    menuController.Show(this.structureBrain.Data, this);
    UIPubMenuController pubMenuController = menuController;
    pubMenuController.OnHidden = pubMenuController.OnHidden + (System.Action) (() =>
    {
      menuController = (UIPubMenuController) null;
      this.HasChanged = true;
      Time.timeScale = 1f;
      PlayerFarming.SetStateForAllPlayers();
      foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
      {
        if (!FollowerManager.FollowerLocked(locationFollower.Brain.Info.ID))
          locationFollower.Brain.CheckChangeTask();
      }
      GameManager.GetInstance().OnConversationEnd();
    });
    menuController.OnItemQueued += (System.Action<InventoryItem.ITEM_TYPE>) (item => this.UpdateCurrentDrink());
    menuController.OnItemRemovedFromQueue += (System.Action<InventoryItem.ITEM_TYPE, int>) ((item, index) =>
    {
      if (index == 0)
        this.structureBrain.Data.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
      this.UpdateCurrentDrink();
    });
  }

  public override void Update()
  {
    base.Update();
    if (this.structureBrain == null)
      return;
    if (this.currentMeal != InventoryItem.ITEM_TYPE.NONE)
      this.DisplayUI();
    else
      this.drinkProgressContainer.gameObject.SetActive(false);
    this.drinkingActive.gameObject.SetActive(Structures_Pub.IsDrinking);
    this.drinkingInactive.gameObject.SetActive(!Structures_Pub.IsDrinking);
    this.bell.Interactable = !Structures_Pub.IsDrinking && this.GetAmountOfPreparedDrinks() > 0;
  }

  public void UpdateCurrentDrink()
  {
    int currentMeal1 = (int) this.currentMeal;
    this.currentMeal = this.structure.Structure_Info.CurrentCookingMeal == null ? (this.structureBrain.GetBestPossibleDrink() == null ? InventoryItem.ITEM_TYPE.NONE : this.structureBrain.GetBestPossibleDrink().MealType) : this.structure.Structure_Info.CurrentCookingMeal.MealType;
    int currentMeal2 = (int) this.currentMeal;
    if (currentMeal1 == currentMeal2)
      return;
    this.itemDisplay.SetImage(this.currentMeal);
  }

  public void RemoveDrinkFromTable(int tablePosition)
  {
    this.foodStorage.itemDisplays[tablePosition].gameObject.SetActive(false);
  }

  public void DisplayUI()
  {
    this.drinkProgressContainer.gameObject.SetActive(true);
    this.drinkRadialProgress.material.SetFloat("_Arc2", (float) ((this.structureBrain.Data.CurrentCookingMeal != null ? (double) this.structureBrain.Data.CurrentCookingMeal.CookedTime / (double) this.structureBrain.Data.CurrentCookingMeal.CookingDuration : 0.0) * 360.0 * -1.0 + 360.0));
  }

  public void CheckDrinksCreated()
  {
    if (DataManager.Instance.drinksCreated <= 15 || this.givenOutfit || DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Special_6) || !DataManager.Instance.TailorEnabled)
      return;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject, 6f);
    this.playerFarming.state.facingAngle = this.playerFarming.state.LookAngle = 0.0f;
    GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, this.playerFarming.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Special_6;
      this.givenOutfit = true;
      GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
    }));
  }

  public void DrinkFinishedBrewing()
  {
    ++DataManager.Instance.drinksCreated;
    this.structure.Brain.Data.QueuedResources.Add(this.currentMeal);
    if ((UnityEngine.Object) this.foodStorage != (UnityEngine.Object) null && this.structureBrain != null)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.transform.position);
      this.foodStorage.StructureBrain.DepositItemUnstacked(this.currentMeal);
      this.foodStorage.UpdateFoodDisplayed();
      this.StartCoroutine((IEnumerator) this.Delay(0.01f, (System.Action) (() => this.foodStorage.itemDisplays[this.structure.Brain.Data.QueuedResources.Count - 1].transform.DOPunchScale(Vector3.one * 0.25f, 0.25f).SetEase<Tweener>(Ease.OutBounce))));
      if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_BEER)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_bubbly_drink", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_COCKTAIL)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_carbonated_drink", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_WINE || this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_GIN)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_MUSHROOM_JUICE || this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_POOP_JUICE)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_icecream", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_EGGNOG)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_crack_egg", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_CHILLI)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_LIGHTNING)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_SIN)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_GRASS)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_flat_drink", this.transform.position);
      else if (this.currentMeal == InventoryItem.ITEM_TYPE.DRINK_GRASS)
        AudioManager.Instance.PlayOneShot("event:/building/brewery/select_icecream", this.transform.position);
    }
    CookingData.CookedMeal(this.currentMeal);
    this.structureBrain.Data.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
    this.structureBrain.Data.Fuel -= 10;
  }

  public void UpdateDisplays() => this.foodStorage.UpdateFoodDisplayed();

  public void BeginDrinking()
  {
    this.structureBrain.BeginDrinking();
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, true, excludeFreezing: true) && follower.Brain.CurrentTaskType != FollowerTaskType.Sleep && follower.Brain.CurrentTaskType != FollowerTaskType.SleepBedRest && follower.Brain.CurrentTaskType != FollowerTaskType.Drinking)
      {
        if (follower.Brain.CurrentTaskType == FollowerTaskType.EatMeal)
          follower.Brain.CurrentTask.Abort();
        int drinkReservedSeat = this.structureBrain.GetFollowerDrinkReservedSeat(follower.Brain.Info.ID);
        if (drinkReservedSeat != -1)
        {
          follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Drink(drinkReservedSeat, this.structureBrain.FoodStorage.Data.Inventory[drinkReservedSeat], this.structureBrain));
        }
        else
        {
          follower.Brain.SetPersonalOverrideTask(FollowerTaskType.Drinking);
          follower.Brain.CheckChangeTask();
        }
      }
    }
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.OpenPub);
    this.CheckDrinksCreated();
  }

  public void StopDrinking() => this.structureBrain.StopDrinking();

  public Vector3 GetSeatPosition(int pos)
  {
    return this.seats[Mathf.Clamp(pos, 0, this.seats.Length - 1)].transform.position;
  }

  public int GetAmountOfPreparedDrinks()
  {
    int ofPreparedDrinks = 0;
    for (int index = 0; index < this.foodStorage.StructureBrain.Data.Inventory.Count; ++index)
    {
      if (this.foodStorage.StructureBrain.Data.Inventory[index] != null)
        ++ofPreparedDrinks;
    }
    return ofPreparedDrinks;
  }

  public IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SetSpriteAtlasArcCenterOffset()
  {
    Vector2 center = this.drinkRadialProgress.sprite.textureRect.center;
    this.drinkRadialProgress.material.SetVector("_ArcCenterOffset", (Vector4) (new Vector2(center.x / (float) this.drinkRadialProgress.sprite.texture.width, center.y / (float) this.drinkRadialProgress.sprite.texture.height) - Vector2.one * 0.5f));
  }

  [CompilerGenerated]
  public void \u003CCheckDrinksCreated\u003Eb__33_0()
  {
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, this.playerFarming.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Special_6;
    this.givenOutfit = true;
    GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
  }

  [CompilerGenerated]
  public void \u003CDrinkFinishedBrewing\u003Eb__34_0()
  {
    this.foodStorage.itemDisplays[this.structure.Brain.Data.QueuedResources.Count - 1].transform.DOPunchScale(Vector3.one * 0.25f, 0.25f).SetEase<Tweener>(Ease.OutBounce);
  }
}
