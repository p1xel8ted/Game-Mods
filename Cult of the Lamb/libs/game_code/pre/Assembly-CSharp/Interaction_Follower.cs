// Decompiled with JetBrains decompiler
// Type: Interaction_Follower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMBiomeGeneration;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class Interaction_Follower : Interaction
{
  private Villager_Info v_i;
  protected WorshipperInfoManager wim;
  private StateMachine RecruitState;
  public StateMachine StateMachine;
  public bool ForceSpecificSkin;
  [SpineSkin("", "", true, false, false)]
  public string ForceSkin = "";
  [SerializeField]
  private SkeletonAnimation portalSpine;
  public ParticleSystem recruitParticles;
  public SkeletonAnimation skeletonAnimation;
  public FollowerInfo followerInfo;
  private string q1Title = "Interactions/FollowerSpawn/Convert/Title";
  private string q1Description = "Interactions/FollowerSpawn/Convert/Description";
  private string q2Title = "Interactions/FollowerSpawn/Consume/Title";
  private string q2Description = "Interactions/FollowerSpawn/Consume/Description";
  private string skin;
  public System.Action followerInfoAssigned;
  private Objectives_FindFollower findFollowerObjective;
  private string sRescue;
  [HideInInspector]
  public int Cost;
  protected bool Activated;
  private int Souls;
  private StateMachine CompanionState;
  public GameObject ConversionBone;
  private EventInstance receiveLoop;

  protected virtual void Start()
  {
    this.RecruitState = this.GetComponent<StateMachine>();
    this.skeletonAnimation = this.GetComponentInChildren<SkeletonAnimation>();
    this.UpdateLocalisation();
    this.StateMachine = this.GetComponent<StateMachine>();
    this.skin = this.ForceSpecificSkin ? this.ForceSkin : "";
    if (!this.ForceSpecificSkin)
    {
      this.skin = DataManager.GetRandomLockedSkin();
      if (this.skin.IsNullOrEmpty())
        this.skin = DataManager.GetRandomSkin();
    }
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Type == Objectives.TYPES.FIND_FOLLOWER && ((Objectives_FindFollower) objective).TargetLocation == BiomeGenerator.Instance.DungeonLocation)
      {
        this.findFollowerObjective = (Objectives_FindFollower) objective;
        break;
      }
    }
    this.followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, this.skin);
    if (this.followerInfo.SkinName == "Giraffe")
      this.followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Sparkles");
    if (this.findFollowerObjective != null)
    {
      this.followerInfo.SkinName = this.findFollowerObjective.FollowerSkin;
      this.followerInfo.SkinCharacter = this.followerInfo.SkinName.Contains("Boss") ? WorshipperData.Instance.GetSkinIndexFromName(this.followerInfo.SkinName) : WorshipperData.Instance.GetSkinIndexFromName(this.followerInfo.SkinName.StripNumbers());
      this.followerInfo.Name = this.findFollowerObjective.TargetFollowerName;
      this.skin = this.followerInfo.SkinName;
    }
    this.v_i = Villager_Info.NewCharacter(this.skin);
    this.wim = this.GetComponent<WorshipperInfoManager>();
    this.wim.SetV_I(this.v_i);
    this.ActivateDistance = 3f;
    System.Action followerInfoAssigned = this.followerInfoAssigned;
    if (followerInfoAssigned == null)
      return;
    followerInfoAssigned();
  }

  public override void OnDisableInteraction() => base.OnDisableInteraction();

  protected override void OnDestroy() => base.OnDestroy();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sRescue = ScriptLocalization.Interactions.Rescue;
  }

  public override void GetLabel()
  {
    this.Label = this.Activated || !this.Interactable ? "" : this.sRescue;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    this.EndIndicateHighlighted();
    this.state = state;
    this.Activated = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
    PlayerFarming.Instance.GoToAndStop(this.transform.position + Vector3.left * 1.5f, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.FollowerChoiceIE())));
  }

  private IEnumerator GiveSouls()
  {
    Interaction_Follower interactionFollower1 = this;
    yield return (object) new WaitForSeconds(1.5f);
    interactionFollower1.Souls = interactionFollower1.Cost;
    while (true)
    {
      Interaction_Follower interactionFollower2 = interactionFollower1;
      int num1 = interactionFollower1.Souls - 1;
      int num2 = num1;
      interactionFollower2.Souls = num2;
      if (num1 >= 0)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionFollower1.state.transform.position);
        ResourceCustomTarget.Create(interactionFollower1.ConversionBone, interactionFollower1.state.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
        yield return (object) new WaitForSeconds(0.2f);
      }
      else
        break;
    }
    Inventory.ChangeItemQuantity(20, interactionFollower1.Cost);
  }

  private IEnumerator PositionPlayer()
  {
    Interaction_Follower interactionFollower = this;
    yield return (object) new WaitForSeconds(0.25f);
    interactionFollower.state.facingAngle = Utils.GetAngle(interactionFollower.state.transform.position, interactionFollower.transform.position);
    Vector3 TargetPosition = interactionFollower.transform.position + ((double) interactionFollower.state.transform.position.x < (double) interactionFollower.transform.position.x ? Vector3.left : Vector3.right) * 1.5f;
    while ((double) Vector3.Distance(interactionFollower.state.transform.position, TargetPosition) > 0.10000000149011612)
    {
      interactionFollower.state.transform.position = Vector3.Lerp(interactionFollower.state.transform.position, TargetPosition, 2f * Time.deltaTime);
      yield return (object) null;
    }
  }

  private IEnumerator ConvertToWarrior()
  {
    Interaction_Follower interactionFollower = this;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionFollower.ConversionBone, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    interactionFollower.state.GetComponent<HealthPlayer>().untouchable = true;
    yield return (object) new WaitForSeconds(0.25f);
    SimpleSpineAnimator playerAnimator = interactionFollower.state.GetComponentInChildren<SimpleSpineAnimator>();
    playerAnimator.Animate("floating", 0, true);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    if (!DataManager.GetFollowerSkinUnlocked(interactionFollower.skin))
      DataManager.SetFollowerSkinUnlocked(interactionFollower.skin);
    FollowerManager.CreateNewRecruit(interactionFollower.followerInfo, NotificationCentre.NotificationType.NewRecruit);
    RoomLockController.RoomCompleted();
    ThoughtData data = FollowerThoughts.GetData((double) UnityEngine.Random.value >= 0.699999988079071 ? ((double) UnityEngine.Random.value > 0.30000001192092896 ? Thought.InstantBelieverRescued : Thought.ResentfulRescued) : Thought.GratefulRecued);
    data.Init();
    interactionFollower.followerInfo.Thoughts.Add(data);
    playerAnimator.Animate("floating-land", 0, false);
    playerAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionFollower.gameObject);
  }

  private IEnumerator FollowerChoiceIE()
  {
    Interaction_Follower interactionFollower = this;
    interactionFollower.Interactable = false;
    GameManager.GetInstance().OnConversationNext(interactionFollower.ConversionBone, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    if (true)
      yield return (object) interactionFollower.StartCoroutine((IEnumerator) interactionFollower.ConvertIE());
    else
      yield return (object) interactionFollower.StartCoroutine((IEnumerator) interactionFollower.ConsumefollowerRoutine());
    RoomLockController.RoomCompleted();
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    interactionFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionFollower.gameObject);
  }

  private IEnumerator ConvertIE()
  {
    Interaction_Follower interactionFollower = this;
    AudioManager.Instance.PlayOneShot("event:/followers/rescue", interactionFollower.gameObject);
    interactionFollower.skeletonAnimation.AnimationState.SetAnimation(0, "convert-short", false);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", interactionFollower.gameObject);
    interactionFollower.recruitParticles.Play();
    interactionFollower.portalSpine.gameObject.SetActive(true);
    interactionFollower.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    interactionFollower.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    interactionFollower.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration = PlayerFarming.Instance.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration - 1f);
    FollowerManager.CreateNewRecruit(interactionFollower.followerInfo, NotificationCentre.NotificationType.NewRecruit);
    DataManager.SetFollowerSkinUnlocked(interactionFollower.followerInfo.SkinName);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
    int num1 = (int) interactionFollower.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    if (interactionFollower.findFollowerObjective != null)
    {
      interactionFollower.findFollowerObjective.Complete();
      ObjectiveManager.UpdateObjective((ObjectivesData) interactionFollower.findFollowerObjective);
      StoryData followerStoryData = Quests.GetFollowerStoryData(interactionFollower.findFollowerObjective.Follower);
      if (followerStoryData != null)
      {
        foreach (StoryDataItem storyDataItem in Quests.GetChildStoryDataItemsFromStoryDataItem(followerStoryData.EntryStoryItem))
          storyDataItem.TargetFollowerID_1 = interactionFollower.followerInfo.ID;
      }
    }
    float num2 = UnityEngine.Random.value;
    Thought thought = Thought.None;
    if ((double) num2 < 0.699999988079071)
    {
      float num3 = UnityEngine.Random.value;
      if ((double) num3 <= 0.30000001192092896)
        thought = Thought.HappyConvert;
      else if ((double) num3 > 0.30000001192092896 && (double) num3 < 0.60000002384185791)
        thought = Thought.GratefulConvert;
      else if ((double) num3 >= 0.60000002384185791)
        thought = Thought.SkepticalConvert;
    }
    else
      thought = (double) UnityEngine.Random.value > 0.30000001192092896 || DataManager.Instance.Followers.Count <= 0 ? Thought.InstantBelieverConvert : Thought.ResentfulConvert;
    ThoughtData data = FollowerThoughts.GetData(thought);
    data.Init();
    interactionFollower.followerInfo.Thoughts.Add(data);
  }

  private IEnumerator ConsumefollowerRoutine()
  {
    Interaction_Follower interactionFollower = this;
    GameManager.GetInstance().OnConversationNext(interactionFollower.gameObject, 5f);
    GameManager.GetInstance().AddPlayerToCamera();
    Vector3 vector3 = (double) interactionFollower.state.transform.position.x < (double) interactionFollower.transform.position.x ? Vector3.left : Vector3.right;
    Vector3 TargetPosition = interactionFollower.transform.position + vector3 * 2f;
    PlayerFarming.Instance.GoToAndStop(TargetPosition);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, interactionFollower.transform.position);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sacrifice", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/negative_acknowledge", interactionFollower.gameObject);
    interactionFollower.skeletonAnimation.AnimationState.SetAnimation(0, "sacrifice", false);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(3.23333335f);
    int i = 0;
    while (++i < 50)
    {
      BlackSoul blackSoul = InventoryItem.SpawnBlackSoul(1, interactionFollower.transform.position + Vector3.back);
      if ((UnityEngine.Object) blackSoul != (UnityEngine.Object) null)
        blackSoul.SetAngle((float) (270 + UnityEngine.Random.Range(-90, 90)), new Vector2(2f, 4f));
      yield return (object) new WaitForSeconds(0.01f);
    }
    yield return (object) new WaitForSeconds(1f);
  }
}
