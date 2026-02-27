// Decompiled with JetBrains decompiler
// Type: Interaction_Cook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Cook : Interaction
{
  public Interaction_Cauldron cauldron;
  public Structure structure;
  public float CookingDuration = 0.5f;
  [HideInInspector]
  public string sCook;
  [HideInInspector]
  public string sCancel;
  private Coroutine cookingRoutine;
  [HideInInspector]
  public EventInstance loopingSoundInstance;

  private void Start()
  {
    this.HoldToInteract = true;
    this.UpdateLocalisation();
    this.HasSecondaryInteraction = true;
  }

  public override void GetLabel()
  {
    this.Label = this.sCook;
    this.Interactable = true;
  }

  public override void GetSecondaryLabel()
  {
    this.SecondaryInteractable = true;
    this.SecondaryLabel = !(this.cauldron is Interaction_Kitchen) || this.structure.Structure_Info.QueuedMeals.Count >= 5 ? "" : LocalizationManager.GetTranslation("UI/Queue");
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (!(this.cauldron is Interaction_Kitchen) || this.structure.Structure_Info.QueuedMeals.Count >= 5)
      return;
    int num = this.structure.Structure_Info.Inventory.Count - 1;
    while (num >= 0)
      --num;
    this.cauldron.enabled = true;
    this.enabled = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sCook = ScriptLocalization.Interactions.Cook;
    this.sCancel = ScriptLocalization.Interactions.Cancel;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.cookingRoutine != null)
      return;
    this.cookingRoutine = this.StartCoroutine((IEnumerator) this.CookFood());
  }

  public void DoCook() => this.StartCoroutine((IEnumerator) this.CookFood());

  public override void OnDisableInteraction()
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public virtual IEnumerator CookFood()
  {
    Interaction_Cook interactionCook = this;
    InventoryItem.ITEM_TYPE MealToCreate = InventoryItem.ITEM_TYPE.NONE;
    CookingData.CookedMeal(MealToCreate);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    interactionCook.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/cooking/cooking_loop", interactionCook.gameObject, true);
    yield return (object) new WaitForEndOfFrame();
    interactionCook.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionCook.state.facingAngle = Utils.GetAngle(interactionCook.state.transform.position, interactionCook.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    if ((double) interactionCook.CookingDuration > 0.5)
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) new WaitForSeconds(interactionCook.CookingDuration);
    bool Rotten = false;
    bool flag = false;
    Structures_FoodStorage foodStorage = Structures_FoodStorage.GetAvailableFoodStorage(interactionCook.structure.Structure_Info.Position, interactionCook.structure.Structure_Info.Location);
    if (foodStorage != null)
    {
      foreach (Interaction_FoodStorage foodStorage1 in Interaction_FoodStorage.FoodStorages)
      {
        Interaction_FoodStorage s = foodStorage1;
        if ((UnityEngine.Object) s != (UnityEngine.Object) null && s.StructureInfo.ID == foodStorage.Data.ID && (UnityEngine.Object) s.gameObject != (UnityEngine.Object) null && (UnityEngine.Object) interactionCook.transform != (UnityEngine.Object) null)
        {
          flag = true;
          AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionCook.transform.position);
          ResourceCustomTarget.Create(s.gameObject, interactionCook.transform.position, MealToCreate, (System.Action) (() =>
          {
            foodStorage.DepositItemUnstacked(MealToCreate);
            s.UpdateFoodDisplayed();
            s.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f, 2).SetEase<Tweener>(Ease.InOutBack);
          }));
          break;
        }
      }
    }
    if (flag)
      InventoryItem.Spawn(MealToCreate, 1, interactionCook.transform.position, 14f, (System.Action<PickUp>) (pickUp =>
      {
        Meal component = pickUp.GetComponent<Meal>();
        component.CreateStructureLocation = this.structure.Structure_Info.Location;
        component.CreateStructureOnStop = true;
        component.SetRotten = Rotten;
      }));
    AudioManager.Instance.PlayOneShot("event:/cooking/meal_cooked", interactionCook.transform.position);
    AudioManager.Instance.StopLoop(interactionCook.loopingSoundInstance);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    interactionCook.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionCook.structure.Structure_Info.Inventory.Clear();
    interactionCook.structure.Structure_Info.Fuel -= 2;
    interactionCook.cauldron.enabled = true;
    interactionCook.enabled = false;
    ++DataManager.Instance.MealsCooked;
    ObjectiveManager.CheckObjectives(Objectives.TYPES.COOK_MEALS);
    Debug.Log((object) "BBB");
    if (!DataManager.Instance.CookedFirstFood)
      DataManager.Instance.CookedFirstFood = true;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (!FollowerManager.FollowerLocked(allBrain.Info.ID, true))
        allBrain.CheckChangeTask();
    }
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CookFirstMeal);
    interactionCook.cookingRoutine = (Coroutine) null;
  }
}
