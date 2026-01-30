// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerKitchen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using Lamb.UI.KitchenMenu;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FollowerKitchen : Interaction_Kitchen
{
  public static List<Interaction_FollowerKitchen> FollowerKitchens = new List<Interaction_FollowerKitchen>();
  [SerializeField]
  public GameObject cookProgressContainer;
  [SerializeField]
  public SpriteRenderer cookRadialProgress;
  [SerializeField]
  public InventoryItemDisplay itemDisplay;
  [SerializeField]
  public Interaction_FoodStorage foodStorage;
  [SerializeField]
  public GameObject cookPosition;
  public List<InventoryItem> _itemsInTheAir = new List<InventoryItem>();
  public InventoryItem.ITEM_TYPE currentMeal;
  public bool soundLoopActive;

  public GameObject CookPosition => this.cookPosition;

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_FollowerKitchen.FollowerKitchens.Add(this);
    if (this.structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_FollowerKitchen.FollowerKitchens.Remove(this);
    if ((UnityEngine.Object) this.structure != (UnityEngine.Object) null && this.structure.Brain != null)
    {
      ((Structures_Kitchen) this.structure.Brain).OnMealFinishedCooking -= new Structures_Kitchen.CookEvent(this.UpdateCurrentMeal);
      Structure.OnItemDeposited -= new Structure.StructureInventoryChanged(this.OnItemDeposited);
    }
    this.soundLoopActive = false;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public override void OnBrainAssigned()
  {
    base.OnBrainAssigned();
    ((Structures_Kitchen) this.structure.Brain).OnMealFinishedCooking += new Structures_Kitchen.CookEvent(this.UpdateCurrentMeal);
    Structure.OnItemDeposited += new Structure.StructureInventoryChanged(this.OnItemDeposited);
    this.UpdateCurrentMeal();
    Structures_Kitchen brain = (Structures_Kitchen) this.structure.Brain;
    if (!brain.IsContainingFoodStorage)
    {
      brain.FoodStorage = new Structures_FoodStorage(0);
      brain.FoodStorage.Data = StructuresData.GetInfoByType(StructureBrain.TYPES.FOOD_STORAGE, 0);
      brain.FoodStorage.Data.QueuedMeals = new List<Interaction_Kitchen.QueuedMeal>();
      foreach (InventoryItem.ITEM_TYPE queuedResource in brain.Data.QueuedResources)
        brain.FoodStorage.DepositItemUnstacked(queuedResource);
    }
    if (this.currentMeal != InventoryItem.ITEM_TYPE.NONE)
      this.CookingMealAnimation.AnimationState.SetAnimation(0, "cook", true);
    this.foodStorage.Structure.Brain = (StructureBrain) brain.FoodStorage;
    this.foodStorage.UpdateFoodDisplayed();
  }

  public void OnItemDeposited(Structure structure, InventoryItem item)
  {
    if (!((UnityEngine.Object) structure == (UnityEngine.Object) this.structure))
      return;
    this.UpdateCurrentMeal();
  }

  public void UpdateCurrentMeal()
  {
    if (this.structure.Structure_Info.CurrentCookingMeal != null)
      this.currentMeal = this.structure.Structure_Info.CurrentCookingMeal.MealType;
    else if (((Structures_Kitchen) this.structure.Brain).GetBestPossibleMeal() != null)
      this.currentMeal = ((Structures_Kitchen) this.structure.Brain).GetBestPossibleMeal().MealType;
    else
      this.currentMeal = InventoryItem.ITEM_TYPE.NONE;
  }

  public override void Start() => this.SetSpriteAtlasArcCenterOffset();

  public override void Update()
  {
    base.Update();
    if (this.StructureInfo != null)
    {
      if (this.currentMeal != InventoryItem.ITEM_TYPE.NONE)
        this.DisplayUI();
      else
        this.cookProgressContainer.gameObject.SetActive(false);
    }
    this.CookingOn.SetActive(this.currentMeal != 0);
    this.CookingOff.SetActive(this.currentMeal == InventoryItem.ITEM_TYPE.NONE);
    if (this.currentMeal != InventoryItem.ITEM_TYPE.NONE && !this.soundLoopActive)
    {
      this.soundLoopActive = true;
      this.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/cooking/cooking_loop", this.gameObject, true);
    }
    else if (this.soundLoopActive && this.currentMeal == InventoryItem.ITEM_TYPE.NONE)
    {
      this.soundLoopActive = false;
      AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    }
    if (!((UnityEngine.Object) this.structure != (UnityEngine.Object) null) || this.structure.Brain == null)
      return;
    this.CookingMealAnimation.gameObject.SetActive(this.currentMeal != 0);
    this.CookingMealAnimation.transform.position = this.foodStorage.itemDisplays[Mathf.Clamp(this.structure.Brain.Data.QueuedResources.Count, 0, this.foodStorage.StructureBrain.Capacity - 1)].transform.position;
  }

  public void DisplayUI()
  {
    this.cookProgressContainer.gameObject.SetActive(true);
    InventoryItem.ITEM_TYPE Type = InventoryItem.ITEM_TYPE.NONE;
    if (this.structure.Structure_Info.CurrentCookingMeal != null)
      Type = this.structure.Structure_Info.CurrentCookingMeal.MealType;
    else if (((Structures_Kitchen) this.structure.Brain).GetBestPossibleMeal() != null)
      Type = ((Structures_Kitchen) this.structure.Brain).GetBestPossibleMeal().MealType;
    this.itemDisplay.SetImage(Type, false);
    this.SetProgress(this.StructureInfo.CurrentCookingMeal != null ? this.StructureInfo.CurrentCookingMeal.CookedTime / this.StructureInfo.CurrentCookingMeal.CookingDuration : 0.0f);
  }

  public override void ShowMenu()
  {
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    this.state.facingAngle = Utils.GetAngle(this.state.transform.position, this.transform.position);
    GameManager.GetInstance().OnConversationNew();
    HUD_Manager.Instance.Hide(false, 0);
    Time.timeScale = 0.0f;
    UIFollowerKitchenMenuController menuController = MonoSingleton<UIManager>.Instance.FollowerKitchenMenuControllerTemplate.Instantiate<UIFollowerKitchenMenuController>();
    menuController.Show(this.StructureInfo, this);
    UIFollowerKitchenMenuController kitchenMenuController = menuController;
    kitchenMenuController.OnHidden = kitchenMenuController.OnHidden + (System.Action) (() =>
    {
      menuController = (UIFollowerKitchenMenuController) null;
      this.HasChanged = true;
      Time.timeScale = 1f;
      PlayerFarming.SetStateForAllPlayers();
      foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
        locationFollower.Brain.CheckChangeTask();
      GameManager.GetInstance().OnConversationEnd();
    });
    menuController.OnItemQueued += (System.Action<InventoryItem.ITEM_TYPE>) (item => this.UpdateCurrentMeal());
    menuController.OnItemRemovedFromQueue += (System.Action<InventoryItem.ITEM_TYPE, int>) ((item, index) =>
    {
      if (index == 0)
        this.StructureInfo.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
      this.UpdateCurrentMeal();
    });
  }

  public void DepositItem()
  {
    this.structure.DepositInventory(this._itemsInTheAir[0]);
    this._itemsInTheAir.RemoveAt(0);
  }

  public override void MealFinishedCooking()
  {
    if (this.currentMeal == InventoryItem.ITEM_TYPE.NONE)
      this.currentMeal = this.structure.Structure_Info.CurrentCookingMeal == null ? (((Structures_Kitchen) this.structure.Brain).GetBestPossibleMeal() == null ? InventoryItem.ITEM_TYPE.NONE : ((Structures_Kitchen) this.structure.Brain).GetBestPossibleMeal().MealType) : this.structure.Structure_Info.CurrentCookingMeal.MealType;
    MeshRenderer CookingMealAnimationMeshRenderer = (MeshRenderer) null;
    try
    {
      CookingMealAnimationMeshRenderer = this.CookingMealAnimation.gameObject.GetComponent<MeshRenderer>();
    }
    catch
    {
      Debug.LogWarning((object) "Attempt to get CookingMealAnimation component MeshRenderer failed");
    }
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CookFirstMeal);
    DataManager.Instance.CookedFirstFood = true;
    this.structure.Brain.Data.QueuedResources.Add(this.currentMeal);
    if ((UnityEngine.Object) this.foodStorage != (UnityEngine.Object) null && this.StructureInfo != null)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.transform.position);
      this.foodStorage.StructureBrain.DepositItemUnstacked(this.currentMeal);
      this.foodStorage.UpdateFoodDisplayed();
      this.StartCoroutine((IEnumerator) this.Delay(0.01f, (System.Action) (() =>
      {
        this.CookingMealAnimation.gameObject.SetActive(false);
        int index = this.structure.Brain.Data.QueuedResources.Count - 1;
        if (index < 0 || index >= this.foodStorage.itemDisplays.Length || !((UnityEngine.Object) this.foodStorage.itemDisplays[index] != (UnityEngine.Object) null))
          return;
        this.foodStorage.itemDisplays[index].transform.DOPunchScale(Vector3.one * 0.25f, 0.25f).SetEase<Tweener>(Ease.OutBounce);
      })));
    }
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType != FollowerTaskType.Cook)
        follower.Brain.CheckChangeTask();
    }
    CookingData.CookedMeal(this.currentMeal);
    ++DataManager.Instance.MealsCooked;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    this.StructureInfo.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
    this.StructureInfo.Fuel -= 10;
    if (((Structures_Kitchen) this.structure.Brain).FoodStorage.Data.QueuedResources.Count >= this.foodStorage.StructureBrain.Capacity - 1)
      return;
    this.CookingMealAnimation.AnimationState.SetAnimation(0, "start", false);
    this.CookingMealAnimation.AnimationState.AddAnimation(0, "cook", true, 0.0f);
    this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() =>
    {
      if (!((UnityEngine.Object) CookingMealAnimationMeshRenderer != (UnityEngine.Object) null))
        return;
      CookingMealAnimationMeshRenderer.enabled = true;
    })));
  }

  public IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SetProgress(float normalizedProgress)
  {
    this.cookRadialProgress.material.SetFloat("_Arc2", (float) (360.0 * (1.0 - (double) normalizedProgress)));
  }

  public void SetSpriteAtlasArcCenterOffset()
  {
    Vector2 center = this.cookRadialProgress.sprite.textureRect.center;
    this.cookRadialProgress.material.SetVector("_ArcCenterOffset", (Vector4) (new Vector2(center.x / (float) this.cookRadialProgress.sprite.texture.width, center.y / (float) this.cookRadialProgress.sprite.texture.height) - Vector2.one * 0.5f));
  }
}
