// Decompiled with JetBrains decompiler
// Type: Interaction_TempleAltar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.AltarMenu;
using Lamb.UI.Rituals;
using Lamb.UI.SermonWheelOverlay;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.Rituals;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_TempleAltar : Interaction
{
  public static Interaction_TempleAltar Instance;
  private SermonController SermonController;
  [SerializeField]
  private Collider2D Collider;
  public ChurchFollowerManager ChurchFollowerManager;
  public GameObject RitualAvailable;
  public Animator RitualAvailableAnimator;
  public GameObject distortionObject;
  public GameObject SermonAvailableObject;
  public GameObject RitualAvailableObject;
  public GameObject Menu;
  public Sprite AltarEmpty;
  public Sprite SpriteOn;
  public Sprite SpriteOff;
  public Sprite SpriteNotPurchased;
  public Material SpriteOnMaterial;
  public Material SpriteOffMaterial;
  public SpriteRenderer spriteRenderer;
  public bool Activated;
  public GameObject Notification;
  public GameObject DoctrineXPPrefab;
  private UIDoctrineBar UIDoctrineBar;
  public static SermonsAndRituals.SermonRitualType CurrentType;
  public UIFollowerXP UIFollowerXPPrefab;
  public GameObject DoctrineUpgradePrefab;
  private bool firstChoice;
  private float barLocalXP;
  public Light RitualLighting;
  public SkeletonAnimation PortalEffect;
  private bool performedSermon;
  private string sPreachSermon;
  private string sAlreadyGivenSermon;
  private string sSacrifice;
  private string sRitual;
  private string sRequiresTemple2;
  private string sRequireMoreFollowers;
  private string sInteract;
  private bool initialInteraction = true;
  public SermonCategory SermonCategory;
  private bool SermonsStillAvailable = true;
  private EventInstance sermonLoop;
  private bool GivenAnswer;
  private int RewardLevel;
  public static DoctrineUpgradeSystem.DoctrineType DoctrineUnlockType;
  public GameObject RitualCameraPosition;
  private bool HasBuiltTemple2;
  public SimpleSetCamera CloseUpCamera;
  public SimpleSetCamera SimpleSetCamera;
  public SimpleSetCamera RitualCloseSetCamera;
  public GameObject FrontWall;
  private bool Activating;
  private Ritual CurrentRitual;
  private UpgradeSystem.Type RitualType;
  private int fleece;

  public void ResetSprite()
  {
    if ((UnityEngine.Object) this.spriteRenderer == (UnityEngine.Object) null)
      return;
    if (DataManager.Instance.PreviousSermonDayIndex < TimeManager.CurrentDay)
      this.spriteRenderer.sprite = this.SpriteOn;
    else
      this.spriteRenderer.sprite = this.SpriteOff;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.HasSecondaryInteraction = false;
    this.HasBuiltTemple2 = DataManager.Instance.HasBuiltTemple2;
    this.RitualAvailableAnimator.Play("Hidden");
    this.distortionObject.SetActive(false);
    this.ResetSprite();
  }

  private bool CheckCanAfford(UpgradeSystem.Type type)
  {
    List<StructuresData.ItemCost> cost = UpgradeSystem.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      if (Inventory.GetItemQuantity((int) cost[index].CostItem) < cost[index].CostValue)
        return false;
    }
    return true;
  }

  private void Start()
  {
    Interaction_TempleAltar.Instance = this;
    this.UpdateLocalisation();
    this.ActivateDistance = 1.5f;
    this.Interactable = true;
    this.SecondaryInteractable = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sInteract = ScriptLocalization.Interactions.TempleAltar;
  }

  public override void GetLabel() => this.Label = this.Activated ? "" : this.sInteract;

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    base.OnInteract(state);
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    this.Activated = true;
    this.HasChanged = true;
    this.OnBecomeNotCurrent();
    this.RitualAvailableAnimator.Play("Hidden");
    GameManager.GetInstance().OnConversationNew(false, false, true);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 8f);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    CultFaithManager.Instance.BarController.UseQueuing = false;
    this.Collider.enabled = false;
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.AltarPosition.gameObject, this.gameObject, GoToCallback: (System.Action) (() =>
    {
      this.Collider.enabled = true;
      if (this.initialInteraction)
      {
        Ritual.FollowerToAttendSermon = Ritual.GetFollowersAvailableToAttendSermon();
        foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
          ChurchFollowerManager.Instance.AddBrainToAudience(brain);
      }
      this.initialInteraction = false;
      this.StartCoroutine((IEnumerator) this.DelayMenu());
    }));
  }

  private IEnumerator DelayMenu()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_TempleAltar interactionTempleAltar = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Time.timeScale = 0.0f;
      UIAltarMenuController altarMenuController = MonoSingleton<UIManager>.Instance.ShowAltarMenu();
      // ISSUE: reference to a compiler-generated method
      altarMenuController.OnSermonSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__44_0);
      // ISSUE: reference to a compiler-generated method
      altarMenuController.OnRitualsSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__44_1);
      // ISSUE: reference to a compiler-generated method
      altarMenuController.OnPlayerUpgradesSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__44_2);
      // ISSUE: reference to a compiler-generated method
      altarMenuController.OnDoctrineSelected += new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__44_3);
      // ISSUE: reference to a compiler-generated method
      altarMenuController.OnCancel = altarMenuController.OnCancel + new System.Action(interactionTempleAltar.\u003CDelayMenu\u003Eb__44_4);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    PlayerFarming.Instance.Spine.UseDeltaTime = false;
    interactionTempleAltar.spriteRenderer.sprite = interactionTempleAltar.AltarEmpty;
    AudioManager.Instance.PlayOneShot("event:/sermon/book_pickup", PlayerFarming.Instance.gameObject);
    interactionTempleAltar.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    interactionTempleAltar.state.transform.DOMove(ChurchFollowerManager.Instance.AltarPosition.transform.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    PlayerFarming.Instance.Spine.UseDeltaTime = false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void DoSermon()
  {
    if (DataManager.Instance.PreviousSermonDayIndex >= TimeManager.CurrentDay)
    {
      this.DoCancel();
      this.CloseAndSpeak("AlreadyGivenSermon");
    }
    else if (DataManager.Instance.Followers.Count <= 0)
    {
      this.DoCancel();
      this.CloseAndSpeak("NoFollowers");
    }
    else if (Ritual.FollowersAvailableToAttendSermon() <= 0)
    {
      this.DoCancel();
      this.CloseAndSpeak("NoFollowersAvailable");
    }
    else
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.performedSermon = true;
      this.SermonController = this.GetComponent<SermonController>();
      this.SermonController.Play(this.state);
    }
  }

  private void DoRitual()
  {
    if (Ritual.FollowersAvailableToAttendSermon() <= 0)
    {
      this.DoCancel();
      this.CloseAndSpeak("NoFollowers");
    }
    else if (Ritual.FollowersAvailableToAttendSermon() <= 0)
    {
      this.DoCancel();
      this.CloseAndSpeak("NoFollowersAvailable");
    }
    else
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.left * 2.25f);
      GameManager.GetInstance().OnConversationNext(this.state.gameObject, 6f);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/start_ritual", PlayerFarming.Instance.gameObject);
      this.RitualAvailableAnimator.Play("Hidden");
      this.StartCoroutine((IEnumerator) this.OpenRitualMenuRoutine());
      this.Activated = false;
    }
  }

  private void DoDoctrine()
  {
    Time.timeScale = 0.0f;
    PlayerFarming.Instance.Spine.UseDeltaTime = false;
    this.Activated = false;
    UIDoctrineMenuController doctrineMenuController = MonoSingleton<UIManager>.Instance.DoctrineMenuTemplate.Instantiate<UIDoctrineMenuController>();
    doctrineMenuController.Show();
    doctrineMenuController.OnHide = doctrineMenuController.OnHide + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.OnInteract(this.state);
    });
  }

  private void DoPlayerUpgrade()
  {
    GameManager.GetInstance().CameraSetOffset(Vector3.left * 2.25f);
    GameManager.GetInstance().OnConversationNext(this.state.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/start_ritual", PlayerFarming.Instance.gameObject);
    this.RitualAvailableAnimator.Play("Hidden");
    this.StartCoroutine((IEnumerator) this.OpenPlayerUpgradeRoutine());
    this.Activated = false;
  }

  private IEnumerator OpenPlayerUpgradeRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.OpenPlayerUpgradeMenu();
  }

  private void OpenPlayerUpgradeMenu()
  {
    UIPlayerUpgradesMenuController playerUpgradesMenuInstance = MonoSingleton<UIManager>.Instance.ShowPlayerUpgradesMenu();
    UIPlayerUpgradesMenuController upgradesMenuController1 = playerUpgradesMenuInstance;
    upgradesMenuController1.OnCancel = upgradesMenuController1.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.OnInteract(this.state);
    });
    UIPlayerUpgradesMenuController upgradesMenuController2 = playerUpgradesMenuInstance;
    upgradesMenuController2.OnHidden = upgradesMenuController2.OnHidden + (System.Action) (() => playerUpgradesMenuInstance = (UIPlayerUpgradesMenuController) null);
  }

  private void DoCancel()
  {
    SimulationManager.UnPause();
    AudioManager.Instance.StopLoop(this.sermonLoop);
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", PlayerFarming.Instance.gameObject);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    Time.timeScale = 1f;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().OnConversationEnd();
    CultFaithManager.Instance.BarController.UseQueuing = false;
    this.initialInteraction = true;
    if (Ritual.FollowerToAttendSermon != null)
    {
      foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      {
        FollowerBrain f = followerBrain;
        Follower followerById = FollowerManager.FindFollowerByID(f.Info.ID);
        if ((bool) (UnityEngine.Object) followerById)
        {
          FollowerManager.FindFollowerByID(f.Info.ID).Spine.UseDeltaTime = true;
          followerById.ShowAllFollowerIcons();
          followerById.Spine.UseDeltaTime = true;
          followerById.UseUnscaledTime = false;
        }
        if (this.performedSermon)
        {
          int num = 0;
          if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_III))
            num = 1;
          if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_IV))
            num = 2;
          for (int index = 0; index < num; ++index)
          {
            AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerById.transform.position);
            ResourceCustomTarget.Create(Interaction_DonationBox.Instance.gameObject, followerById.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => Interaction_DonationBox.Instance.DepositCoin()));
          }
        }
        f.CompleteCurrentTask();
        this.StartCoroutine((IEnumerator) this.DelayCallback(1f, (System.Action) (() =>
        {
          if (f.CurrentTaskType != FollowerTaskType.AttendTeaching)
            return;
          f.CompleteCurrentTask();
        })));
      }
    }
    this.ResetSprite();
    ChurchFollowerManager.Instance.ClearAudienceBrains();
    if (Ritual.FollowerToAttendSermon != null)
      Ritual.FollowerToAttendSermon.Clear();
    this.performedSermon = false;
    this.Activated = false;
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.ResetSprite();
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  private void CloseAndSpeak(string ConversationEntryTerm)
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "FollowerInteractions/" + ConversationEntryTerm, "worship")
    }, (List<MMTools.Response>) null, (System.Action) null));
  }

  public IEnumerator FollowersEnterForSermonRoutine(bool shufflePosition = false)
  {
    if (Ritual.FollowerToAttendSermon == null || Ritual.FollowerToAttendSermon.Count <= 0)
      Ritual.FollowerToAttendSermon = Ritual.GetFollowersAvailableToAttendSermon();
    if (TimeManager.CurrentPhase == DayPhase.Night)
      DataManager.Instance.WokeUpEveryoneDay = TimeManager.CurrentDay;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
      {
        followerBrain.ShouldReconsiderTask = false;
        followerById.HideAllFollowerIcons();
        followerById.Spine.UseDeltaTime = false;
        followerById.UseUnscaledTime = true;
      }
      if (!(followerBrain.CurrentTask is FollowerTask_AttendTeaching) || shufflePosition)
      {
        if (followerBrain.CurrentTask != null)
          followerBrain.CurrentTask.Abort();
        followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching());
        followerBrain.ShouldReconsiderTask = false;
        if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
          followerBrain.CurrentTask.Arrive();
        yield return (object) new WaitForSecondsRealtime(UnityEngine.Random.Range(0.05f, 0.15f));
      }
    }
    float timer = 0.0f;
    while (!this.FollowersInPosition() && (double) (timer += Time.deltaTime) < 10.0)
      yield return (object) null;
  }

  private bool ReadyForTeaching()
  {
    bool flag = true;
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.ShouldReconsiderTask || followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation || followerBrain.Location != PlayerFarming.Location || followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching && followerBrain.CurrentTask.State != FollowerTaskState.GoingTo)
        flag = false;
    }
    return flag;
  }

  private bool FollowersInPosition()
  {
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation || followerBrain.Location != PlayerFarming.Location || followerBrain.CurrentTaskType == FollowerTaskType.AttendTeaching && followerBrain.CurrentTask.State != FollowerTaskState.Doing)
      {
        if (followerBrain.Location != PlayerFarming.Location)
        {
          followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_AttendTeaching());
          followerBrain.ShouldReconsiderTask = false;
          if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
            followerBrain.CurrentTask.Arrive();
        }
        return false;
      }
    }
    return true;
  }

  private IEnumerator TeachSermonRoutine()
  {
    Interaction_TempleAltar interactionTempleAltar1 = this;
    interactionTempleAltar1.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("build", 0, true);
    AudioManager.Instance.PlayOneShot("event:/sermon/start_sermon", PlayerFarming.Instance.gameObject);
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", PlayerFarming.Instance.gameObject);
    interactionTempleAltar1.StartCoroutine((IEnumerator) interactionTempleAltar1.CentrePlayer());
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 12f);
    yield return (object) interactionTempleAltar1.StartCoroutine((IEnumerator) interactionTempleAltar1.FollowersEnterForSermonRoutine());
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 7f);
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -0.5f));
    yield return (object) new WaitForSeconds(0.5f);
    interactionTempleAltar1.SermonsStillAvailable = false;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Afterlife) < 4)
      interactionTempleAltar1.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Food) < 4)
      interactionTempleAltar1.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Possession) < 4)
      interactionTempleAltar1.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.WorkAndWorship) < 4)
      interactionTempleAltar1.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.LawAndOrder) < 4)
      interactionTempleAltar1.SermonsStillAvailable = true;
    if (DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Special) < 3)
    {
      Debug.Log((object) "A");
      interactionTempleAltar1.SermonCategory = SermonCategory.Special;
      interactionTempleAltar1.SermonsStillAvailable = true;
      interactionTempleAltar1.StartCoroutine((IEnumerator) interactionTempleAltar1.GatherFollowers());
    }
    else if (interactionTempleAltar1.SermonsStillAvailable)
    {
      Interaction_TempleAltar interactionTempleAltar = interactionTempleAltar1;
      AudioManager.Instance.PlayOneShot("event:/sermon/sermon_menu_appear", PlayerFarming.Instance.gameObject);
      SermonCategory finalisedCategory = SermonCategory.None;
      UISermonWheelController sermonWheelController = MonoSingleton<UIManager>.Instance.SermonWheelTemplate.Instantiate<UISermonWheelController>();
      sermonWheelController.OnItemChosen = sermonWheelController.OnItemChosen + (System.Action<SermonCategory>) (chosenCategory =>
      {
        Debug.Log((object) $"Chose category {chosenCategory}".Colour(Color.yellow));
        finalisedCategory = chosenCategory;
      });
      sermonWheelController.OnHide = sermonWheelController.OnHide + (System.Action) (() =>
      {
        if (finalisedCategory == SermonCategory.None)
        {
          interactionTempleAltar.DoCancel();
          foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
          {
            try
            {
              followerBrain.CurrentTask.End();
            }
            catch (Exception ex)
            {
              Debug.Log((object) followerBrain.Info.Name);
            }
          }
        }
        else
        {
          interactionTempleAltar.SermonCategory = finalisedCategory;
          interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.GatherFollowers());
          AudioManager.Instance.PlayOneShot("event:/sermon/select_sermon", PlayerFarming.Instance.gameObject);
        }
      });
      sermonWheelController.Show();
    }
    else
    {
      interactionTempleAltar1.SermonCategory = SermonCategory.None;
      interactionTempleAltar1.StartCoroutine((IEnumerator) interactionTempleAltar1.GatherFollowers());
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.sermonLoop);
  }

  private new void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.sermonLoop);
    Ritual.OnEnd -= new System.Action<bool>(this.RitualOnEnd);
  }

  public void PulseDisplacementObject(Vector3 Position)
  {
    this.distortionObject.transform.position = Position;
    if (this.distortionObject.gameObject.activeSelf)
    {
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DORestart();
      this.distortionObject.transform.DOPlay();
    }
    else
    {
      this.distortionObject.SetActive(true);
      this.distortionObject.transform.localScale = Vector3.zero;
      this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.distortionObject.SetActive(false)));
    }
  }

  public IEnumerator GatherFollowers()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 1f));
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 7f);
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
    interactionTempleAltar.sermonLoop = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", PlayerFarming.Instance.gameObject, true, false);
    yield return (object) new WaitForSeconds(0.6f);
    interactionTempleAltar.PulseDisplacementObject(interactionTempleAltar.state.transform.position);
    yield return (object) new WaitForSeconds(0.4f);
    ChurchFollowerManager.Instance.StartSermonEffect();
    bool askedQuestion = false;
    if (interactionTempleAltar.SermonsStillAvailable)
    {
      askedQuestion = true;
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(interactionTempleAltar.DoctrineXPPrefab, GameObject.FindWithTag("Canvas").transform);
      interactionTempleAltar.UIDoctrineBar = gameObject.GetComponent<UIDoctrineBar>();
      float xp = DoctrineUpgradeSystem.GetXPBySermon(interactionTempleAltar.SermonCategory);
      float target = DoctrineUpgradeSystem.GetXPTargetBySermon(interactionTempleAltar.SermonCategory);
      float num = 1.5f - (target - xp);
      yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Show(xp, interactionTempleAltar.SermonCategory));
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 11f);
      int studyIncrementCount = Mathf.FloorToInt(DataManager.Instance.TempleStudyXP / 0.1f);
      float delay = 1.5f / (float) studyIncrementCount;
      if (studyIncrementCount > 0)
      {
        DataManager.Instance.TempleStudyXP -= (float) studyIncrementCount * 0.1f;
        for (int i = 0; i < studyIncrementCount; ++i)
        {
          xp += 0.1f;
          if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_SermonEfficiency))
            xp += 0.05f;
          if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_SermonEfficiencyII))
            xp += 0.05f;
          target = DoctrineUpgradeSystem.GetXPTargetBySermon(interactionTempleAltar.SermonCategory);
          // ISSUE: reference to a compiler-generated method
          SoulCustomTarget.Create(interactionTempleAltar.gameObject, ChurchFollowerManager.Instance.StudySlots[UnityEngine.Random.Range(0, ChurchFollowerManager.Instance.StudySlots.Length)].transform.position, Color.white, new System.Action(interactionTempleAltar.\u003CGatherFollowers\u003Eb__63_1), 0.2f, 500f);
          yield return (object) new WaitForSeconds(delay);
          if ((double) xp >= (double) target)
          {
            yield return (object) new WaitForSeconds(0.5f);
            yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
            AudioManager.Instance.PlayOneShot("event:/sermon/upgrade_menu_appear");
            yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.FlashBarRoutine(0.3f, 1f));
            yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Hide());
            yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.AskQuestionRoutine());
            xp = 0.0f;
            if (DoctrineUpgradeSystem.GetLevelBySermon(interactionTempleAltar.SermonCategory) < 4)
            {
              if (i < studyIncrementCount - 1)
              {
                yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Show(0.0f, interactionTempleAltar.SermonCategory));
                interactionTempleAltar.barLocalXP = 0.0f;
              }
            }
            else
              break;
          }
        }
        yield return (object) new WaitForSeconds(1f);
        yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
        yield return (object) new WaitForSeconds(0.5f);
      }
      int followersCount = Ritual.FollowerToAttendSermon.Count;
      delay = 1.5f / (float) followersCount;
      interactionTempleAltar.barLocalXP = xp;
      int count = 0;
      do
      {
        xp += 0.1f;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_SermonEfficiency))
          xp += 0.05f;
        if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Temple_SermonEfficiencyII))
          xp += 0.05f;
        target = DoctrineUpgradeSystem.GetXPTargetBySermon(interactionTempleAltar.SermonCategory);
        // ISSUE: reference to a compiler-generated method
        SoulCustomTarget.Create(interactionTempleAltar.gameObject, Ritual.FollowerToAttendSermon[count].LastPosition, Color.white, new System.Action(interactionTempleAltar.\u003CGatherFollowers\u003Eb__63_0), 0.2f, 500f);
        yield return (object) new WaitForSeconds(delay);
        if ((double) xp >= (double) target)
        {
          yield return (object) new WaitForSeconds(0.5f);
          yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
          yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.FlashBarRoutine(0.3f, 1f));
          yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Hide());
          yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.AskQuestionRoutine());
          xp = 0.0f;
          if (DoctrineUpgradeSystem.GetLevelBySermon(interactionTempleAltar.SermonCategory) < 4)
          {
            if (count < followersCount - 1)
            {
              yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.Show(0.0f, interactionTempleAltar.SermonCategory));
              interactionTempleAltar.barLocalXP = 0.0f;
            }
          }
          else
            break;
        }
        ++count;
      }
      while (count < followersCount);
      ChurchFollowerManager.Instance.EndSermonEffect();
      yield return (object) new WaitForSeconds(0.5f);
      yield return (object) interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.UIDoctrineBar.UpdateSecondBar(xp, 0.5f));
      yield return (object) new WaitForSeconds(0.5f);
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionTempleAltar.UIDoctrineBar.gameObject);
      DoctrineUpgradeSystem.SetXPBySermon(interactionTempleAltar.SermonCategory, xp);
    }
    else
    {
      yield return (object) new WaitForSeconds(1f);
      ChurchFollowerManager.Instance.EndSermonEffect();
    }
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("DELIVER_FIRST_SERMON"));
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", PlayerFarming.Instance.gameObject);
    int num1 = (int) interactionTempleAltar.sermonLoop.stop(STOP_MODE.ALLOWFADEOUT);
    AudioManager.Instance.StopLoop(interactionTempleAltar.sermonLoop);
    yield return (object) new WaitForSeconds(0.333333343f);
    interactionTempleAltar.ResetSprite();
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", PlayerFarming.Instance.gameObject);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    DataManager.Instance.PreviousSermonDayIndex = TimeManager.CurrentDay;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.AttendTeaching)
      {
        if (DataManager.Instance.UnlockededASermon > 1 && interactionTempleAltar.SermonCategory != SermonCategory.None && interactionTempleAltar.SermonCategory == DataManager.Instance.PreviousSermonCategory)
          allBrain.AddThought(Thought.WatchedSermonSameAsYesterday);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.SermonEnthusiast))
          allBrain.AddThought(Thought.WatchedSermonDevotee);
        else
          allBrain.AddThought(Thought.WatchedSermon);
        allBrain.AddAdoration(FollowerBrain.AdorationActions.Sermon, (System.Action) null);
        interactionTempleAltar.StartCoroutine((IEnumerator) interactionTempleAltar.DelayFollowerReaction(allBrain, UnityEngine.Random.Range(0.1f, 0.5f)));
        FollowerManager.FindFollowerByID(allBrain.Info.ID)?.ShowAllFollowerIcons(false);
      }
    }
    DataManager.Instance.PreviousSermonCategory = interactionTempleAltar.SermonCategory;
    interactionTempleAltar.ResetSprite();
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GiveSermon);
    interactionTempleAltar.Activated = false;
    yield return (object) new WaitForSeconds(1.5f);
    if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.SermonEnthusiast))
      CultFaithManager.AddThought(Thought.Cult_SermonEnthusiast_Trait);
    else
      CultFaithManager.AddThought(Thought.Cult_Sermon);
    if (DataManager.Instance.WokeUpEveryoneDay == TimeManager.CurrentDay && TimeManager.CurrentPhase == DayPhase.Night && !FollowerBrainStats.IsWorkThroughTheNight)
      CultFaithManager.AddThought(Thought.Cult_WokeUpEveryone);
    if (!askedQuestion)
      PlayerFarming.Instance.health.BlueHearts += 2f;
  }

  private void IncrementXPBar()
  {
    this.barLocalXP += 0.1f;
    this.StartCoroutine((IEnumerator) this.UIDoctrineBar.UpdateFirstBar(this.barLocalXP, 0.1f));
  }

  public IEnumerator AskQuestionRoutine()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    AudioManager.Instance.PlayOneShot("event:/sermon/sermon_speech_bubble", PlayerFarming.Instance.gameObject);
    interactionTempleAltar.GivenAnswer = false;
    interactionTempleAltar.RewardLevel = DoctrineUpgradeSystem.GetLevelBySermon(interactionTempleAltar.SermonCategory) + 1;
    string TermToSpeak = $"DoctrineUpgradeSystem/{(object) interactionTempleAltar.SermonCategory}{(object) interactionTempleAltar.RewardLevel}";
    if (interactionTempleAltar.SermonCategory == SermonCategory.Special)
      TermToSpeak = " ";
    Debug.Log((object) $"Text: DoctrineUpgradeSystem/{(object) interactionTempleAltar.SermonCategory}{(object) interactionTempleAltar.RewardLevel}");
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    Entries.Add(new ConversationEntry(PlayerFarming.Instance.CameraBone, TermToSpeak));
    Entries[0].CharacterName = "DoctrineUpgradeSystem/DoctrinalDecision";
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    List<DoctrineResponse> DoctrineResponses = new List<DoctrineResponse>()
    {
      new DoctrineResponse(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, true, new System.Action(interactionTempleAltar.\u003CAskQuestionRoutine\u003Eb__67_0)),
      new DoctrineResponse(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, false, new System.Action(interactionTempleAltar.\u003CAskQuestionRoutine\u003Eb__67_1))
    };
    if (interactionTempleAltar.SermonCategory == SermonCategory.Special)
    {
      // ISSUE: reference to a compiler-generated method
      DoctrineResponses = new List<DoctrineResponse>()
      {
        new DoctrineResponse(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, true, new System.Action(interactionTempleAltar.\u003CAskQuestionRoutine\u003Eb__67_2))
      };
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null, DoctrineResponses), false, false, false, showControlPrompt: false);
    while (!interactionTempleAltar.GivenAnswer)
      yield return (object) null;
    Interaction_TempleAltar.DoctrineUnlockType = DoctrineUpgradeSystem.GetSermonReward(interactionTempleAltar.SermonCategory, interactionTempleAltar.RewardLevel, interactionTempleAltar.firstChoice);
    DoctrineUpgradeSystem.UnlockAbility(Interaction_TempleAltar.DoctrineUnlockType);
    DoctrineUpgradeSystem.SetLevelBySermon(interactionTempleAltar.SermonCategory, DoctrineUpgradeSystem.GetLevelBySermon(interactionTempleAltar.SermonCategory) + 1);
    HUD_Manager.Instance.Hide(true, 0);
  }

  private void Reply(bool FirstChoice)
  {
    this.firstChoice = FirstChoice;
    this.GivenAnswer = true;
    Debug.Log((object) ("CALLBACK! " + FirstChoice.ToString()));
  }

  public IEnumerator DelayFollowerReaction(FollowerBrain brain, float Delay)
  {
    yield return (object) new WaitForSecondsRealtime(Delay);
    Follower f = FollowerManager.FindFollowerByID(brain.Info.ID);
    if (!((UnityEngine.Object) f == (UnityEngine.Object) null))
    {
      f.HideAllFollowerIcons();
      f.Spine.UseDeltaTime = false;
      f.UseUnscaledTime = true;
      f.TimedAnimation("Reactions/react-enlightened1", 1.5f, (System.Action) (() => f.Brain.CurrentTask.StartAgain(f)), false, false);
      float num = 0.0f;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        num += allBrain.Stats.Happiness;
      if ((double) num / (100.0 * (double) FollowerBrain.AllBrains.Count) <= 0.20000000298023224)
        NotificationCentre.Instance.PlayGenericNotification(NotificationCentre.NotificationType.LowFaithDonation);
    }
  }

  public IEnumerator CentrePlayer()
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    interactionTempleAltar.state.facingAngle = 270f;
    float Progress = 0.0f;
    Vector3 StartPosition = interactionTempleAltar.state.transform.position;
    while ((double) (Progress += Time.deltaTime) < 0.25)
    {
      interactionTempleAltar.state.transform.position = Vector3.Lerp(StartPosition, ChurchFollowerManager.Instance.AltarPosition.position, Mathf.SmoothStep(0.0f, 1f, Progress / 0.25f));
      yield return (object) null;
    }
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_Temple2) || !this.HasBuiltTemple2)
    {
      MonoSingleton<Indicator>.Instance.PlayShake();
    }
    else
    {
      if (this.Activating)
        return;
      base.OnSecondaryInteract(state);
      this.Activating = true;
      this.OnBecomeNotCurrent();
      this.HasChanged = true;
      GameManager.GetInstance().OnConversationNew(false);
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 8f);
      GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
      PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.AltarPosition.gameObject, this.gameObject, GoToCallback: (System.Action) (() =>
      {
        AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/start_ritual", PlayerFarming.Instance.gameObject);
        this.RitualAvailableAnimator.Play("Hidden");
        this.StartCoroutine((IEnumerator) this.CentrePlayer());
        this.StartCoroutine((IEnumerator) this.OpenRitualMenuRoutine());
      }));
    }
  }

  private IEnumerator OpenRitualMenuRoutine()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.OpenRitualMenu();
  }

  private void OpenRitualMenu()
  {
    UIRitualsMenuController ritualsMenuController = MonoSingleton<UIManager>.Instance.RitualsMenuTemplate.Instantiate<UIRitualsMenuController>();
    ritualsMenuController.Show();
    ritualsMenuController.OnRitualSelected += new System.Action<UpgradeSystem.Type>(this.PerformRitual);
    ritualsMenuController.OnCancel = ritualsMenuController.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      this.OnInteract(this.state);
    });
  }

  public void PerformRitual(UpgradeSystem.Type RitualType)
  {
    this.RitualType = RitualType;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    if ((UnityEngine.Object) this.CurrentRitual != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentRitual);
    Ritual.OnEnd += new System.Action<bool>(this.RitualOnEnd);
    Debug.Log((object) ("Perform ritual: " + (object) RitualType));
    ObjectiveManager.SetRitualObjectivesFailLocked();
    switch (RitualType)
    {
      case UpgradeSystem.Type.Ritual_Sacrifice:
        RitualSacrifice ritualSacrifice = this.gameObject.AddComponent<RitualSacrifice>();
        ritualSacrifice.RitualLight = this.RitualLighting;
        ritualSacrifice.Play();
        this.CurrentRitual = (Ritual) ritualSacrifice;
        break;
      case UpgradeSystem.Type.Ritual_Reindoctrinate:
        RitualReindoctrinate ritualReindoctrinate = this.gameObject.AddComponent<RitualReindoctrinate>();
        ritualReindoctrinate.Play();
        this.CurrentRitual = (Ritual) ritualReindoctrinate;
        break;
      case UpgradeSystem.Type.Ritual_ConsumeFollower:
        RitualConsumeFollower ritualConsumeFollower = this.gameObject.AddComponent<RitualConsumeFollower>();
        ritualConsumeFollower.Play();
        this.CurrentRitual = (Ritual) ritualConsumeFollower;
        break;
      case UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1:
        RitualFlockOfTheFaithful flockOfTheFaithful = this.gameObject.AddComponent<RitualFlockOfTheFaithful>();
        flockOfTheFaithful.ritualLight = this.RitualLighting;
        flockOfTheFaithful.Play();
        this.CurrentRitual = (Ritual) flockOfTheFaithful;
        break;
      case UpgradeSystem.Type.Ritual_UnlockWeapon:
        this.StartCoroutine((IEnumerator) this.DelayCallback(0.5f, (System.Action) (() => (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/UI Unlock Weapon"), GameObject.FindWithTag("Canvas").transform) as GameObject).GetComponent<UIUnlockWeapon>().Init((System.Action) (() =>
        {
          this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualUnlockWeapon>();
          this.CurrentRitual.Play();
        }), (System.Action) (() => this.OpenRitualMenu())))));
        break;
      case UpgradeSystem.Type.Ritual_UnlockCurse:
        this.StartCoroutine((IEnumerator) this.DelayCallback(0.5f, (System.Action) (() => (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/UI Unlock Curse"), GameObject.FindWithTag("Canvas").transform) as GameObject).GetComponent<UIUnlockCurse>().Init((System.Action) (() =>
        {
          this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualUnlockCurse>();
          this.CurrentRitual.Play();
        }), (System.Action) (() => this.OpenRitualMenu())))));
        break;
      case UpgradeSystem.Type.Ritual_FasterBuilding:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFasterBuilding>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Enlightenment:
        Debug.Log((object) "ENLIGHTENMENT!");
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualElightenment>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_WorkThroughNight:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualWorkThroughNight>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Holiday:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualWorkHoliday>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_AlmsToPoor:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualAlmsToPoor>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_DonationRitual:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualDonation>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Fast:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFast>();
        this.CurrentRitual.Play();
        using (List<FollowerBrain>.Enumerator enumerator = FollowerBrain.AllBrains.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            FollowerBrain current = enumerator.Current;
            current.Stats.Starvation = 0.0f;
            current.Stats.Satiation = 100f;
            int num = UnityEngine.Random.Range(0, 10);
            if (Utils.Between((float) num, 0.0f, 6f))
              current.AddThought(Thought.Fasting);
            else if (Utils.Between((float) num, 6f, 8f))
              current.AddThought(Thought.AngryAboutFasting);
            else if (Utils.Between((float) num, 8f, 10f))
              current.AddThought(Thought.HappyAboutFasting);
          }
          break;
        }
      case UpgradeSystem.Type.Ritual_Feast:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFeast>();
        this.CurrentRitual.Play();
        using (List<FollowerBrain>.Enumerator enumerator = FollowerBrain.AllBrains.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            FollowerBrain current = enumerator.Current;
            current.Stats.Starvation = 0.0f;
            current.Stats.Satiation = 100f;
            current.AddThought(Thought.FeastTable);
          }
          break;
        }
      case UpgradeSystem.Type.Ritual_HarvestRitual:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualHarvest>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_FishingRitual:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFishing>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Ressurect:
        RitualRessurect ritualRessurect = this.gameObject.AddComponent<RitualRessurect>();
        ritualRessurect.Play();
        ritualRessurect.RitualLight = this.RitualLighting;
        this.CurrentRitual = (Ritual) ritualRessurect;
        break;
      case UpgradeSystem.Type.Ritual_Funeral:
        RitualFuneral ritualFuneral = this.gameObject.AddComponent<RitualFuneral>();
        ritualFuneral.Play();
        this.CurrentRitual = (Ritual) ritualFuneral;
        break;
      case UpgradeSystem.Type.Ritual_Fightpit:
        RitualFightpit ritualFightpit = this.gameObject.AddComponent<RitualFightpit>();
        ritualFightpit.Play();
        this.CurrentRitual = (Ritual) ritualFightpit;
        break;
      case UpgradeSystem.Type.Ritual_Wedding:
        RitualWedding ritualWedding = this.gameObject.AddComponent<RitualWedding>();
        ritualWedding.Play();
        this.CurrentRitual = (Ritual) ritualWedding;
        break;
      case UpgradeSystem.Type.Ritual_AssignFaithEnforcer:
        RitualFaithEnforcer ritualFaithEnforcer = this.gameObject.AddComponent<RitualFaithEnforcer>();
        ritualFaithEnforcer.Play();
        this.CurrentRitual = (Ritual) ritualFaithEnforcer;
        break;
      case UpgradeSystem.Type.Ritual_AssignTaxCollector:
        RitualTaxEnforcer ritualTaxEnforcer = this.gameObject.AddComponent<RitualTaxEnforcer>();
        ritualTaxEnforcer.Play();
        this.CurrentRitual = (Ritual) ritualTaxEnforcer;
        break;
      case UpgradeSystem.Type.Ritual_Brainwashing:
        RitualBrainwashing ritualBrainwashing = this.gameObject.AddComponent<RitualBrainwashing>();
        ritualBrainwashing.Play();
        this.CurrentRitual = (Ritual) ritualBrainwashing;
        break;
      case UpgradeSystem.Type.Ritual_Ascend:
        RitualAscend ritualAscend = this.gameObject.AddComponent<RitualAscend>();
        ritualAscend.Play();
        this.CurrentRitual = (Ritual) ritualAscend;
        break;
      case UpgradeSystem.Type.Ritual_FirePit:
        this.CurrentRitual = (Ritual) this.gameObject.AddComponent<RitualFirePit>();
        this.CurrentRitual.Play();
        break;
      case UpgradeSystem.Type.Ritual_Halloween:
        RitualHalloween ritualHalloween = this.gameObject.AddComponent<RitualHalloween>();
        ritualHalloween.Play();
        this.CurrentRitual = (Ritual) ritualHalloween;
        break;
    }
  }

  private IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  private void UnlockHeartsCallback(bool cancelled)
  {
    Ritual.OnEnd -= new System.Action<bool>(this.UnlockHeartsCallback);
    this.StartCoroutine((IEnumerator) this.UnlockHeartsCallbackRoutine());
  }

  private IEnumerator UnlockHeartsCallbackRoutine()
  {
    yield return (object) new WaitForSeconds(5f);
    GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
  }

  private void RitualOnEnd(bool cancelled)
  {
    if (!cancelled && this.RitualType != UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1)
    {
      foreach (StructuresData.ItemCost itemCost in UpgradeSystem.GetCost(this.RitualType))
        Inventory.ChangeItemQuantity((int) itemCost.CostItem, -itemCost.CostValue);
    }
    this.ResetSprite();
    Ritual.OnEnd -= new System.Action<bool>(this.RitualOnEnd);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.CurrentRitual);
    this.Activating = false;
    PlayerFarming.Instance.Spine.UseDeltaTime = true;
    if (cancelled)
      return;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIRST_RITUAL"));
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PerformAnyRitual);
    if (!DataManager.Instance.ShowLoyaltyBars)
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.WaitForConversationToEnd((System.Action) (() =>
      {
        Onboarding.Instance.RatLoyalty.SetActive(true);
        Onboarding.Instance.RatLoyalty.GetComponent<Interaction_SimpleConversation>().Play();
      })));
    switch (this.RitualType)
    {
      case UpgradeSystem.Type.Ritual_Sacrifice:
        this.StartCoroutine((IEnumerator) this.UnlockSacrifices());
        UpgradeSystem.AddCooldown(this.RitualType, 8400f);
        break;
      case UpgradeSystem.Type.Ritual_ConsumeFollower:
        UpgradeSystem.AddCooldown(this.RitualType, 1200f);
        break;
      case UpgradeSystem.Type.Ritual_UnlockWeapon:
        UpgradeSystem.AddCooldown(this.RitualType, 1200f);
        break;
      case UpgradeSystem.Type.Ritual_UnlockCurse:
        UpgradeSystem.AddCooldown(this.RitualType, 1200f);
        break;
      case UpgradeSystem.Type.Ritual_FasterBuilding:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_Enlightenment:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_WorkThroughNight:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_Holiday:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_AlmsToPoor:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_DonationRitual:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_Fast:
        UpgradeSystem.AddCooldown(this.RitualType, 12000f);
        break;
      case UpgradeSystem.Type.Ritual_Feast:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_HarvestRitual:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_FishingRitual:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_Ressurect:
        UpgradeSystem.AddCooldown(this.RitualType, 3600f);
        break;
      case UpgradeSystem.Type.Ritual_Funeral:
        UpgradeSystem.AddCooldown(this.RitualType, 1200f);
        break;
      case UpgradeSystem.Type.Ritual_Fightpit:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_Wedding:
        UpgradeSystem.AddCooldown(this.RitualType, 3600f);
        break;
      case UpgradeSystem.Type.Ritual_AssignFaithEnforcer:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_AssignTaxCollector:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_Brainwashing:
        UpgradeSystem.AddCooldown(this.RitualType, 8400f);
        break;
      case UpgradeSystem.Type.Ritual_Ascend:
        UpgradeSystem.AddCooldown(this.RitualType, 6000f);
        break;
      case UpgradeSystem.Type.Ritual_FirePit:
        UpgradeSystem.AddCooldown(this.RitualType, 8400f);
        break;
    }
  }

  private IEnumerator WaitForConversationToEnd(System.Action callback)
  {
    while (LetterBox.IsPlaying)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    System.Action action = callback;
    if (action != null)
      action();
  }

  private void Close()
  {
    this.ResetSprite();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().OnConversationEnd();
    this.Activating = false;
  }

  public void GetAbilityRoutine(UpgradeSystem.Type Type)
  {
    this.StartCoroutine((IEnumerator) this.GetAbilityRoutineIE(Type));
  }

  private IEnumerator UnlockSacrifices()
  {
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.sacrificesCompleted == 0)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIRST_SACRIFICE"));
    ++DataManager.Instance.sacrificesCompleted;
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.sacrificesCompleted >= 10)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("SACRIFICE_FOLLOWERS"));
    yield return (object) null;
  }

  private IEnumerator GetAbilityRoutineIE(UpgradeSystem.Type Type)
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "gameover-fast", true);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    EventInstance receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    float Progress = 0.0f;
    float Duration = 3.66666675f;
    float StartingZoom = GameManager.GetInstance().CamFollowTarget.distance;
    while ((double) (Progress += Time.deltaTime) < (double) Duration - 0.5)
    {
      GameManager.GetInstance().CameraSetZoom(Mathf.Lerp(StartingZoom, 4f, Progress / Duration));
      if (Time.frameCount % 10 == 0)
        SoulCustomTarget.Create(PlayerFarming.Instance.CrownBone.gameObject, interactionTempleAltar.transform.position, Color.red, (System.Action) null, 0.2f);
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 4f);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "specials/special-activate", false);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
    int num = (int) receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "idle", true);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    UITutorialOverlayController TutorialOverlay = (UITutorialOverlayController) null;
    switch (Type)
    {
      case UpgradeSystem.Type.Ability_Eat:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.TheHunger))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.TheHunger);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_Resurrection:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Resurrection))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Resurrection);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_BlackHeart:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.DarknessWithin))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.DarknessWithin);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_TeleportHome:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Omnipresence))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Omnipresence);
          break;
        }
        break;
    }
    while ((UnityEngine.Object) TutorialOverlay != (UnityEngine.Object) null)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.MonsterHeart);
    interactionTempleAltar.ResetSprite();
    interactionTempleAltar.OnInteract(interactionTempleAltar.state);
  }

  public void GetFleeceRoutine(int oldFleece, int newFleece)
  {
    this.StartCoroutine((IEnumerator) this.GetFleeceRoutineIE(oldFleece, newFleece));
  }

  private IEnumerator GetFleeceRoutineIE(int oldFleece, int newFleece)
  {
    Interaction_TempleAltar interactionTempleAltar = this;
    interactionTempleAltar.fleece = newFleece;
    PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + oldFleece.ToString());
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "unlock-fleece", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    EventInstance receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    PlayerFarming.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionTempleAltar.AnimationState_Event);
    yield return (object) new WaitForSeconds(3f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
    int num = (int) receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.UnlockFleece);
    interactionTempleAltar.ResetSprite();
    interactionTempleAltar.OnInteract(interactionTempleAltar.state);
  }

  private void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "change-skin"))
      return;
    PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
    PlayerFarming.Instance.simpleSpineAnimator?.SetSkin("Lamb_" + this.fleece.ToString());
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/hearts_of_the_faithful/hearts_appear", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", this.gameObject.transform.position);
  }
}
