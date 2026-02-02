// Decompiled with JetBrains decompiler
// Type: Interaction_Follower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMBiomeGeneration;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class Interaction_Follower : Interaction
{
  public static List<Interaction_Follower> Recruits = new List<Interaction_Follower>();
  public Villager_Info v_i;
  public WorshipperInfoManager wim;
  public StateMachine RecruitState;
  public StateMachine StateMachine;
  public bool ForceSpecificSkin;
  [SpineSkin("", "", true, false, false)]
  public string ForceSkin = "";
  [SerializeField]
  public SkeletonAnimation portalSpine;
  [SerializeField]
  public int index;
  public ParticleSystem recruitParticles;
  public SkeletonAnimation skeletonAnimation;
  public FollowerInfo followerInfo;
  public string q1Title = "Interactions/FollowerSpawn/Convert/Title";
  public string q1Description = "Interactions/FollowerSpawn/Convert/Description";
  public string q2Title = "Interactions/FollowerSpawn/Consume/Title";
  public string q2Description = "Interactions/FollowerSpawn/Consume/Description";
  public string skin;
  public bool initialised;
  public System.Action followerInfoAssigned;
  public Objectives_FindFollower findFollowerObjective;
  public string sRescue;
  [HideInInspector]
  public int Cost;
  public bool Activated;
  public int Souls;
  public StateMachine CompanionState;
  public GameObject ConversionBone;
  public EventInstance receiveLoop;

  public virtual void Start()
  {
    Interaction_Follower.Recruits.Add(this);
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
    if (this.followerInfo.SkinName == "Poppy")
      this.followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Poppy");
    if (this.followerInfo.SkinName == "Pudding")
      this.followerInfo.Name = LocalizationManager.GetTranslation("FollowerNames/Pudding");
    if (this.findFollowerObjective != null)
    {
      this.followerInfo.SkinName = this.findFollowerObjective.FollowerSkin;
      if (this.findFollowerObjective.FollowerSkin.Contains("Jalala") && this.index == 1)
        this.followerInfo.SkinName = "Rinor";
      bool flag = true;
      if (this.findFollowerObjective.Follower == 99996 && this.followerInfo.SkinName == "Mushroom")
        flag = false;
      if (this.findFollowerObjective.FollowerSkin.Contains("Jalala") && this.index == 1)
      {
        this.followerInfo.SkinName = "Rinor";
        flag = false;
      }
      if (flag)
      {
        this.followerInfo.Siblings.Add(this.findFollowerObjective.Follower);
        FollowerInfo.GetInfoByID(this.findFollowerObjective.Follower, true)?.Siblings.Add(this.followerInfo.ID);
      }
      this.followerInfo.SkinCharacter = this.followerInfo.SkinName.Contains("Boss") ? WorshipperData.Instance.GetSkinIndexFromName(this.followerInfo.SkinName) : WorshipperData.Instance.GetSkinIndexFromName(this.followerInfo.SkinName.StripNumbers());
      this.followerInfo.Name = this.findFollowerObjective.TargetFollowerName;
      this.skin = this.followerInfo.SkinName;
      if (this.findFollowerObjective.FollowerSkin.Contains("Jalala"))
      {
        this.followerInfo.Outfit = FollowerOutfitType.None;
        if (this.index == 0)
        {
          this.followerInfo.ID = 99997;
          this.followerInfo.StartingCursedState = Thought.Injured;
          this.followerInfo.Clothing = FollowerClothingType.Pilgrim_DLC;
        }
        else if (this.index == 1)
        {
          this.followerInfo.Name = "Rinor";
          this.followerInfo.ID = 99999;
          this.followerInfo.Clothing = FollowerClothingType.Pilgrim_DLC2;
        }
      }
    }
    this.v_i = Villager_Info.NewCharacter(this.skin);
    this.wim = this.GetComponent<WorshipperInfoManager>();
    this.wim.SetV_I(this.v_i);
    this.ActivateDistance = 3f;
    if (this.followerInfo.Clothing != FollowerClothingType.None && !this.initialised)
      FollowerBrain.SetFollowerCostume(this.skeletonAnimation.Skeleton, this.followerInfo, forceUpdate: true, setData: false);
    System.Action followerInfoAssigned = this.followerInfoAssigned;
    if (followerInfoAssigned != null)
      followerInfoAssigned();
    this.initialised = true;
  }

  public override void OnDisableInteraction() => base.OnDisableInteraction();

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_Follower.Recruits.Remove(this);
  }

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
    this.state = state;
    this.EndIndicateHighlighted(this.playerFarming);
    this.Activated = true;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: this.playerFarming);
    GameManager.GetInstance().OnConversationNew(false, false, state.GetComponent<PlayerFarming>());
    GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject);
    this.playerFarming.GoToAndStop(this.transform.position + Vector3.left * 1.5f, this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.FollowerChoiceIE())));
  }

  public IEnumerator GiveSouls()
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

  public IEnumerator PositionPlayer()
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

  public IEnumerator ConvertToWarrior()
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
    if (!DataManager.GetFollowerSkinUnlocked(interactionFollower.skin) && interactionFollower.skin != "Mushroom" && interactionFollower.skin != "Bug")
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

  public IEnumerator FollowerChoiceIE()
  {
    Interaction_Follower interactionFollower = this;
    interactionFollower.Interactable = false;
    GameManager.GetInstance().OnConversationNext(interactionFollower.ConversionBone, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    if (true)
      yield return (object) interactionFollower.StartCoroutine((IEnumerator) interactionFollower.ConvertIE());
    else
      yield return (object) interactionFollower.StartCoroutine((IEnumerator) interactionFollower.ConsumefollowerRoutine());
    if (Interaction_Follower.Recruits.Count <= 1)
    {
      RoomLockController.RoomCompleted();
      PlayerReturnToBase.Disabled = false;
    }
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.SetStateForAllPlayers();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionFollower.gameObject);
  }

  public IEnumerator ConvertIE(System.Action callback = null)
  {
    Interaction_Follower interactionFollower = this;
    AudioManager.Instance.PlayOneShot("event:/followers/rescue", interactionFollower.gameObject);
    interactionFollower.skeletonAnimation.AnimationState.SetAnimation(0, "convert-short", false);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", interactionFollower.gameObject);
    interactionFollower.recruitParticles.Play();
    interactionFollower.portalSpine.gameObject.SetActive(true);
    interactionFollower.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", interactionFollower.playerFarming.gameObject);
    interactionFollower.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", interactionFollower.playerFarming.gameObject, true);
    if ((UnityEngine.Object) interactionFollower.state == (UnityEngine.Object) null)
      interactionFollower.state = PlayerFarming.Instance.state;
    interactionFollower.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration = interactionFollower.playerFarming.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration - 1f);
    FollowerManager.CreateNewRecruit(interactionFollower.followerInfo, NotificationCentre.NotificationType.NewRecruit);
    if (interactionFollower.skin != "Mushroom" && interactionFollower.skin != "Bug")
      DataManager.SetFollowerSkinUnlocked(interactionFollower.followerInfo.SkinName);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", interactionFollower.playerFarming.gameObject);
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
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator ConsumefollowerRoutine()
  {
    Interaction_Follower interactionFollower = this;
    GameManager.GetInstance().OnConversationNext(interactionFollower.gameObject, 5f);
    GameManager.GetInstance().AddPlayerToCamera();
    Vector3 vector3 = (double) interactionFollower.state.transform.position.x < (double) interactionFollower.transform.position.x ? Vector3.left : Vector3.right;
    Vector3 TargetPosition = interactionFollower.transform.position + vector3 * 2f;
    interactionFollower.playerFarming.GoToAndStop(TargetPosition);
    while (interactionFollower.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionFollower.playerFarming.state.facingAngle = Utils.GetAngle(interactionFollower.playerFarming.transform.position, interactionFollower.transform.position);
    interactionFollower.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionFollower.playerFarming.simpleSpineAnimator.Animate("sacrifice", 0, false);
    interactionFollower.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
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

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__28_0()
  {
    this.StartCoroutine((IEnumerator) this.FollowerChoiceIE());
  }
}
