// Decompiled with JetBrains decompiler
// Type: Interaction_Cauldron
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.KitchenMenu;
using src.Extensions;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Cauldron : Interaction
{
  public Structure structure;
  private float Delay;
  private GameObject Player;
  private bool beingMoved;
  public Collider2D Collider;
  private EventInstance loopingSoundInstance;
  private Vector3 mealLocation;
  private PickUp _pickUp;

  private void Awake() => this.ActivateDistance = 1.5f;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.UpdateLocalisation();
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    PlacementRegion.OnBuildingBeganMoving += new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced += new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
  }

  private void OnBuildingBeganMoving(int structureID)
  {
    int num = structureID;
    int? id = this.structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = true;
  }

  private void OnBuildingPlaced(int structureID)
  {
    int num = structureID;
    int? id = this.structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = false;
  }

  private void OnBrainAssigned()
  {
    if (this.structure.Type == StructureBrain.TYPES.COOKING_FIRE)
      DataManager.Instance.HasBuiltCookingFire = true;
    this.UpdatePathfinding();
  }

  private void UpdatePathfinding()
  {
    if ((UnityEngine.Object) this.Collider != (UnityEngine.Object) null)
      AstarPath.active.UpdateGraphs(this.Collider.bounds);
    else
      Debug.Log((object) "DIDNT WORK");
  }

  public event Interaction_Cauldron.ItemEvent OnFoodModified;

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.Cook;

  public override void OnDisableInteraction()
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    PlacementRegion.OnBuildingBeganMoving -= new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced -= new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
  }

  protected override void OnDestroy() => base.OnDestroy();

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.ShowMenu();
  }

  protected virtual void ShowMenu()
  {
    UICookingFireMenuController cookingFireMenu = MonoSingleton<UIManager>.Instance.ShowCookingFireMenu(this.structure.Structure_Info);
    cookingFireMenu.OnConfirm += (System.Action) (() => this.StartCoroutine((IEnumerator) this.Cook()));
    UICookingFireMenuController fireMenuController = cookingFireMenu;
    fireMenuController.OnHidden = fireMenuController.OnHidden + (System.Action) (() => cookingFireMenu = (UICookingFireMenuController) null);
  }

  private void SetMealPos() => this.mealLocation = this._pickUp.transform.position;

  protected IEnumerator Cook()
  {
    Interaction_Cauldron interactionCauldron = this;
    interactionCauldron.Interactable = false;
    float CookingDuration = 1.5f;
    InventoryItem.ITEM_TYPE MealToCreate = InventoryItem.ITEM_TYPE.NONE;
    CookingData.CookedMeal(MealToCreate);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    interactionCauldron.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/cooking/cooking_loop", interactionCauldron.gameObject, true);
    yield return (object) new WaitForEndOfFrame();
    interactionCauldron.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionCauldron.state.facingAngle = Utils.GetAngle(interactionCauldron.state.transform.position, interactionCauldron.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, CookingDuration);
    if ((double) CookingDuration > 0.5)
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) new WaitForSecondsRealtime(CookingDuration);
    if (CookingData.TryDiscoverRecipe(MealToCreate))
    {
      AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.transform.position);
      AudioManager.Instance.PlayOneShot("event:/cooking/add_food_ingredient", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionCauldron.transform.position);
      PlayerFarming.Instance.simpleSpineAnimator.Animate("reactions/react-happy", 0, true);
      yield return (object) new WaitForSecondsRealtime(1.5f);
      UIRecipeConfirmationOverlayController recipeConfirmationInstance = MonoSingleton<UIManager>.Instance.RecipeConfirmationTemplate.Instantiate<UIRecipeConfirmationOverlayController>();
      recipeConfirmationInstance.Show(MealToCreate, true);
      UIRecipeConfirmationOverlayController overlayController = recipeConfirmationInstance;
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => recipeConfirmationInstance = (UIRecipeConfirmationOverlayController) null);
      MonoSingleton<UIManager>.Instance.SetMenuInstance((UIMenuBase) recipeConfirmationInstance);
      while ((UnityEngine.Object) recipeConfirmationInstance != (UnityEngine.Object) null)
        yield return (object) null;
    }
    interactionCauldron.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    bool flag = false;
    Structures_FoodStorage foodStorage = Structures_FoodStorage.GetAvailableFoodStorage(interactionCauldron.structure.Structure_Info.Position, interactionCauldron.structure.Structure_Info.Location);
    if (foodStorage != null)
    {
      foreach (Interaction_FoodStorage foodStorage1 in Interaction_FoodStorage.FoodStorages)
      {
        Interaction_FoodStorage s = foodStorage1;
        if ((UnityEngine.Object) s != (UnityEngine.Object) null && s.StructureInfo.ID == foodStorage.Data.ID && (UnityEngine.Object) s.gameObject != (UnityEngine.Object) null && (UnityEngine.Object) interactionCauldron.transform != (UnityEngine.Object) null)
        {
          flag = true;
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionCauldron.mealLocation);
          ResourceCustomTarget.Create(s.gameObject, interactionCauldron.mealLocation, MealToCreate, (System.Action) (() =>
          {
            foodStorage.DepositItemUnstacked(MealToCreate);
            s.UpdateFoodDisplayed();
            s.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f, 2).SetEase<Tweener>(Ease.InOutBack);
          }));
          break;
        }
      }
    }
    if (!flag)
      InventoryItem.Spawn(MealToCreate, 1, interactionCauldron.transform.position, 14f, (System.Action<PickUp>) (pickUp =>
      {
        Meal component = pickUp.GetComponent<Meal>();
        component.CreateStructureLocation = this.structure.Structure_Info.Location;
        component.CreateStructureOnStop = true;
        component.SetRotten = false;
      }));
    interactionCauldron.structure.Structure_Info.Inventory.Clear();
    AudioManager.Instance.PlayOneShot("event:/cooking/meal_cooked", interactionCauldron.transform.position);
    AudioManager.Instance.StopLoop(interactionCauldron.loopingSoundInstance);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    interactionCauldron.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionCauldron.structure.Brain.UpdateFuel(10);
    ++DataManager.Instance.MealsCooked;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    if (!DataManager.Instance.CookedFirstFood)
    {
      DataManager.Instance.CookedFirstFood = true;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        allBrain.CompleteCurrentTask();
        allBrain.SetPersonalOverrideTask(FollowerTaskType.EatMeal);
      }
    }
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID, true))
        allBrain.CheckChangeTask();
    }
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CookFirstMeal);
    interactionCauldron.HasChanged = true;
    interactionCauldron.Interactable = true;
  }

  public delegate void ItemEvent(float normAmount);
}
