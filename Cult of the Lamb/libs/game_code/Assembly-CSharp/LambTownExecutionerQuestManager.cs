// Decompiled with JetBrains decompiler
// Type: LambTownExecutionerQuestManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class LambTownExecutionerQuestManager : MonoBehaviour
{
  [SerializeField]
  public Transform playerPosition;
  [SerializeField]
  public SkeletonAnimation portalSpine;
  [SerializeField]
  public ParticleSystem recruitParticles;
  [SerializeField]
  public GameObject executioner;
  [SerializeField]
  public SkeletonAnimation executionerSpine;
  [SerializeField]
  public Transform executionerPosition;
  [SerializeField]
  public List<ConversationEntry> executionerEntries;
  [Header("Intro")]
  [SerializeField]
  public Interaction_SimpleConversation introConversation;
  [SerializeField]
  public Transform executionerIntroPosition;
  [Header("Animations")]
  [SpineAnimation("", "", true, false, dataField = "executionerSpine")]
  [SerializeField]
  public string dieLoopAnimation;
  [SpineAnimation("", "", true, false, dataField = "executionerSpine")]
  [SerializeField]
  public string executeAnimation;
  [SerializeField]
  public Transform graveyardNPCPosition;
  [SerializeField]
  public SkeletonAnimation graveyardNPC;
  [SerializeField]
  public List<LambTownExecutionerQuestManager.GhostSpineEntry> ghostSpines;
  [SerializeField]
  public List<SkeletonAnimation> ghostBackgroundSpines;
  [SerializeField]
  public float ghostsPositionRadius = 4f;
  [SerializeField]
  [Range(0.0f, 360f)]
  public float positionAngleOffset;
  [SerializeField]
  public List<ConversationEntry> graveyardNPCEntries;
  [SerializeField]
  public List<SimpleBark> executionerInWoolhavenReactionBarks;
  [SerializeField]
  public List<ConversationEntry> killReactionEntries;
  [SerializeField]
  public List<ConversationEntry> killExecutionerReactionEntries;
  [SerializeField]
  public List<ConversationEntry> indoctraintedExecutionerReactionEntries;
  [SerializeField]
  public List<ConversationEntry> indoctraintedReactionEntries;
  public string outroPrayingLoopSFX = "event:/dlc/dialogue/executioner/baseform_praying_loop";
  public string outroExecuteSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_outro_execute";
  public string poofsIntoFollowerSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/woolhaven_poof_into_follower";
  public string lambGetsBlueprintSFX = "event:/Stings/Choir_mid";
  public string outroCorpseAppearsSFX = "event:/dlc/dungeon06/enemy/miniboss_executioner/story_outro_execute_corpse_death";
  public EventInstance prayingLoopInstanceSFX;

  public static bool CanPlayExecutionerIndoctrinationSequence
  {
    get
    {
      if (!DataManager.Instance.ExecutionerPardoned || !DataManager.Instance.NPCGhostGraveyardRescued || DataManager.Instance.ExecutionerWoolhavenExecuted || DataManager.Instance.ExecutionerWoolhavenSaved || TimeManager.CurrentDay - DataManager.Instance.ExecutionerInWoolhavenDay < 3 || DataManager.Instance.ExecutionerInWoolhavenDay == -1 || !DataManager.Instance.OnboardedLegendaryWeapons)
        return false;
      return !DataManager.Instance.BeatenYngya || DataManager.Instance.FinalDLCMap;
    }
  }

  public void OnEnable()
  {
    this.executionerSpine.gameObject.SetActive(false);
    this.executioner.SetActive(false);
    this.SetReactionBarksActive(false);
    this.introConversation.gameObject.SetActive(false);
    if (!DataManager.Instance.ExecutionerPardoned || !DataManager.Instance.NPCGhostGraveyardRescued || DataManager.Instance.ExecutionerWoolhavenExecuted || DataManager.Instance.ExecutionerWoolhavenSaved)
      return;
    if (LambTownExecutionerQuestManager.CanPlayExecutionerIndoctrinationSequence)
    {
      this.executionerSpine.gameObject.SetActive(true);
      this.StartCoroutine((IEnumerator) this.WaitForTransitionAndPlay());
    }
    else
    {
      if (TimeManager.CurrentDay - DataManager.Instance.ExecutionerPardonedDay < 3)
        return;
      if (DataManager.Instance.ExecutionerInWoolhavenDay == -1)
      {
        this.executioner.transform.position = this.executionerIntroPosition.position;
        ExecutionerNPC npc = this.executioner.GetComponent<ExecutionerNPC>();
        npc.enabled = false;
        this.introConversation.gameObject.SetActive(true);
        this.introConversation.Callback.AddListener((UnityAction) (() =>
        {
          npc.enabled = true;
          DataManager.Instance.ExecutionerInWoolhavenDay = TimeManager.CurrentDay;
        }));
      }
      this.MakeAllGhostGrumpy();
      this.executioner.SetActive(true);
      this.SetReactionBarksActive(true);
    }
  }

  public void OnDisable() => this.introConversation.Callback.RemoveAllListeners();

  public void MakeAllGhostGrumpy()
  {
    foreach (GhostNPC ghostNpC in GhostNPC.GhostNPCs)
      ghostNpC.SetGrumpyEmotion();
  }

  public IEnumerator WaitForTransitionAndPlay()
  {
    while (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Idle || (UnityEngine.Object) LetterBox.Instance != (UnityEngine.Object) null && LetterBox.IsPlaying)
      yield return (object) null;
    this.SetExecutionInteraction();
  }

  public void SetExecutionInteraction()
  {
    SimulationManager.Pause();
    this.RemoveInactiveGhostsConvos();
    this.SetGhostsActive(true);
    this.executionerSpine.AnimationState.SetAnimation(0, this.dieLoopAnimation, true);
    this.prayingLoopInstanceSFX = AudioManager.Instance.CreateLoop(this.outroPrayingLoopSFX, this.executionerSpine.gameObject, true);
    List<MMTools.Response> Responses = new List<MMTools.Response>();
    string str1 = "Conversation_NPC/GraveyardNPC/Executioner/Choice_Indoctrinate";
    Responses.Add(new MMTools.Response(str1, (System.Action) (() => this.StartCoroutine((IEnumerator) this.Indoctrinate())), str1));
    string str2 = "Conversation_NPC/GraveyardNPC/Executioner/Choice_Kill";
    Responses.Add(new MMTools.Response(str2, (System.Action) (() => this.StartCoroutine((IEnumerator) this.Kill())), str2));
    ConversationObject conversationObject = new ConversationObject(this.graveyardNPCEntries, Responses, (System.Action) null);
    HUD_Manager.Instance.Hide(false);
    PlayerFarming.Instance.GoToAndStop(this.playerPosition.position, GoToCallback: (System.Action) (() => MMConversation.Play(conversationObject, SnapLetterBox: true)), groupAction: true);
  }

  public IEnumerator Kill()
  {
    LambTownExecutionerQuestManager executionerQuestManager = this;
    AudioManager.Instance.StopLoop(executionerQuestManager.prayingLoopInstanceSFX, STOP_MODE.IMMEDIATE);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(executionerQuestManager.executionerPosition.gameObject);
    yield return (object) new WaitForSeconds(0.3f);
    bool playingConvo = true;
    MMConversation.Play(new ConversationObject(executionerQuestManager.killExecutionerReactionEntries, (List<MMTools.Response>) null, (System.Action) (() => playingConvo = false)), SetPlayerIdleOnComplete: false, SnapLetterBox: true);
    while (playingConvo)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(executionerQuestManager.executionerPosition.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    executionerQuestManager.executionerSpine.AnimationState.SetAnimation(0, executionerQuestManager.executeAnimation, false);
    AudioManager.Instance.PlayOneShot(executionerQuestManager.outroExecuteSFX);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "break-axe", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(6.5f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    DataManager.Instance.ExecutionerWoolhavenExecuted = true;
    if (!DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Axe_Legendary) && DataManager.Instance.OnboardedLegendaryWeapons)
      yield return (object) executionerQuestManager.StartCoroutine((IEnumerator) executionerQuestManager.PlayerPickUpWeapon());
    yield return (object) new WaitForSeconds(0.3f);
    if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER))
    {
      yield return (object) executionerQuestManager.StartCoroutine((IEnumerator) executionerQuestManager.PlayerPickUpThrophyDeco(executionerQuestManager.executionerSpine.transform.position));
      yield return (object) new WaitForSeconds(0.5f);
    }
    MMConversation.Play(new ConversationObject(executionerQuestManager.killReactionEntries, (List<MMTools.Response>) null, (System.Action) (() => this.EndExecution())), SnapLetterBox: true);
  }

  public IEnumerator Indoctrinate()
  {
    LambTownExecutionerQuestManager executionerQuestManager = this;
    AudioManager.Instance.StopLoop(executionerQuestManager.prayingLoopInstanceSFX, STOP_MODE.IMMEDIATE);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(executionerQuestManager.executionerPosition.gameObject);
    yield return (object) new WaitForSeconds(2f);
    FollowerInfo executionerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, "Executioner");
    executionerInfo.Name = ScriptLocalization.NAMES.RevealedExecutioner;
    executionerInfo.ID = 10010;
    executionerInfo.Traits.Add(FollowerTrait.TraitType.Blind);
    if (BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count == 0)
      BiomeBaseManager.Instance.SpawnExistingRecruits = false;
    DataManager.Instance.Followers_Recruit.Add(executionerInfo);
    DataManager.SetFollowerSkinUnlocked("Executioner");
    executionerQuestManager.executionerSpine.gameObject.SetActive(false);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(executionerQuestManager.executionerPosition.position);
    AudioManager.Instance.PlayOneShot(executionerQuestManager.poofsIntoFollowerSFX);
    FollowerManager.SpawnedFollower follower = FollowerManager.SpawnCopyFollower(executionerInfo, executionerQuestManager.executionerPosition.position, executionerQuestManager.transform.parent, FollowerLocation.Base);
    double num1 = (double) follower.Follower.SetBodyAnimation("unconverted", true);
    follower.Follower.Interaction_FollowerInteraction.eventListener.SetPitchAndVibrator(1f, 1f, -1);
    GameManager.GetInstance().OnConversationNext(follower.Follower.gameObject, 6f);
    Vector3 TargetPosition = follower.Follower.transform.position + Vector3.right * 1.5f;
    PlayerFarming.Instance.GoToAndStop(TargetPosition, follower.Follower.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    for (int index = 0; index < executionerQuestManager.indoctraintedExecutionerReactionEntries.Count; ++index)
      executionerQuestManager.indoctraintedExecutionerReactionEntries[index].Speaker = follower.Follower.gameObject;
    bool playingConvo = true;
    MMConversation.Play(new ConversationObject(executionerQuestManager.indoctraintedExecutionerReactionEntries, (List<MMTools.Response>) null, (System.Action) (() => playingConvo = false)), SetPlayerIdleOnComplete: false, SnapLetterBox: true);
    while (playingConvo)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.Follower.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    if (!StructuresData.GetUnlocked(StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER))
    {
      yield return (object) executionerQuestManager.StartCoroutine((IEnumerator) executionerQuestManager.PlayerPickUpThrophyDeco(follower.Follower.transform.position));
      yield return (object) new WaitForSeconds(0.5f);
    }
    GameManager.GetInstance().OnConversationNext(follower.Follower.gameObject);
    follower.Follower.Spine.GetComponent<SimpleSpineAnimator>().enabled = false;
    AudioManager.Instance.PlayOneShot("event:/followers/ascend", executionerQuestManager.gameObject);
    yield return (object) new WaitForEndOfFrame();
    follower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    double num2 = (double) follower.Follower.SetBodyAnimation("convert-short", false);
    executionerQuestManager.recruitParticles.Play();
    executionerQuestManager.portalSpine.gameObject.SetActive(true);
    executionerQuestManager.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration = PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "specials/special-activate-long", false).Animation.Duration;
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(duration - 1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    float num3 = UnityEngine.Random.value;
    Thought thought = Thought.None;
    if ((double) num3 < 0.699999988079071)
    {
      float num4 = UnityEngine.Random.value;
      if ((double) num4 <= 0.30000001192092896)
        thought = Thought.HappyConvert;
      else if ((double) num4 > 0.30000001192092896 && (double) num4 < 0.60000002384185791)
        thought = Thought.GratefulConvert;
      else if ((double) num4 >= 0.60000002384185791)
        thought = Thought.SkepticalConvert;
    }
    else
      thought = (double) UnityEngine.Random.value > 0.30000001192092896 || DataManager.Instance.Followers.Count <= 0 ? Thought.InstantBelieverConvert : Thought.ResentfulConvert;
    ThoughtData data = FollowerThoughts.GetData(thought);
    data.Init();
    executionerInfo.Thoughts.Add(data);
    FollowerManager.CleanUpCopyFollower(follower);
    DataManager.Instance.ExecutionerWoolhavenSaved = true;
    MMConversation.Play(new ConversationObject(executionerQuestManager.indoctraintedReactionEntries, (List<MMTools.Response>) null, (System.Action) (() => this.EndExecution())), SnapLetterBox: true);
  }

  public void EndExecution() => this.StartCoroutine((IEnumerator) this.EndExecutionSequence());

  public IEnumerator EndExecutionSequence()
  {
    LambTownExecutionerQuestManager executionerQuestManager = this;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    yield return (object) executionerQuestManager.StartCoroutine((IEnumerator) LambTownExecutionerQuestManager.FadeIn());
    executionerQuestManager.executionerSpine.gameObject.SetActive(false);
    executionerQuestManager.SetGhostsActive(false);
    yield return (object) executionerQuestManager.StartCoroutine((IEnumerator) LambTownExecutionerQuestManager.FadeOut());
    PlayerFarming.SetStateForAllPlayers();
    SimulationManager.UnPause();
  }

  public void SetGhostPositions()
  {
    List<SkeletonAnimation> activeGhostSpines = this.GetActiveGhostSpines();
    for (int index = activeGhostSpines.Count - 1; index >= 0; ++index)
    {
      if (this.ghostBackgroundSpines.Contains(activeGhostSpines[index]))
        activeGhostSpines.Remove(activeGhostSpines[index]);
    }
    for (int index = 0; index < activeGhostSpines.Count; ++index)
      activeGhostSpines[index].transform.position = this.GetCirclePosition(index, activeGhostSpines.Count);
    this.graveyardNPC.transform.position = this.graveyardNPCPosition.position;
  }

  public Vector3 GetCirclePosition(int index, int ghostCount)
  {
    float f = (float) (((double) index * (360.0 / (double) ghostCount) + (double) this.positionAngleOffset) * (Math.PI / 180.0));
    return this.executionerPosition.position + new Vector3(this.ghostsPositionRadius * Mathf.Cos(f), this.ghostsPositionRadius * Mathf.Sin(f));
  }

  public void SetGhostsActive(bool active)
  {
    foreach (Component activeGhostSpine in this.GetActiveGhostSpines())
      activeGhostSpine.gameObject.SetActive(active);
    this.graveyardNPC.gameObject.SetActive(active);
    foreach (Component ghostNpC in GhostNPC.GhostNPCs)
      ghostNpC.gameObject.SetActive(!active);
  }

  public List<SkeletonAnimation> GetActiveGhostSpines()
  {
    List<SkeletonAnimation> activeGhostSpines = new List<SkeletonAnimation>();
    foreach (LambTownExecutionerQuestManager.GhostSpineEntry ghostSpine in this.ghostSpines)
    {
      if (DataManager.Instance.GetVariable(ghostSpine.ActiveVariable))
        activeGhostSpines.Add(ghostSpine.Spine);
    }
    return activeGhostSpines;
  }

  public void SetReactionBarksActive(bool active)
  {
    for (int index = 0; index < this.executionerInWoolhavenReactionBarks.Count; ++index)
      this.executionerInWoolhavenReactionBarks[index].gameObject.SetActive(active);
  }

  public IEnumerator PlayerPickUpWeapon()
  {
    LambTownExecutionerQuestManager executionerQuestManager = this;
    PickUp pickup = (PickUp) null;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1, executionerQuestManager.executionerPosition.position, result: (Action<PickUp>) (p =>
    {
      pickup = p;
      pickup.GetComponent<Interaction_BrokenWeapon>().SetWeapon(EquipmentType.Axe_Legendary);
    }));
    while ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
      yield return (object) null;
    pickup.enabled = false;
    pickup.child.transform.localScale = Vector3.one;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", executionerQuestManager.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 itemTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    bool isMoving = true;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = pickup.transform.DOMove(itemTargetPosition, 1.5f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() => isMoving = false);
    while (isMoving)
      yield return (object) null;
    pickup.transform.position = itemTargetPosition;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    yield return (object) new WaitForSeconds(1.5f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    Inventory.AddItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1);
    DataManager.Instance.AddLegendaryWeaponToUnlockQueue(EquipmentType.Axe_Legendary);
    pickup.GetComponent<Interaction_BrokenWeapon>().StartBringWeaponToBlacksmithObjective();
    UnityEngine.Object.Destroy((UnityEngine.Object) pickup.gameObject);
  }

  public void RemoveInactiveGhostsConvos()
  {
    List<SkeletonAnimation> activeGhostSpines = this.GetActiveGhostSpines();
    activeGhostSpines.Add(this.graveyardNPC);
    for (int index = this.killReactionEntries.Count - 1; index >= 0; --index)
    {
      if (!activeGhostSpines.Contains(this.killReactionEntries[index].SkeletonData))
        this.killReactionEntries.RemoveAt(index);
    }
    for (int index = this.indoctraintedReactionEntries.Count - 1; index >= 0; --index)
    {
      if (!activeGhostSpines.Contains(this.indoctraintedReactionEntries[index].SkeletonData))
        this.indoctraintedReactionEntries.RemoveAt(index);
    }
  }

  public IEnumerator PlayerPickUpThrophyDeco(Vector3 itemSpawnPosition)
  {
    LambTownExecutionerQuestManager executionerQuestManager = this;
    AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", executionerQuestManager.transform.position);
    FoundItemPickUp deco = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, itemSpawnPosition).GetComponent<FoundItemPickUp>();
    deco.DecorationType = StructureBrain.TYPES.DECORATION_BOSS_TROPHY_DLC_EXECUTIONER;
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 itemTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    yield return (object) new WaitForSeconds(0.5f);
    deco.GetComponent<PickUp>().enabled = false;
    bool wait = true;
    deco.transform.DOMove(itemTargetPosition, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => wait = false));
    while (wait)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot(executionerQuestManager.lambGetsBlueprintSFX);
    deco.transform.position = itemTargetPosition;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    UnityEngine.Object.Destroy((UnityEngine.Object) deco.gameObject);
    wait = true;
    StructuresData.CompleteResearch(deco.DecorationType);
    StructuresData.SetRevealed(deco.DecorationType);
    UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
    buildMenuController.Show(deco.DecorationType);
    UIBuildMenuController buildMenuController1 = buildMenuController;
    buildMenuController1.OnHidden = buildMenuController1.OnHidden + (System.Action) (() =>
    {
      wait = false;
      buildMenuController = (UIBuildMenuController) null;
    });
    while (wait)
      yield return (object) null;
  }

  public static IEnumerator FadeIn()
  {
    bool waitingForFade = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  public static IEnumerator FadeOut()
  {
    bool waitingForFade = true;
    MMTransition.ResumePlay((System.Action) (() => waitingForFade = false));
    while (waitingForFade)
      yield return (object) null;
  }

  [Serializable]
  public struct GhostSpineEntry
  {
    public SkeletonAnimation Spine;
    public DataManager.Variables ActiveVariable;
  }
}
