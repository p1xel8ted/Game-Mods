// Decompiled with JetBrains decompiler
// Type: MidasKilled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
public class MidasKilled : Interaction
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public EnemyWolfGuardian_Axe enemy;
  [SerializeField]
  public PickUp midasSkull;
  [SerializeField]
  public SkeletonAnimation portalSpine;
  public EventInstance receiveLoop;

  public override void OnEnable() => base.OnEnable();

  public override void OnDisable() => base.OnDisable();

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.KillMidasIE());
  }

  public IEnumerator KillMidasIE()
  {
    MidasKilled midasKilled = this;
    RoomLockController.CloseAll();
    PlayerReturnToBase.Disabled = true;
    GameManager.SetGlobalOcclusionActive(false);
    while (PlayerFarming.AnyPlayerGotoAndStopping())
      yield return (object) null;
    FMODLoopSound loopSound = midasKilled.GetComponent<FMODLoopSound>();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(midasKilled.enemy.gameObject, "Conversation_NPC/MidasKilled/Enemy/0"),
      new ConversationEntry(midasKilled.spine.gameObject, "Conversation_NPC/MidasKilled/Midas/0"),
      new ConversationEntry(midasKilled.enemy.gameObject, "Conversation_NPC/MidasKilled/Enemy/1")
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false);
    AudioManager.Instance.SetMusicParam(SoundParams.RoomID, 1f);
    yield return (object) null;
    bool loopPlaying = true;
    while (MMConversation.isPlaying)
    {
      if (MMConversation.Position != -1 && MMConversation.CURRENT_CONVERSATION.Entries[MMConversation.Position].CharacterName == ScriptLocalization.NAMES.Midas)
      {
        loopSound.StopLoop();
        loopPlaying = false;
      }
      else if (!loopPlaying)
      {
        loopPlaying = true;
        loopSound.PlayLoop();
      }
      yield return (object) null;
    }
    midasKilled.enemy.Spine.AnimationState.SetAnimation(0, "attack-axe1", false);
    midasKilled.enemy.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/midas/wolf_guardian_kills_midas", midasKilled.enemy.Spine.transform.position);
    GameManager.GetInstance().OnConversationNext(midasKilled.spine.gameObject, 5f);
    yield return (object) new WaitForSeconds(1f);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(midasKilled.spine.transform.position);
    midasKilled.spine.gameObject.SetActive(false);
    midasKilled.midasSkull.gameObject.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.SetGlobalOcclusionActive(true);
    midasKilled.Interactable = false;
    yield return (object) new WaitForSeconds(1f);
    if ((UnityEngine.Object) midasKilled.enemy != (UnityEngine.Object) null)
    {
      midasKilled.enemy.enabled = true;
      midasKilled.enemy.DashParticles.gameObject.SetActive(true);
      midasKilled.enemy.transform.localScale = Vector3.one;
      AudioManager.Instance.SetMusicParam(SoundParams.RoomID, 5f);
    }
    while ((UnityEngine.Object) midasKilled.enemy != (UnityEngine.Object) null)
      yield return (object) null;
    AudioManager.Instance.SetMusicParam(SoundParams.RoomID, 7f);
    Interaction_PickUpLoot component = midasKilled.midasSkull.GetComponent<Interaction_PickUpLoot>();
    component.enabled = true;
    component.OnInteraction += new Interaction.InteractionEvent(midasKilled.\u003CKillMidasIE\u003Eb__7_0);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) AudioManager.Instance != (UnityEngine.Object) null)
      AudioManager.Instance.StopLoop(this.receiveLoop);
    AudioManager.Instance.SetMusicParam(SoundParams.RoomID, 0.0f);
  }

  public IEnumerator RecruitMidasSkullIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    MidasKilled midasKilled = this;
    if (num1 != 0)
    {
      if (num1 != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
      int num2 = (int) midasKilled.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
      GameManager.GetInstance().OnConversationEnd();
      DataManager.Instance.GivenMidasSkull = true;
      DataManager.Instance.ShowMidasKilling = false;
      DataManager.Instance.MidasBankUnlocked = true;
      DataManager.Instance.MidasBankIntro = true;
      RoomLockController.RoomCompleted();
      PlayerReturnToBase.Disabled = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PayMidas);
    FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, "Midas");
    followerInfo.TraitsSet = true;
    followerInfo.Traits.Clear();
    followerInfo.Name = ScriptLocalization.NAMES.Midas;
    followerInfo.ID = 100006;
    followerInfo.Special = FollowerSpecialType.Midas_Arm;
    followerInfo.CursedState = Thought.Child;
    DataManager.Instance.Followers_Recruit.Add(followerInfo);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(midasKilled.midasSkull.gameObject, 7f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", midasKilled.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    midasKilled.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    midasKilled.portalSpine.gameObject.SetActive(true);
    float duration1 = midasKilled.portalSpine.AnimationState.SetAnimation(0, "convert-short", false).Animation.Duration;
    midasKilled.midasSkull.enabled = false;
    midasKilled.midasSkull.GetComponent<Interaction_PickUpLoot>().enabled = false;
    midasKilled.midasSkull.transform.DOLocalMoveZ(-1f, 1.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    midasKilled.midasSkull.transform.DOLocalMoveZ(1f, 1.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(1.25f);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration2 = PlayerFarming.Instance.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration;
    GameManager.GetInstance().WaitForSeconds(duration1 - 1.25f, new System.Action(midasKilled.\u003CRecruitMidasSkullIE\u003Eb__10_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(duration2 - 1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  [CompilerGenerated]
  public void \u003CKillMidasIE\u003Eb__7_0(StateMachine state)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RecruitMidasSkullIE());
  }

  [CompilerGenerated]
  public void \u003CRecruitMidasSkullIE\u003Eb__10_0()
  {
    this.midasSkull.gameObject.SetActive(false);
  }
}
