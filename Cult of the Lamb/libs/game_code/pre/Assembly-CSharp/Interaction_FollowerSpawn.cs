// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerSpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FollowerSpawn : Interaction
{
  public bool DisableOnHighlighted;
  private FollowerInfo v_i;
  private FollowerInfoManager wim;
  private StateMachine RecruitState;
  public SkeletonAnimation Spine;
  [SerializeField]
  private SkeletonAnimation portalSpine;
  public ParticleSystem recruitParticles;
  private string ForceSkin = "";
  public FollowerInfo _followerInfo;
  private FollowerOutfit _outfit;
  private LayerMask collisionMask;
  private string q1Title = "Interactions/FollowerSpawn/Convert/Title";
  private string q1Description = "Interactions/FollowerSpawn/Convert/Description";
  private string q2Title = "Interactions/FollowerSpawn/Consume/Title";
  private string q2Description = "Interactions/FollowerSpawn/Consume/Description";
  private EventInstance receiveLoop;
  public System.Action followerInfoAssigned;
  public Material NormalMaterial;
  public Material BW_Material;
  private EventInstance LoopInstance;
  public AnimationCurve absorbSoulCurve;
  protected string sRescue;
  [HideInInspector]
  public int Cost;
  private bool Activated;

  protected virtual void Start()
  {
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.RecruitState = this.GetComponent<StateMachine>();
  }

  private IEnumerator WaitForFollowerToStopMoving(float Duration)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_FollowerSpawn interactionFollowerSpawn = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().OnConversationEnd();
      interactionFollowerSpawn.Interactable = true;
      interactionFollowerSpawn.GetLabel();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFollowerSpawn.gameObject, 5f);
    interactionFollowerSpawn.GetLabel();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(Duration);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void IndicateHighlighted()
  {
    if (this.DisableOnHighlighted)
      return;
    base.IndicateHighlighted();
  }

  public override void EndIndicateHighlighted() => base.EndIndicateHighlighted();

  public void Play(string ForceSkin = "Boss Mama Maggot", string ForceName = "", bool animate = true)
  {
    this.StopAllCoroutines();
    if (animate)
    {
      this.Interactable = false;
      this.StartCoroutine((IEnumerator) this.WaitForFollowerToStopMoving(this.Spine.AnimationState.SetAnimation(0, "transform", false).Animation.Duration));
    }
    this.Spine.AnimationState.AddAnimation(0, "unconverted", true, 0.0f);
    this.ForceSkin = ForceSkin;
    this.wim = this.GetComponent<FollowerInfoManager>();
    this.wim.ForceSkin = true;
    this.wim.ForceOutfitSkin = true;
    this.wim.ForceSkinOverride = ForceSkin;
    this.wim.ForceOutfitSkinOverride = "Clothes/Rags";
    if ((double) Vector3.Distance(this.transform.position, Vector3.zero) > 4.5)
      this.transform.DOMove(this.transform.position + (Vector3.zero - this.transform.position) * 0.5f, 0.3f);
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(ForceSkin);
    if (colourData != null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[0].SlotAndColours)
        this.wim.SetSlotColour(slotAndColour.Slot, slotAndColour.color);
    }
    this.wim.NewV_I();
    this.UpdateLocalisation();
    this._followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, ForceSkin);
    if (this._followerInfo.SkinName == "Giraffe")
      ForceName = LocalizationManager.GetTranslation("FollowerNames/Sparkles");
    if (ForceName != "")
      this._followerInfo.Name = ForceName;
    this.ActivateDistance = 3f;
    if (ForceName == ScriptLocalization.NAMES.DeathNPC)
    {
      this._followerInfo.ID = 666;
      FollowerBrain.GetOrCreateBrain(this._followerInfo).AddTrait(FollowerTrait.TraitType.Immortal);
    }
    System.Action followerInfoAssigned = this.followerInfoAssigned;
    if (followerInfoAssigned == null)
      return;
    followerInfoAssigned();
  }

  private IEnumerator FollowerChoiceIE()
  {
    Interaction_FollowerSpawn interactionFollowerSpawn = this;
    GameManager.GetInstance().AddPlayerToCamera();
    if (DataManager.Instance.FirstFollowerSpawnInteraction && (bool) (UnityEngine.Object) MiniBossController.Instance)
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(interactionFollowerSpawn.gameObject, "Conversation_NPC/FollowerSpawn/Line1"),
        new ConversationEntry(interactionFollowerSpawn.gameObject, "Conversation_NPC/FollowerSpawn/Line2")
      };
      Entries[0].CharacterName = MiniBossController.Instance.DisplayName;
      Entries[0].Zoom = 5f;
      Entries[0].SetZoom = true;
      Entries[1].CharacterName = MiniBossController.Instance.DisplayName;
      Entries[1].Zoom = 5f;
      Entries[1].SetZoom = true;
      Entries[0].DefaultAnimation = "unconverted";
      Entries[1].DefaultAnimation = "unconverted";
      Entries[0].Animation = "unconverted-talk";
      Entries[0].LoopAnimation = true;
      Entries[1].Animation = "unconverted-talk";
      Entries[1].LoopAnimation = true;
      foreach (ConversationEntry conversationEntry in Entries)
      {
        conversationEntry.soundPath = "event:/dialogue/followers/general_talk";
        conversationEntry.pitchValue = interactionFollowerSpawn._followerInfo.follower_pitch;
        conversationEntry.vibratoValue = interactionFollowerSpawn._followerInfo.follower_vibrato;
      }
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
      while (MMConversation.isPlaying)
        yield return (object) null;
      DataManager.Instance.FirstFollowerSpawnInteraction = false;
    }
    interactionFollowerSpawn.Interactable = false;
    if (true)
      yield return (object) interactionFollowerSpawn.StartCoroutine((IEnumerator) interactionFollowerSpawn.ConvertFollower());
    else
      yield return (object) interactionFollowerSpawn.StartCoroutine((IEnumerator) interactionFollowerSpawn.ConsumefollowerRoutine());
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.75f);
    interactionFollowerSpawn.state.CURRENT_STATE = StateMachine.State.Idle;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionFollowerSpawn.gameObject);
  }

  public IEnumerator ConsumefollowerRoutine(bool move = true)
  {
    Interaction_FollowerSpawn interactionFollowerSpawn = this;
    if ((UnityEngine.Object) interactionFollowerSpawn.state == (UnityEngine.Object) null)
      interactionFollowerSpawn.state = interactionFollowerSpawn.GetComponent<StateMachine>();
    GameManager.GetInstance().OnConversationNext(interactionFollowerSpawn.gameObject, 5f);
    if (move)
    {
      Vector3 vector3 = (double) interactionFollowerSpawn.state.transform.position.x < (double) interactionFollowerSpawn.transform.position.x ? Vector3.left : Vector3.right;
      Vector3 TargetPosition = interactionFollowerSpawn.transform.position + vector3 * 2f;
      PlayerFarming.Instance.GoToAndStop(TargetPosition);
      while (PlayerFarming.Instance.GoToAndStopping)
        yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/followers/consume_start", interactionFollowerSpawn.gameObject);
    interactionFollowerSpawn.state.facingAngle = Utils.GetAngle(interactionFollowerSpawn.state.transform.position, interactionFollowerSpawn.transform.position);
    interactionFollowerSpawn.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sacrifice-long", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    interactionFollowerSpawn.Spine.AnimationState.SetAnimation(0, "sacrifice-long", false);
    yield return (object) new WaitForSeconds(0.1f);
    interactionFollowerSpawn.Spine.CustomMaterialOverride.Clear();
    interactionFollowerSpawn.Spine.CustomMaterialOverride.Add(interactionFollowerSpawn.NormalMaterial, interactionFollowerSpawn.BW_Material);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial, PlayerFarming.Instance.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().CamFollowTarget.targetDistance += 2f;
    interactionFollowerSpawn.LoopInstance = AudioManager.Instance.CreateLoop("event:/followers/consume_loop", interactionFollowerSpawn.gameObject, true);
    yield return (object) new WaitForSeconds(3f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.StopLoop(interactionFollowerSpawn.LoopInstance);
    AudioManager.Instance.PlayOneShot("event:/followers/consume_end", interactionFollowerSpawn.gameObject);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    interactionFollowerSpawn.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    PlayerFarming.Instance.GetXP(10f);
  }

  private IEnumerator SpawnSouls(
    Vector3 fromPosition,
    Vector3 targetPosition,
    float startingDelay,
    float min)
  {
    float delay = startingDelay;
    for (int i = 0; i < 30; ++i)
    {
      float time = (float) i / 30f;
      delay = Mathf.Clamp(delay * (1f - this.absorbSoulCurve.Evaluate(time)), min, float.MaxValue);
      SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, fromPosition, Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) this.absorbSoulCurve.Evaluate(time))));
      yield return (object) new WaitForSeconds(delay);
    }
  }

  public IEnumerator ConvertFollower()
  {
    Interaction_FollowerSpawn interactionFollowerSpawn = this;
    if ((UnityEngine.Object) interactionFollowerSpawn.state == (UnityEngine.Object) null)
      interactionFollowerSpawn.state = interactionFollowerSpawn.GetComponent<StateMachine>();
    GameManager.GetInstance().OnConversationNext(interactionFollowerSpawn.gameObject, 5f);
    AudioManager.Instance.PlayOneShot("event:/followers/ascend", interactionFollowerSpawn.gameObject);
    interactionFollowerSpawn.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionFollowerSpawn.Spine.AnimationState.SetAnimation(0, "convert-short", false);
    if ((bool) (UnityEngine.Object) interactionFollowerSpawn.recruitParticles)
      interactionFollowerSpawn.recruitParticles.Play();
    interactionFollowerSpawn.portalSpine.gameObject.SetActive(true);
    interactionFollowerSpawn.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
    yield return (object) new WaitForEndOfFrame();
    float duration = PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "specials/special-activate-long", false).Animation.Duration;
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration - 1f);
    FollowerManager.CreateNewRecruit(interactionFollowerSpawn._followerInfo, NotificationCentre.NotificationType.NewRecruit);
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
    interactionFollowerSpawn._followerInfo.Thoughts.Add(data);
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

  private new void OnDestroy() => AudioManager.Instance.StopLoop(this.LoopInstance);

  private IEnumerator PositionPlayer()
  {
    Interaction_FollowerSpawn interactionFollowerSpawn = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFollowerSpawn.state.gameObject, 5f);
    yield return (object) new WaitForSeconds(0.25f);
    interactionFollowerSpawn.GetComponent<Collider2D>().enabled = false;
    Vector3 direction = (double) interactionFollowerSpawn.state.transform.position.x < (double) interactionFollowerSpawn.transform.position.x ? Vector3.left : Vector3.right;
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) interactionFollowerSpawn.transform.position, (Vector2) direction, 1.5f, (int) interactionFollowerSpawn.collisionMask).collider != (UnityEngine.Object) null)
      direction *= -1f;
    Vector3 TargetPosition = interactionFollowerSpawn.transform.position + direction * 1.5f;
    PlayerFarming.Instance.GoToAndStop(TargetPosition);
    interactionFollowerSpawn.RecruitState.facingAngle = Utils.GetAngle(interactionFollowerSpawn.RecruitState.transform.position, PlayerFarming.Instance.transform.position);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    interactionFollowerSpawn.state.facingAngle = Utils.GetAngle(interactionFollowerSpawn.state.transform.position, interactionFollowerSpawn.transform.position);
    interactionFollowerSpawn.StartCoroutine((IEnumerator) interactionFollowerSpawn.FollowerChoiceIE());
  }
}
