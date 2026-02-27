// Decompiled with JetBrains decompiler
// Type: FollowerRecruit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FollowerRecruit : Interaction
{
  public GameObject Menu;
  public GameObject MenuRecruit;
  public Follower Follower;
  public GameObject CameraBone;
  public AudioClip SacrificeSting;
  public interaction_FollowerInteraction FollowerInteraction;
  [SerializeField]
  private SkeletonAnimation portalSpine;
  public System.Action StatueCallback;
  public bool triggered;
  private const float triggerDistance = 10f;
  public static FollowerRecruit.FollowerEventDelegate OnFollowerRecruited;
  private string dString;
  private bool Activating;
  public GameObject FollowerRoleMenu;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  private List<MeshRenderer> FollowersTurnedOff = new List<MeshRenderer>();
  public static System.Action OnNewRecruit;
  public bool RecruitOnComplete = true;

  private void Start()
  {
    this.UpdateLocalisation();
    this.IgnoreTutorial = true;
    this.Follower.State.CURRENT_STATE = StateMachine.State.Idle;
    this.ActivateDistance = 2f;
    this.Interactable = false;
    this.SecondaryInteractable = false;
    this.Follower.Brain.Info.Outfit = FollowerOutfitType.Rags;
    this.Follower.SetOutfit(FollowerOutfitType.Rags, false);
    this.Follower.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.FollowerEvent);
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    if (!this.triggered || this.Interactable)
      return;
    this.StartCoroutine((IEnumerator) this.DelayedInteractable());
  }

  public void SpawnAnim(bool pause = false)
  {
    this.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num = (double) this.Follower.SetBodyAnimation("spawn-in-base", false);
    this.Follower.AddBodyAnimation("pray", true, 0.0f);
    if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.OldAge)
      this.Follower.SetOutfit(FollowerOutfitType.Old, false);
    else if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.Ill)
      this.Follower.SetFaceAnimation("Emotions/emotion-sick", true);
    else if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.BecomeStarving)
      this.Follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
    else if (this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.Dissenter)
    {
      this.Follower.SetFaceAnimation("Emotions/emotion-dissenter", true);
      this.Follower.SetFaceAnimation("Emotions/emotion-dissenter", true);
      this.Follower.SetOutfit(FollowerOutfitType.Rags, false, this.Follower.Brain._directInfoAccess.StartingCursedState);
    }
    if (pause)
    {
      this.Follower.Spine.AnimationState.TimeScale = 0.0f;
    }
    else
    {
      this.portalSpine.gameObject.SetActive(true);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ShowPortal());
      AudioManager.Instance.PlayOneShot("event:/followers/teleport_to_base", this.Follower.gameObject);
      this.Follower.Spine.AnimationState.TimeScale = 1f;
    }
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator ShowPortal()
  {
    while (this.portalSpine.AnimationState == null)
      yield return (object) null;
    this.portalSpine.AnimationState.SetAnimation(0, "spawn-in-base", false);
  }

  private IEnumerator DelayedInteractable()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerRecruit followerRecruit = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      followerRecruit.Interactable = true;
      followerRecruit.SecondaryInteractable = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.dString = ScriptLocalization.Interactions.Indoctrinate;
  }

  public void ManualTriggerAnimateIn()
  {
    this.Follower.Spine.gameObject.SetActive(true);
    this.SpawnAnim();
    this.StartCoroutine((IEnumerator) this.DelayedInteractable());
    this.triggered = true;
  }

  protected override void Update()
  {
    base.Update();
    if (this.triggered || !(bool) (UnityEngine.Object) PlayerFarming.Instance || (double) Vector3.Distance(PlayerFarming.Instance.transform.position, this.transform.position) >= 10.0 || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive)
      return;
    this.Follower.Spine.gameObject.SetActive(true);
    this.SpawnAnim();
    this.StartCoroutine((IEnumerator) this.DelayedInteractable());
    this.triggered = true;
  }

  public override void GetLabel()
  {
    this.Label = !this.Interactable || this.Activating || !this.triggered ? "" : this.dString;
  }

  public override void GetSecondaryLabel() => this.SecondaryLabel = "";

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Activating = true;
    PlayerFarming.Instance.unitObject.speed = 0.0f;
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      this.DoRecruit(state, true);
      BiomeConstants.Instance.DepthOfFieldTween(1.5f, 4.5f, 10f, 1f, 145f);
      SimulationManager.Pause();
    })));
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    this.Activating = true;
    PlayerFarming.Instance.GoToAndStop(this.transform.position + ((double) state.transform.position.x < (double) this.transform.position.x ? new Vector3(-1.5f, -0.5f) : new Vector3(1.5f, -0.5f)), this.gameObject, GoToCallback: (System.Action) (() => this.DoRecruit(state, false)));
  }

  private void CallbackClose() => this.state.CURRENT_STATE = StateMachine.State.Idle;

  public void DoRecruit(StateMachine state, bool customise, bool newRecruit = true)
  {
    this.state = state;
    GameManager.GetInstance().CamFollowTarget.SnappyMovement = true;
    AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("FIRST_FOLLOWER"));
    if (DataManager.Instance.Followers.Count >= 4)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("GAIN_FIVE_FOLLOWERS"));
    if (DataManager.Instance.Followers.Count >= 9)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("TEN_FOLLOWERS"));
    if (DataManager.Instance.Followers.Count >= 19)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("TWENTY_FOLLOWERS"));
    if (newRecruit)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != this.Follower.Brain && allBrain.Location == this.Follower.Brain.Location)
          allBrain.AddThought(Thought.CultHasNewRecruit);
      }
    }
    DataManager.Instance.GameOverEnabled = true;
    if (DataManager.Instance.InGameOver)
    {
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GameOver);
      DataManager.Instance.InGameOver = false;
      DataManager.Instance.DisplayGameOverWarning = false;
      DataManager.Instance.GameOver = false;
    }
    FollowerRecruit.FollowerEventDelegate followerRecruited = FollowerRecruit.OnFollowerRecruited;
    if (followerRecruited != null)
      followerRecruited(this.Follower.Brain._directInfoAccess);
    this.CompleteCallBack(FollowerRole.Worker, customise);
  }

  private void CompleteCallBack(FollowerRole FollowerRole, bool customise)
  {
    PlayerFarming.Instance.GoToAndStop(this.transform.position + new Vector3(-1.5f, -0.1f), this.gameObject, GoToCallback: (System.Action) (() => PlayerFarming.Instance.transform.position = this.transform.position + new Vector3(-1.5f, -0.1f)));
    this.Follower.Brain.Info.FollowerRole = FollowerRole;
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    List<string> stringList = new List<string>()
    {
      "Conversation_NPC/FollowerSpawn/Line1",
      "Conversation_NPC/FollowerSpawn/Line2"
    };
    Entries.Add(new ConversationEntry(this.gameObject, stringList[UnityEngine.Random.Range(0, stringList.Count)])
    {
      soundPath = "event:/dialogue/followers/general_talk",
      pitchValue = this.Follower.Brain._directInfoAccess.follower_pitch,
      vibratoValue = this.Follower.Brain._directInfoAccess.follower_vibrato,
      SetZoom = true,
      Zoom = 4f,
      Offset = new Vector3(0.0f, -0.5f, 0.0f),
      CharacterName = $"<color=yellow>{this.Follower.Brain.Info.Name}</color>"
    });
    GameManager.GetInstance().CamFollowTarget.ZoomSpeedConversation = 1f;
    GameManager.GetInstance().CamFollowTarget.MaxZoomInConversation = 100f;
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.StartCoroutine((IEnumerator) this.SimpleNewRecruitRoutine(true)))), false);
  }

  private IEnumerator SimpleNewRecruitRoutine(bool customise)
  {
    FollowerRecruit followerRecruit = this;
    GameManager.GetInstance().OnConversationNext(followerRecruit.CameraBone, 4f);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(followerRecruit.transform.position, followerRecruit.Follower.transform.position);
    yield return (object) new WaitForSeconds(0.3f);
    if (customise)
    {
      followerRecruit.FollowersTurnedOff.Clear();
      foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
      {
        SkeletonAnimation spine = locationFollower.Spine;
        if (spine.gameObject.activeSelf && (double) Vector3.Distance(spine.transform.position, followerRecruit.transform.position) < 1.0 && (double) spine.transform.position.y < (double) followerRecruit.transform.position.y)
        {
          Debug.Log((object) ("Turning off gameobject: " + spine.name));
          MeshRenderer component = spine.gameObject.GetComponent<MeshRenderer>();
          component.enabled = false;
          followerRecruit.FollowersTurnedOff.Add(component);
        }
      }
      GameManager.GetInstance().CameraSetOffset(new Vector3(-2f, 0.0f, 0.0f));
      UIFollowerIndoctrinationMenuController indoctrinationMenuInstance = MonoSingleton<UIManager>.Instance.ShowIndoctrinationMenu(followerRecruit.Follower);
      // ISSUE: reference to a compiler-generated method
      indoctrinationMenuInstance.OnIndoctrinationCompleted += new System.Action(followerRecruit.\u003CSimpleNewRecruitRoutine\u003Eb__34_0);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController1 = indoctrinationMenuInstance;
      // ISSUE: reference to a compiler-generated method
      indoctrinationMenuController1.OnShown = indoctrinationMenuController1.OnShown + new System.Action(followerRecruit.\u003CSimpleNewRecruitRoutine\u003Eb__34_1);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController2 = indoctrinationMenuInstance;
      // ISSUE: reference to a compiler-generated method
      indoctrinationMenuController2.OnHide = indoctrinationMenuController2.OnHide + new System.Action(followerRecruit.\u003CSimpleNewRecruitRoutine\u003Eb__34_2);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController3 = indoctrinationMenuInstance;
      indoctrinationMenuController3.OnHidden = indoctrinationMenuController3.OnHidden + (System.Action) (() => indoctrinationMenuInstance = (UIFollowerIndoctrinationMenuController) null);
    }
    else
      followerRecruit.StartCoroutine((IEnumerator) followerRecruit.CharacterSetupCallback());
  }

  private void FollowerEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "indoctrinated") || this.Follower.Brain._directInfoAccess.StartingCursedState == Thought.OldAge)
      return;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.gameObject);
    CameraManager.instance.ShakeCameraForDuration(0.25f, 1f, 0.33f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    this.Follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    this.Follower.SetOutfit(FollowerOutfitType.Follower, false);
    this.Follower.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.FollowerEvent);
  }

  private IEnumerator CharacterSetupCallback()
  {
    FollowerRecruit recruit = this;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    recruit.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("recruit", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(recruit.transform.position, recruit.Follower.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", recruit.Follower.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", PlayerFarming.Instance.gameObject);
    double num1 = (double) recruit.Follower.SetBodyAnimation("Indoctrinate/indoctrinate-finish", false);
    yield return (object) new WaitForSeconds(4f);
    recruit.Follower.SimpleAnimator?.ResetAnimationsToDefaults();
    if (recruit.RecruitOnComplete)
      FollowerManager.RecruitFollower(recruit, false);
    yield return (object) new WaitForEndOfFrame();
    recruit.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) null;
    if (recruit.Follower.Brain.Stats.Thoughts.Count <= 0)
    {
      float num2 = UnityEngine.Random.value;
      if ((double) num2 <= 0.20000000298023224)
        recruit.Follower.Brain.AddThought(Thought.EnthusiasticNewRecruit);
      else if ((double) num2 > 0.20000000298023224 && (double) num2 < 0.800000011920929)
        recruit.Follower.Brain.AddThought(Thought.HappyNewRecruit);
      else if ((double) num2 >= 0.800000011920929)
        recruit.Follower.Brain.AddThought(Thought.UnenthusiasticNewRecruit);
    }
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      ThoughtData thought = allBrain.GetThought(Thought.Fasting);
      if (thought != null)
        recruit.Follower.Brain.AddThought(thought.Clone());
    }
    Debug.Log((object) ("RecruitOnComplete:  " + recruit.RecruitOnComplete.ToString()));
    if (recruit.RecruitOnComplete)
    {
      Debug.Log((object) "OnNewRecruit?.Invoke();");
      recruit.FollowerInteraction.enabled = true;
      recruit.FollowerInteraction.SelectTask(recruit.state, false, false);
      System.Action onNewRecruit = FollowerRecruit.OnNewRecruit;
      if (onNewRecruit != null)
        onNewRecruit();
      if (DataManager.Instance.FirstTimeInDungeon)
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.GetNewFollowersFromDungeon);
      FollowerManager.SpawnExistingRecruits(BiomeBaseManager.Instance.RecruitSpawnLocation.transform.position);
      CultFaithManager.AddThought(Thought.Cult_NewFolllower);
    }
    if (recruit.Follower.Brain.HasTrait(FollowerTrait.TraitType.NaturallySkeptical))
      CultFaithManager.AddThought(Thought.Cult_NewRecruitSkeptical);
    if (recruit.Follower.Brain.HasTrait(FollowerTrait.TraitType.NaturallyObedient))
      CultFaithManager.AddThought(Thought.Cult_NewRecruitObedient);
    UnityEngine.Object.Destroy((UnityEngine.Object) recruit);
  }

  public void ContinueRecruit()
  {
    Debug.Log((object) nameof (ContinueRecruit));
    this.StartCoroutine((IEnumerator) this.ContinueRecruitRoutine());
  }

  private IEnumerator ContinueRecruitRoutine()
  {
    FollowerRecruit recruit = this;
    Debug.Log((object) "ContinueRecruitRoutine ");
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(recruit.CameraBone, 4f);
    recruit.Follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    recruit.Follower.SetOutfit(FollowerOutfitType.Follower, false);
    recruit.Follower.SimpleAnimator.ResetAnimationsToDefaults();
    double num = (double) recruit.Follower.SetBodyAnimation("recruit-end", false);
    recruit.Follower.AddBodyAnimation("idle", true, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("recruit-end", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    if (recruit.RecruitOnComplete)
      FollowerManager.RecruitFollower(recruit, false);
    recruit.state.CURRENT_STATE = StateMachine.State.Idle;
    System.Action statueCallback = recruit.StatueCallback;
    if (statueCallback != null)
      statueCallback();
    UnityEngine.Object.Destroy((UnityEngine.Object) recruit);
  }

  public void InstantRecruit(bool followPlayer = false)
  {
    this.StartCoroutine((IEnumerator) this.InstantRecruitRoutine(followPlayer));
  }

  private IEnumerator InstantRecruitRoutine(bool followPlayer)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    FollowerRecruit recruit = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) recruit);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    recruit.Follower.SimpleAnimator.ResetAnimationsToDefaults();
    recruit.Follower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    recruit.Follower.SetOutfit(FollowerOutfitType.Follower, false);
    FollowerManager.RecruitFollower(recruit, followPlayer);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void CallbackSacrifice()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CarryBone.gameObject, 8f);
    PlayerFarming.Instance.GoToAndStop(ChurchFollowerManager.Instance.AltarPosition.gameObject, this.gameObject, GoToCallback: new System.Action(this.ContinueSacrifice));
  }

  private void ContinueSacrifice()
  {
    this.StartCoroutine((IEnumerator) ChurchFollowerManager.Instance.DoSacrificeRoutine((Interaction) this, this.Follower.Brain.Info.ID, new System.Action(this.CompleteSacrifice)));
  }

  private void CompleteSacrifice()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.Follower.SimpleAnimator.ResetAnimationsToDefaults();
    FollowerManager.RemoveRecruit(this.Follower.Brain.Info.ID);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    ChurchFollowerManager.Instance.ExitAllFollowers();
  }

  public delegate void FollowerEventDelegate(FollowerInfo info);
}
