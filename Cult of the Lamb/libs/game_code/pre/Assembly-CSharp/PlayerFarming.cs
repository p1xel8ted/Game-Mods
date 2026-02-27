// Decompiled with JetBrains decompiler
// Type: PlayerFarming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Ara;
using MMRoomGeneration;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class PlayerFarming : BaseMonoBehaviour
{
  public static System.Action OnCrownReturn;
  public static System.Action OnCrownReturnSubtle;
  public static PlayerFarming Instance;
  public static FollowerLocation Location = FollowerLocation.None;
  public static FollowerLocation LastLocation = FollowerLocation.None;
  public GrowAndFade growAndFade;
  public GameObject CameraBone;
  private GameObject _CameraBone;
  public GameObject FishingLineBone;
  public GameObject BleatAOEParticle;
  public Transform CrownBone;
  private PlayerController _playerController;
  private UnitObject _unitObject;
  private Farm farm;
  private float ArrowAttackDelay;
  private float DodgeDelay;
  private HUD_Inventory InventoryMenu;
  public Inventory inventory;
  public PlayerSimpleInventory simpleInventory;
  private WeaponSelectMenu WeaponSelect;
  public StateMachine _state;
  private float AimAngle;
  public List<CurseChargeBar> CurseChargeBars;
  public List<CurseChargeBar> WeaponChargeBars;
  private Health AimTarget;
  public GameObject WateringCanAim;
  public float WateringCanScale;
  public float WateringCanScaleSpeed;
  public float WateringCanBob;
  public SkeletonAnimation Spine;
  public int CarryingDeadFollowerID = -1;
  public bool NearGrave;
  public bool NearCompostBody;
  private PlayerArrows playerArrows;
  public AudioClip ShootArrowClip;
  public ParticleSystem HealingParticles;
  public SimpleSpineAnimator simpleSpineAnimator;
  public Chain chain;
  public Transform CarryBone;
  public ColliderEvents PlayerDamageCollider;
  public AnimationCurve CurseAimingCurve;
  public LineRenderer CurseAimLine;
  public GameObject CurseTarget;
  public Material originalMaterial;
  public Material BW_Material;
  private bool _Healing;
  [HideInInspector]
  public CircleCollider2D circleCollider2D;
  private Skin PlayerSkin;
  private WorkPlace ClosestWorkPlace;
  private WorkPlaceSlot ClosestWorkPlaceSlot;
  private Dwelling ClosestDwelling;
  private DwellingSlot ClosestDwellingSlot;
  public bool GoToAndStopping;
  public bool IdleOnEnd;
  public GameObject LookToObject;
  private System.Action GoToCallback;
  private float maxDuration = -1f;
  private float startMoveTimestamp;
  private bool Meditating;
  public bool HoldingAttack;
  public AraTrail trail;
  public bool BlockMeditation;
  public bool DodgeQueued;
  private PlayerWeapon _playerWeapon;
  private PlayerSpells _playerSpells;
  public bool AllowDodging = true;
  public float HeavyAttackCharge;
  private bool isDashAttacking;
  private PlayerFarming.Attack CurrentAttack;
  public int CurrentCombo = -1;
  private float ResetCombo;
  public List<PlayerFarming.Attack> AttackList = new List<PlayerFarming.Attack>();
  public List<PlayerFarming.Attack> HeavyAttackList = new List<PlayerFarming.Attack>();
  public PlayerFarming.Attack DashAttack;
  public static float LeftoverSouls = 0.0f;
  public static System.Action OnGetXP;
  public static System.Action OnGetDiscipleXP;
  private static readonly int ScorchPos = Shader.PropertyToID("_ScorchPos");

  public PlayerController playerController
  {
    get
    {
      if ((UnityEngine.Object) this._playerController == (UnityEngine.Object) null)
        this._playerController = this.gameObject.GetComponent<PlayerController>();
      return this._playerController;
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

  public Health health { get; private set; }

  public static Health Health
  {
    get
    {
      return (UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null ? (Health) null : PlayerFarming.Instance.health;
    }
  }

  public Vector3 PreviousPosition { get; private set; }

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

  private IEnumerator DoHealing()
  {
    float HealTimer = 0.0f;
    while ((double) DataManager.Instance.PLAYER_HEALTH < (double) DataManager.Instance.PLAYER_TOTAL_HEALTH)
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

  private void Awake() => PlayerFarming.Instance = this;

  private void Start()
  {
    this.circleCollider2D = this.GetComponent<CircleCollider2D>();
    this.playerArrows = this.GetComponent<PlayerArrows>();
    this.InitHUD();
    this.inventory = this.GetComponent<Inventory>();
    foreach (Component curseChargeBar in this.CurseChargeBars)
      curseChargeBar.gameObject.SetActive(false);
    foreach (Component weaponChargeBar in this.WeaponChargeBars)
      weaponChargeBar.gameObject.SetActive(false);
    this.simpleInventory = this.GetComponent<PlayerSimpleInventory>();
    this.simpleSpineAnimator = this.GetComponentInChildren<SimpleSpineAnimator>();
    this.PlayerDamageCollider.SetActive(false);
    this.PlayerDamageCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
    this.HealingParticles.Stop();
    DataManager.OnChangeTool += new DataManager.ChangeToolAction(this.ChangeTool);
    Inventory.CURRENT_WEAPON = Inventory.CURRENT_WEAPON;
    this.SetSkin();
  }

  private void SetSkinOnHit(HealthPlayer target) => this.SetSkin();

  public Skin SetSkin() => this.SetSkin(!FaithAmmo.CanAfford((float) PlayerSpells.AmmoCost));

  public Skin SetSkin(bool BlackAndWhite)
  {
    this.PlayerSkin = new Skin("Player Skin");
    this.PlayerSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin($"Lamb_{DataManager.Instance.PlayerFleece.ToString()}{(BlackAndWhite ? "_BW" : "")}"));
    string str = WeaponData.Skins.Normal.ToString().ToString();
    if (DataManager.Instance.CurrentWeapon != EquipmentType.None)
      str = EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).Skin.ToString();
    this.PlayerSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Weapons/" + str));
    if ((double) this.health.HP + (double) this.health.BlackHearts + (double) this.health.BlueHearts + (double) this.health.SpiritHearts <= 1.0 && (double) DataManager.Instance.PLAYER_TOTAL_HEALTH != 2.0)
      this.PlayerSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Hurt2"));
    else if ((double) this.health.HP + (double) this.health.BlackHearts + (double) this.health.BlueHearts + (double) this.health.SpiritHearts <= 2.0 && (double) DataManager.Instance.PLAYER_TOTAL_HEALTH != 2.0 || (double) this.health.HP + (double) this.health.BlackHearts + (double) this.health.BlueHearts + (double) this.health.SpiritHearts <= 1.0 && (double) DataManager.Instance.PLAYER_TOTAL_HEALTH == 2.0)
      this.PlayerSkin.AddSkin(this.Spine.Skeleton.Data.FindSkin("Hurt1"));
    this.Spine.Skeleton.SetSkin(this.PlayerSkin);
    this.Spine.Skeleton.SetSlotsToSetupPose();
    return this.PlayerSkin;
  }

  private void InitHUD()
  {
    this.InventoryMenu = UnityEngine.Object.FindObjectOfType<HUD_Inventory>();
    if ((UnityEngine.Object) this.InventoryMenu != (UnityEngine.Object) null)
      this.InventoryMenu.gameObject.SetActive(false);
    this.WeaponSelect = UnityEngine.Object.FindObjectOfType<WeaponSelectMenu>();
    if (!((UnityEngine.Object) this.WeaponSelect != (UnityEngine.Object) null))
      return;
    this.WeaponSelect.gameObject.SetActive(false);
  }

  public Follower PickedUpFollower { get; private set; }

  public void DropDeadFollower()
  {
    if (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle_CarryingBody && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving_CarryingBody || !((UnityEngine.Object) Interaction_HarvestMeat.CurrentMovingBody != (UnityEngine.Object) null))
      return;
    Interaction_HarvestMeat.CurrentMovingBody.DropBody();
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

  private void DetachFollower()
  {
    this.PickedUpFollower = (Follower) null;
    this.ResetCarryStructureColours();
    this.chain.Disconnect();
  }

  private void ResetCarryStructureColours()
  {
    if ((UnityEngine.Object) this.ClosestWorkPlaceSlot != (UnityEngine.Object) null)
      this.ClosestWorkPlaceSlot.transform.localScale = Vector3.one;
    if (!((UnityEngine.Object) this.ClosestDwellingSlot != (UnityEngine.Object) null))
      return;
    this.ClosestDwellingSlot.transform.localScale = Vector3.one;
  }

  public void CustomAnimation(string Animation, bool Loop)
  {
    this.StartCoroutine((IEnumerator) this.CustomAnimationRoutine(Animation, Loop));
  }

  private IEnumerator CustomAnimationRoutine(string Animation, bool Loop)
  {
    this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    this.simpleSpineAnimator.Animate(Animation, 0, Loop);
  }

  private IEnumerator BleatRoutine()
  {
    PlayerFarming playerFarming = this;
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage", playerFarming.gameObject);
    playerFarming.simpleSpineAnimator.Animate("bleat", 0, false);
    yield return (object) new WaitForSeconds(0.4f);
    if (playerFarming.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  private IEnumerator BlessFollower(Follower follower)
  {
    int currentTaskType = (int) follower.Brain.CurrentTaskType;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.Brain.Stats.ReceivedBlessing = true;
    follower.FacePosition(PlayerFarming.Instance.transform.position);
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
      CultFaithManager.AddThought(Thought.Cult_Bless);
      double num2 = (double) follower.SetBodyAnimation("idle", true);
      if (task != null)
        follower.Brain.HardSwapToTask(task);
      else
        follower.Brain.CompleteCurrentTask();
    }));
  }

  private IEnumerator IntimidateFollower(Follower follower)
  {
    int currentTaskType = (int) follower.Brain.CurrentTaskType;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.Brain.Stats.Intimidated = true;
    follower.FacePosition(PlayerFarming.Instance.transform.position);
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

  private IEnumerator InspireFollower(Follower follower)
  {
    int currentTaskType = (int) follower.Brain.CurrentTaskType;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.Brain.Stats.Inspired = true;
    follower.FacePosition(PlayerFarming.Instance.transform.position);
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
    float maxDuration = 20f)
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
    this.maxDuration = maxDuration;
    this.startMoveTimestamp = Time.unscaledTime;
    if (DisableCollider)
      this.GetComponent<CircleCollider2D>().enabled = false;
    this.unitObject.givePath(TargetPosition.transform.position);
    this.unitObject.EndOfPath += new System.Action(this.EndGoToAndStop);
    PlayerFarming.GoToEvent goToAndStopBegin = PlayerFarming.OnGoToAndStopBegin;
    if (goToAndStopBegin == null)
      return;
    goToAndStopBegin(TargetPosition.transform.position);
  }

  public void GoToAndStop(
    Vector3 TargetPosition,
    GameObject LookToObject = null,
    bool IdleOnEnd = false,
    bool DisableCollider = false,
    System.Action GoToCallback = null,
    float maxDuration = 20f)
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
    this.maxDuration = maxDuration;
    this.startMoveTimestamp = Time.unscaledTime;
    if (DisableCollider)
      this.GetComponent<CircleCollider2D>().enabled = false;
    this.unitObject.givePath(TargetPosition);
    this.unitObject.EndOfPath += new System.Action(this.EndGoToAndStop);
    PlayerFarming.GoToEvent goToAndStopBegin = PlayerFarming.OnGoToAndStopBegin;
    if (goToAndStopBegin == null)
      return;
    goToAndStopBegin(TargetPosition);
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
    this.GetComponent<CircleCollider2D>().enabled = true;
    this.unitObject.EndOfPath -= new System.Action(this.EndGoToAndStop);
  }

  private void Update()
  {
    if ((double) Time.timeScale <= 0.0)
      return;
    if (this.state.CURRENT_STATE == StateMachine.State.Dodging && TrinketManager.DamageEnemyOnRoll())
    {
      if (!this.PlayerDamageCollider.isActiveAndEnabled)
      {
        this.trail.Clear();
        AudioManager.Instance.PlayOneShot("event:/player/damage_roll", this.transform.position);
      }
      this.PlayerDamageCollider.SetActive(true);
    }
    else
    {
      if (this.PlayerDamageCollider.isActiveAndEnabled)
        this.trail.Clear();
      this.PlayerDamageCollider.SetActive(false);
    }
    if (TrinketManager.HasTrinket(TarotCards.Card.BlackSoulAutoRecharge) && !TrinketManager.IsOnCooldown(TarotCards.Card.BlackSoulAutoRecharge) && (double) FaithAmmo.Ammo < (double) FaithAmmo.Total)
    {
      FaithAmmo.Instance.DoFlash = false;
      this.GetBlackSoul(giveXp: false);
      FaithAmmo.Instance.DoFlash = true;
      TrinketManager.TriggerCooldown(TarotCards.Card.BlackSoulAutoRecharge);
    }
    this.Spine.timeScale = this.state.CURRENT_STATE == StateMachine.State.Attacking ? this.Spine.timeScale : 1f;
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      if (this.Meditating && (this.state.CURRENT_STATE != StateMachine.State.Meditate || PlayerFarming.Instance.GoToAndStopping))
      {
        this.Meditating = false;
        if ((double) Time.timeScale == 3.0)
          Time.timeScale = 1f;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        GameManager.GetInstance().CameraResetTargetZoom();
        this.Spine.UseDeltaTime = true;
        MonoSingleton<Indicator>.Instance.Activate();
      }
      if (this.state.CURRENT_STATE == StateMachine.State.Meditate)
      {
        if (InputManager.Gameplay.GetMeditateButtonHeld() && !PlayerFarming.Instance.GoToAndStopping)
          return;
        this.Meditating = false;
        Time.timeScale = 1f;
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        GameManager.GetInstance().CameraResetTargetZoom();
        this.Spine.UseDeltaTime = true;
        MonoSingleton<Indicator>.Instance.Activate();
        return;
      }
      if ((UnityEngine.Object) this.state != (UnityEngine.Object) null && PlayerFarming.Location == FollowerLocation.Base && !this.BlockMeditation && !PlayerFarming.Instance.GoToAndStopping && InputManager.Gameplay.GetMeditateButtonDown() && this.state.CURRENT_STATE != StateMachine.State.Meditate && this.state.CURRENT_STATE != StateMachine.State.Idle_CarryingBody && this.state.CURRENT_STATE != StateMachine.State.Moving_CarryingBody && this.state.CURRENT_STATE != StateMachine.State.CustomAction0 && !LetterBox.IsPlaying)
      {
        this.Meditating = true;
        Time.timeScale = 3f;
        this.state.CURRENT_STATE = StateMachine.State.Meditate;
        GameManager.GetInstance().CameraSetTargetZoom(15f);
        this.Spine.UseDeltaTime = false;
      }
    }
    else if (this.state.CURRENT_STATE == StateMachine.State.Meditate && !GameManager.IsDungeon(PlayerFarming.Location))
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    if (this.GoToAndStopping)
    {
      if ((UnityEngine.Object) this.PickedUpFollower != (UnityEngine.Object) null && this.state.CURRENT_STATE != StateMachine.State.TimedAction)
        this.UpdatePickedUpFollower();
      if ((double) this.maxDuration == -1.0 || (double) Time.time <= (double) this.startMoveTimestamp + (double) this.maxDuration)
        return;
      this.EndGoToAndStop();
    }
    else
    {
      if (this.state.CURRENT_STATE == StateMachine.State.Map || this.state.CURRENT_STATE == StateMachine.State.Heal || this.state.CURRENT_STATE == StateMachine.State.CustomAnimation || this.state.CURRENT_STATE == StateMachine.State.Grabbed || this.state.CURRENT_STATE == StateMachine.State.DashAcrossIsland || this.state.CURRENT_STATE == StateMachine.State.Grapple || this.state.CURRENT_STATE == StateMachine.State.SpawnIn || this.state.CURRENT_STATE == StateMachine.State.Respawning || this.state.CURRENT_STATE == StateMachine.State.Dieing || this.state.CURRENT_STATE == StateMachine.State.Dead || this.state.CURRENT_STATE == StateMachine.State.Converting || this.state.CURRENT_STATE == StateMachine.State.InActive || this.state.CURRENT_STATE == StateMachine.State.Building || this.state.CURRENT_STATE == StateMachine.State.Unconverted || this.state.CURRENT_STATE == StateMachine.State.FoundItem || this.state.CURRENT_STATE == StateMachine.State.GameOver || this.state.CURRENT_STATE == StateMachine.State.FinalGameOver)
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
        if (this.state.CURRENT_STATE == StateMachine.State.ChargingHeavyAttack || this.state.CURRENT_STATE == StateMachine.State.InActive || this.state.CURRENT_STATE == StateMachine.State.TimedAction || this.state.CURRENT_STATE == StateMachine.State.SignPostAttack || this.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack)
          return;
        if ((UnityEngine.Object) this.PickedUpFollower != (UnityEngine.Object) null)
        {
          if (this.state.CURRENT_STATE == StateMachine.State.TimedAction)
            return;
          this.UpdatePickedUpFollower();
        }
        else if (this.simpleInventory.Item != InventoryItem.ITEM_TYPE.NONE)
        {
          if (!InputManager.Gameplay.GetAttackButtonHeld())
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
          this.PreviousPosition = this.transform.position;
        }
      }
    }
  }

  public void ShowProjectileChargeBars()
  {
    int num = 1;
    bool requiresCharging = false;
    if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PrimaryEquipmentType == EquipmentType.Fireball || EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PrimaryEquipmentType == EquipmentType.MegaSlash)
      requiresCharging = true;
    if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).EquipmentType == EquipmentType.Fireball_Swarm)
      requiresCharging = false;
    if (DataManager.Instance.CurrentCurse == EquipmentType.Tentacles_Circular)
      num = 4;
    for (int index = 0; index < num; ++index)
      this.CurseChargeBars[index].ShowProjectileCharge(requiresCharging);
  }

  public void HideProjectileChargeBars()
  {
    foreach (CurseChargeBar curseChargeBar in this.CurseChargeBars)
      curseChargeBar.HideProjectileCharge();
  }

  public void SetAimingRecticuleScaleAndRotation(int chargeBarIndex, Vector3 scale, Vector3 euler)
  {
    this.CurseChargeBars[chargeBarIndex].SetAimingRecticuleScaleAndRotation(scale, euler);
  }

  public void ShowWeaponChargeBars()
  {
    Debug.Log((object) "SHOW!!");
    int num = 1;
    for (int index = 0; index < num; ++index)
      this.WeaponChargeBars[index].ShowProjectileCharge(false);
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

  private void UpdatePickedUpFollower()
  {
    this.PickedUpFollower.transform.position = Vector3.Lerp(this.PickedUpFollower.transform.position, this.CarryBone.position with
    {
      y = this.transform.position.y
    } + new Vector3(1f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 1f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), 0.0f), 15f * Time.deltaTime);
    this.PickedUpFollower.State.facingAngle = Utils.GetAngle(this.transform.position, this.PickedUpFollower.transform.position);
  }

  private void ChangeTool(int PrevTool, int NewTool)
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

  private void CookFood() => this.simpleInventory.GiveItem(InventoryItem.ITEM_TYPE.MEAT);

  private void EatFood()
  {
    this.TimedAction(1f, (System.Action) null);
    this.simpleInventory.RemoveItem();
  }

  private void WaterPlants()
  {
    Vector3 vector3 = new Vector3(0.8f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.8f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)));
    if (InputManager.Gameplay.GetAttackButtonHeld())
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
        this._playerSpells = this.GetComponent<PlayerSpells>();
      return this._playerSpells;
    }
  }

  public bool DodgeRoll()
  {
    if (!this.AllowDodging || this.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody || this.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody || this.state.CURRENT_STATE == StateMachine.State.WeaponSelect || this.state.CURRENT_STATE == StateMachine.State.Meditate || this.state.CURRENT_STATE == StateMachine.State.Dead)
      return false;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
        if (InputManager.Gameplay.GetBleatButtonDown())
        {
          if (PlayerFarming.Location == FollowerLocation.Base)
            this.StartCoroutine((IEnumerator) this.BleatRoutine());
          return false;
        }
        break;
    }
    if ((!((UnityEngine.Object) this.playerWeapon != (UnityEngine.Object) null) || this.playerWeapon.CurrentWeapon == null || !((UnityEngine.Object) this.playerWeapon.CurrentWeapon.WeaponData != (UnityEngine.Object) null) || !this.playerWeapon.CurrentWeapon.WeaponData.CanBreakDodge || this.playerWeapon.CurrentAttackState != PlayerWeapon.AttackState.Begin) && (this.state.CURRENT_STATE == StateMachine.State.Attacking && (UnityEngine.Object) this.playerWeapon != (UnityEngine.Object) null && this.playerWeapon.CurrentAttackState == PlayerWeapon.AttackState.Begin || this.state.CURRENT_STATE == StateMachine.State.Casting))
    {
      if ((double) this.DodgeDelay <= 0.0 && InputManager.Gameplay.GetDodgeButtonDown())
        this.DodgeQueued = true;
      return false;
    }
    if (this.state.CURRENT_STATE == StateMachine.State.Dodging || this.state.CURRENT_STATE == StateMachine.State.CustomAction0 || !this.DodgeQueued && ((double) this.DodgeDelay > 0.0 || !InputManager.Gameplay.GetDodgeButtonDown()))
      return false;
    this.DodgeQueued = false;
    this.playerController.StopHitEffects();
    this.HideProjectileChargeBars();
    double facingAngle = (double) this.state.facingAngle;
    this.playerController.forceDir = (double) this.playerController.xDir != 0.0 || (double) this.playerController.yDir != 0.0 ? Utils.GetAngle(Vector3.zero, new Vector3(this.playerController.xDir, this.playerController.yDir)) : this.state.facingAngle;
    this.playerController.speed = this.playerController.DodgeSpeed * 1.2f * DataManager.Instance.DodgeDistanceMultiplier;
    this.state.CURRENT_STATE = StateMachine.State.Dodging;
    this.playerWeapon.StopAttackRoutine();
    AudioManager.Instance.PlayOneShot("event:/player/dash_roll", this.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.DodgeDelay = this.playerController.DodgeDelay;
    this.health.ForgiveRecentDamage();
    if (TrinketManager.DropBombOnRoll() && !TrinketManager.IsOnCooldown(TarotCards.Card.BombOnRoll))
    {
      Bomb.CreateBomb(this.transform.position, this.health, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
      TrinketManager.TriggerCooldown(TarotCards.Card.BombOnRoll);
    }
    if (TrinketManager.DropBlackGoopOnRoll() && !TrinketManager.IsOnCooldown(TarotCards.Card.GoopOnRoll))
    {
      TrapGoop.CreateGoop(this.transform.position, 5, 0.5f, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
      TrinketManager.TriggerCooldown(TarotCards.Card.GoopOnRoll);
    }
    PlayerFarming.PlayerEvent onDodge = PlayerFarming.OnDodge;
    if (onDodge != null)
      onDodge();
    return true;
  }

  private void Sword()
  {
    if ((double) this.ArrowAttackDelay > 0.0 || !InputManager.Gameplay.GetAttackButtonDown())
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

  private void StartHeavyAttack()
  {
  }

  private void DoAttack()
  {
    this.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    this.Spine.AnimationState.SetAnimation(0, this.CurrentAttack.AnimationName, false);
    this.state.Timer = this.CurrentAttack.PostTimer;
    this.ArrowAttackDelay = this.CurrentAttack.AttackDelay;
    Addressables.InstantiateAsync((object) $"Assets/Prefabs/Enemies/Weapons/PlayerSwipe{this.CurrentCombo}.prefab", this.transform.position + new Vector3(0.5f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f), Quaternion.identity, this.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      obj.Result.GetComponent<Swipe>().Init(this.transform.position + new Vector3(0.5f * Mathf.Cos(this.state.facingAngle * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(this.state.facingAngle * ((float) Math.PI / 180f)), -0.5f), this.state.facingAngle, this.health.team, this.health, (Action<Health, Health.AttackTypes>) null, 1f);
      this.ResetCombo = 0.3f;
      CameraManager.shakeCamera(this.CurrentAttack.CamShake, this.state.facingAngle);
      AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
    });
    if (!this.isDashAttacking)
      return;
    this.playerController.speed = this.CurrentAttack.Speed;
  }

  private void OnTriggerEnterEvent(Collider2D collider)
  {
    Health componentInParent = collider.GetComponentInParent<Health>();
    if (!(bool) (UnityEngine.Object) componentInParent || componentInParent.team != Health.Team.Team2)
      return;
    componentInParent.DealDamage(0.5f, this.gameObject, this.transform.position);
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

  public void GetBlackSoul(int delta = 1, bool giveXp = true)
  {
    if (delta == 0)
      return;
    PlayerFarming.LeftoverSouls += Demon.GetDemonLeftovers();
    if ((double) PlayerFarming.LeftoverSouls >= 1.0)
    {
      --PlayerFarming.LeftoverSouls;
      ++delta;
    }
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.gameObject);
    Inventory.BlackSouls += delta;
    ++DataManager.Instance.dungeonRunXPOrbs;
    PlayerFarming.GetBlackSoulAction onGetBlackSoul = PlayerFarming.OnGetBlackSoul;
    if (onGetBlackSoul != null)
      onGetBlackSoul(delta);
    if (!(delta > 0 & giveXp) || !EquipmentManager.GetWeaponData(DataManager.Instance.CurrentWeapon).ContainsAttachmentType(AttachmentEffect.Fervour))
      return;
    this.GetXP(1f * DungeonModifier.HasPositiveModifier(DungeonPositiveModifier.DoubleXP, 2f, 1f) * TrinketManager.GetBlackSoulsMultiplier());
  }

  public void GetXP(float Delta)
  {
    DataManager.Instance.XP += Mathf.RoundToInt(Delta);
    System.Action onGetXp = PlayerFarming.OnGetXP;
    if (onGetXp != null)
      onGetXp();
    if (DataManager.Instance.XP < DataManager.TargetXP[Mathf.Min(DataManager.Instance.Level, DataManager.TargetXP.Count - 1)])
      return;
    if (DataManager.Instance.Level == 0)
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.UnlockSacredKnowledge);
    DataManager.Instance.XP = 0;
    ++DataManager.Instance.Level;
    ++UpgradeSystem.AbilityPoints;
  }

  public void GetDisciple(float Delta)
  {
    this.StartCoroutine((IEnumerator) this.AddDisciple(Delta));
  }

  private IEnumerator AddDisciple(float Delta)
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

  private void OnDisable()
  {
    HealthPlayer.OnHPUpdated -= new HealthPlayer.HPUpdated(this.SetSkinOnHit);
    if (!((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) this))
      return;
    PlayerFarming.Instance = (PlayerFarming) null;
  }

  private void OnEnable()
  {
    PlayerFarming.Instance = this;
    HealthPlayer.OnHPUpdated += new HealthPlayer.HPUpdated(this.SetSkinOnHit);
    this.health = this.gameObject.GetComponent<Health>();
  }

  private void OnDestroy()
  {
    DataManager.OnChangeTool -= new DataManager.ChangeToolAction(this.ChangeTool);
    PlayerFarming.LeftoverSouls = 0.0f;
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

  public delegate void GetBlackSoulAction(int DeltaValue);
}
