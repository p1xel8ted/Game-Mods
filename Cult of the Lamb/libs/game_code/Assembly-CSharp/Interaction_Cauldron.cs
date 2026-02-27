// Decompiled with JetBrains decompiler
// Type: Interaction_Cauldron
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public float Delay;
  public GameObject Player;
  public bool beingMoved;
  public Collider2D Collider;
  public new EventInstance loopingSoundInstance;
  public Vector3 mealLocation;
  public PickUp _pickUp;

  public void Awake() => this.ActivateDistance = 1.5f;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.UpdateLocalisation();
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    PlacementRegion.OnBuildingBeganMoving += new PlacementRegion.BuildingEvent(this.OnBuildingBeganMoving);
    PlacementRegion.OnBuildingPlaced += new PlacementRegion.BuildingEvent(this.OnBuildingPlaced);
  }

  public void OnBuildingBeganMoving(int structureID)
  {
    int num = structureID;
    int? id = this.structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = true;
  }

  public void OnBuildingPlaced(int structureID)
  {
    int num = structureID;
    int? id = this.structure?.Structure_Info?.ID;
    int valueOrDefault = id.GetValueOrDefault();
    if (!(num == valueOrDefault & id.HasValue))
      return;
    this.beingMoved = false;
  }

  public virtual void OnBrainAssigned()
  {
    if (this.structure.Type == StructureBrain.TYPES.COOKING_FIRE)
      DataManager.Instance.HasBuiltCookingFire = true;
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.UpdatePathfinding();
  }

  public void UpdatePathfinding()
  {
    if ((UnityEngine.Object) this.Collider != (UnityEngine.Object) null && (UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
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

  public override void OnDestroy() => base.OnDestroy();

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.ShowMenu();
  }

  public virtual void ShowMenu()
  {
    UICookingFireMenuController cookingFireMenu = MonoSingleton<UIManager>.Instance.ShowCookingFireMenu(this.structure.Structure_Info);
    cookingFireMenu.OnConfirm += (System.Action) (() => this.StartCoroutine(this.Cook()));
    UICookingFireMenuController fireMenuController = cookingFireMenu;
    fireMenuController.OnHidden = fireMenuController.OnHidden + (System.Action) (() => cookingFireMenu = (UICookingFireMenuController) null);
  }

  public void SetMealPos() => this.mealLocation = this._pickUp.transform.position;

  public IEnumerator Cook()
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
      GameManager.GetInstance().OnConversationNext(interactionCauldron.playerFarming.CameraBone, 6f);
    yield return (object) new WaitForSecondsRealtime(CookingDuration);
    if (CookingData.TryDiscoverRecipe(MealToCreate))
    {
      AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", interactionCauldron.playerFarming.transform.position);
      AudioManager.Instance.PlayOneShot("event:/cooking/add_food_ingredient", interactionCauldron.playerFarming.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact, interactionCauldron.playerFarming);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionCauldron.transform.position);
      interactionCauldron.playerFarming.simpleSpineAnimator.Animate("reactions/react-happy", 0, true);
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
