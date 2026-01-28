// Decompiled with JetBrains decompiler
// Type: RitualFirePit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RitualFirePit : Ritual
{
  public override UpgradeSystem.Type RitualType
  {
    get
    {
      return UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2) ? UpgradeSystem.Type.Ritual_FirePit_2 : UpgradeSystem.Type.Ritual_FirePit;
    }
  }

  public override void Play()
  {
    base.Play();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RitualRoutine());
  }

  public IEnumerator RitualRoutine()
  {
    RitualFirePit ritualFirePit = this;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2))
      AudioManager.Instance.PlayOneShot("event:/dlc/ritual/bonfire_start");
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    Interaction_FireDancePit firePit = ChurchFollowerManager.Instance.FirePit;
    firePit.gameObject.SetActive(true);
    firePit.transform.position = new Vector3(firePit.transform.position.x, firePit.transform.position.y, 2f);
    firePit.transform.DOMoveZ(0.0f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    firePit.GetComponent<BoxCollider2D>().enabled = true;
    Bounds bounds = firePit.GetComponent<BoxCollider2D>().bounds;
    AstarPath.active.UpdateGraphs(bounds);
    PlayerFarming.Instance.GoToAndStop(firePit.PlayerPosition.transform.position, DisableCollider: true, GoToCallback: (System.Action) (() =>
    {
      Interaction_TempleAltar.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("idle", 0, true);
      PlayerFarming.Instance.state.transform.DOMove(firePit.PlayerPosition.transform.position, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }));
    Interaction_TempleAltar.Instance.SimpleSetCamera.Play();
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.bongos_singing);
    Debug.Log((object) "A");
    while (firePit.StructureInfo == null)
      yield return (object) null;
    Debug.Log((object) "B");
    yield return (object) ritualFirePit.StartCoroutine((IEnumerator) ritualFirePit.WaitForFollowersToTakeSeat(0));
    Debug.Log((object) "C");
    PlayerFarming.Instance.Spine.skeleton.FindBone("ritualring").Rotation += 60f;
    PlayerFarming.Instance.Spine.skeleton.UpdateWorldTransform();
    PlayerFarming.Instance.Spine.skeleton.Update(Time.deltaTime);
    ChurchFollowerManager.Instance.PlayOverlay(ChurchFollowerManager.OverlayType.Ritual, "resurrect");
    BiomeConstants.Instance.ChromaticAbberationTween(2f, BiomeConstants.Instance.ChromaticAberrationDefaultValue, 1f);
    BiomeConstants.Instance.VignetteTween(2f, BiomeConstants.Instance.VignetteDefaultValue, 0.7f);
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6.5f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/fire-ritual-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("rituals/fire-ritual-loop", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.66f);
    Interaction_TempleAltar.Instance.SimpleSetCamera.Reset();
    Interaction_TempleAltar.Instance.CloseUpCamera.Play();
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.RedOverlay.gameObject.SetActive(true);
    ChurchFollowerManager.Instance.RedOverlay.alpha = 0.0f;
    ChurchFollowerManager.Instance.RedOverlay.DOFade(1f, 1f).SetDelay<TweenerCore<float, float, FloatOptions>>(1.2f);
    DeviceLightingManager.TransitionLighting(Color.black, Color.red, 1.2f);
    firePit.BonfireOn.SetActive(true);
    firePit.BonfireOff.SetActive(false);
    bool isBonfireLevelTwo = UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2);
    AudioManager.Instance.PlayOneShot(isBonfireLevelTwo ? "event:/dlc/ritual/bonfire_fire_light" : "event:/cooking/fire_start", firePit.gameObject);
    ritualFirePit.loopedSound = AudioManager.Instance.CreateLoop("event:/fire/big_fire", firePit.gameObject, true);
    yield return (object) new WaitForSeconds(1.2f);
    DeviceLightingManager.TransitionLighting(Color.red, Color.red, 4f);
    List<FollowerBrain> followers = new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon());
    foreach (FollowerBrain followerBrain in followers)
    {
      if (followerBrain.CurrentTask is FollowerTask_ChangeLocation)
        followerBrain.CurrentTask.Arrive();
      Follower followerById = FollowerManager.FindFollowerByID(followerBrain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
      {
        followerById.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num = (double) followerById.SetBodyAnimation("dance", true);
      }
    }
    yield return (object) new WaitForSeconds(4f);
    ChurchFollowerManager.Instance.RedOverlay.DOFade(0.0f, 1f);
    DeviceLightingManager.TransitionLighting(Color.red, Color.black, 2f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("rituals/fire-ritual-stop", 0, false, 0.0f);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    foreach (FollowerBrain followerBrain in followers)
      followerBrain.CompleteCurrentTask();
    firePit.BonfireOn.SetActive(false);
    firePit.BonfireOff.SetActive(true);
    AudioManager.Instance.PlayOneShot(isBonfireLevelTwo ? "event:/dlc/ritual/bonfire_descend" : "event:/cooking/fire_start", firePit.gameObject);
    AudioManager.Instance.StopLoop(ritualFirePit.loopedSound);
    firePit.transform.DOMoveZ(4f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(1f);
    ChurchFollowerManager.Instance.RedOverlay.gameObject.SetActive(false);
    firePit.gameObject.SetActive(false);
    firePit.GetComponent<BoxCollider2D>().enabled = false;
    AstarPath.active.UpdateGraphs(bounds);
    BiomeConstants.Instance.ChromaticAbberationTween(1f, 1f, BiomeConstants.Instance.ChromaticAberrationDefaultValue);
    BiomeConstants.Instance.VignetteTween(1f, 0.7f, BiomeConstants.Instance.VignetteDefaultValue);
    ChurchFollowerManager.Instance.EndRitualOverlay();
    GameManager.GetInstance().CamFollowTarget.targetDistance = 11f;
    Interaction_TempleAltar.Instance.RitualLighting.gameObject.SetActive(false);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    float num1 = 0.0f;
    foreach (FollowerBrain brain in Ritual.FollowerToAttendSermon)
    {
      float Delay = UnityEngine.Random.Range(0.1f, 0.5f);
      num1 += Delay;
      ritualFirePit.StartCoroutine((IEnumerator) ritualFirePit.DelayFollowerReaction(brain, Delay));
    }
    yield return (object) new WaitForSeconds(1.5f);
    Interaction_TempleAltar.Instance.CloseUpCamera.Reset();
    DataManager.Instance.LastFeastDeclared = TimeManager.TotalElapsedGameTime;
    ObjectiveManager.CompleteRitualObjective(UpgradeSystem.Type.Ritual_FirePit_2);
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit_2))
      DataManager.Instance.LastWarmthRitualDeclared = TimeManager.TotalElapsedGameTime;
    ritualFirePit.CompleteRitual();
    yield return (object) new WaitForSeconds(1f);
    CultFaithManager.AddThought(Thought.DancePit);
  }

  public IEnumerator MoveFollower(Follower follower, int index)
  {
    RitualFirePit ritualFirePit = this;
    List<Vector3> positions = new List<Vector3>()
    {
      Vector3.left,
      Vector3.up,
      Vector3.right,
      Vector3.down
    };
    bool waiting = true;
    follower.HoodOff(onComplete: (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    yield return (object) ritualFirePit.StartCoroutine((IEnumerator) follower.GoToRoutine(ChurchFollowerManager.Instance.RitualCenterPosition.position + positions[index]));
    double num = (double) follower.SetBodyAnimation("dance-hooded", true);
  }

  public IEnumerator WaitForFollowersToTakeSeat(int firepitID)
  {
    Debug.Log((object) nameof (WaitForFollowersToTakeSeat));
    GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.RitualCameraPosition, 12f);
    bool getFollowers = Ritual.FollowerToAttendSermon == null || Ritual.FollowerToAttendSermon.Count <= 0;
    if (getFollowers)
      Ritual.FollowerToAttendSermon = new List<FollowerBrain>();
    foreach (FollowerBrain followerBrain in new List<FollowerBrain>((IEnumerable<FollowerBrain>) Ritual.GetFollowersAvailableToAttendSermon()))
    {
      if (getFollowers)
        Ritual.FollowerToAttendSermon.Add(followerBrain);
      if (followerBrain.CurrentTask != null)
        followerBrain.CurrentTask.Abort();
      followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_DanceFirePit(firepitID));
      if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
        followerBrain.CurrentTask.Arrive();
      FollowerManager.FindFollowerByID(followerBrain.Info.ID)?.HideAllFollowerIcons();
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.15f));
    }
    yield return (object) null;
    Debug.Log((object) ("RECALCULATE! " + Ritual.FollowerToAttendSermon.Count.ToString()));
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
      followerBrain.CurrentTask?.RecalculateDestination();
    float timer = 0.0f;
    while (!this.FollowersInPosition(firepitID) && (double) (timer += Time.deltaTime) < 10.0)
    {
      Debug.Log((object) "WAITING");
      yield return (object) null;
    }
    SimulationManager.Pause();
    GameManager.GetInstance().OnConversationNext(Interaction_TempleAltar.Instance.PortalEffect.gameObject, 8f);
  }

  public bool FollowersInPosition(int firepitID)
  {
    foreach (FollowerBrain followerBrain in Ritual.FollowerToAttendSermon)
    {
      if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation || followerBrain.Location != PlayerFarming.Location || followerBrain.CurrentTaskType == FollowerTaskType.DanceFirePit && followerBrain.CurrentTask.State != FollowerTaskState.Doing)
      {
        if (followerBrain.Location != PlayerFarming.Location)
        {
          followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_DanceFirePit(firepitID));
          followerBrain.ShouldReconsiderTask = false;
          followerBrain.DesiredLocation = FollowerLocation.Church;
          if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
            followerBrain.CurrentTask.Arrive();
        }
        else if (followerBrain.CurrentTaskType != FollowerTaskType.DanceFirePit)
        {
          if (followerBrain.CurrentTask != null)
            followerBrain.CurrentTask.Abort();
          followerBrain.HardSwapToTask((FollowerTask) new FollowerTask_DanceFirePit(firepitID));
          if (followerBrain.CurrentTaskType == FollowerTaskType.ChangeLocation)
            followerBrain.CurrentTask.Arrive();
        }
        return false;
      }
    }
    return true;
  }

  public Vector3 GetDancePosition(Vector3 position)
  {
    return position + Vector3.up * 3f + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(2f, 4f);
  }
}
