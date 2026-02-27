// Decompiled with JetBrains decompiler
// Type: Follower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using Pathfinding;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
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
  private float _timedActionTimer;
  private System.Action _timedAction;
  private bool _dying;
  private List<Vector3> _currentPath;
  private int _currentWaypoint;
  private Vector3 _startPos;
  private float _t;
  private Vector3 _destPos;
  private float _speed;
  private float _stoppingDistance = 0.1f;
  private System.Action _onPathComplete;
  private bool _isResumed;
  public Material NormalMaterial;
  public Material BW_Material;
  private SimpleBark simpleBark;
  public string strCurrentTask = "";
  private FollowerAdorationUI adorationUI;
  private static readonly int LeaderEncounterColorBoost = Shader.PropertyToID("_LeaderEncounterColorBoost");
  public System.Action OnFollowerBrainAssigned;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string TestAnimation;
  public System.Action OnUpgradeDiscipleRoutineComplete;
  private RaycastHit LockToGroundHit;
  private Vector3 LockToGroundPosition;
  private Vector3 LockToGroundNewPosition;
  private Tween completedQuestIconTween;
  private bool UseDeltaTime = true;

  public void PlayParticles() => this.ParticleSystem.Play();

  public FollowerBrain Brain { get; private set; }

  public FollowerOutfit Outfit { get; private set; }

  public bool IsPaused => !this._isResumed;

  public bool UseUnscaledTime { get; set; }

  public bool OverridingEmotions { get; set; }

  public bool OverridingOutfit { get; set; }

  public HatType CurrentHat { get; private set; }

  public FollowerAdorationUI AdorationUI => this.adorationUI;

  private void Start()
  {
    if ((bool) (UnityEngine.Object) CultFaithManager.Instance)
      CultFaithManager.Instance.OnThoughtModified += new CultFaithManager.ThoughtEvent(this.OnThoughtModified);
    this.adorationUI = this.GetComponentInChildren<FollowerAdorationUI>(true);
    this.BlessedTodayIcon.gameObject.SetActive(false);
    this.simpleBark = this.GetComponent<SimpleBark>();
    if ((UnityEngine.Object) this.simpleBark != (UnityEngine.Object) null)
      this.simpleBark.enabled = false;
    foreach (FollowerPet.FollowerPetType pet in this.Brain._directInfoAccess.Pets)
      FollowerPet.Create(pet, this);
  }

  private void OnDestroy()
  {
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
  }

  private void OnEnable()
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
  }

  private void OnDisable()
  {
    Follower.Followers.Remove(this);
    this.Pause();
    this.Spine.AnimationState.Start -= new Spine.AnimationState.TrackEntryDelegate(this.SetEmotionAnimation);
  }

  public void Init(FollowerBrain brain, FollowerOutfit outfit)
  {
    this.Brain = brain;
    this.Outfit = outfit;
    if (!DataManager.Instance.DLC_Cultist_Pack && DataManager.CultistDLCSkins.Contains(this.Brain.Info.SkinName.StripNumbers()))
    {
      WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[brain.Info.SkinCharacter];
      brain.Info.SkinColour = character.SlotAndColours.RandomIndex<WorshipperData.SlotsAndColours>();
      brain.Info.SkinCharacter = WorshipperData.Instance.GetRandomAvailableSkin(true, true);
      brain.Info.SkinVariation = WorshipperData.Instance.GetColourData(WorshipperData.Instance.GetSkinNameFromIndex(brain.Info.SkinCharacter)).Skin.RandomIndex<WorshipperData.CharacterSkin>();
      brain.Info.SkinName = character.Skin[0].Skin;
    }
    this.Outfit.SetOutfit(this.Spine, false);
    this.Spine.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.SetEmotionAnimation);
    if ((bool) (UnityEngine.Object) this.SimpleAnimator)
      this.SimpleAnimator.AnimationTrack = 1;
    System.Action followerBrainAssigned = this.OnFollowerBrainAssigned;
    if (followerBrainAssigned != null)
      followerBrainAssigned();
    this.Brain.SavedFollowerTaskDestination = Vector3.zero;
    if (brain.Location == FollowerLocation.Base && DataManager.Instance.CompletedQuestFollowerIDs.Contains(brain.Info.ID))
    {
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective is Objectives_TalkToFollower && objective.Follower == brain.Info.ID && string.IsNullOrEmpty(((Objectives_TalkToFollower) objective).ResponseTerm))
        {
          this.ShowCompletedQuestIcon(true);
          return;
        }
      }
      DataManager.Instance.CompletedQuestFollowerIDs.Remove(brain.Info.ID);
    }
    if (FollowerManager.FollowerLocked(this.Brain.Info.ID))
      return;
    if (this.Brain.ThoughtExists(Thought.PropogandaSpeakers))
      this.PropagandaIcon.gameObject.SetActive(true);
    if (this.Brain.ThoughtExists(Thought.PropogandaSpeakers))
      return;
    this.PropagandaIcon.SetActive(false);
  }

  private void PlayAnimation()
  {
    this.Brain.CompleteCurrentTask();
    this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    double num = (double) this.SetBodyAnimation(this.TestAnimation, true);
  }

  private void MakeDissenter() => this.Brain.MakeDissenter();

  private void MakeSick() => this.Brain.MakeSick();

  private void TestUpgrade()
  {
    this.Brain.AddAdoration(FollowerBrain.AdorationActions.Gift, (System.Action) null);
  }

  private IEnumerator InstantUpgradeToDisciple(System.Action OnBecomeDiscipleComplete)
  {
    this.PlayParticles();
    this.SetOutfit(FollowerOutfitType.Follower, false);
    yield return (object) null;
    System.Action action = OnBecomeDiscipleComplete;
    if (action != null)
      action();
  }

  public IEnumerator UpgradeToDiscipleRoutine()
  {
    Follower follower = this;
    Debug.Log((object) "Upgrade to disciple routine");
    if (follower.Brain.CurrentTaskType != FollowerTaskType.ManualControl && follower.Brain.CurrentTaskType != FollowerTaskType.AttendTeaching)
    {
      follower.Brain.CompleteCurrentTask();
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    }
    follower.TimedAnimation("level-up", 1.5f);
    yield return (object) new WaitForSecondsRealtime(0.6666667f);
    BiomeConstants.Instance.EmitHeartPickUpVFX(follower.CameraBone.transform.position, 0.0f, "black", "burst_big", (double) Time.timeScale == 1.0);
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", follower.gameObject.transform.position);
    yield return (object) new WaitForSecondsRealtime(1f);
    if (follower.Brain.CurrentTaskType != FollowerTaskType.AttendTeaching)
      follower.Brain.CompleteCurrentTask();
    Debug.Log((object) "LEVELED UP");
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.Disciple);
    follower.adorationUI.SetObjects();
  }

  public void Resume()
  {
    if (this._isResumed)
      return;
    this._isResumed = true;
    this.Seeker.pathCallback += new OnPathDelegate(this.OnCheckSafeSpawn);
    this.Seeker.StartPath(this.transform.position, LocationManager.LocationManagers[this.Brain.Location].SafeSpawnCheckPosition);
  }

  private void OnCheckSafeSpawn(Path path)
  {
    this.Seeker.pathCallback -= new OnPathDelegate(this.OnCheckSafeSpawn);
    ABPath abPath = (ABPath) path;
    if (path.error || (double) Vector3.Distance(abPath.originalEndPoint, abPath.endPoint) > 1.5)
      this.transform.position = abPath.originalEndPoint;
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

  public void HideAllFollowerIcons()
  {
    if ((bool) (UnityEngine.Object) this.FollowerWarningIcons)
      this.FollowerWarningIcons.SetActive(false);
    if ((bool) (UnityEngine.Object) this.CompletedQuestIcon)
      this.CompletedQuestIcon.SetActive(false);
    this.GetComponentInChildren<FollowerAdorationUI>(true)?.Hide();
    this.GetComponentInChildren<UIFollowerTwitchName>(true)?.Hide();
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
    this.GetComponentInChildren<UIFollowerTwitchName>()?.Show();
  }

  public void StartTeleportToTransitionPosition()
  {
    if (this.Brain.CurrentTask != null)
    {
      Vector3 destination = this.Brain.CurrentTask.GetDestination(this);
      if (this.Brain.CurrentTask != null && (this.Brain.CurrentTask.State == FollowerTaskState.None || this.Brain.CurrentTask.State == FollowerTaskState.GoingTo))
      {
        this.Seeker.pathCallback += new OnPathDelegate(this.EndTeleportToTransitionPosition);
        this.Seeker.StartPath(this.Brain.LastPosition, destination);
      }
      else
      {
        this.transform.position = destination;
        this.Resume();
      }
    }
    else
    {
      this.transform.position = this.Brain.LastPosition;
      this.Resume();
    }
  }

  public void EndTeleportToTransitionPosition(Path p)
  {
    this.Seeker.pathCallback -= new OnPathDelegate(this.EndTeleportToTransitionPosition);
    if (!p.error)
    {
      List<Vector3> vectorPath = p.vectorPath;
      int index = (vectorPath.Count - 1) / 2;
      this.transform.position = vectorPath.Count % 2 != 0 ? vectorPath[index] : (vectorPath[index] + vectorPath[index + 1]) / 2f;
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

  private void SetHealth()
  {
    this.Health.HP = this.Brain.Stats.HP;
    this.Health.totalHP = this.Brain.Stats.MaxHP;
  }

  private void Update()
  {
    this.strCurrentTask = this.Brain == null || this.Brain.CurrentTask == null ? "" : this.Brain.CurrentTask.Type.ToString();
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
    if (!this.BlessedTodayIcon.gameObject.activeSelf && (this.Brain.Stats.ReceivedBlessing || this.Brain.Stats.Intimidated || this.Brain.Stats.Inspired))
    {
      this.BlessedTodayIcon.gameObject.SetActive(true);
      this.BlessedTodayIcon.transform.localScale = Vector3.zero;
      DG.Tweening.Sequence s = DOTween.Sequence();
      s.AppendInterval(2f);
      s.Append((Tween) this.BlessedTodayIcon.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    }
    else
    {
      if (!this.BlessedTodayIcon.gameObject.activeSelf || this.Brain.Stats.ReceivedBlessing || this.Brain.Stats.Intimidated || this.Brain.Stats.Inspired)
        return;
      this.BlessedTodayIcon.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.BlessedTodayIcon.gameObject.SetActive(false)));
    }
  }

  private void LateUpdate()
  {
    if (PlayerFarming.Location != FollowerLocation.Base || (double) Time.timeScale == 0.0)
      return;
    this.LockToGroundPosition = this.transform.position + Vector3.back * 3f;
    if (UnityEngine.Physics.Raycast(this.LockToGroundPosition, Vector3.forward, out this.LockToGroundHit, 4f))
    {
      if ((UnityEngine.Object) this.LockToGroundHit.collider.gameObject.GetComponent<MeshCollider>() != (UnityEngine.Object) null)
      {
        this.LockToGroundNewPosition = this.transform.position;
        this.LockToGroundNewPosition.z = this.LockToGroundHit.point.z;
        this.transform.position = this.LockToGroundNewPosition;
      }
    }
    else
    {
      this.LockToGroundNewPosition = this.transform.position;
      this.LockToGroundNewPosition.z = 0.0f;
      this.transform.position = this.LockToGroundNewPosition;
    }
    this.Brain.LastPosition = this.transform.position;
  }

  public void Tick(float deltaGameTime)
  {
    if (this.Brain == null || this._dying)
      return;
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
    else if ((this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_IsDemon) && !(this.Brain.CurrentTask is FollowerTask_ChangeLocation)) && (this.Brain.Location == FollowerLocation.Demon || this.Brain.Location == FollowerLocation.Base && DataManager.Instance.Followers_Demons_IDs.Contains(this.Brain.Info.ID)))
    {
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_IsDemon());
      this.Brain.CurrentTask?.Arrive();
    }
    else
    {
      if ((this.Brain.CurrentTask == null || !(this.Brain.CurrentTask is FollowerTask_Imprisoned)) && DataManager.Instance.Followers_Imprisoned_IDs.Contains(this.Brain.Info.ID))
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
            goto label_48;
          }
        }
        foreach (StructureBrain structureBrain in structuresOfType)
        {
          if (structureBrain != null && structureBrain.Data != null && structureBrain.Data.FollowerID == -1)
          {
            structureBrain.Data.FollowerID = this.Brain.Info.ID;
            this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Imprisoned(structureBrain.Data.ID));
            this.ClearPath();
            this.transform.position = this.Brain.CurrentTask.GetDestination(this);
            this.Brain.LastPosition = this.transform.position;
            this.Brain.CurrentTask.Arrive();
            goto label_48;
          }
        }
        DataManager.Instance.Followers_Imprisoned_IDs.Remove(this.Brain.Info.ID);
      }
      if (this.State.CURRENT_STATE != StateMachine.State.TimedAction && this.Brain.Location == FollowerLocation.Base)
      {
        Vector3 a = this.Brain.LastPosition;
        if ((UnityEngine.Object) this.transform != (UnityEngine.Object) null && (UnityEngine.Object) this != (UnityEngine.Object) null)
          a = this.transform.position;
        if (this.Brain.CurrentTask == null || !this.Brain.CurrentTask.BlockTaskChanges && !this.Brain.CurrentTask.BlockReactTasks)
        {
          if (this.Brain.CurrentTask != null && this.Brain.Location == FollowerLocation.Base && !(this.Brain.CurrentTask is FollowerTask_GetAttention) && DataManager.Instance.CurrentOnboardingFollowerID == this.Brain.Info.ID && !FollowerManager.FollowerLocked(this.Brain.Info.ID) && this.Brain.Info.CursedState == Thought.None && (double) this.Brain.Stats.Adoration < (double) this.Brain.Stats.MAX_ADORATION)
          {
            this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_GetAttention(Follower.ComplaintType.GiveOnboarding, false));
          }
          else
          {
            foreach (Follower follower in FollowerManager.FollowersAtLocation(this.Brain.Location))
            {
              if ((UnityEngine.Object) follower != (UnityEngine.Object) null && (UnityEngine.Object) follower != (UnityEngine.Object) this && (UnityEngine.Object) follower.transform != (UnityEngine.Object) null && (double) Vector3.Distance(a, follower.transform.position) < 6.0 && !FollowerManager.FollowerLocked(follower.Brain.Info.ID))
              {
                if (this.Brain.CheckForInteraction(follower.Brain))
                  goto label_48;
              }
            }
            this.Brain.SpeakersInRange = 0;
            foreach (Structure structure in Structure.Structures)
            {
              if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Structure_Info != null)
              {
                float CheckDistance = Vector3.Distance(a, structure.Structure_Info.Position);
                if ((double) CheckDistance < 8.0)
                {
                  if (this.Brain.CheckForInteraction(structure, CheckDistance))
                    break;
                }
              }
            }
          }
        }
      }
    }
label_48:
    this.Brain.Tick(deltaGameTime);
    if (Time.frameCount % 5 != 0)
      return;
    this.Brain.SpeakersInRange = 0;
    foreach (Structure structure in Structure.Structures)
    {
      if (this.Brain.CheckForSpeakers(structure))
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

  private bool CheckForInteractionWithPlayer() => false;

  private void OnStateChanged(FollowerState newState, FollowerState oldState)
  {
    oldState?.Cleanup(this);
    newState?.Setup(this);
    this.SetEmotionAnimation();
    this.SetOverrideOutfit();
  }

  private void OnTaskChanged(FollowerTask newTask, FollowerTask oldTask)
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
    this.Interaction_FollowerInteraction.enabled = newTask == null || !newTask.DisablePickUpInteraction;
  }

  private void OnFollowerTaskStateChanged(FollowerTaskState oldState, FollowerTaskState newState)
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
        Debug.Log((object) $"{(object) oldState}  {(object) newState}");
        throw new ArgumentException("Should never change a Task state once it's Done! " + this.Brain.Info.Name);
    }
    switch (newState)
    {
      case FollowerTaskState.None:
        throw new ArgumentException("Should never change a Task state back to None!");
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

  private void OnHit(
    GameObject attacker,
    Vector3 attackLocation,
    Health.AttackTypes attackType,
    bool FromBehind)
  {
    this.Brain.Stats.HP = this.Health.HP;
    this.Brain.Stats.Motivate(1);
  }

  private void OnDie(
    GameObject attacker,
    Vector3 attackLocation,
    Health victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.Brain.DiedOfIllness = true;
    this.Brain.CheckChangeTask();
  }

  private void OnFaithChanged(int followerID, float newValue, float oldValue, float change)
  {
    if (followerID != this.Brain.Info.ID)
      return;
    bool flag = (double) change > 0.0;
    UITextPopUp.Create($"{(flag ? (object) "+" : (object) "")}{change}", flag ? Color.green : Color.red, this.gameObject, new Vector3(0.0f, 2f));
  }

  private void OnFearLoveChanged(int followerID, float newValue, float oldValue, float change)
  {
    if (followerID != this.Brain.Info.ID)
      return;
    bool flag = (double) change > 0.0;
    UITextPopUp.Create($"{(flag ? (object) "+" : (object) "")}{change}", flag ? Color.green : Color.red, this.gameObject, new Vector3(0.0f, 2f));
  }

  public void DieWithAnimation(
    string animation,
    float duration,
    string deadAnimation = "dead",
    bool playAnimation = true,
    int dir = 1,
    NotificationCentre.NotificationType deathNotificationType = NotificationCentre.NotificationType.Died,
    Action<GameObject> callback = null,
    bool force = false)
  {
    this.TimedAnimation(animation, duration, (System.Action) (() =>
    {
      int deathNotificationType1 = (int) deathNotificationType;
      string str = deadAnimation;
      int num1 = playAnimation ? 1 : 0;
      int Dir = dir;
      string deadAnimation1 = str;
      Action<GameObject> callback1 = callback;
      int num2 = force ? 1 : 0;
      this.Die((NotificationCentre.NotificationType) deathNotificationType1, num1 != 0, Dir, deadAnimation1, callback1, num2 != 0);
    }));
    this._dying = true;
  }

  public void Die(
    NotificationCentre.NotificationType deathNotificationType = NotificationCentre.NotificationType.Died,
    bool PlayAnimation = true,
    int Dir = 1,
    string deadAnimation = "dead",
    Action<GameObject> callback = null,
    bool force = false)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FollowerDieIE(deathNotificationType, PlayAnimation, Dir, deadAnimation, callback, force));
  }

  private IEnumerator FollowerDieIE(
    NotificationCentre.NotificationType deathNotificationType = NotificationCentre.NotificationType.Died,
    bool PlayAnimation = true,
    int Dir = 1,
    string deadAnimation = "dead",
    Action<GameObject> callback = null,
    bool force = false)
  {
    Follower follower = this;
    while (!force && (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || follower.Brain.Location != FollowerLocation.Base || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving))
      yield return (object) null;
    if (deathNotificationType != NotificationCentre.NotificationType.Ascended)
    {
      if (deathNotificationType == NotificationCentre.NotificationType.DiedFromDeadlyMeal)
        follower.Brain.DiedFromDeadlyDish = true;
      if (deathNotificationType != NotificationCentre.NotificationType.MurderedByYou)
      {
        SimulationManager.Pause();
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(follower.gameObject);
        string deathText = follower.Brain._directInfoAccess.GetDeathText();
        if (!string.IsNullOrEmpty(deathText))
          LetterBox.Instance.ShowSubtitle(deathText);
        yield return (object) new WaitForSeconds(1f);
        SimulationManager.UnPause();
      }
      StructuresData structure = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
      structure.FollowerID = follower.Brain.Info.ID;
      structure.Dir = Dir;
      StructureManager.BuildStructure(follower.Brain.Location, structure, follower.transform.position, Vector2Int.one, false, (Action<GameObject>) (g =>
      {
        DeadWorshipper component = g.GetComponent<DeadWorshipper>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.PlayAnimation = PlayAnimation;
          component.DeadAnimation = deadAnimation;
          component.Setup();
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
      }));
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
      follower.Brain.Die(deathNotificationType);
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) follower.gameObject);
    if (deathNotificationType != NotificationCentre.NotificationType.Ascended && deathNotificationType != NotificationCentre.NotificationType.MurderedByYou)
    {
      yield return (object) new WaitForSeconds(3f);
      GameManager.GetInstance().OnConversationEnd();
    }
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
      double num = (double) this.SetBodyAnimation(this.AnimWalking, true);
      this.State.CURRENT_STATE = StateMachine.State.Moving;
      this._currentPath = new List<Vector3>();
      this._currentPath.Add(this.transform.position);
      this._currentPath.Add(destination);
      this._currentWaypoint = 1;
      this._startPos = this.transform.position;
      this._destPos = this._currentPath[1];
    }
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
      double num = (double) this.SetBodyAnimation(this.AnimWalking, true);
      this.State.CURRENT_STATE = StateMachine.State.Moving;
    }
    this._currentPath = p.vectorPath;
    this._currentWaypoint = 1;
    this._startPos = this.transform.position;
    this._destPos = this._currentPath[1];
    this._t = 0.0f;
  }

  private void UpdateMovement()
  {
    float num1 = this.UseUnscaledTime ? GameManager.FixedUnscaledDeltaTime : GameManager.FixedDeltaTime;
    if (this._currentPath == null || this._currentWaypoint >= this._currentPath.Count || this.State.CURRENT_STATE != StateMachine.State.Moving)
    {
      this._speed += (float) ((0.0 - (double) this._speed) / 4.0) * num1;
    }
    else
    {
      this.State.facingAngle = Utils.GetAngle(this.transform.position, this._currentPath[this._currentWaypoint]);
      if ((double) this._t >= 1.0)
      {
        this._t = 0.0f;
        ++this._currentWaypoint;
        if (this._currentWaypoint == this._currentPath.Count)
        {
          this.ClearPath();
          double num2 = (double) this.SetBodyAnimation(this.AnimIdle, true);
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
      else if (this.Brain != null && this.Brain.CurrentState != null && (double) this._speed < (double) this.Brain.CurrentState.MaxSpeed)
      {
        this._speed += 1f * num1;
        this._speed = Mathf.Clamp(this._speed, 0.0f, this.Brain.Location == FollowerLocation.Church ? 2.25f : this.Brain.CurrentState.MaxSpeed);
        this._speed *= this.Brain.Info.Necklace == InventoryItem.ITEM_TYPE.Necklace_2 ? 1.25f : 1f;
      }
    }
    if ((double) this._speed <= 0.0)
      return;
    float num3 = this._speed / Vector3.Distance(this._startPos, this._destPos);
    this.FacePosition(this._destPos);
    this.transform.position = Vector3.Lerp(this._startPos, this._destPos, this._t);
    this._t += num3 * (this.UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
    this._t = Mathf.Clamp01(this._t);
  }

  public void HideStats()
  {
  }

  public void PickUp()
  {
    if (this.Brain.CurrentTaskType != FollowerTaskType.ManualControl)
      this.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    this.Brain.CurrentTask.ClearDestination();
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
    this.Spine.AnimationState.SetAnimation(0, animName, loop);
  }

  public float SetBodyAnimation(string animName, bool loop)
  {
    return this.Spine.AnimationState.SetAnimation(1, animName, loop).Animation.Duration;
  }

  public void AddBodyAnimation(string animName, bool loop, float Delay)
  {
    this.Spine.AnimationState.AddAnimation(1, animName, loop, Delay);
  }

  private void SetEmotionAnimation(TrackEntry trackEntry)
  {
    if (trackEntry.TrackIndex != 1)
      return;
    this.SetEmotionAnimation();
    this.SetOverrideOutfit();
  }

  private void SetEmotionAnimation()
  {
    if (this.OverridingEmotions)
      return;
    this.SetFaceAnimation("Emotions/emotion-normal", true);
    if (this.Brain.Info.CursedState == Thought.Dissenter)
      this.SetFaceAnimation("Emotions/emotion-dissenter", true);
    else if (this.Brain.Stats.HasLevelledUp)
      this.SetFaceAnimation("Emotions/emotion-enlightened", true);
    else if (FollowerBrainStats.BrainWashed)
      this.SetFaceAnimation("Emotions/emotion-brainwashed", true);
    else if (this.Brain.Info.CursedState == Thought.Ill)
      this.SetFaceAnimation("Emotions/emotion-sick", true);
    else if ((double) this.Brain.Stats.Rest <= 20.0)
    {
      this.SetFaceAnimation("Emotions/emotion-tired", true);
    }
    else
    {
      if ((double) CultFaithManager.CurrentFaith >= 0.0 && (double) CultFaithManager.CurrentFaith <= 25.0)
        this.SetFaceAnimation("Emotions/emotion-angry", true);
      if ((double) CultFaithManager.CurrentFaith > 25.0 && (double) CultFaithManager.CurrentFaith <= 40.0)
        this.SetFaceAnimation("Emotions/emotion-unhappy", true);
      if ((double) CultFaithManager.CurrentFaith > 40.0 && (double) CultFaithManager.CurrentFaith <= 80.0)
        this.SetFaceAnimation("Emotions/emotion-normal", true);
      if ((double) CultFaithManager.CurrentFaith <= 75.0)
        return;
      this.SetFaceAnimation("Emotions/emotion-happy", true);
    }
  }

  public void SetHat(HatType hatType)
  {
    this.CurrentHat = hatType;
    this.SetOverrideOutfit(true);
  }

  private void SetOverrideOutfit(bool forceUpdate = false)
  {
    if (this.OverridingOutfit)
      return;
    if (this.Brain.CurrentTaskType != FollowerTaskType.Refinery)
    {
      if (this.Brain.Info.TaxEnforcer)
        this.CurrentHat = HatType.TaxEnforcer;
      else if (this.Brain.Info.FaithEnforcer)
        this.CurrentHat = HatType.FaithEnforcer;
    }
    if (this.Brain.CurrentTaskType == FollowerTaskType.MissionaryComplete || this.Brain.CurrentTaskType == FollowerTaskType.MissionaryInProgress || this.Brain.CurrentTaskType == FollowerTaskType.ChangeLocation)
      return;
    if (this.Brain.Location == FollowerLocation.Base && FollowerBrainStats.IsHoliday && this.Outfit.CurrentOutfit != FollowerOutfitType.Holiday)
      this.Outfit.SetOutfit(this.Spine, FollowerOutfitType.Holiday, this.Brain.Info.Necklace, false, hat: this.CurrentHat);
    else if (this.Brain.Location == FollowerLocation.Base && this.Outfit.CurrentOutfit != FollowerOutfitType.Old && this.Brain.Info.CursedState == Thought.OldAge)
      this.Outfit.SetOutfit(this.Spine, FollowerOutfitType.Old, this.Brain.Info.Necklace, false, hat: this.CurrentHat);
    else if (this.Brain.Location == FollowerLocation.Base && this.Outfit.CurrentOutfit == FollowerOutfitType.Holiday && !FollowerBrainStats.IsHoliday)
    {
      this.Outfit.SetOutfit(this.Spine, FollowerOutfitType.Follower, this.Brain.Info.Necklace, false, hat: this.CurrentHat);
    }
    else
    {
      if (!forceUpdate)
        return;
      this.Outfit.SetOutfit(this.Spine, FollowerOutfitType.Follower, this.Brain.Info.Necklace, false, hat: this.CurrentHat);
    }
  }

  public void SetOutfit(FollowerOutfitType outfitType, bool hooded, Thought overrideCursedState = Thought.None)
  {
    this.Outfit.SetOutfit(this.Spine, outfitType, this.Brain.Info.Necklace, hooded, overrideCursedState, this.CurrentHat);
    this.Brain._directInfoAccess.Outfit = outfitType;
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

  public void AddTrait(FollowerTrait.TraitType Trait) => this.Brain.AddTrait(Trait);

  public void AddBathroom() => this.Brain.Stats.Bathroom += 10f;

  public void CheckRole()
  {
    Debug.Log((object) $"{this.Brain.Info.Name}   {(object) this.Brain.Info.FollowerRole}");
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

  private IEnumerator HoodOffWaitForEndOfFrame(string animation = "idle", bool snap = false, System.Action onComplete = null)
  {
    Follower follower = this;
    if (follower.Outfit.IsHooded)
    {
      if (snap)
      {
        follower.Outfit.SetOutfit(follower.Spine, follower.Brain.Info.Outfit, follower.Brain.Info.Necklace, false, hat: follower.CurrentHat);
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
    float waitDuration;
    if (!this.Outfit.IsHooded)
    {
      double num = (double) this.SetBodyAnimation(this.AnimHoodUp, false);
      this.AddBodyAnimation(animation, true, 0.0f);
      waitDuration = 0.6333333f;
      while ((double) (waitDuration -= Time.deltaTime) > 0.0)
        yield return (object) null;
      this.Outfit.SetOutfit(this.Spine, this.Brain.Info.Outfit, this.Brain.Info.Necklace, true, hat: this.CurrentHat);
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
    this.Outfit.SetOutfit(this.Spine, this.Brain.Info.Outfit, this.Brain.Info.Necklace, false, hat: this.CurrentHat);
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
      this._timedActionTimer = 0.0f;
      System.Action timedAction = this._timedAction;
      if (timedAction != null)
        timedAction();
      this._timedAction = (System.Action) null;
    }
    if (this._dying)
      return;
    this.ClearPath();
    this._timedActionTimer = timer;
    this.State.CURRENT_STATE = StateMachine.State.TimedAction;
    double num = (double) this.SetBodyAnimation(animation, Loop);
    this._timedAction = onComplete;
  }

  private void OnThoughtModified(Thought thought)
  {
    if (thought <= Thought.Ill)
    {
      if (thought != Thought.Brainwashed && thought != Thought.Dissenter && thought != Thought.Ill)
        return;
    }
    else if (thought != Thought.Holiday && thought != Thought.Cult_CureDissenter && thought != Thought.Cult_MushroomEncouraged_Trait)
      return;
    this.SetOutfit(this.Outfit.CurrentOutfit, this.Outfit.IsHooded);
  }

  public void PlayerGetSoul(int devotion)
  {
    this.Brain.Stats.DevotionGiven += devotion;
    PlayerFarming.Instance.GetSoul(devotion);
  }

  public void ShowBarkMessage()
  {
    this.simpleBark.Entries = new List<ConversationEntry>(1)
    {
      new ConversationEntry(this.gameObject, "HELLLLLLLLOOOOO HOW ARE YOU?")
    };
    this.simpleBark.ActivateDistance = 0.0f;
    if (!this.Spine.GetComponent<Renderer>().isVisible)
      return;
    this.simpleBark.Entries[0].Offset = new Vector3(0.0f, -0.5f, 0.0f);
    this.simpleBark.Renderer = this.Spine.GetComponent<Renderer>();
    this.simpleBark.enabled = true;
    this.simpleBark.Show();
    MMConversation.mmConversation.SpeechBubble.transform.localScale = Vector3.one * 0.5f;
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
  }
}
