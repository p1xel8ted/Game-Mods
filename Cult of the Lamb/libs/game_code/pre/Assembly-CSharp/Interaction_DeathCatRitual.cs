// Decompiled with JetBrains decompiler
// Type: Interaction_DeathCatRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DeathCatRitual : Interaction
{
  [SerializeField]
  private Transform playerPosition;
  [SerializeField]
  private Transform spawnPosition;
  [SerializeField]
  private GameObject RitualLighting;
  [SerializeField]
  private GameObject LightingOverride;
  [SerializeField]
  private GameObject DappleLight;
  private List<FollowerManager.SpawnedFollower> spawnedFollowers = new List<FollowerManager.SpawnedFollower>();
  private int TargetFollowerCount = 20;
  private bool EnoughFollowers;
  private EventInstance loopedInstanceOutro;
  private EventInstance loopedInstance;

  public override void GetLabel()
  {
    if (DataManager.Instance.DeathCatBeaten)
    {
      this.Label = "";
    }
    else
    {
      base.GetLabel();
      this.EnoughFollowers = DataManager.Instance.Followers.Count >= this.TargetFollowerCount;
      this.HoldToInteract = this.EnoughFollowers;
      string str;
      if (!this.EnoughFollowers)
        str = $"{ScriptLocalization.Interactions.Requires}<color=red> {(object) DataManager.Instance.Followers.Count} / {(object) this.TargetFollowerCount} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
      else
        str = $"{ScriptLocalization.Interactions.PerformRitual} | {(object) DataManager.Instance.Followers.Count} / {(object) this.TargetFollowerCount} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
      this.Label = str;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!this.EnoughFollowers)
      MonoSingleton<Indicator>.Instance.PlayShake();
    else
      this.StartCoroutine((IEnumerator) this.RitualIE());
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "Spin")
    {
      Debug.Log((object) "Spin sfx");
      CameraManager.shakeCamera(0.1f, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
      MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, alsoRumble: true, coroutineSupport: (MonoBehaviour) this);
      this.loopedInstanceOutro = AudioManager.Instance.CreateLoop("event:/player/jump_spin_float", PlayerFarming.Instance.gameObject, true);
    }
    else
    {
      if (!(e.Data.Name == "whiteEyes"))
        return;
      AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", PlayerFarming.Instance.gameObject);
    }
  }

  private IEnumerator RitualIE()
  {
    Interaction_DeathCatRitual interactionDeathCatRitual = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDeathCatRitual.gameObject);
    bool waiting = true;
    PlayerFarming.Instance.GoToAndStop(interactionDeathCatRitual.playerPosition.position, interactionDeathCatRitual.gameObject, GoToCallback: (System.Action) (() =>
    {
      PlayerFarming.Instance.state.transform.DOMove(this.playerPosition.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      waiting = false;
    }));
    while (waiting)
      yield return (object) null;
    SimulationManager.Pause();
    List<FollowerBrain> brains = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = brains.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead.Contains(brains[index]._directInfoAccess))
        brains.RemoveAt(index);
    }
    yield return (object) new WaitForSeconds(1f);
    interactionDeathCatRitual.loopedInstance = AudioManager.Instance.CreateLoop("event:/followers/warp_in_pre_deathcat", PlayerFarming.Instance.gameObject, true);
    for (int i = 0; i < brains.Count; ++i)
    {
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(brains[i]._directInfoAccess, interactionDeathCatRitual.spawnPosition.position, interactionDeathCatRitual.transform.parent, PlayerFarming.Location);
      interactionDeathCatRitual.spawnedFollowers.Add(spawnedFollower);
      spawnedFollower.Follower.transform.position = interactionDeathCatRitual.GetFollowerPosition(i, brains.Count);
      spawnedFollower.Follower.transform.localScale = new Vector3((double) PlayerFarming.Instance.transform.position.x > (double) spawnedFollower.Follower.transform.position.x ? -1f : 1f, 1f, 1f);
      spawnedFollower.Follower.State.LookAngle = spawnedFollower.Follower.State.facingAngle;
      spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      spawnedFollower.Follower.SimpleAnimator.enabled = false;
      spawnedFollower.Follower.SetFaceAnimation("Emotions/emotion-happy", true);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spawnedFollower.Follower.gameObject);
      AudioManager.Instance.PlayOneShot("event:/material/footstep_water", spawnedFollower.Follower.gameObject);
      spawnedFollower.Follower.TimedAnimation("spawn-in", 0.466666669f, (System.Action) (() => spawnedFollower.Follower.AddBodyAnimation("worship", true, 0.0f)));
      yield return (object) new WaitForSeconds(0.05f);
    }
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/final-ritual-start", 0, false);
    AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower", PlayerFarming.Instance.transform.position);
    AudioManager.Instance.PlayOneShot("event:/sermon/sermon_speech_bubble", PlayerFarming.Instance.transform.position);
    PlayerFarming.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionDeathCatRitual.HandleEvent);
    yield return (object) new WaitForSeconds(0.5f);
    foreach (FollowerManager.SpawnedFollower spawnedFollower in interactionDeathCatRitual.spawnedFollowers)
    {
      interactionDeathCatRitual.StartCoroutine((IEnumerator) interactionDeathCatRitual.SpawnSouls(spawnedFollower.Follower.transform.position));
      yield return (object) new WaitForSeconds(0.1f);
    }
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", PlayerFarming.Instance.transform.position);
    interactionDeathCatRitual.RitualLighting.SetActive(true);
    interactionDeathCatRitual.LightingOverride.SetActive(false);
    BiomeConstants.Instance.ImpactFrameForDuration();
    interactionDeathCatRitual.DappleLight.SetActive(false);
    MMVibrate.RumbleContinuous(0.0f, 1f);
    CameraManager.instance.ShakeCameraForDuration(0.6f, 1f, 4.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 0.75f);
    yield return (object) new WaitForSeconds(4.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    MMVibrate.StopRumble();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.WhiteFade, MMTransition.NO_SCENE, 5f, "", (System.Action) null);
    yield return (object) new WaitForSeconds(2f);
    for (int index = interactionDeathCatRitual.spawnedFollowers.Count - 1; index >= 0; --index)
      FollowerManager.CleanUpCopyFollower(interactionDeathCatRitual.spawnedFollowers[index]);
    GameManager.GetInstance().OnConversationEnd();
    BiomeGenerator.ChangeRoom(0, 1);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    yield return (object) new WaitForSeconds(4.75f);
    PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionDeathCatRitual.HandleEvent);
    AudioManager.Instance.StopLoop(interactionDeathCatRitual.loopedInstanceOutro);
  }

  public void Skip() => this.StartCoroutine((IEnumerator) this.SkipRoutine());

  private IEnumerator SkipRoutine()
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.WhiteFade, MMTransition.NO_SCENE, 5f, "", (System.Action) null);
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    yield return (object) new WaitForSeconds(2f);
    BiomeGenerator.ChangeRoom(0, 1);
  }

  private Vector3 GetFollowerPosition(int index, int total)
  {
    if (total <= this.TargetFollowerCount)
    {
      float num = 3f;
      float f = (float) ((double) index * (360.0 / (double) total) * (Math.PI / 180.0));
      return this.playerPosition.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    }
    int b = 8;
    float num1;
    float f1;
    if (index < b)
    {
      num1 = 3f;
      f1 = (float) ((double) index * (360.0 / (double) Mathf.Min(total, b)) * (Math.PI / 180.0));
    }
    else
    {
      num1 = 4f;
      f1 = (float) ((double) (index - b) * (360.0 / (double) (total - b)) * (Math.PI / 180.0));
    }
    return this.playerPosition.position + new Vector3(num1 * Mathf.Cos(f1), num1 * Mathf.Sin(f1));
  }

  private IEnumerator SpawnSouls(Vector3 Position)
  {
    float delay = 0.5f;
    float Count = 5f;
    for (int i = 0; (double) i < (double) Count; ++i)
    {
      float num = (float) i / Count;
      SoulCustomTargetLerp.Create(PlayerFarming.Instance.CrownBone.gameObject, Position, 0.5f, Color.red);
      yield return (object) new WaitForSeconds(delay - 0.2f * num);
    }
  }
}
