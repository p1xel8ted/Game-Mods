// Decompiled with JetBrains decompiler
// Type: Interaction_Ranchable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerInteractionWheel;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

#nullable disable
public class Interaction_Ranchable : Interaction
{
  public static List<InventoryItem.ITEM_TYPE> CommonFavouriteFood = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.BERRY,
    InventoryItem.ITEM_TYPE.PUMPKIN,
    InventoryItem.ITEM_TYPE.BEETROOT,
    InventoryItem.ITEM_TYPE.CAULIFLOWER
  };
  public static List<InventoryItem.ITEM_TYPE> UncommonFavouriteFood = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.GRASS
  };
  public static Vector3[] positionCheckMoveDirections = new Vector3[8]
  {
    Vector3.left,
    Vector3.right,
    Vector3.up,
    Vector3.down,
    new Vector3(1f, 1f),
    new Vector3(-1f, 1f),
    new Vector3(1f, -1f),
    new Vector3(-1f, -1f)
  };
  public const float DELIVER_ANIMAL_INTERACTION_ACTIVE_DISTANCE = 2.5f;
  public RanchableFeralWarning RanchableFeralWarning;
  public RanchableInjuredWarning RanchableInjuredWarning;
  public static List<Interaction_Ranchable> Ranchables = new List<Interaction_Ranchable>();
  public static List<Interaction_Ranchable> DeadRanchables = new List<Interaction_Ranchable>();
  public System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch> OnEnterHutch;
  public System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch> OnEnteredHutch;
  [SerializeField]
  public SimpleRadialProgressBar interactionProgressBar;
  [SerializeField]
  public SimpleRadialProgressBar racingProgressBar;
  public const int PHASES_PER_GROWTH = 5;
  public const int MAX_GROWTH = 6;
  public const int MAX_HUNGER = 100;
  public const int BABY_GROWTH = 2;
  public const int OLD_AGE = 15;
  public const float MAX_DEFAULT_MOVE_DISTANCE = 10f;
  public const int MEAT_DROP_MULTIPLIER = 3;
  public const float STARVING_THRESHOLD = 25f;
  public const float BREEDING_CHANCE_WITH_HUTCH = 0.2f;
  public const float ESCAPED_SPEED_MULTIPLIER = 2.5f;
  public const float FOLLOW_PLAYER_SPEED_MULTIPLIER = 4f;
  public const float ATTACK_SPEED_MULTIPLIER = 4f;
  public const float TIME_BETWEEN_POOPS = 1920f;
  public const float TIME_BETWEEN_WASHES = 4800f;
  public static Vector2 TIME_BETWEEN_WOLVES = new Vector2(1200f, 3120f);
  public const float MAKE_HAPPY_INTERACTION_TIME = 10f;
  public float makeHappyTimer;
  public float raceTimer;
  public Vector2 MAX_SPEED = new Vector2(-0.5f, 1.5f);
  [SerializeField]
  public InventoryItem.ITEM_TYPE ranchableType;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public LineRenderer leash;
  [SerializeField]
  public GameObject leashPosition;
  [SerializeField]
  public GameObject inverter;
  [SerializeField]
  public GameObject emptyOutline;
  [SerializeField]
  public WorshipperBubble bubble;
  [SerializeField]
  public GameObject stinky;
  [SerializeField]
  public GameObject eatenTodayIcon;
  [SerializeField]
  public GameObject deadBody;
  [SerializeField]
  public AnimationCurve slackCurve;
  [SerializeField]
  public AnimationCurve rigidCurve;
  [SerializeField]
  public GameObject animalNameParent;
  [SerializeField]
  public TMP_Text animalNameText;
  [SerializeField]
  public AnimationAdorationUI adorationUI;
  [SerializeField]
  public WorshipperData.SlotsAndColours[] colours;
  [CompilerGenerated]
  public PlacementRegion.TileGridTile \u003CCurrentTile\u003Ek__BackingField;
  public PlacementRegion.TileGridTile currentTile;
  public List<PlacementRegion.TileGridTile> topPriorityTiles = new List<PlacementRegion.TileGridTile>();
  public List<PlacementRegion.TileGridTile> lowPriorityTiles = new List<PlacementRegion.TileGridTile>();
  public List<PlacementRegion.TileGridTile> excludedTiles = new List<PlacementRegion.TileGridTile>();
  public List<PlacementRegion.TileGridTile> reservedTiles = new List<PlacementRegion.TileGridTile>();
  public StructuresData.Ranchable_Animal animal;
  public UIFollowerInteractionWheelOverlayController animalInteractionWheel;
  public Interaction_Ranch ranch;
  public UnitObject unitObject;
  public SpringJoint2D springJoint;
  public Rigidbody2D rigidbody;
  [CompilerGenerated]
  public int \u003CHits\u003Ek__BackingField;
  public Structures_RanchTrough targetTrough;
  public Structures_RanchHutch targetHutch;
  public Interaction_Ranchable targetAnimal;
  public LayerMask collisionMask;
  public EventInstance eatingLoopSFX;
  public EventInstance feralLoopSFX;
  public EventInstance ridingLoopSFX;
  public string ridingSpeedParameter = "animalMovementSpeed";
  public float moveTimer = 10f;
  public bool forceBlockMovement;
  public float originalMaxSpeed;
  public PlayerFarming followedPlayer;
  public Coroutine bubbleRoutine;
  public Coroutine postFeedRoutine;
  public Interaction_WolfBase enemyWolf;
  public float leashTimestamp = -1f;
  public bool destroyed;
  public bool isBeingFed;
  public int currentHutchID = -1;
  [CompilerGenerated]
  public bool \u003CReservedByPlayer\u003Ek__BackingField;
  public static List<InventoryItem.ITEM_TYPE> shearables = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.ANIMAL_GOAT,
    InventoryItem.ITEM_TYPE.ANIMAL_LLAMA,
    InventoryItem.ITEM_TYPE.ANIMAL_TURTLE,
    InventoryItem.ITEM_TYPE.ANIMAL_COW
  };
  public static List<InventoryItem.ITEM_TYPE> milkables = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.ANIMAL_COW
  };
  public InventoryItem.ITEM_TYPE FlowerType;
  public bool overrideDieChance;
  public bool BeingAscended;
  public float LastHealedTime = float.NegativeInfinity;
  public float LastAttackTime = float.NegativeInfinity;
  public float CacheSpeed;

  public float raceTime => (float) Interaction_RacingGate.RacingGates.Count * 1.25f;

  public InventoryItem.ITEM_TYPE RanchableType => this.ranchableType;

  public SkeletonAnimation Spine => this.spine;

  public PlacementRegion.TileGridTile CurrentTile
  {
    get => this.\u003CCurrentTile\u003Ek__BackingField;
    set => this.\u003CCurrentTile\u003Ek__BackingField = value;
  }

  public StructuresData.Ranchable_Animal Animal => this.animal;

  public UnitObject UnitObject => this.unitObject;

  public Health Health => this.unitObject.health;

  public int Hits
  {
    get => this.\u003CHits\u003Ek__BackingField;
    set => this.\u003CHits\u003Ek__BackingField = value;
  }

  public float Adoration => this.animal == null ? 0.0f : this.animal.Adoration;

  public int Level => this.animal == null ? 1 : this.animal.Level;

  public Interaction_Ranchable.State CurrentState
  {
    get => this.animal.State;
    set
    {
      if (this.animal.State == Interaction_Ranchable.State.Dead && value != Interaction_Ranchable.State.Dead)
        return;
      this.animal.State = value;
      if (this.animal.State == Interaction_Ranchable.State.Leashed && this.animal.State == Interaction_Ranchable.State.Riding || !((UnityEngine.Object) LambTownController.Instance != (UnityEngine.Object) null) || PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
        return;
      LambTownController.Instance.IcegoreDropOffItem.SetOnState(false);
      Interaction_DLCFurnace.Instance.dropOffAnimalFurnace.SetOnState(false);
    }
  }

  public bool isStarving => (double) this.animal.Satiation <= 25.0;

  public bool ReservedByPlayer
  {
    get => this.\u003CReservedByPlayer\u003Ek__BackingField;
    set => this.\u003CReservedByPlayer\u003Ek__BackingField = value;
  }

  public bool FeelsOvercrowded => (UnityEngine.Object) this.ranch != (UnityEngine.Object) null && this.ranch.IsOvercrowded;

  public string idle_anim
  {
    get
    {
      if (this.animal.Ailment == Interaction_Ranchable.Ailment.Feral)
        return "idle-feral";
      if (this.animal.Ailment == Interaction_Ranchable.Ailment.Injured)
      {
        switch (this.animal.Type)
        {
          case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
          case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
            return "idle-injured-turtle-snail";
          case InventoryItem.ITEM_TYPE.ANIMAL_COW:
            return "idle-injured-cow";
          default:
            return "idle-injured";
        }
      }
      else
      {
        if (this.FeelsOvercrowded)
          return "idle_overcrowded";
        if (this.isStarving)
          return "idle_starving";
        if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
          return "idle_blizzard";
        return this.animal.WorkedReady ? "idle_ready" : "idle";
      }
    }
  }

  public string walk_anim
  {
    get
    {
      if (this.animal.Ailment == Interaction_Ranchable.Ailment.Feral)
        return "walk-feral";
      if (this.animal.Ailment == Interaction_Ranchable.Ailment.Injured)
      {
        switch (this.animal.Type)
        {
          case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
          case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
            return "walk-injured-turtle-snail";
          default:
            return "walk-injured";
        }
      }
      else
        return SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard ? "walk2" : "walk";
    }
  }

  public string animalNameLevelLabel
  {
    get
    {
      if (this.animal == null)
        return string.Empty;
      string format = this.animal.Age < 15 ? (this.animal.Age >= 2 ? "" : LocalizationManager.GetTranslation("UI/RanchAssignMenu/Baby")) : LocalizationManager.GetTranslation("UI/RanchAssignMenu/Old");
      string str;
      if (!string.IsNullOrEmpty(format))
      {
        if (!string.IsNullOrEmpty(this.animal.GivenName))
          str = string.Format(format, (object) $"{this.animal.GivenName.Colour(StaticColors.YellowColorHex)} - {ScriptLocalization.Interactions.Level} {this.animal.Level.ToNumeral()}");
        else
          str = string.Format(format, (object) $"{InventoryItem.LocalizedName(this.animal.Type).Colour(StaticColors.YellowColorHex)} - {ScriptLocalization.Interactions.Level} {this.animal.Level.ToNumeral()}");
      }
      else if (!string.IsNullOrEmpty(this.animal.GivenName))
        str = $"{this.animal.GivenName.Colour(StaticColors.YellowColorHex)} - {ScriptLocalization.Interactions.Level} {this.animal.Level.ToNumeral()}";
      else
        str = $"{InventoryItem.LocalizedName(this.animal.Type).Colour(StaticColors.YellowColorHex)} - {ScriptLocalization.Interactions.Level} {this.animal.Level.ToNumeral()}";
      return this.animal.BestFriend ? "\uF004 " + str : str;
    }
  }

  public static int getResourceGrowthRateDays(StructuresData.Ranchable_Animal animal)
  {
    switch (Interaction_Ranchable.getResourceGrowthRate(animal))
    {
      case Interaction_Ranchable.GrowthRate.Slow:
        return 3;
      case Interaction_Ranchable.GrowthRate.Normal:
        return 2;
      case Interaction_Ranchable.GrowthRate.Fast:
        return 2;
      default:
        return 2;
    }
  }

  public static Interaction_Ranchable.GrowthRate getResourceGrowthRate(
    StructuresData.Ranchable_Animal animal)
  {
    switch (animal.Type)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        return Interaction_Ranchable.GrowthRate.Fast;
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        return Interaction_Ranchable.GrowthRate.Slow;
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        return Interaction_Ranchable.GrowthRate.Slow;
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        return Interaction_Ranchable.GrowthRate.Slow;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        return Interaction_Ranchable.GrowthRate.Slow;
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        return Interaction_Ranchable.GrowthRate.Slow;
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        return Interaction_Ranchable.GrowthRate.Normal;
      default:
        return Interaction_Ranchable.GrowthRate.Normal;
    }
  }

  public void IncreaseLevelDebug() => ++this.animal.Level;

  public void Awake()
  {
    this.rigidbody = this.GetComponent<Rigidbody2D>();
    this.unitObject = this.GetComponent<UnitObject>();
    this.spine.gameObject.SetActive(false);
    this.spine.zSpacing = UnityEngine.Random.Range(-1f / 500f, -3f / 1000f);
    CoopManager.Instance.OnPlayerJoined += new System.Action(this.OnPlayerJoined);
    CoopManager.Instance.OnPlayerLeft += new System.Action(this.OnPlayerLeft);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureModified);
    this.UnitObject.NewPath += new UnitObject.Action(this.UnitObject_NewPath);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.UnitObject.NewPath -= new UnitObject.Action(this.UnitObject_NewPath);
    Interaction_Ranchable.Ranchables.Remove(this);
    Interaction_Ranchable.DeadRanchables.Remove(this);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    Interaction_Ranch.OnRanchUpdated -= new Interaction_Ranch.RanchEvent(this.CheckBreakingOut);
    CoopManager.Instance.OnPlayerJoined -= new System.Action(this.OnPlayerJoined);
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.OnPlayerLeft);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureModified);
    AudioManager.Instance.StopLoop(this.eatingLoopSFX);
    AudioManager.Instance.StopLoop(this.feralLoopSFX);
    AudioManager.Instance.StopLoop(this.ridingLoopSFX);
    if (this.animal.State == Interaction_Ranchable.State.EnteringHutch)
    {
      System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch> onEnterHutch = this.OnEnterHutch;
      if (onEnterHutch != null)
        onEnterHutch(this.animal, this.targetHutch);
      System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch> onEnteredHutch = this.OnEnteredHutch;
      if (onEnteredHutch != null)
        onEnteredHutch(this.animal, this.targetHutch);
    }
    else if (this.animal.State == Interaction_Ranchable.State.Animating)
      this.CurrentState = Interaction_Ranchable.State.Default;
    if ((bool) (UnityEngine.Object) this.unitObject)
      this.unitObject.EndOfPath -= new System.Action(this.OnEndPath);
    this.destroyed = true;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.isBeingFed = false;
    Interaction_RacingGate.OnStartRace += new System.Action(this.OnStartRacing);
    Interaction_RacingGate.OnFinishRace += new System.Action<float>(this.OnFinishRacing);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (this.CurrentState == Interaction_Ranchable.State.Leashed)
      this.DetatchLeash();
    else if (this.CurrentState == Interaction_Ranchable.State.Riding)
      this.EndRiding();
    Interaction_RacingGate.OnStartRace -= new System.Action(this.OnStartRacing);
    Interaction_RacingGate.OnFinishRace -= new System.Action<float>(this.OnFinishRacing);
    this.interactionProgressBar.Hide(true);
    this.racingProgressBar.Hide(true);
    AudioManager.Instance.StopLoop(this.eatingLoopSFX);
    AudioManager.Instance.StopLoop(this.feralLoopSFX);
    AudioManager.Instance.StopLoop(this.ridingLoopSFX);
    this.postFeedRoutine = (Coroutine) null;
    if (PlayerFarming.Location == FollowerLocation.Base || !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      if (this.destroyed || this.CurrentState != Interaction_Ranchable.State.BreakingOut)
        return;
      this.transform.parent = BaseLocationManager.Instance.UnitLayer;
      this.transform.position = Vector3.zero;
    }));
  }

  public void Configure(StructuresData.Ranchable_Animal animal, Interaction_Ranch ranch)
  {
    this.spine.gameObject.SetActive(true);
    this.unitObject.SpeedMultiplier += animal.Speed;
    this.originalMaxSpeed = this.unitObject.maxSpeed;
    if (animal.State == Interaction_Ranchable.State.Dead)
      Interaction_Ranchable.DeadRanchables.Add(this);
    else
      Interaction_Ranchable.Ranchables.Add(this);
    this.CurrentTile = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.transform.position);
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.animal = animal;
    this.ranch = ranch;
    this.UpdateGrowthState();
    this.unitObject.EndOfPath += new System.Action(this.OnEndPath);
    Interaction_Ranch.OnRanchUpdated += new Interaction_Ranch.RanchEvent(this.CheckBreakingOut);
    if (this.CurrentState == Interaction_Ranchable.State.BabyInHutch)
      this.spine.AnimationState.SetAnimation(0, "sleeping", true);
    else if (this.CurrentState == Interaction_Ranchable.State.EnteringHutch)
    {
      this.CurrentState = Interaction_Ranchable.State.InsideHutch;
    }
    else
    {
      this.spine.AnimationState.SetAnimation(0, "appear", false);
      this.spine.AnimationState.AddAnimation(0, this.idle_anim, true, 0.0f);
      this.moveTimer = UnityEngine.Random.Range(10f, 15f);
    }
    if (this.CurrentState == Interaction_Ranchable.State.InsideHutch)
    {
      Structures_RanchHutch hutch = this.GetHutch();
      if (hutch != null)
        this.transform.position = hutch.Data.Position;
      this.spine.gameObject.SetActive(false);
    }
    else if (this.CurrentState == Interaction_Ranchable.State.Animating)
      this.CurrentState = Interaction_Ranchable.State.Default;
    else if (this.CurrentState == Interaction_Ranchable.State.Dead)
    {
      this.UpdateSkin();
      this.SetDead();
    }
    else if ((UnityEngine.Object) ranch != (UnityEngine.Object) null && (this.CurrentState == Interaction_Ranchable.State.BreakingOut || !ranch.IsValid))
      this.CheckBreakingOut();
    else if (this.CurrentState == Interaction_Ranchable.State.Overcrowded)
      this.spine.AnimationState.SetAnimation(0, "idle_starving", true);
    else if (this.CurrentState == Interaction_Ranchable.State.Leashed || this.CurrentState == Interaction_Ranchable.State.Riding || this.CurrentState == Interaction_Ranchable.State.FollowPlayer)
      this.ResetAnimalState();
    else if (this.CurrentState == Interaction_Ranchable.State.Default && animal.IsPlayersAnimal())
      this.StartFollowingPlayer();
    this.animalNameParent.gameObject.SetActive(!string.IsNullOrEmpty(animal.GivenName));
    this.animalNameText.text = animal.GivenName;
    this.interactionProgressBar.Hide(true);
    this.racingProgressBar.Hide(true);
    if (animal.Ailment == Interaction_Ranchable.Ailment.Feral)
    {
      AudioManager.Instance.StopLoop(this.feralLoopSFX);
      if (animal != null && animal.Ailment == Interaction_Ranchable.Ailment.Feral)
        this.feralLoopSFX = AudioManager.Instance.CreateLoop("event:/dlc/animal/shared/feral_loop", this.gameObject, true);
    }
    if (PlayerFarming.Location != FollowerLocation.Base || (double) animal.Satiation > 50.0 || !((UnityEngine.Object) ranch != (UnityEngine.Object) null) || animal.State != Interaction_Ranchable.State.Default)
      return;
    foreach (Structures_RanchTrough trough in ranch.Brain.GetTroughsContainingFood())
    {
      bool flag = false;
      foreach (PlacementRegion.TileGridTile ranchingTile in ranch.Brain.RanchingTiles)
      {
        if (ranchingTile.Position == trough.Data.GridTilePosition)
        {
          int num = (int) ranch.Brain.AnimalEatFood(animal, trough);
          this.UpdateSkin();
          flag = true;
          break;
        }
      }
      if (flag)
        break;
    }
  }

  public override void Update()
  {
    base.Update();
    if (this.animal.State == Interaction_Ranchable.State.Dead)
      return;
    bool flag1 = (double) (this.transform.position - this.animal.LastPosition).sqrMagnitude > 9.9999997473787516E-05;
    bool flag2 = true;
    if (this.targetTrough != null || (UnityEngine.Object) this.animalInteractionWheel != (UnityEngine.Object) null || this.CurrentState == Interaction_Ranchable.State.Leashed || this.CurrentState == Interaction_Ranchable.State.Sleeping || this.CurrentState == Interaction_Ranchable.State.InsideHutch || this.CurrentState == Interaction_Ranchable.State.BabyInHutch || this.CurrentState == Interaction_Ranchable.State.Animating || this.CurrentState == Interaction_Ranchable.State.Riding || this.CurrentState == Interaction_Ranchable.State.EnteringHutch || this.CurrentState == Interaction_Ranchable.State.FollowPlayer || this.CurrentState == Interaction_Ranchable.State.MovingToAttack)
      flag2 = false;
    if ((double) Time.time > (double) this.moveTimer & flag2)
      this.Move();
    if (flag2 && PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom && this.CurrentState == Interaction_Ranchable.State.Default && this.unitObject.state.CURRENT_STATE != StateMachine.State.Moving)
      this.Move();
    this.stinky.gameObject.SetActive(this.animal.Ailment == Interaction_Ranchable.Ailment.Stinky & flag2);
    this.inverter.transform.localScale = new Vector3(this.spine.Skeleton.ScaleX, 1f, 1f);
    if (this.CurrentState != Interaction_Ranchable.State.Default && this.bubbleRoutine != null)
    {
      this.StopCoroutine(this.bubbleRoutine);
      this.bubble.Close();
      this.bubbleRoutine = (Coroutine) null;
    }
    if (this.CurrentState == Interaction_Ranchable.State.BreakingOut)
    {
      if ((PlayerFarming.Location == FollowerLocation.Base || PlayerFarming.Location == FollowerLocation.Church || PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom) && this.animal.IsPlayersAnimal() && this.IsFollowingPlayerActive())
        this.StartFollowingPlayer();
      if ((double) this.animal.Satiation <= 0.0 && TimeManager.CurrentDay > DataManager.Instance.LastAnimalToStarveDay)
      {
        DataManager.Instance.LastAnimalToStarveDay = TimeManager.CurrentDay;
        this.animal.CauseOfDeath = Interaction_Ranchable.CauseOfDeath.DiedFromStarvation;
        this.Die();
      }
    }
    else if (this.CurrentState == Interaction_Ranchable.State.Riding)
      this.UpdateRiding();
    else if (this.CurrentState == Interaction_Ranchable.State.Leashed)
      this.UpdateLeash();
    else if (this.CurrentState == Interaction_Ranchable.State.Overcrowded)
    {
      if (flag1)
      {
        if (this.spine.AnimationState.GetCurrent(0).Animation.Name != this.walk_anim)
          this.spine.AnimationState.SetAnimation(0, this.walk_anim, true);
      }
      else if (this.spine.AnimationState.GetCurrent(0).Animation.Name != "idle_starving")
        this.spine.AnimationState.SetAnimation(0, "idle_starving", true);
      if (!this.FeelsOvercrowded)
        this.CurrentState = Interaction_Ranchable.State.Default;
    }
    else if ((this.CurrentState == Interaction_Ranchable.State.Sleeping || this.CurrentState == Interaction_Ranchable.State.Default) && this.FeelsOvercrowded)
      this.CurrentState = Interaction_Ranchable.State.Overcrowded;
    else if (this.CurrentState == Interaction_Ranchable.State.Sleeping && TimeManager.CurrentPhase != DayPhase.Night)
    {
      this.CurrentState = Interaction_Ranchable.State.Default;
      if (this.animal != null && this.animal.Ailment == Interaction_Ranchable.Ailment.Feral)
        this.feralLoopSFX = AudioManager.Instance.CreateLoop("event:/dlc/animal/shared/feral_loop", this.gameObject, true);
    }
    else if (this.CurrentState == Interaction_Ranchable.State.Sleeping)
    {
      if (this.spine.AnimationState.GetCurrent(0).Animation.Name != "sleeping")
        this.spine.AnimationState.SetAnimation(0, "sleeping", true);
    }
    else if (this.CurrentState == Interaction_Ranchable.State.FollowPlayer)
      this.UpdateFollowPlayer();
    else if (this.CurrentState == Interaction_Ranchable.State.MovingToAttack)
      this.UpdateMovingToAttack();
    else if (this.CurrentState == Interaction_Ranchable.State.Default && this.IsFollowingPlayerActive())
    {
      this.CurrentState = Interaction_Ranchable.State.FollowPlayer;
      this.CheckBreakingOut();
    }
    else if ((double) this.animal.Satiation <= 0.0 && TimeManager.CurrentDay > DataManager.Instance.LastAnimalToStarveDay)
    {
      DataManager.Instance.LastAnimalToStarveDay = TimeManager.CurrentDay;
      this.animal.CauseOfDeath = Interaction_Ranchable.CauseOfDeath.DiedFromStarvation;
      this.Die();
    }
    else if (this.isStarving && this.bubbleRoutine == null && PlayerFarming.Location == FollowerLocation.Base)
      this.bubbleRoutine = this.StartCoroutine((IEnumerator) this.BubbleRoutine(WorshipperBubble.SPEECH_TYPE.STARVING));
    else if ((double) this.animal.Injured == 0.0)
    {
      this.animal.CauseOfDeath = Interaction_Ranchable.CauseOfDeath.DiedFromInjury;
      this.Die();
    }
    else if (this.animal.Ailment == Interaction_Ranchable.Ailment.Feral && (double) this.animal.FeralCalming <= 0.0)
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
      NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalNotFeral", this.animal.GetName());
      this.Calm();
    }
    if (this.CurrentState == Interaction_Ranchable.State.Leashed || this.CurrentState == Interaction_Ranchable.State.Riding || this.CurrentState == Interaction_Ranchable.State.InsideHutch || this.CurrentState == Interaction_Ranchable.State.BabyInHutch)
      this.eatenTodayIcon.gameObject.SetActive(false);
    else
      this.eatenTodayIcon.SetActive(this.animal.EatenToday);
    this.spine.gameObject.SetActive(this.CurrentState != Interaction_Ranchable.State.InsideHutch);
    this.animal.LastPosition = this.transform.position;
  }

  public void OnPlayerJoined()
  {
    if (this.CurrentState == Interaction_Ranchable.State.FollowPlayer || !this.animal.IsPlayersAnimal())
      return;
    this.StartFollowingPlayer();
  }

  public new void OnPlayerLeft()
  {
    if (!this.animal.IsPlayersAnimal())
      return;
    for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
    {
      if (PlayerFarming.players.Count <= index && DataManager.Instance.FollowingPlayerAnimals[index] == this.animal)
        this.ResetAnimalState();
    }
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.SecondaryInteractable = false;
    this.HasThirdInteraction = false;
    this.ThirdInteractable = false;
    if (this.CurrentState == Interaction_Ranchable.State.InsideHutch || this.CurrentState == Interaction_Ranchable.State.BabyInHutch || this.CurrentState == Interaction_Ranchable.State.MovingToAttack || this.targetHutch != null || this.isBeingFed || this.BeingAscended)
      this.Label = "";
    else if (this.CurrentState == Interaction_Ranchable.State.Dead)
    {
      this.Interactable = true;
      this.Label = string.Format(ScriptLocalization.Interactions.CleanUpDeadAnimal, (object) this.DieNameString());
    }
    else if (this.CurrentState == Interaction_Ranchable.State.Leashed || this.CurrentState == Interaction_Ranchable.State.Riding)
    {
      this.Interactable = true;
      Interaction_Ranch closestRanch = this.GetClosestRanch();
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this._playerFarming.transform.position);
      if (!this.animal.IsPlayersAnimal() && (UnityEngine.Object) closestRanch != (UnityEngine.Object) null && closestRanch.IsValid && tileAtWorldPosition != null && (closestRanch.Brain.RanchingTiles.Contains(tileAtWorldPosition) || closestRanch.Brain.ContainsFence(tileAtWorldPosition) || tileAtWorldPosition.ObjectOnTile == StructureBrain.TYPES.RANCH || tileAtWorldPosition.ObjectOnTile == StructureBrain.TYPES.RANCH_2))
      {
        this.Interactable = true;
        this.Label = LocalizationManager.GetTranslation("UI/ReleaseInRanch");
      }
      else if (PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom)
      {
        this.Interactable = true;
        if (DataManager.Instance.SacrificeTableInventory.Count <= 0 && SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !DataManager.Instance.CompletedOfferingThisBlizzard)
          LambTownController.Instance.IcegoreDropOffItem.SetOnState(true);
        else
          LambTownController.Instance.IcegoreDropOffItem.SetOnState(false);
        if ((double) Vector3.Distance(this.transform.position, LambTownController.Instance.RanchingJobBoard.transform.position) < 2.5 && !LambTownController.Instance.RanchingJobBoard.BoardCompleted)
        {
          if (ObjectiveManager.IsAnimalValid(this.Animal))
          {
            LambTownController.Instance.RanchDropOffItem.SetDropOffState(true, true);
            this.Outliner.OutlineLayers[0].Clear();
            this.Outliner.OutlineLayers[0].Add(LambTownController.Instance.RanchingJobBoard.OutlineTarget);
            this.Label = $"{ScriptLocalization.Interactions_Bank.Deposit} {this.animal.GetName()}";
          }
          else if (!ObjectiveManager.HasAnimalObjectiveActive())
          {
            LambTownController.Instance.RanchDropOffItem.SetDropOffState(true, false);
            this.Label = ScriptLocalization.UI_PauseScreen_Quests.NoActiveQuests;
          }
          else
          {
            LambTownController.Instance.RanchDropOffItem.SetDropOffState(true, false);
            this.Label = ScriptLocalization.UI_JobBoard.WrongAnimal;
          }
          LambTownController.Instance.IcegoreDropOffItem.SetDropOffState(false, false);
        }
        else if ((double) Vector3.Distance(this.transform.position, Interaction_SacrificeTable.Instance.transform.position) < 2.5 && DataManager.Instance.SacrificeTableInventory.Count <= 0 && SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !DataManager.Instance.CompletedOfferingThisBlizzard)
        {
          this.Outliner.OutlineLayers[0].Clear();
          this.Outliner.OutlineLayers[0].Add(Interaction_SacrificeTable.Instance.OutlineTarget);
          this.Label = ScriptLocalization.Interactions.SacrificeAnimal;
          LambTownController.Instance.RanchDropOffItem.SetDropOffState(false, false);
          LambTownController.Instance.IcegoreDropOffItem.SetDropOffState(true, true);
        }
        else
        {
          if (this.Outliner.OutlineLayers[0].Contains(LambTownController.Instance.RanchingJobBoard.OutlineTarget))
            this.Outliner.OutlineLayers[0].Remove(LambTownController.Instance.RanchingJobBoard.OutlineTarget);
          if (this.Outliner.OutlineLayers[0].Contains(Interaction_SacrificeTable.Instance.OutlineTarget))
            this.Outliner.OutlineLayers[0].Remove(Interaction_SacrificeTable.Instance.OutlineTarget);
          this.Label = ScriptLocalization.UI_PrisonMenu.Release;
          LambTownController.Instance.RanchDropOffItem.SetDropOffState(false, false);
          LambTownController.Instance.IcegoreDropOffItem.SetDropOffState(false, false);
        }
      }
      else
      {
        this.Interactable = true;
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.FurnaceAnimal) && Interaction_DLCFurnace.Instance.CanAddFuel())
          Interaction_DLCFurnace.Instance.dropOffAnimalFurnace.SetOnState(true);
        else
          Interaction_DLCFurnace.Instance.dropOffAnimalFurnace.SetOnState(false);
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.FurnaceAnimal) && (UnityEngine.Object) Interaction_DLCFurnace.Instance != (UnityEngine.Object) null && Interaction_DLCFurnace.Instance.CanAddFuel() && (double) Vector3.Distance(this.transform.position, Interaction_DLCFurnace.Instance.transform.position) < 2.5)
        {
          this.Outliner.OutlineLayers[0].Clear();
          this.Outliner.OutlineLayers[0].Add(Interaction_DLCFurnace.Instance.OutlineTarget);
          this.Label = LocalizationManager.GetTranslation("Interactions/BurnAnimal");
          Interaction_DLCFurnace.Instance.dropOffAnimalFurnace.SetDropOffState(true, true);
        }
        else
        {
          if ((UnityEngine.Object) Interaction_DLCFurnace.Instance != (UnityEngine.Object) null && this.Outliner.OutlineLayers[0].Contains(Interaction_DLCFurnace.Instance.OutlineTarget))
            this.Outliner.OutlineLayers[0].Remove(Interaction_DLCFurnace.Instance.OutlineTarget);
          this.Label = ScriptLocalization.UI_PrisonMenu.Release;
          Interaction_DLCFurnace.Instance.dropOffAnimalFurnace.SetDropOffState(false, false);
        }
      }
    }
    else if (Interaction_WolfBase.WolvesActive)
    {
      this.Interactable = false;
      this.Label = "";
    }
    else
    {
      this.Interactable = true;
      this.Label = this.animalNameLevelLabel;
      this.HasSecondaryInteraction = false;
      this.SecondaryInteractable = false;
      if (this.Animal.WorkedReady && !this.Animal.WorkedToday && (this.animal.Ailment == Interaction_Ranchable.Ailment.None || this.animal.Ailment == Interaction_Ranchable.Ailment.Injured))
      {
        this.HasSecondaryInteraction = true;
        this.SecondaryInteractable = true;
      }
      if (this.Animal.Ailment == Interaction_Ranchable.Ailment.Stinky)
      {
        this.HasSecondaryInteraction = true;
        this.SecondaryInteractable = true;
      }
      if (!Interaction_Ranchable.milkables.Contains(this.Animal.Type) || this.animal.MilkedToday || !this.animal.MilkedReady)
        return;
      this.HasThirdInteraction = true;
      this.ThirdInteractable = true;
    }
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    if (this.Animal.Ailment == Interaction_Ranchable.Ailment.Stinky)
      this.SecondaryLabel = ScriptLocalization.Interactions.Clean;
    else
      this.SecondaryLabel = this.SecondaryInteractable ? Interaction_Ranchable.GetWorkDescription(this.Animal, true) : "";
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    if (this.Animal.Ailment == Interaction_Ranchable.Ailment.Stinky)
      this.StartCoroutine((IEnumerator) this.CleanIE());
    else
      this.StartCoroutine((IEnumerator) this.WorkIE());
  }

  public override void GetThirdLabel()
  {
    base.GetSecondaryLabel();
    this.ThirdLabel = this.ThirdInteractable ? Interaction_Ranchable.GetMilkDescription() : "";
  }

  public override void OnThirdInteract(StateMachine state)
  {
    base.OnThirdInteract(state);
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    this.StartCoroutine((IEnumerator) this.MilkAnimalIE());
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom)
    {
      if ((double) Vector3.Distance(this.transform.position, LambTownController.Instance.RanchingJobBoard.transform.position) < 2.5 && !LambTownController.Instance.RanchingJobBoard.BoardCompleted)
      {
        if (ObjectiveManager.IsAnimalValid(this.Animal))
        {
          Debug.Log((object) "Animal is valid, completing objective");
          ObjectiveManager.CompleteAnimalObjective(this.Animal.Type, this.animal.Level);
          LambTownController.Instance.RanchDropOffItem.SetOnState(false);
          GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => LambTownController.Instance.RanchingJobBoard.OnInteract(state)));
          if (this.CurrentState == Interaction_Ranchable.State.Riding)
            this.EndRiding(false);
          else if (this.CurrentState == Interaction_Ranchable.State.Leashed)
            this.DetatchLeash(false);
          this.moveTimer = Time.time + 5f;
          return;
        }
        Debug.Log((object) "Animal is NOT valid, completing objective");
        this.playerFarming.indicator.PlayShake();
        return;
      }
      if ((double) Vector3.Distance(this.transform.position, Interaction_SacrificeTable.Instance.transform.position) < 2.5 && DataManager.Instance.SacrificeTableInventory.Count <= 0 && SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard && !DataManager.Instance.CompletedOfferingThisBlizzard)
      {
        Interaction_SacrificeTable.Instance.OnInteract(state);
        if (this.CurrentState == Interaction_Ranchable.State.Riding)
          this.EndRiding();
        else if (this.CurrentState == Interaction_Ranchable.State.Leashed)
          this.DetatchLeash();
        LambTownController.Instance.IcegoreDropOffItem.SetOnState(false);
        return;
      }
    }
    else if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.FurnaceAnimal) && Interaction_DLCFurnace.Instance.CanAddFuel() && (UnityEngine.Object) Interaction_DLCFurnace.Instance != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, Interaction_DLCFurnace.Instance.transform.position) < 2.5)
    {
      Interaction_DLCFurnace.Instance.OnInteract(state);
      Interaction_DLCFurnace.Instance.dropOffAnimalFurnace.SetOnState(false);
      return;
    }
    this.ReservedByPlayer = true;
    if (this.CurrentState == Interaction_Ranchable.State.Riding)
    {
      if ((UnityEngine.Object) LambTownController.Instance != (UnityEngine.Object) null && (UnityEngine.Object) LambTownController.Instance.IcegoreDropOffItem != (UnityEngine.Object) null)
        LambTownController.Instance.IcegoreDropOffItem.SetOnState(false);
      if ((UnityEngine.Object) Interaction_DLCFurnace.Instance != (UnityEngine.Object) null && (UnityEngine.Object) Interaction_DLCFurnace.Instance.dropOffAnimalFurnace != (UnityEngine.Object) null)
        Interaction_DLCFurnace.Instance.dropOffAnimalFurnace.SetOnState(false);
      this.EndRiding();
    }
    else if (this.CurrentState == Interaction_Ranchable.State.Leashed)
    {
      if ((UnityEngine.Object) LambTownController.Instance != (UnityEngine.Object) null && (UnityEngine.Object) LambTownController.Instance.IcegoreDropOffItem != (UnityEngine.Object) null)
        LambTownController.Instance.IcegoreDropOffItem.SetOnState(false);
      if ((UnityEngine.Object) Interaction_DLCFurnace.Instance != (UnityEngine.Object) null && (UnityEngine.Object) Interaction_DLCFurnace.Instance.dropOffAnimalFurnace != (UnityEngine.Object) null)
        Interaction_DLCFurnace.Instance.dropOffAnimalFurnace.SetOnState(false);
      this.DetatchLeash();
    }
    else if (this.CurrentState == Interaction_Ranchable.State.Dead)
    {
      this.StartCoroutine((IEnumerator) this.RemoveDeadBodyIE());
    }
    else
    {
      GameManager.GetInstance().OnConversationNew(false);
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -1f));
      HUD_Manager.Instance.Hide(false, 0);
      this.playerFarming.state.CURRENT_STATE = StateMachine.State.InActive;
      LetterBox.Instance.ShowSubtitle(this.animal.GivenName);
      GameManager.GetInstance().OnConversationNext(this.gameObject, 2f);
      this._playerFarming.state.facingAngle = Utils.GetAngle(this._playerFarming.transform.position, this.transform.position);
      SimulationManager.Pause();
      this.ClearPath();
      List<CommandItem> commandItems;
      if (this.animal.Ailment == Interaction_Ranchable.Ailment.Injured)
      {
        this.FlowerType = InventoryItem.ITEM_TYPE.FLOWER_RED;
        foreach (InventoryItem.ITEM_TYPE allFlower in InventoryItem.AllFlowers)
        {
          if (Inventory.GetItemQuantity(allFlower) >= 3)
          {
            this.FlowerType = allFlower;
            break;
          }
        }
        commandItems = AnimalCommandGroups.InjuredCommands(this.animal, this.FlowerType);
      }
      else
        commandItems = this.animal.Ailment != Interaction_Ranchable.Ailment.Feral ? (this.animal.Ailment != Interaction_Ranchable.Ailment.Stinky ? (!this.FeelsOvercrowded ? AnimalCommandGroups.DefaultCommands(this.animal) : AnimalCommandGroups.OvercrowdedCommands(this.animal)) : AnimalCommandGroups.StinkyCommands(this.animal)) : AnimalCommandGroups.FeralCommands(this.animal);
      this.animalInteractionWheel = MonoSingleton<UIManager>.Instance.FollowerInteractionWheelTemplate.Instantiate<UIFollowerInteractionWheelOverlayController>();
      this.animalInteractionWheel.Show(this.animal, commandItems, false, true);
      UIFollowerInteractionWheelOverlayController interactionWheel1 = this.animalInteractionWheel;
      interactionWheel1.OnItemChosen = interactionWheel1.OnItemChosen + new System.Action<FollowerCommands[]>(this.OnAnimalCommandFinalized);
      CoopManager.Instance.OnPlayerLeft += new System.Action(this.UIOnPlayerLeft);
      UIFollowerInteractionWheelOverlayController interactionWheel2 = this.animalInteractionWheel;
      interactionWheel2.OnHidden = interactionWheel2.OnHidden + (System.Action) (() =>
      {
        this.OnEndPath();
        CoopManager.Instance.OnPlayerLeft -= new System.Action(this.UIOnPlayerLeft);
      });
      UIFollowerInteractionWheelOverlayController interactionWheel3 = this.animalInteractionWheel;
      interactionWheel3.OnCancel = interactionWheel3.OnCancel + new System.Action(this.Close);
    }
    if (this.postFeedRoutine == null)
      return;
    this.StopCoroutine(this.postFeedRoutine);
  }

  public void Close() => this.StartCoroutine((IEnumerator) this.CloseRoutine());

  public IEnumerator CloseRoutine()
  {
    yield return (object) new UnityEngine.WaitForSeconds(0.05f);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    if (this.CurrentState != Interaction_Ranchable.State.Riding && this.CurrentState != Interaction_Ranchable.State.Leashed && !this.animal.IsPlayersAnimal())
      this.ReservedByPlayer = false;
    SimulationManager.UnPause();
  }

  public void OnAnimalCommandFinalized(params FollowerCommands[] followerCommands)
  {
    SimulationManager.UnPause();
    FollowerCommands followerCommand = followerCommands[0];
    FollowerCommands followerCommands1 = followerCommands.Length > 1 ? followerCommands[1] : FollowerCommands.None;
    switch (followerCommand)
    {
      case FollowerCommands.AreYouSureYes:
        switch (followerCommands1)
        {
          case FollowerCommands.Ascend:
            this.StartCoroutine((IEnumerator) this.AscendIE());
            return;
          case FollowerCommands.Slaughter:
            this.StartCoroutine((IEnumerator) this.SlaughterIE());
            return;
          case FollowerCommands.Sacrifice:
            this.StartCoroutine((IEnumerator) this.SacrificeIE());
            return;
          default:
            return;
        }
      case FollowerCommands.Ascend:
        this.StartCoroutine((IEnumerator) this.AscendIE());
        return;
      case FollowerCommands.Harvest:
        if (!this.animal.WorkedToday && this.animal.WorkedReady)
        {
          this.StartCoroutine((IEnumerator) this.WorkIE());
          return;
        }
        break;
      case FollowerCommands.FeedGrass:
        this.Feed(InventoryItem.ITEM_TYPE.GRASS);
        return;
      case FollowerCommands.FeedPoop:
        this.Feed(InventoryItem.ITEM_TYPE.POOP);
        return;
      case FollowerCommands.FeedFollowerMeat:
        this.Feed(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT);
        return;
      case FollowerCommands.Walk:
        this.AttachLeash();
        break;
      case FollowerCommands.Ride:
        if ((double) this.animal.Level >= (double) AnimalInteractionModel.RideItem.REQUIRED_LEVEL)
        {
          this.BeginRiding();
          break;
        }
        break;
      case FollowerCommands.Clean:
        this.StartCoroutine((IEnumerator) this.CleanIE());
        return;
      case FollowerCommands.PetAnimal:
        if (!this.animal.PetToday)
          this.StartCoroutine((IEnumerator) this.PetIE());
        else
          this.ReservedByPlayer = false;
        GameManager.GetInstance().OnConversationEnd();
        return;
      case FollowerCommands.NameAnimal:
        this.StartCoroutine((IEnumerator) this.NameAnimalIE());
        return;
      case FollowerCommands.FollowPlayer:
        this.StartFollowingPlayer();
        break;
      case FollowerCommands.StopFollowingPlayer:
        this.EndFollowingPlayer();
        break;
      case FollowerCommands.FeedBerry:
        this.Feed(InventoryItem.ITEM_TYPE.BERRY);
        return;
      case FollowerCommands.FeedFlowerRed:
        this.Feed(InventoryItem.ITEM_TYPE.FLOWER_RED);
        return;
      case FollowerCommands.FeedMushroom:
        this.Feed(InventoryItem.ITEM_TYPE.MUSHROOM_BIG);
        return;
      case FollowerCommands.FeedPumpkin:
        this.Feed(InventoryItem.ITEM_TYPE.PUMPKIN);
        return;
      case FollowerCommands.FeedBeetroot:
        this.Feed(InventoryItem.ITEM_TYPE.BEETROOT);
        return;
      case FollowerCommands.FeedCauliflower:
        this.Feed(InventoryItem.ITEM_TYPE.CAULIFLOWER);
        return;
      case FollowerCommands.FeedGrapes:
        this.Feed(InventoryItem.ITEM_TYPE.GRAPES);
        return;
      case FollowerCommands.FeedHops:
        this.Feed(InventoryItem.ITEM_TYPE.HOPS);
        return;
      case FollowerCommands.FeedFish:
        this.Feed(InventoryItem.ITEM_TYPE.FISH);
        return;
      case FollowerCommands.FeedFishBig:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_BIG);
        return;
      case FollowerCommands.FeedFishBlowfish:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_BLOWFISH);
        return;
      case FollowerCommands.FeedFishCrab:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_CRAB);
        return;
      case FollowerCommands.FeedFishLobster:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_LOBSTER);
        return;
      case FollowerCommands.FeedFishOctopus:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_OCTOPUS);
        return;
      case FollowerCommands.FeedFishSmall:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_SMALL);
        return;
      case FollowerCommands.FeedFishSquid:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_SQUID);
        return;
      case FollowerCommands.FeedFishSwordfish:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_SWORDFISH);
        return;
      case FollowerCommands.FeedFishCod:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_COD);
        return;
      case FollowerCommands.FeedFishCatfish:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_CATFISH);
        return;
      case FollowerCommands.FeedFishPike:
        this.Feed(InventoryItem.ITEM_TYPE.FISH_PIKE);
        return;
      case FollowerCommands.FeedMeat:
        this.Feed(InventoryItem.ITEM_TYPE.MEAT);
        return;
      case FollowerCommands.FeedMeatMorsel:
        this.Feed(InventoryItem.ITEM_TYPE.MEAT_MORSEL);
        return;
      case FollowerCommands.FeedYolk:
        this.Feed(InventoryItem.ITEM_TYPE.YOLK);
        return;
      case FollowerCommands.FeedChilli:
        this.Feed(InventoryItem.ITEM_TYPE.CHILLI);
        return;
      case FollowerCommands.Calm:
        if ((double) TimeManager.TotalElapsedGameTime > (double) this.animal.AilmentGameTime + 600.0)
        {
          this.StartCoroutine((IEnumerator) this.CalmIE());
          return;
        }
        break;
      case FollowerCommands.MilkAnimal:
        if (!this.animal.MilkedToday && this.animal.MilkedReady)
        {
          this.StartCoroutine((IEnumerator) this.MilkAnimalIE());
          return;
        }
        break;
      case FollowerCommands.Heal:
        if (Inventory.GetItemQuantity(this.FlowerType) >= 3)
        {
          this.StartCoroutine((IEnumerator) this.HealIE());
          return;
        }
        break;
    }
    this.Close();
  }

  public override void OnDrawGizmos()
  {
    base.OnDrawGizmos();
    if (!((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null))
      return;
    PlacementRegion.TileGridTile tileGridTile = (PlacementRegion.TileGridTile) null;
    for (int index1 = 0; index1 < Interaction_Ranchable.Ranchables.Count; ++index1)
    {
      if (Interaction_Ranchable.Ranchables[index1].CurrentTile != null)
      {
        tileGridTile = (PlacementRegion.TileGridTile) null;
        if ((UnityEngine.Object) Interaction_Ranchable.Ranchables[index1] != (UnityEngine.Object) this)
        {
          for (int index2 = 0; index2 < Interaction_Ranchable.positionCheckMoveDirections.Length; ++index2)
          {
            PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(Interaction_Ranchable.Ranchables[index1].CurrentTile.WorldPosition + Interaction_Ranchable.positionCheckMoveDirections[index2]);
            if (tileAtWorldPosition != null)
              Gizmos.DrawSphere(tileAtWorldPosition.WorldPosition, 0.3f);
          }
        }
      }
    }
  }

  public bool IsTileBlocked(PlacementRegion.TileGridTile tile)
  {
    return tile == null || tile.ObjectID != -1;
  }

  public void Move()
  {
    if (PlayerFarming.Location == FollowerLocation.Church || MMTransition.IsPlaying)
    {
      this.ClearPath();
    }
    else
    {
      this.spine.AnimationState.SetAnimation(0, this.walk_anim, true);
      this.moveTimer = Time.time + UnityEngine.Random.Range(10f, 17.5f);
      if (this.CurrentState == Interaction_Ranchable.State.BreakingOut && PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
      {
        Vector3 vector3 = (Vector3) UnityEngine.Random.insideUnitCircle * 5f;
        if ((bool) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) vector3.normalized, 5f, (int) this.collisionMask))
          vector3 *= -1f;
        Vector3 targetLocation = this.transform.position + vector3;
        targetLocation.x = Mathf.Clamp(targetLocation.x, PlacementRegion.X_Constraints.x, PlacementRegion.X_Constraints.y);
        targetLocation.y = Mathf.Clamp(targetLocation.y, PlacementRegion.Y_Constraints.x, PlacementRegion.Y_Constraints.y);
        this.unitObject.givePath(targetLocation);
      }
      else
      {
        this.currentTile = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.transform.position);
        this.topPriorityTiles.Clear();
        this.lowPriorityTiles.Clear();
        this.excludedTiles.Clear();
        for (int index = 0; index < Interaction_Ranchable.Ranchables.Count; ++index)
        {
          if (Interaction_Ranchable.Ranchables[index].CurrentTile != null)
          {
            if ((UnityEngine.Object) Interaction_Ranchable.Ranchables[index] != (UnityEngine.Object) this)
              this.excludedTiles.AddRange((IEnumerable<PlacementRegion.TileGridTile>) Interaction_Ranchable.Ranchables[index].reservedTiles);
            this.excludedTiles.Add(Interaction_Ranchable.Ranchables[index].CurrentTile);
          }
        }
        if ((UnityEngine.Object) this.ranch != (UnityEngine.Object) null && this.ranch.Brain != null)
        {
          for (int index1 = 0; index1 < this.ranch.Brain.RanchingTiles.Count; ++index1)
          {
            PlacementRegion.TileGridTile ranchingTile = this.ranch.Brain.RanchingTiles[index1];
            if (!this.excludedTiles.Contains(ranchingTile) && !this.ranch.Brain.ContainsFence(ranchingTile) && !this.IsTileBlocked(ranchingTile) && (double) Vector2Int.Distance(this.currentTile.Position, ranchingTile.Position) < 10.0)
            {
              for (int index2 = 0; index2 < Interaction_Ranchable.Ranchables.Count; ++index2)
              {
                if (Interaction_Ranchable.Ranchables[index2].CurrentTile != null && (double) Vector3.Distance(this.ranch.Brain.RanchingTiles[index1].WorldPosition, Interaction_Ranchable.Ranchables[index2].CurrentTile.WorldPosition) <= 1.0)
                {
                  this.lowPriorityTiles.Add(this.ranch.Brain.RanchingTiles[index1]);
                  break;
                }
              }
              if (!this.lowPriorityTiles.Contains(this.ranch.Brain.RanchingTiles[index1]))
                this.topPriorityTiles.Add(this.ranch.Brain.RanchingTiles[index1]);
            }
          }
        }
        if (this.topPriorityTiles.Count == 0)
        {
          if (this.lowPriorityTiles.Count == 0)
          {
            if ((UnityEngine.Object) this.ranch != (UnityEngine.Object) null && this.ranch.Brain != null)
            {
              for (int index = 0; index < this.ranch.Brain.RanchingTiles.Count; ++index)
              {
                PlacementRegion.TileGridTile ranchingTile = this.ranch.Brain.RanchingTiles[index];
                if (!this.excludedTiles.Contains(ranchingTile) && !this.ranch.Brain.ContainsFence(ranchingTile) && !this.IsTileBlocked(ranchingTile))
                  this.topPriorityTiles.Add(this.ranch.Brain.RanchingTiles[index]);
              }
            }
          }
          else
            Debug.Log((object) "NO TILES AVAILALBLE = FALL BACK!");
        }
        PlacementRegion.TileGridTile tileGridTile = (PlacementRegion.TileGridTile) null;
        if (this.targetHutch != null)
        {
          foreach (PlacementRegion.TileGridTile ranchingTile in this.ranch.Brain.RanchingTiles)
          {
            if (ranchingTile.ObjectID == this.targetHutch.Data.ID)
            {
              tileGridTile = ranchingTile;
              break;
            }
          }
        }
        else if ((double) this.animal.Satiation < 50.0 && (UnityEngine.Object) this.ranch != (UnityEngine.Object) null && this.animal.State == Interaction_Ranchable.State.Default)
        {
          foreach (Structures_RanchTrough structuresRanchTrough in this.ranch.Brain.GetTroughsContainingFood())
          {
            bool flag = false;
            foreach (PlacementRegion.TileGridTile ranchingTile in this.ranch.Brain.RanchingTiles)
            {
              if (ranchingTile.Position == structuresRanchTrough.Data.GridTilePosition)
              {
                tileGridTile = ranchingTile;
                this.targetTrough = structuresRanchTrough;
                flag = true;
                break;
              }
            }
            if (flag)
              break;
          }
        }
        if (PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom)
        {
          this.CurrentState = Interaction_Ranchable.State.Default;
          this.unitObject.givePath(new Vector3(0.0f, 10f, 0.0f));
        }
        else if (tileGridTile != null)
        {
          this.CurrentTile = tileGridTile;
          this.unitObject.givePath(this.CurrentTile.WorldPosition);
          this.UpdateReservedTiles();
        }
        else if (this.topPriorityTiles.Count > 0)
        {
          this.CurrentTile = this.topPriorityTiles[UnityEngine.Random.Range(0, this.topPriorityTiles.Count)];
          this.unitObject.givePath(this.CurrentTile.WorldPosition, forceAStar: true);
          this.UpdateReservedTiles();
        }
        else
          this.spine.AnimationState.SetAnimation(0, this.idle_anim, true);
      }
    }
  }

  public void OnEndPath()
  {
    if (this.CurrentState == Interaction_Ranchable.State.Dead)
      return;
    if (PlayerFarming.Location != FollowerLocation.Base && (double) this.transform.position.y < 11.0 && (this.CurrentState == Interaction_Ranchable.State.BreakingOut || this.CurrentState == Interaction_Ranchable.State.Default))
    {
      this.transform.parent = BaseLocationManager.Instance.UnitLayer;
      this.transform.position = Vector3.zero;
    }
    else
    {
      this.Interactable = true;
      if (this.targetHutch != null)
      {
        if (this.targetHutch.Data.Destroyed)
        {
          this.targetHutch = (Structures_RanchHutch) null;
        }
        else
        {
          this.CurrentState = Interaction_Ranchable.State.EnteringHutch;
          this.currentHutchID = this.targetHutch.Data.ID;
          System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch> onEnterHutch = this.OnEnterHutch;
          if (onEnterHutch != null)
            onEnterHutch(this.animal, this.targetHutch);
          this.spine.transform.DOLocalMoveZ(-0.5f, 0.5f);
          this.spine.transform.DOLocalMoveZ(0.0f, 0.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f);
          Vector3 oldScale = this.spine.transform.localScale;
          this.spine.transform.DOScale(0.0f, 0.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f);
          this.transform.DOMove(this.targetHutch.Data.Position, 1f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
          {
            System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch> onEnteredHutch = this.OnEnteredHutch;
            if (onEnteredHutch != null)
              onEnteredHutch(this.animal, this.targetHutch);
            this.spine.transform.localScale = oldScale;
            this.CurrentState = Interaction_Ranchable.State.InsideHutch;
            this.targetHutch = (Structures_RanchHutch) null;
            AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/barn_enter", this.transform.position);
          }));
        }
      }
      else if (this.targetTrough != null && this.targetTrough.Data.Inventory.Count > 0)
      {
        InventoryItem.ITEM_TYPE food = this.ranch.Brain.AnimalEatFood(this.animal, this.targetTrough);
        this.UpdateSkin();
        this.Eat(food);
        this.targetTrough = (Structures_RanchTrough) null;
      }
      else
      {
        if ((double) TimeManager.TotalElapsedGameTime > (double) this.animal.TimeSinceLastWash && this.animal.Ailment == Interaction_Ranchable.Ailment.None && !this.isStarving && ((UnityEngine.Object) this.ranch == (UnityEngine.Object) null || (UnityEngine.Object) this.ranch != (UnityEngine.Object) null && this.ranch.Structure.Type != StructureBrain.TYPES.RANCH_2))
        {
          this.animal.Ailment = Interaction_Ranchable.Ailment.Stinky;
          this.animal.AilmentGameTime = TimeManager.TotalElapsedGameTime;
          NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalStinky", this.animal.GetName());
          this.UpdateSkin();
        }
        if (this.CurrentState != Interaction_Ranchable.State.MovingToAttack && this.CurrentState != Interaction_Ranchable.State.Animating)
          this.targetAnimal = (Interaction_Ranchable) null;
        if (this.CurrentState != Interaction_Ranchable.State.BreakingOut && TimeManager.CurrentPhase == DayPhase.Night && !this.animal.IsPlayersAnimal())
        {
          if (this.CurrentState != Interaction_Ranchable.State.Animating && this.CurrentState != Interaction_Ranchable.State.Riding && this.CurrentState != Interaction_Ranchable.State.Leashed)
            this.Sleep();
        }
        else if (this.CurrentState != Interaction_Ranchable.State.Animating)
        {
          this.spine.AnimationState.SetAnimation(0, this.idle_anim, true);
          if (this.CurrentState == Interaction_Ranchable.State.BreakingOut)
            this.CheckBreakingOut();
        }
      }
      if ((double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.TimeSinceLastWolf && DataManager.Instance.OnboardedRanchingWolves && !this.animal.IsPlayersAnimal() && SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.None && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (UnityEngine.Object) this.enemyWolf == (UnityEngine.Object) null && (double) UnityEngine.Random.value < 0.20000000298023224)
        this.SpawnWolfSequence();
      this.UpdateSkin();
    }
  }

  public void OnNewDayStarted()
  {
    this.UpdateGrowthState();
    if (this.animal.Age < 15)
      return;
    this.CheckIfItsTimeToDie();
  }

  public void CheckIfItsTimeToDie()
  {
    if (this.animal == null || this.animal.State == Interaction_Ranchable.State.Dead || this.animal.IsPlayersAnimal() || this.animal.State == Interaction_Ranchable.State.Leashed || this.animal.State == Interaction_Ranchable.State.Riding || (double) UnityEngine.Random.value > (double) (this.animal.Age / 100) && !this.overrideDieChance)
      return;
    this.animal.CauseOfDeath = Interaction_Ranchable.CauseOfDeath.DiedFromOldAge;
    this.Die();
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalDiedFromOldAge", this.DieNameString());
  }

  public string DieNameString()
  {
    string format = this.animal.Age < 15 ? (this.animal.Age >= 2 ? "" : LocalizationManager.GetTranslation("UI/RanchAssignMenu/Baby")) : LocalizationManager.GetTranslation("UI/RanchAssignMenu/Old");
    string str = string.IsNullOrEmpty(format) ? (string.IsNullOrEmpty(this.animal.GivenName) ? InventoryItem.LocalizedName(this.animal.Type).Colour(StaticColors.YellowColorHex) : this.animal.GivenName.Colour(StaticColors.YellowColorHex)) : (string.IsNullOrEmpty(this.animal.GivenName) ? string.Format(format, (object) InventoryItem.LocalizedName(this.animal.Type).Colour(StaticColors.YellowColorHex)) : string.Format(format, (object) this.animal.GivenName.Colour(StaticColors.YellowColorHex)));
    if (this.animal.BestFriend)
      str = "\uF004 " + str;
    return str;
  }

  public void OnNewPhaseStarted()
  {
    this.UpdateSkin();
    if (this.CurrentState == Interaction_Ranchable.State.InsideHutch)
    {
      this.moveTimer = 0.0f;
      this.CurrentState = Interaction_Ranchable.State.Default;
    }
    else
    {
      if (this.CurrentState == Interaction_Ranchable.State.BreakingOut || this.CurrentState == Interaction_Ranchable.State.Animating || this.CurrentState == Interaction_Ranchable.State.Riding || this.CurrentState == Interaction_Ranchable.State.Leashed || TimeManager.CurrentPhase != DayPhase.Night || this.animal.IsPlayersAnimal())
        return;
      this.Sleep();
    }
  }

  public void UpdateGrowthState()
  {
    if (this.CurrentState == Interaction_Ranchable.State.Dead)
      return;
    this.UpdateSkin();
  }

  public void UpdateSkin()
  {
    int animalGrowthState = Structures_Ranch.GetAnimalGrowthState(this.animal);
    Skin newSkin = new Skin("Skin");
    string lower = this.ranchableType.ToString().Replace("ANIMAL_", "").ToLower();
    char upper = char.ToUpper(lower[0]);
    string str1 = lower.Remove(0, 1).Insert(0, upper.ToString());
    string str2 = str1;
    string skinName = !this.animal.WorkedToday || !Interaction_Ranchable.shearables.Contains(this.animal.Type) ? (!this.animal.WorkedReady || this.animal.WorkedToday ? $"{str2}/{str1}" : $"{str2}/{str1}_Ready") : $"{str2}/{str1}_Sheared";
    newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin(skinName));
    if (this.animal.Age < 2)
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Baby"));
    else if (this.animal.Age >= 15)
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Prime"));
    else if ((double) this.animal.Satiation <= 25.0)
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Starving"));
    else if (animalGrowthState >= 6)
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Fat"));
    else
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Normal"));
    if (this.animal.Ailment == Interaction_Ranchable.Ailment.Feral)
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Feral"));
    else if (this.animal.Ailment == Interaction_Ranchable.Ailment.Injured)
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Injured"));
    else if (this.animal.Ailment == Interaction_Ranchable.Ailment.Stinky)
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Stinky"));
    else if (this.animal.State == Interaction_Ranchable.State.Overcrowded)
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("State/Starving"));
    if (this.Animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Horns/{this.animal.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Ears/{this.animal.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Head/{this.animal.Head}"));
    }
    else if (this.Animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Horns/{this.animal.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Ears/{this.animal.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Head/{this.animal.Head}"));
    }
    else if (this.Animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Tail/{this.animal.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Ears/{this.animal.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Head/{this.animal.Head}"));
    }
    else if (this.Animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Claws/{this.animal.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Shell/{this.animal.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Head/{this.animal.Head}"));
    }
    else if (this.Animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Body/{this.animal.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Face/{this.animal.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Shell/{this.animal.Head}"));
    }
    else if (this.Animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Body/{this.animal.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Eyes/{this.animal.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Head/{this.animal.Head}"));
    }
    else if (this.Animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
    {
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Body/{this.animal.Horns}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Face/{this.animal.Ears}"));
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin($"{str1}/Head/{this.animal.Head}"));
    }
    if (this.Animal.IsPlayersAnimal())
      newSkin.AddSkin(this.spine.Skeleton.Data.FindSkin("Necklaces/1"));
    this.spine.Skeleton.SetSkin(newSkin);
    this.spine.Skeleton.SetSlotsToSetupPose();
    if ((UnityEngine.Object) AnimalData.Instance != (UnityEngine.Object) null)
      this.colours = AnimalData.Instance.GetAnimalColors(this.animal.Type);
    foreach (WorshipperData.SlotAndColor slotAndColour in this.colours[Mathf.Clamp(this.animal.Colour, 0, this.colours.Length - 1)].SlotAndColours)
    {
      Slot slot = this.spine.Skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  public void UIOnPlayerLeft()
  {
    if (!(bool) (UnityEngine.Object) this.animalInteractionWheel)
      return;
    bool flag = false;
    foreach (UnityEngine.Object player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) this.playerFarming == player)
        flag = true;
    }
    if (flag)
      return;
    this.Close();
  }

  public void AttachLeash()
  {
    this.StopAllCoroutines();
    this.ClearPath();
    ObjectiveManager.CompleteWalkAnimalObjective(this.animal.ID);
    if (PlayerFarming.Location == FollowerLocation.Base)
      BaseGoopDoor.MainDoor.SetDoorUp();
    if (PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom)
      BaseGoopDoor.WoolhavenDoor.SetDoorUp();
    this.CurrentState = Interaction_Ranchable.State.Leashed;
    this.springJoint = this.gameObject.AddComponent<SpringJoint2D>();
    this.springJoint.frequency = 3.5f;
    this.springJoint.autoConfigureDistance = false;
    this.springJoint.distance = 1f;
    this.springJoint.connectedBody = this._playerFarming.GetComponent<Rigidbody2D>();
    this.PriorityWeight = float.MaxValue;
    this.ActivateDistance = 5f;
    this.OutlineTarget = this.emptyOutline;
    this.simpleSpineFlash.SetFacingType(SimpleSpineFlash.SetFacingMode.Ignore);
    this._playerFarming.AllowDodging = false;
    this._playerFarming.BlockMeditation = true;
    if ((bool) (UnityEngine.Object) this.ranch)
    {
      int num1 = this.ranch.IsOvercrowded ? 1 : 0;
      AnimalData.RemoveAnimal(this.animal, this.ranch.Brain.Data.Animals);
      int num2 = this.ranch.IsOvercrowded ? 1 : 0;
      if (num1 != num2)
        this.NotifyOvercrowded(this.ranch);
    }
    if (!DataManager.Instance.BreakingOutAnimals.Contains(this.animal))
      DataManager.Instance.BreakingOutAnimals.Add(this.animal);
    this.leashTimestamp = Time.time;
    this.CurrentTile = (PlacementRegion.TileGridTile) null;
    this.makeHappyTimer = 0.0f;
    this.HasChanged = true;
    this.HasSecondaryInteraction = false;
    this.SecondaryInteractable = false;
  }

  public void DetatchLeash(bool resetMovement = true)
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.springJoint);
    this.interactionProgressBar.Hide(true);
    this.PriorityWeight = 1f;
    this.ActivateDistance = 1f;
    this.leash.positionCount = 0;
    this.OutlineTarget = (GameObject) null;
    this.simpleSpineFlash.SetFacingType(SimpleSpineFlash.SetFacingMode.Normal);
    this._playerFarming.AllowDodging = true;
    this._playerFarming.BlockMeditation = false;
    this.CurrentState = Interaction_Ranchable.State.BreakingOut;
    this.Interactable = false;
    this.ReservedByPlayer = false;
    this.leashTimestamp = -1f;
    this.ClearPath();
    if (resetMovement)
      this.moveTimer = 0.0f;
    this.CheckBreakingOut(true);
    this.HasChanged = true;
    BaseGoopDoor.DoorDown();
    if (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
      return;
    BaseGoopDoor.WoolhavenDoor.CheckWoolhavenDoor();
    if (!resetMovement)
      return;
    this.Move();
  }

  public void BeginRiding()
  {
    this.StopAllCoroutines();
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/mount", this.transform.position);
    this.ridingLoopSFX = this.animal.Type != InventoryItem.ITEM_TYPE.ANIMAL_SNAIL ? (this.animal.Type != InventoryItem.ITEM_TYPE.ANIMAL_SPIDER ? (this.animal.Type != InventoryItem.ITEM_TYPE.ANIMAL_TURTLE ? (this.animal.Type != InventoryItem.ITEM_TYPE.ANIMAL_CRAB ? AudioManager.Instance.CreateLoop("event:/dlc/animal/shared/riding_loop", this.gameObject, true) : AudioManager.Instance.CreateLoop("event:/dlc/animal/crab/riding_loop", this.gameObject, true)) : AudioManager.Instance.CreateLoop("event:/dlc/animal/turtle/riding_loop", this.gameObject, true)) : AudioManager.Instance.CreateLoop("event:/dlc/animal/spider/riding_loop", this.gameObject, true)) : AudioManager.Instance.CreateLoop("event:/dlc/animal/snail/riding_loop", this.gameObject, true);
    this.ClearPath();
    if (PlayerFarming.Location == FollowerLocation.Base)
      BaseGoopDoor.MainDoor.SetDoorUp();
    if (PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom)
      BaseGoopDoor.WoolhavenDoor.SetDoorUp();
    this.CurrentState = Interaction_Ranchable.State.Riding;
    this.PriorityWeight = float.MaxValue;
    this.ActivateDistance = 5f;
    this.OutlineTarget = this.emptyOutline;
    this.simpleSpineFlash.SetFacingType(SimpleSpineFlash.SetFacingMode.Ignore);
    this._playerFarming.AllowDodging = false;
    this.transform.parent = this._playerFarming.transform;
    this.transform.localPosition = Vector3.zero;
    this._playerFarming.Spine.transform.localPosition -= Vector3.forward * 0.2f;
    if ((bool) (UnityEngine.Object) this.ranch)
    {
      int num1 = this.ranch.IsOvercrowded ? 1 : 0;
      AnimalData.RemoveAnimal(this.animal, this.ranch.Brain.Data.Animals);
      int num2 = this.ranch.IsOvercrowded ? 1 : 0;
      if (num1 != num2)
        this.NotifyOvercrowded(this.ranch);
    }
    if (!DataManager.Instance.BreakingOutAnimals.Contains(this.animal))
      DataManager.Instance.BreakingOutAnimals.Add(this.animal);
    this.leashTimestamp = Time.time;
    this.makeHappyTimer = 0.0f;
    this.raceTimer = 0.0f;
    this.HasSecondaryInteraction = false;
    this.SecondaryInteractable = false;
    this.animalNameParent.transform.DOScale(0.0f, 0.5f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.animalNameParent.gameObject.SetActive(false)));
    this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Moving, "ride-animal");
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.Moving;
    this.playerFarming.state.LockStateChanges = true;
    this.playerFarming.playerController.OverrideForwardMovement = true;
    float num = Mathf.Clamp(this.animal.Speed, this.MAX_SPEED.x, this.MAX_SPEED.y);
    if (this.isStarving)
      num -= 0.5f;
    this._playerFarming.playerController.RunSpeed *= 1f + num;
    this.SetRidingSpeedSFXParam(this._playerFarming.playerController.RunSpeed);
    ShowHPBar component = this.GetComponent<ShowHPBar>();
    if ((bool) (UnityEngine.Object) component)
      component.Hide();
    this.HasChanged = true;
  }

  public void EndRiding(bool resetMovement = true)
  {
    this.racingProgressBar.Hide(true);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/dismount", this.transform.position);
    AudioManager.Instance.StopLoop(this.ridingLoopSFX);
    this.PriorityWeight = 1f;
    this.ActivateDistance = 1f;
    this.leash.positionCount = 0;
    this.OutlineTarget = (GameObject) null;
    this.simpleSpineFlash.SetFacingType(SimpleSpineFlash.SetFacingMode.Normal);
    this._playerFarming.AllowDodging = true;
    this.transform.parent = this.playerFarming.transform.parent;
    this.CurrentState = Interaction_Ranchable.State.BreakingOut;
    this.Interactable = false;
    this._playerFarming.playerController.RunSpeed = 5.5f;
    this.playerFarming.simpleSpineAnimator.ResetAnimationsToDefaults();
    this.playerFarming.state.LockStateChanges = false;
    this.playerFarming.playerController.OverrideForwardMovement = false;
    this._playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    this.ReservedByPlayer = false;
    this.leashTimestamp = -1f;
    if (resetMovement)
      this.moveTimer = 0.0f;
    this.raceTimer = 0.0f;
    this.animalNameParent.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    this.animalNameParent.gameObject.SetActive(!string.IsNullOrEmpty(this.animal.GivenName));
    this.CheckBreakingOut(true);
    this.HasChanged = true;
    BaseGoopDoor.DoorDown();
    if (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom)
      return;
    BaseGoopDoor.WoolhavenDoor.CheckWoolhavenDoor();
    if (!resetMovement)
      return;
    this.Move();
  }

  public void UpdateRiding()
  {
    if (this.racingProgressBar.IsVisible)
    {
      if ((double) this.raceTimer >= (double) this.raceTime)
        this.racingProgressBar.Hide();
      this.racingProgressBar.UpdateProgress(Mathf.Clamp01((float) (1.0 - (double) this.raceTimer / (double) this.raceTime)));
      if (this.spine.AnimationState.GetCurrent(0).Animation.Name.Contains("walk"))
        this.raceTimer += Time.deltaTime;
    }
    this.spine.Skeleton.ScaleX = (double) this._playerFarming.state.facingAngle <= 90.0 || (double) this._playerFarming.state.facingAngle >= 270.0 ? -1f : 1f;
    this.transform.localPosition = Vector3.zero;
    this.HasChanged = true;
    this._playerFarming.interactor.CurrentInteraction = (Interaction) this;
    switch (this._playerFarming.state.CURRENT_STATE)
    {
      case StateMachine.State.Moving:
      case StateMachine.State.Moving_Winter:
        if (!this.spine.AnimationState.GetCurrent(0).Animation.Name.Contains("walk"))
        {
          this.spine.AnimationState.SetAnimation(0, this.walk_anim, true);
          break;
        }
        goto default;
      default:
        switch (this._playerFarming.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
          case StateMachine.State.Idle_Winter:
            if (!this.spine.AnimationState.GetCurrent(0).Animation.Name.Contains("idle"))
            {
              this.spine.AnimationState.SetAnimation(0, this.idle_anim, true);
              break;
            }
            break;
        }
        break;
    }
    this.playerFarming.playerController.speed = this.playerFarming.playerController.RunSpeed;
    this.SetRidingSpeedSFXParam(this._playerFarming.playerController.speed);
  }

  public void UpdateLeash()
  {
    if (this.spine.AnimationState.GetCurrent(0).Animation.Name.Contains("walk"))
      this.makeHappyTimer += Time.deltaTime;
    if ((double) this.makeHappyTimer >= 10.0 && !this.animal.PlayerMadeHappyToday)
    {
      this.AddAdoration(50f);
      this.animal.PlayerMadeHappyToday = true;
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_small");
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/love_hearts", this.transform.position);
      this.interactionProgressBar.Hide();
    }
    if (!this.interactionProgressBar.IsVisible && !this.animal.PlayerMadeHappyToday)
      this.interactionProgressBar.Show();
    this.interactionProgressBar.UpdateProgress(Mathf.Min(1f, this.makeHappyTimer / 10f));
    float angle = Utils.GetAngle(this.transform.position, this._playerFarming.transform.position);
    this.spine.Skeleton.ScaleX = (double) angle <= 90.0 || (double) angle >= 270.0 ? -1f : 1f;
    float t = Vector3.Distance(this.transform.position, this._playerFarming.transform.position) - 0.7f;
    AnimationCurve curve = new AnimationCurve();
    for (int index = 0; index < this.slackCurve.keys.Length; ++index)
      curve.AddKey(this.slackCurve.keys[index].time, Mathf.Lerp(this.slackCurve.keys[index].value, this.rigidCurve.keys[index].value, t));
    GameObject handBone = this._playerFarming.HandBones[0];
    if ((double) Vector3.Distance(this._playerFarming.HandBones[1].transform.position, this.transform.position) < (double) Vector3.Distance(this._playerFarming.HandBones[0].transform.position, this.transform.position))
      handBone = this._playerFarming.HandBones[1];
    switch (this._playerFarming.state.CURRENT_STATE)
    {
      case StateMachine.State.Moving:
      case StateMachine.State.Moving_Winter:
        if (!this.spine.AnimationState.GetCurrent(0).Animation.Name.Contains("walk"))
        {
          this.spine.AnimationState.SetAnimation(0, this.walk_anim, true);
          break;
        }
        goto default;
      default:
        switch (this._playerFarming.state.CURRENT_STATE)
        {
          case StateMachine.State.Idle:
          case StateMachine.State.Idle_Winter:
            if (!this.spine.AnimationState.GetCurrent(0).Animation.Name.Contains("idle"))
            {
              this.spine.AnimationState.SetAnimation(0, this.idle_anim, true);
              break;
            }
            break;
        }
        break;
    }
    Vector3[] ropePoints = this.GetRopePoints(this.leashPosition.transform.position, handBone.transform.position, curve);
    this.leash.positionCount = ropePoints.Length;
    this.leash.SetPositions(ropePoints);
    this.HasChanged = true;
    this._playerFarming.interactor.CurrentInteraction = (Interaction) this;
  }

  public void StartFollowingPlayer()
  {
    this.StopAllCoroutines();
    this.ClearPath();
    this.CurrentState = Interaction_Ranchable.State.FollowPlayer;
    this.PriorityWeight = 0.0f;
    this.ActivateDistance = 1f;
    if ((UnityEngine.Object) this.followedPlayer == (UnityEngine.Object) null)
      this.followedPlayer = this._playerFarming;
    this.moveTimer = 0.0f;
    this.unitObject.SpeedMultiplier = 4f;
    this.unitObject.maxSpeed = this.originalMaxSpeed * 4f;
    if ((UnityEngine.Object) this.followedPlayer != (UnityEngine.Object) null)
    {
      int index = PlayerFarming.players.IndexOf(this.followedPlayer);
      if (DataManager.Instance.FollowingPlayerAnimals[index] != null && DataManager.Instance.FollowingPlayerAnimals[index] != this.animal)
      {
        foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
        {
          if (ranchable.animal == DataManager.Instance.FollowingPlayerAnimals[index])
          {
            ranchable.EndFollowingPlayer();
            break;
          }
        }
      }
      DataManager.Instance.FollowingPlayerAnimals[index] = this.animal;
    }
    if ((UnityEngine.Object) this.ranch != (UnityEngine.Object) null)
      AnimalData.RemoveAnimal(this.animal, this.ranch.Brain.Data.Animals);
    this.HasChanged = true;
    this.ReservedByPlayer = true;
    this.UpdateSkin();
  }

  public void AbortAnyHutchMating()
  {
    Debug.Log((object) $"AbortAnyHutchMatingRituals: Attempting to abort any funny hutch business... {{CurrentState: {this.Animal.State}, TargetHutch: {this.targetHutch?.Data.ID.ToString() ?? "None"}}}");
    Structures_RanchHutch hutch = this.targetHutch ?? this.GetHutch();
    if (hutch == null)
      Debug.Log((object) "No hutch was set no mating to cancel");
    else
      this.ranch.Brain.AbortMating(hutch);
  }

  public void EndFollowingPlayer()
  {
    this.PriorityWeight = 1f;
    this.ActivateDistance = 1f;
    this.unitObject.SpeedMultiplier = 1f;
    this.unitObject.maxSpeed = this.originalMaxSpeed;
    this.CurrentState = Interaction_Ranchable.State.BreakingOut;
    this.ReservedByPlayer = false;
    DataManager.Instance.FollowingPlayerAnimals[PlayerFarming.players.IndexOf(this.followedPlayer)] = (StructuresData.Ranchable_Animal) null;
    this.followedPlayer = (PlayerFarming) null;
    this.CurrentState = Interaction_Ranchable.State.BreakingOut;
    this.CheckBreakingOut();
    this.UpdateSkin();
  }

  public void UpdateMovingToAttack()
  {
    if ((UnityEngine.Object) this.targetAnimal == (UnityEngine.Object) null || this.targetAnimal.CurrentState == Interaction_Ranchable.State.Dead || (UnityEngine.Object) this.targetAnimal.ranch != (UnityEngine.Object) this.ranch || this.targetAnimal.CurrentState == Interaction_Ranchable.State.Animating || this.targetAnimal.ReservedByPlayer)
      this.ResetAnimalState();
    else if ((double) Vector3.Distance(this.transform.position, this.targetAnimal.transform.position) < 1.0)
    {
      this.ClearPath();
      this.animal.State = Interaction_Ranchable.State.Animating;
      int AnimationDuration = 3;
      this.spine.AnimationState.SetAnimation(0, "attack-" + this.GetSharedAnimationAnimalName(), false);
      this.spine.AnimationState.AddAnimation(0, this.idle_anim, true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/attack", this.transform.position);
      this.unitObject.state.facingAngle = Utils.GetAngle(this.transform.position, this.targetAnimal.transform.position);
      this.StartCoroutine((IEnumerator) this.WaitForSeconds(0.266666681f, (System.Action) (() =>
      {
        if (this.CurrentState == Interaction_Ranchable.State.Dead)
          return;
        if ((UnityEngine.Object) this.targetAnimal != (UnityEngine.Object) null && this.targetAnimal.CurrentState != Interaction_Ranchable.State.Dead && this.CurrentState != Interaction_Ranchable.State.Dead && this.targetAnimal.CurrentState != Interaction_Ranchable.State.Animating && !this.targetAnimal.ReservedByPlayer)
        {
          this.targetAnimal.Injure();
          this.targetAnimal = (Interaction_Ranchable) null;
          this.LastAttackTime = Time.time;
        }
        else
          Debug.Log((object) "Prevented");
        this.StartCoroutine((IEnumerator) this.WaitForSeconds((float) AnimationDuration - 0.266666681f, (System.Action) (() => this.ResetAnimalState())));
      })));
    }
    else
    {
      if ((double) Time.time <= (double) this.moveTimer)
        return;
      this.unitObject.SpeedMultiplier = 4f;
      this.unitObject.maxSpeed = this.originalMaxSpeed * 4f;
      this.spine.AnimationState.SetAnimation(0, this.walk_anim, true);
      this.moveTimer = Time.time + 0.2f;
      this.unitObject.givePath(this.targetAnimal.transform.position);
    }
  }

  public void UpdateFollowPlayer()
  {
    if ((UnityEngine.Object) this.followedPlayer == (UnityEngine.Object) null)
    {
      if (PlayerFarming.players.Count == 0)
        return;
      for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
      {
        if (DataManager.Instance.FollowingPlayerAnimals[index] == this.animal && PlayerFarming.players.Count > index)
          this.followedPlayer = PlayerFarming.players[index];
      }
      if (!((UnityEngine.Object) this.followedPlayer == (UnityEngine.Object) null))
        return;
      this.ResetAnimalState();
    }
    else
    {
      if ((double) Time.time <= (double) this.moveTimer || this.targetTrough != null || !((UnityEngine.Object) this.animalInteractionWheel == (UnityEngine.Object) null))
        return;
      this.unitObject.SpeedMultiplier = 4f;
      this.unitObject.maxSpeed = this.originalMaxSpeed * 4f;
      this.spine.AnimationState.SetAnimation(0, this.walk_anim, true);
      this.moveTimer = Time.time + 1f;
      Vector3 targetLocation = this.followedPlayer.transform.position - (this.followedPlayer.transform.position - this.transform.position).normalized * 1.5f;
      if ((double) targetLocation.y > (double) PlacementRegion.Y_Constraints.y)
        this.unitObject.givePath(Vector3.zero, forceAStar: true);
      else
        this.unitObject.givePath(targetLocation);
    }
  }

  public void CleanupOtherReferences()
  {
    this.AbortAnyHutchMating();
    this.RemoveFollowingPlayer();
  }

  public void RemoveFollowingPlayer()
  {
    for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
    {
      if (DataManager.Instance.FollowingPlayerAnimals[index] == this.animal)
        DataManager.Instance.FollowingPlayerAnimals[index] = (StructuresData.Ranchable_Animal) null;
    }
  }

  public Vector3[] GetRopePoints(Vector3 from, Vector3 to, AnimationCurve curve)
  {
    Vector3[] ropePoints = new Vector3[curve.keys.Length];
    for (int index = 0; index < ropePoints.Length; ++index)
    {
      float num = (float) index / (float) (ropePoints.Length - 1);
      Vector3 vector3 = Vector3.Lerp(from, to, num);
      vector3.z += 1f - curve.Evaluate(num);
      ropePoints[index] = vector3;
    }
    return ropePoints;
  }

  public void Slaughter(bool dropLoot = true)
  {
    if (dropLoot)
    {
      int meatCount = this.animal.GetMeatCount();
      for (int index = 0; index < meatCount; ++index)
      {
        if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
        {
          List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.FISH,
            InventoryItem.ITEM_TYPE.FISH_BIG,
            InventoryItem.ITEM_TYPE.FISH_SMALL
          };
          InventoryItem.Spawn(itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)], 1, this.transform.position);
        }
        else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
        {
          List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>()
          {
            InventoryItem.ITEM_TYPE.PUMPKIN,
            InventoryItem.ITEM_TYPE.CAULIFLOWER,
            InventoryItem.ITEM_TYPE.BEETROOT
          };
          InventoryItem.Spawn(itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)], 1, this.transform.position);
        }
        else
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, 1, this.transform.position);
      }
      for (int index = 0; index < Mathf.RoundToInt((float) meatCount / 2f); ++index)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 1, this.transform.position);
    }
    this.CleanupOtherReferences();
    DataManager.Instance.BreakingOutAnimals.Remove(this.animal);
    this.ranch.Brain.RemoveAnimal(this.animal);
    DataManager.Instance.DeadAnimalsTemporaryList.Add(this.animal);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Die(bool showStinky = true)
  {
    if (this.CurrentState == Interaction_Ranchable.State.Dead)
      return;
    this.PlayDieVO();
    this.StopAllCoroutines();
    this.ClearPath();
    this.animal.State = Interaction_Ranchable.State.Dead;
    this.animal.Ailment = Interaction_Ranchable.Ailment.None;
    Interaction_Ranchable.Ranchables.Remove(this);
    Interaction_Ranchable.DeadRanchables.Add(this);
    this.StartCoroutine((IEnumerator) this.DieIE(showStinky));
  }

  public void SetDead()
  {
    this.spine.AnimationState.SetAnimation(0, "dead-" + this.GetAnimationAnimalName(), true);
    this.stinky.gameObject.SetActive(true);
    if (this.animal.CauseOfDeath == Interaction_Ranchable.CauseOfDeath.DiedFromOldAge)
      return;
    this.spine.gameObject.SetActive(false);
    this.deadBody.gameObject.SetActive(true);
    this.OutlineTarget = this.deadBody;
  }

  public void Injure()
  {
    Debug.Log((object) "Injure()!!");
    if (this.CurrentState == Interaction_Ranchable.State.Dead)
      return;
    this.StopAllCoroutines();
    this.ClearPath();
    this.animal.Ailment = Interaction_Ranchable.Ailment.Injured;
    this.UpdateSkin();
    AudioManager.Instance.StopLoop(this.eatingLoopSFX);
    this.targetAnimal = (Interaction_Ranchable) null;
    if ((double) this.animal.Injured < 0.0)
      this.animal.Injured = 15f;
    this.StartCoroutine((IEnumerator) this.InjureIE());
  }

  public IEnumerator InjureIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_Ranchable interactionRanchable = this;
    Interaction_Ranchable a;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionRanchable.spine.AnimationState.SetAnimation(0, interactionRanchable.idle_anim, true);
      a.CurrentState = Interaction_Ranchable.State.Default;
      interactionRanchable.moveTimer = 0.0f;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionRanchable.transform.position);
    BiomeConstants.Instance.EmitHitVFX(interactionRanchable.transform.position, Quaternion.identity.z, "HitFX_Weak");
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/injure", interactionRanchable.transform.position);
    interactionRanchable.PlayInjuredVO();
    a = Interaction_Ranch.GetAnimal(interactionRanchable.animal);
    a.CurrentState = Interaction_Ranchable.State.Animating;
    interactionRanchable.spine.AnimationState.SetAnimation(0, "die-" + interactionRanchable.GetAnimationAnimalName(), false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new UnityEngine.WaitForSeconds(0.8333333f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DieIE(bool showStinky = true)
  {
    Interaction_Ranchable interactionRanchable = this;
    AudioManager.Instance.StopLoop(interactionRanchable.feralLoopSFX, STOP_MODE.IMMEDIATE);
    interactionRanchable.spine.AnimationState.SetAnimation(0, "die-" + interactionRanchable.GetSharedAnimationAnimalName(), false);
    yield return (object) new UnityEngine.WaitForSeconds(2.66666675f);
    interactionRanchable.spine.AnimationState.SetAnimation(0, "dead-" + interactionRanchable.GetAnimationAnimalName(), true);
    interactionRanchable.spine.AnimationState.AddAnimation(0, "dead-" + interactionRanchable.GetAnimationAnimalName(), true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/death_poof", interactionRanchable.transform.position);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionRanchable.transform.position);
    interactionRanchable.stinky.gameObject.SetActive(showStinky);
    if (interactionRanchable.animal.CauseOfDeath != Interaction_Ranchable.CauseOfDeath.DiedFromOldAge)
    {
      interactionRanchable.spine.gameObject.SetActive(false);
      interactionRanchable.deadBody.gameObject.SetActive(true);
      interactionRanchable.OutlineTarget = interactionRanchable.deadBody;
    }
  }

  public IEnumerator SlaughterIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    interactionRanchable.AbortAnyHutchMating();
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/butcher_swing", interactionRanchable.gameObject);
    interactionRanchable._playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable._playerFarming.state.transform.position, interactionRanchable.transform.position);
    interactionRanchable._playerFarming.CustomAnimation("actions/butcher-animal", true);
    yield return (object) new UnityEngine.WaitForSeconds(0.6f);
    interactionRanchable.spine.AnimationState.SetAnimation(0, "die-" + interactionRanchable.GetAnimationAnimalName(), true);
    interactionRanchable.PlayDieVO();
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", interactionRanchable.gameObject);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.ReservedByPlayer = false;
    interactionRanchable.Slaughter();
  }

  public IEnumerator RemoveAnimal(bool spawnBones = true)
  {
    Interaction_Ranchable interactionRanchable = this;
    int bonesToSpawn;
    int i1;
    if (spawnBones)
    {
      bonesToSpawn = Structures_Ranch.GetAnimalGrowthState(interactionRanchable.animal);
      for (i1 = 0; i1 < bonesToSpawn; ++i1)
      {
        yield return (object) new UnityEngine.WaitForSeconds(0.1f);
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 1, interactionRanchable.transform.position);
      }
    }
    if (interactionRanchable.animal.CauseOfDeath == Interaction_Ranchable.CauseOfDeath.DiedFromOldAge)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/cleanup_dead", interactionRanchable.transform.position);
      bonesToSpawn = interactionRanchable.animal.GetMeatCount();
      int i2;
      for (i2 = 0; i2 < bonesToSpawn; ++i2)
      {
        yield return (object) new UnityEngine.WaitForSeconds(0.01f);
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, 1, interactionRanchable.transform.position);
      }
      i1 = interactionRanchable.animal.GetLootCount();
      InventoryItem workLoot = Interaction_Ranchable.GetWorkLoot(interactionRanchable.animal)[0];
      for (i2 = 0; i2 < i1; ++i2)
      {
        yield return (object) new UnityEngine.WaitForSeconds(0.1f);
        InventoryItem.Spawn((InventoryItem.ITEM_TYPE) workLoot.type, 1, interactionRanchable.transform.position);
      }
      workLoot = (InventoryItem) null;
    }
    DataManager.Instance.BreakingOutAnimals.Remove(interactionRanchable.animal);
    interactionRanchable.CleanupOtherReferences();
    if ((UnityEngine.Object) interactionRanchable.ranch != (UnityEngine.Object) null)
    {
      int num1 = interactionRanchable.ranch.IsOvercrowded ? 1 : 0;
      interactionRanchable.ranch.Brain.RemoveAnimal(interactionRanchable.animal);
      int num2 = interactionRanchable.ranch.IsOvercrowded ? 1 : 0;
      if (num1 != num2)
        interactionRanchable.NotifyOvercrowded(interactionRanchable.ranch);
    }
    DataManager.Instance.DeadAnimalsTemporaryList.Add(interactionRanchable.animal);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionRanchable.gameObject);
  }

  public IEnumerator RemoveDeadBodyIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    yield return (object) new WaitForEndOfFrame();
    interactionRanchable._playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    interactionRanchable._playerFarming.CustomAnimation("actions/harvest_meat", true);
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 6f);
    AudioManager.Instance.PlayOneShot(interactionRanchable.animal.CauseOfDeath == Interaction_Ranchable.CauseOfDeath.DiedFromStarvation ? "event:/dlc/animal/shared/cleanup_start_skeleton" : "event:/dlc/animal/shared/cleanup_start", interactionRanchable._playerFarming.transform.position);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    interactionRanchable._playerFarming.playerChoreXPBarController.AddChoreXP(interactionRanchable.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.enabled = false;
    interactionRanchable.isBeingFed = true;
    interactionRanchable.Interactable = false;
    interactionRanchable.HasChanged = true;
    interactionRanchable.StartCoroutine((IEnumerator) interactionRanchable.RemoveAnimal());
  }

  public void Sacrifice()
  {
    int num = Structures_Ranch.GetAnimalGrowthState(this.animal) * 3;
    for (int index = 0; index < num; ++index)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 1, this.transform.position);
    this.ranch.Brain.RemoveAnimal(this.animal);
    DataManager.Instance.DeadAnimalsTemporaryList.Add(this.animal);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public IEnumerator SacrificeIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/player/harvest_meat", interactionRanchable.gameObject);
    interactionRanchable._playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    interactionRanchable._playerFarming.CustomAnimation("cast-spell2", false);
    interactionRanchable.spine.AnimationState.SetAnimation(0, "die-" + interactionRanchable.GetAnimationAnimalName(), true);
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 6f);
    yield return (object) new UnityEngine.WaitForSeconds(0.66f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", interactionRanchable.gameObject);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.ReservedByPlayer = false;
    interactionRanchable.Sacrifice();
  }

  public IEnumerator PetIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    if (!interactionRanchable.animal.PetToday)
      interactionRanchable.AddAdoration(50f);
    GameManager.GetInstance().OnConversationNew();
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    interactionRanchable.animal.PetToday = true;
    yield return (object) new WaitForEndOfFrame();
    interactionRanchable._playerFarming.CustomAnimation("pet-dog", false);
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    interactionRanchable.PlayPetVO();
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 6f);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    BiomeConstants.Instance.EmitHeartPickUpVFX(interactionRanchable.transform.position, 0.0f, "red", "burst_small");
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/love_hearts", interactionRanchable.transform.position);
    yield return (object) new UnityEngine.WaitForSeconds(0.8f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.ReservedByPlayer = false;
    interactionRanchable.Clean();
  }

  public IEnumerator CalmIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    GameManager.GetInstance().OnConversationNew();
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    interactionRanchable.ClearPath();
    bool Waiting = true;
    int num1 = (double) interactionRanchable.transform.position.x < (double) interactionRanchable._playerFarming.transform.position.x ? -1 : 1;
    interactionRanchable._playerFarming.GoToAndStop(interactionRanchable.transform.position + new Vector3(-1f * (float) num1, -0.2f), interactionRanchable.gameObject, GoToCallback: (System.Action) (() => Waiting = false));
    while (Waiting)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    interactionRanchable._playerFarming.CustomAnimation("actions/calm-animal", false);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/calm", interactionRanchable.transform.position);
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 6f);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    interactionRanchable._playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    float num2 = 5f;
    float num3 = interactionRanchable.animal.FeralCalming - num2;
    float num4 = interactionRanchable.RanchableFeralWarning.PlaySequence(num3 / 10f, 0.3f);
    interactionRanchable.animal.FeralCalming -= num2;
    yield return (object) new UnityEngine.WaitForSeconds(num4 - 0.5f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionRanchable.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(interactionRanchable.transform.position, 0.0f, "black", "burst_big");
    yield return (object) new UnityEngine.WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.ReservedByPlayer = false;
    interactionRanchable.animal.AilmentGameTime = TimeManager.TotalElapsedGameTime;
    if ((double) interactionRanchable.animal.FeralCalming > 0.0)
    {
      interactionRanchable.ResetAnimalState();
    }
    else
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionRanchable.transform.position);
      interactionRanchable.Calm();
    }
  }

  public void Calm()
  {
    if (this.animal.Ailment == Interaction_Ranchable.Ailment.Feral)
      AudioManager.Instance.StopLoop(this.feralLoopSFX);
    this.animal.Ailment = Interaction_Ranchable.Ailment.None;
    this.UpdateSkin();
    this.ResetAnimalState();
  }

  public IEnumerator AscendIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 4f);
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    interactionRanchable.BeingAscended = true;
    if (interactionRanchable.bubbleRoutine != null)
    {
      interactionRanchable.StopCoroutine(interactionRanchable.bubbleRoutine);
      interactionRanchable.bubble.Close();
      interactionRanchable.bubbleRoutine = (Coroutine) null;
    }
    if ((UnityEngine.Object) interactionRanchable.animalNameParent != (UnityEngine.Object) null)
      interactionRanchable.animalNameParent.gameObject.SetActive(false);
    bool Waiting = true;
    int num = (double) interactionRanchable.transform.position.x < (double) interactionRanchable._playerFarming.transform.position.x ? -1 : 1;
    interactionRanchable._playerFarming.GoToAndStop(interactionRanchable.transform.position + new Vector3(-1.5f * (float) num, -0.2f), interactionRanchable.gameObject, GoToCallback: (System.Action) (() => Waiting = false));
    while (Waiting)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    interactionRanchable._playerFarming.CustomAnimation("ascend-animal", false);
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    yield return (object) new UnityEngine.WaitForSeconds(0.5f);
    interactionRanchable._playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    string animationName = "ascend-" + interactionRanchable.GetAnimationAnimalName();
    interactionRanchable.spine.AnimationState.SetAnimation(0, animationName, false);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/ascend", interactionRanchable.transform.position);
    float animDuration = 4.33333349f;
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    yield return (object) new UnityEngine.WaitForSeconds(1.5f);
    List<InventoryItem> meatLoot = Interaction_Ranchable.GetMeatLoot(interactionRanchable.animal);
    foreach (InventoryItem inventoryItem in meatLoot)
      inventoryItem.quantity = Mathf.RoundToInt((float) inventoryItem.quantity);
    Mathf.Min(meatLoot[0].quantity, 20);
    yield return (object) new UnityEngine.WaitForSeconds(animDuration - 1.5f - 0.0f);
    BiomeConstants.Instance.ChromaticAbberationTween(0.5f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      interactionRanchable.spine.gameObject.SetActive(false);
      yield return (object) null;
    }
    int quantity = 10;
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/cleanup_dead", interactionRanchable.transform.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, quantity, interactionRanchable.transform.position + new Vector3(0.0f, 0.0f, -2f));
    foreach (InventoryItem inventoryItem in meatLoot)
      Inventory.AddItem(inventoryItem.type, Mathf.Max(0, inventoryItem.quantity - quantity));
    List<InventoryItem> workLoot = Interaction_Ranchable.GetWorkLoot(interactionRanchable.animal);
    bool flag = interactionRanchable.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW;
    foreach (InventoryItem inventoryItem in workLoot)
      InventoryItem.Spawn((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity * Mathf.RoundToInt(AnimalInteractionModel.AscendItem.MULTIPLIER_RESOURCES), interactionRanchable.transform.position + new Vector3(0.0f, 0.0f, -2f));
    if (flag)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MILK, 5, interactionRanchable.transform.position + new Vector3(0.0f, 0.0f, -2f));
    yield return (object) null;
    interactionRanchable.ReservedByPlayer = false;
    DataManager.Instance.BreakingOutAnimals.Remove(interactionRanchable.animal);
    interactionRanchable.CleanupOtherReferences();
    if ((UnityEngine.Object) interactionRanchable.ranch != (UnityEngine.Object) null)
      interactionRanchable.ranch.Brain.RemoveAnimal(interactionRanchable.animal);
    DataManager.Instance.DeadAnimalsTemporaryList.Add(interactionRanchable.animal);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionRanchable.gameObject);
  }

  public IEnumerator HealIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    GameManager.GetInstance().OnConversationNew();
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    bool Waiting = true;
    int num1 = (double) interactionRanchable.transform.position.x < (double) interactionRanchable._playerFarming.transform.position.x ? -1 : 1;
    interactionRanchable._playerFarming.GoToAndStop(interactionRanchable.transform.position + new Vector3(-1f * (float) num1, -0.2f), interactionRanchable.gameObject, GoToCallback: (System.Action) (() => Waiting = false));
    while (Waiting)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/heal", interactionRanchable.transform.position);
    interactionRanchable._playerFarming.CustomAnimation("Actions/Calm-animal", false);
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    for (int index = 0; index < 3; ++index)
      ResourceCustomTarget.Create(interactionRanchable.gameObject, interactionRanchable.playerFarming.transform.position, interactionRanchable.FlowerType, (System.Action) null);
    Inventory.ChangeItemQuantity(interactionRanchable.FlowerType, -3);
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 6f);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    interactionRanchable._playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    float num2 = interactionRanchable.RanchableInjuredWarning.PlaySequence(0.0f, 0.3f);
    interactionRanchable.animal.Injured = -1f;
    interactionRanchable.LastHealedTime = Time.time;
    yield return (object) new UnityEngine.WaitForSeconds(num2 - 1.2f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionRanchable.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(interactionRanchable.transform.position, 0.0f, "black", "burst_big");
    yield return (object) new UnityEngine.WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.ReservedByPlayer = false;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionRanchable.transform.position);
    interactionRanchable.Calm();
  }

  public IEnumerator CleanIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    if (interactionRanchable.isStarving)
      interactionRanchable.animal.Satiation += 10f;
    GameManager.GetInstance().OnConversationNew();
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/clean", interactionRanchable.transform.position);
    yield return (object) new WaitForEndOfFrame();
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    interactionRanchable._playerFarming.CustomAnimation("actions/cleaning-animal", true);
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 5f);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.animal.Ailment = Interaction_Ranchable.Ailment.None;
    interactionRanchable.UpdateSkin();
    interactionRanchable.stinky.gameObject.SetActive(false);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    interactionRanchable.ReservedByPlayer = false;
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalNotStinky", interactionRanchable.animal.GetName());
    interactionRanchable.Clean();
  }

  public void Clean(bool playerCleaned = true)
  {
    if (!playerCleaned)
    {
      this.AddAdoration(20f);
      this.animal.Ailment = Interaction_Ranchable.Ailment.None;
      this.UpdateSkin();
      this.stinky.gameObject.SetActive(false);
    }
    this.animal.TimeSinceLastWash = TimeManager.TotalElapsedGameTime + 4800f + (float) UnityEngine.Random.Range(-1200, 1200);
    this.ResetAnimalState(playerCleaned);
  }

  public void Work(bool dropLoot = true)
  {
    if (dropLoot)
    {
      List<InventoryItem> workLoot = Interaction_Ranchable.GetWorkLoot(this.animal);
      for (int index1 = 0; index1 < workLoot.Count; ++index1)
      {
        int quantity = workLoot[index1].quantity;
        for (int index2 = 0; index2 < quantity; ++index2)
          InventoryItem.Spawn((InventoryItem.ITEM_TYPE) workLoot[index1].type, 1, this.transform.position);
      }
    }
    this.animal.WorkedReady = false;
    this.animal.WorkedToday = true;
    this.HasChanged = true;
    this.ResetAnimalState();
    this.UpdateSkin();
  }

  public IEnumerator WorkIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    interactionRanchable.ClearPath();
    interactionRanchable.moveTimer = 8f;
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    yield return (object) new WaitForEndOfFrame();
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    interactionRanchable._playerFarming.CustomAnimation("actions/shear", true);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/shear", interactionRanchable.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 6f);
    yield return (object) new UnityEngine.WaitForSeconds(0.66f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.ReservedByPlayer = false;
    interactionRanchable.Work();
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.SheerAnimal) && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.ReturnWoolToRancher))
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SheerAnimal);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FeedAnimal);
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/RanchingAnimal", Objectives.CustomQuestTypes.ReturnWoolToRancher), true, true);
      if ((bool) (UnityEngine.Object) LambTownController.Instance)
      {
        LambTownController.Instance.SetSaleAnimal();
        MonoSingleton<UIManager>.Instance.ForceBlockPause = false;
      }
    }
  }

  public IEnumerator MilkAnimalIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    interactionRanchable.ClearPath();
    interactionRanchable.moveTimer = 8f;
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    yield return (object) new WaitForEndOfFrame();
    interactionRanchable._playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/cow/milk", interactionRanchable.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 6f);
    yield return (object) new UnityEngine.WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.ReservedByPlayer = false;
    interactionRanchable.MilkAnimal();
  }

  public void MilkAnimal(bool dropLoot = true)
  {
    if (dropLoot)
    {
      List<InventoryItem> inventoryItemList = new List<InventoryItem>()
      {
        new InventoryItem(InventoryItem.ITEM_TYPE.MILK, 1)
      };
      for (int index1 = 0; index1 < inventoryItemList.Count; ++index1)
      {
        int quantity = inventoryItemList[index1].quantity;
        for (int index2 = 0; index2 < quantity; ++index2)
          InventoryItem.Spawn((InventoryItem.ITEM_TYPE) inventoryItemList[index1].type, 1, this.transform.position);
      }
    }
    this.animal.MilkedReady = false;
    this.animal.MilkedToday = true;
    this.HasChanged = true;
    this.ResetAnimalState();
    this.UpdateSkin();
  }

  public static List<InventoryItem> GetMeatLoot(StructuresData.Ranchable_Animal animal)
  {
    InventoryItem.ITEM_TYPE Type = InventoryItem.ITEM_TYPE.MEAT_MORSEL;
    int meatCount = animal.GetMeatCount();
    if (meatCount > 2)
      Type = InventoryItem.ITEM_TYPE.MEAT;
    if (animal.Ailment == Interaction_Ranchable.Ailment.Feral)
      Type = InventoryItem.ITEM_TYPE.FOLLOWER_MEAT;
    return new List<InventoryItem>()
    {
      new InventoryItem(Type, meatCount)
    };
  }

  public static InventoryItem GetNecklaceLoot(StructuresData.Ranchable_Animal animal)
  {
    UnityEngine.Random.InitState(animal.ID);
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>((IEnumerable<InventoryItem.ITEM_TYPE>) InventoryItem.ItemsThatCanBeGivenToFollower);
    itemTypeList.Remove(InventoryItem.ITEM_TYPE.GIFT_MEDIUM);
    itemTypeList.Remove(InventoryItem.ITEM_TYPE.GIFT_SMALL);
    itemTypeList.Remove(InventoryItem.ITEM_TYPE.Necklace_Bell);
    itemTypeList.Remove(InventoryItem.ITEM_TYPE.Necklace_Light);
    itemTypeList.Remove(InventoryItem.ITEM_TYPE.Necklace_Dark);
    return new InventoryItem(itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)]);
  }

  public static List<InventoryItem> GetWorkLoot(StructuresData.Ranchable_Animal animal)
  {
    int lootCount = animal.GetLootCount();
    switch (animal.Type)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        return new List<InventoryItem>()
        {
          new InventoryItem(InventoryItem.ITEM_TYPE.WOOL, lootCount)
        };
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        return new List<InventoryItem>()
        {
          new InventoryItem(InventoryItem.ITEM_TYPE.MAGMA_STONE, lootCount)
        };
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        return new List<InventoryItem>()
        {
          new InventoryItem(InventoryItem.ITEM_TYPE.CRYSTAL, lootCount)
        };
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        return new List<InventoryItem>()
        {
          new InventoryItem(InventoryItem.ITEM_TYPE.SPIDER_WEB, lootCount)
        };
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
        itemTypeList.Add(InventoryItem.ITEM_TYPE.SEED);
        if (DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2))
          itemTypeList.Add(InventoryItem.ITEM_TYPE.SEED_PUMPKIN);
        if (DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3))
          itemTypeList.Add(InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER);
        if (DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_4))
          itemTypeList.Add(InventoryItem.ITEM_TYPE.SEED_BEETROOT);
        if (DataManager.Instance.PleasureEnabled)
        {
          itemTypeList.Add(InventoryItem.ITEM_TYPE.SEED_HOPS);
          itemTypeList.Add(InventoryItem.ITEM_TYPE.SEED_GRAPES);
        }
        if (DataManager.Instance.TailorEnabled)
          itemTypeList.Add(InventoryItem.ITEM_TYPE.SEED_COTTON);
        if (SeasonsManager.Active)
          itemTypeList.Add(InventoryItem.ITEM_TYPE.SEED_CHILLI);
        List<InventoryItem> workLoot = new List<InventoryItem>();
        for (int index = 0; index < lootCount; ++index)
          workLoot.Add(new InventoryItem(itemTypeList[UnityEngine.Random.Range(0, itemTypeList.Count)], 1));
        return workLoot;
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        return new List<InventoryItem>()
        {
          new InventoryItem(InventoryItem.ITEM_TYPE.WOOL, lootCount)
        };
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        return new List<InventoryItem>()
        {
          new InventoryItem(InventoryItem.ITEM_TYPE.WOOL, lootCount)
        };
      default:
        return new List<InventoryItem>()
        {
          new InventoryItem(InventoryItem.ITEM_TYPE.WOOL, lootCount)
        };
    }
  }

  public static bool IsWoolWorkLoot(InventoryItem.ITEM_TYPE animalType)
  {
    return animalType == InventoryItem.ITEM_TYPE.ANIMAL_GOAT || animalType == InventoryItem.ITEM_TYPE.ANIMAL_COW || animalType == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA;
  }

  public void DebugMakeFeral() => this.Feed(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT);

  public void Feed(InventoryItem.ITEM_TYPE food)
  {
    this.animal.EatenToday = true;
    this.moveTimer = Time.time + 5f;
    this.animal.Satiation += (float) Interaction_Ranchable.FoodSatiation(food);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/feed", this.transform.position);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.FeedAnimal);
    ObjectiveManager.CompleteFeedAnimalObjective(food, this.animal.ID);
    if (food == InventoryItem.ITEM_TYPE.POOP)
    {
      if (this.animal.Ailment == Interaction_Ranchable.Ailment.None && (double) UnityEngine.Random.value < 0.25 && !this.isStarving)
      {
        this.animal.Ailment = Interaction_Ranchable.Ailment.Stinky;
        this.animal.AilmentGameTime = TimeManager.TotalElapsedGameTime;
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalStinky", this.animal.GetName());
        this.UpdateSkin();
      }
    }
    else if (food == InventoryItem.ITEM_TYPE.FOLLOWER_MEAT)
    {
      if (this.animal.Ailment == Interaction_Ranchable.Ailment.None)
      {
        AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/feral_start", this.transform.position);
        this.feralLoopSFX = AudioManager.Instance.CreateLoop("event:/dlc/animal/shared/feral_loop", this.gameObject, true);
        this.animal.Ailment = Interaction_Ranchable.Ailment.Feral;
        this.animal.AilmentGameTime = 0.0f;
        this.animal.FeralCalming = 10f;
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchAnimalFeral", this.animal.GetName());
        this.UpdateSkin();
        BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
        if (this.TryScheduleRanchFight(this.ranch))
          this.moveTimer = Time.time - 0.01f;
        this.Close();
        return;
      }
    }
    else
      this.UpdateHappyState();
    Inventory.ChangeItemQuantity(food, -1);
    ResourceCustomTarget.Create(this.gameObject, this._playerFarming.transform.position, food, (System.Action) (() =>
    {
      this.isBeingFed = true;
      if (food == this.animal.FavouriteFood)
      {
        if (this.animal.IsFavouriteFoodRevealed)
          this.Close();
        this.EatFavouriteFood(food, this.animal.IsFavouriteFoodRevealed);
      }
      else
      {
        this.Eat(food);
        this.Close();
      }
    }));
  }

  public void Poop()
  {
    this.moveTimer = Time.time + 5f;
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(1f, (System.Action) (() =>
    {
      if (!((UnityEngine.Object) this.ranch != (UnityEngine.Object) null) || this.ranch.Brain == null)
        return;
      this.ranch.Brain.AnimalPooped(this.animal, this.transform.position, new System.Action<GameObject>(this.LerpPoop));
    })));
    this.spine.AnimationState.SetAnimation(0, "poop", false);
    this.spine.AnimationState.AddAnimation(0, this.idle_anim, true, 0.0f);
  }

  public void LerpPoop(GameObject poop)
  {
    Vector3 position = poop.transform.position;
    poop.transform.position = this.transform.position;
    poop.transform.localScale = Vector3.zero;
    poop.transform.DOMove(position, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    poop.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public void Eat(InventoryItem.ITEM_TYPE food)
  {
    this.eatingLoopSFX = AudioManager.Instance.CreateLoop("event:/dlc/animal/shared/eating_loop", this.gameObject, true);
    this.CurrentState = Interaction_Ranchable.State.Animating;
    this.moveTimer = Time.time + 5f;
    if (InventoryItem.IsFish(food) || InventoryItem.IsMeat(food))
      this.spine.AnimationState.SetAnimation(0, "eat_meat", true);
    else if (food == InventoryItem.ITEM_TYPE.POOP)
      this.spine.AnimationState.SetAnimation(0, "eat_poop", true);
    else
      this.spine.AnimationState.SetAnimation(0, "eat_grass", true);
    this.postFeedRoutine = this.StartCoroutine((IEnumerator) this.WaitForSeconds(5f, (System.Action) (() =>
    {
      this.postFeedRoutine = (Coroutine) null;
      AudioManager.Instance.StopLoop(this.eatingLoopSFX);
      this.UpdateSkin();
      this.ResetAnimalState();
    })));
    DOVirtual.DelayedCall(1.5f, (TweenCallback) (() => this.isBeingFed = false));
  }

  public void EatFavouriteFood(InventoryItem.ITEM_TYPE food, bool discovered = true)
  {
    this.eatingLoopSFX = AudioManager.Instance.CreateLoop("event:/dlc/animal/shared/eating_loop", this.gameObject, true);
    this.CurrentState = Interaction_Ranchable.State.Animating;
    this.moveTimer = Time.time + 5f;
    if (InventoryItem.IsFish(food) || InventoryItem.IsMeat(food))
      this.spine.AnimationState.SetAnimation(0, "eat_meat", true);
    else if (food == InventoryItem.ITEM_TYPE.POOP)
      this.spine.AnimationState.SetAnimation(0, "eat_poop", true);
    else
      this.spine.AnimationState.SetAnimation(0, "eat_grass", true);
    if (!discovered)
    {
      NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/RanchableFavourtieFood", this.animal.GetName(), FontImageNames.GetIconByType(this.animal.FavouriteFood));
      this.animal.IsFavouriteFoodRevealed = true;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.gameObject, 5f);
    }
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(5f, (System.Action) (() =>
    {
      AudioManager.Instance.StopLoop(this.eatingLoopSFX);
      if (this.ReservedByPlayer)
        return;
      this.ResetAnimalState();
    })));
    if (!discovered)
    {
      DOVirtual.DelayedCall(1.5f, (TweenCallback) (() =>
      {
        this.AddAdoration((float) this.GetFoodAdoration(food));
        BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/love_hearts", this.transform.position);
        this.isBeingFed = false;
      }));
      DOVirtual.DelayedCall(3f, (TweenCallback) (() => this.Close()));
    }
    else
      DOVirtual.DelayedCall(1.5f, (TweenCallback) (() =>
      {
        this.AddAdoration((float) this.GetFoodAdoration(food));
        this.isBeingFed = false;
      }));
  }

  public int GetFoodAdoration(InventoryItem.ITEM_TYPE food) => 50;

  public void CheckBreakingOut() => this.CheckBreakingOut(false);

  public void CheckBreakingOut(bool usePlayerPosition)
  {
    if (this.CurrentState == Interaction_Ranchable.State.Riding || this.CurrentState == Interaction_Ranchable.State.Leashed || this.CurrentState == Interaction_Ranchable.State.BabyInHutch || this.CurrentState == Interaction_Ranchable.State.InsideHutch || this.CurrentState == Interaction_Ranchable.State.Dead || this.CurrentState == Interaction_Ranchable.State.FollowPlayer || PlayerFarming.Location != FollowerLocation.Base)
      return;
    for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length && PlayerFarming.players.Count > index; ++index)
    {
      if (DataManager.Instance.FollowingPlayerAnimals[index] == this.animal)
      {
        this.followedPlayer = PlayerFarming.players[index];
        this.StartFollowingPlayer();
        return;
      }
    }
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(usePlayerPosition ? this._playerFarming.transform.position : this.transform.position);
      if (ranch.IsValid && tileAtWorldPosition != null && (ranch.Brain.RanchingTiles.Contains(tileAtWorldPosition) || ranch.Brain.GetRanchTiles().Contains(tileAtWorldPosition) || usePlayerPosition && ranch.Brain.ContainsFence(tileAtWorldPosition)))
      {
        this.Captured(ranch);
        return;
      }
    }
    if (!DataManager.Instance.BreakingOutAnimals.Contains(this.animal))
    {
      DataManager.Instance.BreakingOutAnimals.Add(this.animal);
      AnimalData.RemoveAnimal(this.animal, this.ranch.Brain.Data.Animals);
    }
    if ((bool) (UnityEngine.Object) this.ranch)
      this.ranch.HasChanged = true;
    this.unitObject.SpeedMultiplier = 2.5f;
    this.unitObject.maxSpeed = this.originalMaxSpeed * 2.5f;
    this.CurrentState = Interaction_Ranchable.State.BreakingOut;
    this.moveTimer = 0.0f;
    this.Interactable = true;
  }

  public void NotifyOvercrowded(Interaction_Ranch r)
  {
    if ((UnityEngine.Object) r == (UnityEngine.Object) null)
      return;
    bool isOvercrowded = r.IsOvercrowded;
    foreach (StructuresData.Ranchable_Animal animal1 in r.Brain.Data.Animals)
    {
      Interaction_Ranchable animal2 = Interaction_Ranch.GetAnimal(animal1);
      if ((UnityEngine.Object) animal2 != (UnityEngine.Object) null && animal2.CurrentState != Interaction_Ranchable.State.Dead)
      {
        BiomeConstants.Instance.EmitSmokeExplosionVFX(animal2.transform.position);
        animal2.CurrentState = isOvercrowded ? Interaction_Ranchable.State.Overcrowded : Interaction_Ranchable.State.Default;
        animal2.UpdateSkin();
      }
    }
    AudioManager.Instance.PlayOneShot(isOvercrowded ? "event:/dlc/animal/shared/overcrowded_start" : "event:/dlc/animal/shared/overcrowded_stop", this.transform.position);
    if (isOvercrowded)
      NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.RanchOvercrowded);
    else
      NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.RanchNotOvercrowded);
  }

  public void Captured(Interaction_Ranch r)
  {
    if ((UnityEngine.Object) this.ranch != (UnityEngine.Object) null && this.ranch.Brain.Data.ID != r.Brain.Data.ID)
      this.ranch.Brain.RemoveAnimal(this.animal);
    this.ranch = r;
    this.ClearPath();
    this.CurrentState = Interaction_Ranchable.State.Default;
    this.unitObject.SpeedMultiplier = 1f;
    this.unitObject.maxSpeed = this.originalMaxSpeed;
    int num1 = r.IsOvercrowded ? 1 : 0;
    if (!r.Brain.Data.Animals.Contains(this.animal))
      AnimalData.AddAnimal(this.animal, r.Brain.Data.Animals);
    int num2 = r.IsOvercrowded ? 1 : 0;
    if (num1 != num2)
      this.NotifyOvercrowded(r);
    foreach (StructuresData.Ranchable_Animal animal1 in r.Brain.Data.Animals)
    {
      if (animal1 != this.animal && animal1.Ailment == Interaction_Ranchable.Ailment.Feral && this.animal.Age >= 2)
      {
        Interaction_Ranchable animal2 = Interaction_Ranch.GetAnimal(animal1);
        if ((UnityEngine.Object) animal2 != (UnityEngine.Object) null && animal2.CurrentState != Interaction_Ranchable.State.Dead && this.CurrentState != Interaction_Ranchable.State.Dead && this.animal.Ailment != Interaction_Ranchable.Ailment.Injured && !animal2.ReservedByPlayer && (double) Time.time - (double) this.LastHealedTime > 5.0 && (double) Time.time - (double) animal2.LastAttackTime > 10.0)
        {
          animal2.targetAnimal = this;
          animal2.moveTimer = Time.time + 0.5f;
          animal2.LastAttackTime = Time.time;
          animal2.CurrentState = Interaction_Ranchable.State.MovingToAttack;
          break;
        }
      }
    }
    DataManager.Instance.BreakingOutAnimals.Remove(this.animal);
    r.HasChanged = true;
    if (this.CurrentTile == null || this.CurrentTile != null && !this.ranch.Brain.RanchingTiles.Contains(this.CurrentTile))
    {
      if (this.ranch.Brain.RanchingTiles.Count <= 0)
        return;
      PlacementRegion.TileGridTile tileGridTile = (PlacementRegion.TileGridTile) null;
      for (int index1 = 0; index1 < this.ranch.Brain.RanchingTiles.Count; ++index1)
      {
        PlacementRegion.TileGridTile ranchingTile = this.ranch.Brain.RanchingTiles[index1];
        bool flag = false;
        for (int index2 = 0; index2 < Interaction_Ranchable.Ranchables.Count; ++index2)
        {
          if ((UnityEngine.Object) Interaction_Ranchable.Ranchables[index2] != (UnityEngine.Object) this && Interaction_Ranchable.Ranchables[index2].CurrentTile != null && Interaction_Ranchable.Ranchables[index2].CurrentTile == ranchingTile)
          {
            flag = true;
            break;
          }
        }
        if (!flag && (tileGridTile == null || (double) Vector3.Distance(ranchingTile.WorldPosition, this.transform.position) < (double) Vector3.Distance(tileGridTile.WorldPosition, this.transform.position)))
          tileGridTile = ranchingTile;
      }
      if (tileGridTile != null)
      {
        this.unitObject.givePath(tileGridTile.WorldPosition);
      }
      else
      {
        Debug.LogWarning((object) "All ranch tiles are occupied! Picking a fallback tile.");
        this.unitObject.givePath(this.ranch.Brain.RanchingTiles[UnityEngine.Random.Range(0, this.ranch.Brain.RanchingTiles.Count)].WorldPosition);
      }
    }
    else
    {
      Debug.Log((object) "SKIP CAPTURED MOVE!");
      if (this.CurrentTile != null)
      {
        this.unitObject.givePath(this.CurrentTile.WorldPosition);
      }
      else
      {
        if (this.ranch.Brain.RanchingTiles.Count <= 0)
          return;
        this.unitObject.givePath(this.ranch.Brain.RanchingTiles[UnityEngine.Random.Range(0, this.ranch.Brain.RanchingTiles.Count)].WorldPosition);
      }
    }
  }

  public void ClearPath()
  {
    this.unitObject.ClearPaths();
    if (this.CurrentState == Interaction_Ranchable.State.Dead)
      return;
    this.spine.AnimationState.SetAnimation(0, this.idle_anim, true);
  }

  public void TargetHutch(Structures_RanchHutch hutch)
  {
    this.ClearPath();
    this.targetHutch = hutch;
    this.Move();
    this.moveTimer = float.MaxValue;
  }

  public bool IsTargetingHutch(Structures_RanchHutch hutch)
  {
    return this.targetHutch != null && hutch == this.targetHutch;
  }

  public void ResetTargetHutch()
  {
    this.targetHutch = (Structures_RanchHutch) null;
    this.moveTimer = 0.0f;
    this.spine.gameObject.SetActive(true);
    this.CurrentState = Interaction_Ranchable.State.Default;
    this.Move();
  }

  public static int FoodSatiation(InventoryItem.ITEM_TYPE food)
  {
    if (InventoryItem.IsFish(food))
      return 75;
    return InventoryItem.IsMeat(food) || food != InventoryItem.ITEM_TYPE.POOP ? 50 : 10;
  }

  public Interaction_Ranch GetClosestRanch()
  {
    if (Interaction_Ranch.Ranches.Count <= 0)
      return (Interaction_Ranch) null;
    Interaction_Ranch closestRanch = Interaction_Ranch.Ranches[0];
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      if ((double) Vector3.Distance(ranch.transform.position, this.transform.position) < (double) Vector3.Distance(closestRanch.transform.position, this.transform.position))
        closestRanch = ranch;
    }
    return closestRanch;
  }

  public Structures_RanchHutch GetTargetHutch()
  {
    return this.animal.State == Interaction_Ranchable.State.EnteringHutch ? this.targetHutch : (Structures_RanchHutch) null;
  }

  public Structures_RanchHutch GetHutch()
  {
    if ((UnityEngine.Object) this.ranch != (UnityEngine.Object) null)
    {
      foreach (PlacementRegion.TileGridTile ranchingTile in this.ranch.Brain.RanchingTiles)
      {
        if (ranchingTile.ObjectOnTile == StructureBrain.TYPES.RANCH_HUTCH)
        {
          Structures_RanchHutch structureById = StructureManager.GetStructureByID<Structures_RanchHutch>(ranchingTile.ObjectID);
          if (structureById != null)
          {
            foreach (StructuresData.Ranchable_Animal animal in structureById.Data.Animals)
            {
              if (animal.ID == this.animal.ID)
                return structureById;
            }
          }
        }
      }
    }
    return (Structures_RanchHutch) null;
  }

  public IEnumerator BubbleRoutine(WorshipperBubble.SPEECH_TYPE type)
  {
    float bubbleTimer = 0.3f;
    while (true)
    {
      if ((double) (bubbleTimer -= Time.deltaTime) < 0.0)
      {
        this.bubble.Play(type);
        bubbleTimer = (float) (4 + UnityEngine.Random.Range(0, 2));
      }
      if (type != WorshipperBubble.SPEECH_TYPE.STARVING || this.isStarving)
      {
        if (type != WorshipperBubble.SPEECH_TYPE.ILL || this.animal.Ailment == Interaction_Ranchable.Ailment.Stinky)
          yield return (object) null;
        else
          goto label_6;
      }
      else
        break;
    }
    this.bubble.Close();
    this.bubbleRoutine = (Coroutine) null;
    yield break;
label_6:
    this.bubble.Close();
    this.bubbleRoutine = (Coroutine) null;
  }

  public void ClearCurrentPath() => this.ClearPath();

  public bool Damage(GameObject attacker)
  {
    Vector3 normalized = (this.transform.position - attacker.transform.position).normalized;
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, (Vector2) normalized, 3f, (int) this.collisionMask);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      this.transform.DOMove((Vector3) raycastHit2D.point - normalized * 0.2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCubic);
    else
      this.unitObject.DoKnockBack(Utils.GetAngle(attacker.transform.position, this.transform.position), 1f, 0.5f);
    this.simpleSpineFlash.FlashFillRed();
    ++this.Hits;
    if (this.Hits < 5)
      return false;
    this.animal.CauseOfDeath = Interaction_Ranchable.CauseOfDeath.DiedFromWolf;
    this.Die();
    return true;
  }

  public static bool IsAnyRanchableInPlayerInteraction()
  {
    for (int index = 0; index < Interaction_Ranchable.Ranchables.Count; ++index)
    {
      if (Interaction_Ranchable.Ranchables[index].CurrentState == Interaction_Ranchable.State.Leashed || Interaction_Ranchable.Ranchables[index].CurrentState == Interaction_Ranchable.State.Riding)
        return true;
    }
    return false;
  }

  public string GetGivenName() => this.animal.GivenName;

  public string GetSharedAnimationAnimalName()
  {
    switch (this.animal.Type)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        return "goat";
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        return "turtle";
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        return "crab";
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        return "spider";
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        return "snail";
      default:
        return "goat";
    }
  }

  public string GetAnimationAnimalName()
  {
    switch (this.animal.Type)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_GOAT:
        return "goat";
      case InventoryItem.ITEM_TYPE.ANIMAL_TURTLE:
        return "turtle";
      case InventoryItem.ITEM_TYPE.ANIMAL_CRAB:
        return "crab";
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        return "spider";
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        return "snail";
      case InventoryItem.ITEM_TYPE.ANIMAL_COW:
        return "cow";
      case InventoryItem.ITEM_TYPE.ANIMAL_LLAMA:
        return "llama";
      default:
        return "goat";
    }
  }

  public IEnumerator WaitForSeconds(float duration, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(duration);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void UpdateHappyState()
  {
  }

  public IEnumerator NameAnimalIE()
  {
    Interaction_Ranchable interactionRanchable = this;
    interactionRanchable.animal.State = Interaction_Ranchable.State.Animating;
    yield return (object) new WaitForEndOfFrame();
    interactionRanchable._playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionRanchable._playerFarming.state.facingAngle = Utils.GetAngle(interactionRanchable.state.transform.position, interactionRanchable.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionRanchable._playerFarming.CameraBone, 6f);
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadCultNameAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    UICultNameMenuController cultNameMenuInstance = MonoSingleton<UIManager>.Instance.CultNameMenuTemplate.Instantiate<UICultNameMenuController>();
    cultNameMenuInstance.RequiresName = false;
    cultNameMenuInstance.Show(interactionRanchable.animal.GivenName, true, false);
    if (string.IsNullOrEmpty(interactionRanchable.animal.GivenName))
      cultNameMenuInstance.ShowEmptyName();
    cultNameMenuInstance.SetTitle(LocalizationManager.GetTranslation("UI/NameAnimal"));
    cultNameMenuInstance.OnNameConfirmed += (System.Action<string>) (name =>
    {
      this.animal.GivenName = name;
      LetterBox.Instance.ShowSubtitle(this.animal.GivenName.Colour(StaticColors.YellowColorHex));
      this.animalNameParent.gameObject.SetActive(!string.IsNullOrEmpty(this.animal.GivenName));
      this.animalNameText.text = this.animal.GivenName;
    });
    UICultNameMenuController nameMenuController1 = cultNameMenuInstance;
    nameMenuController1.OnHide = nameMenuController1.OnHide + (System.Action) (() => { });
    UICultNameMenuController nameMenuController2 = cultNameMenuInstance;
    nameMenuController2.OnHidden = nameMenuController2.OnHidden + (System.Action) (() => cultNameMenuInstance = (UICultNameMenuController) null);
    while ((UnityEngine.Object) cultNameMenuInstance != (UnityEngine.Object) null)
      yield return (object) null;
    interactionRanchable.ResetAnimalState();
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchable.ReservedByPlayer = false;
  }

  public void Sleep()
  {
    this.unitObject.EndOfPath -= new System.Action(this.Sleep);
    this.CurrentState = Interaction_Ranchable.State.Sleeping;
    this.spine.AnimationState.SetAnimation(0, "sleeping", true);
    AudioManager.Instance.StopLoop(this.feralLoopSFX);
  }

  public void Sleep(Vector3 position)
  {
    this.unitObject.ClearPaths();
    this.unitObject.givePath(position);
    this.unitObject.EndOfPath += new System.Action(this.Sleep);
  }

  public void ResetAnimalState(bool usePlayerPosition = false)
  {
    AudioManager.Instance.StopLoop(this.eatingLoopSFX);
    this.targetAnimal = (Interaction_Ranchable) null;
    this.CurrentState = Interaction_Ranchable.State.BreakingOut;
    this.CheckBreakingOut(usePlayerPosition);
  }

  public bool IsFollowingPlayerActive()
  {
    for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
    {
      if (DataManager.Instance.FollowingPlayerAnimals[index] == this.animal)
        return PlayerFarming.players.Count < index;
    }
    return false;
  }

  public void UnitObject_NewPath()
  {
    int num = this.gameObject.activeInHierarchy ? 1 : 0;
  }

  public static string GetWorkDescription(
    StructuresData.Ranchable_Animal animal,
    bool shortDescription = false)
  {
    int lootCount = animal.GetLootCount();
    string str1 = "Wool";
    if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      str1 = "Crystals";
    else if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
      str1 = "MAGMA_STONE";
    else if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      str1 = "Seeds";
    else if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      str1 = "Silk";
    string format = LocalizationManager.GetTranslation($"FollowerInteractions/Harvest_{str1}/Description");
    if (shortDescription)
    {
      if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT || animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA || animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      {
        LocalizationManager.GetTranslation("FollowerInteractions/Shear");
        format = LocalizationManager.GetTranslation("FollowerInteractions/Shear") + " {0}";
      }
      else
        format = LocalizationManager.GetTranslation("FollowerInteractions/Harvest") + " {0}";
    }
    string str2 = CostFormatter.FormatCost((InventoryItem.ITEM_TYPE) Interaction_Ranchable.GetWorkLoot(animal)[0].type, lootCount, ignoreAffordability: true);
    return string.Format(format, (object) str2);
  }

  public bool TryScheduleRanchFight(Interaction_Ranch r, Interaction_Ranchable preferredTarget = null)
  {
    if ((UnityEngine.Object) r == (UnityEngine.Object) null || this.CurrentState == Interaction_Ranchable.State.Dead || this.CurrentState == Interaction_Ranchable.State.Leashed || this.CurrentState == Interaction_Ranchable.State.Riding || this.CurrentState == Interaction_Ranchable.State.FollowPlayer || this.animal.Ailment != Interaction_Ranchable.Ailment.Feral || this.animal.Age <= 2 || (double) Time.time - (double) this.LastHealedTime <= 5.0 || (double) Time.time - (double) this.LastAttackTime <= 10.0)
      return false;
    Interaction_Ranchable interactionRanchable = preferredTarget;
    if ((UnityEngine.Object) interactionRanchable == (UnityEngine.Object) null)
    {
      foreach (StructuresData.Ranchable_Animal animal1 in r.Brain.Data.Animals)
      {
        if (animal1 != this.animal && animal1.Ailment != Interaction_Ranchable.Ailment.Feral && this.animal.Age > 2)
        {
          Interaction_Ranchable animal2 = Interaction_Ranch.GetAnimal(animal1);
          if ((UnityEngine.Object) animal2 != (UnityEngine.Object) null && animal2.CurrentState != Interaction_Ranchable.State.Dead && this.CurrentState != Interaction_Ranchable.State.Dead && animal2.animal.Ailment != Interaction_Ranchable.Ailment.Injured && !animal2.ReservedByPlayer && (double) Time.time - (double) this.LastHealedTime > 5.0)
          {
            interactionRanchable = animal2;
            break;
          }
        }
      }
    }
    if ((UnityEngine.Object) interactionRanchable == (UnityEngine.Object) null)
      return false;
    this.targetAnimal = interactionRanchable;
    this.moveTimer = Time.time + 0.5f;
    this.LastAttackTime = Time.time;
    this.CurrentState = Interaction_Ranchable.State.MovingToAttack;
    return true;
  }

  public static string GetMilkDescription()
  {
    int cost = 1;
    return string.Format(LocalizationManager.GetTranslation("FollowerInteractions/MilkAnimal") + " {0}", (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.MILK, cost, ignoreAffordability: true));
  }

  public static Interaction_Ranchable GetAnimal(StructuresData.Ranchable_Animal animal)
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if (ranchable.Animal == animal)
        return ranchable;
    }
    foreach (Interaction_Ranchable deadRanchable in Interaction_Ranchable.DeadRanchables)
    {
      if (deadRanchable.Animal == animal)
        return deadRanchable;
    }
    return (Interaction_Ranchable) null;
  }

  public void SpawnWolfSequence(System.Action<Interaction_WolfBase> callback = null)
  {
    DataManager.Instance.TimeSinceLastWolf = TimeManager.TotalElapsedGameTime + UnityEngine.Random.Range(Interaction_Ranchable.TIME_BETWEEN_WOLVES.x, Interaction_Ranchable.TIME_BETWEEN_WOLVES.y);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.SpawnWolfIE(callback));
  }

  public void SpawnWolf(float offset, bool givePath, System.Action<Interaction_WolfBase> callback = null)
  {
    Interaction_WolfBase.SpawnWolf(new Vector3(offset, this.transform.position.y), this, givePath, (System.Action<Interaction_WolfBase>) (result =>
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(result.transform.position);
      this.enemyWolf = result;
      System.Action<Interaction_WolfBase> action = callback;
      if (action == null)
        return;
      action(this.enemyWolf);
    }));
  }

  public IEnumerator SpawnWolfIE(System.Action<Interaction_WolfBase> callback = null)
  {
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || MMConversation.isPlaying || LetterBox.IsPlaying || PlayerFarming.Location != FollowerLocation.Base || PlayerFarming.IsAnyPlayerInInteractionWithRanchable() || PlayerFarming.Instance.GoToAndStopping || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive || SimulationManager.IsPaused)
    {
      yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
    }
    if (!this.BeingAscended)
    {
      GameManager.GetInstance().OnConversationNew();
      Interaction_WolfBase.WolfTarget = UnityEngine.Random.Range(Mathf.Min(Interaction_Ranchable.Ranchables.Count * 2, 8), Mathf.Min(Interaction_Ranchable.Ranchables.Count * 3, 20));
      int num1;
      Interaction_WolfBase.WolfCount = num1 = 0;
      Interaction_WolfBase.WolfDied = num1;
      Interaction_WolfBase.WolfFled = num1;
      AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/arrival_camera_whoosh");
      int offset = (double) UnityEngine.Random.value < 0.5 ? -28 : 28;
      this.SpawnWolf((float) offset, false, (System.Action<Interaction_WolfBase>) (result =>
      {
        this.enemyWolf = result;
        this.enemyWolf.enabled = false;
        this.enemyWolf.CurrentState = Interaction_WolfBase.State.Animating;
        this.enemyWolf.UnitObject.ClearPaths();
        System.Action<Interaction_WolfBase> action = callback;
        if (action != null)
          action(this.enemyWolf);
        GameManager.GetInstance().OnConversationNext(this.enemyWolf.gameObject);
      }));
      AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/arrival_poof");
      while ((UnityEngine.Object) this.enemyWolf == (UnityEngine.Object) null)
        yield return (object) null;
      this.enemyWolf.Spine.AnimationState.SetAnimation(0, "howl", false);
      this.enemyWolf.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/vocals_shared/dog_basic_small/howl", this.enemyWolf.transform.position);
      yield return (object) new UnityEngine.WaitForSeconds(1.5f);
      int num2 = Mathf.Min(Interaction_Ranchable.Ranchables.Count, 5);
      for (int index = 0; index < num2; ++index)
      {
        if (Interaction_Ranchable.Ranchables[index].animal != this.Animal && Interaction_Ranchable.Ranchables[index].CurrentState != Interaction_Ranchable.State.BabyInHutch)
        {
          Interaction_Ranchable animal = Interaction_Ranchable.GetAnimal(Interaction_Ranchable.Ranchables[index].animal);
          if ((UnityEngine.Object) animal != (UnityEngine.Object) null)
            animal.SpawnWolf((float) offset * 1.2f, true);
        }
      }
      yield return (object) new UnityEngine.WaitForSeconds(0.933333337f);
      HUD_DisplayName.Play("Notifications/WolvesIncoming", 3, HUD_DisplayName.Positions.Centre, HUD_DisplayName.textBlendMode.DungeonFinal);
      AudioManager.Instance.PlayMusic("event:/music/forest/forest_main");
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.SpecialCombat);
      this.enemyWolf.enabled = true;
      this.enemyWolf.GivePath();
      if (this.enemyWolf.CurrentState != Interaction_WolfBase.State.Fleeing)
        this.enemyWolf.CurrentState = Interaction_WolfBase.State.Preying;
      yield return (object) new UnityEngine.WaitForSeconds(2f);
      Interaction_WolfBase.WolfEvent onWolvesBegan = Interaction_WolfBase.OnWolvesBegan;
      if (onWolvesBegan != null)
        onWolvesBegan();
      foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
        ranchable.Hits = 0;
      GameManager.GetInstance().OnConversationEnd();
    }
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming = null)
  {
    if (!this.Interactable)
      return;
    base.IndicateHighlighted(playerFarming);
    this.CacheSpeed = this.unitObject.maxSpeed;
    this.unitObject.maxSpeed = 0.0f;
    if (this.animal.State == Interaction_Ranchable.State.Dead)
      return;
    this.adorationUI.Show();
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.EndIndicateHighlighted(playerFarming);
    this.unitObject.maxSpeed = this.CacheSpeed;
    this.adorationUI.Hide();
  }

  public void PlayDieIcegoreVO()
  {
    string vo = "";
    if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
      vo = "event:/dlc/animal/goat/death_from_icegore_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      vo = "event:/dlc/animal/crab/death_from_icegore_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      vo = "event:/dlc/animal/snail/death_from_icegore_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      vo = "event:/dlc/animal/spider/death_from_icegore_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
      vo = "event:/dlc/animal/llama/death_from_icegore_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
      vo = "event:/dlc/animal/turtle/death_from_icegore_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      vo = "event:/dlc/animal/cow/death_from_icegore_vo";
    this.PlayVO(vo);
  }

  public void PlayDieVO()
  {
    string vo = "";
    if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
      vo = "event:/dlc/animal/goat/death_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      vo = "event:/dlc/animal/crab/death_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      vo = "event:/dlc/animal/snail/death_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      vo = "event:/dlc/animal/spider/death_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
      vo = "event:/dlc/animal/llama/death_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
      vo = "event:/dlc/animal/turtle/death_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      vo = "event:/dlc/animal/cow/death_vo";
    this.PlayVO(vo);
  }

  public void PlayPetVO()
  {
    string vo = "";
    if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
      vo = "event:/dlc/animal/goat/pet_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      vo = "event:/dlc/animal/crab/pet_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      vo = "event:/dlc/animal/snail/pet_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      vo = "event:/dlc/animal/spider/pet_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
      vo = "event:/dlc/animal/llama/pet_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
      vo = "event:/dlc/animal/turtle/pet_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      vo = "event:/dlc/animal/cow/pet_vo";
    this.PlayVO(vo);
  }

  public void PlayEmergeVO()
  {
    string vo = "";
    if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
      vo = "event:/dlc/animal/goat/baby_emerge_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      vo = "event:/dlc/animal/crab/baby_emerge_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      vo = "event:/dlc/animal/snail/baby_emerge_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      vo = "event:/dlc/animal/spider/baby_emerge_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
      vo = "event:/dlc/animal/llama/baby_emerge_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
      vo = "event:/dlc/animal/turtle/baby_emerge_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      vo = "event:/dlc/animal/cow/baby_emerge_vo";
    this.PlayVO(vo);
  }

  public void PlayAppearVO()
  {
    string vo = "";
    if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
      vo = "event:/dlc/animal/goat/appear_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      vo = "event:/dlc/animal/crab/appear_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      vo = "event:/dlc/animal/snail/appear_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      vo = "event:/dlc/animal/spider/appear_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
      vo = "event:/dlc/animal/llama/appear_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
      vo = "event:/dlc/animal/turtle/appear_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      vo = "event:/dlc/animal/cow/appear_vo";
    this.PlayVO(vo);
  }

  public void PlayInjuredVO()
  {
    string vo = "";
    if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
      vo = "event:/dlc/animal/goat/injure_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      vo = "event:/dlc/animal/crab/injure_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      vo = "event:/dlc/animal/snail/injure_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      vo = "event:/dlc/animal/spider/injure_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
      vo = "event:/dlc/animal/llama/injure_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
      vo = "event:/dlc/animal/turtle/injure_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      vo = "event:/dlc/animal/cow/injure_vo";
    this.PlayVO(vo);
  }

  public void PlayIdleVO()
  {
    string vo = "";
    if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_GOAT)
      vo = "event:/dlc/animal/goat/idle_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_CRAB)
      vo = "event:/dlc/animal/crab/idle_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      vo = "event:/dlc/animal/snail/idle_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      vo = "event:/dlc/animal/spider/idle_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
      vo = "event:/dlc/animal/llama/idle_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_TURTLE)
      vo = "event:/dlc/animal/turtle/idle_vo";
    else if (this.animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_COW)
      vo = "event:/dlc/animal/cow/idle_vo";
    this.PlayVO(vo);
  }

  public void HitWithSnowball()
  {
    this.PlayPetVO();
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/pet", this.transform.position);
    this.CurrentState = Interaction_Ranchable.State.Animating;
    this.spine.AnimationState.SetAnimation(0, "hit", false);
    this.spine.AnimationState.AddAnimation(0, this.idle_anim, true, 0.0f);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      if (this.CurrentState != Interaction_Ranchable.State.Animating)
        return;
      this.CurrentState = Interaction_Ranchable.State.Animating;
    }));
  }

  public void AddAdoration(float amount)
  {
    if (this.animal.State == Interaction_Ranchable.State.Dead)
      return;
    this.animal.Adoration += amount;
    this.StartCoroutine((IEnumerator) this.adorationUI.IncreaseAdorationIE());
    if ((double) this.animal.Adoration < 100.0)
      return;
    ++this.animal.Level;
    this.animal.Adoration = 0.0f;
  }

  public static InventoryItem.ITEM_TYPE DrawFavouriteFood()
  {
    if ((double) UnityEngine.Random.value < 0.05000000074505806)
    {
      int index = UnityEngine.Random.Range(0, Interaction_Ranchable.UncommonFavouriteFood.Count);
      return Interaction_Ranchable.UncommonFavouriteFood[index];
    }
    int index1 = UnityEngine.Random.Range(0, Interaction_Ranchable.CommonFavouriteFood.Count);
    return Interaction_Ranchable.CommonFavouriteFood[index1];
  }

  public bool MakeFeralByObjective()
  {
    foreach (Objectives_FeedAnimal customObjective in ObjectiveManager.GetCustomObjectives(Objectives.TYPES.FEED_ANIMAL))
    {
      if (customObjective.TargetAnimal == this.animal.ID && customObjective.Food == InventoryItem.ITEM_TYPE.FOLLOWER_MEAT)
        return true;
    }
    return false;
  }

  public void OnStructureModified(StructuresData structure)
  {
    if (this.currentHutchID == -1 || structure.ID != this.currentHutchID || this.CurrentState != Interaction_Ranchable.State.InsideHutch && this.CurrentState != Interaction_Ranchable.State.EnteringHutch)
      return;
    this.CurrentState = Interaction_Ranchable.State.Default;
  }

  public void OnStartRacing()
  {
    if (this.CurrentState != Interaction_Ranchable.State.Riding || this.animal.RacedToday)
      return;
    this.racingProgressBar.Show();
    this.interactionProgressBar.Hide(true);
    this.makeHappyTimer = 0.0f;
    this.raceTimer = 0.0f;
  }

  public void OnFinishRacing(float time)
  {
    if (this.CurrentState != Interaction_Ranchable.State.Riding)
      return;
    if (this.racingProgressBar.IsVisible && (double) this.raceTimer < (double) this.raceTime)
    {
      this.AddAdoration(100f);
      this.animal.RacedToday = true;
      BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_small");
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/love_hearts", this.transform.position);
    }
    this.racingProgressBar.Hide(true);
  }

  public void PlayVO(string vo)
  {
    if (string.IsNullOrEmpty(vo))
      return;
    AudioManager.Instance.PlayOneShotAndSetParameterValue(vo, "isAnimalBaby", this.IsBaby() ? 1f : 0.0f, this.transform);
  }

  public bool IsBaby() => this.animal.Age < 2;

  public void SetRidingSpeedSFXParam(float playerSpeed)
  {
    AudioManager.Instance.SetEventInstanceParameter(this.ridingLoopSFX, this.ridingSpeedParameter, Mathf.InverseLerp(0.0f, 1f, playerSpeed));
  }

  public void UpdateReservedTiles()
  {
    this.reservedTiles.Clear();
    if (!((UnityEngine.Object) this.ranch != (UnityEngine.Object) null) || this.CurrentState == Interaction_Ranchable.State.BreakingOut)
      return;
    for (int index = 0; index < Interaction_Ranchable.positionCheckMoveDirections.Length; ++index)
    {
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.CurrentTile.WorldPosition + Interaction_Ranchable.positionCheckMoveDirections[index]);
      if (tileAtWorldPosition != null)
        this.reservedTiles.Add(tileAtWorldPosition);
    }
  }

  [CompilerGenerated]
  public void \u003COnDisable\u003Eb__132_0()
  {
    if (this.destroyed || this.CurrentState != Interaction_Ranchable.State.BreakingOut)
      return;
    this.transform.parent = BaseLocationManager.Instance.UnitLayer;
    this.transform.position = Vector3.zero;
  }

  [CompilerGenerated]
  public void \u003CBeginRiding\u003Eb__161_0()
  {
    this.animalNameParent.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CUpdateMovingToAttack\u003Eb__168_1() => this.ResetAnimalState();

  [CompilerGenerated]
  public void \u003CPoop\u003Eb__203_0()
  {
    if (!((UnityEngine.Object) this.ranch != (UnityEngine.Object) null) || this.ranch.Brain == null)
      return;
    this.ranch.Brain.AnimalPooped(this.animal, this.transform.position, new System.Action<GameObject>(this.LerpPoop));
  }

  [CompilerGenerated]
  public void \u003CEat\u003Eb__205_0()
  {
    this.postFeedRoutine = (Coroutine) null;
    AudioManager.Instance.StopLoop(this.eatingLoopSFX);
    this.UpdateSkin();
    this.ResetAnimalState();
  }

  [CompilerGenerated]
  public void \u003CEat\u003Eb__205_1() => this.isBeingFed = false;

  [CompilerGenerated]
  public void \u003CHitWithSnowball\u003Eb__253_0()
  {
    if (this.CurrentState != Interaction_Ranchable.State.Animating)
      return;
    this.CurrentState = Interaction_Ranchable.State.Animating;
  }

  public enum State
  {
    Default,
    Animating,
    BreakingOut,
    Leashed,
    Riding,
    EnteringHutch,
    InsideHutch,
    BabyInHutch,
    Sleeping,
    Dead,
    FollowPlayer,
    Overcrowded,
    MovingToAttack,
  }

  public enum CauseOfDeath
  {
    Default,
    DiedFromOldAge,
    DiedFromStarvation,
    DiedFromWolf,
    DiedFromInjury,
  }

  public enum Ailment
  {
    None,
    Feral,
    Stinky,
    Injured,
  }

  public enum Mood
  {
    Happy,
    Sad,
  }

  public enum GrowthRate
  {
    Slow,
    Normal,
    Fast,
  }
}
