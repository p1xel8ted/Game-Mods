// Decompiled with JetBrains decompiler
// Type: Interaction_DeathCatRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public Transform playerPosition;
  [SerializeField]
  public Transform spawnPosition;
  [SerializeField]
  public GameObject RitualLighting;
  [SerializeField]
  public GameObject LightingOverride;
  [SerializeField]
  public GameObject DappleLight;
  public List<FollowerManager.SpawnedFollower> spawnedFollowers = new List<FollowerManager.SpawnedFollower>();
  public int TargetFollowerCount = 20;
  public bool EnoughFollowers;
  public EventInstance loopedInstanceOutro;
  public EventInstance loopedInstance;

  public void Awake()
  {
    this.EnoughFollowers = DataManager.Instance.Followers.Count >= this.TargetFollowerCount;
    if (!this.EnoughFollowers)
      return;
    this.InitializeFollowers();
  }

  public void InitializeFollowers()
  {
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = followerBrainList.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead.Contains(followerBrainList[index]._directInfoAccess))
        followerBrainList.RemoveAt(index);
    }
    for (int index = 0; index < followerBrainList.Count; ++index)
    {
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(followerBrainList[index]._directInfoAccess, this.spawnPosition.position, this.transform.parent, PlayerFarming.Location);
      spawnedFollower.Follower.gameObject.SetActive(false);
      this.spawnedFollowers.Add(spawnedFollower);
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    for (int index = this.spawnedFollowers.Count - 1; index >= 0; --index)
      FollowerManager.CleanUpCopyFollower(this.spawnedFollowers[index]);
    this.spawnedFollowers.Clear();
  }

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
      if (LocalizeIntegration.IsArabic())
      {
        string str1 = LocalizeIntegration.ReverseText(this.TargetFollowerCount.ToString());
        string str2 = LocalizeIntegration.ReverseText(DataManager.Instance.Followers.Count.ToString());
        string str3;
        if (!this.EnoughFollowers)
          str3 = $"{ScriptLocalization.Interactions.Requires}<color=red> {str1} / {str2} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
        else
          str3 = $"{ScriptLocalization.Interactions.PerformRitual} | {str1} / {str2} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
        this.Label = str3;
      }
      else
      {
        string str;
        if (!this.EnoughFollowers)
          str = $"{ScriptLocalization.Interactions.Requires}<color=red> {DataManager.Instance.Followers.Count.ToString()} / {this.TargetFollowerCount.ToString()} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
        else
          str = $"{ScriptLocalization.Interactions.PerformRitual} | {DataManager.Instance.Followers.Count.ToString()} / {this.TargetFollowerCount.ToString()} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
        this.Label = str;
      }
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!this.EnoughFollowers)
      this.playerFarming.indicator.PlayShake();
    else
      this.StartCoroutine((IEnumerator) this.RitualIE());
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "Spin")
    {
      Debug.Log((object) "Spin sfx");
      CameraManager.shakeCamera(0.1f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
      MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) this);
      this.loopedInstanceOutro = AudioManager.Instance.CreateLoop("event:/player/jump_spin_float", this.playerFarming.gameObject, true);
    }
    else
    {
      if (!(e.Data.Name == "whiteEyes"))
        return;
      AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", this.playerFarming.gameObject);
    }
  }

  public IEnumerator RitualIE()
  {
    Interaction_DeathCatRitual interactionDeathCatRitual = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDeathCatRitual.gameObject);
    int count = 0;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      PlayerFarming playerFarming = player;
      Vector3 pos = interactionDeathCatRitual.playerPosition.position;
      if (PlayerFarming.playersCount > 1)
        pos.x += (UnityEngine.Object) playerFarming == (UnityEngine.Object) PlayerFarming.players[0] ? -0.75f : 0.75f;
      playerFarming.GoToAndStop(pos, interactionDeathCatRitual.gameObject, GoToCallback: (System.Action) (() =>
      {
        playerFarming.state.transform.DOMove(pos, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        ++count;
      }));
    }
    while (count < PlayerFarming.playersCount)
      yield return (object) null;
    SimulationManager.Pause();
    for (int index = interactionDeathCatRitual.spawnedFollowers.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead.Contains(interactionDeathCatRitual.spawnedFollowers[index].FollowerBrain._directInfoAccess))
      {
        FollowerManager.CleanUpCopyFollower(interactionDeathCatRitual.spawnedFollowers[index]);
        interactionDeathCatRitual.spawnedFollowers.RemoveAt(index);
      }
    }
    yield return (object) new WaitForSeconds(1f);
    interactionDeathCatRitual.loopedInstance = AudioManager.Instance.CreateLoop("event:/followers/warp_in_pre_deathcat", interactionDeathCatRitual.playerFarming.gameObject, true);
    for (int i = 0; i < interactionDeathCatRitual.spawnedFollowers.Count; ++i)
    {
      FollowerManager.SpawnedFollower spawnedFollower = interactionDeathCatRitual.spawnedFollowers[i];
      spawnedFollower.Follower.gameObject.SetActive(true);
      spawnedFollower.Follower.transform.position = interactionDeathCatRitual.GetFollowerPosition(i, interactionDeathCatRitual.spawnedFollowers.Count);
      spawnedFollower.Follower.transform.localScale = new Vector3((double) interactionDeathCatRitual.playerFarming.transform.position.x > (double) spawnedFollower.Follower.transform.position.x ? -1f : 1f, 1f, 1f);
      spawnedFollower.Follower.State.LookAngle = spawnedFollower.Follower.State.facingAngle;
      spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      spawnedFollower.Follower.SimpleAnimator.enabled = false;
      spawnedFollower.Follower.SetFaceAnimation("Emotions/emotion-happy", true);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spawnedFollower.Follower.gameObject);
      AudioManager.Instance.PlayOneShot("event:/material/footstep_water", spawnedFollower.Follower.gameObject);
      spawnedFollower.Follower.TimedAnimation("spawn-in", 0.466666669f, (System.Action) (() => spawnedFollower.Follower.AddBodyAnimation("worship", true, 0.0f)));
      yield return (object) new WaitForSeconds(0.05f);
    }
    if (interactionDeathCatRitual._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
    else
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_speak_to_follower_noBookPage");
    if (interactionDeathCatRitual._playerFarming.isLamb)
      AudioManager.Instance.PlayOneShot("event:/sermon/sermon_speech_bubble", interactionDeathCatRitual.playerFarming.transform.position);
    else
      AudioManager.Instance.PlayOneShot("event:/sermon/goat_sermon_speech_bubble", interactionDeathCatRitual.playerFarming.transform.position);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      player.simpleSpineAnimator.Animate("rituals/final-ritual-start", 0, false);
      player.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionDeathCatRitual.HandleEvent);
    }
    yield return (object) new WaitForSeconds(0.5f);
    foreach (FollowerManager.SpawnedFollower spawnedFollower in interactionDeathCatRitual.spawnedFollowers)
    {
      interactionDeathCatRitual.StartCoroutine((IEnumerator) interactionDeathCatRitual.SpawnSouls(spawnedFollower.Follower.transform.position));
      yield return (object) new WaitForSeconds(0.1f);
    }
    AudioManager.Instance.PlayOneShot("event:/rituals/blood_sacrifice", interactionDeathCatRitual.playerFarming.transform.position);
    interactionDeathCatRitual.RitualLighting.SetActive(true);
    interactionDeathCatRitual.LightingOverride.SetActive(false);
    BiomeConstants.Instance.ImpactFrameForDuration();
    interactionDeathCatRitual.DappleLight.SetActive(false);
    MMVibrate.RumbleContinuous(0.0f, 1f, interactionDeathCatRitual.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.6f, 1f, 4.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 0.75f);
    yield return (object) new WaitForSeconds(4.5f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    MMVibrate.StopRumble();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.WhiteFade, MMTransition.NO_SCENE, 5f, "", (System.Action) null);
    yield return (object) new WaitForSeconds(2f);
    for (int index = interactionDeathCatRitual.spawnedFollowers.Count - 1; index >= 0; --index)
      FollowerManager.CleanUpCopyFollower(interactionDeathCatRitual.spawnedFollowers[index]);
    interactionDeathCatRitual.spawnedFollowers.Clear();
    GameManager.GetInstance().OnConversationEnd();
    BiomeGenerator.ChangeRoom(0, 1);
    PlayerFarming.SetStateForAllPlayers();
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    yield return (object) new WaitForSeconds(4.75f);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionDeathCatRitual.HandleEvent);
    AudioManager.Instance.StopLoop(interactionDeathCatRitual.loopedInstanceOutro);
  }

  public void Skip() => this.StartCoroutine((IEnumerator) this.SkipRoutine());

  public IEnumerator SkipRoutine()
  {
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.WhiteFade, MMTransition.NO_SCENE, 5f, "", (System.Action) null);
    AudioManager.Instance.SetMusicRoomID(0, "deathcat_room_id");
    yield return (object) new WaitForSeconds(2f);
    BiomeGenerator.ChangeRoom(0, 1);
  }

  public Vector3 GetFollowerPosition(int index, int total)
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

  public IEnumerator SpawnSouls(Vector3 Position)
  {
    float delay = 0.5f;
    float Count = 5f;
    for (int i = 0; (double) i < (double) Count; ++i)
    {
      float num = (float) i / Count;
      SoulCustomTargetLerp.Create(PlayerFarming.players[UnityEngine.Random.Range(0, PlayerFarming.playersCount)].CrownBone.gameObject, Position, 0.5f, Color.red);
      yield return (object) new WaitForSeconds(delay - 0.2f * num);
    }
  }
}
