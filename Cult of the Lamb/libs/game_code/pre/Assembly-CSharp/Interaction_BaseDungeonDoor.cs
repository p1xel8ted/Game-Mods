// Decompiled with JetBrains decompiler
// Type: Interaction_BaseDungeonDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_BaseDungeonDoor : Interaction
{
  public int FollowerCount = 5;
  public GameObject RitualPosition;
  public GameObject RitualReceiveDevotionPosition;
  public FollowerLocation Location;
  public string SceneName;
  public BoxCollider2D CollideForDoor;
  public BoxCollider2D BlockingCollider;
  public SimpleSetCamera SimpleSetCamera;
  public SkeletonAnimation DoorSpine;
  public GameObject Lights;
  public GameObject DoorLights;
  public GameObject DoorToMove;
  private Vector3 OpenDoorPosition = new Vector3(0.0f, -2.5f, 4f);
  [SerializeField]
  private ParticleSystem doorSmokeParticleSystem;
  [SerializeField]
  private SkeletonRendererCustomMaterials spineMaterialOverride;
  public GameObject RitualLighting;
  [SerializeField]
  private GameObject BeholderEyeStatues;
  private bool Used;
  private string SRequires;
  private string SOpenDoor;
  [TermsPopup("")]
  public string PlaceName;
  public Color PlaceColor;
  private string PlaceString;
  private List<FollowerBrain> brains;
  private bool HaveFollowers;
  private bool Blocking;
  private List<FollowerManager.SpawnedFollower> spawnedFollowers = new List<FollowerManager.SpawnedFollower>();
  private int NumGivingDevotion;
  public EventInstance LoopedSound;
  private EventInstance loopedInstanceOutro;

  public bool Unlocked { get; private set; }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 2f;
    this.Used = false;
    this.Lights.SetActive(false);
    this.spineMaterialOverride.enabled = false;
    this.Unlocked = DataManager.Instance.UnlockedDungeonDoor.Contains(this.Location);
    if (this.Unlocked && !this.Blocking)
    {
      this.DoorToMove.transform.localPosition = this.OpenDoorPosition;
      this.DoorToMove.SetActive(false);
    }
    else
      this.DoorToMove.transform.localPosition = Vector3.zero;
    this.OpenDoor();
    this.DoorSpine.AnimationState.SetAnimation(0, Mathf.Clamp(DataManager.Instance.GetDungeonLayer(this.Location) - 1, 0, 3).ToString(), false);
    if (DataManager.Instance.BossesCompleted.Contains(this.Location))
      this.DoorSpine.AnimationState.SetAnimation(0, "beaten", true);
    else if (DataManager.Instance.BossesEncountered.Contains(this.Location))
      this.DoorSpine.AnimationState.SetAnimation(0, "4", true);
    this.DoorLights.SetActive(this.GetFollowerCount());
    if (this.Location == FollowerLocation.Dungeon1_1 && !DataManager.Instance.IntroDoor1)
      this.DoorLights.SetActive(false);
    this.CheckBeholders();
  }

  private void CheckBeholders()
  {
    this.BeholderEyeStatues.SetActive(false);
    switch (this.Location)
    {
      case FollowerLocation.Dungeon1_1:
        if (!DataManager.Instance.CheckKilledBosses("Boss Beholder 1"))
          break;
        this.BeholderEyeStatues.SetActive(true);
        break;
      case FollowerLocation.Dungeon1_2:
        if (!DataManager.Instance.CheckKilledBosses("Boss Beholder 2"))
          break;
        this.BeholderEyeStatues.SetActive(true);
        break;
      case FollowerLocation.Dungeon1_3:
        if (!DataManager.Instance.CheckKilledBosses("Boss Beholder 3"))
          break;
        this.BeholderEyeStatues.SetActive(true);
        break;
      case FollowerLocation.Dungeon1_4:
        if (!DataManager.Instance.CheckKilledBosses("Boss Beholder 4"))
          break;
        this.BeholderEyeStatues.SetActive(true);
        break;
    }
  }

  private void Start()
  {
    this.OnEnableInteraction();
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.SRequires = ScriptLocalization.Interactions.Requires;
    this.SOpenDoor = ScriptLocalization.Interactions.OpenDoor;
    this.PlaceString = LocalizationManager.GetTranslation(this.PlaceName);
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    if (this.Unlocked)
      return;
    MonoSingleton<Indicator>.Instance.ShowTopInfo($"<sprite name=\"img_SwirleyLeft\"> {this.PlaceString.Colour(this.PlaceColor)} <sprite name=\"img_SwirleyRight\">");
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    MonoSingleton<Indicator>.Instance.HideTopInfo();
  }

  public bool GetFollowerCount()
  {
    this.brains = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = this.brains.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead.Contains(this.brains[index]._directInfoAccess))
        this.brains.RemoveAt(index);
    }
    return this.brains.Count >= this.FollowerCount;
  }

  public override void GetLabel()
  {
    if (this.Unlocked)
    {
      this.Label = $"<sprite name=\"img_SwirleyLeft\"> {this.PlaceString.Colour(this.PlaceColor)} <sprite name=\"img_SwirleyRight\">";
      this.Interactable = false;
    }
    else
    {
      this.Interactable = true;
      this.HaveFollowers = this.GetFollowerCount();
      string str;
      if (!this.HaveFollowers)
        str = $"{this.SRequires}<color=red> {(object) DataManager.Instance.Followers.Count}</color> / {(object) this.FollowerCount} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
      else
        str = $"{this.SOpenDoor} | {(object) this.FollowerCount} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
      this.Label = str;
    }
  }

  public override void IndicateHighlighted()
  {
    if (this.Unlocked)
      return;
    base.IndicateHighlighted();
  }

  private void OpenDoor()
  {
    if (this.Unlocked && !this.Blocking)
    {
      this.CollideForDoor.enabled = true;
      this.BlockingCollider.enabled = false;
      this.Lights.SetActive(true);
    }
    else
    {
      this.CollideForDoor.enabled = false;
      this.BlockingCollider.enabled = true;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.HaveFollowers)
    {
      this.StartCoroutine((IEnumerator) this.DoRitualRoutine());
    }
    else
    {
      this.IndicateHighlighted();
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
      MonoSingleton<Indicator>.Instance.PlayShake();
    }
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    for (int index = this.spawnedFollowers.Count - 1; index >= 0; --index)
      FollowerManager.CleanUpCopyFollower(this.spawnedFollowers[index]);
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.OpenDoorRoutine());

  public void Block()
  {
    Debug.Log((object) "BLOCK ME!");
    this.Blocking = true;
    this.DoorToMove.transform.DOLocalMove(Vector3.zero, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.Unlocked = false;
    this.OpenDoor();
  }

  public void Unblock()
  {
    this.Blocking = false;
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(this.Location))
      return;
    this.DoorToMove.transform.DOLocalMove(this.OpenDoorPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    this.Unlocked = true;
    this.OpenDoor();
  }

  private IEnumerator DoRitualRoutine()
  {
    Interaction_BaseDungeonDoor interactionBaseDungeonDoor = this;
    interactionBaseDungeonDoor.spineMaterialOverride.enabled = true;
    yield return (object) null;
    SimulationManager.Pause();
    bool Waiting = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBaseDungeonDoor.state.gameObject);
    PlayerFarming.Instance.GoToAndStop(interactionBaseDungeonDoor.RitualPosition.transform.position + new Vector3(0.0f, -2f), interactionBaseDungeonDoor.RitualPosition, GoToCallback: (System.Action) (() =>
    {
      PlayerFarming.Instance.transform.position = this.RitualPosition.transform.position + new Vector3(0.0f, -2f);
      Waiting = false;
    }));
    yield return (object) new WaitForSeconds(1f);
    List<FollowerBrain> brains = new List<FollowerBrain>((IEnumerable<FollowerBrain>) FollowerBrain.AllBrains);
    for (int index = brains.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Dead.Contains(brains[index]._directInfoAccess) || FollowerManager.FollowerLocked(brains[index].Info.ID))
        brains.RemoveAt(index);
    }
    yield return (object) new WaitForSeconds(1f);
    for (int i = 0; i < brains.Count; ++i)
    {
      FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(brains[i]._directInfoAccess, interactionBaseDungeonDoor.RitualPosition.transform.position, interactionBaseDungeonDoor.transform.parent, PlayerFarming.Location);
      interactionBaseDungeonDoor.spawnedFollowers.Add(spawnedFollower);
      spawnedFollower.Follower.transform.position = interactionBaseDungeonDoor.GetFollowerPosition(i, brains.Count);
      spawnedFollower.Follower.State.facingAngle = (double) spawnedFollower.Follower.transform.position.x > 0.0 ? 180f : 0.0f;
      spawnedFollower.Follower.State.LookAngle = spawnedFollower.Follower.State.facingAngle;
      spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      spawnedFollower.Follower.SetFaceAnimation("Emotions/emotion-happy", true);
      if ((bool) (UnityEngine.Object) spawnedFollower.Follower.GetComponentInChildren<ShadowLockToGround>())
        spawnedFollower.Follower.GetComponentInChildren<ShadowLockToGround>().enabled = false;
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spawnedFollower.Follower.gameObject);
      spawnedFollower.Follower.TimedAnimation("spawn-in", 0.466666669f, (System.Action) (() =>
      {
        spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num = (double) spawnedFollower.Follower.SetBodyAnimation("dance", true);
      }));
      yield return (object) new WaitForSeconds(0.05f);
    }
    while (Waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNext(CrownStatueController.Instance.CameraPosition, 10f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/door-ritual", 0, false);
    AudioManager.Instance.PlayOneShot("event:/Stings/white_eyes", PlayerFarming.Instance.gameObject);
    interactionBaseDungeonDoor.RitualLighting.SetActive(true);
    BiomeConstants.Instance.ImpactFrameForDuration();
    interactionBaseDungeonDoor.LoopedSound = AudioManager.Instance.CreateLoop("event:/door/eye_beam_door_open", true);
    MMVibrate.RumbleContinuous(0.0f, 2f);
    CameraManager.instance.ShakeCameraForDuration(0.6f, 0.7f, 2f);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 0.75f);
    PlayerFarming.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseDungeonDoor.HandleEvent);
    yield return (object) new WaitForSeconds(1f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 5f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
    interactionBaseDungeonDoor.NumGivingDevotion = 0;
    foreach (FollowerManager.SpawnedFollower spawnedFollower in interactionBaseDungeonDoor.spawnedFollowers)
    {
      ++interactionBaseDungeonDoor.NumGivingDevotion;
      spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) spawnedFollower.Follower.SetBodyAnimation("worship", true);
      interactionBaseDungeonDoor.StartCoroutine((IEnumerator) interactionBaseDungeonDoor.SpawnSouls(spawnedFollower.Follower.Spine.transform.position));
      yield return (object) new WaitForSeconds(0.1f);
    }
    while (interactionBaseDungeonDoor.NumGivingDevotion > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionBaseDungeonDoor.HandleEvent);
    MMVibrate.StopRumble();
    interactionBaseDungeonDoor.RitualLighting.SetActive(false);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    AudioManager.Instance.StopLoop(interactionBaseDungeonDoor.LoopedSound);
    yield return (object) interactionBaseDungeonDoor.StartCoroutine((IEnumerator) interactionBaseDungeonDoor.OpenDoorRoutine());
    yield return (object) new WaitForSeconds(1f);
    foreach (FollowerManager.SpawnedFollower spawnedFollower1 in interactionBaseDungeonDoor.spawnedFollowers)
    {
      FollowerManager.SpawnedFollower spawnedFollower = spawnedFollower1;
      interactionBaseDungeonDoor.StartCoroutine((IEnumerator) interactionBaseDungeonDoor.PlaySoundDelay(spawnedFollower.Follower.gameObject));
      spawnedFollower.Follower.TimedAnimation("spawn-out", 0.8666667f, (System.Action) (() => FollowerManager.CleanUpCopyFollower(spawnedFollower)));
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionBaseDungeonDoor.spawnedFollowers.Clear();
  }

  private IEnumerator PlaySoundDelay(GameObject spawnedFollower)
  {
    yield return (object) new WaitForSeconds(0.566666663f);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spawnedFollower);
  }

  private IEnumerator SpawnSouls(Vector3 Position)
  {
    float delay = 0.5f;
    float Count = 8f;
    for (int i = 0; (double) i < (double) Count; ++i)
    {
      float num = (float) i / Count;
      SoulCustomTargetLerp.Create(this.RitualReceiveDevotionPosition.gameObject, Position + Vector3.forward * 2f + Vector3.up, 0.5f, Color.red);
      yield return (object) new WaitForSeconds(delay - 0.2f * num);
    }
    --this.NumGivingDevotion;
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
      if (!(e.Data.Name == "sfxTrigger"))
        return;
      AudioManager.Instance.CreateLoop("event:/Stings/lamb_ascension", PlayerFarming.Instance.gameObject, true);
    }
  }

  private Vector3 GetFollowerPosition(int index, int total)
  {
    if (total <= 12)
    {
      float num = 3f;
      float f = (float) ((double) index * (360.0 / (double) total) * (Math.PI / 180.0));
      return this.RitualPosition.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
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
    return this.RitualPosition.transform.position + new Vector3(num1 * Mathf.Cos(f1), num1 * Mathf.Sin(f1));
  }

  public void FadeDoorLight()
  {
    this.DoorLights.SetActive(true);
    SpriteRenderer component = this.DoorLights.GetComponent<SpriteRenderer>();
    component.color = component.color with { a = 0.0f };
    component.DOFade(1f, 2f);
    DataManager.Instance.IntroDoor1 = true;
  }

  public IEnumerator OpenDoorRoutine()
  {
    Interaction_BaseDungeonDoor interactionBaseDungeonDoor = this;
    GameManager.GetInstance().OnConversationNew();
    interactionBaseDungeonDoor.SimpleSetCamera.Play();
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(interactionBaseDungeonDoor.Location))
      DataManager.Instance.UnlockedDungeonDoor.Add(interactionBaseDungeonDoor.Location);
    AudioManager.Instance.PlayOneShot("event:/door/door_unlock", interactionBaseDungeonDoor.gameObject);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/door/door_lower", interactionBaseDungeonDoor.gameObject);
    interactionBaseDungeonDoor.doorSmokeParticleSystem.Play();
    float Progress = 0.0f;
    float Duration = 3f;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, Duration);
    Vector3 StartingPosition = interactionBaseDungeonDoor.DoorToMove.transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      interactionBaseDungeonDoor.DoorToMove.transform.position = Vector3.Lerp(StartingPosition, StartingPosition + interactionBaseDungeonDoor.OpenDoorPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/door/door_done", interactionBaseDungeonDoor.gameObject);
    interactionBaseDungeonDoor.doorSmokeParticleSystem.Stop();
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    interactionBaseDungeonDoor.SimpleSetCamera.Reset();
    interactionBaseDungeonDoor.spineMaterialOverride.enabled = false;
    interactionBaseDungeonDoor.Unlocked = true;
    interactionBaseDungeonDoor.OpenDoor();
    interactionBaseDungeonDoor.Lights.SetActive(true);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player") || this.Used)
      return;
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopCurrentAtmos();
    AudioManager.Instance.PlayOneShot("event:/Stings/boss_door_complete");
    AudioManager.Instance.PlayOneShot("event:/ui/map_location_appear");
    PlayerFarming.Instance.GetBlackSoul(Mathf.RoundToInt(FaithAmmo.Total - FaithAmmo.Ammo), false);
    this.Used = true;
    MMTransition.StopCurrentTransition();
    Interaction_BaseDungeonDoor.GetFloor(this.Location);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.SceneName, 1f, "", new System.Action(this.FadeSave));
    GameManager.GetInstance().OnConversationNew();
  }

  public static void GetFloor(FollowerLocation Location)
  {
    DataManager.LocationAndLayer locationAndLayer = DataManager.LocationAndLayer.ContainsLocation(Location, DataManager.Instance.CachePreviousRun);
    int num1 = 4;
    int num2 = DataManager.Instance.GetDungeonLayer(Location);
    bool flag = num2 >= num1 || DataManager.Instance.DungeonCompleted(Location);
    DataManager.Instance.DungeonBossFight = num2 >= num1 && !DataManager.Instance.DungeonCompleted(Location);
    if (flag)
    {
      num2 = DataManager.RandomSeed.Next(1, num1 + 1);
      if (locationAndLayer != null)
      {
        while (num2 == locationAndLayer.Layer)
          num2 = DataManager.RandomSeed.Next(1, num1 + 1);
      }
    }
    GameManager.DungeonUseAllLayers = flag;
    if (flag)
      GameManager.CurrentDungeonLayer = 4;
    else
      GameManager.NextDungeonLayer(num2);
    GameManager.NewRun("", false);
    if (locationAndLayer != null)
    {
      locationAndLayer.Layer = num2;
      Debug.Log((object) ("Now set cached layer to: " + (object) locationAndLayer.Layer));
    }
    else
      DataManager.Instance.CachePreviousRun.Add(new DataManager.LocationAndLayer(Location, num2));
  }

  private void FadeSave() => SaveAndLoad.Save();
}
