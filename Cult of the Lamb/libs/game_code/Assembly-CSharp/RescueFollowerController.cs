// Decompiled with JetBrains decompiler
// Type: RescueFollowerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class RescueFollowerController : BaseMonoBehaviour
{
  public List<BaseMonoBehaviour> DisableForConversation = new List<BaseMonoBehaviour>();
  public List<SkeletonAnimation> ConversationSkeletons = new List<SkeletonAnimation>();
  public GameObject GoopCylinder;
  public GameObject Altar;
  public GameObject AltarBroken;
  public UnityEvent IntroConversationCallbacks;
  public GameObject GameObjectToDestroyIfAlreadyCompleted;
  public GameObject CameraTarget;
  public Vector3 TriggerArea = Vector3.zero;
  public float TriggerRadius = 5f;
  public BarricadeLine barricadeLine;
  public UnityEvent Callbacks;
  public bool Completed;
  [SerializeField]
  public Interaction_Follower follower;
  [SerializeField]
  public Interaction_Follower secondFollower;
  [SerializeField]
  public string freedAnimation = "tied-to-altar-rescue";
  public bool SetVariableOnComplete;
  public DataManager.Variables VariableToComplete;
  public bool introCompleted;
  public bool releasedFollower;

  public void OnEnable()
  {
    this.Invoke("DisableFollower", 0.1f);
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
  }

  public void Awake()
  {
    foreach (Behaviour behaviour in this.DisableForConversation)
      behaviour.enabled = false;
  }

  public void IntroPlay() => this.TriggerRadius = 10f;

  public void DisableFollower()
  {
    if (this.releasedFollower)
      return;
    this.follower.Interactable = false;
    if (!((UnityEngine.Object) this.secondFollower != (UnityEngine.Object) null))
      return;
    this.secondFollower.Interactable = false;
  }

  public void OnDisable()
  {
  }

  public IEnumerator WaitForPlayer()
  {
    RescueFollowerController followerController = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.FollowerAmbience);
    if (!followerController.introCompleted)
    {
      bool requiresIntro = false;
      if (!DataManager.Instance.FirstFollowerRescue || !DataManager.Instance.FirstDungeon1RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_1 || !DataManager.Instance.FirstDungeon2RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_2 || !DataManager.Instance.FirstDungeon3RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_3 || !DataManager.Instance.FirstDungeon4RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_4 || !DataManager.Instance.FirstDungeon6RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_5)
        requiresIntro = true;
      foreach (Behaviour behaviour in followerController.DisableForConversation)
        behaviour.enabled = false;
      while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
        yield return (object) null;
      while (LetterBox.IsPlaying)
        yield return (object) null;
      while (MMTransition.IsPlaying)
        yield return (object) null;
      yield return (object) new WaitForSeconds(0.25f);
      bool waiting = true;
      while (waiting)
      {
        foreach (PlayerFarming player in PlayerFarming.players)
        {
          if ((double) Vector3.Distance(followerController.transform.position + followerController.TriggerArea, player.transform.position) < (double) followerController.TriggerRadius)
            waiting = false;
        }
        yield return (object) null;
      }
      foreach (UnityEngine.Object @object in followerController.DisableForConversation)
      {
        if (@object == (UnityEngine.Object) null)
          requiresIntro = false;
      }
      if (PlayerFarming.Location == FollowerLocation.Dungeon1_5)
        requiresIntro = false;
      if (requiresIntro)
      {
        BlockingDoor.CloseAll();
        RoomLockController.CloseAll();
        if (PlayerFarming.Location == FollowerLocation.Dungeon1_6)
          followerController.DoConversation(false);
        else
          followerController.DoConversation();
        while (MMConversation.CURRENT_CONVERSATION != null)
          yield return (object) null;
        followerController.IntroConversationCallbacks?.Invoke();
      }
      Debug.Log((object) "BEGIN COMBAT");
      EnemyRoundsBase.Instance?.BeginCombat(true, new System.Action(followerController.Close));
    }
    foreach (BaseMonoBehaviour baseMonoBehaviour in followerController.DisableForConversation)
    {
      if ((UnityEngine.Object) baseMonoBehaviour != (UnityEngine.Object) null)
        baseMonoBehaviour.enabled = true;
    }
    BiomeGenerator.Instance.CurrentRoom.Active = true;
    followerController.GoopCylinder.SetActive(true);
    if ((bool) (UnityEngine.Object) followerController.barricadeLine)
      followerController.barricadeLine.Close();
    BlockingDoor.CloseAll();
    RoomLockController.CloseAll();
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.SpecialCombat);
    followerController.introCompleted = true;
    while ((UnityEngine.Object) EnemyRoundsBase.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    if ((bool) (UnityEngine.Object) EnemyRoundsBase.Instance && EnemyRoundsBase.Instance.Completed && !followerController.Completed)
      followerController.Close();
  }

  public void ReleaseFollower()
  {
    if (this.SetVariableOnComplete)
      DataManager.Instance.SetVariable(this.VariableToComplete, true);
    this.releasedFollower = true;
    this.Callbacks?.Invoke();
    this.follower.Interactable = true;
    if (!((UnityEngine.Object) this.secondFollower != (UnityEngine.Object) null))
      return;
    this.secondFollower.Interactable = true;
  }

  public void Close()
  {
    if (this.Completed)
      return;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardAmbience);
    Debug.Log((object) "CLOSE!");
    this.StartCoroutine((IEnumerator) this.CloseRoutine());
  }

  public IEnumerator CloseRoutine()
  {
    RescueFollowerController followerController = this;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(followerController.CameraTarget, 4f);
    yield return (object) new WaitForSeconds(1f);
    CircleCollider2D component = followerController.GoopCylinder.GetComponent<CircleCollider2D>();
    Bounds bounds = component.bounds;
    component.enabled = false;
    AstarPath.active.UpdateGraphs(bounds);
    followerController.GoopCylinder.SetActive(false);
    yield return (object) new WaitForSeconds(0.5f);
    if ((bool) (UnityEngine.Object) followerController.barricadeLine)
    {
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
      followerController.barricadeLine.Open();
      yield return (object) new WaitForSeconds(1.5f);
    }
    CameraManager.instance.ShakeCameraForDuration(1.3f, 1.4f, 0.4f);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(followerController.Altar.transform.position + Vector3.back * 0.5f);
    BiomeConstants.Instance.EmitGroundSmashVFXParticles(followerController.Altar.transform.position + Vector3.back * 0.05f);
    BiomeConstants.Instance.EmitParticleChunk(BiomeConstants.TypeOfParticle.stone, followerController.transform.position, Vector3.forward * 50f, 20);
    followerController.Altar.SetActive(false);
    followerController.AltarBroken.SetActive(true);
    followerController.follower.skeletonAnimation.AnimationState.SetAnimation(0, followerController.freedAnimation, false);
    followerController.follower.skeletonAnimation.AnimationState.AddAnimation(0, "unconverted", true, 0.0f);
    if ((UnityEngine.Object) followerController.secondFollower != (UnityEngine.Object) null)
      GameManager.GetInstance().WaitForSeconds(1f, new System.Action(followerController.\u003CCloseRoutine\u003Eb__28_0));
    AudioManager.Instance.PlayOneShot("event:/followers/break_free");
    yield return (object) new WaitForSeconds(1.5f);
    followerController.ReleaseFollower();
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    followerController.Completed = true;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.TriggerArea, this.TriggerRadius, Color.yellow);
  }

  public void DoConversation(bool OverwriteSoundPath = true)
  {
    List<ConversationEntry> Entries = PlayerFarming.Location == FollowerLocation.IntroDungeon ? this.GetFirstConvo() : this.GetDungeonConvo();
    if (OverwriteSoundPath)
    {
      foreach (ConversationEntry conversationEntry in Entries)
        conversationEntry.soundPath = "event:/enemy/vocals/humanoid/warning";
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
  }

  public List<ConversationEntry> GetFirstConvo()
  {
    return new List<ConversationEntry>()
    {
      new ConversationEntry(this.DisableForConversation[0].gameObject, "Conversation_NPC/FollowerRescue/FollowerRescue/Ritual0"),
      new ConversationEntry(this.DisableForConversation[1].gameObject, "Conversation_NPC/FollowerRescue/FollowerRescue/Ritual1")
    };
  }

  public List<ConversationEntry> GetDungeonConvo()
  {
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_6)
    {
      DataManager.Instance.FirstDungeon6RescueRoom = true;
      return this.GetD6Convo();
    }
    BaseMonoBehaviour baseMonoBehaviour = this.DisableForConversation[UnityEngine.Random.Range(0, this.DisableForConversation.Count)];
    string TermToSpeak = "";
    switch (PlayerFarming.Location - 7)
    {
      case FollowerLocation.Church:
        TermToSpeak = "Conversation_NPC/Leshy/BackStory/0";
        break;
      case FollowerLocation.Base:
        TermToSpeak = "Conversation_NPC/Heket/BackStory/0";
        break;
      case FollowerLocation.Lumberjack:
        TermToSpeak = "Conversation_NPC/Kallamar/BackStory/0";
        break;
      case FollowerLocation.Hub1:
        TermToSpeak = "Conversation_NPC/Shamura/BackStory/0";
        break;
    }
    return new List<ConversationEntry>()
    {
      new ConversationEntry(baseMonoBehaviour.gameObject, TermToSpeak)
    };
  }

  public List<ConversationEntry> GetD6Convo()
  {
    return new List<ConversationEntry>()
    {
      new ConversationEntry(this.ConversationSkeletons[0].gameObject, "Conversation_NPC/FollowerOnboarding/Freezing/2")
    };
  }

  [CompilerGenerated]
  public void \u003CCloseRoutine\u003Eb__28_0()
  {
    PlayerReturnToBase.Disabled = true;
    this.secondFollower.skeletonAnimation.AnimationState.SetAnimation(0, this.freedAnimation, false);
    this.secondFollower.skeletonAnimation.AnimationState.AddAnimation(0, "unconverted", true, 0.0f);
  }
}
