// Decompiled with JetBrains decompiler
// Type: Interaction_Kitchen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Kitchen : Interaction_Cauldron
{
  public Transform SpawnMealPosition;
  public GameObject CookingOn;
  public GameObject CookingOff;
  public SkeletonAnimation CookingMealAnimation;
  public static List<Interaction_Kitchen> Kitchens = new List<Interaction_Kitchen>();
  private UICookingMinigameOverlayController _uiCookingMinigameOverlayController;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  private void ShowCooking(bool Show)
  {
    this.CookingOn.SetActive(Show);
    this.CookingOff.SetActive(!Show);
  }

  private void Start()
  {
    Interaction_Kitchen.Kitchens.Add(this);
    this.ShowCooking(false);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_Kitchen.Kitchens.Remove(this);
  }

  protected override void ShowMenu()
  {
    UIKitchenMenuController kitchenMenuInstance = MonoSingleton<UIManager>.Instance.ShowKitchenMenu(this.structure.Structure_Info);
    UIKitchenMenuController kitchenMenuController1 = kitchenMenuInstance;
    kitchenMenuController1.OnHidden = kitchenMenuController1.OnHidden + (System.Action) (() => kitchenMenuInstance = (UIKitchenMenuController) null);
    UIKitchenMenuController kitchenMenuController2 = kitchenMenuInstance;
    kitchenMenuController2.OnConfirm = kitchenMenuController2.OnConfirm + new System.Action(this.CookAll);
  }

  private void CookAll()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone);
    this.ShowCooking(true);
    this.CookingMealAnimation.AnimationState.SetAnimation(0, "start", false);
    this.CookingMealAnimation.AnimationState.AddAnimation(0, "cook", true, 0.0f);
    PlayerFarming.Instance.GoToAndStop(this.StructureInfo.Position + new Vector3(0.1f, 2.5f), this.transform.parent.gameObject, GoToCallback: (System.Action) (() =>
    {
      this._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.CookingMinigameOverlayControllerTemplate.Instantiate<UICookingMinigameOverlayController>();
      this._uiCookingMinigameOverlayController.Initialise(this.StructureInfo, this);
      this._uiCookingMinigameOverlayController.OnCook += new System.Action(this.OnCook);
      this._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(this.OnUnderCook);
      this._uiCookingMinigameOverlayController.OnBurn += new System.Action(this.OnBurn);
      this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    }));
  }

  private void OnCook()
  {
    AudioManager.Instance.PlayOneShot("event:/cooking/meal_cooked", this.transform.position);
    this.MealFinishedCooking();
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, true))
        follower.Brain.CheckChangeTask();
    }
    if (this.StructureInfo.QueuedMeals.Count > 0)
      return;
    this.EndCooking();
  }

  private void OnUnderCook()
  {
    AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", this.transform.position);
    this.structure.Structure_Info.QueuedMeals[0].MealType = InventoryItem.ITEM_TYPE.MEAL_BURNED;
    this.MealFinishedCooking();
    if (this.StructureInfo.QueuedMeals.Count > 0)
      return;
    this.EndCooking();
  }

  private void OnBurn()
  {
    AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", this.transform.position);
    this.structure.Structure_Info.QueuedMeals[0].MealType = InventoryItem.ITEM_TYPE.MEAL_BURNED;
    this.MealFinishedCooking();
    if (this.StructureInfo.QueuedMeals.Count > 0)
      return;
    this.EndCooking();
  }

  private void EndCooking()
  {
    this.StartCoroutine((IEnumerator) this.DelayHideCooking());
    this._uiCookingMinigameOverlayController.OnCook -= new System.Action(this.OnCook);
    this._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(this.OnUnderCook);
    this._uiCookingMinigameOverlayController.OnBurn -= new System.Action(this.OnBurn);
    this._uiCookingMinigameOverlayController = (UICookingMinigameOverlayController) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  private IEnumerator DelayHideCooking()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.ShowCooking(false);
  }

  private void SkinGiven() => GameManager.GetInstance().OnConversationEnd();

  private IEnumerator CreatePoopSkin()
  {
    Interaction_Kitchen interactionKitchen = this;
    yield return (object) new WaitForSeconds(0.2f);
    interactionKitchen.ShowCooking(false);
    if ((bool) (UnityEngine.Object) interactionKitchen._uiCookingMinigameOverlayController)
    {
      interactionKitchen._uiCookingMinigameOverlayController.OnCook -= new System.Action(interactionKitchen.OnCook);
      interactionKitchen._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(interactionKitchen.OnUnderCook);
      interactionKitchen._uiCookingMinigameOverlayController.OnBurn -= new System.Action(interactionKitchen.OnBurn);
      interactionKitchen._uiCookingMinigameOverlayController.Close();
    }
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 12f);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 3f);
    MMVibrate.RumbleContinuous(0.0f, 1f);
    for (int i = 0; i < UnityEngine.Random.Range(10, 20); ++i)
    {
      // ISSUE: reference to a compiler-generated method
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.POOP, 1, interactionKitchen.SpawnMealPosition.position, (float) UnityEngine.Random.Range(9, 11), new System.Action<PickUp>(interactionKitchen.\u003CCreatePoopSkin\u003Eb__20_0));
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f));
    }
    MMVibrate.StopRumble();
    AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win");
    FollowerSkinCustomTarget.Create(interactionKitchen.SpawnMealPosition.position, PlayerFarming.Instance.transform.position, 1f, "Poop", new System.Action(interactionKitchen.SkinGiven));
  }

  public void MealFinishedCooking()
  {
    Structures_FoodStorage foodStorage = Structures_FoodStorage.GetAvailableFoodStorage(this.StructureInfo.Position, this.StructureInfo.Location);
    InventoryItem.ITEM_TYPE mealType = this.structure.Structure_Info.QueuedMeals[0].MealType;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CookFirstMeal);
    DataManager.Instance.CookedFirstFood = true;
    if (mealType == InventoryItem.ITEM_TYPE.MEAL_POOP)
    {
      ++DataManager.Instance.PoopMealsCreated;
      if (DataManager.Instance.PoopMealsCreated == UnityEngine.Random.Range(5, 12) && !DataManager.GetFollowerSkinUnlocked("Poop"))
        this.StartCoroutine((IEnumerator) this.CreatePoopSkin());
      else if (DataManager.Instance.PoopMealsCreated >= 12 && !DataManager.GetFollowerSkinUnlocked("Poop"))
        this.StartCoroutine((IEnumerator) this.CreatePoopSkin());
    }
    bool flag = false;
    if (foodStorage != null && this.StructureInfo != null)
    {
      foreach (Interaction_FoodStorage foodStorage1 in Interaction_FoodStorage.FoodStorages)
      {
        Interaction_FoodStorage s = foodStorage1;
        if ((UnityEngine.Object) s != (UnityEngine.Object) null && s.StructureInfo.ID == foodStorage.Data.ID && (UnityEngine.Object) s.gameObject != (UnityEngine.Object) null && (UnityEngine.Object) this.transform != (UnityEngine.Object) null)
        {
          ResourceCustomTarget.Create(s.gameObject, this.SpawnMealPosition.position, mealType, (System.Action) (() =>
          {
            AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.transform.position);
            foodStorage.DepositItemUnstacked(mealType);
            s.UpdateFoodDisplayed();
            s.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f, 2).SetEase<Tweener>(Ease.InOutBack);
            foreach (Follower follower in Follower.Followers)
              follower.Brain.CheckChangeTask();
          }));
          flag = true;
          break;
        }
      }
    }
    if (!flag)
    {
      InventoryItem.Spawn(mealType, 1, this.SpawnMealPosition.position, (float) UnityEngine.Random.Range(9, 11), (System.Action<PickUp>) (pickUp =>
      {
        Meal component = pickUp.GetComponent<Meal>();
        component.CreateStructureLocation = this.StructureInfo.Location;
        component.CreateStructureOnStop = true;
      }));
    }
    else
    {
      foreach (Follower follower in Follower.Followers)
        follower.Brain.CheckChangeTask();
    }
    CookingData.CookedMeal(mealType);
    ++DataManager.Instance.MealsCooked;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    this.CookingMealAnimation.AnimationState.SetAnimation(0, "start", false);
    this.CookingMealAnimation.AnimationState.AddAnimation(0, "cook", true, 0.0f);
    this.StructureInfo.QueuedMeals.RemoveAt(0);
    this.StructureInfo.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
    this.StructureInfo.Fuel -= 10;
  }

  public class QueuedMeal
  {
    public InventoryItem.ITEM_TYPE MealType;
    public List<InventoryItem> Ingredients = new List<InventoryItem>();
    public float CookingDuration;
    public float CookedTime = -1f;
  }
}
