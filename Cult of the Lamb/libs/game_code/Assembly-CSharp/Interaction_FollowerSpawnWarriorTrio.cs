// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerSpawnWarriorTrio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_FollowerSpawnWarriorTrio : Interaction
{
  public bool DisableOnHighlighted;
  public FollowerInfo v_i;
  public FollowerInfoManager wim;
  public Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> warriorFollowers;
  public StateMachine[] stateMachines;
  public SkeletonAnimation[] followerSpines;
  public SkeletonAnimation[] portalSpines;
  public string centreFollowerName;
  public ParticleSystem[] recruitParticles;
  public string ForceSkin = "";
  public FollowerInfo[] followerInfo;
  public FollowerOutfit _outfit;
  public LayerMask collisionMask;
  public string q1Title = "Interactions/FollowerSpawn/Convert/Title";
  public string q1Description = "Interactions/FollowerSpawn/Convert/Description";
  public string q2Title = "Interactions/FollowerSpawn/Consume/Title";
  public string q2Description = "Interactions/FollowerSpawn/Consume/Description";
  public string animationTransform = "transform";
  public string animationUnconverted = "unconverted";
  public string animationConvertShort = "convert-short";
  public string animationIdle = "idle";
  public string animationSpecialActivateLong = "specials/special-activate-long";
  public string followerAscendSFX = "event:/followers/ascend";
  public EventInstance LoopInstance;
  public EventInstance receiveLoop;
  public System.Action followerInfoAssigned;
  public System.Action OnFollowerDisabled;
  public string sRescue;
  [HideInInspector]
  public int Cost;
  public bool Activated;
  public float playerDistance = 3f;

  public override void OnDisable()
  {
    base.OnDisable();
    System.Action followerDisabled = this.OnFollowerDisabled;
    if (followerDisabled == null)
      return;
    followerDisabled();
  }

  public virtual void Start()
  {
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
  }

  public IEnumerator WaitForFollowerToStopMoving(float Duration)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_FollowerSpawnWarriorTrio spawnWarriorTrio = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().OnConversationEnd();
      spawnWarriorTrio.OnFollowerDisabled = (System.Action) null;
      spawnWarriorTrio.Interactable = true;
      spawnWarriorTrio.GetLabel();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    spawnWarriorTrio.OnFollowerDisabled += new System.Action(spawnWarriorTrio.\u003CWaitForFollowerToStopMoving\u003Eb__30_0);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(spawnWarriorTrio.gameObject, 5f);
    spawnWarriorTrio.GetLabel();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(Duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
    if (this.DisableOnHighlighted)
      return;
    base.IndicateHighlighted(playerFarming);
  }

  public float GetTransformAnimLength(GameObject follower)
  {
    return follower.GetComponentInChildren<SkeletonAnimation>().skeleton.Data.FindAnimation(this.animationTransform).Duration;
  }

  public void Play(
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> warriorFollowers,
    GameObject centreFollower)
  {
    this.StopAllCoroutines();
    this.Interactable = false;
    this.warriorFollowers = warriorFollowers;
    this.stateMachines = this.GetStateMachines(warriorFollowers);
    this.followerSpines = this.GetSpinesFromParentName(warriorFollowers, "Follower");
    this.portalSpines = this.GetSpinesFromParentName(warriorFollowers, "VFX");
    this.recruitParticles = this.GetRecruitParticles(warriorFollowers);
    this.centreFollowerName = warriorFollowers[centreFollower].displayName;
    this.StartCoroutine((IEnumerator) this.DoTransformAnimationSequence());
    this.StartCoroutine((IEnumerator) this.WaitForFollowerToStopMoving(this.GetTransformAnimLength(centreFollower)));
    this.SetupFollowerInfoManagers(warriorFollowers);
    this.SetupFollowerInfos(warriorFollowers);
    this.MoveFollowersIfTooFarAwayFromCentre(warriorFollowers);
    this.UpdateLocalisation();
  }

  public StateMachine[] GetStateMachines(
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> warriorFollowers)
  {
    StateMachine[] stateMachines = new StateMachine[3];
    GameObject[] array = warriorFollowers.Keys.ToArray<GameObject>();
    for (int index = 0; index < array.Length; ++index)
    {
      StateMachine component = array[index].GetComponent<StateMachine>();
      stateMachines[index] = component;
    }
    return stateMachines;
  }

  public void MoveFollowersIfTooFarAwayFromCentre(
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> warriorFollowers)
  {
    if ((double) Vector3.Distance(this.transform.position, Vector3.zero) <= 4.5)
      return;
    Vector3 vector3 = (Vector3.zero - this.transform.position) * 0.5f;
    foreach (GameObject key in warriorFollowers.Keys)
      key.transform.DOMove(this.transform.position + vector3 + warriorFollowers[key].relativePosition, 0.3f);
  }

  public SkeletonAnimation[] GetSpinesFromParentName(
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> warriorFollowers,
    string objectName)
  {
    SkeletonAnimation[] spinesFromParentName = new SkeletonAnimation[warriorFollowers.Keys.Count];
    GameObject[] array = warriorFollowers.Keys.ToArray<GameObject>();
    for (int index = 0; index < array.Length; ++index)
    {
      foreach (Transform componentsInChild in array[index].GetComponentsInChildren<Transform>(true))
      {
        if (componentsInChild.name == objectName)
        {
          SkeletonAnimation componentInChildren = componentsInChild.GetComponentInChildren<SkeletonAnimation>();
          spinesFromParentName[index] = componentInChildren;
          break;
        }
      }
    }
    return spinesFromParentName;
  }

  public ParticleSystem[] GetRecruitParticles(
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> warriorFollowers)
  {
    ParticleSystem[] recruitParticles = new ParticleSystem[warriorFollowers.Keys.Count];
    GameObject[] array = warriorFollowers.Keys.ToArray<GameObject>();
    for (int index = 0; index < array.Length; ++index)
    {
      ParticleSystem componentInChildren = array[index].GetComponentInChildren<ParticleSystem>(true);
      recruitParticles[index] = componentInChildren;
    }
    return recruitParticles;
  }

  public void SetupFollowerInfoManagers(
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> warriorFollowers)
  {
    foreach (GameObject key in warriorFollowers.Keys)
    {
      FollowerInfoManager component = key.GetComponent<FollowerInfoManager>();
      component.ForceSkin = true;
      component.ForceOutfitSkin = true;
      component.ForceSkinOverride = warriorFollowers[key].skinName;
      component.ForceOutfitSkinOverride = "Clothes/Rags";
      WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(warriorFollowers[key].skinName);
      if (colourData != null)
      {
        foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[0].SlotAndColours)
          component.SetSlotColour(slotAndColour.Slot, slotAndColour.color);
      }
      component.NewV_I();
    }
  }

  public IEnumerator DoTransformAnimationSequence()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_FollowerSpawnWarriorTrio spawnWarriorTrio = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      spawnWarriorTrio.AddFollowerAnimaions(spawnWarriorTrio.animationUnconverted);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) spawnWarriorTrio.StartCoroutine((IEnumerator) spawnWarriorTrio.SetFollowerAnimaions(spawnWarriorTrio.animationTransform, playOffset: 0.1f));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator SetFollowerAnimaions(string animation, bool loop = false, float playOffset = 0.0f)
  {
    SkeletonAnimation[] skeletonAnimationArray = this.followerSpines;
    for (int index = 0; index < skeletonAnimationArray.Length; ++index)
    {
      skeletonAnimationArray[index].AnimationState.SetAnimation(0, animation, loop);
      yield return (object) new WaitForSeconds(playOffset);
    }
    skeletonAnimationArray = (SkeletonAnimation[]) null;
  }

  public void AddFollowerAnimaions(string animation, bool loop = true)
  {
    foreach (SkeletonAnimation followerSpine in this.followerSpines)
      followerSpine.AnimationState.AddAnimation(0, animation, loop, 0.0f);
  }

  public void SetupFollowerInfos(
    Dictionary<GameObject, Interaction_FollowerSpawnWarriorTrio.FollowerObjectMetadata> warriorFollowers)
  {
    this.followerInfo = new FollowerInfo[warriorFollowers.Keys.Count];
    GameObject[] array = warriorFollowers.Keys.ToArray<GameObject>();
    for (int index = 0; index < array.Length; ++index)
    {
      FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, warriorFollowers[array[index]].skinName);
      string displayName = warriorFollowers[array[index]].displayName;
      followerInfo.Name = LocalizationManager.GetTranslation(displayName);
      if (warriorFollowers[array[index]].skinName == "Boss Dog 4")
        followerInfo.ID = 10016;
      else if (warriorFollowers[array[index]].skinName == "Boss Dog 5")
        followerInfo.ID = 10015;
      else if (warriorFollowers[array[index]].skinName == "Boss Dog 6")
        followerInfo.ID = 10014;
      this.followerInfo[index] = followerInfo;
    }
    this.ActivateDistance = 3f;
    System.Action followerInfoAssigned = this.followerInfoAssigned;
    if (followerInfoAssigned == null)
      return;
    followerInfoAssigned();
  }

  public IEnumerator FollowerChoiceIE()
  {
    Interaction_FollowerSpawnWarriorTrio spawnWarriorTrio = this;
    GameManager.GetInstance().AddPlayerToCamera();
    if ((bool) (UnityEngine.Object) MiniBossController.Instance)
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(spawnWarriorTrio.gameObject, "Conversation_NPC/WarriorTrio/Indoctrinate/0"),
        new ConversationEntry(spawnWarriorTrio.gameObject, "Conversation_NPC/WarriorTrio/Indoctrinate/1")
      };
      Entries[0].CharacterName = spawnWarriorTrio.centreFollowerName;
      Entries[0].Zoom = 5f;
      Entries[0].SetZoom = true;
      Entries[0].DefaultAnimation = "unconverted";
      Entries[0].Animation = "unconverted-talk";
      Entries[0].LoopAnimation = true;
      foreach (ConversationEntry conversationEntry in Entries)
      {
        conversationEntry.soundPath = "event:/dialogue/followers/general_talk";
        conversationEntry.pitchValue = spawnWarriorTrio.followerInfo[0].follower_pitch;
        conversationEntry.vibratoValue = spawnWarriorTrio.followerInfo[0].follower_vibrato;
        conversationEntry.followerID = spawnWarriorTrio.followerInfo[0].ID;
      }
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    spawnWarriorTrio.Interactable = false;
    yield return (object) spawnWarriorTrio.StartCoroutine((IEnumerator) spawnWarriorTrio.ConvertFollowers());
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.75f);
    spawnWarriorTrio.state.CURRENT_STATE = StateMachine.State.Idle;
    spawnWarriorTrio.DestroyAllFollowers();
  }

  public void SetFollowersToState(StateMachine.State newState)
  {
    foreach (StateMachine stateMachine in this.stateMachines)
      stateMachine.CURRENT_STATE = newState;
  }

  public void SetFollowersToFacePosition(Vector3 targetPosition)
  {
    foreach (StateMachine stateMachine in this.stateMachines)
      stateMachine.facingAngle = Utils.GetAngle(stateMachine.transform.position, targetPosition);
  }

  public IEnumerator ConvertFollowers()
  {
    Interaction_FollowerSpawnWarriorTrio spawnWarriorTrio = this;
    GameManager.GetInstance().OnConversationNext(spawnWarriorTrio.gameObject, 5f);
    AudioManager.Instance.PlayOneShot(spawnWarriorTrio.followerAscendSFX, spawnWarriorTrio.gameObject);
    spawnWarriorTrio.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnWarriorTrio.StartCoroutine((IEnumerator) spawnWarriorTrio.SetFollowerAnimaions(spawnWarriorTrio.animationConvertShort, playOffset: 0.1f));
    spawnWarriorTrio.PlayAllRecruitParticles();
    spawnWarriorTrio.PlayAllPortalAnimations();
    yield return (object) new WaitForEndOfFrame();
    spawnWarriorTrio.playerFarming = PlayerFarming.players[0];
    float duration = spawnWarriorTrio.playerFarming.Spine.AnimationState.SetAnimation(0, spawnWarriorTrio.animationSpecialActivateLong, false).Animation.Duration;
    spawnWarriorTrio.playerFarming.Spine.AnimationState.AddAnimation(0, spawnWarriorTrio.animationIdle, true, 0.0f);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration - 1f);
    spawnWarriorTrio.CreateNewRecruits(spawnWarriorTrio.followerInfo);
    spawnWarriorTrio.AddRandomThoughtsToFollowers(spawnWarriorTrio.followerInfo);
  }

  public void PlayAllPortalAnimations()
  {
    foreach (SkeletonAnimation portalSpine in this.portalSpines)
    {
      portalSpine.gameObject.SetActive(true);
      portalSpine.AnimationState.SetAnimation(0, this.animationConvertShort, false);
    }
  }

  public void PlayAllRecruitParticles()
  {
    foreach (ParticleSystem recruitParticle in this.recruitParticles)
      recruitParticle.Play();
  }

  public void CreateNewRecruits(FollowerInfo[] followerInfo)
  {
    foreach (FollowerInfo f in followerInfo)
    {
      foreach (FollowerInfo followerInfo1 in followerInfo)
      {
        if (f != followerInfo1)
          f.Siblings.Add(followerInfo1.ID);
      }
      FollowerManager.CreateNewRecruit(f, NotificationCentre.NotificationType.NewRecruit);
    }
  }

  public void AddRandomThoughtsToFollowers(FollowerInfo[] followerInfo)
  {
    foreach (FollowerInfo followerInfo1 in followerInfo)
    {
      float num1 = UnityEngine.Random.value;
      Thought thought = Thought.None;
      if ((double) num1 < 0.699999988079071)
      {
        float num2 = UnityEngine.Random.value;
        if ((double) num2 <= 0.30000001192092896)
          thought = Thought.HappyConvert;
        else if ((double) num2 > 0.30000001192092896 && (double) num2 < 0.60000002384185791)
          thought = Thought.GratefulConvert;
        else if ((double) num2 >= 0.60000002384185791)
          thought = Thought.SkepticalConvert;
      }
      else
        thought = (double) UnityEngine.Random.value > 0.30000001192092896 || DataManager.Instance.Followers.Count <= 0 ? Thought.InstantBelieverConvert : Thought.ResentfulConvert;
      ThoughtData data = FollowerThoughts.GetData(thought);
      data.Init();
      followerInfo1.Thoughts.Add(data);
    }
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sRescue = ScriptLocalization.Interactions.Convert;
  }

  public override void GetLabel()
  {
    if (this.Activated)
      this.Label = "";
    else if (this.Interactable)
    {
      if (this.sRescue == null)
        this.UpdateLocalisation();
      this.Label = this.sRescue;
    }
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.state = state;
    state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.StartCoroutine((IEnumerator) this.PositionPlayer());
    this.Activated = true;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.LoopInstance);
  }

  public new void OnDestroy() => AudioManager.Instance.StopLoop(this.LoopInstance);

  public IEnumerator PositionPlayer()
  {
    Interaction_FollowerSpawnWarriorTrio spawnWarriorTrio = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(spawnWarriorTrio.state.gameObject, 5f);
    yield return (object) new WaitForSeconds(0.25f);
    spawnWarriorTrio.SetFollowerCollidersEnabled(false);
    Vector3 direction = (double) spawnWarriorTrio.state.transform.position.x < (double) spawnWarriorTrio.transform.position.x ? Vector3.left : Vector3.right;
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) spawnWarriorTrio.transform.position, (Vector2) direction, spawnWarriorTrio.playerDistance, (int) spawnWarriorTrio.collisionMask).collider != (UnityEngine.Object) null)
      direction *= -1f;
    Vector3 vector3 = spawnWarriorTrio.transform.position + direction * spawnWarriorTrio.playerDistance;
    spawnWarriorTrio.playerFarming.GoToAndStop(vector3);
    spawnWarriorTrio.SetFollowersToFacePosition(vector3);
    while (spawnWarriorTrio.playerFarming.GoToAndStopping)
      yield return (object) null;
    spawnWarriorTrio.state.facingAngle = Utils.GetAngle(spawnWarriorTrio.state.transform.position, spawnWarriorTrio.transform.position);
    spawnWarriorTrio.StartCoroutine((IEnumerator) spawnWarriorTrio.FollowerChoiceIE());
  }

  public void SetFollowerCollidersEnabled(bool state)
  {
    foreach (GameObject gameObject in this.warriorFollowers.Keys.ToArray<GameObject>())
      gameObject.GetComponent<Collider2D>().enabled = state;
  }

  public void DestroyAllFollowers()
  {
    GameObject[] array = this.warriorFollowers.Keys.ToArray<GameObject>();
    for (int index = array.Length - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) array[index]);
  }

  [CompilerGenerated]
  public void \u003CWaitForFollowerToStopMoving\u003Eb__30_0()
  {
    this.Interactable = true;
    this.GetLabel();
  }

  public class FollowerObjectMetadata
  {
    public string skinName;
    public string displayName;
    public Vector3 relativePosition;

    public FollowerObjectMetadata(string skinName, string displayName, Vector3 relativePosition)
    {
      this.skinName = skinName;
      this.displayName = displayName;
      this.relativePosition = relativePosition;
    }
  }
}
