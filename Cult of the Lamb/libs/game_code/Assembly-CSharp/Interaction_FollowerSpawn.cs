// Decompiled with JetBrains decompiler
// Type: Interaction_FollowerSpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_FollowerSpawn : Interaction
{
  public bool DisableOnHighlighted;
  public FollowerInfo v_i;
  public FollowerInfoManager wim;
  public StateMachine RecruitState;
  public SkeletonAnimation Spine;
  [SerializeField]
  public SkeletonAnimation portalSpine;
  public ParticleSystem recruitParticles;
  public string ForceSkin = "";
  public FollowerInfo _followerInfo;
  public FollowerOutfit _outfit;
  public LayerMask collisionMask;
  public string q1Title = "Interactions/FollowerSpawn/Convert/Title";
  public string q1Description = "Interactions/FollowerSpawn/Convert/Description";
  public string q2Title = "Interactions/FollowerSpawn/Consume/Title";
  public string q2Description = "Interactions/FollowerSpawn/Consume/Description";
  public Thought cursedState;
  public EventInstance receiveLoop;
  public System.Action followerInfoAssigned;
  public System.Action OnFollowerDisabled;
  public Material NormalMaterial;
  public Material BW_Material;
  public EventInstance LoopInstance;
  public AnimationCurve absorbSoulCurve;
  public string sRescue;
  [HideInInspector]
  public int Cost;
  public bool Activated;

  public SkeletonAnimation PortalSpine => this.portalSpine;

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
    this.RecruitState = this.GetComponent<StateMachine>();
  }

  public IEnumerator WaitForFollowerToStopMoving(float Duration)
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
      interactionFollowerSpawn.OnFollowerDisabled = (System.Action) null;
      interactionFollowerSpawn.Interactable = true;
      interactionFollowerSpawn.GetLabel();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionFollowerSpawn.OnFollowerDisabled += new System.Action(interactionFollowerSpawn.\u003CWaitForFollowerToStopMoving\u003Eb__23_0);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFollowerSpawn.gameObject, 5f);
    interactionFollowerSpawn.GetLabel();
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

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
    base.EndIndicateHighlighted(playerFarming);
  }

  public void Play(
    string ForceSkin = "Boss Mama Maggot",
    string ForceName = "",
    bool animate = true,
    Thought cursedState = Thought.None,
    bool moveToZero = true)
  {
    this.StopAllCoroutines();
    this.cursedState = cursedState;
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
    if ((double) Vector3.Distance(this.transform.position, Vector3.zero) > 4.5 & moveToZero)
      this.transform.DOMove(this.transform.position + (Vector3.zero - this.transform.position) * 0.5f, 0.3f);
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(ForceSkin);
    if (colourData != null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[0].SlotAndColours)
        this.wim.SetSlotColour(slotAndColour.Slot, slotAndColour.color);
    }
    this.wim.NewV_I();
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_6 && !ForceSkin.ToLower().Contains("beholder"))
    {
      this.wim.v_i.Traits.Add(FollowerTrait.TraitType.Mutated);
      DataManager.Instance.RecruitedRotFollower = true;
      this.wim.SetOutfit();
    }
    this.UpdateLocalisation();
    this._followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, ForceSkin);
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_6 && !ForceSkin.ToLower().Contains("beholder"))
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.Mutated);
    if (this._followerInfo.SkinName == "Giraffe")
      ForceName = LocalizationManager.GetTranslation("FollowerNames/Sparkles");
    if (this._followerInfo.SkinName == "Poppy")
      ForceName = LocalizationManager.GetTranslation("FollowerNames/Poppy");
    if (this._followerInfo.SkinName == "Pudding")
      ForceName = LocalizationManager.GetTranslation("FollowerNames/Pudding");
    if (ForceName != "")
      this._followerInfo.Name = ForceName;
    this.ActivateDistance = 3f;
    if (ForceName == ScriptLocalization.NAMES.DeathNPC)
    {
      this._followerInfo.ID = 666;
      FollowerBrain.GetOrCreateBrain(this._followerInfo).AddTrait(FollowerTrait.TraitType.Immortal);
    }
    else if (ForceName == ScriptLocalization.NAMES_CultLeaders.Dungeon1)
    {
      this._followerInfo.ID = 99990;
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.Blind);
    }
    else if (ForceName == ScriptLocalization.NAMES_CultLeaders.Dungeon2)
      this._followerInfo.ID = 99991;
    else if (ForceName == ScriptLocalization.NAMES_CultLeaders.Dungeon3)
      this._followerInfo.ID = 99992;
    else if (ForceName == ScriptLocalization.NAMES_CultLeaders.Dungeon4)
      this._followerInfo.ID = 99993;
    else if (ForceName == ScriptLocalization.NAMES.Midas)
      this._followerInfo.ID = 100006;
    else if (this._followerInfo.SkinName == "Yngya")
    {
      this._followerInfo.ID = 100007;
      this._followerInfo.Traits.Add(FollowerTrait.TraitType.FreezeImmune);
    }
    else if (ForceName == "River Boy")
      this._followerInfo.ID = 10009;
    else if (this._followerInfo.SkinName == "Executioner")
      this._followerInfo.ID = 10010;
    else if (this._followerInfo.SkinName == "Boss Dog 1")
      this._followerInfo.ID = 10011;
    else if (this._followerInfo.SkinName == "Boss Dog 2")
      this._followerInfo.ID = 10012;
    else if (this._followerInfo.SkinName == "Boss Dog 3")
      this._followerInfo.ID = 10013;
    else if (this._followerInfo.SkinName == "Boss Dog 4")
      this._followerInfo.ID = 10016;
    else if (this._followerInfo.SkinName == "Boss Dog 5")
      this._followerInfo.ID = 10015;
    else if (this._followerInfo.SkinName == "Boss Dog 6")
      this._followerInfo.ID = 10014;
    if (FollowerManager.UniqueFollowerIDs.Contains(this._followerInfo.ID))
      this._followerInfo.LifeExpectancy *= 2;
    System.Action followerInfoAssigned = this.followerInfoAssigned;
    if (followerInfoAssigned == null)
      return;
    followerInfoAssigned();
  }

  public IEnumerator FollowerChoiceIE()
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
      Entries[0].CharacterName = !MiniBossController.Instance.multipleBoss ? MiniBossController.Instance.DisplayName : (MiniBossController.Instance as DoubleMinibossController).lastMinibossName;
      Entries[0].Zoom = 5f;
      Entries[0].SetZoom = true;
      Entries[1].CharacterName = !MiniBossController.Instance.multipleBoss ? MiniBossController.Instance.DisplayName : (MiniBossController.Instance as DoubleMinibossController).lastMinibossName;
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
        conversationEntry.followerID = interactionFollowerSpawn._followerInfo.ID;
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
    if (move && PlayerFarming.Location != FollowerLocation.HubShore)
    {
      Vector3 vector3 = (double) interactionFollowerSpawn.state.transform.position.x < (double) interactionFollowerSpawn.transform.position.x ? Vector3.left : Vector3.right;
      Vector3 TargetPosition = interactionFollowerSpawn.transform.position + vector3 * 2f;
      interactionFollowerSpawn.playerFarming.GoToAndStop(TargetPosition);
      while (interactionFollowerSpawn.playerFarming.GoToAndStopping)
        yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/followers/consume_start", interactionFollowerSpawn.gameObject);
    interactionFollowerSpawn.state.facingAngle = Utils.GetAngle(interactionFollowerSpawn.state.transform.position, interactionFollowerSpawn.transform.position);
    interactionFollowerSpawn.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionFollowerSpawn.playerFarming.simpleSpineAnimator.Animate("sacrifice-long", 0, false);
    interactionFollowerSpawn.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    interactionFollowerSpawn.Spine.AnimationState.SetAnimation(0, "sacrifice-long", false);
    yield return (object) new WaitForSeconds(0.1f);
    interactionFollowerSpawn.Spine.CustomMaterialOverride.Clear();
    interactionFollowerSpawn.Spine.CustomMaterialOverride.Add(interactionFollowerSpawn.NormalMaterial, interactionFollowerSpawn.BW_Material);
    interactionFollowerSpawn.playerFarming.Spine.CustomMaterialOverride.Clear();
    interactionFollowerSpawn.playerFarming.Spine.CustomMaterialOverride.Add(interactionFollowerSpawn.playerFarming.originalMaterial, interactionFollowerSpawn.playerFarming.BW_Material);
    HUD_Manager.Instance.ShowBW(0.33f, 0.0f, 1f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().CamFollowTarget.targetDistance += 2f;
    interactionFollowerSpawn.LoopInstance = AudioManager.Instance.CreateLoop("event:/followers/consume_loop", interactionFollowerSpawn.gameObject, true);
    yield return (object) new WaitForSeconds(3f);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.StopLoop(interactionFollowerSpawn.LoopInstance);
    AudioManager.Instance.PlayOneShot("event:/followers/consume_end", interactionFollowerSpawn.gameObject);
    interactionFollowerSpawn.playerFarming.Spine.CustomMaterialOverride.Clear();
    interactionFollowerSpawn.Spine.CustomMaterialOverride.Clear();
    HUD_Manager.Instance.ShowBW(0.33f, 1f, 0.0f);
    interactionFollowerSpawn.playerFarming.GetXP(10f);
  }

  public IEnumerator SpawnSouls(
    Vector3 fromPosition,
    Vector3 targetPosition,
    float startingDelay,
    float min)
  {
    Interaction_FollowerSpawn interactionFollowerSpawn = this;
    float delay = startingDelay;
    for (int i = 0; i < 30; ++i)
    {
      float time = (float) i / 30f;
      delay = Mathf.Clamp(delay * (1f - interactionFollowerSpawn.absorbSoulCurve.Evaluate(time)), min, float.MaxValue);
      SoulCustomTarget.Create(interactionFollowerSpawn.playerFarming.gameObject, fromPosition, Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) interactionFollowerSpawn.absorbSoulCurve.Evaluate(time))));
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
    interactionFollowerSpawn.playerFarming = PlayerFarming.players[0];
    float duration = interactionFollowerSpawn.playerFarming.Spine.AnimationState.SetAnimation(0, "specials/special-activate-long", false).Animation.Duration;
    interactionFollowerSpawn.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration - 1f);
    interactionFollowerSpawn._followerInfo.StartingCursedState = interactionFollowerSpawn.cursedState;
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

  public new void OnDestroy() => AudioManager.Instance.StopLoop(this.LoopInstance);

  public IEnumerator PositionPlayer()
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
    if (PlayerFarming.Location != FollowerLocation.HubShore)
      interactionFollowerSpawn.playerFarming.GoToAndStop(TargetPosition);
    interactionFollowerSpawn.RecruitState.facingAngle = Utils.GetAngle(interactionFollowerSpawn.RecruitState.transform.position, interactionFollowerSpawn.playerFarming.transform.position);
    while (interactionFollowerSpawn.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionFollowerSpawn.state.facingAngle = Utils.GetAngle(interactionFollowerSpawn.state.transform.position, interactionFollowerSpawn.transform.position);
    interactionFollowerSpawn.StartCoroutine((IEnumerator) interactionFollowerSpawn.FollowerChoiceIE());
  }

  [CompilerGenerated]
  public void \u003CWaitForFollowerToStopMoving\u003Eb__23_0()
  {
    this.Interactable = true;
    this.GetLabel();
  }
}
