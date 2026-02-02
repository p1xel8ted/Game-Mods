// Decompiled with JetBrains decompiler
// Type: Interaction_Kitchen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MessagePack;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Kitchen : Interaction_Cauldron
{
  public Transform SpawnMealPosition;
  public GameObject CookingOn;
  public GameObject CookingOff;
  public SkeletonAnimation CookingMealAnimation;
  public bool isLoadingAssets;
  public static List<Interaction_Kitchen> Kitchens = new List<Interaction_Kitchen>();
  public UICookingMinigameOverlayController _uiCookingMinigameOverlayController;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public void ShowCooking(bool Show)
  {
    this.CookingOn.SetActive(Show);
    this.CookingOff.SetActive(!Show);
  }

  public virtual void Start()
  {
    Interaction_Kitchen.Kitchens.Add(this);
    this.ShowCooking(false);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_Kitchen.Kitchens.Remove(this);
  }

  public override void OnBrainAssigned()
  {
    base.OnBrainAssigned();
    if (!DataManager.Instance.SurvivalModeActive || !DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.PlayerHunger) || !DataManager.Instance.HasBuiltCookingFire)
      return;
    MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.PlayerHunger);
  }

  public override void ShowMenu()
  {
    UIKitchenMenuController kitchenMenuInstance = MonoSingleton<UIManager>.Instance.ShowKitchenMenu(this.structure.Structure_Info);
    UIKitchenMenuController kitchenMenuController1 = kitchenMenuInstance;
    kitchenMenuController1.OnHidden = kitchenMenuController1.OnHidden + (System.Action) (() => kitchenMenuInstance = (UIKitchenMenuController) null);
    UIKitchenMenuController kitchenMenuController2 = kitchenMenuInstance;
    kitchenMenuController2.OnConfirm = kitchenMenuController2.OnConfirm + new System.Action(this.CookAll);
  }

  public void CookAll()
  {
    if (this.isLoadingAssets)
      return;
    GameManager.GetInstance().OnConversationNew();
    this.isLoadingAssets = true;
    this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadCookingMinigameAssets(), (System.Action) (() =>
    {
      this.isLoadingAssets = false;
      this.ShowCooking(true);
      this.CookingMealAnimation.AnimationState.SetAnimation(0, "start", false);
      this.CookingMealAnimation.AnimationState.AddAnimation(0, "cook", true, 0.0f);
      PlayerFarming.Instance.GoToAndStop(this.StructureInfo.Position + new Vector3(0.1f, 2.5f), this.transform.parent.gameObject, GoToCallback: (System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone);
        this._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.CookingMinigameOverlayControllerTemplate.Instantiate<UICookingMinigameOverlayController>();
        this._uiCookingMinigameOverlayController.Initialise(this.StructureInfo, (Interaction) this);
        this._uiCookingMinigameOverlayController.OnCook += new System.Action(this.OnCook);
        this._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(this.OnUnderCook);
        this._uiCookingMinigameOverlayController.OnBurn += new System.Action(this.OnBurn);
        this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
      }));
    })));
  }

  public void OnCook()
  {
    AudioManager.Instance.PlayOneShot("event:/cooking/meal_cooked", this.transform.position);
    this.MealFinishedCooking();
    if (DataManager.Instance.SurvivalModeActive)
      GameManager.GetInstance().WaitForSeconds(5f, (System.Action) (() =>
      {
        foreach (Follower follower in Follower.Followers)
        {
          if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, true))
            follower.Brain.CheckChangeTask();
        }
      }));
    else
      Interaction_Kitchen.\u003COnCook\u003Eg__UpdateFollowerTasks\u007C16_0();
    if (this.StructureInfo.QueuedMeals.Count > 0)
      return;
    this.EndCooking();
  }

  public void OnUnderCook()
  {
    AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", this.transform.position);
    this.structure.Structure_Info.QueuedMeals[0].MealType = InventoryItem.ITEM_TYPE.MEAL_BURNED;
    this.MealFinishedCooking();
    if (this.StructureInfo.QueuedMeals.Count > 0)
      return;
    this.EndCooking();
  }

  public void OnBurn()
  {
    AudioManager.Instance.PlayOneShot("event:/cooking/fire_start", this.transform.position);
    this.structure.Structure_Info.QueuedMeals[0].MealType = InventoryItem.ITEM_TYPE.MEAL_BURNED;
    this.MealFinishedCooking();
    if (this.StructureInfo.QueuedMeals.Count > 0)
      return;
    this.EndCooking();
  }

  public void EndCooking()
  {
    this.StartCoroutine((IEnumerator) this.DelayHideCooking());
    this._uiCookingMinigameOverlayController.OnCook -= new System.Action(this.OnCook);
    this._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(this.OnUnderCook);
    this._uiCookingMinigameOverlayController.OnBurn -= new System.Action(this.OnBurn);
    this._uiCookingMinigameOverlayController = (UICookingMinigameOverlayController) null;
    MonoSingleton<UIManager>.Instance.UnloadCookingMinigameAssets();
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator DelayHideCooking()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.ShowCooking(false);
  }

  public void SkinGiven() => GameManager.GetInstance().OnConversationEnd();

  public IEnumerator CreatePoopSkin()
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
    MMVibrate.RumbleContinuous(0.0f, 1f, interactionKitchen.playerFarming);
    for (int i = 0; i < UnityEngine.Random.Range(10, 20); ++i)
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.POOP, 1, interactionKitchen.SpawnMealPosition.position, (float) UnityEngine.Random.Range(9, 11), new System.Action<PickUp>(interactionKitchen.\u003CCreatePoopSkin\u003Eb__22_0));
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f));
    }
    MMVibrate.StopRumble();
    AudioManager.Instance.PlayOneShot("event:/Stings/gamble_win");
    FollowerSkinCustomTarget.Create(interactionKitchen.SpawnMealPosition.position, PlayerFarming.Instance.transform.position, 1f, "Poop", new System.Action(interactionKitchen.SkinGiven));
  }

  public virtual void MealFinishedCooking()
  {
    int num = this.structure.Structure_Info.QueuedMeals.Count > 0 ? (int) this.structure.Structure_Info.QueuedMeals[0].MealType : (int) this.structure.Structure_Info.CurrentCookingMeal.MealType;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CookFirstMeal);
    DataManager.Instance.CookedFirstFood = true;
    if (num == 69)
    {
      ++DataManager.Instance.PoopMealsCreated;
      if (DataManager.Instance.PoopMealsCreated == UnityEngine.Random.Range(5, 12) && !DataManager.GetFollowerSkinUnlocked("Poop"))
        this.StartCoroutine((IEnumerator) this.CreatePoopSkin());
      else if (DataManager.Instance.PoopMealsCreated >= 12 && !DataManager.GetFollowerSkinUnlocked("Poop"))
        this.StartCoroutine((IEnumerator) this.CreatePoopSkin());
    }
    InventoryItem.Spawn((InventoryItem.ITEM_TYPE) num, 1, this.SpawnMealPosition.position, (float) UnityEngine.Random.Range(9, 11), (System.Action<PickUp>) (pickUp =>
    {
      Meal component = pickUp.GetComponent<Meal>();
      component.CreateStructureLocation = this.StructureInfo.Location;
      component.CreateStructureOnStop = true;
    }));
    CookingData.CookedMeal((InventoryItem.ITEM_TYPE) num);
    ++DataManager.Instance.MealsCooked;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    if ((bool) (UnityEngine.Object) this.CookingMealAnimation)
    {
      this.CookingMealAnimation.AnimationState.SetAnimation(0, "start", false);
      this.CookingMealAnimation.AnimationState.AddAnimation(0, "cook", true, 0.0f);
    }
    if (this.StructureInfo.QueuedMeals.Count > 0)
      this.StructureInfo.QueuedMeals.RemoveAt(0);
    this.StructureInfo.CurrentCookingMeal = (Interaction_Kitchen.QueuedMeal) null;
    this.StructureInfo.Fuel -= 10;
  }

  [CompilerGenerated]
  public void \u003CCookAll\u003Eb__15_0()
  {
    this.isLoadingAssets = false;
    this.ShowCooking(true);
    this.CookingMealAnimation.AnimationState.SetAnimation(0, "start", false);
    this.CookingMealAnimation.AnimationState.AddAnimation(0, "cook", true, 0.0f);
    PlayerFarming.Instance.GoToAndStop(this.StructureInfo.Position + new Vector3(0.1f, 2.5f), this.transform.parent.gameObject, GoToCallback: (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone);
      this._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.CookingMinigameOverlayControllerTemplate.Instantiate<UICookingMinigameOverlayController>();
      this._uiCookingMinigameOverlayController.Initialise(this.StructureInfo, (Interaction) this);
      this._uiCookingMinigameOverlayController.OnCook += new System.Action(this.OnCook);
      this._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(this.OnUnderCook);
      this._uiCookingMinigameOverlayController.OnBurn += new System.Action(this.OnBurn);
      this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    }));
  }

  [CompilerGenerated]
  public void \u003CCookAll\u003Eb__15_1()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone);
    this._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.CookingMinigameOverlayControllerTemplate.Instantiate<UICookingMinigameOverlayController>();
    this._uiCookingMinigameOverlayController.Initialise(this.StructureInfo, (Interaction) this);
    this._uiCookingMinigameOverlayController.OnCook += new System.Action(this.OnCook);
    this._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(this.OnUnderCook);
    this._uiCookingMinigameOverlayController.OnBurn += new System.Action(this.OnBurn);
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
  }

  [CompilerGenerated]
  public static void \u003COnCook\u003Eg__UpdateFollowerTasks\u007C16_0()
  {
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID, true))
        follower.Brain.CheckChangeTask();
    }
  }

  [CompilerGenerated]
  public void \u003CCreatePoopSkin\u003Eb__22_0(PickUp pickUp)
  {
    Meal component = pickUp.GetComponent<Meal>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.CreateStructureLocation = this.StructureInfo.Location;
      component.CreateStructureOnStop = true;
    }
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop");
  }

  [CompilerGenerated]
  public void \u003CMealFinishedCooking\u003Eb__23_0(PickUp pickUp)
  {
    Meal component = pickUp.GetComponent<Meal>();
    component.CreateStructureLocation = this.StructureInfo.Location;
    component.CreateStructureOnStop = true;
  }

  [MessagePackObject(false)]
  public class QueuedMeal
  {
    [Key(0)]
    public InventoryItem.ITEM_TYPE MealType;
    [Key(1)]
    public List<InventoryItem> Ingredients = new List<InventoryItem>();
    [Key(2)]
    public float CookingDuration;
    [Key(3)]
    public float CookedTime = -1f;
  }
}
