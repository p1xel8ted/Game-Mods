// Decompiled with JetBrains decompiler
// Type: Follower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using MMBiomeGeneration;
using MMTools;
using Pathfinding;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Follower : BaseMonoBehaviour
{
  public static List<Follower> Followers = new List<Follower>();
  public GameObject Container;
  public GameObject CameraBone;
  public GameObject PropagandaIcon;
  public const float DEFAULT_MAX_SPEED = 2.25f;
  public const float COMPLAINT_COOLDOWN_GAME_MINUTES = 120f;
  public const float SHARED_COMPLAINT_COOLDOWN_GAME_MINUTES = 5f;
  public static float LastComplaintFromAnyFollowerTime;
  public GameObject UIWorshipperStatsPrefab;
  public ParticleSystem ParticleSystem;
  public GameObject BlessedTodayIcon;
  public GameObject InsomniacIcon;
  public GameObject HibernationIcon;
  public GameObject AestivationIcon;
  public GameObject DrowsyIcon;
  public SkeletonAnimation Portal;
  public ParticleSystem SnowFootsteps;
  public ParticleSystem SnowballHitFX;
  public BoneFollower Head;
  public Seeker Seeker;
  public StateMachine State;
  public SkeletonAnimation Spine;
  public SimpleSpineAnimator SimpleAnimator;
  public Health Health;
  public WorshipperBubble WorshipperBubble;
  public Transform ChainConnection;
  public interaction_FollowerInteraction Interaction_FollowerInteraction;
  public UIFollowerPrayingProgress UIFollowerPrayingProgress;
  public FollowerRadialProgressBar FollowerRadialProgress;
  public GameObject CompletedQuestIcon;
  public GameObject FollowerWarningIcons;
  public GameObject IllnessAura;
  public GameObject Goop;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimIdle;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimHoodUp;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimHoodDown;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimWalking;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimPray;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string AnimWorship;
  [CompilerGenerated]
  public FollowerBrain \u003CBrain\u003Ek__BackingField;
  [CompilerGenerated]
  public FollowerOutfit \u003COutfit\u003Ek__BackingField;
  public float _timedActionTimer;
  public System.Action _timedAction;
  public bool _dying;
  public List<Vector3> _currentPath;
  public int _currentWaypoint;
  public Vector3 _startPos;
  public float _t;
  public Vector3 _destPos;
  public float _speed;
  public float _stoppingDistance = 0.1f;
  public System.Action _onPathComplete;
  [CompilerGenerated]
  public float \u003CSpeedMultiplier\u003Ek__BackingField = 1f;
  public bool _isResumed;
  [CompilerGenerated]
  public bool \u003CUseUnscaledTime\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003COverridingEmotions\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003COverridingOutfit\u003Ek__BackingField;
  public Material NormalMaterial;
  public Material BW_Material;
  public SimpleBark simpleBark;
  public bool showingBark;
  [SerializeField]
  public LayerMask lockToGroundMask;
  public string strCurrentTask = "";
  public FollowerAdorationUI adorationUI;
  public FollowerPleasureUI pleasureUI;
  public string cachedBarkMessage = "";
  public SkeletonAnimationLODManager skeletonAnimationLODManager;
  public static List<Follower> instances = new List<Follower>();
  public int frameCounter;
  public int instanceIndex;
  [CompilerGenerated]
  public List<FollowerPet> \u003CDLCFollowerPets\u003Ek__BackingField = new List<FollowerPet>();
  public static int LeaderEncounterColorBoost = Shader.PropertyToID("_LeaderEncounterColorBoost");
  public System.Action OnFollowerBrainAssigned;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string TestAnimation;
  public System.Action OnLevelUpRoutineComplete;
  public float timeBetweenEnsuringFollowerIsWithinBounds = 2f;
  public static Vector2[] Points = new Vector2[0];
  public float timeBetweenGiggles;
  public EventInstance laughingLoop;
  [CompilerGenerated]
  public bool \u003CIgnoreBounds\u003Ek__BackingField;
  public RaycastHit LockToGroundHit;
  public Vector3 LockToGroundPosition;
  public Vector3 LockToGroundNewPosition;
  [CompilerGenerated]
  public bool \u003CLockToGround\u003Ek__BackingField = true;
  public RaycastHit[] hits = new RaycastHit[1];
  public bool WasDistanceCalculatedOnThisFrame;
  public bool IsUpdateRequired = true;
  public const float MOVEMENT_DELTA = 1f;
  public Vector3 lastUpdatedPosition = Vector3.zero;
  public const float DELAY_BETWEEN_POSITION_CHECK = 0.3f;
  public float lastPositionUpdateTimer;
  public float delayBetweenPositionUpdateSpread;
  public Tween completedQuestIconTween;
  public Coroutine deathCoroutine;
  public bool UsePathing = true;
  public float Angle;
  public Vector3 previousPosition;
  public float timeSincePath;
  public bool UseDeltaTime = true;
  public Coroutine blessFollowerCoroutine;
  public Coroutine pleasureRoutine;
  public GameObject rewardPrefab;
  public bool InGiveSinSequence;
  public EventInstance snakeLoop;
  public float seperatorVX;
  public float seperatorVY;

  public void PlayParticles() => this.ParticleSystem.Play();

  public FollowerBrain Brain
  {
    get => this.\u003CBrain\u003Ek__BackingField;
    set => this.\u003CBrain\u003Ek__BackingField = value;
  }

  public FollowerOutfit Outfit
  {
    get => this.\u003COutfit\u003Ek__BackingField;
    set => this.\u003COutfit\u003Ek__BackingField = value;
  }

  public float SpeedMultiplier
  {
    get => this.\u003CSpeedMultiplier\u003Ek__BackingField;
    set => this.\u003CSpeedMultiplier\u003Ek__BackingField = value;
  }

  public bool IsPaused => !this._isResumed;

  public bool UseUnscaledTime
  {
    get => this.\u003CUseUnscaledTime\u003Ek__BackingField;
    set => this.\u003CUseUnscaledTime\u003Ek__BackingField = value;
  }

  public bool OverridingEmotions
  {
    get => this.\u003COverridingEmotions\u003Ek__BackingField;
    set => this.\u003COverridingEmotions\u003Ek__BackingField = value;
  }

  public bool OverridingOutfit
  {
    get => this.\u003COverridingOutfit\u003Ek__BackingField;
    set => this.\u003COverridingOutfit\u003Ek__BackingField = value;
  }

  public FollowerAdorationUI AdorationUI => this.adorationUI;

  public FollowerPleasureUI PleasureUI => this.pleasureUI;

  public SkeletonAnimationLODManager SkeletonAnimationLODManager
  {
    get => this.skeletonAnimationLODManager;
  }

  public List<FollowerPet> DLCFollowerPets
  {
    get => this.\u003CDLCFollowerPets\u003Ek__BackingField;
    set => this.\u003CDLCFollowerPets\u003Ek__BackingField = value;
  }

  public void Start()
  {
    Follower.instances.Add(this);
    this.instanceIndex = Follower.instances.Count - 1;
    if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
      CultFaithManager.Instance.OnThoughtModified += new CultFaithManager.ThoughtEvent(this.OnThoughtModified);
    this.adorationUI = this.GetComponentInChildren<FollowerAdorationUI>(true);
    this.pleasureUI = this.GetComponentInChildren<FollowerPleasureUI>(true);
    this.BlessedTodayIcon.gameObject.SetActive(false);
    this.simpleBark = this.GetComponent<SimpleBark>();
    if ((UnityEngine.Object) this.simpleBark != (UnityEngine.Object) null)
      this.simpleBark.enabled = false;
    foreach (FollowerPet.FollowerPetType pet in this.Brain._directInfoAccess.Pets)
      this.CreatePet(pet, this.transform.position);
    foreach (FollowerPet.DLCPet dlcPet in this.Brain._directInfoAccess.DLCPets)
      this.CreatePet(dlcPet, this.transform.position);
    this.Brain.OnCursedStateRemoved += new FollowerBrain.CursedEvent(this.Brain_OnCursedStateRemoved);
    if (PlayerFarming.Location == FollowerLocation.Base)
      this.GetComponentInChildren<UIFollowerName>(true)?.Show(false);
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
  }

  public void CopyFollowerConfigure()
  {
    this.Seeker.pathCallback += new OnPathDelegate(this.StartPath);
  }

  public void OnDestroy()
  {
    Follower.instances.Remove(this);
    if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
      CultFaithManager.Instance.OnThoughtModified -= new CultFaithManager.ThoughtEvent(this.OnThoughtModified);
    foreach (FollowerPet followerPet in FollowerPet.FollowerPets)
    {
      for (int index = FollowerPet.FollowerPets.Count - 1; index >= 0; --index)
      {
        if ((UnityEngine.Object) FollowerPet.FollowerPets[index].Follower == (UnityEngine.Object) this)
          UnityEngine.Object.Destroy((UnityEngine.Object) FollowerPet.FollowerPets[index].gameObject);
      }
    }
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
    AudioManager.Instance.StopLoop(this.laughingLoop);
    AudioManager.Instance.StopLoop(this.snakeLoop);
    if (this.Brain == null)
      return;
    this.Brain.OnCursedStateRemoved -= new FollowerBrain.CursedEvent(this.Brain_OnCursedStateRemoved);
    this.Brain.OnPleasureAdded -= new FollowerBrain.PleasureEvent(this.Brain_OnPleasureAdded);
  }

  public void OnEnable()
  {
    Follower.Followers.Add(this);
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null && this.Spine.AnimationState != null)
      this.Spine.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.SetEmotionAnimation);
    Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    foreach (MeshRenderer componentsInChild in this.GetComponentsInChildren<MeshRenderer>())
    {
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) null && (UnityEngine.Object) componentsInChild.sharedMaterial != (UnityEngine.Object) null)
        componentsInChild.sharedMaterial.SetColor(Follower.LeaderEncounterColorBoost, color);
    }
    this.lastUpdatedPosition = this.transform.position;
    this.delayBetweenPositionUpdateSpread = 0.3f + UnityEngine.Random.Range(0.0f, 0.3f);
    for (int index = FollowerPet.FollowerPets.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) FollowerPet.FollowerPets[index].Follower == (UnityEngine.Object) this)
        FollowerPet.FollowerPets[index].gameObject.SetActive(true);
    }
    if (this.Brain != null)
      this.Brain.OnPleasureAdded += new FollowerBrain.PleasureEvent(this.Brain_OnPleasureAdded);
    if (this.Brain != null && this.Brain.CurrentTaskType == FollowerTaskType.ManualControl && this.Brain.CurrentTask != null && this.Brain.CurrentTask is FollowerTask_LightManualControl)
      this.Brain.CompleteCurrentTask();
    this.EnsureWithinBounds();
  }

  public void OnDisable()
  {
    if (this.blessFollowerCoroutine != null)
      this.BlessFollowerInterruptedCallback();
    Follower.Followers.Remove(this);
    this.Pause();
    this.Spine.AnimationState.Start -= new Spine.AnimationState.TrackEntryDelegate(this.SetEmotionAnimation);
    if (this.Brain != null)
      this.Brain.OnPleasureAdded -= new FollowerBrain.PleasureEvent(this.Brain_OnPleasureAdded);
    AudioManager.Instance.StopLoop(this.laughingLoop);
  }

  public void Init(FollowerBrain brain, FollowerOutfit outfit)
  {
    this.Brain = brain;
    this.Outfit = outfit;
    if (!DataManager.Instance.DLC_Cultist_Pack && DataManager.CultistDLCSkins.Contains(this.Brain.Info.SkinName.StripNumbers()) || !DataManager.Instance.DLC_Sinful_Pack && DataManager.SinfulDLCSkins.Contains(this.Brain.Info.SkinName.StripNumbers()) || !DataManager.Instance.DLC_Pilgrim_Pack && DataManager.PilgrimDLCSkins.Contains(this.Brain.Info.SkinName.StripNumbers()) || !DataManager.Instance.DLC_Heretic_Pack && DataManager.HereticDLCSkins.Contains(this.Brain.Info.SkinName.StripNumbers()))
    {
      WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[brain.Info.SkinCharacter];
      brain.Info.SkinColour = character.SlotAndColours.RandomIndex<WorshipperData.SlotsAndColours>();
      brain.Info.SkinCharacter = WorshipperData.Instance.GetRandomAvailableSkin(true, true);
      string skinNameFromIndex = WorshipperData.Instance.GetSkinNameFromIndex(brain.Info.SkinCharacter);
      brain.Info.SkinVariation = WorshipperData.Instance.GetColourData(skinNameFromIndex).Skin.RandomIndex<WorshipperData.CharacterSkin>();
      brain.Info.SkinName = skinNameFromIndex;
    }
    FollowerClothingType clothing;
    if (!DataManager.Instance.DLC_Cultist_Pack)
    {
      clothing = this.Brain.Info.Clothing;
      if (clothing.ToString().StripNumbers().Contains("Cultist_"))
        goto label_10;
    }
    if (!DataManager.Instance.DLC_Sinful_Pack)
    {
      clothing = this.Brain.Info.Clothing;
      if (clothing.ToString().StripNumbers().Contains("DLC_"))
        goto label_10;
    }
    if (!DataManager.Instance.DLC_Pilgrim_Pack)
    {
      clothing = this.Brain.Info.Clothing;
      if (clothing.ToString().StripNumbers().Contains("Pilgrim_"))
        goto label_10;
    }
    if (!DataManager.Instance.DLC_Heretic_Pack)
    {
      clothing = this.Brain.Info.Clothing;
      if (!clothing.ToString().StripNumbers().Contains("Heretic_"))
        goto label_11;
    }
    else
      goto label_11;
label_10:
    this.Brain.Info.Clothing = FollowerClothingType.None;
    this.Brain.Info.ClothingVariant = "";
label_11:
    this.Outfit.SetOutfit(this.Spine, false);
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null && this.Spine.AnimationState != null)
      this.Spine.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.SetEmotionAnimation);
    if ((bool) (UnityEngine.Object) this.SimpleAnimator)
      this.SimpleAnimator.AnimationTrack = 1;
    System.Action followerBrainAssigned = this.OnFollowerBrainAssigned;
    if (followerBrainAssigned != null)
      followerBrainAssigned();
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null && this.Spine.AnimationState != null)
      this.SetEmotionAnimation();
    this.SetOverrideOutfit(true);
    if (FollowerBrain.TemporaryHats.Contains(this.Brain.Info.Hat))
      this.SetHat(FollowerHatType.None);
    this.Brain.SavedFollowerTaskDestination = Vector3.zero;
    if (brain.Location == FollowerLocation.Base)
    {
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective is Objectives_TalkToFollower && objective.Follower == brain.Info.ID && string.IsNullOrEmpty(((Objectives_TalkToFollower) objective).ResponseTerm))
        {
          this.ShowCompletedQuestIcon(true);
          return;
        }
      }
      if (DataManager.Instance.CompletedQuestFollowerIDs.Contains(brain.Info.ID))
        DataManager.Instance.CompletedQuestFollowerIDs.Remove(brain.Info.ID);
    }
    if (!FollowerManager.FollowerLocked(this.Brain.Info.ID))
    {
      if (this.Brain.ThoughtExists(Thought.PropogandaSpeakers))
        this.PropagandaIcon.gameObject.SetActive(true);
      if (!this.Brain.ThoughtExists(Thought.PropogandaSpeakers))
        this.PropagandaIcon.SetActive(false);
    }
    if (brain.CurrentTaskType == FollowerTaskType.ManualControl)
      brain.CompleteCurrentTask();
    this.Brain.OnPleasureAdded -= new FollowerBrain.PleasureEvent(this.Brain_OnPleasureAdded);
    this.Brain.OnPleasureAdded += new FollowerBrain.PleasureEvent(this.Brain_OnPleasureAdded);
    this.timeBetweenEnsuringFollowerIsWithinBounds = 2f;
    if (brain.HasTrait(FollowerTrait.TraitType.OverworkedParent) && (double) UnityEngine.Random.value < 0.10000000149011612)
      brain.AddThought((Thought) UnityEngine.Random.Range(415, 420));
    else if (brain.Info.ID == 99990 && (double) UnityEngine.Random.value < 0.10000000149011612)
      brain.AddThought((Thought) UnityEngine.Random.Range(425, 430));
    else if (brain.Info.ID == 99991 && (double) UnityEngine.Random.value < 0.10000000149011612)
      brain.AddThought((Thought) UnityEngine.Random.Range(430, 435));
    else if (brain.Info.ID == 99992 && (double) UnityEngine.Random.value < 0.10000000149011612)
      brain.AddThought((Thought) UnityEngine.Random.Range(435, 440));
    else if (brain.Info.ID == 99993 && (double) UnityEngine.Random.value < 0.10000000149011612)
      brain.AddThought((Thought) UnityEngine.Random.Range(440, 445));
    else if (brain.Info.ID == 666 && (double) UnityEngine.Random.value < 0.10000000149011612)
      brain.AddThought((Thought) UnityEngine.Random.Range(445, 450));
    else if (FollowerManager.BishopIDs.Contains(brain.Info.ID) && (double) UnityEngine.Random.value < 0.10000000149011612)
      brain.AddThought((Thought) UnityEngine.Random.Range(450, 454));
    if (this.Brain != null && this.Brain.Info.CursedState == Thought.Child)
      this.Brain.CompleteCurrentTask();
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => FollowerBrain.SetFollowerCostume(this.Spine.Skeleton, this.Brain._directInfoAccess, forceUpdate: true)));
  }

  public void CreatePet(FollowerPet.FollowerPetType pet, Vector3 pos, Action<FollowerPet> callback = null)
  {
    FollowerPet.Create(pet, this, pos, callback);
  }

  public void CreatePet(FollowerPet.DLCPet petdata, Vector3 pos, Action<FollowerPet> callback = null)
  {
    FollowerPet.Create(petdata, this, pos, callback);
  }

  public void CreateNewPet(InventoryItem.ITEM_TYPE pet, Vector3 pos, Action<FollowerPet> callback = null)
  {
    FollowerPet.Create(pet, this, pos, callback);
  }

  public void PlayAnimation()
  {
    this.Brain.CompleteCurrentTask();
    this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    double num = (double) this.SetBodyAnimation(this.TestAnimation, true);
  }

  public void MakeDissenter() => this.Brain.MakeDissenter();

  public void MakeSick() => this.Brain.MakeSick();

  public void TestUpgrade()
  {
    this.Brain.AddAdoration(FollowerBrain.AdorationActions.Gift, (System.Action) null);
  }

  public IEnumerator LevelUpRoutine()
  {
    Follower follower = this;
    Debug.Log((object) "Upgrade to disciple routine");
    if (follower.Brain.CurrentTaskType != FollowerTaskType.ManualControl && follower.Brain.CurrentTaskType != FollowerTaskType.AttendTeaching)
    {
      follower.Brain.CompleteCurrentTask();
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_LightManualControl());
    }
    follower.TimedAnimation("level-up", 1.5f);
    yield return (object) new WaitForSecondsRealtime(0.6666667f);
    BiomeConstants.Instance.EmitHeartPickUpVFX(follower.CameraBone.transform.position, 0.0f, "black", "burst_big", false);
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", follower.gameObject.transform.position);
    yield return (object) new WaitForSecondsRealtime(1f);
    if (follower.Brain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      follower.GoTo(follower.Brain.CurrentTask.GetDestination(follower), new System.Action(follower.\u003CLevelUpRoutine\u003Eb__118_0));
    else
      follower.Brain.CompleteCurrentTask();
    Debug.Log((object) "LEVELED UP");
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.Disciple);
    follower.adorationUI.SetObjects();
    TwitchFollowers.SendFollowers();
  }

  public void Resume()
  {
    if (this._isResumed)
      return;
    this._isResumed = true;
    this.Seeker.pathCallback += new OnPathDelegate(this.OnCheckSafeSpawn);
    this.Seeker.StartPath(this.transform.position, LocationManager.LocationManagers[this.Brain.Location].SafeSpawnCheckPosition);
  }

  public void OnCheckSafeSpawn(Path path)
  {
    this.Seeker.pathCallback -= new OnPathDelegate(this.OnCheckSafeSpawn);
    ABPath abPath = (ABPath) path;
    if (path.error || (double) Vector3.Distance(abPath.originalEndPoint, abPath.endPoint) > 1.5 && (double) path.duration > 0.0)
    {
      this.transform.position = path.error || (double) path.duration <= 0.0 ? this.Brain.LastPosition : abPath.originalEndPoint;
      this.EnsureNotZeroZero();
    }
    this.Seeker.pathCallback += new OnPathDelegate(this.StartPath);
    FollowerBrainStats.OnFearLoveChanged += new FollowerBrainStats.StatChangedEvent(this.OnFearLoveChanged);
    this.Brain.OnStateChanged += new Action<FollowerState, FollowerState>(this.OnStateChanged);
    this.Brain.OnTaskChanged += new Action<FollowerTask, FollowerTask>(this.OnTaskChanged);
    if (this.Brain.CurrentTask != null)
    {
      this.OnTaskChanged(this.Brain.CurrentTask, (FollowerTask) null);
      if (this.Brain.CurrentTask != null)
        this.OnFollowerTaskStateChanged(FollowerTaskState.None, this.Brain.CurrentTask.State);
    }
    if (this.Brain.CurrentState == null)
      return;
    this.OnStateChanged(this.Brain.CurrentState, (FollowerState) null);
  }

  public void EnsureNotZeroZero()
  {
    if (!(this.transform.position == Vector3.zero))
      return;
    if ((UnityEngine.Object) TownCentre.Instance != (UnityEngine.Object) null)
      this.transform.position = TownCentre.Instance.RandomPositionInTownCentre();
    else
      this.transform.position = new Vector3((float) UnityEngine.Random.Range(-15, 15), (float) UnityEngine.Random.Range(-5, -15));
    if (this.Brain.CurrentTask == null || this.Brain.CurrentTask.BlockTaskChanges)
      return;
    this.Brain.CompleteCurrentTask();
  }

  public void HideAllFollowerIcons()
  {
    if ((bool) (UnityEngine.Object) this.FollowerWarningIcons)
      this.FollowerWarningIcons.SetActive(false);
    if ((bool) (UnityEngine.Object) this.CompletedQuestIcon)
      this.CompletedQuestIcon.SetActive(false);
    this.GetComponentInChildren<FollowerAdorationUI>(true)?.Hide(false);
    this.GetComponentInChildren<FollowerPleasureUI>(true)?.Hide(false);
    this.GetComponentInChildren<UIFollowerName>(true)?.Hide(false);
    this.BlessedTodayIcon.gameObject.SetActive(false);
    this.WorshipperBubble.Close();
    this.PropagandaIcon.SetActive(false);
  }

  public void ShowAllFollowerIcons(bool excludeLoyaltyBar = true)
  {
    if ((bool) (UnityEngine.Object) this.FollowerWarningIcons)
      this.FollowerWarningIcons.SetActive(true);
    if ((bool) (UnityEngine.Object) this.CompletedQuestIcon)
      this.CompletedQuestIcon.SetActive(DataManager.Instance.CompletedQuestFollowerIDs.Contains(this.Brain.Info.ID));
    if (!excludeLoyaltyBar)
      this.GetComponentInChildren<FollowerAdorationUI>(true)?.Show();
    this.GetComponentInChildren<UIFollowerName>()?.Show();
  }

  public void StartTeleportToTransitionPosition()
  {
    if (this.Brain == null)
      return;
    this.transform.position = this.Brain.LastPosition;
    if (this.Brain.CurrentTask != null)
    {
      Vector3 destination = this.Brain.CurrentTask.GetDestination(this);
      if (destination != Vector3.zero)
        this.transform.position = destination;
    }
    this.EnsureNotZeroZero();
    this.EnsureWithinBounds();
    this.Resume();
  }

  public void EndTeleportToTransitionPosition(Path p)
  {
    this.Seeker.pathCallback -= new OnPathDelegate(this.EndTeleportToTransitionPosition);
    if (!p.error)
    {
      List<Vector3> vectorPath = p.vectorPath;
      int index = (vectorPath.Count - 1) / 2;
      this.transform.position = vectorPath.Count % 2 != 0 ? vectorPath[index] : (vectorPath[index] + vectorPath[index + 1]) / 2f;
      this.EnsureNotZeroZero();
    }
    this.Resume();
  }

  public void Pause()
  {
    if (!this._isResumed)
      return;
    this._isResumed = false;
    this.Seeker.CancelCurrentPathRequest();
    this.Seeker.pathCallback -= new OnPathDelegate(this.StartPath);
    this.Seeker.pathCallback -= new OnPathDelegate(this.OnCheckSafeSpawn);
    this.Seeker.pathCallback -= new OnPathDelegate(this.EndTeleportToTransitionPosition);
    this.ClearPath();
    FollowerBrainStats.OnFearLoveChanged -= new FollowerBrainStats.StatChangedEvent(this.OnFearLoveChanged);
    if (this.Brain == null)
      return;
    this.Brain.OnStateChanged -= new Action<FollowerState, FollowerState>(this.OnStateChanged);
    this.Brain.OnTaskChanged -= new Action<FollowerTask, FollowerTask>(this.OnTaskChanged);
    if (this.Brain.CurrentTask != null)
    {
      this.Brain.CurrentTask.Cleanup(this);
      this.OnTaskChanged((FollowerTask) null, this.Brain.CurrentTask);
    }
    if (this.Brain.CurrentState == null)
      return;
    this.OnStateChanged((FollowerState) null, this.Brain.CurrentState);
  }

  public void SetHealth()
  {
    this.Health.HP = this.Brain.Stats.HP;
    this.Health.totalHP = this.Brain.Stats.MaxHP;
  }

  public bool IgnoreBounds
  {
    get => this.\u003CIgnoreBounds\u003Ek__BackingField;
    set => this.\u003CIgnoreBounds\u003Ek__BackingField = value;
  }

  public void EnsureWithinBounds()
  {
    if (PlayerFarming.Location != FollowerLocation.Base || this.IgnoreBounds)
      return;
    PolygonCollider2D collider = BiomeBaseManager.Instance.Room.Pieces[0].Collider;
    Follower.Points = collider.GetPath(0);
    if (Utils.PointWithinPolygon(this.transform.position, Follower.Points) || collider.OverlapPoint((Vector2) this.transform.position))
      return;
    if ((UnityEngine.Object) TownCentre.Instance != (UnityEngine.Object) null)
      this.transform.position = TownCentre.Instance.RandomPositionInTownCentre();
    else
      this.transform.position = new Vector3((float) UnityEngine.Random.Range(-15, 15), (float) UnityEngine.Random.Range(-5, -15));
  }

  public void Update()
  {
    if (PlayerFarming.Location == FollowerLocation.Base && this.frameCounter % 30 == 0)
    {
      this.timeBetweenEnsuringFollowerIsWithinBounds -= Time.deltaTime;
      if ((double) this.timeBetweenEnsuringFollowerIsWithinBounds <= 0.0 && !this.InGiveSinSequence)
      {
        this.EnsureWithinBounds();
        this.timeBetweenEnsuringFollowerIsWithinBounds = UnityEngine.Random.Range(10f, 20f);
      }
      this.timeBetweenGiggles -= Time.deltaTime;
      if (FollowerBrainStats.IsNudism)
      {
        if (this.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest)
          AudioManager.Instance.StopLoop(this.laughingLoop);
        else if ((double) this.timeBetweenGiggles < 5.0 && (double) this.timeBetweenGiggles > 0.0)
          AudioManager.Instance.StopLoop(this.laughingLoop);
        else if ((double) UnityEngine.Random.value < 0.0005 && (double) this.timeBetweenGiggles <= 0.0 && !MonoSingleton<UIManager>.Instance.MenusBlocked)
        {
          this.timeBetweenGiggles = UnityEngine.Random.Range(5f, 10f);
          this.laughingLoop = (double) UnityEngine.Random.value >= 0.5 ? this.PlayLoopedVO("event:/dialogue/followers/naked_laughing_hehe", this.gameObject) : this.PlayLoopedVO("event:/dialogue/followers/naked_laughing", this.gameObject);
        }
      }
      this.UpdatePropagandaSpeakersThought();
    }
    this.lastPositionUpdateTimer -= Time.deltaTime;
    this.strCurrentTask = this.Brain == null || this.Brain.CurrentTask == null ? "" : FollowerTask.FollowerTaskNames[(int) this.Brain.CurrentTask.Type];
    if (this.State.CURRENT_STATE == StateMachine.State.TimedAction)
    {
      this._timedActionTimer -= !this.UseDeltaTime || this.UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
      if ((double) this._timedActionTimer <= 0.0)
      {
        if (this.State.CURRENT_STATE == StateMachine.State.TimedAction)
          this.State.CURRENT_STATE = StateMachine.State.Idle;
        if (this._timedAction != null)
        {
          System.Action timedAction = this._timedAction;
          this._timedAction = (System.Action) null;
          timedAction();
        }
      }
    }
    else
      this.UpdateMovement();
    if (this.frameCounter % 30 != 0)
      return;
    if (this.Brain != null && PlayerFarming.Location != FollowerLocation.Church && !this.BlessedTodayIcon.gameObject.activeSelf && (this.Brain.Stats.ReceivedBlessing || this.Brain.Stats.Intimidated || this.Brain.Stats.Inspired || this.Brain.Stats.Cuddled))
    {
      this.BlessedTodayIcon.gameObject.SetActive(true);
      this.BlessedTodayIcon.transform.localScale = Vector3.zero;
      DG.Tweening.Sequence s = DOTween.Sequence();
      s.AppendInterval(2f);
      s.AppendCallback((TweenCallback) (() => this.BlessedTodayIcon.transform.localScale = Vector3.one * 3f));
      s.AppendCallback((TweenCallback) (() => AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.transform.position)));
      s.Append((Tween) this.BlessedTodayIcon.transform.DOScale(Vector3.one * 1.25f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true));
    }
    else if (this.BlessedTodayIcon.gameObject.activeSelf && !this.Brain.Stats.ReceivedBlessing && !this.Brain.Stats.Intimidated && !this.Brain.Stats.Inspired && !this.Brain.Stats.Cuddled)
      this.BlessedTodayIcon.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.BlessedTodayIcon.gameObject.SetActive(false))).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    if (this.showingBark && (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) > 7.5)
      this.HideBark();
    if (this.Brain == null || this.Brain.Info.CursedState != Thought.Child || !((UnityEngine.Object) this.Spine != (UnityEngine.Object) null))
      return;
    this.Spine.transform.localScale = Vector3.one * 0.75f;
  }

  public bool LockToGround
  {
    get => this.\u003CLockToGround\u003Ek__BackingField;
    set => this.\u003CLockToGround\u003Ek__BackingField = value;
  }

  public float StoppingDistance => this._stoppingDistance;

  public void LateUpdate()
  {
    if (PlayerFarming.Location != FollowerLocation.Base && PlayerFarming.Location != FollowerLocation.DoorRoom && PlayerFarming.Location != FollowerLocation.Church)
      return;
    if (this.frameCounter % 30 == 0)
    {
      this.LockToGroundPosition = this.transform.position + Vector3.back * 3f;
      if (UnityEngine.Physics.RaycastNonAlloc(this.LockToGroundPosition, Vector3.forward, this.hits, 10f, (int) this.lockToGroundMask) > 0)
      {
        if ((UnityEngine.Object) (this.hits[0].collider as MeshCollider) != (UnityEngine.Object) null)
        {
          this.LockToGroundNewPosition = this.transform.position;
          if (this.LockToGround)
            this.LockToGroundNewPosition.z = this.hits[0].point.z;
          this.transform.position = this.LockToGroundNewPosition;
        }
      }
      else
      {
        this.LockToGroundNewPosition = this.transform.position;
        if (this.LockToGround)
          this.LockToGroundNewPosition.z = 0.0f;
        this.transform.position = this.LockToGroundNewPosition;
      }
      this.SnowFootsteps.gameObject.SetActive((double) Vector3.Distance(this.Brain.LastPosition, this.transform.position) < 1.0);
      if ((double) Vector3.Distance(this.Brain.LastPosition, this.transform.position) > 1.0)
        this.SnowFootsteps.Clear();
      this.Brain.LastPosition = this.transform.position;
      this.WasDistanceCalculatedOnThisFrame = false;
    }
    else
    {
      float z = this.LockToGroundNewPosition.z;
      if (!this.LockToGround)
        z = this.transform.position.z;
      this.LockToGroundNewPosition = this.transform.position;
      this.LockToGroundNewPosition.z = z;
      this.transform.position = this.LockToGroundNewPosition;
    }
    this.frameCounter += 1 + this.instanceIndex;
  }

  public void Tick(float deltaGameTime)
  {
    if (this.Brain == null)
      return;
    if (this.Brain.Info.ID == 100006 && this.Brain.Info.CursedState != Thought.Child && PlayerFarming.Location == FollowerLocation.Base && DataManager.Instance.HasMidasHiding)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      int id;
      bool flag1;
      bool flag2;
      bool flag3;
      if (!this._dying)
      {
        if ((this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_MissionaryComplete)) && this.Brain._directInfoAccess.MissionaryFinished && DataManager.Instance.Followers_OnMissionary_IDs.Contains(this.Brain.Info.ID))
        {
          this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_MissionaryComplete());
          if (this.Brain.CurrentTask != null)
            this.transform.position = this.Brain.CurrentTask.GetDestination(this);
        }
        else if ((this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_OnMissionary) && !(this.Brain.CurrentTask is FollowerTask_ChangeLocation)) && !this.Brain._directInfoAccess.MissionaryFinished && (this.Brain.Location == FollowerLocation.Missionary || this.Brain.Location == FollowerLocation.Base && DataManager.Instance.Followers_OnMissionary_IDs.Contains(this.Brain.Info.ID)))
        {
          FollowerTask nextTask = (FollowerTask) new FollowerTask_OnMissionary();
          nextTask.AnimateOutFromLocation = false;
          this.Brain.HardSwapToTask(nextTask);
          this.Brain.CurrentTask?.Arrive();
        }
        else if ((this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_LeftInTheDungeon) && !(this.Brain.CurrentTask is FollowerTask_ChangeLocation)) && (this.Brain.Location == FollowerLocation.LeftInTheDungeon || this.Brain.Location == FollowerLocation.Base && DataManager.Instance.Followers_LeftInTheDungeon_IDs.Contains(this.Brain.Info.ID)))
        {
          FollowerTask nextTask = (FollowerTask) new FollowerTask_LeftInTheDungeon();
          nextTask.AnimateOutFromLocation = false;
          this.Brain.HardSwapToTask(nextTask);
          this.Brain.CurrentTask?.Arrive();
        }
        else if ((this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_IsDemon) && !(this.Brain.CurrentTask is FollowerTask_ChangeLocation)) && (this.Brain.Location == FollowerLocation.Demon || this.Brain.Location == FollowerLocation.Base && DataManager.Instance.Followers_Demons_IDs.Contains(this.Brain.Info.ID)))
        {
          this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_IsDemon());
          this.Brain.CurrentTask?.Arrive();
        }
        else if (this.Brain.Location == FollowerLocation.Base && !FollowerManager.FollowerLocked(this.Brain.Info.ID) && this.Brain.CanGiveSin() && this.Brain.CurrentTaskType != FollowerTaskType.Floating && DataManager.Instance.PleasureEnabled && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) PlayerFarming.Instance.transform.position) < 5.0 && (this.Brain.CurrentTask == null || !this.Brain.CurrentTask.BlockTaskChanges))
          this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Floating());
        else if (this.Brain.CurrentTaskType != FollowerTaskType.ChosenChild && this.Brain.CurrentTaskType != FollowerTaskType.GetPlayerAttention && this.Brain.CurrentTaskType != FollowerTaskType.ManualControl && this.Brain.CurrentTaskType != FollowerTaskType.LeftInTheDungeon && this.Brain.Info.ID == 100000)
        {
          this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChosenChild());
        }
        else
        {
          if (this.Brain.CurrentTaskType != FollowerTaskType.ChaseWolf && Interaction_WolfBase.WolfTarget > 0 && this.Brain.HasTrait(FollowerTrait.TraitType.WolfHater) && this.Brain.Location == FollowerLocation.Base && !FollowerManager.FollowerLocked(this.Brain.Info.ID) && this.Brain.Info.CursedState == Thought.None && (double) this.Brain._directInfoAccess.Exhaustion <= 0.0 && (this.Brain.CurrentTask == null || !this.Brain.CurrentTask.BlockTaskChanges))
          {
            Interaction_WolfBase closestWolf = Interaction_WolfBase.GetClosestWolf(this.transform.position);
            if ((UnityEngine.Object) closestWolf != (UnityEngine.Object) null)
            {
              if ((closestWolf.CurrentState == Interaction_WolfBase.State.Attacking || closestWolf.CurrentState == Interaction_WolfBase.State.Preying) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) closestWolf.transform.position) < 15.0)
              {
                this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ChaseWolf(closestWolf));
                goto label_86;
              }
              if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) closestWolf.transform.position) < 15.0)
              {
                this.Brain.AddThought(Thought.Wolves);
                goto label_86;
              }
              goto label_86;
            }
          }
          if ((this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_Imprisoned)) && DataManager.Instance.Followers_Imprisoned_IDs.Contains(this.Brain.Info.ID) && this.Brain.CurrentTaskType != FollowerTaskType.ManualControl)
          {
            List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.PRISON);
            foreach (StructureBrain structureBrain in structuresOfType)
            {
              if (structureBrain != null && structureBrain.Data != null && structureBrain.Data.FollowerID == this.Brain.Info.ID)
              {
                this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Imprisoned(structureBrain.Data.ID));
                this.ClearPath();
                this.transform.position = this.Brain.CurrentTask.GetDestination(this);
                this.Brain.LastPosition = this.transform.position;
                this.Brain.CurrentTask.Arrive();
                goto label_86;
              }
            }
            foreach (StructureBrain structureBrain in structuresOfType)
            {
              if (structureBrain != null && structureBrain.Data != null && structureBrain.Data.FollowerID == -1 && !structureBrain.Data.IsCollapsed)
              {
                structureBrain.Data.FollowerID = this.Brain.Info.ID;
                this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Imprisoned(structureBrain.Data.ID));
                this.ClearPath();
                this.transform.position = this.Brain.CurrentTask.GetDestination(this);
                this.Brain.LastPosition = this.transform.position;
                this.Brain.CurrentTask.Arrive();
                goto label_86;
              }
            }
            DataManager.Instance.Followers_Imprisoned_IDs.Remove(this.Brain.Info.ID);
          }
          if (this.State.CURRENT_STATE != StateMachine.State.TimedAction && this.Brain.Location == FollowerLocation.Base)
          {
            Vector3 a = this.Brain.LastPosition;
            if ((UnityEngine.Object) this != (UnityEngine.Object) null && (UnityEngine.Object) this.transform != (UnityEngine.Object) null)
              a = this.transform.position;
            if (this.Brain.CurrentTask == null || !this.Brain.CurrentTask.BlockTaskChanges && !this.Brain.CurrentTask.BlockReactTasks)
            {
              if (this.Brain.Location == FollowerLocation.Base && (this.Brain.CurrentTask == null || this.Brain.CurrentTask != null && !(this.Brain.CurrentTask is FollowerTask_GetAttention) && !this.Brain.CurrentTask.BlockTaskChanges) && DataManager.Instance.CurrentOnboardingFollowerID == this.Brain.Info.ID && !FollowerManager.FollowerLocked(this.Brain.Info.ID, exludeChild: true) && this.Brain.Info.CursedState == Thought.None && (double) this.Brain.Stats.Adoration < (double) this.Brain.Stats.MAX_ADORATION)
              {
                this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.GiveOnboarding, false));
                goto label_86;
              }
              if (this.Brain.Location == FollowerLocation.Base && (this.Brain.CurrentTask == null || this.Brain.CurrentTask != null && !(this.Brain.CurrentTask is FollowerTask_GetAttention) && !this.Brain.CurrentTask.BlockTaskChanges) && !string.IsNullOrEmpty(this.cachedBarkMessage) && !FollowerManager.FollowerLocked(this.Brain.Info.ID, exludeChild: true))
              {
                this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.ShowTwitchMessage, false));
                goto label_86;
              }
              if (this.Brain.HasTrait(FollowerTrait.TraitType.Bastard) && this.Brain.CurrentTask != null && this.Brain.Location == FollowerLocation.Base && !(this.Brain.CurrentTask is FollowerTask_GetAttention) && !FollowerManager.FollowerLocked(this.Brain.Info.ID) && DataManager.Instance.PlayerDeathDay == TimeManager.CurrentDay)
              {
                DataManager.Instance.PlayerDeathDay = -1;
                if ((double) UnityEngine.Random.value < 0.25)
                {
                  this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.Speak, false, $"Bastard_{(this.Brain._directInfoAccess.BastardCounter < 3 ? this.Brain._directInfoAccess.BastardCounter : UnityEngine.Random.Range(0, 3))}/0"));
                  ++this.Brain._directInfoAccess.BastardCounter;
                  goto label_86;
                }
              }
              if (TimeManager.CurrentPhase != DayPhase.Night && this.Brain.Info.CursedState == Thought.None && this.Brain.Info.IsDisciple && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(this.Brain.Info.ID, this.Brain, StructureAndTime.IDTypes.Follower) > 360.0 && (this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_ReeducateDissenter)))
              {
                foreach (Follower follower in Follower.Followers)
                {
                  if (!this.Brain._directInfoAccess.FollowersReeducatedToday.Contains(follower.Brain.Info.ID) && !follower.Brain._directInfoAccess.LeavingCult && follower.Brain.Info.CursedState == Thought.Dissenter && (double) TimeManager.TotalElapsedGameTime - (double) StructureAndTime.GetOrAddTime(follower.Brain.Info.ID, follower.Brain, StructureAndTime.IDTypes.Follower) > 360.0 && follower.Brain.CurrentTaskType != FollowerTaskType.None && follower.Brain.Info.ID != 99996)
                  {
                    StructureAndTime.SetTime(follower.Brain.Info.ID, follower.Brain, StructureAndTime.IDTypes.Follower);
                    StructureAndTime.SetTime(this.Brain.Info.ID, this.Brain, StructureAndTime.IDTypes.Follower);
                    this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ReeducateDissenter(follower.Brain));
                    goto label_86;
                  }
                }
              }
              if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !this.Brain.CheckForLambInteraction(this.transform.position))
              {
                if (this.CheckForUpdate())
                {
                  List<Follower> followerList = FollowerManager.FollowersAtLocation(this.Brain.Location);
                  for (int index = 0; index < followerList.Count; ++index)
                  {
                    if ((UnityEngine.Object) followerList[index] != (UnityEngine.Object) null && (UnityEngine.Object) followerList[index] != (UnityEngine.Object) this && (UnityEngine.Object) followerList[index].transform != (UnityEngine.Object) null)
                    {
                      float distance = Vector3.Distance(a, followerList[index].transform.position);
                      id = followerList[index].Brain.Info.ID;
                      ref int local1 = ref id;
                      flag1 = false;
                      ref bool local2 = ref flag1;
                      flag2 = false;
                      ref bool local3 = ref flag2;
                      // ISSUE: explicit reference operation
                      ref bool local4 = @false;
                      // ISSUE: explicit reference operation
                      ref bool local5 = @true;
                      flag3 = false;
                      ref bool local6 = ref flag3;
                      if (FollowerManager.FollowerLocked(in local1, in local2, in local3, in local4, in local5, in local6))
                      {
                        if (this.Brain.CheckForLockedFollowerInteraction(followerList[index].Brain, distance))
                          goto label_86;
                      }
                      else if (this.Brain.CheckForInteraction(followerList[index].Brain, distance))
                        goto label_86;
                    }
                  }
                }
                if (this.CheckForUpdate())
                {
                  List<Structure> structures = Structure.Structures;
                  for (int index = 0; index < structures.Count; ++index)
                  {
                    if ((UnityEngine.Object) structures[index] != (UnityEngine.Object) null && structures[index].Structure_Info != null)
                    {
                      float CheckDistance = Vector3.Distance(a, structures[index].Structure_Info.Position);
                      if ((double) CheckDistance < 8.0 && this.Brain.CheckForInteraction(structures[index], CheckDistance))
                        goto label_86;
                    }
                  }
                }
              }
              else
                goto label_86;
            }
            else if ((this.Brain.Info.CursedState == Thought.Child || this.Brain.HasTrait(FollowerTrait.TraitType.Zombie)) && (double) this.lastPositionUpdateTimer <= 0.0)
            {
              List<Follower> followerList = FollowerManager.FollowersAtLocation(this.Brain.Location);
              for (int index = 0; index < followerList.Count; ++index)
              {
                if ((UnityEngine.Object) followerList[index] != (UnityEngine.Object) null && (UnityEngine.Object) followerList[index] != (UnityEngine.Object) this && (UnityEngine.Object) followerList[index].transform != (UnityEngine.Object) null)
                {
                  float distance = Vector3.Distance(a, followerList[index].transform.position);
                  id = followerList[index].Brain.Info.ID;
                  ref int local7 = ref id;
                  flag1 = false;
                  ref bool local8 = ref flag1;
                  flag2 = false;
                  ref bool local9 = ref flag2;
                  // ISSUE: explicit reference operation
                  ref bool local10 = @false;
                  // ISSUE: explicit reference operation
                  ref bool local11 = @true;
                  flag3 = false;
                  ref bool local12 = ref flag3;
                  if (FollowerManager.FollowerLocked(in local7, in local8, in local9, in local10, in local11, in local12))
                  {
                    if (this.Brain.CheckForLockedFollowerInteraction(followerList[index].Brain, distance))
                      goto label_86;
                  }
                  else if (this.Brain.CheckForInteraction(followerList[index].Brain, distance))
                    goto label_86;
                }
              }
            }
            if (TimeManager.CurrentPhase == DayPhase.Night && PlayerFarming.Location == FollowerLocation.Base && (double) TimeManager.CurrentPhaseProgress > 0.25 && (double) TimeManager.CurrentPhaseProgress < 0.699999988079071 && this.Brain.CurrentTaskType != FollowerTaskType.StealBed && (double) UnityEngine.Random.value < 9.9999997473787516E-05)
            {
              id = this.Brain.Info.ID;
              ref int local13 = ref id;
              flag1 = false;
              ref bool local14 = ref flag1;
              flag2 = false;
              ref bool local15 = ref flag2;
              // ISSUE: explicit reference operation
              ref bool local16 = @false;
              // ISSUE: explicit reference operation
              ref bool local17 = @true;
              flag3 = false;
              ref bool local18 = ref flag3;
              if (!FollowerManager.FollowerLocked(in local13, in local14, in local15, in local16, in local17, in local18) && this.Brain.HasTrait(FollowerTrait.TraitType.Bastard))
                this.TryStealBed();
            }
          }
        }
label_86:
        this.Brain.Tick(deltaGameTime);
        if ((double) this.lastPositionUpdateTimer <= 0.0)
        {
          if ((double) Vector3.Distance(this.transform.position, this.lastUpdatedPosition) >= 1.0)
            this.lastUpdatedPosition = this.transform.position;
          this.lastPositionUpdateTimer = this.delayBetweenPositionUpdateSpread;
        }
      }
      id = this.Brain.Info.ID;
      ref int local19 = ref id;
      flag1 = false;
      ref bool local20 = ref flag1;
      flag2 = false;
      ref bool local21 = ref flag2;
      // ISSUE: explicit reference operation
      ref bool local22 = @false;
      // ISSUE: explicit reference operation
      ref bool local23 = @true;
      flag3 = false;
      ref bool local24 = ref flag3;
      if (FollowerManager.FollowerLocked(in local19, in local20, in local21, in local22, in local23, in local24) || !this.Brain._directInfoAccess.IsSnowman || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || PlayerFarming.Location != FollowerLocation.Base || this.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Winter || this.Brain.HasTrait(FollowerTrait.TraitType.InfusibleSnowman))
        return;
      this.Die(NotificationCentre.NotificationType.MeltedToDeath, dieAnimation: (double) UnityEngine.Random.value < 0.5 ? "Snowman/melt-shrink" : "Snowman/melt");
    }
  }

  public void UpdatePropagandaSpeakersThought()
  {
    this.Brain.SpeakersInRange = 0;
    for (int index = 0; index < PropagandaSpeaker.PropagandaSpeakers.Count; ++index)
    {
      if (this.Brain.CheckForSpeakers(PropagandaSpeaker.PropagandaSpeakers[index].Structure))
      {
        this.Brain.SpeakersInRange = 1;
        break;
      }
    }
    if (this.Brain.SpeakersInRange > 0 && !FollowerManager.FollowerLocked(this.Brain.Info.ID) && this.Brain.Info.CursedState == Thought.None)
    {
      if (this.Brain.ThoughtExists(Thought.PropogandaSpeakers))
        return;
      this.Brain.AddThought(Thought.PropogandaSpeakers, true);
      if (!(bool) (UnityEngine.Object) this.PropagandaIcon)
        return;
      this.PropagandaIcon.gameObject.SetActive(true);
      this.PropagandaIcon.transform.localScale = Vector3.zero;
      this.PropagandaIcon.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
    else
    {
      if (!this.Brain.ThoughtExists(Thought.PropogandaSpeakers))
        return;
      this.Brain.RemoveThought(Thought.PropogandaSpeakers, true);
      if (!(bool) (UnityEngine.Object) this.PropagandaIcon)
        return;
      this.PropagandaIcon.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.PropagandaIcon.SetActive(false)));
    }
  }

  public bool CheckForUpdate()
  {
    if (this.WasDistanceCalculatedOnThisFrame)
      return this.IsUpdateRequired;
    this.IsUpdateRequired = (double) Vector3.Distance(this.transform.position, this.lastUpdatedPosition) >= 1.0 && (double) this.lastPositionUpdateTimer <= 0.0;
    return this.IsUpdateRequired;
  }

  public bool TryStealBed()
  {
    List<Structures_Bed> structuresBedList = new List<Structures_Bed>((IEnumerable<Structures_Bed>) StructureManager.GetAllStructuresOfType<Structures_Bed>());
    StructureBrain.TYPES types = StructureBrain.TYPES.NONE;
    foreach (Structures_Bed structuresBed in structuresBedList)
    {
      if (structuresBed.Data.ID == this.Brain._directInfoAccess.DwellingID)
      {
        types = structuresBed.Data.Type;
        break;
      }
    }
    foreach (Structures_Bed structuresBed in structuresBedList)
    {
      if (structuresBed.Data.Type > types)
      {
        Follower followerById = FollowerManager.FindFollowerByID(structuresBed.Data.FollowerID);
        if ((UnityEngine.Object) followerById != (UnityEngine.Object) null && !followerById.Brain.Info.IsDisciple)
        {
          this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_StealBed(new Dwelling.DwellingAndSlot(structuresBed.Data.ID, followerById.Brain._directInfoAccess.DwellingSlot, followerById.Brain._directInfoAccess.DwellingLevel)));
          return true;
        }
      }
    }
    return false;
  }

  public bool CheckForInteractionWithPlayer() => false;

  public void OnStateChanged(FollowerState newState, FollowerState oldState)
  {
    oldState?.Cleanup(this);
    newState?.Setup(this);
    this.SetEmotionAnimation();
    this.SetOverrideOutfit();
  }

  public void OnTaskChanged(FollowerTask newTask, FollowerTask oldTask)
  {
    if (oldTask != null)
      oldTask.OnFollowerTaskStateChanged -= new FollowerTask.FollowerTaskDelegate(this.OnFollowerTaskStateChanged);
    if (newTask != null)
    {
      newTask.Setup(this);
      if (oldTask == null || oldTask.Type != FollowerTaskType.ChangeLocation)
        newTask.ClaimReservations();
      newTask.OnFollowerTaskStateChanged += new FollowerTask.FollowerTaskDelegate(this.OnFollowerTaskStateChanged);
    }
    if (!(bool) (UnityEngine.Object) this.Interaction_FollowerInteraction)
      return;
    this.Interaction_FollowerInteraction.enabled = newTask == null || !newTask.DisablePickUpInteraction;
  }

  public void OnFollowerTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
  {
    if (this.Brain.CurrentTask == null)
      return;
    switch (oldState)
    {
      case FollowerTaskState.Idle:
        this.Brain.CurrentTask.OnIdleEnd(this);
        break;
      case FollowerTaskState.GoingTo:
        this.Brain.CurrentTask.OnGoingToEnd(this);
        break;
      case FollowerTaskState.Doing:
        this.Brain.CurrentTask.OnDoingEnd(this);
        break;
      case FollowerTaskState.Finalising:
        this.Brain.CurrentTask.OnFinaliseEnd(this);
        break;
      case FollowerTaskState.Done:
        Debug.Log((object) $"{oldState.ToString()}  {newState.ToString()}");
        break;
    }
    switch (newState)
    {
      case FollowerTaskState.Idle:
        this.Brain.CurrentTask.OnIdleBegin(this);
        break;
      case FollowerTaskState.GoingTo:
        this.Brain.CurrentTask.OnGoingToBegin(this);
        break;
      case FollowerTaskState.Doing:
        this.Brain.CurrentTask.OnDoingBegin(this);
        break;
      case FollowerTaskState.Finalising:
        this.Brain.CurrentTask.OnFinaliseBegin(this);
        break;
      case FollowerTaskState.Done:
        this.Brain.CurrentTask.Cleanup(this);
        this.State.CURRENT_STATE = StateMachine.State.Idle;
        this.ResetStateAnimations();
        break;
    }
  }

  public void ShowCompletedQuestIcon(bool show)
  {
    if (this.completedQuestIconTween != null && this.completedQuestIconTween.active)
      this.completedQuestIconTween.Kill();
    if (show)
    {
      this.CompletedQuestIcon.transform.localScale = Vector3.zero;
      this.CompletedQuestIcon.SetActive(true);
      this.completedQuestIconTween = (Tween) this.CompletedQuestIcon.transform.DOScale(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }
    else
    {
      this.CompletedQuestIcon.transform.localScale = Vector3.one;
      this.completedQuestIconTween = (Tween) this.CompletedQuestIcon.transform.DOScale(0.0f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.CompletedQuestIcon.SetActive(false)));
    }
  }

  public void OnHit(
    GameObject attacker,
    Vector3 attackLocation,
    Health.AttackTypes attackType,
    bool FromBehind)
  {
    this.Brain.Stats.HP = this.Health.HP;
    this.Brain.Stats.Motivate(1);
  }

  public void OnDie(
    GameObject attacker,
    Vector3 attackLocation,
    Health victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.Brain.DiedOfIllness = true;
    this.Brain.CheckChangeTask();
  }

  public void OnFaithChanged(int followerID, float newValue, float oldValue, float change)
  {
    if (followerID != this.Brain.Info.ID)
      return;
    bool flag = (double) change > 0.0;
    UITextPopUp.Create($"{(flag ? (object) "+" : (object) "")}{change}", flag ? Color.green : Color.red, this.gameObject, new Vector3(0.0f, 2f));
  }

  public void OnFearLoveChanged(int followerID, float newValue, float oldValue, float change)
  {
    if (followerID != this.Brain.Info.ID)
      return;
    bool flag = (double) change > 0.0;
    UITextPopUp.Create($"{(flag ? (object) "+" : (object) "")}{change}", flag ? Color.green : Color.red, this.gameObject, new Vector3(0.0f, 2f));
  }

  public void DieWithAnimation(
    string animation,
    string deadAnimation = "dead",
    bool playAnimation = true,
    int dir = 1,
    NotificationCentre.NotificationType deathNotificationType = NotificationCentre.NotificationType.Died,
    Action<GameObject> callback = null,
    bool force = false)
  {
    int deathNotificationType1 = (int) deathNotificationType;
    string str1 = animation;
    string str2 = deadAnimation;
    int num1 = playAnimation ? 1 : 0;
    int Dir = dir;
    string dieAnimation = str1;
    string deadAnimation1 = str2;
    Action<GameObject> callback1 = callback;
    int num2 = force ? 1 : 0;
    this.Die((NotificationCentre.NotificationType) deathNotificationType1, num1 != 0, Dir, dieAnimation, deadAnimation1, callback1, num2 != 0);
  }

  public void Die(
    NotificationCentre.NotificationType deathNotificationType = NotificationCentre.NotificationType.Died,
    bool PlayAnimation = true,
    int Dir = 1,
    string dieAnimation = "die",
    string deadAnimation = "dead",
    Action<GameObject> callback = null,
    bool force = false)
  {
    if (this.deathCoroutine != null)
      return;
    this.deathCoroutine = GameManager.GetInstance().StartCoroutine((IEnumerator) this.FollowerDieIE(deathNotificationType, PlayAnimation, Dir, dieAnimation, deadAnimation, callback, force));
  }

  public IEnumerator FollowerDieIE(
    NotificationCentre.NotificationType deathNotificationType = NotificationCentre.NotificationType.Died,
    bool PlayAnimation = true,
    int Dir = 1,
    string dieAnimation = "die",
    string deadAnimation = "dead",
    Action<GameObject> callback = null,
    bool force = false)
  {
    Follower follower = this;
    bool waiting = true;
    while (true)
    {
      if ((force || PlayerFarming.Location == FollowerLocation.Base && !LetterBox.IsPlaying && !MMConversation.isPlaying && !SimulationManager.IsPaused && follower.Brain.Location == FollowerLocation.Base && PlayerFarming.LongToPerformPlayerStates.Contains(PlayerFarming.Instance.state.CURRENT_STATE)) && !Interaction_WolfBase.WolvesActive)
      {
        if (waiting)
          waiting = false;
        else
          break;
      }
      yield return (object) new WaitForSeconds(0.2f);
    }
    if (follower.Brain._directInfoAccess.IsSnowman && dieAnimation.Equals("die"))
      dieAnimation = "Snowman/die-poof";
    if (follower.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_Deaths_Door && deathNotificationType != NotificationCentre.NotificationType.Ascended && deathNotificationType != NotificationCentre.NotificationType.DiedOnMissionary && deathNotificationType != NotificationCentre.NotificationType.MurderedByYou && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEaten && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEatenBySozo && deathNotificationType != NotificationCentre.NotificationType.MeltedToDeath && deathNotificationType != NotificationCentre.NotificationType.KilledInAFightPit && deathNotificationType != NotificationCentre.NotificationType.SacrificeFollower && deathNotificationType != NotificationCentre.NotificationType.BurntToDeath && deathNotificationType != NotificationCentre.NotificationType.DiedFromRot && deathNotificationType != NotificationCentre.NotificationType.KilledByBlizzardMonster && deathNotificationType != NotificationCentre.NotificationType.SacrificedAwayFromCult)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(follower.gameObject);
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      float animationDuration = follower.SetBodyAnimation(dieAnimation, false);
      yield return (object) new WaitForEndOfFrame();
      follower.ClearPath();
      yield return (object) new WaitForSeconds(animationDuration);
      bool resurrectAsZombie = false;
      string resurectAnimation = (double) UnityEngine.Random.value < 0.5 ? "Deathdoor/resurrect-1" : "Deathdoor/resurrect-2";
      string overrideIdleAnimation = (string) null;
      if (DataManager.Instance.Followers_Imprisoned_IDs.Contains(follower.Brain.Info.ID))
      {
        resurectAnimation = deathNotificationType != NotificationCentre.NotificationType.FrozeToDeath ? "Deathdoor/resurrect-stocks" : "Deathdoor/resurrect-stocks-frozen";
        overrideIdleAnimation = "Prison/stocks";
      }
      else if (deathNotificationType == NotificationCentre.NotificationType.FrozeToDeath)
        resurectAnimation = (double) UnityEngine.Random.value < 0.5 ? "Deathdoor/resurrect-frozen" : "Deathdoor/resurrect-frozen2";
      else if ((double) UnityEngine.Random.value < 0.20000000298023224)
      {
        resurrectAsZombie = true;
        resurectAnimation = "Deathdoor/resurrect-zombie";
        overrideIdleAnimation = "Zombie/zombie-idle";
      }
      if (deathNotificationType == NotificationCentre.NotificationType.DiedFromOldAge)
      {
        follower.Brain.Info.Age -= 10;
        follower.Brain.RemoveCurseState(Thought.OldAge);
        follower.Brain.CheckChangeState();
      }
      follower.Brain.Info.Necklace = InventoryItem.ITEM_TYPE.NONE;
      if (deathNotificationType != NotificationCentre.NotificationType.DiedFromRot)
        BiomeConstants.Instance.EmitSmokeInteractionVFX(follower.transform.position, Vector3.one);
      follower.UpdateOutfit();
      yield return (object) FollowerManager.ResurrectFollower(follower, resurectAnimation, overrideIdleAnimation);
      if (resurrectAsZombie)
      {
        follower.Brain.AddTrait(FollowerTrait.TraitType.Zombie, true);
        follower.Brain.CurrentState = (FollowerState) new FollowerState_Zombie();
        ThoughtData data = FollowerThoughts.GetData((Thought) UnityEngine.Random.Range(454, 458));
        if (data != null)
        {
          data.Init();
          follower.Brain._directInfoAccess.Thoughts.Add(data);
        }
      }
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.ResurrectedFromNecklace, follower.Brain.Info, NotificationFollower.Animation.Happy);
      GameManager.GetInstance().OnConversationEnd();
      follower.deathCoroutine = (Coroutine) null;
    }
    else
    {
      if (deathNotificationType != NotificationCentre.NotificationType.Ascended && deathNotificationType != NotificationCentre.NotificationType.BurntToDeath)
      {
        switch (deathNotificationType)
        {
          case NotificationCentre.NotificationType.DiedFromDeadlyMeal:
            follower.Brain.DiedFromDeadlyDish = true;
            break;
          case NotificationCentre.NotificationType.DiedOnMissionary:
            follower.Brain.DiedFromMissionary = true;
            break;
          case NotificationCentre.NotificationType.StruckByLightning:
            AudioManager.Instance.PlayOneShot("event:/dlc/follower/death_lightning", follower.Brain.LastPosition);
            follower.Brain.DiedFromLightning = true;
            GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => WeatherSystemController.Instance.TriggerLightningStrike(this.Brain.LastPosition)));
            break;
        }
        if (deathNotificationType != NotificationCentre.NotificationType.MurderedByYou && deathNotificationType != NotificationCentre.NotificationType.MurderedByFollower && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEaten && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEatenBySozo && deathNotificationType != NotificationCentre.NotificationType.MeltedToDeath && deathNotificationType != NotificationCentre.NotificationType.KilledByBlizzardMonster)
        {
          if ((UnityEngine.Object) follower.gameObject == (UnityEngine.Object) null)
            yield break;
          SimulationManager.Pause();
          GameManager.GetInstance().OnConversationNew();
          GameManager.GetInstance().OnConversationNext(follower.gameObject);
          string deathText = follower.Brain._directInfoAccess.GetDeathText();
          if (!string.IsNullOrEmpty(deathText))
            LetterBox.Instance.ShowSubtitle(deathText);
          yield return (object) new WaitForSeconds(0.7f);
          AudioManager.Instance.PlayOneShot("event:/Stings/church_bell", follower.gameObject);
          yield return (object) new WaitForSeconds(0.25f);
          SimulationManager.UnPause();
        }
        if (follower.Brain._directInfoAccess.IsSnowman)
          yield return (object) new WaitForSeconds(follower.SetBodyAnimation(dieAnimation, false));
        if (deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEaten && deathNotificationType != NotificationCentre.NotificationType.MeltedToDeath && deathNotificationType != NotificationCentre.NotificationType.KilledByBlizzardMonster && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEatenBySozo)
        {
          StructuresData structure = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
          structure.FollowerID = follower.Brain.Info.ID;
          structure.Dir = Dir;
          if (follower.Brain.FrozeToDeath || follower.Brain.DiedFromOverheating || follower.Brain.BurntToDeath)
            structure.CanBecomeRotten = false;
          switch (deathNotificationType)
          {
            case NotificationCentre.NotificationType.FrozeToDeath:
              AudioManager.Instance.PlayOneShot("event:/dlc/follower/death_freeze", follower.transform.position);
              break;
            case NotificationCentre.NotificationType.DiedFromRot:
              AudioManager.Instance.PlayOneShot("event:/dlc/follower/death_rot", follower.transform.position);
              break;
            default:
              if (dieAnimation == "die")
              {
                Vector3 pos = follower.transform.position;
                GameManager.GetInstance().WaitForSeconds(2.16666675f, (System.Action) (() => AudioManager.Instance.PlayOneShot("event:/dlc/follower/death_fallover", pos)));
                break;
              }
              break;
          }
          StructureManager.BuildStructure(follower.Brain.Location, structure, follower.transform.position, Vector2Int.one, false, (Action<GameObject>) (g =>
          {
            DeadWorshipper component = g.GetComponent<DeadWorshipper>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              CameraFollowTarget.Instance.AddTarget(component.gameObject, 1f);
              component.PlayAnimation = PlayAnimation;
              component.DieAnimation = dieAnimation;
              component.DeadAnimation = deadAnimation;
              component.Setup();
              GameManager.GetInstance().WaitForLetterbox((System.Action) (() =>
              {
                GameManager.GetInstance().CamFollowTarget.ClearAllTargets();
                GameManager.GetInstance().AddPlayersToCamera();
              }));
            }
            if (structure != null)
            {
              PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(structure.Position);
              if (tileAtWorldPosition != null)
                component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
            }
            Action<GameObject> action = callback;
            if (action == null)
              return;
            action(g);
          }), emitParticles: false);
        }
      }
      foreach (FollowerPet dlcFollowerPet in follower.DLCFollowerPets)
      {
        BiomeConstants.Instance.EmitSmokeInteractionVFX(dlcFollowerPet.transform.position, Vector3.one * 0.5f);
        UnityEngine.Object.Destroy((UnityEngine.Object) dlcFollowerPet.gameObject);
      }
      if (follower.Brain != null)
      {
        Dwelling.DwellingAndSlot dwellingAndSlot = follower.Brain.GetDwellingAndSlot();
        if (dwellingAndSlot != null)
        {
          Dwelling dwellingById = Dwelling.GetDwellingByID(dwellingAndSlot.ID);
          if ((UnityEngine.Object) dwellingById != (UnityEngine.Object) null)
            dwellingById.SetBedImage(dwellingAndSlot.dwellingslot, Dwelling.SlotState.UNCLAIMED);
        }
        follower.Brain.Info.NudistWinner = false;
        follower.Brain.Die(deathNotificationType);
      }
      follower.SpawnDieAnimationEffects(dieAnimation);
      UnityEngine.Object.Destroy((UnityEngine.Object) follower.gameObject);
      if (deathNotificationType != NotificationCentre.NotificationType.Ascended && deathNotificationType != NotificationCentre.NotificationType.MurderedByYou && deathNotificationType != NotificationCentre.NotificationType.MurderedByFollower && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEaten && deathNotificationType != NotificationCentre.NotificationType.DiedFromBeingEatenBySozo && deathNotificationType != NotificationCentre.NotificationType.MeltedToDeath && deathNotificationType != NotificationCentre.NotificationType.BurntToDeath && deathNotificationType != NotificationCentre.NotificationType.KilledByBlizzardMonster)
      {
        if (deathNotificationType == NotificationCentre.NotificationType.DiedFromRot)
          yield return (object) new WaitForSeconds(1f);
        yield return (object) new WaitForSeconds(3f);
        GameManager.GetInstance().OnConversationEnd();
      }
    }
  }

  public void SpawnDieAnimationEffects(string dieAnimation)
  {
    if (!this.Brain._directInfoAccess.IsSnowman || !dieAnimation.Equals("Snowman/die-poof"))
      return;
    BiomeConstants.Instance.SpawnPuffEffect(this.transform.position, this.transform.parent);
  }

  public void LeaveWithAnimation()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.LeaveWithAnimationIE());
  }

  public void LeaveWithAnimationSpy()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.LeaveWithAnimationSpyIE());
  }

  public IEnumerator LeaveWithAnimationSpyIE()
  {
    Follower follower = this;
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || follower.Brain.Location != FollowerLocation.Base || !PlayerFarming.LongToPerformPlayerStates.Contains(PlayerFarming.Instance.state.CURRENT_STATE))
      yield return (object) null;
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/tease", follower.gameObject);
    follower.TimedAnimation("Jeer/jeer-at-player2", 3f);
    yield return (object) new WaitForSeconds(3f);
    yield return (object) new WaitForEndOfFrame();
    follower.TimedAnimation("spawn-out", 0.8333333f);
    yield return (object) new WaitForSeconds(0.8333333f);
    Inventory.ChangeItemQuantity(20, -Mathf.Min(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD), Mathf.FloorToInt(follower.Brain.Stats.DissentGold)));
    follower.Brain.CompleteCurrentTask();
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator LeaveWithAnimationIE()
  {
    Follower follower = this;
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || follower.Brain.Location != FollowerLocation.Base || !PlayerFarming.LongToPerformPlayerStates.Contains(PlayerFarming.Instance.state.CURRENT_STATE))
      yield return (object) null;
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    follower.TimedAnimation("wave-angry", 1.93333328f);
    yield return (object) new WaitForSeconds(1.93333328f);
    yield return (object) new WaitForEndOfFrame();
    follower.TimedAnimation("spawn-out-angry", 0.8333333f);
    yield return (object) new WaitForSeconds(0.8333333f);
    Inventory.ChangeItemQuantity(20, -Mathf.Min(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD), Mathf.FloorToInt(follower.Brain.Stats.DissentGold)));
    follower.Brain.CompleteCurrentTask();
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
  }

  public void Leave(
    NotificationCentre.NotificationType leaveNotificationType)
  {
    FollowerManager.FollowerLeave(this.Brain.Info.ID, leaveNotificationType);
    this.Brain.Leave(leaveNotificationType);
    if (!(bool) (UnityEngine.Object) this.gameObject)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void FacePosition(Vector3 positionToFace)
  {
    this.Spine.Skeleton.ScaleX = (double) this.transform.position.x < (double) positionToFace.x ? -1f : 1f;
  }

  public void ClearPath()
  {
    this._currentPath = (List<Vector3>) null;
    this._currentWaypoint = 0;
    this._speed = 0.0f;
    if (!((UnityEngine.Object) this.Seeker != (UnityEngine.Object) null))
      return;
    this.Seeker.CancelCurrentPathRequest();
  }

  public void GoTo(Vector3 destination, System.Action onComplete)
  {
    this.ClearPath();
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
    {
      this.Seeker.StartPath(this.transform.position, destination);
    }
    else
    {
      double num = (double) this.SetBodyAnimation(this.Brain.CurrentState == null || string.IsNullOrEmpty(this.Brain.CurrentState.OverrideWalkAnim) ? this.AnimWalking : this.Brain.CurrentState.OverrideWalkAnim, true);
      this.State.CURRENT_STATE = StateMachine.State.Moving;
      this._currentPath = new List<Vector3>();
      this._currentPath.Add(this.transform.position);
      this._currentPath.Add(destination);
      this._currentWaypoint = 1;
      this._startPos = this.transform.position;
      this._destPos = this._currentPath[1];
    }
    this.timeSincePath = 0.0f;
    this._onPathComplete = onComplete;
  }

  public IEnumerator GoToRoutine(Vector3 destination)
  {
    bool pathComplete = false;
    this.GoTo(destination, (System.Action) (() => pathComplete = true));
    while (!pathComplete)
      yield return (object) null;
  }

  public void StartPath(Path p)
  {
    if (p.error)
      return;
    if (this.State.CURRENT_STATE != StateMachine.State.Moving)
    {
      double num = (double) this.SetBodyAnimation(this.Brain.CurrentState == null || string.IsNullOrEmpty(this.Brain.CurrentState.OverrideWalkAnim) ? this.AnimWalking : this.Brain.CurrentState.OverrideWalkAnim, true);
      this.State.CURRENT_STATE = StateMachine.State.Moving;
    }
    this._currentPath = p.vectorPath;
    this._currentWaypoint = 1;
    this._startPos = this.transform.position;
    this._destPos = this._currentPath[1];
    this._t = 0.0f;
  }

  public void UpdateMovement()
  {
    float num1 = this.UseUnscaledTime ? GameManager.FixedUnscaledDeltaTime : GameManager.FixedDeltaTime;
    if (!this.UsePathing)
    {
      PlayerFarming playerFarming = PlayerFarming.Instance;
      PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(this.transform.position, true, true);
      if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null)
        playerFarming = closestPlayer;
      this.Angle = this.State.facingAngle = Utils.GetAngle(this.transform.position, playerFarming.transform.position);
      this.Angle *= (float) Math.PI / 180f;
      float max = 0.75f * num1;
      this._t = 0.0f;
      if ((double) Vector2.Distance((Vector2) playerFarming.transform.position, (Vector2) this.transform.position) > 2.0)
        this._speed += 0.05f * num1;
      else
        this._speed -= 0.025f * num1;
      if (float.IsNaN(this.State.facingAngle))
        return;
      if (float.IsNaN(this._speed) || float.IsInfinity(this._speed))
        this._speed = 0.0f;
      this._speed = Mathf.Clamp(this._speed, 0.0f, max);
      this.Seperate();
    }
    else if (this._currentPath == null || this._currentWaypoint >= this._currentPath.Count || this.State.CURRENT_STATE != StateMachine.State.Moving)
    {
      this._speed += (float) ((0.0 - (double) this._speed) / 4.0) * num1;
    }
    else
    {
      this.State.facingAngle = Utils.GetAngle(this.transform.position, this._currentPath[this._currentWaypoint]);
      float max = (this.Brain.Location == FollowerLocation.Church || this.Brain.CurrentState == null ? 2.25f : this.Brain.CurrentState.MaxSpeed) * this.SpeedMultiplier;
      if ((double) this._t >= 1.0)
      {
        this._t = 0.0f;
        ++this._currentWaypoint;
        if (this._currentWaypoint == this._currentPath.Count)
        {
          this.ClearPath();
          double num2 = (double) this.SetBodyAnimation(this.Brain.CurrentState == null || string.IsNullOrEmpty(this.Brain.CurrentState.OverrideIdleAnim) ? this.AnimIdle : this.Brain.CurrentState.OverrideIdleAnim, true);
          this.State.CURRENT_STATE = StateMachine.State.Idle;
          System.Action onPathComplete = this._onPathComplete;
          this._onPathComplete = (System.Action) null;
          if (onPathComplete != null)
            onPathComplete();
        }
        else
        {
          this._startPos = this.transform.position;
          this._destPos = this._currentPath[this._currentWaypoint];
        }
      }
      else if (this.Brain != null && (double) this._speed < (double) max)
      {
        this._speed += 1f * num1;
        this._speed = Mathf.Clamp(this._speed, 0.0f, max);
        this._speed *= this.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_2 ? 1.25f : 1f;
      }
      else
        this._speed = Mathf.Clamp(this._speed, 0.0f, max);
    }
    if ((double) this._speed > 0.0 && this.UsePathing)
    {
      float num3 = this._speed / Vector3.Distance(new Vector3(this._startPos.x, this._startPos.y, 0.0f), new Vector3(this._destPos.x, this._destPos.y, 0.0f));
      this.FacePosition(this._destPos);
      this.transform.position = Vector3.Lerp(this._startPos, this._destPos, this._t);
      this._t += num3 * (this.UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
      this._t = Mathf.Clamp01(this._t);
    }
    if (this.Brain?.CurrentTask != null)
    {
      FollowerBrain brain = this.Brain;
      int num4;
      if (brain == null)
      {
        num4 = 0;
      }
      else
      {
        FollowerTaskState? state = brain.CurrentTask?.State;
        FollowerTaskState followerTaskState = FollowerTaskState.GoingTo;
        num4 = state.GetValueOrDefault() == followerTaskState & state.HasValue ? 1 : 0;
      }
      if (num4 != 0 && this._currentPath == null && this.transform.position == this.previousPosition && this.UsePathing)
      {
        this.timeSincePath += Time.deltaTime;
        if ((double) this.timeSincePath > 0.5)
        {
          this.Brain.CurrentTask.ClearDestination();
          this.Brain.CurrentTask.SetState(FollowerTaskState.Wait);
          this.Brain.CurrentTask.SetState(FollowerTaskState.GoingTo);
          this.timeSincePath = 0.0f;
        }
      }
    }
    this.previousPosition = this.transform.position;
  }

  public void FixedUpdate()
  {
    if (this.UsePathing)
      return;
    this.transform.position = this.transform.position + new Vector3(this._speed * Mathf.Cos(this.Angle), this._speed * Mathf.Sin(this.Angle)) * (this.UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
    this.transform.position = this.transform.position + new Vector3(this.seperatorVX, this.seperatorVY) * (this.UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
  }

  public void HideStats()
  {
  }

  public void PickUp()
  {
    if (this.Brain.CurrentTaskType != FollowerTaskType.ManualControl)
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    this.Brain.CurrentTask?.ClearDestination();
    this.State.CURRENT_STATE = StateMachine.State.InActive;
  }

  public void Drop()
  {
    this.transform.position = this.transform.position with
    {
      z = 0.0f
    };
    this.TimedAnimation("put-down", 0.3f, new System.Action(this.Dropped));
    this.HideStats();
  }

  public void Dropped()
  {
    this.Brain.CompleteCurrentTask();
    this.State.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void ApplyEffect(string animation, System.Action effect, float Timer = 1f, bool useDeltaTime = true)
  {
    this.TimedAnimation(animation, Timer, (System.Action) (() => effect()), useDeltaTime);
    this.HideStats();
  }

  public void ResetStateAnimations()
  {
    if (this.Brain.CurrentState != null)
      this.Brain.CurrentState.SetStateAnimations(this);
    else
      this.SimpleAnimator.ResetAnimationsToDefaults();
  }

  public void SetFaceAnimation(string animName, bool loop)
  {
    if (!(this.Spine.AnimationName != animName))
      return;
    this.Spine.AnimationState.SetAnimation(0, animName, loop);
  }

  public float SetBodyAnimation(string animName, bool loop)
  {
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null && this.Spine.AnimationState != null && this.Spine.AnimationName != animName)
    {
      TrackEntry trackEntry = this.Spine.AnimationState.SetAnimation(1, animName, loop);
      return trackEntry != null && trackEntry.Animation != null ? trackEntry.Animation.Duration : 0.0f;
    }
    return this.Spine.AnimationState != null && this.Spine.AnimationState.Tracks != null && this.Spine.AnimationState.Tracks.Count > 0 ? this.Spine.AnimationState.Tracks.Items[0].Animation.Duration : 0.0f;
  }

  public void AddBodyAnimation(string animName, bool loop, float Delay)
  {
    this.Spine.AnimationState.AddAnimation(1, animName, loop, Delay);
  }

  public void SetEmotionAnimation(TrackEntry trackEntry)
  {
    if (trackEntry.TrackIndex != 1)
      return;
    this.SetEmotionAnimation();
    this.SetOverrideOutfit();
  }

  public bool IsBabySad() => (double) this.CalculateCuddleNormDifference() <= 0.0;

  public bool IsBabyAngry() => (double) this.CalculateCuddleNormDifference() <= -2.0;

  public float CalculateCuddleNormDifference()
  {
    return (float) (this.Brain._directInfoAccess.CuddledAmount + 1) - (float) this.Brain.Info.Age / (18f / 3f);
  }

  public void SetEmotionAnimation()
  {
    if (this.OverridingEmotions || this.Brain.Info.HasTrait(FollowerTrait.TraitType.Mutated))
      return;
    if (this.Brain.Info.CursedState == Thought.Dissenter)
      this.SetFaceAnimation("Emotions/emotion-dissenter", true);
    else if (this.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      this.SetFaceAnimation("Emotions/emotion-zombie", true);
    else if (this.Brain.CanGiveSin())
      this.SetFaceAnimation("Emotions/emotion-sin", true);
    else if (this.Brain.CurrentTaskType == FollowerTaskType.RunFromSomething)
      this.SetFaceAnimation("Emotions/emotion-scared", true);
    else if (this.Brain.Stats.HasLevelledUp && !this.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
      this.SetFaceAnimation("Emotions/emotion-enlightened", true);
    else if (FollowerBrainStats.BrainWashed && this.Brain.Info.CursedState != Thought.Child || this.Brain._directInfoAccess.SozoBrainshed)
      this.SetFaceAnimation("Emotions/emotion-brainwashed", true);
    else if (this.Brain.Info.CursedState == Thought.Ill)
      this.SetFaceAnimation("Emotions/emotion-sick", true);
    else if (this.Brain.Info.CursedState == Thought.Soaking)
      this.SetFaceAnimation("Emotions/emotion-soaked", true);
    else if (this.Brain.Info.CursedState == Thought.Freezing)
      this.SetFaceAnimation("Emotions/emotion-freezing", true);
    else if (this.Brain.Info.CursedState == Thought.Overheating)
      this.SetFaceAnimation("Emotions/emotion-overheated", true);
    else if (this.Brain.HasTrait(FollowerTrait.TraitType.Scared) || this.Brain.HasTrait(FollowerTrait.TraitType.CriminalScarred))
      this.SetFaceAnimation("Emotions/emotion-scared", true);
    else if (this.Brain.Info.CursedState == Thought.Injured)
      this.SetFaceAnimation("Emotions/emotion-unhappy", true);
    else if (this.Brain.Info.IsDrunk && this.Brain.CurrentState != null && this.Brain.CurrentState.Type == FollowerStateType.Drunk)
    {
      FollowerState_Drunk currentState = this.Brain.CurrentState as FollowerState_Drunk;
      if (currentState.AngryDrunk)
        this.SetFaceAnimation("Emotions/emotion-drunk-angry", true);
      else if (currentState.SadDrunk)
        this.SetFaceAnimation("Emotions/emotion-drunk-sad", true);
      else
        this.SetFaceAnimation("Emotions/emotion-drunk", true);
    }
    else if (this.Brain.CurrentTaskType == FollowerTaskType.ChaseFollower)
    {
      if (!(this.Brain.CurrentTask is FollowerTask_ChaseFollower currentTask))
        return;
      this.SetFaceAnimation(currentTask.IsLeader ? "Emotions/emotion-angry" : "Emotions/emotion-scared", true);
    }
    else if ((double) this.Brain.Stats.Rest <= 20.0 || this.Brain.HasTrait(FollowerTrait.TraitType.OverworkedParent) || this.Brain.HasTrait(FollowerTrait.TraitType.Drowsy))
      this.SetFaceAnimation("Emotions/emotion-tired", true);
    else if ((double) CultFaithManager.CurrentFaith >= 0.0 && (double) CultFaithManager.CurrentFaith <= 25.0)
      this.SetFaceAnimation("Emotions/emotion-angry", true);
    else if ((double) CultFaithManager.CurrentFaith > 25.0 && (double) CultFaithManager.CurrentFaith <= 40.0)
      this.SetFaceAnimation("Emotions/emotion-unhappy", true);
    else if ((double) CultFaithManager.CurrentFaith > 40.0 && (double) CultFaithManager.CurrentFaith <= 80.0)
    {
      this.SetFaceAnimation("Emotions/emotion-normal", true);
    }
    else
    {
      if ((double) CultFaithManager.CurrentFaith <= 75.0)
        return;
      this.SetFaceAnimation("Emotions/emotion-happy", true);
    }
  }

  public void SetHat(FollowerHatType hatType)
  {
    this.Brain.Info.Hat = hatType;
    this.SetOverrideOutfit(true);
  }

  public void SetOverrideOutfit(bool forceUpdate = false)
  {
    if (this.OverridingOutfit || this.Brain.CurrentTaskType == FollowerTaskType.MissionaryComplete || this.Brain.CurrentTaskType == FollowerTaskType.MissionaryInProgress || this.Brain.CurrentTaskType == FollowerTaskType.ChangeLocation)
      return;
    FollowerBrain.SetFollowerCostume(this.Spine.skeleton, this.Brain._directInfoAccess, forceUpdate: forceUpdate);
  }

  public void SetOutfit(FollowerOutfitType outfitType, bool hooded, Thought overrideCursedState = Thought.None)
  {
    this.Brain._directInfoAccess.Outfit = outfitType;
    this.Outfit.SetInfo(this.Brain._directInfoAccess);
    this.Outfit.SetOutfit(this.Spine, outfitType, this.Brain.Info.Necklace, hooded, overrideCursedState);
  }

  public void BlessFollower(Vector3 targetPos)
  {
    this.blessFollowerCoroutine = this.StartCoroutine((IEnumerator) this.BlessFollowerIE(targetPos));
  }

  public void BlessFollowerInterruptedCallback()
  {
    if (this.blessFollowerCoroutine == null)
      return;
    this.StopCoroutine(this.blessFollowerCoroutine);
    this.blessFollowerCoroutine = (Coroutine) null;
    this.Brain.AddAdoration(FollowerBrain.AdorationActions.Bless, (System.Action) (() =>
    {
      CultFaithManager.AddThought(Thought.Cult_Bless, this.Brain.Info.ID);
      this.Brain.CompleteCurrentTask();
    }));
  }

  public IEnumerator BlessFollowerIE(Vector3 targetPos)
  {
    FollowerTaskType currentTaskType = this.Brain.CurrentTaskType;
    this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    this.Brain.Stats.ReceivedBlessing = true;
    this.FacePosition(targetPos);
    List<FollowerTask> unoccupiedTasksOfType = FollowerBrain.GetAllUnoccupiedTasksOfType(currentTaskType);
    FollowerTask task = (FollowerTask) null;
    if (unoccupiedTasksOfType.Count > 0)
      task = unoccupiedTasksOfType[UnityEngine.Random.Range(0, unoccupiedTasksOfType.Count)];
    if (task != null)
      task.ClaimReservations();
    this.State.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) null;
    double num1 = (double) this.SetBodyAnimation("devotion/devotion-start", false);
    this.AddBodyAnimation("devotion/devotion-waiting", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    this.blessFollowerCoroutine = (Coroutine) null;
    this.Brain.AddAdoration(FollowerBrain.AdorationActions.Bless, (System.Action) (() =>
    {
      CultFaithManager.AddThought(Thought.Cult_Bless, this.Brain.Info.ID);
      if ((UnityEngine.Object) this != (UnityEngine.Object) null && this.gameObject.activeInHierarchy)
      {
        double num2 = (double) this.SetBodyAnimation("idle", true);
      }
      if (task != null)
        this.Brain.HardSwapToTask(task);
      else
        this.Brain.CompleteCurrentTask();
    }));
  }

  public void ButtonDie() => this.Die();

  public void LeaveCult() => this.Brain.LeavingCult = true;

  public void DebugSkinName() => Debug.Log((object) this.Brain.Info.SkinName);

  public void GetAttentionTask()
  {
    this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(this.Brain.GetMostPressingComplaint()));
  }

  public void AddThought(Thought ThoughtType) => this.Brain.AddThought(ThoughtType);

  public void RemoveCursedState(Thought ThoughtType) => this.Brain.RemoveCurseState(ThoughtType);

  public void AddTrait(FollowerTrait.TraitType Trait, bool showNotification = false)
  {
    this.Brain.AddTrait(Trait, showNotification);
  }

  public void AddBathroom() => this.Brain.Stats.Bathroom += 10f;

  public void CheckRole()
  {
    Debug.Log((object) $"{this.Brain.Info.Name}   {this.Brain.Info.FollowerRole.ToString()}");
  }

  public void SetWorshipper()
  {
    this.Brain.Info.FollowerRole = FollowerRole.Worshipper;
    this.Brain.CompleteCurrentTask();
    this.SetOutfit(FollowerOutfitType.Follower, false);
  }

  public void SetMonk()
  {
    this.Brain.Info.FollowerRole = FollowerRole.Monk;
    this.Brain.CompleteCurrentTask();
    this.SetOutfit(FollowerOutfitType.Follower, false);
  }

  public void SetWorker()
  {
    this.Brain.Info.FollowerRole = FollowerRole.Worker;
    this.Brain.CompleteCurrentTask();
    this.SetOutfit(FollowerOutfitType.Follower, false);
  }

  public void HoodOn(string animation, bool snap)
  {
    if (snap)
    {
      this.OverridingOutfit = true;
      this.Outfit.SetOutfit(this.Spine, this.Brain.Info.Outfit, this.Brain.Info.Necklace, true);
      double num = (double) this.SetBodyAnimation(animation, true);
    }
    else
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.PutHoodOnRoutine(animation));
  }

  public void HoodOff(string animation = "idle", bool snap = false, System.Action onComplete = null)
  {
    this.StartCoroutine((IEnumerator) this.HoodOffWaitForEndOfFrame(animation, snap, onComplete));
  }

  public IEnumerator HoodOffWaitForEndOfFrame(string animation = "idle", bool snap = false, System.Action onComplete = null)
  {
    Follower follower = this;
    if (follower.Outfit.IsHooded)
    {
      if (snap)
      {
        follower.OverridingOutfit = false;
        follower.Outfit.SetOutfit(follower.Spine, follower.Brain.Info.Outfit, follower.Brain.Info.Necklace, false);
        double num = (double) follower.SetBodyAnimation(animation, true);
        System.Action action = onComplete;
        if (action != null)
          action();
      }
      else
      {
        yield return (object) null;
        follower.StartCoroutine((IEnumerator) follower.TakeHoodOffRoutine(animation, onComplete));
      }
    }
    else
    {
      System.Action action = onComplete;
      if (action != null)
        action();
    }
  }

  public IEnumerator PutHoodOnRoutine(string animation)
  {
    this.OverridingOutfit = true;
    float waitDuration;
    if (!this.Outfit.IsHooded)
    {
      double num = (double) this.SetBodyAnimation(this.AnimHoodUp, false);
      this.AddBodyAnimation(animation, true, 0.0f);
      waitDuration = 0.6333333f;
      while ((double) (waitDuration -= Time.deltaTime) > 0.0)
        yield return (object) null;
      this.Outfit.SetOutfit(this.Spine, this.Brain.Info.Outfit, this.Brain.Info.Necklace, true, setData: false);
    }
    else
    {
      double num1 = (double) this.SetBodyAnimation(animation, true);
    }
    waitDuration = 0.333333343f;
    while ((double) (waitDuration -= Time.deltaTime) > 0.0)
      yield return (object) null;
  }

  public IEnumerator TakeHoodOffRoutine(string animation = "idle", System.Action onComplete = null)
  {
    double num = (double) this.SetBodyAnimation(this.AnimHoodDown, false);
    this.AddBodyAnimation(animation, true, 0.0f);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this.OverridingOutfit = false;
    this.Outfit.SetOutfit(this.Spine, this.Brain.Info.Outfit, this.Brain.Info.Necklace, false);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    System.Action action = onComplete;
    if (action != null)
      action();
  }

  public void TimedAnimationWithHood(
    string animation,
    float timer,
    System.Action onComplete = null,
    bool Loop = true,
    bool UseDeltaTime = true)
  {
  }

  public void TimedAnimation(
    string animation,
    float timer,
    System.Action onComplete = null,
    bool Loop = true,
    bool useDeltaTime = true)
  {
    this.UseDeltaTime = useDeltaTime;
    this.Spine.UseDeltaTime = useDeltaTime;
    if (this.State.CURRENT_STATE == StateMachine.State.TimedAction)
    {
      System.Action timedAction = this._timedAction;
      this._timedAction = (System.Action) null;
      this._timedActionTimer = 0.0f;
      if (timedAction != null)
        timedAction();
    }
    if (this._dying)
      return;
    this.ClearPath();
    this._timedActionTimer = timer;
    this.State.CURRENT_STATE = StateMachine.State.TimedAction;
    double num = (double) this.SetBodyAnimation(animation, Loop);
    this._timedAction = onComplete;
  }

  public void TimedAnimationWithDuration(
    string animation,
    System.Action onComplete = null,
    bool Loop = true,
    bool useDeltaTime = true,
    string NextAnim = "idle")
  {
    this.UseDeltaTime = useDeltaTime;
    this.Spine.UseDeltaTime = useDeltaTime;
    if (this.State.CURRENT_STATE == StateMachine.State.TimedAction)
    {
      this._timedActionTimer = 0.0f;
      System.Action timedAction = this._timedAction;
      if (timedAction != null)
        timedAction();
      this._timedAction = (System.Action) null;
    }
    if (this._dying)
      return;
    this.ClearPath();
    float num = this.SetBodyAnimation(animation, Loop);
    this.AddBodyAnimation(NextAnim, true, 0.0f);
    this._timedActionTimer = num;
    this.State.CURRENT_STATE = StateMachine.State.TimedAction;
    this._timedAction = onComplete;
  }

  public void OnThoughtModified(Thought thought)
  {
    switch (thought)
    {
      case Thought.Brainwashed:
      case Thought.Dissenter:
      case Thought.Ill:
      case Thought.Holiday:
      case Thought.Cult_CureDissenter:
      case Thought.Cult_MushroomEncouraged_Trait:
        this.SetOutfit(this.Outfit.CurrentOutfit, this.Outfit.IsHooded);
        break;
      case Thought.Cult_NudistRitual:
        if (!FollowerBrainStats.IsNudism)
          this.Brain.Info.NudistWinner = false;
        this.Brain.CheckChangeState();
        break;
      case Thought.Cult_PurgeRitual:
        this.Brain.CheckChangeState();
        break;
    }
  }

  public void Brain_OnCursedStateRemoved()
  {
    this.SetOutfit(this.Outfit.CurrentOutfit, this.Outfit.IsHooded);
  }

  public void PlayerGetSoul(int devotion)
  {
    this.Brain.Stats.DevotionGiven += devotion;
    PlayerFarming.Instance.GetSoul(devotion);
  }

  public void LightningStrikeIncoming()
  {
    this.Interaction_FollowerInteraction.LightningStrikeIncoming();
  }

  public void HideBark()
  {
    this.simpleBark.Close();
    this.showingBark = false;
    this.ShowAllFollowerIcons();
  }

  public void ShowBarkMessageTest() => this.cachedBarkMessage = "Test test testerinooo";

  public void ShowBarkMessage(string message) => this.cachedBarkMessage = message;

  public void ShowTwitchMessage()
  {
    if (this.Brain.CurrentTask is FollowerTask_GetAttention currentTask)
      currentTask.StopSpeechBubble(this);
    this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    string anim = $"Conversations/talk-nice{UnityEngine.Random.Range(1, 4)}";
    if (this.cachedBarkMessage.Contains("love"))
      anim = $"Conversations/talk-love{UnityEngine.Random.Range(1, 4)}";
    else if (this.cachedBarkMessage.Contains("hate"))
      anim = $"Conversations/talk-hate{UnityEngine.Random.Range(1, 4)}";
    double num;
    GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() => num = (double) this.SetBodyAnimation(anim, true)));
    GameManager.GetInstance().WaitForSeconds(7.5f, (System.Action) (() => this.Brain.CompleteCurrentTask()));
    this.ShowBarkMessage();
  }

  public void ShowBarkMessage()
  {
    this.simpleBark.timer = 7.5f;
    this.simpleBark.Entries = new List<ConversationEntry>(1)
    {
      new ConversationEntry(this.gameObject, this.cachedBarkMessage)
    };
    this.simpleBark.ActivateDistance = 0.0f;
    if (this.Spine.GetComponent<Renderer>().isVisible)
    {
      this.HideAllFollowerIcons();
      this.simpleBark.Entries[0].CharacterName = this.Brain.Info.Name;
      this.simpleBark.Renderer = this.Spine.GetComponent<Renderer>();
      this.simpleBark.enabled = true;
      this.simpleBark.Show();
      this.simpleBark.OnClose += (SimpleBark.NormalEvent) (() =>
      {
        this.showingBark = false;
        this.ShowAllFollowerIcons();
      });
      MMConversation.mmConversation.SpeechBubble.ForceOffset = true;
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = -20f;
      MMConversation.mmConversation.SpeechBubble.transform.localScale = Vector3.one * 0.75f;
    }
    this.cachedBarkMessage = "";
    this.showingBark = true;
  }

  public void Brain_OnPleasureAdded()
  {
    if (this.Brain != null && this.Brain.CanGiveSin())
      this.SetEmotionAnimation();
    if (this.pleasureRoutine != null)
      this.StopCoroutine(this.pleasureRoutine);
    if (PlayerFarming.Location != FollowerLocation.Base && PlayerFarming.Location != FollowerLocation.Church && PlayerFarming.Location != FollowerLocation.Dungeon1_5 && PlayerFarming.Location != FollowerLocation.Dungeon1_6)
      return;
    if ((UnityEngine.Object) this.PleasureUI != (UnityEngine.Object) null)
      this.pleasureRoutine = this.StartCoroutine((IEnumerator) this.PleasureUI.IncreasePleasure());
    if (this.Brain == null || !this.Brain.CanGiveSin() || PlayerFarming.Location != FollowerLocation.Church && PlayerFarming.Location != FollowerLocation.Dungeon1_5 && PlayerFarming.Location != FollowerLocation.Dungeon1_6)
      return;
    this.GiveSinToPlayer((System.Action) null);
  }

  public void GiveSinToPlayer(System.Action Callback)
  {
    Debug.Log((object) nameof (GiveSinToPlayer));
    this.StartCoroutine((IEnumerator) this.GiveSinToPlayerRoutine(Callback));
  }

  public IEnumerator GiveSinToPlayerRoutine(System.Action Callback)
  {
    Follower follower = this;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain != null && allBrain.CurrentTaskType == FollowerTaskType.GetPlayerAttention)
        allBrain.CompleteCurrentTask();
    }
    follower.InGiveSinSequence = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject, 8f);
    follower.State.LockStateChanges = true;
    if (!follower.Spine.AnimationState.GetCurrent(1).Animation.Name.Contains("sin"))
    {
      yield return (object) new WaitForSeconds(1f);
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      SimulationManager.Pause();
      yield return (object) new WaitForEndOfFrame();
      double num = (double) follower.SetBodyAnimation("Sin/sin-start", false);
      follower.AddBodyAnimation("Sin/sin-floating", true, 0.0f);
      yield return (object) new WaitForSeconds(1.5f);
    }
    else
    {
      SimulationManager.Pause();
      yield return (object) new WaitForSeconds(1f);
    }
    int direction = (double) follower.transform.position.x < (double) PlayerFarming.Instance.transform.position.x ? 1 : -1;
    bool WaitForPlayer = true;
    Vector3 TargetPosition = follower.transform.position + new Vector3(1.35f * (float) direction, -0.1f);
    string[] strArray = new string[1]{ "Island" };
    Collider2D collider2D = Physics2D.OverlapCircle((Vector2) TargetPosition, 1.5f, LayerMask.GetMask(strArray));
    if ((UnityEngine.Object) collider2D != (UnityEngine.Object) null && ((double) collider2D.ClosestPoint((Vector2) TargetPosition).x < (double) TargetPosition.x && direction == 1 || (double) collider2D.ClosestPoint((Vector2) TargetPosition).x > (double) TargetPosition.x && direction == -1))
      TargetPosition = follower.transform.position + new Vector3(1.35f * (float) -direction, -0.1f);
    Vector3 closestPoint1;
    if (!BiomeGenerator.PointWithinIsland(TargetPosition, out closestPoint1))
    {
      TargetPosition = closestPoint1;
      Vector3 closestPoint2;
      if (!BiomeGenerator.PointWithinIsland(TargetPosition, out closestPoint2))
      {
        Vector3[] vector3Array = new Vector3[3]
        {
          follower.transform.position + new Vector3(1.35f * (float) -direction, -0.1f),
          follower.transform.position + new Vector3(0.0f, 1.35f),
          follower.transform.position + new Vector3(0.0f, -1.35f)
        };
        foreach (Vector3 point in vector3Array)
        {
          Vector3 closestPoint3;
          if (BiomeGenerator.PointWithinIsland(point, out closestPoint3))
          {
            TargetPosition = closestPoint3;
            break;
          }
        }
        if (!BiomeGenerator.PointWithinIsland(TargetPosition, out closestPoint2))
          TargetPosition = follower.transform.position;
      }
    }
    PlayerFarming.Instance.GoToAndStop(TargetPosition, follower.gameObject, GoToCallback: (System.Action) (() =>
    {
      PlayerFarming.Instance.transform.DOMove(TargetPosition, 0.3f);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      AudioManager.Instance.PlayOneShot("event:/player/collect_sin_from_fol", PlayerFarming.Instance.gameObject);
      PlayerFarming.Instance.simpleSpineAnimator.Animate("Sin/collect", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
      this.FacePosition(PlayerFarming.Instance.transform.position);
      WaitForPlayer = false;
    }), maxDuration: 3f);
    bool WillPossess = false;
    if (DataManager.Instance.PreviousSinPointFollowers.Count >= DataManager.MAX_PREV_SIN)
      DataManager.Instance.PreviousSinPointFollowers.RemoveAt(0);
    DataManager.Instance.PreviousSinPointFollowers.Add(follower.Brain.Info.ID);
    int num1 = 0;
    foreach (int sinPointFollower in DataManager.Instance.PreviousSinPointFollowers)
    {
      if (sinPointFollower == follower.Brain.Info.ID)
        ++num1;
    }
    WillPossess = num1 >= 3;
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.SeenGod) & WillPossess)
      WillPossess = (double) UnityEngine.Random.value <= 0.20000000298023224;
    Debug.Log((object) (">>> SinPointCount: " + num1.ToString()));
    while (WaitForPlayer)
      yield return (object) null;
    double num2 = (double) follower.SetBodyAnimation(!WillPossess ? "Sin/sin-collect" : "Sin/sin-collect-floating", false);
    follower.PleasureUI.BarController.SetBarSize(0.0f, false, true);
    ++DataManager.Instance.pleasurePointsRedeemed;
    follower.AddBodyAnimation(!WillPossess ? "idle" : "Sin/sin-floating", true, 0.0f);
    GameObject godTear = (GameObject) null;
    godTear = UnityEngine.Object.Instantiate<GameObject>(follower.rewardPrefab, follower.Spine.transform.position + new Vector3(0.0f, -0.1f, -1f), Quaternion.identity, follower.transform.parent);
    godTear.transform.localScale = Vector3.zero;
    godTear.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    GameManager.GetInstance().OnConversationNext(godTear, 6f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.1f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/possessed/sinned_vom", follower.gameObject);
    yield return (object) new WaitForSeconds(0.4f);
    PlayerSimpleInventory simpleInventory = PlayerFarming.Instance.simpleInventory;
    godTear.transform.DOMove(new Vector3(simpleInventory.ItemImage.transform.position.x, simpleInventory.ItemImage.transform.position.y, -1f), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.25f);
    follower.Brain.Info.Pleasure = 0;
    follower.Spine.transform.DOKill();
    follower.Spine.transform.DOLocalMoveZ(0.0f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(UnityEngine.Random.Range(0.0f, 1f)).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.Brain.CheckChangeState()));
    if (DataManager.Instance.pleasurePointsRedeemed <= 2)
    {
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
      AudioManager.Instance.PlayOneShot("event:/Stings/sins_snake_sting", follower.gameObject);
      yield return (object) new WaitForSeconds(1.25f);
    }
    else
      AudioManager.Instance.PlayOneShot("event:/Stings/sins_snake_sting", follower.gameObject);
    Inventory.AddItem(154, 1);
    godTear.transform.DOScale(0.0f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) godTear.gameObject)));
    if (PlayerFarming.Location == FollowerLocation.Base && !DataManager.Instance.pleasurePointsRedeemedFollowerSpoken || PlayerFarming.Location == FollowerLocation.Church && !DataManager.Instance.pleasurePointsRedeemedTempleFollowerSpoken)
    {
      Vector3 vector3 = follower.transform.position + Vector3.right * 1.5f * (float) direction;
      Vector3 closestPoint4;
      if (!BiomeGenerator.PointWithinIsland(vector3, out closestPoint4))
      {
        vector3 = closestPoint4;
        if (!BiomeGenerator.PointWithinIsland(vector3, out Vector3 _))
        {
          Vector3 closestPoint5;
          vector3 = BiomeGenerator.PointWithinIsland(follower.transform.position + Vector3.right * 1.5f * (float) -direction, out closestPoint5) ? closestPoint5 : follower.transform.position;
        }
      }
      PlayerFarming.Instance.GoToAndStop(vector3, follower.gameObject);
      yield return (object) new WaitForSeconds(1f);
      List<ConversationEntry> Entries = new List<ConversationEntry>();
      if (PlayerFarming.Location == FollowerLocation.Base && !DataManager.Instance.pleasurePointsRedeemedFollowerSpoken)
      {
        Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerFirstSin/0"));
        Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerFirstSin/1"));
        DataManager.Instance.pleasurePointsRedeemedFollowerSpoken = true;
      }
      if (PlayerFarming.Location == FollowerLocation.Church && !DataManager.Instance.pleasurePointsRedeemedTempleFollowerSpoken)
      {
        Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerFirstTempleSin/0"));
        Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerFirstTempleSin/1"));
        DataManager.Instance.pleasurePointsRedeemedTempleFollowerSpoken = true;
      }
      foreach (ConversationEntry conversationEntry in Entries)
      {
        conversationEntry.CharacterName = follower.Brain.Info.Name;
        conversationEntry.DefaultAnimation = "Conversations/talk-nice1";
        conversationEntry.Animation = "Conversations/talk-nice1";
      }
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
      yield return (object) null;
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      if (!ObjectiveManager.GroupExists("Objectives/GroupTitles/Pleasure") && DataManager.Instance.TempleLevel == -1)
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Pleasure", Objectives.CustomQuestTypes.SpendSinInTemple), true);
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    else
      yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.pleasurePointsRedeemed >= 5 && DataManager.Instance.TailorEnabled && !DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Special_5) && PlayerFarming.Location == FollowerLocation.Base && !FoundItemPickUp.IsOutfitPickUpActive(FollowerClothingType.Special_5))
      follower.StartCoroutine((IEnumerator) follower.GiveOutfitIE());
    follower.State.LockStateChanges = false;
    follower.PleasureUI.BarController.SetBarSize(0.0f, false, true);
    if (WillPossess)
    {
      follower.BecomePossessed((System.Action) (() =>
      {
        if (PlayerFarming.Location != FollowerLocation.Church)
          SimulationManager.UnPause();
        this.InGiveSinSequence = false;
        System.Action action = Callback;
        if (action == null)
          return;
        action();
      }), true);
    }
    else
    {
      follower.InGiveSinSequence = false;
      if (PlayerFarming.Location != FollowerLocation.Church)
      {
        SimulationManager.UnPause();
        follower.Brain.CompleteCurrentTask();
      }
      follower.Brain.CheckChangeState();
      System.Action action = Callback;
      if (action != null)
        action();
    }
  }

  public IEnumerator GiveOutfitIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Follower follower = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().OnConversationEnd();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, follower.transform.position).GetComponent<FoundItemPickUp>();
    component.clothingType = FollowerClothingType.Special_5;
    GameManager.GetInstance().OnConversationNext(component.gameObject, 6f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void FloatOutOfChurch()
  {
  }

  public void BecomePossessed(System.Action Callback, bool Force)
  {
    this.StartCoroutine((IEnumerator) this.BecomePossessedIE(Callback, Force));
  }

  public IEnumerator BecomePossessedIE(System.Action Callback, bool Force)
  {
    Follower follower = this;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) null;
    double num1 = (double) follower.SetBodyAnimation("Sin/sin-floating", true);
    while (!Force && (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || !PlayerFarming.LongToPerformPlayerStates.Contains(PlayerFarming.Instance.state.CURRENT_STATE)))
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject, 6f);
    yield return (object) new WaitForSeconds(1.5f);
    if (DataManager.Instance.damnedConversation <= 2)
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>();
      switch (DataManager.Instance.damnedConversation)
      {
        case 0:
          Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerDamn0/0"));
          Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerDamn0/1"));
          break;
        case 1:
          Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerDamn1/0"));
          Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerDamn1/1"));
          break;
        case 2:
          Entries.Add(new ConversationEntry(follower.gameObject, "FollowerInteractions/FollowerDamn2/0"));
          break;
      }
      ++DataManager.Instance.damnedConversation;
      if (DataManager.Instance.damnedConversation >= 3)
        DataManager.Instance.damnedConversation = 0;
      foreach (ConversationEntry conversationEntry in Entries)
      {
        conversationEntry.CharacterName = follower.Brain.Info.Name;
        conversationEntry.Animation = "Sin/sin-floating-talk";
      }
      follower.snakeLoop = AudioManager.Instance.CreateLoop("event:/Stings/sin_snake_music", true, false);
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, new System.Action(follower.\u003CBecomePossessedIE\u003Eb__253_0)), false);
      yield return (object) null;
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/Stings/thenight_sacrifice_followers");
    double num2 = (double) follower.SetBodyAnimation("Sin/sin-spawn-out", false);
    yield return (object) new WaitForSeconds(1.0333333f);
    follower.HideAllFollowerIcons();
    follower.Goop.transform.localScale = Vector3.zero;
    follower.Goop.gameObject.SetActive(true);
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.Append((Tween) follower.Goop.transform.DOScale(1f, 0.35f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    s.AppendInterval(1f);
    s.Append((Tween) follower.Goop.transform.DOScale(0.0f, 0.35f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack));
    follower.Goop.transform.DORotate(new Vector3(0.0f, 0.0f, 90f), 5f);
    yield return (object) new WaitForSeconds(1.73333335f);
    yield return (object) new WaitForSeconds(1f);
    Ritual.FollowerToAttendSermon.Remove(follower.Brain);
    ChurchFollowerManager.Instance?.RemoveBrainFromAudience(follower.Brain);
    follower.Leave(NotificationCentre.NotificationType.BecamePossessed);
    GameManager.GetInstance().OnConversationEnd(ShowHUD: PlayerFarming.Location != 0);
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public void UpdateOutfit()
  {
    FollowerBrain.SetFollowerCostume(this.Spine.Skeleton, this.Brain._directInfoAccess, forceUpdate: true);
  }

  public void Seperate()
  {
    this.seperatorVX = 0.0f;
    this.seperatorVY = 0.0f;
    foreach (Follower follower in Follower.Followers)
    {
      if ((!((UnityEngine.Object) follower != (UnityEngine.Object) null) || !((UnityEngine.Object) follower != (UnityEngine.Object) this) || !follower.UsePathing) && !((UnityEngine.Object) follower == (UnityEngine.Object) this))
      {
        float num = Vector2.Distance((Vector2) follower.gameObject.transform.position, (Vector2) this.transform.position);
        float angle = Utils.GetAngle(follower.gameObject.transform.position, this.transform.position);
        if ((double) num < 0.5)
        {
          this.seperatorVX += (float) ((0.5 - (double) num) / 2.0) * Mathf.Cos(angle * ((float) Math.PI / 180f)) * GameManager.FixedDeltaTime;
          this.seperatorVY += (float) ((0.5 - (double) num) / 2.0) * Mathf.Sin(angle * ((float) Math.PI / 180f)) * GameManager.FixedDeltaTime;
        }
      }
    }
  }

  public void PlayVO(string soundPath, GameObject obj)
  {
    float followerPitch = this.Brain._directInfoAccess.follower_pitch;
    float followerVibrato = this.Brain._directInfoAccess.follower_vibrato;
    AudioManager.Instance.PlayOneShotAndSetParametersValue(soundPath, "follower_pitch", followerPitch, "follower_vibrato", followerVibrato, "followerIsRotten", this.Brain.HasTrait(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f, obj.transform);
  }

  public EventInstance PlayLoopedVO(string soundPath, GameObject obj)
  {
    EventInstance loop = AudioManager.Instance.CreateLoop(soundPath, obj);
    AudioManager.Instance.SetEventInstanceParameter(loop, "follower_pitch", this.Brain._directInfoAccess.follower_pitch);
    AudioManager.Instance.SetEventInstanceParameter(loop, "follower_vibrato", this.Brain._directInfoAccess.follower_vibrato);
    AudioManager.Instance.SetEventInstanceParameter(loop, "followerIsRotten", this.Brain.HasTrait(FollowerTrait.TraitType.Mutated) ? 1f : 0.0f);
    AudioManager.Instance.SetEventInstanceParameter(loop, "followerIsSnowlamb", this.Brain.Info.IsSnowman ? 1f : 0.0f);
    AudioManager.Instance.PlayLoop(loop);
    return loop;
  }

  public void SeasonsManager_OnSeasonChanged(SeasonsManager.Season newSeason)
  {
    this.Brain.CheckChangeState();
  }

  [CompilerGenerated]
  public void \u003CInit\u003Eb__109_0()
  {
    FollowerBrain.SetFollowerCostume(this.Spine.Skeleton, this.Brain._directInfoAccess, forceUpdate: true);
  }

  [CompilerGenerated]
  public void \u003CLevelUpRoutine\u003Eb__118_0()
  {
    this.Brain.CurrentTask.SetState(FollowerTaskState.Doing);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__138_1()
  {
    this.BlessedTodayIcon.transform.localScale = Vector3.one * 3f;
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__138_2()
  {
    AudioManager.Instance.PlayOneShot("event:/followers/gain_loyalty", this.transform.position);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__138_0() => this.BlessedTodayIcon.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003CUpdatePropagandaSpeakersThought\u003Eb__151_0()
  {
    this.PropagandaIcon.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CShowCompletedQuestIcon\u003Eb__166_0()
  {
    this.CompletedQuestIcon.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CBlessFollowerInterruptedCallback\u003Eb__212_0()
  {
    CultFaithManager.AddThought(Thought.Cult_Bless, this.Brain.Info.ID);
    this.Brain.CompleteCurrentTask();
  }

  [CompilerGenerated]
  public void \u003CShowBarkMessage\u003Eb__242_0()
  {
    this.showingBark = false;
    this.ShowAllFollowerIcons();
  }

  [CompilerGenerated]
  public void \u003CBecomePossessedIE\u003Eb__253_0()
  {
    AudioManager.Instance.StopLoop(this.snakeLoop);
  }

  public enum ComplaintType
  {
    Hunger,
    Homeless,
    Sick,
    ReadyToLevelUp,
    NeedBetterHouse,
    FirstTimeSpeakingToPlayer,
    Grateful,
    None,
    GiveQuest,
    CompletedQuest,
    FailedQuest,
    GiveOnboarding,
    ShowTwitchMessage,
    Speak,
  }
}
