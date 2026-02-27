// Decompiled with JetBrains decompiler
// Type: Meal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private bool Activated;
  public SpriteRenderer spriteRenderer;
  public Sprite RottenSprite;
  public FollowerLocation CreateStructureLocation = FollowerLocation.None;
  public bool CreateStructureOnStop;
  public Structure structure;
  private Structures_Meal _StructureInfo;
  public PickUp pickup;
  public static List<Meal> Meals = new List<Meal>();
  [SerializeField]
  private GameObject mealIndicator;
  [SerializeField]
  private GameObject rottenIdicator;
  private bool playedSfx;
  private SkeletonAnimation skeletonAnimation;
  public bool TakenByPlayer;
  private float Timer;

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

  private void Start()
  {
    if (!this.CreateStructureOnStop)
      this.pickup.Speed = 0.0f;
    else if (this.Burned)
      this.BurnedFlamesParticles.gameObject.SetActive(true);
    this.HasSecondaryInteraction = false;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Meal.Meals.Add(this);
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
    if (!this.SetRotten || !((UnityEngine.Object) this.Outliner != (UnityEngine.Object) null))
      return;
    this.Outliner.OutlineLayers[2].Add((UnityEngine.Object) this.OutlineTarget == (UnityEngine.Object) null ? this.gameObject : this.OutlineTarget);
  }

  public bool Rotten => this.StructureInfo != null && this.StructureInfo.Rotten;

  private void OnBrainAssigned()
  {
    if (this.SetRotten)
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
          if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain != null && !FollowerManager.FollowerLocked(follower.Brain.Info.ID) && (mealEffect.MealEffectType == CookingData.MealEffectType.RemovesIllness && follower.Brain.Info.CursedState == Thought.Ill || mealEffect.MealEffectType == CookingData.MealEffectType.RemovesDissent && follower.Brain.Info.CursedState == Thought.Dissenter))
          {
            this.SetTargetFollower(follower);
            break;
          }
        }
      }
    }
  }

  private void SetTargetFollower(Follower f)
  {
    f.Brain.SetPersonalOverrideTask(FollowerTaskType.EatMeal, this.StructureInfo.Type);
    f.Brain.CompleteCurrentTask();
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

  private void OnStructureRemoved(StructuresData structure)
  {
    if (this.StructureInfo == null || structure.ID != this.StructureInfo.ID)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void GetLabel()
  {
    if (this.StructureInfo == null)
    {
      this.Label = "";
    }
    else
    {
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
        else if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Eat) && !this.Activated && !DataManager.Instance.PlayerEaten)
        {
          this.HoldToInteract = false;
          this.Label = ScriptLocalization.Interactions.Eat;
          this.Interactable = true;
        }
        else
        {
          this.HoldToInteract = false;
          this.Label = "";
          this.Interactable = false;
        }
      }
    }
  }

  private bool MealSafeToEat()
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
      if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Eat))
        return;
      base.OnInteract(state);
      this.StartCoroutine((IEnumerator) this.EatRoutine());
      this.Activated = true;
    }
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    CameraManager.shakeCamera(0.05f, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.transform.DOKill();
    this.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
    if (this.playedSfx)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/sweep", this.transform.position);
    this.playedSfx = true;
  }

  private IEnumerator DoClean()
  {
    Meal meal = this;
    meal.playedSfx = false;
    if ((UnityEngine.Object) meal.skeletonAnimation == (UnityEngine.Object) null)
      meal.skeletonAnimation = PlayerFarming.Instance.Spine;
    meal.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(meal.HandleEvent);
    meal.Activated = true;
    meal.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    meal.state.facingAngle = Utils.GetAngle(meal.state.transform.position, meal.transform.position);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("cleaning", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
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

  private new void Update()
  {
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

  private IEnumerator EatRoutine()
  {
    Meal meal = this;
    meal.mealIndicator.SetActive(false);
    meal.rottenIdicator.SetActive(false);
    meal.TakenByPlayer = true;
    DataManager.Instance.PlayerEaten = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    meal.state.facingAngle = Utils.GetAngle(meal.state.transform.position, meal.transform.position);
    foreach (Renderer componentsInChild in meal.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.enabled = false;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    yield return (object) new WaitForSeconds(0.25f);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 4f);
    meal.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.CustomAnimation("eat", true);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/food_eat", meal.gameObject);
    yield return (object) new WaitForSeconds(1.5f);
    CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360));
    if (meal.MealSafeToEat())
    {
      AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", meal.gameObject);
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", meal.gameObject.transform.position);
      BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "red", "burst_big", (double) Time.timeScale == 1.0);
      PlayerFarming.Instance.CustomAnimation("eat-react-good", true);
      yield return (object) new WaitForSeconds(2.16666675f);
    }
    else
    {
      PlayerFarming.Instance.CustomAnimation("eat-react-bad", true);
      yield return (object) new WaitForSeconds(0.233333334f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/hold_back_vom", meal.gameObject);
      yield return (object) new WaitForSeconds(0.733333349f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/vom", meal.gameObject);
      yield return (object) new WaitForSeconds(1.0333333f);
      if (meal.StructureInfo.Type == global::StructureBrain.TYPES.MEAL_FOLLOWER_MEAT)
      {
        AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", meal.gameObject);
        AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", meal.gameObject.transform.position);
        BiomeConstants.Instance.EmitHeartPickUpVFX(PlayerFarming.Instance.CameraBone.transform.position, 0.0f, "red", "burst_big", (double) Time.timeScale == 1.0);
      }
      yield return (object) new WaitForSeconds(0.6f);
    }
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    HealthPlayer component = PlayerFarming.Instance.GetComponent<HealthPlayer>();
    if (meal.StructureInfo.Rotten || meal.StructureInfo.Burned)
    {
      AudioManager.Instance.PlayOneShot("event:/player/gethit", meal.gameObject);
      component.HP -= 2f;
    }
    else
    {
      switch (meal.StructureInfo.Type)
      {
        case global::StructureBrain.TYPES.MEAL_GRASS:
        case global::StructureBrain.TYPES.MEAL_POOP:
          AudioManager.Instance.PlayOneShot("event:/player/gethit", meal.gameObject);
          --component.HP;
          break;
        case global::StructureBrain.TYPES.MEAL_FOLLOWER_MEAT:
          component.BlackHearts += 4f;
          break;
        case global::StructureBrain.TYPES.MEAL_DEADLY:
          AudioManager.Instance.PlayOneShot("event:/player/gethit", meal.gameObject);
          component.HP -= 2f;
          break;
        default:
          component.BlueHearts += 2f;
          break;
      }
    }
    meal.structure.RemoveStructure();
  }
}
