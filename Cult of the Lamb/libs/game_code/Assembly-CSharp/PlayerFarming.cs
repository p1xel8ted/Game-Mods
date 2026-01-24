// Decompiled with JetBrains decompiler
// Type: PlayerFarming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using Rewired;
using Spine;
using Spine.Unity;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unify.Input;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class PlayerFarming : BaseMonoBehaviour
{
  public static List<PlayerFarming> players = new List<PlayerFarming>();
  public static int playersCount;
  public static PlayerFarming Instance;
  public GameObject InstanceMarker;
  [CompilerGenerated]
  public Indicator \u003Cindicator\u003Ek__BackingField;
  public Interactor interactor;
  [HideInInspector]
  public bool IsKnockedOut;
  [HideInInspector]
  public bool IsBurrowing;
  public PlayerWeapon.EquippedWeaponsInfo _CurrentWeaponInfo;
  public GameObject HudHeartsPrefab;
  public GameObject HudRelicPrefab;
  public HUD_Hearts hudHearts;
  [HideInInspector]
  public bool CanPickupBlackSoul;
  public List<TarotCards.TarotCard> RunTrinkets = new List<TarotCards.TarotCard>();
  public List<TarotCards.TarotCard> FoundTrinkets = new List<TarotCards.TarotCard>();
  public List<TarotCards.TarotCard> CorruptedTrinketsOnlyPositive = new List<TarotCards.TarotCard>();
  public List<TarotCards.TarotCard> CorruptedTrinketsOnlyNegative = new List<TarotCards.TarotCard>();
  public Dictionary<TarotCards.Card, float> CardCooldowns = new Dictionary<TarotCards.Card, float>();
  public Player rewiredPlayer;
  public int playerID;
  public static HashSet<StateMachine.State> LongToPerformPlayerStates = new HashSet<StateMachine.State>()
  {
    StateMachine.State.Idle,
    StateMachine.State.Moving,
    StateMachine.State.Meditate,
    StateMachine.State.Idle_CarryingBody,
    StateMachine.State.Moving_CarryingBody,
    StateMachine.State.Idle_Winter,
    StateMachine.State.Moving_Winter
  };
  public System.Action OnCrownReturn;
  public System.Action OnCrownReturnSubtle;
  public System.Action OnHideCrown;
  public static FollowerLocation Location = FollowerLocation.None;
  public static FollowerLocation LastLocation = FollowerLocation.None;
  public GrowAndFade growAndFade;
  public GameObject CameraBone;
  public GameObject _CameraBone;
  public GameObject FishingLineBone;
  public Transform CrownBone;
  [SerializeField]
  public MeshRenderer playerGlow;
  [CompilerGenerated]
  public PlayerDoctrineStone \u003CPlayerDoctrineStone\u003Ek__BackingField;
  public PlayerController _playerController;
  public UnitObject _unitObject;
  public Farm farm;
  public float ArrowAttackDelay;
  [HideInInspector]
  public float DodgeDelay;
  public HUD_Inventory InventoryMenu;
  public Inventory inventory;
  public PlayerSimpleInventory simpleInventory;
  public PlayerCanvas playerCanvas;
  public WeaponSelectMenu WeaponSelect;
  public PlayerAbility playerAbility;
  public GameObject[] HandBones;
  public GameObject ShieldChargeGameObject;
  public StateMachine _state;
  public float AimAngle;
  public List<CurseChargeBar> CurseChargeBars;
  public List<CurseChargeBar> WeaponChargeBars;
  public List<CurseChargeBar> HeavyChargeBars;
  [SerializeField]
  public ParticleSystem coopDamageMultiplierEffect;
  public ParticleSystem.EmissionModule coopDamageMultiplierEffectEmission;
  [SerializeField]
  public GameObject coopBeam;
  [SerializeField]
  public Transform coopBeamContainer;
  [SerializeField]
  public ColliderEvents coopBeamCollider;
  public float chargeSnowball;
  public bool createdExplosion;
  public EventInstance snowballAimInstance;
  public int RedHeartsTemporarilyRemoved;
  public Health AimTarget;
  [CompilerGenerated]
  public HealthPlayer \u003Chealth\u003Ek__BackingField;
  [CompilerGenerated]
  public PlayerChoreXPBarController \u003CplayerChoreXPBarController\u003Ek__BackingField;
  public static string GlobalFirstPlayerPos = "_GlobalFirstPlayerPos";
  public static string GlobalSecondPlayerPos = "_GlobalSecondPlayerPos";
  public GameObject WateringCanAim;
  public float WateringCanScale;
  public float WateringCanScaleSpeed;
  public float WateringCanBob;
  public SkeletonAnimation Spine;
  public int CarryingDeadFollowerID = -1;
  public bool NearGrave;
  public bool CarryingEgg;
  public bool CarryingSnowball;
  [HideInInspector]
  public Interaction_PuzzleItem PuzzlePieceCarried;
  public bool NearCompostBody;
  public bool NearFurnace;
  public StructureBrain NearStructure;
  public PlayerArrows playerArrows;
  public ParticleSystem HealingParticles;
  public SimpleSpineAnimator simpleSpineAnimator;
  public Chain chain;
  public Transform CarryBone;
  public ColliderEvents PlayerDamageCollider;
  public TrailPicker damageOnRollTrail;
  public HighRoller_VFX_Manager HighRollerVFX;
  public AnimationCurve CurseAimingCurve;
  public LineRenderer CurseAimLine;
  public GameObject CurseTarget;
  public GameObject CurseLine;
  public Material originalMaterial;
  public Material BW_Material;
  public GameObject CoopIndicatorLamb;
  public GameObject CoopIndicatorGoat;
  public bool isCoopIndicatorVisible = true;
  [CompilerGenerated]
  public Vector3 \u003CPreviousPosition\u003Ek__BackingField;
  public bool _Healing;
  [HideInInspector]
  public CircleCollider2D circleCollider2D;
  public Skin PlayerSkin;
  [HideInInspector]
  public bool isLamb = true;
  [HideInInspector]
  public bool canUseKeyboard = true;
  [HideInInspector]
  public bool IsGoat;
  [CompilerGenerated]
  public Follower \u003CPickedUpFollower\u003Ek__BackingField;
  public WorkPlace ClosestWorkPlace;
  public WorkPlaceSlot ClosestWorkPlaceSlot;
  public Dwelling ClosestDwelling;
  public DwellingSlot ClosestDwellingSlot;
  public bool GoToAndStopping;
  public bool IdleOnEnd;
  public GameObject LookToObject;
  public System.Action GoToCallback;
  public System.Action AbortGoToCallback;
  public float maxDuration = -1f;
  public bool forcePositionOnTimeout;
  public Vector3 forcePositionOnTimeoutTarget;
  public float startMoveTimestamp;
  public const float gotoAndStopIdleTimeOutSeconds = 1f;
  public const float gotoAndStopIdlePositionTolerance = 0.05f;
  public Vector3 lastPosition;
  public float stillTime;
  public bool isIdle;
  public Interaction currentInteraction;
  public Interaction previousInteraction;
  public float damageOnTouchTimer;
  public bool Meditating;
  public bool HoldingAttack;
  public EquipmentType _currentWeapon = EquipmentType.None;
  public EquipmentType _currentCurse = EquipmentType.None;
  public int currentWeaponLevel;
  public int currentRunWeaponLevel;
  public int currentCurseLevel;
  public int currentRunCurseLevel;
  public RelicType currentRelicType;
  public float RelicChargeAmount;
  public float cameraWeight;
  public float coopSpeedMultiplier;
  public bool wasInConversation;
  public int ZoomX;
  public int ZoomY;
  public static bool AllCoopPlayersInCamera;
  public bool BlockMeditation;
  public bool DodgeQueued;
  public PlayerWeapon _playerWeapon;
  public PlayerSpells _playerSpells;
  public PlayerRelic _playerRelic;
  public bool AllowDodging = true;
  public bool AbilityKeyDown = true;
  public float AbilityKeyDownAccumulated;
  public float HeavyAttackCharge;
  public bool isDashAttacking;
  public PlayerFarming.Attack CurrentAttack;
  public int CurrentCombo = -1;
  public float ResetCombo;
  public List<PlayerFarming.Attack> AttackList = new List<PlayerFarming.Attack>();
  public List<PlayerFarming.Attack> HeavyAttackList = new List<PlayerFarming.Attack>();
  public PlayerFarming.Attack DashAttack;
  public static float LeftoverSouls = 0.0f;
  public static System.Action OnGetXP;
  public int BlackSouls;
  public static System.Action OnGetDiscipleXP;
  public static int ScorchPos = Shader.PropertyToID("_ScorchPos");
  public static float CoopDitherMultiplier = 1f;
  [HideInInspector]
  public float playerWasHidden;
  public bool returnToBaseMeditating;
  public bool InitComplete;
  public bool StartComplete;
  public bool EnableCoopFeatures = true;
  public static Vector3 AveragePlayerPosition;
  public static bool AutoRespawn = false;

  public Indicator indicator
  {
    get => this.\u003Cindicator\u003Ek__BackingField;
    set => this.\u003Cindicator\u003Ek__BackingField = value;
  }

  public PlayerWeapon.EquippedWeaponsInfo CurrentWeaponInfo
  {
    get
    {
      if (this._CurrentWeaponInfo == null)
        this._CurrentWeaponInfo = new PlayerWeapon.EquippedWeaponsInfo();
      return this._CurrentWeaponInfo;
    }
    set => this._CurrentWeaponInfo = value;
  }

  public PlayerDoctrineStone PlayerDoctrineStone
  {
    get => this.\u003CPlayerDoctrineStone\u003Ek__BackingField;
    set => this.\u003CPlayerDoctrineStone\u003Ek__BackingField = value;
  }

  public PlayerController playerController
  {
    get
    {
      return this._playerController ?? (this._playerController = this.gameObject.GetComponent<PlayerController>());
    }
  }

  public UnitObject unitObject
  {
    get
    {
      if ((UnityEngine.Object) this._unitObject == (UnityEngine.Object) null)
      {
        this._unitObject = this.gameObject.GetComponent<UnitObject>();
        this._unitObject.UseFixedDirectionalPathing = true;
      }
      return this._unitObject;
    }
  }

  public StateMachine state
  {
    get
    {
      if ((UnityEngine.Object) this._state == (UnityEngine.Object) null)
        this._state = this.gameObject.GetComponent<StateMachine>();
      return this._state;
    }
  }

  public HealthPlayer health
  {
    get => this.\u003Chealth\u003Ek__BackingField;
    set => this.\u003Chealth\u003Ek__BackingField = value;
  }

  public PlayerChoreXPBarController playerChoreXPBarController
  {
    get => this.\u003CplayerChoreXPBarController\u003Ek__BackingField;
    set => this.\u003CplayerChoreXPBarController\u003Ek__BackingField = value;
  }

  public static PlayerFarming GetPlayerFarmingComponent(GameObject target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      return PlayerFarming.Instance;
    PlayerFarming component = target.GetComponent<PlayerFarming>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null ? PlayerFarming.Instance : component;
  }

  public static void PositionAllPlayers(Vector3 pos, bool increment = true)
  {
    Debug.Log((object) "All players positioned");
    CoopManager.PreventRecycleInCurrentRoom = false;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      Vector3 vector2 = (Vector3) Utils.DegreeToVector2(player.state.facingAngle + 90f);
      Vector3 vector3 = !increment ? Vector3.zero : vector2 / 2f * (float) index;
      player.transform.position = pos + vector3;
    }
    PlayerFarming.AveragePlayerPosition = pos;
  }

  public static void ResetRunData()
  {
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      player.RedHeartsTemporarilyRemoved = 0;
      player.currentWeaponLevel = 1;
      player.currentWeapon = EquipmentType.None;
      player.currentRunWeaponLevel = 1;
      player.currentCurseLevel = 1;
      player.currentCurse = EquipmentType.None;
      player.currentRunCurseLevel = 1;
      player.RelicChargeAmount = 0.0f;
      player.currentRelicType = RelicType.None;
      PlayerFarming.SetResetHealthData(true);
      player.health.InitHP();
    }
  }

  public static void HealAll(float amount)
  {
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      player.health.Heal(amount);
      BiomeConstants.Instance.EmitHeartPickUpVFX(player.CameraBone.transform.position, 0.0f, "red", "burst_big");
    }
  }

  public static void ReloadAllFaith(int quantity = -1)
  {
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((bool) (UnityEngine.Object) player && (bool) (UnityEngine.Object) player.playerSpells && (bool) (UnityEngine.Object) player.playerSpells.faithAmmo)
      {
        if (quantity == -1)
          player.playerSpells.faithAmmo.Reload();
        else
          player.GetBlackSoul(quantity);
      }
    }
  }

  public Vector3 PreviousPosition
  {
    get => this.\u003CPreviousPosition\u003Ek__BackingField;
    set => this.\u003CPreviousPosition\u003Ek__BackingField = value;
  }

  public static event PlayerFarming.PlayerEvent OnDodge;

  public bool Healing
  {
    get => this._Healing;
    set
    {
      if (!this._Healing && value)
      {
        this.HealingParticles.Play();
        this.StartCoroutine((IEnumerator) this.DoHealing());
      }
      if (this._Healing && !value)
      {
        this.StopCoroutine((IEnumerator) this.DoHealing());
        this.HealingParticles.Stop();
      }
      this._Healing = value;
    }
  }

  public IEnumerator DoHealing()
  {
    float HealTimer = 0.0f;
    while ((double) this.health.PLAYER_HEALTH < (double) this.health.PLAYER_TOTAL_HEALTH)
    {
      if ((double) (HealTimer += Time.deltaTime) > 1.0)
      {
        HealTimer = 0.0f;
        ++this.health.HP;
      }
      yield return (object) null;
    }
    this.HealingParticles.Stop();
  }

  public void SpineUseDeltaTime(bool Bool) => this.Spine.UseDeltaTime = Bool;

  public void Awake()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      PlayerFarming.Instance = this;
    this.PlayerDoctrineStone = this.GetComponentInChildren<PlayerDoctrineStone>();
  }

  public void Start()
  {
    if (this.StartComplete)
      return;
    this.StartComplete = true;
    this.coopDamageMultiplierEffectEmission = this.coopDamageMultiplierEffect.emission;
    this.coopDamageMultiplierEffectEmission.rateOverTimeMultiplier = 0.0f;
    if ((UnityEngine.Object) CoopManager.AllPlayerGameObjects[0] == (UnityEngine.Object) null)
      CoopManager.AllPlayerGameObjects[0] = this.gameObject;
    if ((UnityEngine.Object) this.playerGlow != (UnityEngine.Object) null)
      this.playerGlow.sharedMaterial.DOColor(Color.black, 0.0f);
    this.coopBeamCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.CoopBeamCollider_OnTriggerEnterEvent);
    this.circleCollider2D = this.GetComponent<CircleCollider2D>();
    this.playerArrows = this.GetComponent<PlayerArrows>();
    this.interactor = this.GetComponent<Interactor>();
    this.interactor.Init();
    this.indicator = HUD_Manager.Instance.InitialiseIndicator(this);
    this.indicator.playerFarming = this;
    this.indicator.gameObject.SetActive(true);
    this.InitHUD();
    this.inventory = this.GetComponent<Inventory>();
    foreach (Component curseChargeBar in this.CurseChargeBars)
      curseChargeBar.gameObject.SetActive(false);
    foreach (Component weaponChargeBar in this.WeaponChargeBars)
      weaponChargeBar.gameObject.SetActive(false);
    foreach (Component heavyChargeBar in this.HeavyChargeBars)
      heavyChargeBar.gameObject.SetActive(false);
    this.playerAbility = this.GetComponent<PlayerAbility>();
    this.simpleInventory = this.GetComponent<PlayerSimpleInventory>();
    this.simpleSpineAnimator = this.GetComponentInChildren<SimpleSpineAnimator>();
    this.PlayerDamageCollider.SetActive(false);
    this.PlayerDamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
    this.HealingParticles.Stop();
    DataManager.OnChangeTool += new DataManager.ChangeToolAction(this.ChangeTool);
    Inventory.CURRENT_WEAPON = Inventory.CURRENT_WEAPON;
    this.health = this.GetComponent<HealthPlayer>();
    this.health.OnHPUpdated += new HealthPlayer.HPUpdated(this.SetSkinOnHit);
    this.health.OnTotalHPUpdated += new HealthPlayer.HPUpdated(this.SetSkinOnHit);
    this.playerChoreXPBarController = this.GetComponentInChildren<PlayerChoreXPBarController>();
    this.playerChoreXPBarController.playerFarming = this;
    this.playerChoreXPBarController.Init();
    this.CheckIntroDemigodStatus();
    this.SetSkin();
    if (GameManager.IsDungeon(PlayerFarming.Location))
    {
      this.transform.localScale = Vector3.one * Mathf.Lerp(0.66f, 1.4f, this.playerRelic.PlayerScaleModifier / 2f);
      TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStartedInDungeon);
      this.OnNewPhaseStartedInDungeon();
    }
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    CoopManager.RefreshCoopPlayerRewired();
  }

  public void CoopBeamCollider_OnTriggerEnterEvent(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!(bool) (UnityEngine.Object) component || component.team != Health.Team.Team2)
      return;
    component.DealDamage(PlayerWeapon.GetDamage(0.1f, this.playerWeapon.CurrentWeaponLevel, this), this.gameObject, collider.transform.position, AttackType: Health.AttackTypes.NoHitStop);
  }

  public void OnNewPhaseStartedInDungeon()
  {
    if ((UnityEngine.Object) this.playerGlow == (UnityEngine.Object) null)
      return;
    if (TimeManager.IsNight)
      this.playerGlow.sharedMaterial.DOColor(new Color(0.25f, 0.25f, 0.25f), 1f);
    else
      this.playerGlow.sharedMaterial.DOColor(Color.black, 1f);
  }

  public void OnNewPhaseStarted()
  {
    int index = PlayerFarming.players.IndexOf(this);
    if (index == -1 || DataManager.Instance.FollowingPlayerAnimals.Length - 1 < index)
      return;
    DataManager.Instance.FollowingPlayerAnimals[index]?.UpdatePlayersPetState();
  }

  public void SetSkinOnHit(HealthPlayer target) => this.SetSkin();

  public Skin SetSkin()
  {
    bool flag = true;
    if (GameManager.IsDungeon(PlayerFarming.Location) && (UnityEngine.Object) this.gameObject != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.playerSpells && (bool) (UnityEngine.Object) this.playerSpells.faithAmmo)
      flag = this.playerSpells.faithAmmo.CanAfford((float) this.playerSpells.AmmoCost);
    return this.SetSkin(!flag);
  }

  public Skin SetSkin(bool BlackAndWhite)
  {
    this.IsGoat = DataManager.Instance.PlayerVisualFleece == 1003;
    this.PlayerSkin = new Skin("Player Skin");
    this.PlayerSkin.AddSkin(!this.isLamb || this.IsGoat ? this.Spine.Skeleton.Data.FindSkin("Goat") : this.Spine.Skeleton.Data.FindSkin($"Lamb_{DataManager.Instance.PlayerVisualFleece.ToString()}{(BlackAndWhite ? "_BW" : "")}"));
    string str = WeaponData.Skins.Normal.ToString().ToString();
    if (this.currentWeapon != EquipmentType.None && (bool) (UnityEngine.Object) EquipmentManager.GetWeaponData(this.currentWeapon))
      str = EquipmentManager.GetWeaponData(this.currentWeapon).Skin.ToString();
    this.PlayerSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Weapons/" + str));
    if (LocationManager.IsDungeonActive())
    {
      if ((double) this.health.CurrentHP <= 1.0 && (double) this.health.PLAYER_TOTAL_HEALTH != 2.0)
        this.PlayerSkin.AddSkin(!this.isLamb || this.IsGoat ? this.Spine.Skeleton.Data.FindSkin("Goat_Hurt2") : this.Spine.Skeleton.Data.FindSkin("Hurt2"));
      else if ((double) this.health.CurrentHP <= 2.0 && (double) this.health.PLAYER_TOTAL_HEALTH != 2.0 || (double) this.health.CurrentHP <= 1.0 && (double) this.health.PLAYER_TOTAL_HEALTH == 2.0)
        this.PlayerSkin.AddSkin(!this.isLamb || this.IsGoat ? this.Spine.Skeleton.Data.FindSkin("Goat_Hurt1") : this.Spine.Skeleton.Data.FindSkin("Hurt1"));
    }
    this.PlayerSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Mops/" + Mathf.Clamp(this.isLamb ? DataManager.Instance.ChoreXPLevel + 1 : DataManager.Instance.ChoreXPLevel_Coop + 1, 1, 10).ToString()));
    this.Spine.Skeleton.SetSkin(this.PlayerSkin);
    this.Spine.Skeleton.SetSlotsToSetupPose();
    return this.PlayerSkin;
  }

  public void InitHUD()
  {
    this.InventoryMenu = UnityEngine.Object.FindObjectOfType<HUD_Inventory>();
    if ((UnityEngine.Object) this.InventoryMenu != (UnityEngine.Object) null)
      this.InventoryMenu.gameObject.SetActive(false);
    this.WeaponSelect = UnityEngine.Object.FindObjectOfType<WeaponSelectMenu>();
    if (!((UnityEngine.Object) this.WeaponSelect != (UnityEngine.Object) null))
      return;
    this.WeaponSelect.gameObject.SetActive(false);
  }

  public Follower PickedUpFollower
  {
    get => this.\u003CPickedUpFollower\u003Ek__BackingField;
    set => this.\u003CPickedUpFollower\u003Ek__BackingField = value;
  }

  public void DropDeadFollower(bool force = false)
  {
    if (((this.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody ? 1 : (this.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody ? 1 : 0)) | (force ? 1 : 0)) == 0)
      return;
    if ((UnityEngine.Object) Interaction_HarvestMeat.CurrentMovingBody != (UnityEngine.Object) null)
    {
      Interaction_HarvestMeat.CurrentMovingBody.DropBody();
      if (!BaseGoopDoor.IsOpen)
        BaseGoopDoor.DoorDown(this);
    }
    if ((UnityEngine.Object) Interaction_WebberSkull.WebberSkull != (UnityEngine.Object) null)
    {
      Interaction_WebberSkull.WebberSkull.DropBody();
      if (!BaseGoopDoor.IsOpen)
        BaseGoopDoor.DoorDown(this);
    }
    for (int index = 0; index < Interaction_EggFollower.Interaction_EggFollowers.Count; ++index)
    {
      Interaction_EggFollower interactionEggFollower = Interaction_EggFollower.Interaction_EggFollowers[index];
      if (interactionEggFollower.CarryingEgg)
      {
        interactionEggFollower.DropBody();
        if (!BaseGoopDoor.IsOpen)
          BaseGoopDoor.DoorDown(this);
      }
    }
    for (int index = 0; index < Interaction_IceBlock.IceBlocks.Count; ++index)
    {
      Interaction_IceBlock iceBlock = Interaction_IceBlock.IceBlocks[index];
      if (iceBlock.CarryingSnowball)
      {
        iceBlock.DropSnowball();
        if (!BaseGoopDoor.IsOpen)
          BaseGoopDoor.DoorDown(this);
      }
    }
  }

  public void PickUpFollower(Follower f)
  {
    this.PickedUpFollower = f;
    this.chain.SetConnection(f.ChainConnection);
    this.PickedUpFollower.PickUp();
  }

  public void DropFollower()
  {
    this.PickedUpFollower.Drop();
    this.DetachFollower();
  }

  public void DetachFollower()
  {
    this.PickedUpFollower = (Follower) null;
    this.ResetCarryStructureColours();
    this.chain.Disconnect();
  }

  public void ResetCarryStructureColours()
  {
    if ((UnityEngine.Object) this.ClosestWorkPlaceSlot != (UnityEngine.Object) null)
      this.ClosestWorkPlaceSlot.transform.localScale = Vector3.one;
    if (!((UnityEngine.Object) this.ClosestDwellingSlot != (UnityEngine.Object) null))
      return;
    this.ClosestDwellingSlot.transform.localScale = Vector3.one;
  }

  public void CustomAnimation(string Animation, bool Loop)
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || !this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine((IEnumerator) this.CustomAnimationRoutine(Animation, Loop));
  }

  public IEnumerator CustomAnimationRoutine(string Animation, bool Loop)
  {
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    this.simpleSpineAnimator.Animate(Animation, 0, Loop);
  }

  public void CustomAnimationWithCallback(string Animation, bool Loop, System.Action Callback)
  {
    this.StartCoroutine((IEnumerator) this.CustomAnimationWithCallBackRoutine(Animation, Loop, Callback));
  }

  public IEnumerator CustomAnimationWithCallBackRoutine(
    string Animation,
    bool Loop,
    System.Action Callback)
  {
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(this.simpleSpineAnimator.Animate(Animation, 0, Loop).Animation.Duration);
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public IEnumerator BleatRoutine()
  {
    PlayerFarming playerFarming = this;
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    bool flag = false;
    playerFarming.simpleSpineAnimator.Animate(!playerFarming.isLamb || playerFarming.IsGoat ? "bleat-goat3" : "bleat", 0, false);
    if (playerFarming.isLamb && DataManager.Instance.PlayerVisualFleece == 676)
    {
      playerFarming.simpleSpineAnimator.Animate("Cowboy/yeehaw-bleat", 0, false);
      AudioManager.Instance.PlayOneShot("event:/player/yeehaa", playerFarming.gameObject);
      flag = true;
    }
    if (playerFarming.isLamb && !playerFarming.IsGoat)
    {
      if (!flag)
        AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage", playerFarming.gameObject);
      float seconds = 0.4f;
      if (DataManager.Instance.PlayerVisualFleece == 676)
        seconds = 1.167f;
      yield return (object) new WaitForSeconds(seconds);
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_bleat", playerFarming.gameObject);
      CameraManager.shakeCamera(4f);
      yield return (object) new WaitForSeconds(1.5f);
    }
    if (playerFarming.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator BlessFollower(Follower follower)
  {
    PlayerFarming playerFarming = this;
    int currentTaskType = (int) follower.Brain.CurrentTaskType;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.Brain.Stats.ReceivedBlessing = true;
    follower.FacePosition(playerFarming.transform.position);
    List<FollowerTask> unoccupiedTasksOfType = FollowerBrain.GetAllUnoccupiedTasksOfType((FollowerTaskType) currentTaskType);
    FollowerTask task = (FollowerTask) null;
    if (unoccupiedTasksOfType.Count > 0)
      task = unoccupiedTasksOfType[UnityEngine.Random.Range(0, unoccupiedTasksOfType.Count)];
    if (task != null)
      task.ClaimReservations();
    follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    double num1 = (double) follower.SetBodyAnimation("devotion/devotion-start", false);
    follower.AddBodyAnimation("devotion/devotion-waiting", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
    follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Bless, (System.Action) (() =>
    {
      CultFaithManager.AddThought(Thought.Cult_Bless, follower.Brain.Info.ID);
      double num2 = (double) follower.SetBodyAnimation("idle", true);
      if (task != null)
        follower.Brain.HardSwapToTask(task);
      else
        follower.Brain.CompleteCurrentTask();
    }));
  }

  public IEnumerator IntimidateFollower(Follower follower)
  {
    PlayerFarming playerFarming = this;
    int currentTaskType = (int) follower.Brain.CurrentTaskType;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.Brain.Stats.Intimidated = true;
    follower.FacePosition(playerFarming.transform.position);
    List<FollowerTask> unoccupiedTasksOfType = FollowerBrain.GetAllUnoccupiedTasksOfType((FollowerTaskType) currentTaskType);
    FollowerTask task = (FollowerTask) null;
    if (unoccupiedTasksOfType.Count > 0)
      task = unoccupiedTasksOfType[UnityEngine.Random.Range(0, unoccupiedTasksOfType.Count)];
    if (task != null)
      task.ClaimReservations();
    follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    double num = (double) follower.SetBodyAnimation("Reactions/react-intimidate", false);
    follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.75f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
    follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Intimidate, (System.Action) (() =>
    {
      follower.Brain.AddThought(Thought.Intimidated);
      if (task != null)
        follower.Brain.HardSwapToTask(task);
      else
        follower.Brain.CompleteCurrentTask();
    }));
  }

  public IEnumerator InspireFollower(Follower follower)
  {
    PlayerFarming playerFarming = this;
    int currentTaskType = (int) follower.Brain.CurrentTaskType;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.Brain.Stats.Inspired = true;
    follower.FacePosition(playerFarming.transform.position);
    List<FollowerTask> unoccupiedTasksOfType = FollowerBrain.GetAllUnoccupiedTasksOfType((FollowerTaskType) currentTaskType);
    FollowerTask task = (FollowerTask) null;
    if (unoccupiedTasksOfType.Count > 0)
      task = unoccupiedTasksOfType[UnityEngine.Random.Range(0, unoccupiedTasksOfType.Count)];
    if (task != null)
      task.ClaimReservations();
    follower.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    double num1 = (double) follower.SetBodyAnimation("dance", true);
    follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BlessAFollower);
    follower.Brain.AddAdoration(FollowerBrain.AdorationActions.Inspire, (System.Action) (() =>
    {
      CultFaithManager.AddThought(Thought.Cult_Inspire);
      double num2 = (double) follower.SetBodyAnimation("idle", true);
      if (task != null)
        follower.Brain.HardSwapToTask(task);
      else
        follower.Brain.CompleteCurrentTask();
    }));
  }

  public static event PlayerFarming.GoToEvent OnGoToAndStopBegin;

  public void GoToAndStop(
    GameObject TargetPosition,
    GameObject LookToObject = null,
    bool IdleOnEnd = false,
    bool DisableCollider = false,
    System.Action GoToCallback = null,
    float maxDuration = 20f,
    bool forcePositionOnTimeout = false,
    System.Action AbortGoToCallback = null)
  {
    this.GoToAndStop(TargetPosition.transform.position, LookToObject, IdleOnEnd, DisableCollider, GoToCallback, maxDuration, forcePositionOnTimeout, AbortGoToCallback);
  }

  public static void SetStateForAllPlayers(
    StateMachine.State targetState = StateMachine.State.Idle,
    bool abortGoto = false,
    PlayerFarming PlayerNotToInclude = null)
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((UnityEngine.Object) player != (UnityEngine.Object) PlayerNotToInclude)
      {
        player.state.CURRENT_STATE = targetState;
        if (abortGoto)
          player.AbortGoTo();
      }
    }
  }

  public static bool AnyPlayerGotoAndStopping()
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if (PlayerFarming.players[index].GoToAndStopping)
        return true;
    }
    return false;
  }

  public static bool AnyPlayerInConversation()
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if (PlayerFarming.players[index].wasInConversation)
        return true;
    }
    return false;
  }

  public static bool AnyPlayerHasLegendaryWeapon()
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if (PlayerFarming.players[index].playerWeapon.IsLegendaryWeapon())
        return true;
    }
    return false;
  }

  public static bool AnyPlayerHasLegendaryWeapon(EquipmentType legendaryWeapon)
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if (PlayerFarming.players[index].playerWeapon.IsLegendaryWeapon() && PlayerFarming.players[index].currentWeapon == legendaryWeapon)
        return true;
    }
    return false;
  }

  public static void SetCollidersActive(bool collidersActive, bool checkBounds = false)
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming.players[index].circleCollider2D.enabled = collidersActive;
      if (GameManager.IsDungeon(PlayerFarming.Location) & checkBounds && PlayerFarming.Location != FollowerLocation.IntroDungeon)
      {
        Vector3 closestPoint = PlayerFarming.players[index].transform.position;
        Vector3 normalized = closestPoint.normalized;
        if (!BiomeGenerator.PointWithinIsland(PlayerFarming.players[index].transform.position, out closestPoint))
          PlayerFarming.players[index].transform.position = closestPoint - normalized;
      }
    }
  }

  public static void SetFacingAngleForAll(float facingAngle)
  {
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
      PlayerFarming.players[index].state.facingAngle = facingAngle;
  }

  public bool IsInCameraFollower()
  {
    return GameManager.GetInstance().CameraContains(this.CameraBone) || GameManager.GetInstance().CameraContains(this.gameObject);
  }

  public void GoToAndStop(
    Vector3 TargetPosition,
    GameObject LookToObject = null,
    bool IdleOnEnd = false,
    bool DisableCollider = false,
    System.Action GoToCallback = null,
    float maxDuration = 20f,
    bool forcePositionOnTimeout = false,
    System.Action AbortGoToCallback = null,
    bool groupAction = false,
    bool groupAbortCurrentGoto = true,
    bool groupLeader = true,
    bool forceAstar = false,
    Vector3? forcedOtherPosition = null)
  {
    this.playerController.xDir = 0.0f;
    this.playerController.yDir = 0.0f;
    this.unitObject.vx = 0.0f;
    this.unitObject.vy = 0.0f;
    this.unitObject.ClearPaths();
    this.GoToAndStopping = true;
    this.LookToObject = LookToObject;
    this.IdleOnEnd = IdleOnEnd;
    this.GoToCallback = GoToCallback;
    this.AbortGoToCallback = AbortGoToCallback;
    this.maxDuration = maxDuration;
    this.forcePositionOnTimeout = forcePositionOnTimeout;
    this.forcePositionOnTimeoutTarget = TargetPosition;
    this.startMoveTimestamp = Time.unscaledTime;
    this.lastPosition = this.transform.position;
    this.stillTime = 0.0f;
    this.isIdle = false;
    if (DisableCollider)
      this.GetComponent<CircleCollider2D>().enabled = false;
    this.unitObject.givePath(TargetPosition, forceAStar: forceAstar);
    this.unitObject.EndOfPath += new System.Action(this.EndGoToAndStop);
    PlayerFarming.GoToEvent goToAndStopBegin = PlayerFarming.OnGoToAndStopBegin;
    if (goToAndStopBegin != null)
      goToAndStopBegin(TargetPosition);
    if (!(groupAction & groupLeader))
      return;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((UnityEngine.Object) player != (UnityEngine.Object) this)
      {
        if (groupAbortCurrentGoto)
          player.AbortGoTo();
        Vector3? nullable = !forcedOtherPosition.HasValue ? new Vector3?(TargetPosition + new Vector3(0.0f, -1f, 0.0f)) : forcedOtherPosition;
        player.GoToAndStop(nullable.Value, LookToObject, IdleOnEnd, DisableCollider, maxDuration: maxDuration, forcePositionOnTimeout: forcePositionOnTimeout, AbortGoToCallback: AbortGoToCallback, groupAbortCurrentGoto: false, groupLeader: false, forceAstar: forceAstar);
      }
    }
  }

  public void EndGoToAndStop()
  {
    this.GoToAndStopping = false;
    this.unitObject.ClearPaths();
    this.state.CURRENT_STATE = this.IdleOnEnd ? StateMachine.State.Idle : StateMachine.State.InActive;
    if ((UnityEngine.Object) this.LookToObject != (UnityEngine.Object) null)
      this.state.facingAngle = Utils.GetAngle(this.transform.position, this.LookToObject.transform.position);
    System.Action goToCallback = this.GoToCallback;
    if (goToCallback != null)
      goToCallback();
    this.GoToCallback = (System.Action) null;
    this.AbortGoToCallback = (System.Action) null;
    this.GetComponent<CircleCollider2D>().enabled = true;
    this.unitObject.EndOfPath -= new System.Action(this.EndGoToAndStop);
  }

  public void AbortGoTo(bool InvokeAbortCallback = true)
  {
    this.GoToAndStopping = false;
    this.unitObject.ClearPaths();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    if ((UnityEngine.Object) this.LookToObject != (UnityEngine.Object) null)
      this.state.facingAngle = Utils.GetAngle(this.transform.position, this.LookToObject.transform.position);
    this.GetComponent<CircleCollider2D>().enabled = true;
    if (InvokeAbortCallback)
    {
      System.Action abortGoToCallback = this.AbortGoToCallback;
      if (abortGoToCallback != null)
        abortGoToCallback();
    }
    this.GoToCallback = (System.Action) null;
    this.AbortGoToCallback = (System.Action) null;
    this.unitObject.EndOfPath -= new System.Action(this.EndGoToAndStop);
  }

  public void SetInactive()
  {
    if (this.GoToAndStopping)
    {
      this.ClearInactiveLeftover();
      this.unitObject.EndOfPath += new System.Action(this.InactiveOnArrival);
      this.AbortGoToCallback += new System.Action(this.ClearInactiveLeftover);
    }
    else
    {
      this.StopRidingOnAnimal();
      this.state.CURRENT_STATE = StateMachine.State.InActive;
    }
  }

  public void InactiveOnArrival()
  {
    this.ClearInactiveLeftover();
    this.state.CURRENT_STATE = StateMachine.State.InActive;
  }

  public void ClearInactiveLeftover()
  {
    this.unitObject.EndOfPath -= new System.Action(this.InactiveOnArrival);
    this.AbortGoToCallback -= new System.Action(this.ClearInactiveLeftover);
  }

  public void EnableDamageOnTouchCollider(float duration)
  {
    this.damageOnTouchTimer = duration;
    this.PlayerDamageCollider.SetActive(true);
  }

  public EquipmentType currentWeapon
  {
    get => this._currentWeapon;
    set => this._currentWeapon = value;
  }

  public EquipmentType currentCurse
  {
    get => this._currentCurse;
    set => this._currentCurse = value;
  }

  public static void EveryoneJoinPosition(PlayerFarming playerFarming)
  {
    for (int index = 0; index < PlayerFarming.playersCount && !((UnityEngine.Object) PlayerFarming.players[index] == (UnityEngine.Object) playerFarming); ++index)
      PlayerFarming.players[index].GoToAndStop(playerFarming.transform.position, playerFarming.LookToObject, playerFarming.IdleOnEnd, playerFarming.circleCollider2D.enabled, forcePositionOnTimeout: true, groupLeader: false);
  }

  public static float GetClosestPlayerDist(Vector3 pos)
  {
    float closestPlayerDist = float.MaxValue;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      float num = Vector3.Distance(PlayerFarming.players[index].transform.position, pos);
      if ((double) num < (double) closestPlayerDist)
        closestPlayerDist = num;
    }
    return closestPlayerDist;
  }

  public static PlayerFarming GetClosestPlayer(Vector3 pos, bool ignoreDownedPlayers = false)
  {
    float num1 = float.MaxValue;
    PlayerFarming closestPlayer = (PlayerFarming) null;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      float num2 = Vector3.Distance(PlayerFarming.players[index].transform.position, pos);
      bool isKnockedOut = PlayerFarming.players[index].IsKnockedOut;
      bool flag = !ignoreDownedPlayers || !isKnockedOut;
      if ((double) num2 < (double) num1 & flag)
      {
        num1 = num2;
        closestPlayer = PlayerFarming.players[index];
      }
    }
    return closestPlayer;
  }

  public void Update()
  {
    if (!this.isLamb && this.rewiredPlayer != null && this.rewiredPlayer.controllers.joystickCount == 0 && !this.rewiredPlayer.controllers.hasKeyboard)
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = PlayerFarming.players[0];
    if ((double) Time.timeScale <= 0.0)
      return;
    if ((bool) (UnityEngine.Object) this.InstanceMarker)
      this.InstanceMarker.SetActive(false);
    if (this.rewiredPlayer == null && !this.EnableCoopFeatures)
      this.rewiredPlayer = RewiredInputManager.GetPlayer(0);
    if (this.isLamb)
    {
      if ((bool) (UnityEngine.Object) PlayerFarming.Instance.gameObject)
        CompanionCrusade.RefreshParentsAndPositions(PlayerFarming.Instance.gameObject);
      Shader.SetGlobalVector("_GlobalFirstPlayerPos", (Vector4) this.transform.position);
      if (PlayerFarming.players.Count > 1)
        Shader.SetGlobalVector("_GlobalSecondPlayerPos", (Vector4) PlayerFarming.players[1].transform.position);
    }
    else if (!this.isLamb && this.rewiredPlayer != null && this.rewiredPlayer.controllers.joystickCount == 0 && !this.rewiredPlayer.controllers.hasKeyboard && !CoopManager.Instance.temporaryDisableRemoval)
      CoopManager.RemovePlayerFromMenu();
    this.CoopIndicatorLamb.gameObject.SetActive(this.isLamb && CoopManager.CoopActive && SettingsManager.Settings.Accessibility.CoopVisualIndicators && this.isCoopIndicatorVisible);
    this.CoopIndicatorGoat.gameObject.SetActive(!this.isLamb && CoopManager.CoopActive && SettingsManager.Settings.Accessibility.CoopVisualIndicators && this.isCoopIndicatorVisible);
    if (!LetterBox.IsPlaying && !MMConversation.isPlaying)
    {
      CameraFollowTarget camFollowTarget = GameManager.GetInstance().CamFollowTarget;
      if (camFollowTarget.targets.Count <= 1 && camFollowTarget.ContainsAnyPlayer())
      {
        float Weight = 1f;
        if (!this.isLamb)
          Weight = 0.1f;
        camFollowTarget.AddTarget(this.CameraBone, Weight);
      }
      if ((UnityEngine.Object) this == (UnityEngine.Object) PlayerFarming.Instance)
      {
        if (PlayerFarming.playersCount > 1)
        {
          PlayerFarming.AveragePlayerPosition = Vector3.zero;
          for (int index = 0; index < PlayerFarming.playersCount; ++index)
            PlayerFarming.AveragePlayerPosition += PlayerFarming.players[index].transform.position;
          PlayerFarming.AveragePlayerPosition /= (float) PlayerFarming.playersCount;
        }
        else
          PlayerFarming.AveragePlayerPosition = PlayerFarming.Instance.transform.position;
      }
      else if (!this.IsKnockedOut && CoopManager.CoopActive && camFollowTarget.targets.Count == 1 && camFollowTarget.ContainsAnyPlayer())
        GameManager.GetInstance().AddToCamera(this.CameraBone);
    }
    else if (PlayerFarming.playersCount > 1)
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
        PlayerFarming.AveragePlayerPosition += PlayerFarming.players[index].transform.position;
      PlayerFarming.AveragePlayerPosition /= (float) PlayerFarming.playersCount;
    }
    else
      PlayerFarming.AveragePlayerPosition = PlayerFarming.Instance.transform.position;
    if (this.state.CURRENT_STATE == StateMachine.State.Dodging)
    {
      if (!this.createdExplosion && PlayerFarming.playersCount > 1 && TrinketManager.HasTrinket(TarotCards.Card.CoopExplosive) && (double) Vector3.Distance(PlayerFarming.players[0].transform.position, PlayerFarming.players[1].transform.position) < 1.0 && PlayerFarming.players[0].state.CURRENT_STATE == StateMachine.State.Dodging && PlayerFarming.players[1].state.CURRENT_STATE == StateMachine.State.Dodging)
      {
        if ((double) UnityEngine.Random.value < 0.20000000298023224)
          Explosion.CreateExplosion((PlayerFarming.players[0].transform.position + PlayerFarming.players[1].transform.position) / 2f, Health.Team.PlayerTeam, (Health) this.health, 2f);
        this.createdExplosion = true;
      }
    }
    else
      this.createdExplosion = false;
    this.coopDamageMultiplierEffectEmission.rateOverTimeMultiplier = !GameManager.IsDungeon(PlayerFarming.Location) || !TarotCards.IsWithinDistanceForBetterApart() && !TarotCards.IsWithinDistanceForBetterTogether() ? 0.0f : 1f;
    this.coopDamageMultiplierEffectEmission.rateOverTime = (ParticleSystem.MinMaxCurve) (4f * this.coopDamageMultiplierEffectEmission.rateOverTimeMultiplier);
    this.coopBeam.gameObject.SetActive(TrinketManager.HasTrinket(TarotCards.Card.CoopBonded) && PlayerFarming.playersCount > 1 && this.isLamb);
    if (this.coopBeam.gameObject.activeSelf)
    {
      if (PlayerFarming.players[0].IsKnockedOut || PlayerFarming.players[1].IsKnockedOut)
      {
        this.coopBeam.gameObject.SetActive(false);
      }
      else
      {
        Vector3 vector3 = PlayerFarming.players[0].transform.position + (PlayerFarming.players[1].transform.position - PlayerFarming.players[0].transform.position) / 2f;
        float num = Vector3.Distance(PlayerFarming.players[0].transform.position, PlayerFarming.players[1].transform.position);
        this.coopBeam.transform.position = vector3;
        this.coopBeam.transform.LookAt(PlayerFarming.players[1].transform.position, Vector3.forward);
        this.coopBeamContainer.localScale = this.coopBeamContainer.localScale with
        {
          x = num * 0.8f
        };
      }
    }
    this.damageOnTouchTimer -= Time.deltaTime;
    if (this.state.CURRENT_STATE == StateMachine.State.Dodging && TrinketManager.DamageEnemyOnRoll(this))
    {
      if (!this.PlayerDamageCollider.isActiveAndEnabled)
        AudioManager.Instance.PlayOneShot("event:/player/damage_roll", this.transform.position);
      this.PlayerDamageCollider.SetActive(true);
    }
    else
      this.PlayerDamageCollider.SetActive((double) this.damageOnTouchTimer > 0.0);
    if (this.state.CURRENT_STATE != StateMachine.State.Dodging && TrinketManager.HasTrinket(TarotCards.Card.HighRoller, this))
      this.HighRollerVFX.Stop();
    if (TrinketManager.HasTrinket(TarotCards.Card.BlackSoulAutoRecharge, this) && !TrinketManager.IsOnCooldown(TarotCards.Card.BlackSoulAutoRecharge, this) && (double) this.playerSpells.faithAmmo.Ammo < (double) this.playerSpells.faithAmmo.Total && !HUD_Manager.Instance.Hidden && (double) this.playerSpells.faithAmmo.Ammo < (double) this.playerSpells.faithAmmo.Total)
    {
      this.playerSpells.faithAmmo.DoFlash = false;
      this.GetBlackSoul(giveXp: false, playSound: !PlayerFleeceManager.FleeceSwapsWeaponForCurse());
      this.playerSpells.faithAmmo.DoFlash = true;
      TrinketManager.TriggerCooldown(TarotCards.Card.BlackSoulAutoRecharge, this);
    }
    this.Spine.timeScale = this.state.CURRENT_STATE == StateMachine.State.Attacking ? this.Spine.timeScale : 1f;
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      if (this.Meditating && (this.state.CURRENT_STATE != StateMachine.State.Meditate || this.GoToAndStopping))
      {
        this.Meditating = false;
        if ((double) Time.timeScale == 3.0)
          Time.timeScale = 1f;
        if (this.state.CURRENT_STATE != StateMachine.State.InActive)
          this.state.CURRENT_STATE = StateMachine.State.Idle;
        GameManager instance = GameManager.GetInstance();
        if ((UnityEngine.Object) instance == (UnityEngine.Object) null || !instance.CamFollowTarget.IN_CONVERSATION)
          GameManager.GetInstance().CameraResetTargetZoom();
        this.Spine.UseDeltaTime = true;
        this.indicator.Activate();
      }
      if (this.state.CURRENT_STATE == StateMachine.State.Meditate)
      {
        if (InputManager.Gameplay.GetMeditateButtonHeld(this) && !this.GoToAndStopping)
          return;
        this.Meditating = false;
        Time.timeScale = 1f;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        GameManager.GetInstance().CameraResetTargetZoom();
        this.Spine.UseDeltaTime = true;
        this.indicator.Activate();
        return;
      }
      if ((UnityEngine.Object) this.state != (UnityEngine.Object) null && !this.BlockMeditation && !this.GoToAndStopping && this.gameObject.activeSelf && InputManager.Gameplay.GetMeditateButtonDown(this) && this.state.CURRENT_STATE != StateMachine.State.Meditate && this.state.CURRENT_STATE != StateMachine.State.Idle_CarryingBody && this.state.CURRENT_STATE != StateMachine.State.Moving_CarryingBody && this.state.CURRENT_STATE != StateMachine.State.CustomAction0 && this.state.CURRENT_STATE != StateMachine.State.InActive && !this.CarryingSnowball && !LetterBox.IsPlaying && !this.IsRidingAnimal())
      {
        this.Meditating = true;
        this.state.CURRENT_STATE = StateMachine.State.Meditate;
        GameManager.GetInstance().CameraSetTargetZoom(15f);
        this.Spine.UseDeltaTime = false;
        bool flag = true;
        foreach (PlayerFarming player in PlayerFarming.players)
        {
          if (player.state.CURRENT_STATE != StateMachine.State.Meditate)
            flag = false;
        }
        if (flag)
          Time.timeScale = 3f;
      }
    }
    else if (this.state.CURRENT_STATE == StateMachine.State.Meditate && !GameManager.IsDungeon(PlayerFarming.Location))
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    if (this.GoToAndStopping)
    {
      if ((UnityEngine.Object) this.PickedUpFollower != (UnityEngine.Object) null && this.state.CURRENT_STATE != StateMachine.State.TimedAction)
        this.UpdatePickedUpFollower();
      if (this.state.CURRENT_STATE != StateMachine.State.TimedAction)
        this.state.CURRENT_STATE = StateMachine.State.Moving;
      Vector3 position = this.transform.position;
      if ((double) (position - this.lastPosition).sqrMagnitude > 0.0025000001769512892)
      {
        this.stillTime = 0.0f;
        if (this.isIdle)
          this.isIdle = false;
        this.lastPosition = position;
      }
      else
      {
        this.stillTime += Time.deltaTime;
        if (!this.isIdle && (double) this.stillTime >= 1.0)
          this.isIdle = true;
      }
      float unscaledTime = Time.unscaledTime;
      if (((double) this.maxDuration == -1.0 || (double) unscaledTime <= (double) this.startMoveTimestamp + (double) this.maxDuration) && !this.isIdle)
        return;
      this.EndGoToAndStop();
      if (!this.forcePositionOnTimeout)
        return;
      Debug.LogWarning((object) "Player GotoAndStop Timed out, placing player on end point");
      this.transform.position = this.forcePositionOnTimeoutTarget;
    }
    else
    {
      this.TryDropSnowball();
      if (this.state.CURRENT_STATE == StateMachine.State.Map || this.state.CURRENT_STATE == StateMachine.State.Heal || this.state.CURRENT_STATE == StateMachine.State.CustomAnimation || this.state.CURRENT_STATE == StateMachine.State.Grabbed || this.state.CURRENT_STATE == StateMachine.State.DashAcrossIsland || this.state.CURRENT_STATE == StateMachine.State.Grapple || this.state.CURRENT_STATE == StateMachine.State.SpawnIn || this.state.CURRENT_STATE == StateMachine.State.Respawning || this.state.CURRENT_STATE == StateMachine.State.Dieing || this.state.CURRENT_STATE == StateMachine.State.Dead || this.state.CURRENT_STATE == StateMachine.State.Converting || this.state.CURRENT_STATE == StateMachine.State.InActive || this.state.CURRENT_STATE == StateMachine.State.Building || this.state.CURRENT_STATE == StateMachine.State.Unconverted || this.state.CURRENT_STATE == StateMachine.State.FoundItem || this.state.CURRENT_STATE == StateMachine.State.GameOver || this.state.CURRENT_STATE == StateMachine.State.FinalGameOver || !this.gameObject.activeSelf)
        return;
      if (this.state.CURRENT_STATE != StateMachine.State.Dodging)
        this.DodgeDelay -= Time.deltaTime;
      if (this.state.CURRENT_STATE == StateMachine.State.HitThrown || this.state.CURRENT_STATE == StateMachine.State.Casting || this.state.CURRENT_STATE == StateMachine.State.HitRecover)
      {
        if (this.state.CURRENT_STATE != StateMachine.State.HitRecover && this.state.CURRENT_STATE != StateMachine.State.Casting && (this.state.CURRENT_STATE != StateMachine.State.HitThrown || (double) this.playerController.HitTimer <= 0.34999999403953552))
          return;
        this.DodgeRoll();
      }
      else
      {
        this.ArrowAttackDelay -= Time.deltaTime;
        if (this.state.CURRENT_STATE == StateMachine.State.InActive || this.state.CURRENT_STATE == StateMachine.State.TimedAction || this.state.CURRENT_STATE == StateMachine.State.SignPostAttack || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack)
          return;
        if ((UnityEngine.Object) this.PickedUpFollower != (UnityEngine.Object) null)
        {
          if (this.state.CURRENT_STATE == StateMachine.State.TimedAction)
            return;
          this.UpdatePickedUpFollower();
        }
        else if (this.simpleInventory.Item != InventoryItem.ITEM_TYPE.NONE)
        {
          if (!InputManager.Gameplay.GetAttackButtonHeld(this))
            return;
          this.simpleInventory.DropItem();
        }
        else
        {
          switch (Inventory.CURRENT_WEAPON)
          {
            case 3:
              this.WaterPlants();
              this.WateringCanAim.transform.localScale = new Vector3(this.WateringCanScale, this.WateringCanScale);
              break;
          }
          this.DodgeRoll();
          if (this.state.CURRENT_STATE == StateMachine.State.Inventory && !this.InventoryMenu.gameObject.activeSelf)
            this.state.CURRENT_STATE = StateMachine.State.Idle;
          if (this.CarryingSnowball && this.state.CURRENT_STATE != StateMachine.State.Dodging)
          {
            if (InputManager.Gameplay.GetInteractButtonHeld(this) && this.state.CURRENT_STATE != StateMachine.State.ChargingSnowball)
            {
              this.snowballAimInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup("event:/dlc/material/snowball_aim_start", this.transform);
              this.state.CURRENT_STATE = StateMachine.State.ChargingSnowball;
            }
            else if (!InputManager.Gameplay.GetInteractButtonHeld(this) && this.state.CURRENT_STATE == StateMachine.State.ChargingSnowball)
              this.StartCoroutine((IEnumerator) this.ThrowSnowball());
            if (this.state.CURRENT_STATE == StateMachine.State.ChargingSnowball)
              this.ChargeSnowball();
          }
          if (!this.CarryingSnowball || !InputManager.Gameplay.GetInteractButtonUp(this))
            return;
          AudioManager.Instance.StopOneShotInstanceEarly(this.snowballAimInstance, STOP_MODE.ALLOWFADEOUT);
        }
      }
    }
  }

  public void LateUpdate() => this.PreviousPosition = this.transform.position;

  public static void FindAveragePlayerPosition()
  {
    PlayerFarming.AveragePlayerPosition = Vector3.zero;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
      PlayerFarming.AveragePlayerPosition += PlayerFarming.players[index].transform.position;
    PlayerFarming.AveragePlayerPosition /= (float) PlayerFarming.playersCount;
  }

  public void ShowProjectileChargeBars()
  {
    int num = 1;
    bool requiresCharging = false;
    if (EquipmentManager.GetCurseData(this.currentCurse).PrimaryEquipmentType == EquipmentType.Fireball || EquipmentManager.GetCurseData(this.currentCurse).PrimaryEquipmentType == EquipmentType.MegaSlash)
      requiresCharging = true;
    if (EquipmentManager.GetCurseData(this.currentCurse).EquipmentType == EquipmentType.Fireball_Swarm)
      requiresCharging = false;
    if (this.currentCurse == EquipmentType.Tentacles_Circular)
      num = 4;
    for (int index = 0; index < num; ++index)
      this.CurseChargeBars[index].ShowProjectileCharge(requiresCharging);
  }

  public static void SetMainPlayer(Collider2D collider2D)
  {
    PlayerFarming.SetMainPlayer(collider2D.GetComponent<PlayerFarming>());
  }

  public static void SetMainPlayer(StateMachine state)
  {
    PlayerFarming.SetMainPlayer(state.GetComponent<PlayerFarming>());
  }

  public static void SetMainPlayer(PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) PlayerFarming.Instance)
    {
      Debug.Log((object) "Target player is already Instance");
    }
    else
    {
      Debug.Log((object) $"{playerFarming.name} Setting main player at {Time.realtimeSinceStartup.ToString()}");
      PlayerFarming.Instance = playerFarming;
      PlayerFarming.RefreshPlayersCount(false);
    }
  }

  public static void ResetMainPlayer()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((bool) (UnityEngine.Object) player.playerWeapon)
        player.playerWeapon.DoingHeavyAttack = false;
    }
    if (PlayerFarming.players.Count == 0)
      return;
    if ((UnityEngine.Object) PlayerFarming.players[0] == (UnityEngine.Object) PlayerFarming.Instance)
    {
      Debug.Log((object) "Resetting Instance player is already Instance");
    }
    else
    {
      Debug.Log((object) $"{PlayerFarming.Instance.name} is main player when reset at {Time.realtimeSinceStartup.ToString()}");
      PlayerFarming.Instance = PlayerFarming.players[0];
    }
  }

  public void HideProjectileChargeBars()
  {
    foreach (CurseChargeBar curseChargeBar in this.CurseChargeBars)
      curseChargeBar.HideProjectileCharge();
  }

  public void ShowHeavyAttackProjectileChargeBars(float size = 1.5f)
  {
    for (int index = 0; index < this.HeavyChargeBars.Count; ++index)
      this.HeavyChargeBars[index].ShowProjectileCharge(false, size);
  }

  public void UpdateHeavyChargeBar(float fillAmount)
  {
    foreach (CurseChargeBar heavyChargeBar in this.HeavyChargeBars)
      heavyChargeBar.UpdateProjectileChargeBar(fillAmount);
  }

  public void HideHeavyChargeBars()
  {
    foreach (CurseChargeBar heavyChargeBar in this.HeavyChargeBars)
      heavyChargeBar.HideProjectileCharge();
  }

  public void SetAimingRecticuleScaleAndRotation(int chargeBarIndex, Vector3 scale, Vector3 euler)
  {
    this.CurseChargeBars[chargeBarIndex].SetAimingRecticuleScaleAndRotation(scale, euler);
  }

  public void SetHeavyAimingRecticuleScaleAndRotation(
    int chargeBarIndex,
    Vector3 scale,
    Vector3 euler)
  {
    this.HeavyChargeBars[chargeBarIndex].SetAimingRecticuleScaleAndRotation(scale, euler);
  }

  public void ShowWeaponChargeBars(float size = 1.5f)
  {
    int num = 1;
    for (int index = 0; index < num; ++index)
      this.WeaponChargeBars[index].ShowProjectileCharge(false, size);
  }

  public void HideWeaponChargeBars()
  {
    foreach (CurseChargeBar weaponChargeBar in this.WeaponChargeBars)
      weaponChargeBar.HideProjectileCharge();
  }

  public void SetWeaponAimingRecticuleScaleAndRotation(
    int chargeBarIndex,
    Vector3 scale,
    Vector3 euler)
  {
    this.WeaponChargeBars[chargeBarIndex].SetAimingRecticuleScaleAndRotation(scale, euler);
  }

  public void UpdateProjectileChargeBar(float fillAmount)
  {
    foreach (CurseChargeBar curseChargeBar in this.CurseChargeBars)
      curseChargeBar.UpdateProjectileChargeBar(fillAmount);
  }

  public bool CorrectProjectileChargeRelease()
  {
    return this.CurseChargeBars[0].CorrectProjectileChargeRelease();
  }

  public void UpdatePickedUpFollower()
  {
    this.PickedUpFollower.transform.position = Vector3.Lerp(this.PickedUpFollower.transform.position, this.CarryBone.position with
    {
      y = this.transform.position.y
    } + new Vector3(1f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 1f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), 0.0f), 15f * Time.deltaTime);
    this.PickedUpFollower.State.facingAngle = Utils.GetAngle(this.transform.position, this.PickedUpFollower.transform.position);
  }

  public void ChangeTool(int PrevTool, int NewTool)
  {
    if (PrevTool == 3)
    {
      this.Spine.AnimationState.ClearTrack(1);
      this.WateringCanScale = 0.0f;
      this.WateringCanAim.transform.position = this.transform.position;
      this.WateringCanAim.SetActive(false);
    }
    switch (NewTool)
    {
      case 1:
        this.Spine.skeleton.SetAttachment("TOOLS", "SPADE");
        break;
      case 2:
        this.Spine.skeleton.SetAttachment("TOOLS", "SEED_BAG");
        break;
      case 3:
        this.Spine.skeleton.SetAttachment("TOOLS", "WATERING_CAN");
        this.WateringCanAim.SetActive(true);
        break;
    }
  }

  public void Cook() => this.TimedAction(2f, new System.Action(this.CookFood));

  public void CookFood() => this.simpleInventory.GiveItem(InventoryItem.ITEM_TYPE.MEAT);

  public void EatFood()
  {
    this.TimedAction(1f, (System.Action) null);
    this.simpleInventory.RemoveItem();
  }

  public void WaterPlants()
  {
    Vector3 vector3 = new Vector3(0.8f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.8f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    if (InputManager.Gameplay.GetAttackButtonHeld(this))
    {
      this.Spine.AnimationState.SetAnimation(1, "Farming-water_track", true);
      this.WateringCanAim.transform.position = Vector3.Lerp(this.WateringCanAim.transform.position, this.transform.position + vector3, 3.5f * Time.deltaTime);
      this.WateringCanScaleSpeed += (float) ((1.0 - (double) this.WateringCanScale) * 0.20000000298023224);
      this.WateringCanScale += (this.WateringCanScaleSpeed *= 0.7f) + 0.01f * Mathf.Cos(this.WateringCanBob += 0.2f);
      foreach (Crop crop in Crop.Crops)
      {
        if ((double) Vector3.Distance(this.WateringCanAim.transform.position, crop.transform.position) < 0.75)
          crop.DoWork(1f * Time.deltaTime);
      }
      foreach (Fire fire in Fire.Fires)
      {
        if ((double) Vector3.Distance(this.WateringCanAim.transform.position, fire.transform.position) < 1.25)
          fire.DoWork(1f * Time.deltaTime);
      }
    }
    else
    {
      this.WateringCanAim.transform.position = Vector3.Lerp(this.WateringCanAim.transform.position, this.transform.position, 3f * Time.deltaTime);
      if ((double) this.WateringCanScale > 0.0)
      {
        this.WateringCanScale -= 0.1f * GameManager.DeltaTime;
        if ((double) this.WateringCanScale <= 0.0)
          this.WateringCanScale = 0.0f;
      }
      this.Spine.AnimationState.ClearTrack(1);
    }
    this.WateringCanAim.transform.localScale = new Vector3(this.WateringCanScale, this.WateringCanScale);
  }

  public void TimedAction(float Duration, System.Action Callback, string SpineAnimation = "action")
  {
    this.playerController.TimedActionCallback = Callback;
    this.state.CURRENT_STATE = StateMachine.State.TimedAction;
    this.state.Timer = Duration;
    this.Spine.AnimationState.SetAnimation(0, SpineAnimation, true);
  }

  public PlayerWeapon playerWeapon
  {
    get
    {
      if ((UnityEngine.Object) this._playerWeapon == (UnityEngine.Object) null)
        this._playerWeapon = this.GetComponent<PlayerWeapon>();
      return this._playerWeapon;
    }
  }

  public PlayerSpells playerSpells
  {
    get
    {
      if ((UnityEngine.Object) this._playerSpells == (UnityEngine.Object) null)
      {
        this._playerSpells = this.GetComponent<PlayerSpells>();
        this._playerSpells.Init();
      }
      return this._playerSpells;
    }
  }

  public PlayerRelic playerRelic
  {
    get
    {
      if ((UnityEngine.Object) this._playerRelic == (UnityEngine.Object) null)
        this._playerRelic = this.GetComponent<PlayerRelic>();
      return this._playerRelic;
    }
  }

  public bool DodgeRoll()
  {
    if (!this.AllowDodging || this.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody || this.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody || this.state.CURRENT_STATE == StateMachine.State.WeaponSelect || this.state.CURRENT_STATE == StateMachine.State.Meditate || this.state.CURRENT_STATE == StateMachine.State.Dead || this.state.CURRENT_STATE == StateMachine.State.ChargingSnowball)
      return false;
    bool abilityButtonHeld = InputManager.Gameplay.GetFleeceAbilityButtonHeld(this);
    bool abilityButtonDown = InputManager.Gameplay.GetFleeceAbilityButtonDown(this);
    if (abilityButtonHeld)
      this.AbilityKeyDown = true;
    bool bleatButtonUp = InputManager.Gameplay.GetBleatButtonUp(this);
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
      case StateMachine.State.Idle_Winter:
      case StateMachine.State.Moving_Winter:
      case StateMachine.State.ChargingSnowball:
        if (this.AbilityKeyDown && GameManager.IsDungeon(PlayerFarming.Location) && (double) (this.AbilityKeyDownAccumulated += Time.deltaTime) > 0.40000000596046448 && PlayerFleeceManager.BleatToHeal() && !bleatButtonUp)
        {
          if (this.playerAbility.CanHeal())
            this.playerAbility.DoHealRoutine();
          else
            this.AbilityKeyDown = false;
          return false;
        }
        if (abilityButtonDown && GameManager.IsDungeon(PlayerFarming.Location) && !bleatButtonUp && PlayerFleeceManager.BleatToBurrow())
        {
          if (this.playerAbility.CanBurrow())
          {
            this.playerAbility.DoBurrowRoutine();
            break;
          }
          this.AbilityKeyDown = false;
          break;
        }
        if (InputManager.Gameplay.GetBleatButtonDown(this) && !this.IsKnockedOut && (!GameManager.IsDungeon(PlayerFarming.Location) || ((!GameManager.IsDungeon(PlayerFarming.Location) ? 0 : (Health.team2.Count <= 0 ? 1 : 0)) & (bleatButtonUp ? 1 : 0)) != 0))
        {
          this.StartCoroutine((IEnumerator) this.BleatRoutine());
          return false;
        }
        break;
      case StateMachine.State.Heal:
        if (!abilityButtonHeld)
        {
          this.playerAbility.DoEndHealRoutine();
          return false;
        }
        break;
    }
    if (!abilityButtonHeld)
    {
      this.AbilityKeyDownAccumulated = 0.0f;
      this.AbilityKeyDown = false;
    }
    if ((!((UnityEngine.Object) this.playerWeapon != (UnityEngine.Object) null) || this.CurrentWeaponInfo == null || !((UnityEngine.Object) this.CurrentWeaponInfo.WeaponData != (UnityEngine.Object) null) || !this.CurrentWeaponInfo.WeaponData.CanBreakDodge || this.playerWeapon.CurrentAttackState != PlayerWeapon.AttackState.Begin) && (this.state.CURRENT_STATE == StateMachine.State.Attacking && (UnityEngine.Object) this.playerWeapon != (UnityEngine.Object) null && this.playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin || this.state.CURRENT_STATE == StateMachine.State.Casting))
    {
      if ((double) this.DodgeDelay <= 0.0 && InputManager.Gameplay.GetDodgeButtonDown(this) && !this.IsKnockedOut)
        this.DodgeQueued = true;
      return false;
    }
    if (this.IsKnockedOut || this.state.CURRENT_STATE == StateMachine.State.Dodging || this.state.CURRENT_STATE == StateMachine.State.CustomAction0 || !this.DodgeQueued && ((double) this.DodgeDelay > 0.0 || !InputManager.Gameplay.GetDodgeButtonDown(this)) || (PlayerFleeceManager.FleecePreventsRoll() || DataManager.Instance.NoRollInNextCombatRoom) && GameManager.IsDungeon(PlayerFarming.Location))
      return false;
    this.DodgeQueued = false;
    this.playerController.StopHitEffects();
    this.HideProjectileChargeBars();
    double facingAngle = (double) this.state.facingAngle;
    this.simpleSpineAnimator.FlashWhite(false);
    this.playerController.forceDir = (double) this.playerController.xDir != 0.0 || (double) this.playerController.yDir != 0.0 ? Utils.GetAngle(Vector3.zero, new Vector3(this.playerController.xDir, this.playerController.yDir)) : this.state.facingAngle;
    this.playerController.speed = this.playerController.DodgeSpeed * 1.2f * DataManager.Instance.DodgeDistanceMultiplier * TrinketManager.GetDodgeDistanceMultiplier(this);
    this.state.CURRENT_STATE = StateMachine.State.Dodging;
    this.playerWeapon.StopAttackRoutine();
    AudioManager.Instance.PlayOneShot("event:/player/dash_roll", this.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.DodgeDelay = this.playerController.DodgeDelay;
    this.health.ForgiveRecentDamage();
    if (this.health.IsPoisoned && PlayerFleeceManager.FleeceCausesPoisonOnHit())
    {
      TrapPoison.CreatePoison(this.transform.position, 1, 0.0f, GenerateRoom.Instance.transform);
      this.health.ClearPoison();
    }
    if (this.health.IsBurned)
      this.health.ClearBurn();
    if (TrinketManager.DropBombOnRoll(this) && !TrinketManager.IsOnCooldown(TarotCards.Card.BombOnRoll, this))
    {
      Bomb.CreateBomb(this.transform.position, (Health) this.health, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
      TrinketManager.TriggerCooldown(TarotCards.Card.BombOnRoll, this);
    }
    if (TrinketManager.DropBlackGoopOnRoll(this) && !TrinketManager.IsOnCooldown(TarotCards.Card.GoopOnRoll, this))
    {
      TrapGoop.CreateGoop(this.transform.position, 5, 0.5f, this.gameObject, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
      TrinketManager.TriggerCooldown(TarotCards.Card.GoopOnRoll, this);
    }
    if (TrinketManager.HasTrinket(TarotCards.Card.HighRoller, this))
    {
      float t = 0.0f;
      DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 0.5f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.playerRelic.SpawnTrails(RelicType.IceSpikes, 0.05f)));
      this.HighRollerVFX.Play();
    }
    this.playerWeapon.DoingHeavyAttack = false;
    PlayerFarming.PlayerEvent onDodge = PlayerFarming.OnDodge;
    if (onDodge != null)
      onDodge();
    this.damageOnRollTrail.ClearTrails();
    return true;
  }

  public void Sword()
  {
    if ((double) this.ArrowAttackDelay > 0.0 || !InputManager.Gameplay.GetAttackButtonDown(this))
      return;
    this.isDashAttacking = this.state.CURRENT_STATE == StateMachine.State.Dodging;
    if (this.isDashAttacking)
    {
      this.CurrentCombo = 1;
      this.CurrentAttack = this.DashAttack;
    }
    else
    {
      if (++this.CurrentCombo > this.AttackList.Count - 1)
        this.CurrentCombo = 0;
      this.CurrentAttack = this.AttackList[this.CurrentCombo];
    }
    this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    this.state.Timer = this.CurrentAttack.PreTimer;
    this.playerController.TimedActionCallback = new System.Action(this.DoAttack);
    this.HoldingAttack = true;
  }

  public void DoHeavyAttack()
  {
  }

  public void StartHeavyAttack()
  {
  }

  public void DoAttack()
  {
    this.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    this.Spine.AnimationState.SetAnimation(0, this.CurrentAttack.AnimationName, false);
    this.state.Timer = this.CurrentAttack.PostTimer;
    this.ArrowAttackDelay = this.CurrentAttack.AttackDelay;
    Addressables_wrapper.InstantiateAsync((object) $"Assets/Prefabs/Enemies/Weapons/PlayerSwipe{this.CurrentCombo}.prefab", this.transform.position + new Vector3(0.5f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f), Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      obj.Result.GetComponent<Swipe>().Init(this.transform.position + new Vector3(0.5f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f), this.state.facingAngle, this.health.team, (Health) this.health, (Action<Health, Health.AttackTypes, Health.AttackFlags, float>) null, 1f);
      this.ResetCombo = 0.3f;
      CameraManager.shakeCamera(this.CurrentAttack.CamShake, this.state.facingAngle);
      AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
    }));
    if (!this.isDashAttacking)
      return;
    this.playerController.speed = this.CurrentAttack.Speed;
  }

  public void OnTriggerEnterEvent(Collider2D collider)
  {
    Health componentInParent1 = collider.GetComponentInParent<Health>();
    if (!(bool) (UnityEngine.Object) componentInParent1 || componentInParent1.team != Health.Team.Team2)
      return;
    if (this.playerWeapon.RepelProjectiles)
    {
      Projectile componentInParent2 = collider.GetComponentInParent<Projectile>();
      if ((UnityEngine.Object) componentInParent2 != (UnityEngine.Object) null && this.RepelProjectile(componentInParent2))
        return;
    }
    componentInParent1.DealDamage(PlayerWeapon.GetDamage(1f, this.currentWeaponLevel / 2, this), this.gameObject, this.transform.position);
  }

  public bool RepelProjectile(Projectile projectile)
  {
    if (projectile.IsAttachedToProjectileTrap())
      return false;
    float angle = Utils.GetAngle(this.transform.position, projectile.transform.position);
    projectile.Angle = angle;
    projectile.KnockedBack = true;
    projectile.team = Health.Team.PlayerTeam;
    projectile.health.team = Health.Team.PlayerTeam;
    return true;
  }

  public static event PlayerFarming.GetSoulAction OnGetSoul;

  public void GetSoul() => this.GetSoul(1);

  public void GetSoul(int delta = 1)
  {
    PlayerFarming.GetSoulAction onGetSoul = PlayerFarming.OnGetSoul;
    if (onGetSoul != null)
      onGetSoul(delta);
    if (delta <= 0 || UpgradeSystem.UpgradeType != UpgradeSystem.UpgradeTypes.Devotion && UpgradeSystem.UpgradeType != UpgradeSystem.UpgradeTypes.Both)
      return;
    this.GetXP(1f);
  }

  public static event PlayerFarming.GetBlackSoulAction OnGetBlackSoul;

  public static event PlayerFarming.GetBlackSoulAction OnResetBlackSouls;

  public void GetBlackSoul(int delta = 1, bool giveXp = true, bool playSound = true)
  {
    if (delta == 0)
      return;
    PlayerFarming.LeftoverSouls += Demon.GetDemonLeftovers();
    if ((double) PlayerFarming.LeftoverSouls >= 1.0)
    {
      --PlayerFarming.LeftoverSouls;
      ++delta;
    }
    if (playSound)
      AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.gameObject);
    this.BlackSouls += delta;
    ++DataManager.Instance.dungeonRunXPOrbs;
    PlayerFarming.GetBlackSoulAction onGetBlackSoul = PlayerFarming.OnGetBlackSoul;
    if (onGetBlackSoul != null)
      onGetBlackSoul(delta, this);
    if (!(delta > 0 & giveXp) || !EquipmentManager.GetWeaponData(this.currentWeapon).ContainsAttachmentType(AttachmentEffect.Fervour))
      return;
    this.GetXP(1f * DungeonModifier.HasPositiveModifier(DungeonPositiveModifier.DoubleXP, 2f, 1f) * TrinketManager.GetBlackSoulsMultiplier(this));
  }

  public void GetXP(float Delta)
  {
    DataManager.Instance.XP += Mathf.RoundToInt(Delta);
    if (DataManager.Instance.XP >= DataManager.GetTargetXP(Mathf.Min(DataManager.Instance.Level, DataManager.TargetXP.Count - 1)))
    {
      if (DataManager.Instance.Level == 0)
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.UnlockSacredKnowledge);
      DataManager.Instance.XP = 0;
      ++DataManager.Instance.Level;
      ++UpgradeSystem.AbilityPoints;
    }
    System.Action onGetXp = PlayerFarming.OnGetXP;
    if (onGetXp == null)
      return;
    onGetXp();
  }

  public void GetDisciple(float Delta)
  {
    this.StartCoroutine((IEnumerator) this.AddDisciple(Delta));
  }

  public IEnumerator AddDisciple(float Delta)
  {
    yield return (object) null;
    DataManager.Instance.DiscipleXP += Delta;
    System.Action onGetDiscipleXp = PlayerFarming.OnGetDiscipleXP;
    if (onGetDiscipleXp != null)
      onGetDiscipleXp();
    if ((double) DataManager.Instance.DiscipleXP >= (double) DataManager.TargetDiscipleXP[Mathf.Min(DataManager.Instance.DiscipleLevel, DataManager.TargetDiscipleXP.Count - 1)])
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GetFollowerUpgradePoint);
      DataManager.Instance.DiscipleXP = 0.0f;
      ++DataManager.Instance.DiscipleLevel;
      ++UpgradeSystem.DisciplePoints;
      NotificationCentreScreen.Play(NotificationCentre.NotificationType.UpgradeRitualReady);
    }
  }

  public void SetActiveDustEffect(bool isActive) => this.unitObject.emitDustClouds = isActive;

  public static void RefreshPlayersCount(bool initPlayers = true)
  {
    PlayerFarming.playersCount = PlayerFarming.players.Count;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if ((bool) (UnityEngine.Object) player && (bool) (UnityEngine.Object) player.hudHearts && (bool) (UnityEngine.Object) player.hudHearts.coopIndicatorIcon)
      {
        CoopIndicatorIcon coopIndicatorIcon = player.hudHearts.coopIndicatorIcon;
        coopIndicatorIcon.gameObject.SetActive(PlayerFarming.playersCount > 1);
        if (PlayerFarming.players[index].isLamb)
          coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Lamb);
        else
          coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Goat);
        coopIndicatorIcon.SetUsername(index);
      }
      if (initPlayers)
      {
        player.Init();
        player.health.InitHP();
      }
    }
    HUD_Manager.UpdatePlayerHudScale();
    if (PlayerFarming.playersCount == 1)
    {
      if ((double) PlayerFarming.CoopDitherMultiplier != 1.0)
      {
        PlayerFarming.CoopDitherMultiplier = 1f;
        DOTween.To(new DOSetter<float>(GameManager.SetDither), (float) GameManager.GlobalDitherIntensity, SettingsManager.Settings.Accessibility.DitherFadeDistance, 1f).SetEase<Tweener>(Ease.OutQuart);
      }
    }
    else if ((double) PlayerFarming.CoopDitherMultiplier != 1.75)
    {
      PlayerFarming.CoopDitherMultiplier = 1.75f;
      DOTween.To(new DOSetter<float>(GameManager.SetDither), (float) GameManager.GlobalDitherIntensity, SettingsManager.Settings.Accessibility.DitherFadeDistance, 1f).SetEase<Tweener>(Ease.OutQuart);
    }
    CoopManager.RefreshCoopPlayerRewired();
  }

  public static void HidePlayer(PlayerFarming playerFarming, bool withDelay = true)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    PlayerFarming.\u003C\u003Ec__DisplayClass314_0 displayClass3140 = new PlayerFarming.\u003C\u003Ec__DisplayClass314_0();
    // ISSUE: reference to a compiler-generated field
    displayClass3140.playerFarming = playerFarming;
    // ISSUE: reference to a compiler-generated field
    if (displayClass3140.playerFarming.gameObject.activeSelf)
    {
      // ISSUE: reference to a compiler-generated field
      displayClass3140.playerFarming.playerWasHidden = Time.time;
    }
    // ISSUE: reference to a compiler-generated field
    PlayerFarming.players.Remove(displayClass3140.playerFarming);
    if (withDelay)
    {
      // ISSUE: reference to a compiler-generated method
      GameManager.GetInstance().WaitForSeconds(0.1f, new System.Action(displayClass3140.\u003CHidePlayer\u003Eg__Callback\u007C0));
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      displayClass3140.\u003CHidePlayer\u003Eg__Callback\u007C0();
    }
  }

  public void OnDestroy()
  {
    if (this.isLamb)
      Debug.Log((object) " lamb Break here");
    else
      Debug.Log((object) "non lamb Break here");
    if (PlayerFarming.players.Contains(this))
    {
      PlayerFarming.players.Remove(this);
      PlayerFarming.RefreshPlayersCount(false);
      Debug.Log((object) ("Removed player from players array, new length: " + PlayerFarming.players.Count.ToString()));
    }
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) this)
      PlayerFarming.Instance = (PlayerFarming) null;
    this.health.OnHPUpdated -= new HealthPlayer.HPUpdated(this.SetSkinOnHit);
    this.health.OnTotalHPUpdated -= new HealthPlayer.HPUpdated(this.SetSkinOnHit);
    if ((UnityEngine.Object) this.hudHearts != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.hudHearts.gameObject);
    DataManager.OnChangeTool -= new DataManager.ChangeToolAction(this.ChangeTool);
    PlayerFarming.LeftoverSouls = 0.0f;
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStartedInDungeon);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  public void OnEnable()
  {
    this.Init();
    CoopManager.RefreshCoopPlayerRewired();
    if ((bool) (UnityEngine.Object) HideUI.Instance)
      HideUI.Instance.ResetHideTimeLong();
    if (!this.isLamb || !CompanionBaseArea.hasGhostTwins || CompanionBaseArea.AllCompanions.Count != 0 || GameManager.IsDungeon(PlayerFarming.Location))
      return;
    DOVirtual.DelayedCall(0.25f, (TweenCallback) (() => CompanionBaseArea.SpawnCompanionGhosts()));
  }

  public void Init()
  {
    this.health = this.gameObject.GetComponent<HealthPlayer>();
    Transform transform = HUD_Manager.Instance.healthContainer.transform;
    HUD_Manager.Instance.healthManager.Init(this);
    this.playerSpells.Init();
    if (this.EnableCoopFeatures)
    {
      if (!PlayerFarming.players.Contains(this))
      {
        PlayerFarming.players.Add(this);
        PlayerFarming.RefreshPlayersCount();
      }
      if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null)
        GameManager.GetInstance().AddToCamera(this.CameraBone);
      if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
        PlayerFarming.Instance = this;
      if (!this.EnableCoopFeatures)
        this.rewiredPlayer = RewiredInputManager.GetPlayer(0);
    }
    if (this.InitComplete)
      return;
    this.InitComplete = true;
    this.currentWeapon = EquipmentType.None;
    this.currentCurse = EquipmentType.None;
  }

  public static PlayerFarming FindClosestPlayer(
    Vector3 target,
    bool ignoreKnockOut = false,
    bool checkIfActive = false)
  {
    PlayerFarming closestPlayer = (PlayerFarming) null;
    float num1 = float.MaxValue;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((!player.IsKnockedOut || ignoreKnockOut) && (!checkIfActive || player.gameObject.activeInHierarchy))
      {
        float num2 = Vector3.Distance(player.transform.position, target);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          closestPlayer = player;
        }
      }
    }
    return closestPlayer;
  }

  public static GameObject FindClosestPlayerGameObject(Vector3 target, bool ignoreKnockOut = false)
  {
    PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(target, ignoreKnockOut);
    return (UnityEngine.Object) closestPlayer != (UnityEngine.Object) null ? closestPlayer.gameObject : (GameObject) null;
  }

  public static PlayerFarming FindClosePlayer(Vector3 target, float dist)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((double) Vector3.Distance(player.transform.position, target) < (double) dist)
        return player;
    }
    return (PlayerFarming) null;
  }

  public static void CleanAllCharacters(bool forceInstant = false)
  {
    if (CoopManager.CoopActive)
      PlayerFarming.AutoRespawn = true;
    if ((bool) (UnityEngine.Object) PlayerFarming.Instance && !PlayerFarming.Instance.gameObject.activeSelf)
      PlayerFarming.CleanCharacters();
    else if (!forceInstant)
    {
      int num = (bool) (UnityEngine.Object) PlayerFarming.Instance ? 1 : 0;
      PlayerFarming.CleanCharacters();
    }
    else
      PlayerFarming.CleanCharacters();
  }

  public static void CleanCharacters()
  {
    PlayerFarming.ResetMainPlayer();
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if ((bool) (UnityEngine.Object) PlayerFarming.players[index].hudHearts)
        UnityEngine.Object.Destroy((UnityEngine.Object) PlayerFarming.players[index].hudHearts.gameObject);
      if ((bool) (UnityEngine.Object) PlayerFarming.players[index].indicator)
        UnityEngine.Object.Destroy((UnityEngine.Object) PlayerFarming.players[index].indicator.gameObject);
      if ((bool) (UnityEngine.Object) PlayerFarming.players[index])
        UnityEngine.Object.Destroy((UnityEngine.Object) PlayerFarming.players[index].gameObject);
    }
    PlayerFarming.players.Clear();
    PlayerFarming.playersCount = 0;
    PlayerFarming.Instance = (PlayerFarming) null;
    for (int index = 0; index < CoopManager.AllPlayerGameObjects.Length; ++index)
    {
      if ((UnityEngine.Object) CoopManager.AllPlayerGameObjects[index] != (UnityEngine.Object) null)
      {
        PlayerFarming component = CoopManager.AllPlayerGameObjects[index].GetComponent<PlayerFarming>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          if ((bool) (UnityEngine.Object) component.hudHearts)
            UnityEngine.Object.Destroy((UnityEngine.Object) component.hudHearts.gameObject);
          if ((bool) (UnityEngine.Object) component.indicator)
            UnityEngine.Object.Destroy((UnityEngine.Object) component.indicator.gameObject);
          if ((bool) (UnityEngine.Object) component)
            UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
        }
        UnityEngine.Object.Destroy((UnityEngine.Object) CoopManager.AllPlayerGameObjects[index]);
        CoopManager.AllPlayerGameObjects[index] = (GameObject) null;
      }
    }
  }

  public bool IsPlayerWithinScreenView() => this.IsPlayerWithinScreenView(out Vector3 _);

  public bool IsPlayerWithinScreenView(out Vector3 screenPos)
  {
    screenPos = CameraManager.instance.CameraRef.WorldToScreenPoint(this.transform.position);
    return (double) screenPos.x > 0.0 & (double) screenPos.x < (double) Screen.width && (double) screenPos.y > 0.0 && (double) screenPos.y < (double) (Screen.height - 50);
  }

  public static void SetResetHealthData(bool state)
  {
    HealthPlayer.ResetHealthDataP1 = state;
    HealthPlayer.ResetHealthDataP2 = state;
  }

  public static bool IsAnyPlayerKnockedOut()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.IsKnockedOut)
        return true;
    }
    return false;
  }

  public static bool IsAnyPlayerCarryingBody()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody || player.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody)
        return true;
    }
    return false;
  }

  public static bool IsAnyPlayerChargingSnowball()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.state.CURRENT_STATE == StateMachine.State.ChargingSnowball)
        return true;
    }
    return false;
  }

  public static bool IsAnyPlayerInInteractionWithRanchable()
  {
    return Interaction_Ranchable.IsAnyRanchableInPlayerInteraction();
  }

  public bool IsRidingAnimal()
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if ((UnityEngine.Object) ranchable.playerFarming == (UnityEngine.Object) this && ranchable.CurrentState == Interaction_Ranchable.State.Riding)
        return true;
    }
    return false;
  }

  public Interaction_Ranchable GetRidingAnimal()
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if ((UnityEngine.Object) ranchable.playerFarming == (UnityEngine.Object) this && ranchable.CurrentState == Interaction_Ranchable.State.Riding)
        return ranchable;
    }
    return (Interaction_Ranchable) null;
  }

  public static void StopRidingOnAnimals()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
      {
        if ((UnityEngine.Object) ranchable.playerFarming == (UnityEngine.Object) player && ranchable.CurrentState == Interaction_Ranchable.State.Riding)
          ranchable.EndRiding();
      }
    }
  }

  public void StopRidingOnAnimal()
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if ((UnityEngine.Object) ranchable.playerFarming == (UnityEngine.Object) this && ranchable.CurrentState == Interaction_Ranchable.State.Riding)
        ranchable.EndRiding();
    }
  }

  public bool IsLeashingAnimal()
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if ((UnityEngine.Object) ranchable.playerFarming == (UnityEngine.Object) this && ranchable.CurrentState == Interaction_Ranchable.State.Leashed)
        return true;
    }
    return false;
  }

  public Interaction_Ranchable GetLeashingAnimal()
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if ((UnityEngine.Object) ranchable.playerFarming == (UnityEngine.Object) this && ranchable.CurrentState == Interaction_Ranchable.State.Leashed)
        return ranchable;
    }
    return (Interaction_Ranchable) null;
  }

  public void StopLeashingAnimal()
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if ((UnityEngine.Object) ranchable.playerFarming == (UnityEngine.Object) this && ranchable.CurrentState == Interaction_Ranchable.State.Leashed)
        ranchable.DetatchLeash();
    }
  }

  public Interaction_Ranchable GetPlayerAnimal(bool includeDead = false)
  {
    int index = PlayerFarming.players.IndexOf(this);
    if (index == -1)
      return (Interaction_Ranchable) null;
    StructuresData.Ranchable_Animal followingPlayerAnimal = DataManager.Instance.FollowingPlayerAnimals[index];
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if (ranchable.Animal == followingPlayerAnimal)
        return ranchable;
    }
    if (includeDead)
    {
      foreach (Interaction_Ranchable deadRanchable in Interaction_Ranchable.DeadRanchables)
      {
        if (deadRanchable.Animal == followingPlayerAnimal)
          return deadRanchable;
      }
    }
    return (Interaction_Ranchable) null;
  }

  public void CheckIntroDemigodStatus()
  {
    if (PlayerFarming.Location != FollowerLocation.IntroDungeon)
      return;
    this.health.GodMode = Health.CheatMode.Demigod;
  }

  public void SetCoopIndicatorVisibility(bool visible) => this.isCoopIndicatorVisible = visible;

  public void ChargeSnowball()
  {
    this.playerController.ResetSpecialMovingAnimations();
    float z = this.state.facingAngle;
    if (this.canUseKeyboard && InputManager.General.MouseInputActive)
      z = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(this.transform.position), (Vector3) InputManager.General.GetMousePosition(this));
    this.ShowHeavyAttackProjectileChargeBars();
    this.SetHeavyAimingRecticuleScaleAndRotation(0, new Vector3(Mathf.SmoothStep(0.5f, 1f, this.chargeSnowball), 1f, 1f), new Vector3(0.0f, 0.0f, z));
    this.chargeSnowball += Time.deltaTime;
  }

  public void RemoveSnowballInstant()
  {
    Debug.Log((object) "Instant remove / destroy snowball ");
    for (int index = 0; index < Interaction_IceBlock.IceBlocks.Count; ++index)
    {
      Interaction_IceBlock iceBlock = Interaction_IceBlock.IceBlocks[index];
      if (iceBlock.CarryingSnowball && (UnityEngine.Object) iceBlock.playerFarming == (UnityEngine.Object) this)
      {
        iceBlock.StopAllCoroutines();
        UnityEngine.Object.Destroy((UnityEngine.Object) iceBlock.gameObject);
        if (!BaseGoopDoor.IsOpen)
          BaseGoopDoor.DoorDown(this);
      }
    }
    this.playerController.ResetSpecialMovingAnimations();
    this.CarryingSnowball = false;
  }

  public IEnumerator ThrowSnowball()
  {
    PlayerFarming playerFarming = this;
    AudioManager.Instance.PlayOneShot("event:/dlc/material/snowball_aim_letgo", playerFarming.transform.position);
    for (int index = 0; index < Interaction_IceBlock.IceBlocks.Count; ++index)
    {
      Interaction_IceBlock iceBlock = Interaction_IceBlock.IceBlocks[index];
      if (iceBlock.CarryingSnowball && (UnityEngine.Object) iceBlock.playerFarming == (UnityEngine.Object) playerFarming)
      {
        iceBlock.StopAllCoroutines();
        UnityEngine.Object.Destroy((UnityEngine.Object) iceBlock.gameObject);
        if (!BaseGoopDoor.IsOpen)
          BaseGoopDoor.DoorDown(playerFarming);
      }
    }
    playerFarming.playerController.ResetSpecialMovingAnimations();
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
    {
      playerFarming.playerController.SetSpecialMovingAnimations("blizzard/idle-called-center", "blizzard/run-up-called-center", "blizzard/run-down-called-center", "blizzard/run-called-center", "blizzard/run-up-diagonal-called-center", "blizzard/run-horizontal-called-center", StateMachine.State.Idle_Winter);
      playerFarming.playerController.OverrideBlizzardAnims = true;
      playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_Winter;
    }
    if (playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
    {
      Vector3 screenPoint = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(playerFarming.transform.position);
      playerFarming.state.facingAngle = Utils.GetAngle(screenPoint, (Vector3) InputManager.General.GetMousePosition(playerFarming));
    }
    playerFarming.HideHeavyChargeBars();
    float strength = Mathf.Clamp01(playerFarming.chargeSnowball);
    playerFarming.chargeSnowball = 0.0f;
    string Animation = (double) UnityEngine.Random.value < 0.20000000298023224 ? "snowball/throw-fall" : "snowball/throw";
    playerFarming.CustomAnimationWithCallback(Animation, false, (System.Action) (() => this.state.CURRENT_STATE = this.playerController.BlizzardAnims ? StateMachine.State.Idle_Winter : StateMachine.State.Idle));
    yield return (object) new WaitForSeconds(0.3f);
    CameraManager.shakeCamera(2f, playerFarming.state.facingAngle);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact, playerFarming, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    AudioManager.Instance.PlayOneShot("event:/dlc/material/snowball_throw", playerFarming.gameObject);
    Addressables.InstantiateAsync((object) "Assets/Prefabs/Enemies/Weapons/ArrowSnowball.prefab").Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Projectile component = obj.Result.GetComponent<Projectile>();
      component.transform.position = this.transform.position;
      component.Angle = this.state.facingAngle;
      component.team = Health.Team.PlayerTeam;
      component.Owner = (Health) this.health;
      component.Speed = Mathf.Lerp(12f, 14f, strength);
      component.LifeTime = Mathf.Lerp(0.15f, 0.6f, strength);
      component.Trail.Clear();
    });
    playerFarming.CarryingSnowball = false;
  }

  public void TryDropSnowball()
  {
    if (!this.CarryingSnowball || this.state.CURRENT_STATE != StateMachine.State.InActive && !this.GoToAndStopping)
      return;
    for (int index = 0; index < Interaction_IceBlock.IceBlocks.Count; ++index)
    {
      Interaction_IceBlock iceBlock = Interaction_IceBlock.IceBlocks[index];
      if (iceBlock.CarryingSnowball && (UnityEngine.Object) iceBlock.playerFarming == (UnityEngine.Object) this)
      {
        iceBlock.DropSnowball();
        if (BaseGoopDoor.IsOpen)
          break;
        BaseGoopDoor.DoorDown(this);
        break;
      }
    }
  }

  [CompilerGenerated]
  public void \u003CDodgeRoll\u003Eb__272_2()
  {
    this.playerRelic.SpawnTrails(RelicType.IceSpikes, 0.05f);
  }

  [CompilerGenerated]
  public void \u003CDoAttack\u003Eb__285_0(AsyncOperationHandle<GameObject> obj)
  {
    obj.Result.GetComponent<Swipe>().Init(this.transform.position + new Vector3(0.5f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f), this.state.facingAngle, this.health.team, (Health) this.health, (Action<Health, Health.AttackTypes, Health.AttackFlags, float>) null, 1f);
    this.ResetCombo = 0.3f;
    CameraManager.shakeCamera(this.CurrentAttack.CamShake, this.state.facingAngle);
    AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
  }

  public delegate void PlayerEvent();

  public delegate void GoToEvent(Vector3 targetPosition);

  [Serializable]
  public class Attack
  {
    public float PreTimer;
    public float PostTimer;
    public float Speed;
    public float AttackDelay;
    public string AnimationName;
    public string AnimationNameUp;
    public string AnimationNameDown;
    public float CamShake = 0.5f;

    public Attack(
      float PreTimer,
      float PostTimer,
      float AttackDelay,
      float Speed,
      string AnimationName,
      float CamShake)
    {
      this.PreTimer = PreTimer;
      this.PostTimer = PostTimer;
      this.Speed = Speed;
      this.AttackDelay = AttackDelay;
      this.AnimationName = AnimationName;
      this.CamShake = CamShake;
    }
  }

  public delegate void GetSoulAction(int DeltaValue);

  public delegate void GetBlackSoulAction(int DeltaValue, PlayerFarming playerFarming);
}
