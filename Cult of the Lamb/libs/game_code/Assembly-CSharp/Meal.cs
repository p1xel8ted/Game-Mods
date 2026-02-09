// Decompiled with JetBrains decompiler
// Type: Meal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Meal : Interaction
{
  public bool Burned;
  public ParticleSystem BurnedFlamesParticles;
  public bool SetRotten;
  public bool Activated;
  public SpriteRenderer spriteRenderer;
  public Sprite RottenSprite;
  public FollowerLocation CreateStructureLocation = FollowerLocation.None;
  public bool CreateStructureOnStop;
  public Structure structure;
  public Structures_Meal _StructureInfo;
  public PickUp pickup;
  public static List<Meal> Meals = new List<Meal>();
  [SerializeField]
  public GameObject mealIndicator;
  [SerializeField]
  public GameObject rottenIdicator;
  public bool isPlayerEating;
  public bool playedSfx;
  public SkeletonAnimation skeletonAnimation;
  public bool TakenByPlayer;
  public float Timer;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public Structures_Meal StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.structure.Brain as Structures_Meal;
      return this._StructureInfo;
    }
  }

  public void Start()
  {
    if (!this.CreateStructureOnStop)
      this.pickup.Speed = 0.0f;
    else if (this.Burned)
      this.BurnedFlamesParticles.gameObject.SetActive(true);
    this.HasSecondaryInteraction = false;
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    base.IndicateHighlighted(playerFarming);
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
    if (!DataManager.Instance.SurvivalModeActive)
      return;
    PlayerHungerBar.ResetWhite();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Meal.Meals.Add(this);
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
    if (this.structure.Brain != null)
      this.OnBrainAssigned();
    if (!this.SetRotten || !((UnityEngine.Object) this.Outliner != (UnityEngine.Object) null))
      return;
    this.Outliner.OutlineLayers[2].Add((UnityEngine.Object) this.OutlineTarget == (UnityEngine.Object) null ? this.gameObject : this.OutlineTarget);
  }

  public bool Rotten => this.StructureInfo != null && this.StructureInfo.Rotten;

  public void OnBrainAssigned()
  {
    if (this.SetRotten && this.StructureInfo.CanBecomeRotten)
      this.StructureInfo.Rotten = true;
    else if (this.Burned)
    {
      this.StructureInfo.Burned = true;
    }
    else
    {
      foreach (CookingData.MealEffect mealEffect in CookingData.GetMealEffects(CookingData.GetMealFromStructureType(this.StructureInfo.Type)))
      {
        foreach (Follower follower in Follower.Followers)
        {
          if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain != null && !FollowerManager.FollowerLocked(follower.Brain.Info.ID, true, excludeFreezing: true) && CookingData.GetMealFromStructureType(follower.Brain.CurrentOverrideStructureType) == InventoryItem.ITEM_TYPE.NONE && !(follower.Brain.CurrentTask is FollowerTask_AttendRitual) && !(follower.Brain.CurrentTask is FollowerTask_EatMeal) && !(follower.Brain.CurrentTask is FollowerTask_EatStoredFood) && (mealEffect.MealEffectType == CookingData.MealEffectType.RemovesIllness && follower.Brain.Info.CursedState == Thought.Ill || mealEffect.MealEffectType == CookingData.MealEffectType.OldFollowerYoung && follower.Brain.Info.CursedState == Thought.OldAge || mealEffect.MealEffectType == CookingData.MealEffectType.RemoveFreezing && follower.Brain.Info.CursedState == Thought.Freezing || mealEffect.MealEffectType == CookingData.MealEffectType.RemovesDissent && follower.Brain.Info.CursedState == Thought.Dissenter || mealEffect.MealEffectType == CookingData.MealEffectType.InstantlyDie && follower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Targeted || mealEffect.MealEffectType == CookingData.MealEffectType.CausesIllPoopy && follower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Targeted))
          {
            this.SetTargetFollower(follower);
            break;
          }
        }
      }
    }
  }

  public void SetTargetFollower(Follower f)
  {
    f.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, this.StructureInfo.Type);
    f.Brain.HardSwapToTask((FollowerTask) new FollowerTask_EatMeal(this.StructureInfo.ID));
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Meal.Meals.Remove(this);
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
    if (!this.SetRotten || !((UnityEngine.Object) this.Outliner != (UnityEngine.Object) null))
      return;
    this.Outliner.OutlineLayers[2].Remove((UnityEngine.Object) this.OutlineTarget == (UnityEngine.Object) null ? this.gameObject : this.OutlineTarget);
  }

  public void OnStructureRemoved(StructuresData structure)
  {
    if (this.StructureInfo == null || structure.ID != this.StructureInfo.ID)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void GetLabel()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!((UnityEngine.Object) player.interactor.TempInteraction != (UnityEngine.Object) this))
      {
        bool flag = !DataManager.Instance.PlayerEaten;
        if (!player.isLamb)
          flag = !DataManager.Instance.PlayerEaten_COOP;
        if (this.StructureInfo == null)
        {
          this.Label = "";
          break;
        }
        StructuresData structureInfo1 = this.StructureInfo;
        if ((structureInfo1 != null ? (structureInfo1.Burned ? 1 : 0) : 0) != 0)
        {
          this.HoldToInteract = false;
          this.Label = ScriptLocalization.Interactions.CleanBurntFood;
          this.Interactable = true;
        }
        else
        {
          StructuresData structureInfo2 = this.StructureInfo;
          if ((structureInfo2 != null ? (structureInfo2.Rotten ? 1 : 0) : 0) != 0)
          {
            this.HoldToInteract = false;
            this.Label = ScriptLocalization.Interactions.CleanRottenFood;
            this.Interactable = true;
          }
          else if ((UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Eat) & flag || DataManager.Instance.SurvivalModeActive) && !this.Activated)
          {
            this.HoldToInteract = false;
            this.Label = $"{ScriptLocalization.Interactions.Eat} <color=#FFD201>{CookingData.GetLocalizedName(CookingData.GetMealFromStructureType(this.StructureInfo.Type))}";
            this.Interactable = true;
          }
          else
          {
            this.HoldToInteract = false;
            this.Label = "";
            this.Interactable = false;
          }
        }
        if (DataManager.Instance.SurvivalModeActive && !this.isPlayerEating)
          PlayerHungerBar.SetWhite(Mathf.Clamp(DataManager.Instance.SurvivalMode_Hunger + (float) CookingData.GetSatationAmountPlayer(CookingData.GetMealFromStructureType(this.StructureInfo.Type)), 0.0f, 100f) / 100f);
      }
    }
  }

  public bool MealSafeToEat()
  {
    if (this.StructureInfo != null)
    {
      switch (this.StructureInfo.Type)
      {
        case global::StructureBrain.TYPES.MEAL:
        case global::StructureBrain.TYPES.MEAL_MEAT:
        case global::StructureBrain.TYPES.MEAL_GREAT:
        case global::StructureBrain.TYPES.MEAL_GOOD_FISH:
        case global::StructureBrain.TYPES.MEAL_GREAT_FISH:
        case global::StructureBrain.TYPES.MEAL_BAD_FISH:
        case global::StructureBrain.TYPES.MEAL_BERRIES:
        case global::StructureBrain.TYPES.MEAL_MEDIUM_VEG:
        case global::StructureBrain.TYPES.MEAL_BAD_MIXED:
        case global::StructureBrain.TYPES.MEAL_MEDIUM_MIXED:
        case global::StructureBrain.TYPES.MEAL_GREAT_MIXED:
        case global::StructureBrain.TYPES.MEAL_BAD_MEAT:
        case global::StructureBrain.TYPES.MEAL_GREAT_MEAT:
        case global::StructureBrain.TYPES.MEAL_EGG:
        case global::StructureBrain.TYPES.MEAL_SPICY:
        case global::StructureBrain.TYPES.MEAL_SNOW_FRUIT:
        case global::StructureBrain.TYPES.MEAL_MILK_BAD:
        case global::StructureBrain.TYPES.MEAL_MILK_GOOD:
        case global::StructureBrain.TYPES.MEAL_MILK_GREAT:
          return true;
      }
    }
    return false;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    if (this.StructureInfo.Rotten || this.StructureInfo.Burned)
    {
      base.OnInteract(state);
      this.StartCoroutine((IEnumerator) this.DoClean());
      this.Activated = true;
    }
    else
    {
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Eat) && !DataManager.Instance.SurvivalModeActive)
        return;
      base.OnInteract(state);
      this.StartCoroutine((IEnumerator) this.EatRoutine(state.GetComponent<PlayerFarming>()));
      this.Activated = true;
    }
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    CameraManager.shakeCamera(0.05f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.transform.DOKill();
    this.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
    if (this.playedSfx)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/sweep", this.transform.position);
    this.playedSfx = true;
  }

  public IEnumerator DoClean()
  {
    Meal meal = this;
    meal.playedSfx = false;
    meal.skeletonAnimation = meal.playerFarming.Spine;
    meal.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(meal.HandleEvent);
    meal.Activated = true;
    meal.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    meal.state.facingAngle = Utils.GetAngle(meal.state.transform.position, meal.transform.position);
    yield return (object) new WaitForEndOfFrame();
    meal.playerFarming.simpleSpineAnimator.Animate("cleaning", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    meal._playerFarming.playerChoreXPBarController.AddChoreXP(meal.playerFarming);
    float Progress = 0.0f;
    while (InputManager.Gameplay.GetInteractButtonHeld() && (double) (Progress += Time.deltaTime) < 2.0 * ((double) meal.StructureInfo.GrowthStage / 5.0))
    {
      meal.StructureInfo.StartingScale = (float) (1.0 - (double) Progress / (2.0 * ((double) meal.StructureInfo.GrowthStage / 5.0)));
      yield return (object) null;
    }
    if ((double) Progress >= 2.0 * ((double) meal.StructureInfo.GrowthStage / 5.0))
    {
      AudioManager.Instance.PlayOneShot("event:/player/weed_pick", meal.transform.position);
      meal.transform.DOScale(Vector3.zero, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(((global::StructureBrain) meal.StructureBrain).Remove));
    }
    meal.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(meal.HandleEvent);
    meal.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public override void Update()
  {
    base.Update();
    if (this.StructureInfo != null && this.StructureInfo.Rotten)
      this.spriteRenderer.sprite = this.RottenSprite;
    if (this.TakenByPlayer)
      return;
    if (this.StructureInfo != null && this.StructureInfo.Eaten)
    {
      this.structure.RemoveStructure();
    }
    else
    {
      if ((double) (this.Timer += Time.deltaTime) < 1.0)
        return;
      if (this.CreateStructureOnStop && (UnityEngine.Object) this.structure != (UnityEngine.Object) null)
      {
        this.structure.CreateStructure(this.CreateStructureLocation, this.transform.position);
        this.CreateStructureOnStop = false;
      }
      this.rottenIdicator.SetActive(this.StructureInfo != null && this.StructureInfo.Rotten);
      if (this.StructureInfo != null && this.StructureInfo.Burned)
        this.mealIndicator.SetActive(true);
      else
        this.mealIndicator.SetActive(this.StructureInfo == null || !this.StructureInfo.Rotten);
    }
  }

  public IEnumerator EatRoutine(PlayerFarming playerFarming)
  {
    Meal meal = this;
    meal.isPlayerEating = true;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTask is FollowerTask_EatMeal currentTask && currentTask.TargetMeal == meal.StructureInfo.ID)
      {
        currentTask._mealEatenByPlayer = true;
        allBrain.CompleteCurrentTask();
      }
    }
    if (DataManager.Instance.SurvivalModeActive && (double) Mathf.Clamp(DataManager.Instance.SurvivalMode_Hunger + (float) CookingData.GetSatationAmountPlayer(CookingData.GetMealFromStructureType(meal.StructureInfo.Type)), 0.0f, 100f) > 0.0)
      TimeManager.SurvivalDamagedTimer = (double) DataManager.Instance.SurvivalMode_Sleep > 0.0 ? -1f : TimeManager.SurvivalDamagedTimer;
    meal.mealIndicator.SetActive(false);
    meal.rottenIdicator.SetActive(false);
    meal.TakenByPlayer = true;
    meal.StructureBrain.ReservedByPlayer = true;
    if (playerFarming.isLamb)
      DataManager.Instance.PlayerEaten = true;
    else
      DataManager.Instance.PlayerEaten_COOP = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(playerFarming.gameObject, 5f);
    meal.state.facingAngle = Utils.GetAngle(meal.state.transform.position, meal.transform.position);
    foreach (Renderer componentsInChild in meal.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.enabled = false;
    GameManager.GetInstance().OnConversationNext(playerFarming.gameObject, 4f);
    meal.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.CustomAnimation("eat-meal", true);
    AudioManager.Instance.PlayOneShot("event:/player/munch", meal.gameObject);
    yield return (object) new WaitForSeconds(1.75f);
    CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360));
    if (meal.MealSafeToEat())
    {
      if (!DataManager.Instance.SurvivalModeActive)
      {
        AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", meal.gameObject);
        AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", meal.gameObject.transform.position);
        if (!DataManager.Instance.SurvivalModeActive)
          BiomeConstants.Instance.EmitHeartPickUpVFX(playerFarming.CameraBone.transform.position, 0.0f, "red", "burst_big", (double) Time.timeScale == 1.0);
        playerFarming.CustomAnimation("eat-react-good", true);
        yield return (object) new WaitForSeconds(2.16666675f);
      }
    }
    else
    {
      playerFarming.CustomAnimation("eat-react-bad", true);
      yield return (object) new WaitForSeconds(0.233333334f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/hold_back_vom", meal.gameObject);
      yield return (object) new WaitForSeconds(0.733333349f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/vom", meal.gameObject);
      yield return (object) new WaitForSeconds(1.0333333f);
      if (meal.StructureInfo.Type == global::StructureBrain.TYPES.MEAL_FOLLOWER_MEAT)
      {
        AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", meal.gameObject);
        AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", meal.gameObject.transform.position);
        if (!DataManager.Instance.SurvivalModeActive)
          BiomeConstants.Instance.EmitHeartPickUpVFX(playerFarming.CameraBone.transform.position, 0.0f, "red", "burst_big", (double) Time.timeScale == 1.0);
      }
      yield return (object) new WaitForSeconds(0.6f);
    }
    if (DataManager.Instance.SurvivalModeActive)
      DataManager.Instance.SurvivalMode_Hunger = Mathf.Clamp(DataManager.Instance.SurvivalMode_Hunger + (float) CookingData.GetSatationAmountPlayer(CookingData.GetMealFromStructureType(meal.StructureInfo.Type)), 0.0f, 100f);
    GameManager.GetInstance().OnConversationEnd();
    if (!playerFarming.isLamb && !DataManager.Instance.SurvivalModeActive)
    {
      yield return (object) new WaitForSeconds(1f);
      HUD_Manager.Instance.healthManager.ShowSecondPlayerHealth();
    }
    yield return (object) new WaitForSeconds(1f);
    HealthPlayer component = playerFarming.GetComponent<HealthPlayer>();
    if (meal.StructureInfo.Rotten || meal.StructureInfo.Burned)
    {
      AudioManager.Instance.PlayOneShot("event:/player/gethit", meal.gameObject);
      component.HP -= 2f;
    }
    else if (!DataManager.Instance.SurvivalModeActive)
    {
      switch (meal.StructureInfo.Type)
      {
        case global::StructureBrain.TYPES.MEAL_GRASS:
        case global::StructureBrain.TYPES.MEAL_POOP:
          AudioManager.Instance.PlayOneShot("event:/player/gethit", meal.gameObject);
          --component.HP;
          if (playerFarming.isLamb)
          {
            ++DataManager.Instance.PLAYER_REMOVED_HEARTS;
            break;
          }
          ++DataManager.Instance.COOP_PLAYER_REMOVED_HEARTS;
          break;
        case global::StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
          component.BlackHearts += 4f;
          if (playerFarming.isLamb)
          {
            DataManager.Instance.PLAYER_BLACK_HEARTS += 4f;
            break;
          }
          DataManager.Instance.COOP_PLAYER_BLACK_HEARTS += 4f;
          break;
        case global::StructureBrain.TYPES.MEAL_DEADLY:
          AudioManager.Instance.PlayOneShot("event:/player/gethit", meal.gameObject);
          component.HP -= 2f;
          if (playerFarming.isLamb)
          {
            DataManager.Instance.PLAYER_REMOVED_HEARTS += 2f;
            break;
          }
          DataManager.Instance.COOP_PLAYER_REMOVED_HEARTS += 2f;
          break;
        case global::StructureBrain.TYPES.MEAL_SPICY:
          component.FireHearts += 2f;
          if (playerFarming.isLamb)
          {
            DataManager.Instance.PLAYER_FIRE_HEARTS += 2f;
            break;
          }
          DataManager.Instance.COOP_PLAYER_FIRE_HEARTS += 2f;
          break;
        default:
          component.BlueHearts += 2f;
          if (playerFarming.isLamb)
          {
            DataManager.Instance.PLAYER_BLUE_HEARTS += 2f;
            break;
          }
          DataManager.Instance.COOP_PLAYER_BLUE_HEARTS += 2f;
          break;
      }
    }
    meal.structure.RemoveStructure();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.EatMeal);
  }
}
