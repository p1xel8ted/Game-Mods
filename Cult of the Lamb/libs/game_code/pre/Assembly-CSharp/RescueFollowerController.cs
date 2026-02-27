// Decompiled with JetBrains decompiler
// Type: RescueFollowerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
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
  private GameObject Player;
  public BarricadeLine barricadeLine;
  public UnityEvent Callbacks;
  private bool Completed;
  [SerializeField]
  private Interaction_Follower follower;
  [SerializeField]
  private string freedAnimation = "tied-to-altar-rescue";
  public bool SetVariableOnComplete;
  public DataManager.Variables VariableToComplete;
  private bool introCompleted;

  private void OnEnable()
  {
    this.Invoke("DisableFollower", 0.1f);
    this.StartCoroutine((IEnumerator) this.WaitForPlayer());
  }

  private void Awake()
  {
    foreach (Behaviour behaviour in this.DisableForConversation)
      behaviour.enabled = false;
  }

  public void IntroPlay() => this.TriggerRadius = 10f;

  private void DisableFollower() => this.follower.Interactable = false;

  private void OnDisable()
  {
  }

  private IEnumerator WaitForPlayer()
  {
    RescueFollowerController followerController = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.FollowerAmbience);
    if (!followerController.introCompleted)
    {
      bool requiresIntro = false;
      if (!DataManager.Instance.FirstFollowerRescue || !DataManager.Instance.FirstDungeon1RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_1 || !DataManager.Instance.FirstDungeon2RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_2 || !DataManager.Instance.FirstDungeon3RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_3 || !DataManager.Instance.FirstDungeon4RescueRoom && PlayerFarming.Location == FollowerLocation.Dungeon1_4)
        requiresIntro = true;
      foreach (Behaviour behaviour in followerController.DisableForConversation)
        behaviour.enabled = false;
      while ((UnityEngine.Object) (followerController.Player = GameObject.FindGameObjectWithTag("Player")) == (UnityEngine.Object) null)
        yield return (object) null;
      while (LetterBox.IsPlaying)
        yield return (object) null;
      BlockingDoor.CloseAll();
      RoomLockController.CloseAll();
      while ((double) Vector3.Distance(followerController.transform.position + followerController.TriggerArea, followerController.Player.transform.position) > (double) followerController.TriggerRadius)
        yield return (object) null;
      if (requiresIntro)
      {
        followerController.DoConversation();
        while (MMConversation.CURRENT_CONVERSATION != null)
          yield return (object) null;
        followerController.IntroConversationCallbacks?.Invoke();
      }
      Debug.Log((object) "BEGIN COMBAT");
      EnemyRoundsBase.Instance?.BeginCombat(true, new System.Action(followerController.Close));
    }
    foreach (Behaviour behaviour in followerController.DisableForConversation)
      behaviour.enabled = true;
    BiomeGenerator.Instance.CurrentRoom.Active = true;
    followerController.GoopCylinder.SetActive(true);
    if ((bool) (UnityEngine.Object) followerController.barricadeLine)
      followerController.barricadeLine.Close();
    BlockingDoor.CloseAll();
    RoomLockController.CloseAll();
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.SpecialCombat);
    followerController.introCompleted = true;
  }

  public void ReleaseFollower()
  {
    if (this.SetVariableOnComplete)
      DataManager.Instance.SetVariable(this.VariableToComplete, true);
    this.Callbacks?.Invoke();
    this.follower.Interactable = true;
  }

  private void Close()
  {
    if (this.Completed)
      return;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardAmbience);
    Debug.Log((object) "CLOSE!");
    this.StartCoroutine((IEnumerator) this.CloseRoutine());
  }

  private IEnumerator CloseRoutine()
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
    AudioManager.Instance.PlayOneShot("event:/followers/break_free");
    yield return (object) new WaitForSeconds(1.5f);
    followerController.ReleaseFollower();
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    followerController.Completed = true;
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.TriggerArea, this.TriggerRadius, Color.yellow);
  }

  private void DoConversation()
  {
    List<ConversationEntry> Entries = PlayerFarming.Location == FollowerLocation.IntroDungeon ? this.GetFirstConvo() : this.GetDungeonConvo();
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.soundPath = "event:/enemy/vocals/humanoid/warning";
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
  }

  private List<ConversationEntry> GetFirstConvo()
  {
    return new List<ConversationEntry>()
    {
      new ConversationEntry(this.DisableForConversation[0].gameObject, "Conversation_NPC/FollowerRescue/FollowerRescue/Ritual0"),
      new ConversationEntry(this.DisableForConversation[1].gameObject, "Conversation_NPC/FollowerRescue/FollowerRescue/Ritual1")
    };
  }

  private List<ConversationEntry> GetDungeonConvo()
  {
    BaseMonoBehaviour baseMonoBehaviour = this.DisableForConversation[UnityEngine.Random.Range(0, this.DisableForConversation.Count)];
    string TermToSpeak = "";
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        TermToSpeak = "Conversation_NPC/Leshy/BackStory/0";
        break;
      case FollowerLocation.Dungeon1_2:
        TermToSpeak = "Conversation_NPC/Heket/BackStory/0";
        break;
      case FollowerLocation.Dungeon1_3:
        TermToSpeak = "Conversation_NPC/Kallamar/BackStory/0";
        break;
      case FollowerLocation.Dungeon1_4:
        TermToSpeak = "Conversation_NPC/Shamura/BackStory/0";
        break;
    }
    return new List<ConversationEntry>()
    {
      new ConversationEntry(baseMonoBehaviour.gameObject, TermToSpeak)
    };
  }
}
